"""DigiSat Insight: reporting, stock queries and explicit MongoDB administration."""
from __future__ import annotations

import os
import re
import json
import calendar
import secrets
import zipfile
import unicodedata
import urllib.request
import urllib.error
from datetime import date, datetime, time, timedelta
from pathlib import Path
from time import monotonic
from typing import Any, Literal

from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import FileResponse
from fastapi.staticfiles import StaticFiles
from pydantic import BaseModel, Field

try:
    from pymongo import MongoClient
    from pymongo.errors import PyMongoError
except ImportError:  # Helpful startup diagnostics before dependency installation.
    MongoClient = None
    PyMongoError = Exception

ROOT = Path(__file__).parent
try:
    from dotenv import load_dotenv
    load_dotenv(ROOT / ".env", override=False)
except ImportError:  # requirements.txt already includes python-dotenv.
    pass
DATA_DIR = Path(os.getenv("DIGISAT_BACKUP_DIR", r"C:\Digisat\SuiteG6\servidor\backup\dados"))
DEFAULT_URI = "mongodb://127.0.0.1:12220/?directConnection=true&readPreference=secondaryPreferred"
MONGO_URI = os.getenv("MONGO_URI", DEFAULT_URI)
MONGO_DATABASE = os.getenv("MONGO_DATABASE", "")
MONGO_USERNAME = os.getenv("MONGO_USERNAME", "")
MONGO_AUTH_SOURCE = os.getenv("MONGO_AUTH_SOURCE", "admin")
MONGO_PASSWORD_FILE = os.getenv("MONGO_PASSWORD_FILE", "")
MODE = os.getenv("DIGISAT_MODE", "live").lower()
if MODE == "backup":
    MONGO_URI = os.getenv("BACKUP_MONGO_URI", "mongodb://127.0.0.1:12231/?directConnection=true&readPreference=secondaryPreferred")
    MONGO_DATABASE = os.getenv("MONGO_DATABASE", "InsightBackup")
    MONGO_USERNAME = ""
    MONGO_PASSWORD_FILE = ""
MAX_ROWS = min(max(int(os.getenv("MAX_REPORT_ROWS", "500")), 50), 5000)
INSIGHT_USER = os.getenv("INSIGHT_USER", "")
INSIGHT_PASSWORD = os.getenv("INSIGHT_PASSWORD", "")
AI_ENDPOINT = os.getenv("AI_ENDPOINT", "")
AI_MODEL = os.getenv("AI_MODEL", "")
AI_API_KEY = os.getenv("AI_API_KEY", "")
# Defaults follow the DigiSatSuite model so the UI can offer a safe "vendas" scope.
# They only apply when the selected collection actually exposes the field.
STATUS_FIELD = "Situacao.Descricao"
STATUS_EXCLUDED = ["Cancelado", "Rejeitado", "Inutilizado"]
TYPE_FIELD = "_t"
PURCHASE_TYPES = ["NotaFiscalEletronicaEntrada", "NotaFiscalCompra"]
# Explicit movement scopes. "all_sales" preserves the intent of the old
# "somente vendas" checkbox but also leaves out pre-sales and purchase orders,
# which were never sales. "fiscal_sales" is the strictest revenue view.
MOVEMENT_GROUPS: dict[str, list[str] | None] = {
    "fiscal_sales": ["NotaFiscalConsumidorEletronica", "NotaFiscalEletronicaSaida", "NotaFiscalManual"],
    "all_sales": ["NotaFiscalConsumidorEletronica", "NotaFiscalEletronicaSaida", "NotaFiscalManual", "RegistroVenda"],
    "nfce": ["NotaFiscalConsumidorEletronica"],
    "nfe": ["NotaFiscalEletronicaSaida"],
    "manual": ["NotaFiscalManual"],
    "registro_venda": ["RegistroVenda"],
    "purchases": PURCHASE_TYPES,
    "all": None,
}
MOVEMENT_LABELS = {
    "fiscal_sales": "Vendas fiscais (NFC-e, NF-e saída e notas manuais)",
    "all_sales": "Vendas (fiscal + registros de venda)",
    "nfce": "Somente NFC-e",
    "nfe": "Somente NF-e de saída",
    "manual": "Somente notas manuais",
    "registro_venda": "Somente registros de venda",
    "purchases": "Compras e entradas",
    "all": "Tudo (sem filtro de tipo)",
}
KIND_LABELS = {
    "revenue": "Faturamento",
    "top_products": "Produtos mais vendidos",
    "daily": "Vendas por dia",
    "companies": "Por empresa",
    "documents": "Documentos",
    "payments": "Formas de pagamento",
    "ticket": "Ticket médio por dia",
}


def round2_mongo(expression: Any) -> dict:
    """Round half away from zero to 2 decimals using MongoDB 3.4 operators.

    ``$round`` only exists from MongoDB 4.2, so the total gross is scaled by
    100, nudged by half a cent and truncated. The tiny epsilon compensates for
    binary floating-point representation without changing values that are not
    already at a half-cent boundary.
    """
    return {"$divide": [
        {"$trunc": [
            {"$add": [
                {"$multiply": [expression, 100]},
                {"$cond": [{"$gte": [expression, 0]}, 0.500000001, -0.500000001]},
            ]}
        ]},
        100,
    ]}


# Pseudo-fields computed by the aggregation. Newer DigiSat documents stopped
# persisting TotalLiquido on every item, so the item total is reproduced with
# the same fields and order used by DigiSat's MovimentacaoSaidaAppService:
#   TotalBruto = round2(Quantidade * PrecoUnitario)
#   TotalLiquido = TotalBruto - DescontoDigitado - DescontoProporcional
#                  + OutrasDespesasProporcional + OutrasDespesasDigitado
#                  + Percentual.ValorSt + IPI.Valor + Frete + Seguro
_ITEM_GROSS = {"$multiply": [
    {"$ifNull": ["$ItensBase.PrecoUnitario", 0]},
    {"$ifNull": ["$ItensBase.Quantidade", 0]},
]}
COMPUTED_VALUES = {
    "@item_total": {"$subtract": [
        {"$add": [
            round2_mongo(_ITEM_GROSS),
            {"$ifNull": ["$ItensBase.OutrasDespesasProporcional", 0]},
            {"$ifNull": ["$ItensBase.OutrasDespesasDigitado", 0]},
            {"$ifNull": ["$ItensBase.Percentual.ValorSt", 0]},
            {"$ifNull": ["$ItensBase.IPI.Valor", 0]},
            {"$ifNull": ["$ItensBase.Frete", 0]},
            {"$ifNull": ["$ItensBase.Seguro", 0]},
        ]},
        {"$add": [
            {"$ifNull": ["$ItensBase.DescontoDigitado", 0]},
            {"$ifNull": ["$ItensBase.DescontoProporcional", 0]},
        ]},
    ]},
}
COMPUTED_LABELS = {
    "@item_total": ("Valor dos itens (calculado)",
                    "Preço × quantidade arredondado − descontos + outras despesas + IPI/ST + frete + seguro, "
                    "conforme a fórmula do DigiSat. Campos fiscais ausentes entram como zero."),
}
COMPUTED_UNWINDS = {"@item_total": ["ItensBase[]"]}

# DigiSat stores payments inside the movement, never in a joined collection.
# Fiscal documents write ``ItensPagamentos``; older point-of-sale records use
# ``Pagamentos``. Both are read per document so the two are never summed twice.
PAYMENT_ARRAYS = ("ItensPagamentos", "Pagamentos")
PAYMENT_SPECIES_CODE = "EspeciePagamento.Codigo"
PAYMENT_SPECIES_NAME = "EspeciePagamento.Descricao"
# Native TotalizacaoPorEspecie breaks these out per TEF transaction instead of
# per species; the panel keeps the species total and says so in the response.
CARD_PAYMENT_TYPES = {
    "ItemPagamentoCartao",
    "ItemPagamentoCartaoPos",
    "ItemPagamentoTerminalPos",
    "ItemPagamentoCarteiraDigital",
}


