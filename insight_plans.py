"""Typed conversational plans, deterministic refinements, and clarifications."""
from __future__ import annotations

from datetime import date, timedelta
import json
import re
from typing import Literal
from pydantic import BaseModel, Field, ConfigDict, model_validator
from fastapi import HTTPException

KINDS = ("revenue", "top_products", "documents", "daily", "companies", "payments", "ticket", "investigation", "quality")
PROFILES = {
    "panel": {"label": "Painel — filtros selecionados", "description": "Tipos e situações definidos nos filtros. Não representa todos os relatórios nativos."},
    "native_nfce": {"label": "NFC-e concluídas — itens por vendedor", "description": "NFC-e, Concluído, finalidade normal, itens não cancelados. Regras conferidas para o mensal por vendedor; pagamentos continuam por documento, sem rateio."},
    "fiscal_valid": {"label": "Vendas fiscais válidas", "description": "NFC-e, NF-e de saída e notas manuais; sem cancelados, rejeitados e inutilizados."},
}


class Comparison(BaseModel):
    model_config = ConfigDict(extra="forbid")
    start: date
    end: date
    seller_query: str | None = Field(default=None, max_length=120)
    company_value: str | None = Field(default=None, max_length=120)
    label: str = Field(default="Comparação", max_length=150)


class Plan(BaseModel):
    model_config = ConfigDict(extra="ignore")
    collection: str = Field(default="Movimentacoes", max_length=120)
    kind: Literal["revenue", "top_products", "documents", "daily", "companies", "payments", "ticket", "investigation", "quality"] = "revenue"
    start: date
    end: date
    date_field: str = Field(default="DataHoraEmissao", max_length=120)
    value_field: str | None = Field(default="@item_total", max_length=120)
    product_field: str | None = Field(default="ItensBase[].ProdutoServico.Descricao", max_length=120)
    company_field: str | None = Field(default="Empresa.Nome", max_length=120)
    company_value: str | None = Field(default=None, max_length=120)
    only_valid: bool = True
    status_filter: Literal["valid", "all", "completed"] = "valid"
    only_sales: bool = False
    movement_group: Literal["fiscal_sales", "all_sales", "nfce", "nfe", "manual", "registro_venda", "purchases", "all"] | None = "all_sales"
    seller_query: str | None = Field(default=None, max_length=120)
    target_amount: float | None = Field(default=None, ge=0, le=1e12, allow_inf_nan=False)
    profile: Literal["panel", "native_nfce", "fiscal_valid"] = "panel"
    comparison: Comparison | None = None
    page_size: int = Field(default=50, ge=1, le=500)
    offset: int = Field(default=0, ge=0, le=1000000)
    search: str = Field(default="", max_length=100)
    issue: Literal["all", "mismatch", "unpaid", "unassigned", "shared", "negative"] = "all"

    @model_validator(mode="after")
    def period_is_valid(self):
        if self.end < self.start:
            raise ValueError("A data final precisa ser igual ou posterior à inicial.")
        if (self.end - self.start).days > 366:
            raise ValueError("Escolha um período de até 367 dias.")
        if self.comparison and (self.comparison.end < self.comparison.start or (self.comparison.end - self.comparison.start).days > 366):
            raise ValueError("Período de comparação inválido; escolha até 367 dias.")
        return self


def classify(question: str) -> str | None:
    from app import strip_accents
    from report_questions import question_details
    q = strip_accents(question.casefold())
    if re.search(r"qualidade|dados incompletos|duplicad|sem vendedor", q):
        return "quality"
    if question_details(q)["investigate"]:
        return "investigation"
    if re.search(r"ticket|media por (?:venda|nota|documento)", q):
        return "ticket"
    if re.search(r"formas? de pagamento|meios? de pagamento|pix|cartao|dinheiro", q):
        return "payments"
    if re.search(r"por dia|diari[oa]|evolucao", q):
        return "daily"
    if re.search(r"produtos?|mais vendidos", q):
        return "top_products"
    if re.search(r"por empresa|cada empresa|ranking.*(?:empresa|loja)", q):
        return "companies"
    if re.search(r"documentos?|notas?|cupons?|liste.*vendas", q):
        return "documents"
    if re.search(r"faturamento|receita|quanto.*vend|\btotal\b|\bvendas?\b", q):
        return "revenue"
    return None


