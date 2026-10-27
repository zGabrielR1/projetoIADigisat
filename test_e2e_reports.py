"""End-to-end tests for the report API and the diagnostic CLI.

Run with the project virtualenv:
    .\\.venv\\Scripts\\python.exe test_e2e_reports.py -v

The HTTP tests start a real uvicorn process on a free loopback port. The
integration tests read the configured DigiSat MongoDB and are skipped when it
is unavailable. Nothing writes to MongoDB.
"""
from __future__ import annotations

import importlib.util
import base64
import json
import os
import socket
import subprocess
import sys
import tempfile
import time
import unittest
import urllib.error
import urllib.request
from datetime import date, datetime, timedelta
from pathlib import Path

import app

ROOT = Path(__file__).parent
TOOL = ROOT / "engenharia" / "ferramentas" / "diagnostico_relatorios.py"

SEP_START = date(2026, 9, 1)
SEP_END = date(2026, 9, 30)
SELLER2 = "6496ee283a5e352378b9e8f1"

# Deliberately duplicated from app.py so the tests verify the report output
# instead of reusing the same constants the implementation uses.
SALES_TYPES = [
    "NotaFiscalConsumidorEletronica",
    "NotaFiscalEletronicaSaida",
    "NotaFiscalManual",
    "RegistroVenda",
]
EXCLUDED_STATUS = ["Cancelado", "Rejeitado", "Inutilizado"]
KINDS = ["revenue", "top_products", "daily", "companies", "documents", "payments", "ticket"]


def _if(field):
    return {"$ifNull": [field, 0]}


def _round2(expression):
    return {"$divide": [{"$trunc": [{"$add": [
        {"$multiply": [expression, 100]},
        {"$cond": [{"$gte": [expression, 0]}, 0.500000001, -0.500000001]},
    ]}]}, 100]}


ITEM_TOTAL = {"$subtract": [
    {"$add": [
        _round2({"$multiply": [_if("$ItensBase.PrecoUnitario"), _if("$ItensBase.Quantidade")]}),
        _if("$ItensBase.OutrasDespesasProporcional"),
        _if("$ItensBase.OutrasDespesasDigitado"),
        _if("$ItensBase.Percentual.ValorSt"),
        _if("$ItensBase.IPI.Valor"),
        _if("$ItensBase.Frete"),
        _if("$ItensBase.Seguro"),
    ]},
    {"$add": [_if("$ItensBase.DescontoDigitado"), _if("$ItensBase.DescontoProporcional")]},
]}

ITEM_PROJECTION = {
    "DataHoraEmissao": 1, "Empresa.Nome": 1,
    "ItensBase.Quantidade": 1, "ItensBase.PrecoUnitario": 1,
    "ItensBase.DescontoDigitado": 1, "ItensBase.DescontoProporcional": 1,
    "ItensBase.OutrasDespesasProporcional": 1, "ItensBase.OutrasDespesasDigitado": 1,
    "ItensBase.Percentual.ValorSt": 1, "ItensBase.IPI.Valor": 1,
    "ItensBase.Frete": 1, "ItensBase.Seguro": 1, "ItensBase.Cancelado": 1,
    "ItensBase.ProdutoServico.Descricao": 1,
}


def mongo_available() -> bool:
    try:
        client = app.mongo_client()
        client.admin.command("ping")
        client.close()
        return True
    except Exception:
        return False


def scope_match(start: date, end: date, *, company: str | None = None,
                only_valid: bool = True, group: str | None = "all_sales") -> dict:
    """Independent copy of the request-to-Mongo filter used by the API."""
    match = {"DataHoraEmissao": {
        "$gte": datetime.combine(start, datetime.min.time()),
        "$lt": datetime.combine(end + timedelta(days=1), datetime.min.time()),
    }}
    if company is not None:
        match["Empresa.Nome"] = company
    if only_valid:
        match["Situacao.Descricao"] = {"$nin": EXCLUDED_STATUS}
    if group == "all_sales":
        match["_t"] = {"$in": SALES_TYPES}
    elif group is not None:
        types = app.MOVEMENT_GROUPS.get(group)
        if types:
            match["_t"] = {"$in": types}
    return match


def item_rows(db, match: dict, group_field: str | None = None) -> list[dict]:
    group = {"_id": ("$" + group_field.replace("[]", "")) if group_field else None,
             "total": {"$sum": ITEM_TOTAL}, "items": {"$sum": 1}}
    pipeline = [
        {"$match": match},
        {"$project": ITEM_PROJECTION},
        {"$unwind": "$ItensBase"},
        {"$match": {"ItensBase.Cancelado": {"$ne": True}}},
        {"$group": group},
    ]
    return list(db["Movimentacoes"].aggregate(pipeline, maxTimeMS=60000, allowDiskUse=False))