def rebase(expression: Any, new_prefix: str, old_prefix: str = "$ItensBase.") -> Any:
    """Rewrite a computed expression to run inside $map over one item."""
    if isinstance(expression, dict):
        return {key: rebase(value, new_prefix, old_prefix) for key, value in expression.items()}
    if isinstance(expression, list):
        return [rebase(value, new_prefix, old_prefix) for value in expression]
    if isinstance(expression, str) and expression.startswith(old_prefix):
        return new_prefix + expression[len(old_prefix):]
    return expression


def computed_projection(field: str, *mapped: str | None) -> dict:
    """Keep only required inputs before unwinding large DigiSat documents."""
    names: set[str] = set()
    def visit(value):
        if isinstance(value, str) and value.startswith("$") and not value.startswith("$$"):
            names.add(value[1:])
        elif isinstance(value, dict):
            for child in value.values():
                visit(child)
        elif isinstance(value, list):
            for child in value:
                visit(child)
    visit(COMPUTED_VALUES[field])
    names.update(value.replace("[]", "") for value in mapped if value and not value.startswith("@"))
    retained = []
    for name in sorted(names, key=lambda value: (value.count("."), value)):
        if not any(name.startswith(parent + ".") for parent in retained):
            retained.append(name)
    return {"_id": 0, **{name: 1 for name in retained}}
MovementGroup = Literal[
    "fiscal_sales", "all_sales", "nfce", "nfe", "manual", "registro_venda", "purchases", "all"
]
from insight_version import VERSION
app = FastAPI(title="DigiSat Insight", version=VERSION)
app.mount("/static", StaticFiles(directory=ROOT / "static"), name="static")


@app.on_event("startup")
async def require_auth_for_network_bind():
    host = os.getenv("INSIGHT_HOST", "127.0.0.1").lower()
    if host not in {"127.0.0.1", "localhost", "::1"} and (not INSIGHT_USER or not INSIGHT_PASSWORD):
        raise RuntimeError("Configure INSIGHT_USER/INSIGHT_PASSWORD antes de expor o app à rede interna.")


@app.middleware("http")
async def protect_api(request: Request, call_next):
    if request.url.path.startswith("/api/"):
        if not INSIGHT_USER or not INSIGHT_PASSWORD:
            return await call_next(request)  # Localhost-only setup remains usable; deployment docs require credentials.
        supplied = request.headers.get("authorization", "")
        try:
            scheme, encoded = supplied.split(" ", 1)
            import base64
            username, password = base64.b64decode(encoded).decode("utf-8").split(":", 1)
        except Exception:
            username = password = ""
            scheme = ""
        if scheme.lower() != "basic" or not secrets.compare_digest(username, INSIGHT_USER) or not secrets.compare_digest(password, INSIGHT_PASSWORD):
            from fastapi.responses import JSONResponse
            return JSONResponse({"detail": "Autenticação necessária."}, status_code=401)
    return await call_next(request)


def mongo_client(*, for_editor=False):
    if MongoClient is None:
        raise HTTPException(503, "Dependências ausentes. Execute: pip install -r requirements.txt")
    try:
        options = {"serverSelectionTimeoutMS": 2500, "connectTimeoutMS": 2500,
                   "appname": "DigiSatInsight", "readPreference": "secondaryPreferred"}
        if for_editor:
            # The editor is an explicitly write-capable, independently gated module.
            # Reports keep the original privilege policy and read preference.
            options.update(readPreference='primary', appname='DigiSatInsight-Editor',
                           socketTimeoutMS=10000, retryWrites=False)
        if MONGO_PASSWORD_FILE and MONGO_USERNAME:
            password_path = Path(MONGO_PASSWORD_FILE)
            if not password_path.is_absolute():
                password_path = ROOT / password_path
            try:
                lines = [line.strip() for line in password_path.read_text(encoding="utf-8-sig").splitlines() if line.strip()]
            except OSError:
                raise HTTPException(503, "Arquivo de senha MongoDB não encontrado. Confira MONGO_PASSWORD_FILE.") from None
            if len(lines) == 1:
                password = lines[0]
            elif len(lines) == 2 and re.search(r"senha|password", lines[0], re.IGNORECASE):
                password = lines[1]
            else:
                raise HTTPException(503, "Formato de arquivo de senha inválido; use uma senha em uma linha ou um rótulo seguido da senha.")
            options.update(username=MONGO_USERNAME, password=password, authSource=MONGO_AUTH_SOURCE)
            client = MongoClient(MONGO_URI, **options)
        else:
            client = MongoClient(MONGO_URI, **options)
        client.admin.command("ping")
        from insight_connection import enforce
        if not for_editor:
            enforce(client)
        return client
    except PyMongoError as exc:
        raise HTTPException(503, f"MongoDB indisponível ou sem autenticação: {exc.__class__.__name__}. Configure MONGO_URI com usuário de leitura.") from None


def latest_backup():
    """Newest DigisatServer dump that actually carries the movements the panel reports on.

    Backup jobs can leave a partial archive behind (catalogs only, movements empty);
    restoring it would produce a database with nothing to report, so those are skipped.
    """
    try:
        candidates = sorted(DATA_DIR.glob("DigisatServer*.zip"), key=lambda p: p.stat().st_mtime, reverse=True)
    except OSError:
        return None
    for path in candidates:
        try:
            with zipfile.ZipFile(path) as archive:
                info = archive.getinfo("Movimentacoes.bson.gz")
        except (OSError, KeyError, zipfile.BadZipFile):
            continue
        if info.file_size > 1024:
            return path
    return None


@app.get("/")
def home():
    return FileResponse(ROOT / "static" / "index.html")


@app.get("/favicon.ico", include_in_schema=False)
def favicon():
    return FileResponse(ROOT / "static" / "favicon.svg", media_type="image/svg+xml")


@app.get("/api/status")
def status():
    backup = latest_backup()
    result: dict[str, Any] = {
        "configured": bool(MONGO_DATABASE), "database": MONGO_DATABASE or None, "mode": MODE,
        "version": app.version, "max_rows": MAX_ROWS,
        "backup": {"name": backup.name, "modified": datetime.fromtimestamp(backup.stat().st_mtime).isoformat(timespec="minutes"),
                   "bytes": backup.stat().st_size} if backup else None,
        "backup_note": "ZIP encontrado; o app não altera nem abre arquivos WiredTiger diretamente. Restaure em um MongoDB isolado para consultar o backup.",
    }
    try:
        with mongo_client() as client:
            result["connected"] = True
            if MONGO_DATABASE:
                try:
                    client[MONGO_DATABASE].list_collection_names()
                except PyMongoError:
                    result["connected"] = False
                    result["connection_message"] = "A conta não consegue consultar o banco selecionado. Use Configurar conexão para verificar banco e credenciais."
            try:
                result["databases"] = sorted(n for n in client.list_database_names() if n not in ("admin", "config", "local"))
            except PyMongoError:
                result["databases"] = []
                result["database_list_message"] = "A conta não pode listar bancos. Defina MONGO_DATABASE com o nome fornecido pelo DBA."
    except HTTPException as exc:
        result["connected"] = False
        result["connection_message"] = exc.detail
    return result



@app.get("/api/collections")
def collections():
    if not MONGO_DATABASE:
        raise HTTPException(400, "Defina MONGO_DATABASE no arquivo .env.")
    with mongo_client() as client:
        db = client[MONGO_DATABASE]
        names = sorted(db.list_collection_names())
        # Movimentacoes is the collection the reports were designed around; keep it first.
        names.sort(key=lambda name: (name != "Movimentacoes", name.casefold()))
        return [{"name": name, "count": db[name].estimated_document_count()} for name in names]


def flatten(doc: dict, prefix: str = "") -> dict[str, Any]:
    out = {}
    for key, value in doc.items():
        path = f"{prefix}.{key}" if prefix else key
        if isinstance(value, dict):
            out.update(flatten(value, path))
        elif isinstance(value, list):
            out[path] = value
            for item in value[:3]:
                if isinstance(item, dict):
                    out.update(flatten(item, path + "[]"))
        else:
            out[path] = value
    return out


def json_safe(value: Any):
    if isinstance(value, dict): return {str(k): json_safe(v) for k, v in value.items()}
    if isinstance(value, (list, tuple)): return [json_safe(v) for v in value]
    if isinstance(value, (datetime, date)): return value.isoformat()
    if value.__class__.__name__ in ("ObjectId", "Decimal128"): return str(value)
    return value