def clarification(field: str, question: str, options=None):
    return {"field": field, "question": question, "options": options or []}


def classify_optional_model(question: str) -> str | None:
    import app
    import urllib.request
    if not app.AI_ENDPOINT or not app.AI_MODEL:
        return None
    prompt = "Classifique apenas o tipo de relatório: " + ", ".join(KINDS) + '. Retorne JSON {"kind":"..."}; use {"kind":null} se ambíguo ou fora do escopo. Não gere consultas, filtros nem código. investigation investiga divergências; quality verifica dados incompletos.'
    body = json.dumps({"model": app.AI_MODEL, "temperature": 0, "messages": [{"role": "system", "content": prompt}, {"role": "user", "content": question}]}).encode()
    headers = {"Content-Type": "application/json"}
    if app.AI_API_KEY:
        headers["Authorization"] = "Bearer " + app.AI_API_KEY
    url = app.AI_ENDPOINT.rstrip("/")
    if not url.endswith("/chat/completions"):
        url += "/v1/chat/completions"
    try:
        with urllib.request.urlopen(urllib.request.Request(url, data=body, headers=headers), timeout=8) as response:
            result = json.loads(response.read(65536))["choices"][0]["message"]["content"]
        result = json.loads(result)
        kind = result.get("kind") if isinstance(result, dict) else None
        return kind if kind in KINDS else None
    except (OSError, ValueError, KeyError, IndexError, TypeError):
        return None


def explicit_period(text: str, anchor: date):
    import app
    iso = re.findall(r"\b(\d{4}-\d{2}-\d{2})\b", text)
    if iso:
        try:
            values = [date.fromisoformat(value) for value in iso]
        except ValueError:
            raise HTTPException(422, "Data inválida. Use dia/mês/ano ou AAAA-MM-DD.") from None
        return min(values), max(values)
    return app.parse_period(text, anchor)


