"""Question interpretation and independent checks of the sales investigation."""
import math
import json
import unittest
from datetime import date, datetime, timedelta
from unittest.mock import patch, MagicMock

from fastapi import HTTPException
import app
from report_questions import question_details, resolve_seller

EXAMPLE = "Existe uma diferença aparecendo no dia 5/10 no vendedor 2 de cerca de R$100,00 que não consegui descobrir."


class QuestionTests(unittest.TestCase):
    def test_nuanced_discrepancy_extracts_entities(self):
        self.assertEqual(question_details(EXAMPLE), {"investigate": True, "seller_query": "2", "target_amount": 100})

    def test_varied_intents(self):
        for text in ["Os valores não batem ontem", "Está faltando 100 reais no dia 5/10",
                     "Quero conferir os recebimentos", "Por que o fechamento não fecha?",
                     "Investigue vendas compartilhadas entre vendedores", "Há uma divergência no faturamento"]:
            with self.subTest(text=text):
                self.assertTrue(question_details(text)["investigate"])

    def test_seller_names_and_aliases(self):
        for text, expected in [('vendedora "Ana Paula" em setembro', "ana paula"),
                               ("vendedor 2 no dia 5/10", "2"), ("V2 em outubro", "2"),
                               ("vendedor numero 12 ontem", "12"), ("vendedora Ana Paula no dia 5/10", "ana paula")]:
            with self.subTest(text=text):
                self.assertEqual(question_details(text)["seller_query"], expected)

    def test_currency_is_reference_not_date_or_seller(self):
        self.assertEqual(question_details("diferença de R$ 1.234,56 no vendedor 2 em 5/10")["target_amount"], 1234.56)
        self.assertIsNone(question_details("vendedor 2 no dia 5/10")["target_amount"])

    def test_existing_reports_do_not_become_investigations(self):
        for text in ["faturamento deste mês", "produtos mais vendidos", "ticket médio", "formas de pagamento", "vendas por dia"]:
            self.assertFalse(question_details(text)["investigate"])

    def test_written_day_and_numeric_day(self):
        for text in [EXAMPLE, "Não bate no dia 5 de outubro de 2026"]:
            self.assertEqual(app.parse_period(text, date(2026, 10, 26)), (date(2026, 10, 5), date(2026, 10, 5)))

    def test_invalid_date_is_not_silently_ignored(self):
        for text in ["diferença em 31/02", "dia 31 de fevereiro de 2026"]:
            with self.assertRaises(HTTPException):
                app.parse_period(text, date(2026, 10, 26))

    def test_deterministic_investigation_overrides_generic_model(self):
        req = app.AskRequest(question=EXAMPLE, collection="Movimentacoes", start="2026-10-01", end="2026-10-26",
                             date_field="DataHoraEmissao", company_field=None)
        result = {"interpretation": {}}
        with patch.object(app, "AI_ENDPOINT", ""), patch.object(app, "investigation", return_value=result) as run:
            app.ask(req)
        parsed = run.call_args.args[0]
        self.assertEqual(parsed.seller_query, "2")
        self.assertEqual(parsed.start, date(2026, 10, 5))
        self.assertEqual(parsed.target_amount, 100)

    def test_ambiguous_seller_requires_clarification(self):
        db = MagicMock()
        people = [{"_id": "a", "Nome": "VENDEDORA 2 ANA"}, {"_id": "b", "Nome": "VENDEDORA 2 MARIA"}]
        db.Pessoas.find.return_value.max_time_ms.return_value.limit.return_value = people
        db.Movimentacoes.count_documents.return_value = 1
        with self.assertRaises(HTTPException) as caught:
            resolve_seller(db, "2", {})
        self.assertIn("ambíguo", caught.exception.detail)

    def test_invalid_model_shape_falls_back_to_local_report(self):
        req = app.AskRequest(question="ticket médio em 5/10", collection="Movimentacoes", start="2026-10-01",
                             end="2026-10-26", date_field="DataHoraEmissao")
        response = MagicMock()
        response.__enter__.return_value.read.return_value = json.dumps(
            {"choices": [{"message": {"content": "[]"}}]}).encode()
        with patch.object(app, "AI_ENDPOINT", "http://example.invalid"), patch.object(app, "AI_MODEL", "test"), \
             patch.object(app.urllib.request, "urlopen", return_value=response), \
             patch.object(app, "run_report", return_value={"period": {"start": "2026-10-05", "end": "2026-10-05"}}) as run:
            result = app.ask(req)
        self.assertEqual(run.call_args.args[0].kind, "ticket")
        self.assertEqual(result["interpretation"]["method"], "regras locais")

    def test_seller_is_resolved_by_participation_not_arbitrary_first(self):
        db = MagicMock()
        people = [{"_id": "a", "Nome": "VENDEDORA 2 ANA"}, {"_id": "b", "Nome": "VENDEDORA 2 MARIA"}]
        db.Pessoas.find.return_value.max_time_ms.return_value.limit.return_value = people
        db.Movimentacoes.count_documents.side_effect = [0, 2]
        self.assertEqual(resolve_seller(db, "2", {})["ref"], "b")