def brl(value: float) -> str:
    """Format a number as Brazilian currency for API messages."""
    text = f"{value:,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")
    return f"R$ {text}"


def plural_count(value: int, singular: str, plural: str) -> str:
    return f"{value} {singular if value == 1 else plural}"


@app.get("/api/schema/{collection}")
def schema(collection: str):
    if not MONGO_DATABASE:
        raise HTTPException(400, "Defina MONGO_DATABASE no arquivo .env.")
    with mongo_client() as client:
        db = client[MONGO_DATABASE]
        if collection not in db.list_collection_names():
            raise HTTPException(404, "Coleção não encontrada.")
        samples = list(db[collection].find({}, limit=30))
        fields: dict[str, set[str]] = {}
        for doc in samples:
            for key, value in flatten(doc).items():
                fields.setdefault(key, set()).add(type(value).__name__)
        names = list(fields)
        has_price = any(name.casefold().endswith("precounitario") for name in names)
        has_quantity = any(name.casefold().endswith("quantidade") for name in names)
        computed = ([{"name": key, "label": label, "note": note}
                     for key, (label, note) in COMPUTED_LABELS.items()]
                    if has_price and has_quantity else [])
        return {"collection": collection, "sampled": len(samples),
                "fields": [{"name": k, "types": sorted(v)} for k, v in sorted(fields.items())],
                "suggested": suggest_fields(names), "computed": computed}


def suggest_fields(names: list[str]) -> dict[str, str | None]:
    """Pick the most likely date/value/product/company fields for the five reports."""
    lowered = {name.casefold(): name for name in names}

    def pick(exact: list[str], keywords: list[str]) -> str | None:
        for candidate in exact:
            if candidate.casefold() in lowered:
                return lowered[candidate.casefold()]
        for keyword in keywords:
            for name in names:
                if keyword in name.casefold():
                    return name
        return None

    value = pick(
        ["ItensBase[].TotalLiquido", "ItensBase[].TotalBruto", "TotalLiquido", "TotalBruto",
         "ValorTotal", "ValorLiquido", "Total", "Valor"],
        ["totalliquido", "valortotal", "total", "valor"])
    if any(name.casefold().endswith("precounitario") for name in names) and any(
            name.casefold().endswith("quantidade") for name in names):
        value = "@item_total"  # reliable on both old and recent DigiSat movements
    return {
        "date": pick(
            ["DataHoraEmissao", "DataEmissao", "DataMovimentacao", "DataVenda", "DataHora", "Data"],
            ["datahoraemissao", "dataemissao", "datahora", "data"]),
        "value": value,
        "product": pick(
            ["ItensBase[].ProdutoServico.Descricao", "ProdutoServico.Descricao", "Produto.Descricao",
             "DescricaoProduto", "Produto", "Descricao"],
            ["produtoservico.descricao", "produto.descricao", "descricaoproduto", "produto", "descricao"]),
        "company": pick(
            ["Empresa.Nome", "Empresa.NomeFantasia", "Empresa.RazaoSocial", "Loja.Nome", "Filial.Nome", "Empresa"],
            ["empresa.nome", "empresa.nomefantasia", "empresa.razaosocial", "loja.nome", "filial.nome", "empresa"]),
    }


@app.get("/api/values/{collection}")
def values(collection: str, field: str):
    """Distinct values for autocomplete (company names), capped and read-only."""
    if not MONGO_DATABASE:
        raise HTTPException(400, "Defina MONGO_DATABASE no arquivo .env.")
    if not re.fullmatch(r"[\w.$\[\]-]{1,120}", field, re.UNICODE):
        raise HTTPException(400, "Campo inválido.")
    path = field.replace("[]", "")
    with mongo_client() as client:
        db = client[MONGO_DATABASE]
        if collection not in db.list_collection_names():
            raise HTTPException(404, "Coleção não encontrada.")
        try:
            # Autocomplete covers the entire collection, unlike a dated report.
            # Keep it bounded using the budget for broad historical queries.
            raw = db[collection].distinct(path, maxTimeMS=20000)
        except PyMongoError as exc:
            raise HTTPException(422, f"Não foi possível listar valores: {exc.__class__.__name__}") from None
    items = sorted({str(v) for v in raw if isinstance(v, (str, int, float)) and str(v).strip()})[:200]
    return {"field": field, "values": items, "truncated": len(raw) > len(items)}


class ReportRequest(BaseModel):
    collection: str
    kind: str = Field(pattern="^(revenue|top_products|documents|daily|companies|payments|ticket)$")
    start: date
    end: date
    date_field: str
    value_field: str | None = None
    product_field: str | None = None
    company_field: str | None = None
    company_value: str | None = Field(default=None, max_length=120)
    only_valid: bool = False
    only_sales: bool = False
    movement_group: MovementGroup | None = None
    limit: int = Field(default=100, ge=1, le=MAX_ROWS)


class AskRequest(BaseModel):
    question: str
    collection: str
    start: date
    end: date
    date_field: str
    value_field: str | None = None
    product_field: str | None = None
    company_field: str | None = None
    company_value: str | None = Field(default=None, max_length=120)
    only_valid: bool = True
    only_sales: bool = True
    movement_group: MovementGroup | None = None
    limit: int = Field(default=500, ge=1, le=MAX_ROWS)


class ReconciliationRequest(BaseModel):
    collection: str
    start: date
    end: date
    date_field: str
    company_field: str | None = None
    company_value: str | None = Field(default=None, max_length=120)
    only_valid: bool = False
    only_sales: bool = False
    movement_group: MovementGroup | None = None
    limit: int = Field(default=100, ge=1, le=MAX_ROWS)


class InvestigationRequest(ReconciliationRequest):
    seller_query: str | None = Field(default=None, max_length=120)
    target_amount: float | None = Field(default=None, ge=0, le=1e12, allow_inf_nan=False)
    offset: int = Field(default=0, ge=0, le=1000000)
    search: str = Field(default="", max_length=100)
    profile: Literal["panel", "native_nfce", "fiscal_valid"] = "panel"
    issue: Literal["all", "mismatch", "unpaid", "unassigned", "shared", "negative"] = "all"
    status_filter: Literal["valid", "all", "completed"] = "valid"


def escape_field(field: str | None):
    if not field or len(field) > 120 or not re.fullmatch(r"[\w-]+(?:\[\])?(?:\.[\w-]+(?:\[\])?)*", field, re.UNICODE):
        raise HTTPException(400, "Selecione campos válidos descobertos no esquema.")
    return "$" + field.replace("[]", "")


def value_expression(field: str | None):
    """Resolve a mapped field, including safe server-side computed pseudo-fields."""
    if not field:
        return None
    if field in COMPUTED_VALUES:
        return COMPUTED_VALUES[field]
    return escape_field(field)


def array_unwinds(*fields: str | None) -> list[dict]:
    """Unwind only embedded arrays explicitly selected by a user-mapped field."""
    paths: set[str] = set()
    for field in fields:
        if not field:
            continue
        parts = field.split(".")
        prefix: list[str] = []
        for part in parts:
            if part.endswith("[]"):
                prefix.append(part[:-2])
                paths.add(".".join(prefix))
            else:
                prefix.append(part)
    return [{"$unwind": "$" + path} for path in sorted(paths, key=lambda p: (p.count("."), p))]


MONTHS = {
    "janeiro": 1, "fevereiro": 2, "marco": 3, "abril": 4, "maio": 5, "junho": 6,
    "julho": 7, "agosto": 8, "setembro": 9, "outubro": 10, "novembro": 11, "dezembro": 12,
}


def strip_accents(text: str) -> str:
    return "".join(char for char in unicodedata.normalize("NFD", text) if unicodedata.category(char) != "Mn")


def add_months(value: date, delta: int) -> date:
    index = value.month - 1 + delta
    year, month = value.year + index // 12, index % 12 + 1
    return date(year, month, min(value.day, calendar.monthrange(year, month)[1]))


def month_bounds(year: int, month: int) -> tuple[date, date]:
    return date(year, month, 1), date(year, month, calendar.monthrange(year, month)[1])