def payment_total(db, match: dict) -> float:
    pipeline = [
        {"$match": match},
        {"$project": {"payment": {"$cond": [
            {"$gt": [{"$size": {"$ifNull": ["$ItensPagamentos", []]}}, 0]},
            "$ItensPagamentos.Valor",
            {"$ifNull": ["$Pagamentos.Valor", []]},
        ]}}},
        {"$unwind": "$payment"},
        {"$group": {"_id": None, "total": {"$sum": "$payment"}}},
    ]
    rows = list(db["Movimentacoes"].aggregate(pipeline, maxTimeMS=60000, allowDiskUse=False))
    return round(float(rows[0]["total"] or 0), 2) if rows else 0.0


def payload(kind: str, **overrides) -> dict:
    body = {
        "collection": "Movimentacoes",
        "kind": kind,
        "start": SEP_START.isoformat(),
        "end": SEP_END.isoformat(),
        "date_field": "DataHoraEmissao",
        "value_field": "@item_total",
        "product_field": "ItensBase[].ProdutoServico.Descricao",
        "company_field": "Empresa.Nome",
        "company_value": None,
        "only_valid": True,
        "only_sales": False,
        "movement_group": "all_sales",
        "limit": 50,
    }
    body.update(overrides)
    return body


class MongoIntegration(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        if not mongo_available():
            raise unittest.SkipTest("MongoDB is not available for the E2E integration tests")
        cls.client = app.mongo_client()
        cls.db = cls.client[app.MONGO_DATABASE]

    @classmethod
    def tearDownClass(cls):
        cls.client.close()


class ApiE2E(MongoIntegration):
    """Starts a real uvicorn process and exercises every report endpoint."""

    @classmethod
    def setUpClass(cls):
        super().setUpClass()
        cls.port = cls._free_port()
        descriptor, path = tempfile.mkstemp(suffix=".log")
        os.close(descriptor)
        cls.log_path = Path(path)
        cls.log = open(cls.log_path, "w", encoding="utf-8")
        environment = os.environ.copy()
        environment.update({"PYTHONIOENCODING": "utf-8", "INSIGHT_HOST": "127.0.0.1",
                            "INSIGHT_USER": "", "INSIGHT_PASSWORD": ""})
        cls.process = subprocess.Popen(
            [sys.executable, "-m", "uvicorn", "app:app", "--host", "127.0.0.1",
             "--port", str(cls.port), "--log-level", "warning"],
            cwd=str(ROOT), env=environment, stdout=cls.log, stderr=subprocess.STDOUT,
        )
        cls.base = f"http://127.0.0.1:{cls.port}"
        deadline = time.time() + 30
        while time.time() < deadline:
            try:
                with urllib.request.urlopen(cls.base + "/api/status", timeout=2) as response:
                    if response.status == 200:
                        return
            except Exception:
                time.sleep(0.25)
        cls.process.terminate()
        cls.process.wait(timeout=10)
        cls.log.close()
        tail = cls.log_path.read_text(encoding="utf-8", errors="replace")[-2000:]
        raise RuntimeError(f"uvicorn did not start on {cls.base}\n{tail}")

    @classmethod
    def tearDownClass(cls):
        cls.process.terminate()
        try:
            cls.process.wait(timeout=10)
        except subprocess.TimeoutExpired:
            cls.process.kill()
            cls.process.wait(timeout=5)
        cls.log.close()
        try:
            cls.log_path.unlink(missing_ok=True)
        except PermissionError:
            pass
        super().tearDownClass()

    @staticmethod
    def _free_port() -> int:
        with socket.socket() as sock:
            sock.bind(("127.0.0.1", 0))
            return sock.getsockname()[1]

    def request(self, path: str, body: dict | None = None) -> tuple[int, dict]:
        data = json.dumps(body).encode() if body is not None else None
        request = urllib.request.Request(
            self.base + path, data=data, method="POST" if data is not None else "GET",
            headers={"Content-Type": "application/json"} if data is not None else {})
        try:
            with urllib.request.urlopen(request, timeout=90) as response:
                return response.status, json.loads(response.read().decode("utf-8"))
        except urllib.error.HTTPError as error:
            raw = error.read().decode("utf-8", errors="replace")
            error.close()
            try:
                return error.code, json.loads(raw)
            except json.JSONDecodeError:
                return error.code, {"detail": raw}

    def assert_ok(self, path: str, body: dict | None = None) -> dict:
        status, data = self.request(path, body)
        self.assertEqual(status, 200, f"{path} returned {status}: {data}")
        return data

    # ---------------------------------------------------------------- surface
    def test_status_collections_schema_and_static_ui(self):
        status = self.assert_ok("/api/status")
        self.assertTrue(status["connected"])
        self.assertEqual(status["database"], app.MONGO_DATABASE)

        collections = self.assert_ok("/api/collections")
        self.assertIn("Movimentacoes", [item["name"] for item in collections])

        schema = self.assert_ok("/api/schema/Movimentacoes")
        self.assertGreater(schema["sampled"], 0)
        self.assertTrue(schema["fields"])
        self.assertTrue(schema["suggested"]["date"])
        self.assertTrue(any(item["name"] == "@item_total" for item in schema["computed"]))
        self.assertEqual(schema["suggested"]["value"], "@item_total")

        values = self.assert_ok("/api/values/Movimentacoes?field=Empresa.Nome")
        self.assertTrue(values["values"])

        with urllib.request.urlopen(self.base + "/", timeout=30) as response:
            html = response.read().decode("utf-8")
        for kind in KINDS:
            self.assertIn(f'data-kind="{kind}"', html, f"missing report card for {kind}")
        with urllib.request.urlopen(self.base + "/static/app.js", timeout=30) as response:
            script = response.read().decode("utf-8")
        self.assertIn("KIND_LABELS", script)
        for kind in KINDS:
            self.assertIn(f"{kind}:", script)

    # --------------------------------------------------------------- reports
    def test_every_report_kind_has_consistent_shape_and_totals(self):
        expected_match = scope_match(SEP_START, SEP_END)
        expected_rows = item_rows(self.db, expected_match)
        expected_total = round(sum(float(row["total"] or 0) for row in expected_rows), 2)
        expected_items = sum(int(row["items"] or 0) for row in expected_rows)
        expected_payments = payment_total(self.db, expected_match)
        expected_documents = self.db["Movimentacoes"].count_documents(expected_match)

        for kind in KINDS:
            with self.subTest(kind=kind):
                data = self.assert_ok("/api/report", payload(kind))
                self.assertTrue(data["read_only"])
                self.assertEqual(data["period"], {"start": SEP_START.isoformat(), "end": SEP_END.isoformat()})
                self.assertGreater(data["documents"], 0)
                self.assertIsInstance(data["rows"], list)
                self.assertEqual(data["scope"]["documents_in_scope"], expected_documents)
                self.assertEqual(data["scope"]["documents_in_period"],
                                 self.db["Movimentacoes"].count_documents(
                                     scope_match(SEP_START, SEP_END, only_valid=False, group=None)))
                self.assertLessEqual(len(data["rows"]), 50)
                if kind in ("revenue", "daily", "ticket"):
                    total = round(sum(float(row.get("total") or 0) for row in data["rows"]), 2)
                    self.assertAlmostEqual(total, expected_total, delta=0.02,
                                           msg=f"{kind} total does not match the database")
                if kind == "revenue":
                    self.assertAlmostEqual(float(data["rows"][0]["total"]), expected_total, delta=0.02)
                    self.assertAlmostEqual(data["payments_total"], expected_payments, delta=0.02)
                    self.assertEqual(data["rows"][0]["items"], expected_items)
                if kind == "daily":
                    self.assertEqual(sum(int(row["documents"]) for row in data["rows"]), expected_documents)
                    self.assertEqual([row["_id"] for row in data["rows"]], sorted(row["_id"] for row in data["rows"]))
                if kind in ("top_products", "companies"):
                    field = ("ItensBase[].ProdutoServico.Descricao" if kind == "top_products"
                             else "Empresa.Nome")
                    expected_groups = {row["_id"]: row for row in item_rows(self.db, expected_match, field)}
                    for row in data["rows"][:5]:
                        expected_row = expected_groups.get(row["_id"])
                        self.assertIsNotNone(expected_row, f"{kind} row {row['_id']!r} not found independently")
                        self.assertAlmostEqual(float(row["total"]), float(expected_row["total"] or 0),
                                               delta=0.02)
                        self.assertEqual(int(row["items"]), int(expected_row["items"] or 0))
                    if kind == "top_products":
                        totals = [float(row.get("total") or 0) for row in data["rows"]]
                        self.assertEqual(totals, sorted(totals, reverse=True))
                if kind == "documents":
                    self.assertEqual(len(data["rows"]), min(50, expected_documents))
                    for row in data["rows"]:
                        self.assertTrue(SEP_START.isoformat() <= str(row["Data"])[:10] <= SEP_END.isoformat())
                if kind == "payments":
                    self.assertAlmostEqual(sum(float(row["total"]) for row in data["rows"]),
                                           expected_payments, delta=0.02)
                    self.assertAlmostEqual(sum(float(row["participacao"]) for row in data["rows"]),
                                           100.0, delta=0.1)
                    self.assertLessEqual(data["documents_with_payment"], data["documents"])
                if kind == "ticket":
                    self.assertEqual(sum(int(row["documents"]) for row in data["rows"]), expected_documents)
                    self.assertAlmostEqual(data["ticket_total"],
                                           round(expected_total / expected_documents, 2), delta=0.02)

    # ---------------------------------------------------------- parameters
    def test_only_valid_parameter_includes_or_excludes_cancelled_documents(self):
        valid = self.assert_ok("/api/report", payload("revenue", only_valid=True))
        everything = self.assert_ok("/api/report", payload("revenue", only_valid=False))
        self.assertLessEqual(valid["documents"], everything["documents"])
        cancelled_match = scope_match(SEP_START, SEP_END, only_valid=False)
        cancelled_match["Situacao.Descricao"] = {"$in": EXCLUDED_STATUS}
        cancelled = self.db["Movimentacoes"].count_documents(cancelled_match)
        self.assertEqual(everything["documents"] - valid["documents"], cancelled)
        if cancelled:
            self.assertGreater(valid["scope"]["excluded"], 0)

    def test_movement_group_parameter_is_ordered_and_applied(self):
        counts = {}
        totals = {}
        for group in ("nfce", "fiscal_sales", "all_sales", "all"):
            data = self.assert_ok("/api/report", payload("revenue", movement_group=group))
            counts[group] = data["documents"]
            totals[group] = float(data["rows"][0]["total"]) if data["rows"] else 0.0
            self.assertEqual(data["scope"]["movement_group"], group)
        self.assertLessEqual(counts["nfce"], counts["fiscal_sales"])
        self.assertLessEqual(counts["fiscal_sales"], counts["all_sales"])
        self.assertLessEqual(counts["all_sales"], counts["all"])
        self.assertLessEqual(totals["nfce"], totals["fiscal_sales"] + 0.01)
        self.assertLessEqual(totals["fiscal_sales"], totals["all_sales"] + 0.01)
        purchases = self.assert_ok("/api/report", payload("revenue", movement_group="purchases"))
        self.assertGreater(purchases["documents"], 0)

    def test_company_parameter_filters_and_missing_field_is_rejected(self):
        values = self.assert_ok("/api/values/Movimentacoes?field=Empresa.Nome")["values"]
        if not values:
            self.skipTest("No company value available")
        company = values[0]
        filtered = self.assert_ok("/api/report", payload("revenue", company_value=company))
        unfiltered = self.assert_ok("/api/report", payload("revenue"))
        self.assertLessEqual(filtered["documents"], unfiltered["documents"])
        self.assertLessEqual(
            sum(float(row.get("total") or 0) for row in filtered["rows"]),
            sum(float(row.get("total") or 0) for row in unfiltered["rows"]) + 0.01)
        empty = self.assert_ok("/api/report", payload("revenue", company_value="__NO_SUCH_COMPANY__"))
        self.assertEqual(empty["documents"], 0)
        self.assertTrue(empty["warnings"])
        status, body = self.request("/api/report", payload("revenue", company_value=company, company_field=None))
        self.assertEqual(status, 400)
        self.assertIn("empresa", str(body["detail"]).lower())

    def test_date_parameter_is_inclusive_and_invalid_ranges_are_rejected(self):
        one_day = self.assert_ok("/api/report", payload("revenue", start="2026-09-01", end="2026-09-01"))
        month = self.assert_ok("/api/report", payload("revenue"))
        self.assertLessEqual(one_day["documents"], month["documents"])
        self.assertLessEqual(float(one_day["rows"][0]["total"]), float(month["rows"][0]["total"]) + 0.01)
        status, body = self.request("/api/report", payload("revenue", start="2026-09-30", end="2026-09-01"))
        self.assertEqual(status, 400)
        self.assertIn("data final", str(body["detail"]).lower())
        empty = self.assert_ok("/api/report", payload("revenue", start="2020-01-01", end="2020-01-02"))
        self.assertEqual(empty["documents"], 0)
        self.assertTrue(empty["warnings"])

    def test_limit_parameter_caps_rows_and_is_bounded(self):
        for kind in ("top_products", "daily", "companies", "payments", "ticket", "documents"):
            with self.subTest(kind=kind):
                data = self.assert_ok("/api/report", payload(kind, limit=3))
                self.assertLessEqual(len(data["rows"]), 3)
                self.assertLessEqual(len(data["rows"]), data.get("limit", 3))
        status, _ = self.request("/api/report", payload("revenue", limit=0))
        self.assertEqual(status, 422)
        status, _ = self.request("/api/report", payload("revenue", limit=100000))
        self.assertEqual(status, 422)

    def test_required_mapping_parameters_are_explained(self):
        cases = [
            ("revenue", {"value_field": None}, "valor"),
            ("top_products", {"product_field": None}, "produto"),
            ("companies", {"company_field": None}, "empresa"),
            ("ticket", {"value_field": None}, "valor"),
        ]
        for kind, overrides, word in cases:
            with self.subTest(kind=kind):
                status, body = self.request("/api/report", payload(kind, **overrides))
                self.assertEqual(status, 400)
                self.assertIn(word, str(body["detail"]).lower())
        status, _ = self.request("/api/report", payload("revenue", date_field="bad field!"))
        self.assertEqual(status, 400)
        unknown = payload("revenue")
        unknown["kind"] = "not_a_kind"
        status, _ = self.request("/api/report", unknown)
        self.assertEqual(status, 422)
        status, _ = self.request("/api/report", payload("revenue", movement_group="not_a_group"))
        self.assertEqual(status, 422)

    # -------------------------------------------------------------- ask/recon
    def test_ask_classifies_each_report_and_uses_the_same_contract(self):
        questions = {
            "revenue": "faturamento de 01/09/2026 a 30/09/2026",
            "top_products": "quais os produtos mais vendidos?",
            "daily": "vendas por dia de 01/09/2026 a 30/09/2026",
            "companies": "vendas por empresa de 01/09/2026 a 30/09/2026",
            "documents": "notas emitidas de 01/09/2026 a 30/09/2026",
            "payments": "formas de pagamento de 01/09/2026 a 30/09/2026",
            "ticket": "ticket médio de 01/09/2026 a 30/09/2026",
        }
        for kind, question in questions.items():
            with self.subTest(kind=kind):
                body = payload(kind)
                body.pop("kind")
                body["question"] = question
                data = self.assert_ok("/api/ask", body)
                self.assertEqual(data["interpretation"]["kind"], kind)
                self.assertTrue(data["interpretation"]["operation_allowlisted"])
                self.assertTrue(data["interpretation"]["summary"][0].startswith("Relatório:"))
                self.assertEqual(data["period"], {"start": "2026-09-01", "end": "2026-09-30"})
        status, _ = self.request("/api/ask", {**payload("revenue"), "question": "   "})
        self.assertEqual(status, 400)

    def test_reconciliation_parameter_and_arithmetic_invariants(self):
        data = self.assert_ok("/api/reconciliation", {
            "collection": "Movimentacoes", "start": SEP_START.isoformat(), "end": SEP_END.isoformat(),
            "date_field": "DataHoraEmissao", "company_field": "Empresa.Nome", "company_value": None,
            "only_valid": True, "only_sales": False, "movement_group": "all_sales", "limit": 20,
        })
        self.assertEqual(round(data["items_total"] - data["payments_total"], 2), data["difference"])
        self.assertEqual(data["documents_with_payment"] + data["documents_without_payment"], data["documents"])
        self.assertEqual(sum(int(row["documents"]) for row in data["by_type"]), data["documents"])
        self.assertAlmostEqual(sum(float(row["items"]) for row in data["by_type"]), data["items_total"], delta=0.02)
        self.assertAlmostEqual(sum(float(row["payments"]) for row in data["by_type"]), data["payments_total"], delta=0.02)
        self.assertEqual(data["truncated"], data["difference_count"] > len(data["rows"]))
        status, _ = self.request("/api/reconciliation", {
            "collection": "Movimentacoes", "start": "2026-09-30", "end": "2026-09-01",
            "date_field": "DataHoraEmissao", "only_valid": True, "only_sales": False,
            "movement_group": "all_sales", "limit": 20,
        })
        self.assertEqual(status, 400)


    def test_stock_expiry_matches_independent_lot_balances(self):
        companies = self.assert_ok('/api/workspace/stock/companies')['companies']
        if not companies:
            self.skipTest('No product companies available')
        company = companies[0]['id']
        payload = {'company_id': company, 'reference_date':'2026-10-27', 'horizon_days':366,
                   'include_expired':True, 'positive_only':False, 'page_size':2}
        result = self.assert_ok('/api/workspace/stock/expiry', payload)
        from bson import ObjectId
        from decimal import Decimal
        stocks = {s['_id']:s for s in self.db.Estoques.find({'_t':'EstoqueLote'}, {'Itens':1,'Quantidades':1})}
        links = self.db.ProdutosServicosEmpresa.find({'EmpresaReferencia':ObjectId(company), '_t':'ProdutoEmpresa', 'EstoqueReferencia':{'$in':list(stocks)}}, {'ProdutoServicoReferencia':1,'EstoqueReferencia':1})
        expected = []
        for link in links:
            product = self.db.ProdutosServicos.find_one({'_id':link['ProdutoServicoReferencia'], 'Ativo':True,'_t':'Produto'}, {'CodigoInterno':1})
            if not product:
                continue
            source = stocks[link['EstoqueReferencia']]
            for lot in source.get('Itens',[]):
                if lot['DataValidade'].date() > date(2027,10,28):
                    continue
                balance = sum(Decimal(str(q['Quantidade'])) for q in source.get('Quantidades',[]) if q['EstoqueReferencia']==lot['_id'])
                expected.append((lot['DataValidade'].date().isoformat(),str(product.get('CodigoInterno') or ''),str(lot.get('Numero') or ''),float(balance)))
        self.assertEqual(result['result']['total_lots'],len(expected))
        for row in result['result']['rows']:
            self.assertIn((row['Validade'],row['Codigo'],row['Lote'],row['Quantidade']),expected)
        self.assertLessEqual(len(result['result']['rows']),2)
        status,_ = self.request('/api/workspace/exports',{'run_id':result['run_id'],'format':'csv','complete':True})
        self.assertEqual(status,422)
        status,_ = self.request('/api/workspace/stock/expiry',{**payload,'company_id':'$where'})
        self.assertEqual(status,422)
        self.assertEqual(self.assert_ok('/api/workspace/stock/expiry',{**payload,'company_id':str(ObjectId())})['result']['total_lots'],0)


class AuthE2E(MongoIntegration):
    """A second real server with Basic Auth enabled, to prove the middleware."""

    USER = "e2e"
    PASSWORD = "e2e-secret"

    @classmethod
    def setUpClass(cls):
        super().setUpClass()
        with socket.socket() as sock:
            sock.bind(("127.0.0.1", 0))
            cls.port = sock.getsockname()[1]
        descriptor, path = tempfile.mkstemp(suffix=".log")
        os.close(descriptor)
        cls.log_path = Path(path)
        cls.log = open(cls.log_path, "w", encoding="utf-8")
        environment = os.environ.copy()
        environment.update({
            "PYTHONIOENCODING": "utf-8", "INSIGHT_HOST": "127.0.0.1",
            "INSIGHT_USER": cls.USER, "INSIGHT_PASSWORD": cls.PASSWORD,
        })
        cls.process = subprocess.Popen(
            [sys.executable, "-m", "uvicorn", "app:app", "--host", "127.0.0.1",
             "--port", str(cls.port), "--log-level", "warning"],
            cwd=str(ROOT), env=environment, stdout=cls.log, stderr=subprocess.STDOUT)
        cls.base = f"http://127.0.0.1:{cls.port}"
        deadline = time.time() + 30
        while time.time() < deadline:
            try:
                with urllib.request.urlopen(cls.base + "/", timeout=2) as response:
                    if response.status == 200:
                        return
            except Exception:
                time.sleep(0.25)
        cls.process.terminate()
        cls.process.wait(timeout=10)
        cls.log.close()
        tail = cls.log_path.read_text(encoding="utf-8", errors="replace")[-2000:]
        raise RuntimeError(f"auth uvicorn did not start on {cls.base}\n{tail}")

    @classmethod
    def tearDownClass(cls):
        cls.process.terminate()
        try:
            cls.process.wait(timeout=10)
        except subprocess.TimeoutExpired:
            cls.process.kill()
            cls.process.wait(timeout=5)
        cls.log.close()
        try:
            cls.log_path.unlink(missing_ok=True)
        except PermissionError:
            pass
        super().tearDownClass()

    def call(self, path: str, body: dict | None = None, authenticated: bool = False):
        data = json.dumps(body).encode() if body is not None else None
        headers = {"Content-Type": "application/json"} if data is not None else {}
        if authenticated:
            token = base64.b64encode(f"{self.USER}:{self.PASSWORD}".encode()).decode()
            headers["Authorization"] = f"Basic {token}"
        request = urllib.request.Request(self.base + path, data=data,
                                         method="POST" if data is not None else "GET",
                                         headers=headers)
        try:
            with urllib.request.urlopen(request, timeout=60) as response:
                raw = response.read().decode("utf-8", errors="replace")
                try:
                    return response.status, json.loads(raw)
                except json.JSONDecodeError:
                    return response.status, {"text": raw}
        except urllib.error.HTTPError as error:
            raw = error.read().decode("utf-8", errors="replace")
            error.close()
            try:
                return error.code, json.loads(raw)
            except json.JSONDecodeError:
                return error.code, {"detail": raw}

    def test_basic_auth_protects_only_the_api(self):
        status, _ = self.call("/api/status")
        self.assertEqual(status, 401)
        status, data = self.call("/api/status", authenticated=True)
        self.assertEqual(status, 200)
        self.assertTrue(data["connected"])
        status, _ = self.call("/")
        self.assertEqual(status, 200)
        status, _ = self.call("/api/report", payload("revenue"))
        self.assertEqual(status, 401)
        status, data = self.call("/api/report", payload("revenue"), authenticated=True)
        self.assertEqual(status, 200)
        self.assertTrue(data["read_only"])


class DiagnosticToolE2E(unittest.TestCase):
    """Runs the diagnostic CLI as a real subprocess, with UTF-8 output."""

    @classmethod
    def setUpClass(cls):
        if not mongo_available():
            raise unittest.SkipTest("MongoDB is not available for the CLI E2E tests")
        cls.environment = os.environ.copy()
        cls.environment["PYTHONIOENCODING"] = "utf-8"
        cls._year_output = None

    def year_output(self) -> subprocess.CompletedProcess:
        cls = type(self)
        if cls._year_output is None:
            cls._year_output = self.run_tool("--year", "2026")
        return cls._year_output

    def run_tool(self, *arguments: str) -> subprocess.CompletedProcess:
        return subprocess.run(
            [sys.executable, str(TOOL), *arguments], cwd=str(ROOT), env=self.environment,
            capture_output=True, text=True, encoding="utf-8", errors="replace", timeout=180,
        )

    def test_help_lists_every_parameter(self):
        result = self.run_tool("--help")
        self.assertEqual(result.returncode, 0)
        for flag in ("--year", "--company", "--date", "--seller", "--seller-ref", "--explain"):
            self.assertIn(flag, result.stdout)

    def test_explain_documents_both_report_bases_and_every_parameter(self):
        result = self.run_tool("--explain")
        self.assertEqual(result.returncode, 0)
        for text in ("TotalLiquido", "documento inteiro", "somente os itens",
                     "--year", "--company", "--date", "--seller", "--seller-ref", "--explain",
                     "documento", "VENDEDORA", "outros", "pago"):
            self.assertIn(text, result.stdout)

    @staticmethod
    def total_from(output: str) -> float:
        line = [line for line in output.splitlines() if line.startswith("TOTAL")][0]
        return float(line.split()[-1])

    def test_year_reproduces_the_monthly_report(self):
        result = self.year_output()
        self.assertEqual(result.returncode, 0, result.stderr)
        self.assertIn("Monthly totals 2026", result.stdout)
        self.assertIn("VENDEDORA 2", result.stdout)
        self.assertIn("TOTAL", result.stdout)
        client = app.mongo_client()
        try:
            match = {
                "DataHoraEmissao": {"$gte": datetime(2026, 1, 1), "$lt": datetime(2027, 1, 1)},
                "_t": "NotaFiscalConsumidorEletronica",
                "Situacao.Descricao": "Concluído",
                "FinalidadeEmissaoNfe.Codigo": 1,
            }
            expected = round(sum(float(row["total"] or 0)
                                 for row in item_rows(client[app.MONGO_DATABASE], match)), 2)
        finally:
            client.close()
        self.assertAlmostEqual(self.total_from(result.stdout), expected, delta=1.0)

    def test_company_parameter_filters_documents(self):
        client = app.mongo_client()
        try:
            companies = client[app.MONGO_DATABASE]["Movimentacoes"].distinct("Empresa.Nome")
        finally:
            client.close()
        if not companies:
            self.skipTest("No company value available")
        unfiltered = self.run_tool("--date", "2026-10-05", "--seller-ref", SELLER2)
        filtered = self.run_tool("--date", "2026-10-05", "--seller-ref", SELLER2,
                                 "--company", companies[0])
        self.assertEqual(filtered.returncode, 0, filtered.stderr)
        self.assertIn("difference:", filtered.stdout)
        empty = self.run_tool("--date", "2026-10-05", "--seller-ref", SELLER2,
                              "--company", "__NO_SUCH_COMPANY__")
        self.assertEqual(empty.returncode, 0, empty.stderr)
        self.assertIn("difference: 0.00", empty.stdout)
        self.assertEqual(unfiltered.returncode, 0, unfiltered.stderr)

    def test_shared_documents_reproduce_the_oct_5_case(self):
        result = self.run_tool("--date", "2026-10-05", "--seller-ref", SELLER2)
        self.assertEqual(result.returncode, 0, result.stderr)
        for text in ("001/217242", "001/217322", "64.98", "39.98",
                     "2145.11", "2040.15", "104.96"):
            self.assertIn(text, result.stdout)

    def test_seller_name_resolves_to_the_reference(self):
        result = self.run_tool("--date", "2026-10-05", "--seller", "VENDEDORA 2")
        self.assertEqual(result.returncode, 0, result.stderr)
        self.assertIn(SELLER2, result.stdout)
        self.assertIn("104.96", result.stdout)

    def test_invalid_inputs_are_explained(self):
        missing = self.run_tool("--date", "2026-10-05")
        self.assertEqual(missing.returncode, 2)
        self.assertIn("seller", missing.stderr.lower())
        bad_date = self.run_tool("--date", "05/10/2026", "--seller-ref", SELLER2)
        self.assertEqual(bad_date.returncode, 2)
        bad_ref = self.run_tool("--date", "2026-10-05", "--seller-ref", "000000000000000000000000")
        self.assertNotEqual(bad_ref.returncode, 0)
        self.assertIn("not found", (bad_ref.stdout + bad_ref.stderr).lower())
        no_args = self.run_tool()
        self.assertEqual(no_args.returncode, 2)


class ToolUnitTests(unittest.TestCase):
    """Pure functions of the diagnostic tool, without MongoDB."""

    @classmethod
    def setUpClass(cls):
        spec = importlib.util.spec_from_file_location("diagnostico_relatorios", TOOL)
        cls.tool = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(cls.tool)

    def test_round2_uses_half_away_from_zero(self):
        for value, expected in ((1.005, 1.01), (-1.005, -1.01), (2.675, 2.68),
                                (24.99, 24.99), (0.004, 0.0), (-0.004, 0.0)):
            self.assertAlmostEqual(self.tool.round2(value), expected, places=2)

    def test_item_total_uses_every_formula_component(self):
        item = {
            "Quantidade": 2, "PrecoUnitario": 10,
            "DescontoDigitado": 1, "DescontoProporcional": 0.5,
            "OutrasDespesasProporcional": 2, "OutrasDespesasDigitado": 0,
            "Percentual": {"ValorSt": 1}, "IPI": {"Valor": 0.5},
            "Frete": 1, "Seguro": 0.25,
        }
        self.assertAlmostEqual(self.tool.item_total(item), 23.25, places=2)

    def test_item_total_ignores_missing_optional_fields(self):
        self.assertAlmostEqual(self.tool.item_total({"Quantidade": 1, "PrecoUnitario": 24.99}), 24.99, places=2)


class DocumentationTests(unittest.TestCase):
    """Keeps the parameter reference complete as the models change."""

    def test_every_api_parameter_is_documented(self):
        text = (ROOT / "docs" / "testes-e2e-relatorios.md").read_text(encoding="utf-8")
        for model in (app.ReportRequest, app.AskRequest, app.ReconciliationRequest):
            for name in model.model_fields:
                self.assertIn(f"`{name}`", text, f"{model.__name__}.{name} is not documented")

    def test_every_cli_parameter_is_documented(self):
        text = (ROOT / "docs" / "testes-e2e-relatorios.md").read_text(encoding="utf-8")
        for flag in ("--year", "--company", "--date", "--seller", "--seller-ref", "--explain"):
            self.assertIn(flag, text)


if __name__ == "__main__":
    unittest.main(verbosity=2)
