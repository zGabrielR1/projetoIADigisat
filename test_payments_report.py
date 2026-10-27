"""Offline tests for the payment-methods report. No database or AI calls."""
import unittest
from unittest.mock import patch

import app


class PaymentSummaryTests(unittest.TestCase):
    def test_participation_and_totals(self):
        summary = app.summarize_payments(
            by_species=[
                {"_id": {"codigo": 1, "descricao": "Dinheiro"}, "total": 75.0, "items": 3},
                {"_id": {"codigo": 7, "descricao": "Cartão POS"}, "total": 25.0, "items": 1},
            ],
            totals=[{"total": 100.0, "items": 4}],
            by_type=[{"_id": "ItemPagamentoDinheiro", "items": 3},
                     {"_id": "ItemPagamentoCartaoPos", "items": 1}],
            species_count=2,
        )
        self.assertEqual(summary["total"], 100.0)
        self.assertEqual(summary["items"], 4)
        self.assertEqual(summary["rows"][0]["_id"], "Dinheiro")
        self.assertEqual(summary["rows"][0]["participacao"], 75.0)
        self.assertEqual(summary["rows"][1]["participacao"], 25.0)
        self.assertEqual(summary["cards"], 1)
        self.assertFalse(summary["truncated"])

    def test_missing_description_and_truncation(self):
        summary = app.summarize_payments(
            by_species=[{"_id": {"codigo": 99, "descricao": None}, "total": 10.0, "items": 1}],
            totals=[{"total": 10.0, "items": 1}],
            by_type=[],
            species_count=3,
        )
        self.assertEqual(summary["rows"][0]["_id"], "Sem espécie")
        self.assertEqual(summary["cards"], 0)
        self.assertTrue(summary["truncated"])

    def test_zero_total_does_not_divide(self):
        summary = app.summarize_payments(
            by_species=[{"_id": {"codigo": 1, "descricao": "Dinheiro"}, "total": 0.0, "items": 1}],
            totals=[{"total": 0.0, "items": 1}],
            by_type=[],
            species_count=1,
        )
        self.assertEqual(summary["rows"][0]["participacao"], 0.0)


class PaymentClassificationTests(unittest.TestCase):
    def classify(self, question):
        request = app.AskRequest(
            question=question, collection="Movimentacoes",
            start="2026-09-01", end="2026-09-30", date_field="DataHoraEmissao",
        )
        captured = {}

        def report(payload):
            captured["kind"] = payload.kind
            return {"period": {"start": "2026-09-01", "end": "2026-09-30"}}

        with (
            patch.object(app, "AI_ENDPOINT", ""),
            patch.object(app, "MONGO_DATABASE", "test"),
            patch.object(app, "run_report", side_effect=report),
        ):
            app.ask(request)
        return captured["kind"]

    def test_explicit_payment_phrase(self):
        self.assertEqual(self.classify("Quais as formas de pagamento deste mês?"), "payments")

    def test_species_keyword(self):
        self.assertEqual(self.classify("total por espécie de pagamento"), "payments")

    def test_cash_keyword(self):
        self.assertEqual(self.classify("quanto entrou em dinheiro?"), "payments")

    def test_documents_keep_priority_for_payment_mention(self):
        self.assertEqual(self.classify("documentos sem pagamento em setembro"), "documents")

    def test_unrelated_question_is_not_reclassified(self):
        self.assertEqual(self.classify("quais os produtos mais vendidos?"), "top_products")


class PaymentRequestValidationTests(unittest.TestCase):
    def test_payments_is_an_allowed_kind(self):
        request = app.ReportRequest(
            collection="Movimentacoes", kind="payments",
            start="2026-09-01", end="2026-09-30", date_field="DataHoraEmissao",
        )
        self.assertEqual(request.kind, "payments")

    def test_unknown_kind_is_rejected(self):
        with self.assertRaises(Exception):
            app.ReportRequest(
                collection="Movimentacoes", kind="payment_methods",
                start="2026-09-01", end="2026-09-30", date_field="DataHoraEmissao",
            )


if __name__ == "__main__":
    unittest.main()