def parse_period(text: str, today: date) -> tuple[date, date] | None:
    """Understand common pt-BR period expressions. Returns an inclusive range."""
    q = strip_accents(text.casefold())

    # Explicit dd/mm/yyyy (or dd/mm) wins over relative words.
    matches = list(re.finditer(r"\b(\d{1,2})/(\d{1,2})(?:/(\d{2,4}))?\b", q))
    if matches:
        def as_date(match: re.Match) -> date | None:
            year = int(match.group(3)) if match.group(3) else today.year
            if year < 100:
                year += 2000
            try:
                return date(year, int(match.group(2)), int(match.group(1)))
            except ValueError:
                raise HTTPException(422, "A pergunta contém uma data inválida. Use dia/mês/ano.") from None
        parsed = [d for d in (as_date(m) for m in matches[:2]) if d]
        if parsed:
            start = parsed[0]
            if len(parsed) > 1:
                end = parsed[1]
            elif "desde" in q or "a partir" in q or "ate hoje" in q:
                end = today
            else:
                end = start
            return (start, end) if start <= end else (end, start)

    def days_back(count: int) -> tuple[date, date]:
        return today - timedelta(days=max(count, 1) - 1), today

    if "anteontem" in q:
        day = today - timedelta(days=2)
        return day, day
    if re.search(r"\bhoje\b", q):
        return today, today
    if re.search(r"\bontem\b", q):
        day = today - timedelta(days=1)
        return day, day
    match = re.search(r"ultim[oa]s?\s+(\d{1,3})\s+dias?", q)
    if match:
        return days_back(int(match.group(1)))
    match = re.search(r"ultim[oa]s?\s+(\d{1,3})\s+semanas?", q)
    if match:
        return days_back(int(match.group(1)) * 7)
    match = re.search(r"ultim[oa]s?\s+(\d{1,2})\s+meses?", q)
    if match:
        return add_months(today, -int(match.group(1))) + timedelta(days=1), today
    if re.search(r"(semana passada|ultima semana)", q):
        monday = today - timedelta(days=today.weekday() + 7)
        return monday, monday + timedelta(days=6)
    if re.search(r"(esta semana|nesta semana|semana atual)", q):
        monday = today - timedelta(days=today.weekday())
        return monday, today
    if re.search(r"(mes passado|ultimo mes)", q):
        previous = add_months(today.replace(day=1), -1)
        return month_bounds(previous.year, previous.month)
    if re.search(r"(este mes|neste mes|deste mes|desse mes|mes atual|no mes\b)", q):
        return today.replace(day=1), today
    if re.search(r"(ano passado|ultimo ano)", q):
        return date(today.year - 1, 1, 1), date(today.year - 1, 12, 31)
    if re.search(r"(este ano|neste ano|ano atual)", q):
        return date(today.year, 1, 1), today

    match = re.search(r"\b(" + "|".join(MONTHS) + r")(?:\s+de\s+(\d{4}))?\b", q)
    if match:
        month = MONTHS[match.group(1)]
        year = int(match.group(2)) if match.group(2) else today.year
        if not match.group(2) and month > today.month:
            year -= 1
        day_match = re.search(r"\b(?:dia\s+)?(\d{1,2})\s+de\s+" + match.group(1) + r"\b", q)
        if day_match:
            try:
                day = date(year, month, int(day_match.group(1)))
            except ValueError:
                raise HTTPException(422, "A pergunta contém uma data inválida. Use dia/mês/ano.") from None
            return day, day
        return month_bounds(year, month)
    return None


def normalize_text(text: str) -> str:
    return re.sub(r"[^a-z0-9]+", " ", strip_accents(text.casefold())).strip()


_COMPANY_CACHE: dict[tuple[str, str, str], tuple[float, list[str]]] = {}


def resolve_company(coll, field: str | None, question: str) -> str | None:
    """Match a company mentioned in the question against distinct values."""
    if not field:
        return None
    path = field.replace("[]", "")
    key = (MONGO_DATABASE, coll.name, path)
    cached = _COMPANY_CACHE.get(key)
    if cached and monotonic() - cached[0] < 300:
        candidates = cached[1]
    else:
        try:
            raw = coll.distinct(path, maxTimeMS=5000)
        except PyMongoError:
            return None
        candidates = [value for value in raw if isinstance(value, str) and len(value.strip()) >= 3]
        _COMPANY_CACHE[key] = (monotonic(), candidates)
    if not candidates or re.search(r"\btodas?\s+(as\s+)?empresas?\b", strip_accents(question.casefold())):
        return None
    normalized_question = " " + normalize_text(question) + " "
    best: tuple[int, str] | None = None
    for candidate in candidates:
        normalized = normalize_text(candidate)
        if not normalized:
            continue
        if f" {normalized} " in normalized_question:
            score = len(normalized) + 1000
        else:
            tokens = [token for token in normalized.split() if len(token) > 3]
            if not tokens:
                continue
            hits = sum(1 for token in tokens if f" {token} " in normalized_question)
            if hits / len(tokens) < 0.8:
                continue
            score = len(normalized)
        if best is None or score > best[0]:
            best = (score, candidate)
    return best[1] if best else None


def movement_filter(req) -> dict | None:
    """Translate the explicit movement scope into a Mongo filter.

    Clients that predate ``movement_group`` still get the old binary
    ``only_sales`` behaviour. An explicit group always wins so the UI can
    distinguish fiscal sales, legacy POS records, and purchases.
    """
    group = getattr(req, "movement_group", None)
    if group:
        if group not in MOVEMENT_GROUPS:
            raise HTTPException(400, "Escopo de movimentação inválido.")
        types = MOVEMENT_GROUPS[group]
        return None if types is None else {TYPE_FIELD: {"$in": types}}
    if getattr(req, "only_sales", False):
        return {TYPE_FIELD: {"$nin": PURCHASE_TYPES}}
    return None


def scope_metadata(req) -> dict[str, Any]:
    group = getattr(req, "movement_group", None)
    if group:
        label = MOVEMENT_LABELS[group]
    elif getattr(req, "only_sales", False):
        label = "Somente vendas (compatibilidade)"
    else:
        label = "Tudo (sem filtro de tipo)"
    return {
        "movement_group": group,
        "movement_label": label,
        "movement_types": MOVEMENT_GROUPS.get(group) if group else None,
        "only_valid": bool(getattr(req, "only_valid", False)),
        "only_sales": bool(getattr(req, "only_sales", False)),
    }


def period_match(req) -> tuple[dict, dict]:
    """Period filter plus the optional valid/movement scope, counted separately."""
    match = {req.date_field.replace("[]", ""): {
        "$gte": datetime.combine(req.start, time.min),
        "$lt": datetime.combine(req.end + timedelta(days=1), time.min),
    }}
    if req.company_value:
        if not req.company_field:
            raise HTTPException(400, "Selecione o campo de empresa para aplicar esse filtro.")
        escape_field(req.company_field)
        match[req.company_field.replace("[]", "")] = req.company_value
    scoped = dict(match)
    # These only filter when the field exists; in other collections they are a no-op.
    if req.only_valid:
        scoped[STATUS_FIELD] = {"$nin": STATUS_EXCLUDED}
    movement = movement_filter(req)
    if movement:
        scoped.update(movement)
    return match, scoped


def measure_coverage(coll, base: list[dict], val: str) -> dict[str, Any] | None:
    """Share of aggregated rows that actually carry the selected value field.

    A value field that exists only on part of the rows silently produces a total
    that ignores the rest, so the ratio is measured and reported to the user.
    """
    pipeline = base + [{"$group": {
        "_id": None,
        "rows": {"$sum": 1},
        "with_value": {"$sum": {"$cond": [{"$in": [{"$type": val}, ["missing", "null"]]}, 0, 1]}},
    }}]
    try:
        rows = list(coll.aggregate(pipeline, maxTimeMS=15000, allowDiskUse=False))
    except PyMongoError:
        return None
    if not rows:
        return None
    total = rows[0].get("rows", 0)
    filled = rows[0].get("with_value", 0)
    return {"rows": total, "with_value": filled, "missing": total - filled,
            "pct": round(100.0 * filled / total, 1) if total else 0.0}


def payment_source_expression() -> dict:
    """Pick one payment array per document without summing both."""
    fiscal = {"$size": {"$ifNull": ["$ItensPagamentos", []]}}
    return {"$cond": [
        {"$gt": [fiscal, 0]},
        "$ItensPagamentos",
        {"$ifNull": ["$Pagamentos", []]},
    ]}


