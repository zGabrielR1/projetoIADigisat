"""Offline regression tests. Use --prepared to validate the unapplied patch in memory."""
import sys
import unittest
from pathlib import Path
from unittest.mock import patch

import app


class CompanyInterpretationTests(unittest.TestCase):
    def check_company(self, selected, detected, expected, lookup_error=False):
        request = app.AskRequest(
            question="faturamento", collection="Movimentacoes",
            start="2026-10-01", end="2026-10-21",
            date_field="DataHoraEmissao", company_field="EmpresaReferencia",
            company_value=selected,
        )

        def report(payload):
            self.assertEqual(payload.company_value, expected)
            return {"period": {"start": "2026-10-01", "end": "2026-10-21"}}

        with (
            patch.object(app, "AI_ENDPOINT", ""),
            patch.object(app, "MONGO_DATABASE", "test"),
            patch.object(app, "mongo_client") as client,
            patch.object(app, "resolve_company", return_value=detected),
            patch.object(app, "run_report", side_effect=report),
        ):
            client.return_value.__enter__.return_value.__getitem__.return_value.list_collection_names.return_value = ["Movimentacoes"]
            if lookup_error:
                client.side_effect = app.HTTPException(503, "Unavailable")
            result = app.ask(request)

        self.assertEqual(result["interpretation"]["company"], expected)
        self.assertIn("Empresa: " + (expected or "todas"), result["interpretation"]["summary"])

    def test_manual_company(self):
        self.check_company("Empresa A", None, "Empresa A")

    def test_detected_company_overrides_manual(self):
        self.check_company("Empresa A", "Empresa B", "Empresa B")

    def test_all_companies(self):
        self.check_company(None, None, None)

    def test_lookup_failure_preserves_manual_company(self):
        self.check_company("Empresa A", None, "Empresa A", lookup_error=True)


if __name__ == "__main__":
    if "--prepared" in sys.argv:
        sys.argv.remove("--prepared")
        source = Path(app.__file__).read_text(encoding="utf-8")
        original = '    summary.append(f"Empresa: {matched_company}" if matched_company else "Empresa: todas")'
        if original in source:
            source = source.replace(original, '    effective_company = payload.get("company_value")\n    summary.append(f"Empresa: {effective_company}" if effective_company else "Empresa: todas")')
            source = source.replace('"period_source": period_source, "company": matched_company,', '"period_source": period_source, "company": effective_company,')
            exec(compile(source, app.__file__, "exec"), app.__dict__)
        print("Validating prepared correction IN MEMORY; application file unchanged.")
    unittest.main()