class InvestigationIntegration(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        try:
            cls.client = app.mongo_client()
            cls.db = cls.client[app.MONGO_DATABASE]
            cls.client.admin.command("ping")
        except Exception as exc:
            raise unittest.SkipTest(f"Local MongoDB unavailable: {type(exc).__name__}")

    @classmethod
    def tearDownClass(cls):
        cls.client.close()

    def ask(self, limit=500, question=EXAMPLE):
        with patch.object(app, "AI_ENDPOINT", ""):
            return app.ask(app.AskRequest(question=question, collection="Movimentacoes", start="2026-10-01",
                                         end="2026-10-26", date_field="DataHoraEmissao", value_field="@item_total",
                                         movement_group="all_sales", limit=limit))

    def test_all_sales_and_shared_document_difference_independently(self):
        result = self.ask()
        data = result["investigation"]
        ref = data["seller_reference"]
        start = datetime(2026, 10, 5)
        docs = list(self.db.Movimentacoes.find({"DataHoraEmissao": {"$gte": start, "$lt": start + timedelta(days=1)},
                    "_t": {"$in": app.MOVEMENT_GROUPS["all_sales"]}, "Situacao.Descricao": {"$nin": app.STATUS_EXCLUDED}}))
        def total(item):
            def f(*path):
                v = item
                for key in path:
                    v = v.get(key, {}) if isinstance(v, dict) else {}
                return float(v or 0)
            gross = f("Quantidade") * f("PrecoUnitario")
            gross = math.trunc(gross * 100 + (0.500000001 if gross >= 0 else -0.500000001)) / 100
            return gross - f("DescontoDigitado") - f("DescontoProporcional") + f("OutrasDespesasDigitado") + f("OutrasDespesasProporcional") + f("Percentual", "ValorSt") + f("IPI", "Valor") + f("Frete") + f("Seguro")
        document_total = seller_total = 0
        count = 0
        for doc in docs:
            items = [i for i in doc.get("ItensBase", []) if not i.get("Cancelado")]
            mine = [i for i in items if str(i.get("VendedorReferencia", "")) == ref]
            if not mine:
                continue
            count += 1
            document_total += sum(total(i) for i in items)
            seller_total += sum(total(i) for i in mine)
        self.assertEqual(result["documents"], count)
        self.assertEqual(len(result["rows"]), count)
        self.assertAlmostEqual(data["totals"]["Itens"], document_total, places=2)
        self.assertAlmostEqual(data["totals"]["Vendedor"], seller_total, places=2)
        self.assertAlmostEqual(data["totals"]["Outros"], document_total - seller_total, places=2)
        self.assertAlmostEqual(data["totals"]["Outros"], 104.96, places=2)
        self.assertEqual(data["totals"]["Diferenca"], 0)
        self.assertTrue(any(row["Outros"] > 0 for row in result["rows"]))
        self.assertTrue(all(len(doc["items"]) > 0 for doc in data["details"]))

    def test_limit_does_not_change_totals_and_does_not_filter_to_target(self):
        full = self.ask()
        limited = self.ask(limit=1)
        self.assertEqual(limited["investigation"]["totals"], full["investigation"]["totals"])
        self.assertEqual(len(limited["rows"]), 1)
        self.assertTrue(limited["investigation"]["truncated"])
        self.assertGreater(full["documents"], 1)

    def test_unknown_seller_does_not_return_everyones_sales(self):
        with self.assertRaises(HTTPException) as caught:
            self.ask(question='diferença no vendedor "NaoExisteXYZ" em 5/10')
        self.assertEqual(caught.exception.status_code, 422)

    def test_without_seller_includes_unpaid_sales(self):
        result = self.ask(question="As vendas não batem com pagamentos em 5/10, diferença de R$ 250,00")
        self.assertIsNone(result["investigation"]["seller"])
        self.assertAlmostEqual(result["investigation"]["totals"]["Diferenca"], 249.26, places=2)


if __name__ == "__main__":
    unittest.main()