def summarize_payments(by_species: list[dict], totals: list[dict], by_type: list[dict],
                       species_count: int) -> dict[str, Any]:
    """Shape the aggregation result into report rows and headline numbers."""
    grand_total = round(float(totals[0].get("total") or 0), 2) if totals else 0.0
    entries = int(totals[0].get("items") or 0) if totals else 0
    rows: list[dict[str, Any]] = []
    for item in by_species:
        key = item.get("_id") or {}
        value = round(float(item.get("total") or 0), 2)
        rows.append({
            "codigo": key.get("codigo"),
            "_id": key.get("descricao") or "Sem espécie",
            "total": value,
            "items": int(item.get("items") or 0),
            "participacao": round(100.0 * value / grand_total, 2) if abs(grand_total) > 0.01 else 0.0,
        })
    cards = sum(int(t.get("items") or 0) for t in by_type
                if (t.get("_id") or "") in CARD_PAYMENT_TYPES)
    return {
        "rows": rows, "total": grand_total, "items": entries, "cards": cards,
        "species": species_count, "truncated": species_count > len(rows),
    }


def payment_report(coll, req: ReportRequest, scoped_match: dict, match: dict) -> dict[str, Any]:
    """Payment methods per species, mapped from DigiSat's TotalizacaoPorEspecie."""
    pipeline = [
        {"$match": scoped_match},
        {"$project": {"pay": payment_source_expression()}},
        {"$unwind": "$pay"},
        {"$facet": {
            "by_species": [
                {"$group": {
                    "_id": {"codigo": "$pay." + PAYMENT_SPECIES_CODE,
                            "descricao": {"$ifNull": ["$pay." + PAYMENT_SPECIES_NAME, "Sem espécie"]}},
                    "total": {"$sum": "$pay.Valor"},
                    "items": {"$sum": 1},
                }},
                {"$sort": {"total": -1, "items": -1}},
                {"$limit": req.limit},
            ],
            "totals": [{"$group": {
                "_id": None, "total": {"$sum": "$pay.Valor"}, "items": {"$sum": 1},
            }}],
            "species_count": [
                {"$group": {"_id": {
                    "codigo": "$pay." + PAYMENT_SPECIES_CODE,
                    "descricao": "$pay." + PAYMENT_SPECIES_NAME,
                }}},
                {"$count": "n"},
            ],
            "by_type": [{"$group": {"_id": "$pay._t", "items": {"$sum": 1}}}],
        }},
    ]
    try:
        facet_rows = list(coll.aggregate(pipeline, maxTimeMS=20000, allowDiskUse=False))
    except PyMongoError as exc:
        raise HTTPException(422, f"Consulta não concluída: {exc.__class__.__name__}") from None

    facet = facet_rows[0] if facet_rows else {}
    count_rows = facet.get("species_count") or []
    species_count = int(count_rows[0].get("n") or 0) if count_rows else 0
    summary = summarize_payments(facet.get("by_species") or [], facet.get("totals") or [],
                                 facet.get("by_type") or [], species_count)

    documents = coll.count_documents(scoped_match)
    documents_period = coll.count_documents(match)
    documents_with_payment = coll.count_documents({
        **scoped_match,
        "$or": [{name + ".0": {"$exists": True}} for name in PAYMENT_ARRAYS],
    })
    documents_without_payment = documents - documents_with_payment

    warnings: list[str] = []
    if not documents:
        if documents_period:
            warnings.append(
                f"{plural_count(documents_period, 'documento', 'documentos')} no período, mas nenhum ficou no "
                f"escopo selecionado. Desmarque o escopo para vê-los.")
        else:
            warnings.append("Nenhum documento foi encontrado no período selecionado.")
    elif documents_without_payment:
        warnings.append(
            f"{plural_count(documents_without_payment, 'documento do escopo não tem', 'documentos do escopo não têm')} "
            f"pagamento registrado em ItensPagamentos nem em Pagamentos; eles não entram em nenhuma espécie.")
    if documents and not summary["rows"]:
        warnings.append("Nenhuma parcela de pagamento foi encontrada nos documentos do escopo.")

    card_note = ""
    if summary["cards"]:
        card_note = (f" {plural_count(summary['cards'], 'parcela', 'parcelas')} de cartão, carteira digital ou "
                     f"terminal POS: o relatório nativo detalha cada transação TEF (bandeira e crédito/débito); "
                     f"aqui elas estão agrupadas pela espécie do pagamento.")
    scope = {
        **scope_metadata(req),
        "documents_in_period": documents_period, "documents_in_scope": documents,
        "excluded": documents_period - documents,
    }
    return {
        "rows": summary["rows"], "count": len(summary["rows"]),
        "documents": documents, "documents_with_payment": documents_with_payment,
        "documents_without_payment": documents_without_payment,
        "period": {"start": str(req.start), "end": str(req.end)},
        "read_only": True, "coverage": None, "warnings": warnings, "scope": scope,
        "value_field": None, "payments_total": summary["total"],
        "payment_entries": summary["items"], "payment_species": summary["species"],
        "truncated": summary["truncated"], "limit": req.limit, "value_note": None,
        "payments_note": (
            "Agrupado por ItensPagamentos[].EspeciePagamento (código e descrição) e somado por Valor; "
            "Pagamentos[] é usado apenas quando o documento não tem ItensPagamentos. "
            "O relatório nativo TotalizacaoPorEspecie filtra natureza 1, finalidade normal e situação 2 "
            "(ou 14 + 2 quando o filtro é 99) e usa o instante final sem expandir o dia; "
            "aqui valem o escopo de tipo e os documentos válidos escolhidos na tela. "
            "Parcelas de PagamentoRecebimento não são lidas nesta visão." + card_note
        ),
    }


def document_total_expression(field: str | None):
    """Per-document total, summing an embedded array when the field is one."""
    if not field:
        return None
    if field == "@item_total":
        return {"$sum": {"$map": {
            "input": {"$ifNull": ["$ItensBase", []]},
            "as": "item",
            "in": rebase(COMPUTED_VALUES["@item_total"], "$$item."),
        }}}
    path = field.replace("[]", "")
    expression: Any = "$" + path
    if "[]" in field:
        expression = {"$sum": {"$ifNull": [expression, []]}}
    return expression


def summarize_ticket(group_rows: list[dict]) -> dict[str, Any]:
    """Turn per-day totals into ticket médio rows and a period average."""
    rows: list[dict[str, Any]] = []
    for row in group_rows:
        documents = int(row.get("documents") or 0)
        total = round(float(row.get("total") or 0), 2)
        rows.append({
            "_id": row.get("_id"),
            "documents": documents,
            "total": total,
            "ticket": round(total / documents, 2) if documents else 0.0,
        })
    total = round(sum(row["total"] for row in rows), 2)
    documents = sum(row["documents"] for row in rows)
    return {
        "rows": rows, "total": total, "documents": documents,
        "ticket": round(total / documents, 2) if documents else 0.0,
    }


