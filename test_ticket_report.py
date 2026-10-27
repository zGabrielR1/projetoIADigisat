"""Offline tests for the ticket-médio report. No database or AI calls."""
import unittest
from unittest.mock import patch

import app


class TicketSummaryTests(unittest.TestCase):
    def test_daily_and_period_ticket(self):
        summary = app.summarize_ticket([
            {"_id": "2026-09-01", "documents": 4, "total": 100.0},
            {"_id": "2026-09-02", "documents": 2, "total": 60.0},
        ])
        self.assertEqual(summary["rows"][0]["ticket"], 25.0)
        self.assertEqual(summary["rows"][1]["ticket"], 30.0)
        self.assertEqual(summary["total"], 160.0)
        self.assertEqual(summary["documents"], 6)
        self.assertEqual(summary["ticket"], 26.67)

    def test_zero_documents_does_not_divide(self):
        summary = app.summarize_ticket([{"_id": "2026-09-01", "documents": 0, "total": 0.0}])
        self.assertEqual(summary["rows"][0]["ticket"], 0.0)
        self.assertEqual(summary["ticket"], 0.0)


class DocumentTotalExpressionTests(unittest.TestCase):
    def test_computed_item_total_sums_the_array(self):
        expression = app.document_total_expression("@item_total")
        self.assertIn("$sum", expression)
        self.assertIn("$map", expression["$sum"])

    def test_array_field_is_summed(self):
        expression = app.document_total_expression("ItensBase[].TotalLiquido")
        self.assertEqual(expression, {"$sum": {"$ifNull": ["$ItensBase.TotalLiquido", []]}})

    def test_scalar_field_is_used_directly(self):
        self.assertEqual(app.document_total_expression("Total"), "$Total")


class TicketClassificationTests(unittest.TestCase):
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

    def test_ticket_keyword(self):
        self.assertEqual(self.classify("qual o ticket médio deste mês?"), "ticket")

    def test_average_per_document(self):
        self.assertEqual(self.classify("média por nota no mês passado"), "ticket")

    def test_ticket_wins_over_product_words(self):
        self.assertEqual(self.classify("ticket médio dos produtos"), "ticket")

    def test_ticket_is_an_allowed_kind(self):
        request = app.ReportRequest(
            collection="Movimentacoes", kind="ticket",
            start="2026-09-01", end="2026-09-30", date_field="DataHoraEmissao",
        )
        self.assertEqual(request.kind, "ticket")


if __name__ == "__main__":
    unittest.main()