def infer_plan(question: str, base: Plan, has_context: bool) -> tuple[Plan, dict | None]:
    import app
    from report_questions import question_details
    q = app.strip_accents(question.casefold()).strip()
    data = base.model_dump(mode="json")
    data["offset"] = 0
    details = question_details(question)
    seller_prompt = None
    if re.search(r"\bvendedor(?:a)?\b", q) and not details["seller_query"] and not re.search(r"todos|todas|mesmo|mesma|aquele|aquela|sem filtro|sem vendedor", q):
        data["kind"] = "investigation"
        seller_prompt = clarification("seller_query", "Qual vendedor você quer consultar? Informe o número ou o nome completo.")
    if re.search(r"aquele vendedor|aquela vendedora|mesmo vendedor|mesma vendedora", q) and not base.seller_query:
        data["kind"] = "investigation"
        seller_prompt = clarification("seller_query", "Qual vendedor? Informe o número cadastrado ou o nome completo.")
    if has_context and base.seller_query and re.fullmatch(r"e\s+(?:o|a)\s+(\d+)\s*\??", q):
        details["seller_query"] = re.search(r"\d+", q).group()
    inferred = classify(question)
    if inferred is None and not has_context and not seller_prompt:
        inferred = classify_optional_model(question)
    if inferred:
        data["kind"] = inferred
    elif not has_context and not seller_prompt:
        return base, clarification("kind", "Qual consulta você quer iniciar?", [
            {"label": "Faturamento", "value": "revenue"}, {"label": "Investigar uma diferença", "value": "investigation"},
            {"label": "Listar vendas", "value": "documents"}, {"label": "Qualidade dos dados", "value": "quality"}])
    if details["seller_query"]:
        data["seller_query"] = details["seller_query"]
        data["kind"] = "investigation" if data["kind"] not in {"quality"} else "quality"
    if re.search(r"todos? (?:os )?vendedores|todas? (?:as )?vendedoras|sem filtro de vendedor", q):
        data["seller_query"] = None
    if details["target_amount"] is not None:
        data["target_amount"] = details["target_amount"]
    if re.search(r"sem (?:o )?valor (?:aproximado|de referencia)", q):
        data["target_amount"] = None
    if re.search(r"aquele vendedor|aquela vendedora|mesmo vendedor", q) and not data["seller_query"]:
        seller_prompt = clarification("seller_query", "Qual vendedor? Informe o número cadastrado ou o nome completo.")
    scope_patterns = [("nfce", r"nfc[ -]?e"), ("nfe", r"(?<!c)nf[ -]?e(?: de saida)?"),
                      ("manual", r"notas? manuais"), ("registro_venda", r"registros? de venda"),
                      ("purchases", r"compras|entradas"), ("all_sales", r"todas as vendas|fiscal e registros")]
    for scope, pattern in scope_patterns:
        if re.search(pattern, q):
            data["movement_group"] = scope
            if data["profile"] == "native_nfce" and scope != "nfce":
                data["profile"] = "panel"
            break
    if re.search(r"sem cancelad|exclu\w* cancelad|documentos validos", q):
        data["only_valid"], data["status_filter"] = True, "valid"
    if re.search(r"inclu\w* cancelad|inclusive cancelad|com cancelad|todas as situacoes", q):
        data["only_valid"], data["status_filter"] = False, "all"
        data["profile"] = "panel"
    if re.search(r"so\s+(?:as\s+)?concluid|somente\s+(?:as\s+)?concluid|situacao\s+concluid", q):
        data["only_valid"], data["status_filter"] = True, "completed"
    if re.search(r"perfil nativo|igual ao nativo|nfce concluidas|nfc-e concluidas", q):
        data["profile"] = "native_nfce"
    if re.search(r"perfil (?:do )?painel", q):
        data["profile"] = "panel"
    if re.search(r"todas as empresas|sem filtro de empresa", q):
        data["company_value"] = None
    quoted_companies = re.findall(r'empresa\s+["“]([^"”]+)["”]', question, re.I)
    if quoted_companies:
        data["company_field"] = "Empresa.Nome"
        data["company_value"] = quoted_companies[0]
    for issue, pattern in [("unpaid", r"somente.*sem pagamento|so.*sem pagamento"),
                           ("unassigned", r"somente.*sem vendedor|so.*sem vendedor"),
                           ("mismatch", r"somente.*(?:diferenca|divergencia)|so.*(?:diferenca|divergencia)"),
                           ("shared", r"somente.*compartilhad|so.*compartilhad")]:
        if re.search(pattern, q):
            data["issue"] = issue
            data["kind"] = "quality" if issue == "unassigned" else "investigation"
    if re.search(r"mostrar todas|liste todas|sem filtro de problema", q):
        data["issue"] = "all"
    compare = bool(re.search(r"\bcompar(?:e|ar|acao|ando|a|em|ativo|ativa|ativos|ativas)\b|\bversus\b|\bvs\b|\bcontra\b", q))
    if re.search(r"sem comparacao|remova a comparacao", q):
        data["comparison"] = None
        compare = False
    if compare:
        data["kind"] = "investigation"
        split = re.split(r"\s+(?:com|versus|vs\.?|contra)\s+", question, maxsplit=1, flags=re.I)
        sellers = re.findall(r"\bvendedor(?:a)?\s+(\d+)\b", q)
        first_period = explicit_period(split[0], date.today()) if len(split) > 1 else None
        if first_period:
            if (first_period[1] - first_period[0]).days > 366:
                return Plan(**data), clarification("period", "Escolha um período de até 367 dias para a base da comparação.")
            data["start"], data["end"] = map(str, first_period)
        other = explicit_period(split[1], date.today()) if len(split) > 1 else None
        start = date.fromisoformat(data["start"])
        end = date.fromisoformat(data["end"])
        if len(sellers) >= 2:
            data["seller_query"] = sellers[0]
        if other:
            a, b = other
            if (b - a).days > 366:
                return Plan(**data), clarification("comparison_period", "Escolha um período de comparação de até 367 dias.")
        elif re.search(r"ontem", q):
            a = b = date.today() - timedelta(days=1)
        elif re.search(r"anterior|passad", q):
            b = start - timedelta(days=1)
            a = b - (end - start)
        elif len(sellers) < 2 and len(quoted_companies) < 2 and not (has_context and (details["seller_query"] or quoted_companies)):
            return Plan(**data), clarification("comparison_period", "Com qual período comparar? Informe uma data ou um intervalo, por exemplo 06/10/2026.")
        else:
            a, b = start, end
        other_seller = sellers[1] if len(sellers) >= 2 else data["seller_query"]
        if has_context and len(sellers) < 2 and details["seller_query"] and not first_period:
            other_seller = details["seller_query"]
            data["seller_query"] = base.seller_query
        other_company = quoted_companies[1] if len(quoted_companies) > 1 else data["company_value"]
        if has_context and len(quoted_companies) == 1 and not first_period:
            other_company = quoted_companies[0]
            data["company_value"] = base.company_value
        data["comparison"] = {"start": str(a), "end": str(b), "seller_query": other_seller,
                              "company_value": other_company,
                              "label": "Comparação solicitada"}
    else:
        parsed = explicit_period(question, date.today())
        if re.search(r"dia seguinte|proximo dia", q) and has_context:
            parsed = base.start + timedelta(days=1), base.end + timedelta(days=1)
        elif re.search(r"dia anterior", q) and has_context:
            parsed = base.start - timedelta(days=1), base.end - timedelta(days=1)
        if parsed:
            if (parsed[1] - parsed[0]).days > 366:
                return Plan(**data), clarification("period", "Escolha um período de até 367 dias. Qual intervalo deseja consultar?")
            data["start"], data["end"] = map(str, parsed)
        elif re.search(r"\bdia\s+\d{1,2}\b", q) and not seller_prompt:
            return Plan(**data), clarification("period", "Qual a data completa? Informe dia/mês/ano para não escolhermos um mês incorreto.")
        elif re.search(r"mesmo dia|naquele dia", q) and (not has_context or base.start != base.end) and not seller_prompt:
            return Plan(**data), clarification("period", "A qual dia você se refere? Informe dia/mês/ano.")
    if data["profile"] == "native_nfce":
        data.update(movement_group="nfce", only_valid=True, status_filter="completed", kind="quality" if data["kind"] == "quality" else "investigation")
    elif data["profile"] == "fiscal_valid":
        data.update(movement_group="fiscal_sales", only_valid=True)
    if seller_prompt:
        return Plan(**data), seller_prompt
    if not inferred and has_context and all(data.get(k) == v for k, v in base.model_dump(mode="json").items() if k != "offset") and not re.search(r"atualiz|repet|execut|refa[cç]|novamente|mostrar|mostre|cancelad|empresas|vendedor|nfc|situac|perfil|compartilhad|valid|pagamento|\bdia\b|\bmes\b|\bano\b|ontem|hoje", q):
        return Plan(**data), clarification("kind", "Não identifiquei o ajuste. Informe um vendedor, uma data, um escopo ou escolha o relatório.",
                                         [{"label": "Faturamento", "value": "revenue"}, {"label": "Investigação", "value": "investigation"}, {"label": "Documentos", "value": "documents"}])
    return Plan(**data), None


def plan_summary(plan: Plan) -> list[str]:
    import app
    return [f'Consulta: {app.KIND_LABELS.get(plan.kind, {"investigation": "Investigação", "quality": "Qualidade dos dados"}.get(plan.kind, plan.kind))}',
            f'Período: {plan.start:%d/%m/%Y} a {plan.end:%d/%m/%Y}',
            f'Vendedor: {plan.seller_query or "todos"}', f'Empresa: {plan.company_value or "todas"}',
            f'Perfil: {PROFILES[plan.profile]["label"]}',
            f'Escopo: {app.MOVEMENT_LABELS.get(plan.movement_group, "filtros selecionados")}',
            "Situações: Concluído" if plan.status_filter == "completed" or plan.profile == "native_nfce" else "Situações: válidas" if plan.only_valid else "Situações: todas"]