def ticket_report(coll, req: ReportRequest, scoped_match: dict, match: dict,
                  date_expr: Any) -> dict[str, Any]:
    """Daily average ticket, with the native total taken from the document items."""
    value = document_total_expression(req.value_field)
    if value is None:
        raise HTTPException(400, "Mapeie o campo de valor para calcular o ticket médio.")
    pipeline = [
        {"$match": scoped_match},
        {"$project": {
            "dia": {"$dateToString": {"format": "%Y-%m-%d", "date": date_expr}},
            "valor": value,
        }},
        {"$group": {
            "_id": "$dia",
            "documents": {"$sum": 1},
            "total": {"$sum": "$valor"},
        }},
        {"$sort": {"_id": 1}},
        {"$limit": req.limit},
    ]
    try:
        grouped = json_safe(list(coll.aggregate(pipeline, maxTimeMS=15000, allowDiskUse=False)))
    except PyMongoError as exc:
        raise HTTPException(422, f"Consulta não concluída: {exc.__class__.__name__}") from None

    summary = summarize_ticket(grouped)
    documents = coll.count_documents(scoped_match)
    documents_period = coll.count_documents(match)
    documents_with_value = documents
    if req.value_field == "@item_total":
        documents_with_value = coll.count_documents({**scoped_match, "ItensBase.0": {"$exists": True}})

    warnings: list[str] = []
    if not documents:
        if documents_period:
            warnings.append(
                f"{plural_count(documents_period, 'documento', 'documentos')} no período, mas nenhum ficou no "
                f"escopo selecionado. Desmarque o escopo para vê-los.")
        else:
            warnings.append("Nenhum documento foi encontrado no período selecionado.")
    elif documents_with_value < documents:
        warnings.append(
            f"{plural_count(documents - documents_with_value, 'documento do escopo não tem', 'documentos do escopo não têm')} "
            f"itens com valor; eles entram na contagem de documentos e reduzem o ticket médio.")
    if documents and not summary["rows"]:
        warnings.append("Nenhum valor foi encontrado nos documentos do escopo.")

    scope = {
        **scope_metadata(req),
        "documents_in_period": documents_period, "documents_in_scope": documents,
        "excluded": documents_period - documents,
    }
    return {
        "rows": summary["rows"], "count": len(summary["rows"]),
        "documents": documents, "documents_with_value": documents_with_value,
        "period": {"start": str(req.start), "end": str(req.end)},
        "read_only": True, "coverage": None, "warnings": warnings, "scope": scope,
        "value_field": req.value_field, "payments_total": None,
        "ticket_total": summary["ticket"], "value_note": (
            COMPUTED_LABELS[req.value_field][1] if req.value_field in COMPUTED_LABELS else None),
        "ticket_note": (
            "Ticket médio = total dos itens ÷ documentos de cada dia. "
            "O relatório nativo TicketMedioPorOrigemETipo agrupa por empresa, data e tipo de movimentação; "
            "aqui a empresa é um filtro da tela, o tipo é o escopo de movimentação e o dia é a chave do gráfico."
        ),
    }


def run_report(req: ReportRequest):
    if req.end < req.start:
        raise HTTPException(400, "A data final precisa ser igual ou posterior à inicial.")
    date_expr = escape_field(req.date_field)
    val = value_expression(req.value_field)
    product = escape_field(req.product_field) if req.product_field else None
    company = escape_field(req.company_field) if req.company_field else None
    match, scoped_match = period_match(req)
    if not MONGO_DATABASE:
        raise HTTPException(400, "Defina MONGO_DATABASE no arquivo .env.")
    with mongo_client() as client:
        db = client[MONGO_DATABASE]
        if req.collection not in db.list_collection_names():
            raise HTTPException(404, "Coleção não encontrada.")
        coll = db[req.collection]
        if req.kind == "payments":
            return payment_report(coll, req, scoped_match, match)
        if req.kind == "ticket":
            return ticket_report(coll, req, scoped_match, match, date_expr)
        base: list[dict] = [{"$match": scoped_match}]
        if req.kind != "documents":
            if req.value_field in COMPUTED_VALUES:
                base.append({"$project": computed_projection(req.value_field, req.date_field, req.product_field, req.company_field)})
            unwind = [req.value_field]
            if req.kind == "top_products":
                unwind.append(req.product_field)
            if req.kind == "companies":
                unwind.append(req.company_field)
            unwind.extend(COMPUTED_UNWINDS.get(req.value_field or "", []))
            base += array_unwinds(*unwind)
        if req.kind == "revenue":
            if not val: raise HTTPException(400, "Mapeie o campo de valor para este relatório.")
            pipeline = base + [
                # MongoDB 3.4 stores report values as BSON numeric types; $convert
                # is intentionally avoided because it arrived in a later server release.
                {"$group": {"_id": None, "total": {"$sum": val}, "items": {"$sum": 1}}},
                {"$project": {"_id": 0, "total": 1, "items": 1}},
            ]
        elif req.kind in ("top_products", "companies"):
            field = product if req.kind == "top_products" else company
            if not field: raise HTTPException(400, "Mapeie o campo de produto/empresa para este relatório.")
            group = {"_id": field, "items": {"$sum": 1}}
            if val: group["total"] = {"$sum": val}
            pipeline = base + [{"$group": group}, {"$sort": {"total": -1, "items": -1}}, {"$limit": req.limit}]
        elif req.kind == "daily":
            group = {"_id": {"$dateToString": {"format": "%Y-%m-%d", "date": date_expr}}, "items": {"$sum": 1}}
            if val: group["total"] = {"$sum": val}
            pipeline = base + [{"$group": group}, {"$sort": {"_id": 1}}, {"$limit": req.limit}]
        else:  # documents
            # Never expose full Mongo documents: only a flat, human-readable list.
            selected = {"_id": 0, "Data": "$" + req.date_field.replace("[]", ""),
                        "Numero": "$Numero", "Serie": "$Serie"}
            if req.company_field:
                selected["Empresa"] = "$" + req.company_field.replace("[]", "")
            if req.product_field:
                raw_product = req.product_field
                if "[]" in raw_product:
                    head, tail = raw_product.split("[]", 1)
                    tail = tail.lstrip(".")
                    selected["Produtos"] = {"$map": {
                        "input": "$" + head, "as": "item",
                        "in": ("$$item." + tail) if tail else "$$item",
                    }}
                else:
                    selected["Produto"] = "$" + raw_product
            if req.value_field == "@item_total":
                selected["Valor"] = round2_mongo({"$sum": {"$map": {
                    "input": "$ItensBase", "as": "item",
                    "in": rebase(COMPUTED_VALUES["@item_total"], "$$item."),
                }}})
            elif req.value_field:
                selected["Valor"] = "$" + req.value_field.replace("[]", "")
            selected["Situacao"] = "$Situacao.Descricao"
            pipeline = base + [{"$project": selected}, {"$limit": req.limit}]
        try:
            rows = json_safe(list(coll.aggregate(pipeline, maxTimeMS=15000, allowDiskUse=False)))
        except PyMongoError as exc:
            raise HTTPException(422, f"Consulta não concluída: {exc.__class__.__name__}") from None

        # Money actually registered as payment, used to sanity-check the item total.
        payments_total = None
        if req.kind == "revenue":
            try:
                # Fiscal documents store payments in ItensPagamentos; older
                # point-of-sale records (RegistroVenda) use Pagamentos instead.
                # Prefer the fiscal array and fall back per document so the two
                # are never summed together and double-counted.
                paid = list(coll.aggregate([
                    {"$match": scoped_match},
                    {"$project": {"payment": {"$cond": [
                        {"$gt": [{"$size": {"$ifNull": ["$ItensPagamentos", []]}}, 0]},
                        "$ItensPagamentos.Valor",
                        {"$ifNull": ["$Pagamentos.Valor", []]},
                    ]}}},
                    {"$unwind": "$payment"},
                    {"$group": {"_id": None, "total": {"$sum": "$payment"}}},
                ], maxTimeMS=15000, allowDiskUse=False))
                if paid and paid[0].get("total") is not None:
                    payments_total = round(float(paid[0]["total"]), 2)
            except (PyMongoError, TypeError, ValueError):
                payments_total = None

        # Documents matched by the period filter, counted before any array unwinding.
        documents = coll.count_documents(scoped_match)
        documents_period = coll.count_documents(match)
        if req.kind == "daily":
            notes_by_day: dict[str, int] = {}
            try:
                for entry in coll.aggregate([{"$match": scoped_match},
                                             {"$group": {"_id": {"$dateToString": {"format": "%Y-%m-%d", "date": date_expr}},
                                                         "n": {"$sum": 1}}},
                                             {"$sort": {"_id": 1}}], maxTimeMS=15000, allowDiskUse=False):
                    notes_by_day[entry["_id"]] = entry["n"]
            except PyMongoError:
                notes_by_day = {}
            for row in rows:
                row["documents"] = notes_by_day.get(row.get("_id"), 0)

        coverage = (measure_coverage(coll, base, val)
                    if val and req.kind != "documents" and not (req.value_field or "").startswith("@") else None)
        warnings: list[str] = []
        if coverage and coverage["missing"]:
            warnings.append(
                f"O campo '{req.value_field}' está preenchido em {coverage['with_value']} de {coverage['rows']} "
                f"registros ({coverage['pct']}%). Os {coverage['missing']} registros sem valor entraram com zero, "
                f"então o total não representa todos os itens do período.")
        if coverage and coverage["rows"] == 0:
            warnings.append(f"O campo '{req.value_field}' não aparece em nenhum registro do período.")
        if not documents:
            if documents_period:
                warnings.append(
                    f"{plural_count(documents_period, 'documento', 'documentos')} no período, mas nenhum ficou no "
                    f"escopo selecionado. Desmarque o escopo para vê-los.")
            else:
                warnings.append("Nenhum documento foi encontrado no período selecionado.")
        scope = {
            **scope_metadata(req),
            "documents_in_period": documents_period, "documents_in_scope": documents,
            "excluded": documents_period - documents,
        }
        return {"rows": rows, "count": len(rows), "documents": documents, "period": {"start": str(req.start), "end": str(req.end)},
                "read_only": True, "coverage": coverage, "warnings": warnings, "scope": scope,
                "value_field": req.value_field, "payments_total": payments_total,
                "value_note": COMPUTED_LABELS[req.value_field][1] if req.value_field in COMPUTED_LABELS else None}


@app.post("/api/report")
def report(req: ReportRequest):
    return run_report(req)


@app.post("/api/reconciliation")
def reconciliation(req: ReconciliationRequest):
    """Per-document comparison between the item formula and registered payments.

    The item formula is the same one used by the revenue report; this endpoint
    only exposes the documents that explain a gap, plus a summary by movement
    type. It is intentionally separate from /api/report so large periods can
    still load the headline total quickly and drill down on demand.
    """
    if req.end < req.start:
        raise HTTPException(400, "A data final precisa ser igual ou posterior à inicial.")
    date_expr = escape_field(req.date_field)
    if req.company_field:
        escape_field(req.company_field)
    match, scoped_match = period_match(req)
    if not MONGO_DATABASE:
        raise HTTPException(400, "Defina MONGO_DATABASE no arquivo .env.")

    company_expr = ("$" + req.company_field.replace("[]", "")) if req.company_field else "$Empresa.Nome"
    difference_match = {"$or": [
        {"Diferenca": {"$gt": 0.011}},
        {"Diferenca": {"$lt": -0.011}},
    ]}
    pipeline = [
        {"$match": scoped_match},
        {"$project": {
            "_id": 0,
            "Data": date_expr,
            "Numero": "$Numero",
            "Serie": "$Serie",
            "Empresa": company_expr,
            "Tipo": {"$arrayElemAt": [{"$ifNull": ["$_t", []]}, -1]},
            "Situacao": "$Situacao.Descricao",
            "Itens": {"$sum": {"$map": {
                "input": {"$ifNull": ["$ItensBase", []]},
                "as": "item",
                "in": rebase(COMPUTED_VALUES["@item_total"], "$$item."),
            }}},
            # Fiscal documents use ItensPagamentos; legacy point-of-sale records
            # use Pagamentos. Choose per document so the two are never summed.
            "Pagamentos": {"$sum": {"$cond": [
                {"$gt": [{"$size": {"$ifNull": ["$ItensPagamentos", []]}}, 0]},
                {"$ifNull": ["$ItensPagamentos.Valor", []]},
                {"$ifNull": ["$Pagamentos.Valor", []]},
            ]}},
        }},
        {"$project": {
            "Data": 1, "Numero": 1, "Serie": 1, "Empresa": 1, "Tipo": 1, "Situacao": 1,
            "Itens": 1, "Pagamentos": 1,
            "Diferenca": {"$subtract": ["$Itens", "$Pagamentos"]},
            "SemPagamento": {"$lte": ["$Pagamentos", 0]},
        }},
        {"$facet": {
            "by_type": [
                {"$group": {
                    "_id": "$Tipo",
                    "documents": {"$sum": 1},
                    "items": {"$sum": "$Itens"},
                    "payments": {"$sum": "$Pagamentos"},
                    "without_payment": {"$sum": {"$cond": ["$SemPagamento", 1, 0]}},
                }},
                {"$project": {
                    "_id": 0,
                    "Tipo": {"$ifNull": ["$_id", "Sem tipo"]},
                    "documents": 1, "items": 1, "payments": 1, "without_payment": 1,
                    "difference": {"$subtract": ["$items", "$payments"]},
                }},
                {"$addFields": {"difference_abs": {"$abs": "$difference"}}},
                {"$sort": {"difference_abs": -1, "documents": -1}},
                {"$project": {"difference_abs": 0}},
            ],
            "rows": [
                {"$match": difference_match},
                {"$addFields": {"DiferencaAbs": {"$abs": "$Diferenca"}}},
                {"$sort": {"DiferencaAbs": -1}},
                {"$limit": req.limit},
                {"$project": {"DiferencaAbs": 0}},
            ],
            "difference_count": [
                {"$match": difference_match},
                {"$group": {"_id": None, "n": {"$sum": 1}}},
            ],
        }},
    ]

    with mongo_client() as client:
        db = client[MONGO_DATABASE]
        if req.collection not in db.list_collection_names():
            raise HTTPException(404, "Coleção não encontrada.")
        coll = db[req.collection]
        documents_period = coll.count_documents(match)
        try:
            facet_rows = list(coll.aggregate(pipeline, maxTimeMS=20000, allowDiskUse=False))
        except PyMongoError as exc:
            raise HTTPException(422, f"Conferência não concluída: {exc.__class__.__name__}") from None

    facet = facet_rows[0] if facet_rows else {}
    raw_by_type = facet.get("by_type") or []
    items_total = sum(float(row.get("items") or 0) for row in raw_by_type)
    payments_total = sum(float(row.get("payments") or 0) for row in raw_by_type)
    documents = sum(int(row.get("documents") or 0) for row in raw_by_type)
    without_payment = sum(int(row.get("without_payment") or 0) for row in raw_by_type)
    items_total = round(items_total, 2)
    payments_total = round(payments_total, 2)
    difference = round(items_total - payments_total, 2)
    difference_pct = round(100 * difference / abs(payments_total), 2) if abs(payments_total) > 0.01 else None

    by_type = json_safe(raw_by_type)
    for row in by_type:
        row["items"] = round(float(row.get("items") or 0), 2)
        row["payments"] = round(float(row.get("payments") or 0), 2)
        row["difference"] = round(float(row.get("difference") or 0), 2)

    detail = json_safe(facet.get("rows") or [])
    for row in detail:
        row["Itens"] = round(float(row.get("Itens") or 0), 2)
        row["Pagamentos"] = round(float(row.get("Pagamentos") or 0), 2)
        row["Diferenca"] = round(float(row.get("Diferenca") or 0), 2)
        row["SemPagamento"] = bool(row.get("SemPagamento"))
    diff_count_rows = facet.get("difference_count") or []
    difference_count = int(diff_count_rows[0].get("n") or 0) if diff_count_rows else 0

    warnings: list[str] = []
    if not documents:
        if documents_period:
            warnings.append(
                f"{plural_count(documents_period, 'documento', 'documentos')} no período, mas nenhum ficou no escopo "
                f"selecionado.")
        else:
            warnings.append("Nenhum documento foi encontrado no período selecionado.")
    if without_payment:
        warnings.append(
            f"{plural_count(without_payment, 'documento não tem', 'documentos não têm')} pagamento registrado; "
            f"nesses casos a diferença é o próprio valor dos itens.")
    if documents and abs(difference) > 0.01:
        if difference_pct is None:
            warnings.append(
                f"Os itens somam {brl(items_total)}, mas não há pagamentos registrados no escopo; "
                f"diferença de {brl(difference)}.")
        else:
            pct_text = f"{difference_pct:.2f}".replace(".", ",")
            warnings.append(
                f"Os itens somam {brl(items_total)} e os pagamentos {brl(payments_total)}; diferença de "
                f"{brl(difference)} ({pct_text}% sobre os pagamentos).")

    return {
        "period": {"start": str(req.start), "end": str(req.end)},
        "scope": {
            **scope_metadata(req),
            "documents_in_period": documents_period,
            "documents_in_scope": documents,
            "excluded": documents_period - documents,
        },
        "read_only": True,
        "items_total": items_total,
        "payments_total": payments_total,
        "difference": difference,
        "difference_pct": difference_pct,
        "documents": documents,
        "documents_with_payment": documents - without_payment,
        "documents_without_payment": without_payment,
        "difference_count": difference_count,
        "by_type": by_type,
        "rows": detail,
        "limit": req.limit,
        "truncated": difference_count > len(detail),
        "warnings": warnings,
        "value_note": COMPUTED_LABELS["@item_total"][1],
        "payments_note": "ItensPagamentos[].Valor, com Pagamentos[].Valor como alternativa para registros antigos.",
    }


@app.post("/api/investigation")
def investigation(req: InvestigationRequest):
    from report_questions import investigate
    try:
        return investigate(req)
    except PyMongoError as exc:
        raise HTTPException(503, f"Não foi possível consultar os dados da investigação: {exc.__class__.__name__}.") from None


@app.post("/api/ask")
def ask(req: AskRequest):
    from report_questions import question_details
    details = question_details(req.question)
    q = req.question.casefold()
    if not q.strip():
        raise HTTPException(400, "Digite o que deseja consultar.")
    kind = None
    method = "regras locais"
    if AI_ENDPOINT and AI_MODEL:
        # The model receives only the question and may choose one of seven fixed operations.
        # Database names, field names, results and credentials are never sent to it.
        prompt = ('Classifique a pergunta em exatamente um tipo: revenue, top_products, documents, daily, companies, payments, ticket, investigation. '
                  'Responda apenas JSON {"kind":"..."}. Use revenue quando o pedido for faturamento ou total, '
                  'mesmo que cite uma empresa. Use companies apenas para ranking ou distribuição por empresa. '
                  'Use payments para formas ou meios de pagamento e espécies como dinheiro, cartão ou PIX. '
                  'Use ticket para ticket médio. Use investigation para investigar diferenças, valores que não batem, '
                  'documentos compartilhados entre vendedores, falta ou sobra de pagamentos. '
                  'Se incerto, use revenue. Nunca gere consulta ou código.')
        body = json.dumps({"model": AI_MODEL, "temperature": 0, "messages": [{"role": "system", "content": prompt}, {"role": "user", "content": req.question}]}).encode()
        headers = {"Content-Type": "application/json"}
        if AI_API_KEY: headers["Authorization"] = f"Bearer {AI_API_KEY}"
        try:
            url = AI_ENDPOINT.rstrip("/")
            if not url.endswith("/chat/completions"): url += "/v1/chat/completions"
            with urllib.request.urlopen(urllib.request.Request(url, data=body, headers=headers), timeout=8) as response:
                answer = json.loads(response.read().decode())["choices"][0]["message"]["content"]
            decoded = json.loads(answer)
            kind = decoded.get("kind") if isinstance(decoded, dict) else None
            if kind not in {"revenue", "top_products", "documents", "daily", "companies", "payments", "ticket", "investigation"}: kind = None
            method = "classificação de tipo por modelo; operação validada por lista permitida"
        except (OSError, ValueError, TypeError, KeyError, IndexError, urllib.error.URLError):
            kind = None
    if details["investigate"]:
        kind = "investigation"
        method = "interpretação local de investigação, data, vendedor e valor aproximado"
    if kind is None:
        method = "regras locais"
        explicit_payment = re.search(
            r"(formas?\s+de\s+pagamento|meios?\s+de\s+pagamento|esp[eé]cies?\s+de\s+pagamento|"
            r"por\s+esp[eé]cie|como\s+os\s+clientes\s+pagam)", q)
        if explicit_payment:
            kind = "payments"
        elif "ticket" in q or re.search(r"m[eé]dia\s+por\s+(venda|nota|documento)", q):
            kind = "ticket"
        elif any(word in q for word in ("produto", "item", "vendid")):
            kind = "top_products"
        elif re.search(r"(por|cada|distribu\w*)\s+(a\s+)?empresa", q):
            kind = "companies"
        elif any(word in q for word in ("documento", "nota", "cupom", "nfce")):
            kind = "documents"
        elif any(word in q for word in ("por dia", "diário", "diario", "diária", "diaria", "evolução", "evolucao")):
            kind = "daily"
        elif any(word in q for word in ("pagamento", "pagamentos", "dinheiro", "cartão", "cartao", "pix")):
            kind = "payments"
        elif any(word in q for word in ("faturamento", "receita", "valor", "venda")):
            kind = "revenue"
        elif any(word in q for word in ("empresa", "loja", "filial")):
            kind = "companies"
    if kind is None:
        raise HTTPException(422, "Não consegui identificar o relatório. Tente faturamento, vendas por dia, produtos mais vendidos, formas de pagamento, empresas ou documentos.")

    payload = req.model_dump(exclude={"question"})
    parsed_period = parse_period(req.question, date.today())
    period_source = "filtros"
    if parsed_period:
        payload["start"], payload["end"] = parsed_period
        period_source = "pergunta"

    matched_company = None
    if payload.get("company_field") and MONGO_DATABASE:
        try:
            with mongo_client() as client:
                if payload["collection"] in client[MONGO_DATABASE].list_collection_names():
                    matched_company = resolve_company(client[MONGO_DATABASE][payload["collection"]],
                                                      payload["company_field"], req.question)
                    if matched_company:
                        payload["company_value"] = matched_company
        except HTTPException:
            matched_company = None

    if re.search(r"\btodas?\s+(?:as\s+)?empresas?\b", strip_accents(q)):
        payload["company_value"] = None
    if kind == "investigation":
        result = investigation(InvestigationRequest(**payload, seller_query=details["seller_query"],
                                                   target_amount=details["target_amount"]))
        result["interpretation"]["method"] = method
        result["interpretation"]["period_source"] = period_source
        return result
    result = run_report(ReportRequest(**payload, kind=kind))

    period = result["period"]
    def br(value: str) -> str:
        year, month, day = value.split("-")
        return f"{day}/{month}/{year}"
    summary = [f"Relatório: {KIND_LABELS[kind]}", f"Período: {br(period['start'])} a {br(period['end'])}"]
    effective_company = payload.get("company_value")
    summary.append(f"Empresa: {effective_company}" if effective_company else "Empresa: todas")
    if req.movement_group:
        summary.append("Escopo: " + MOVEMENT_LABELS[req.movement_group])
    elif req.only_valid or req.only_sales:
        parts = []
        if req.only_sales:
            parts.append("somente vendas")
        if req.only_valid:
            parts.append("documentos válidos")
        summary.append("Escopo: " + " · ".join(parts))
    result["interpretation"] = {
        "kind": kind, "kind_label": KIND_LABELS[kind], "method": method, "operation_allowlisted": True,
        "period_source": period_source, "company": effective_company,
        "scope": scope_metadata(req),
        "summary": summary,
    }
    return result


# The 1.0 workspace keeps its local state separate from the DigiSat data source.
from insight_workspace import router as workspace_router, startup as workspace_startup, shutdown as workspace_shutdown
app.include_router(workspace_router)
from insight_editor import router as editor_router
app.include_router(editor_router)
from insight_editor_tools import router as editor_tools_router
app.include_router(editor_tools_router)
from insight_finance import router as finance_router
app.include_router(finance_router)
from insight_native import router as native_router
app.include_router(native_router)
from insight_bi import router as bi_router
app.include_router(bi_router)
from insight_bi_finance import router as bi_finance_router
app.include_router(bi_finance_router)


@app.get('/bi-finance', include_in_schema=False)
def financial_bi_page():
    return FileResponse(ROOT / 'static' / 'bi-finance.html')


@app.get('/bi', include_in_schema=False)
def business_intelligence_page():
    return FileResponse(ROOT / 'static' / 'bi.html')


@app.get('/database-editor', include_in_schema=False)
def database_editor_page():
    return FileResponse(ROOT / 'static' / 'editor.html')


@app.get('/reports', include_in_schema=False)
def report_catalog_page():
    return FileResponse(ROOT / 'static' / 'reports.html')


@app.get('/finance-reports', include_in_schema=False)
def finance_reports_page():
    return FileResponse(ROOT / 'static' / 'finance.html')


@app.get('/native-reports', include_in_schema=False)
def native_reports_page():
    return FileResponse(ROOT / 'static' / 'native.html')

app.add_event_handler("startup", workspace_startup)
app.add_event_handler("shutdown", workspace_shutdown)


@app.middleware("http")
async def response_cache_policy(request: Request, call_next):
    response = await call_next(request)
    response.headers["Cache-Control"] = "no-store" if request.url.path.startswith("/api/") else "no-cache"
    return response
