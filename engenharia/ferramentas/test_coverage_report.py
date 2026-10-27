import hashlib
import json
from pathlib import Path
import tempfile
import unittest

import coverage_report


class CoverageReportTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory()
        self.addCleanup(self.temp.cleanup)
        self.root = Path(self.temp.name)
        (self.root / "docs").mkdir()
        (self.root / "engenharia/ferramentas").mkdir(parents=True)
        (self.root / "docs/catalogo-relatorios.md").write_text(
            "| Module | Title | Class | Methods | File | Coverage |\n"
            "|---|---|---|---|---|---|\n"
            "| A | First | `First` | `Get` | `file` | Entrada catalogada; pending |\n"
            "| A | Second | `Second` | `Get` | `file` | Entrada catalogada; pending |\n"
            "| A | Partial | `Partial` | `Get` | `file` | Rastreado; detalhe pendente |\n",
            encoding="utf-8",
        )
        (self.root / "source.cs").write_bytes(b"retained source")
        (self.root / "docs/trace.md").write_text("Trace", encoding="utf-8")
        self.ledger = {
            "traces": [{"module": "A", "class": "First", "report": "docs/trace.md"}],
            "sources": [{"path": "source.cs", "sha256": hashlib.sha256(b"retained source").hexdigest()}],
        }

    def report(self):
        (self.root / "engenharia/ferramentas/continuation_coverage.json").write_text(
            json.dumps(self.ledger), encoding="utf-8")
        return coverage_report.build_report(self.root)

    def test_overlay_excludes_traced_entries_and_preserves_partial_gap(self):
        result = self.report()
        self.assertEqual(result["catalog_entries"], 3)
        self.assertEqual([x["class"] for x in result["pending_without_continuation"]], ["Second"])
        self.assertEqual([x["class"] for x in result["partial_with_pending_detail"]], ["Partial"])
        self.assertEqual(result["issues"], [])

    def test_changed_source_is_reported(self):
        (self.root / "source.cs").write_bytes(b"changed")
        self.assertIn("Source changed", self.report()["issues"][0])

    def test_missing_source_is_reported(self):
        (self.root / "source.cs").unlink()
        self.assertIn("Missing source", self.report()["issues"][0])

    def test_duplicate_or_unknown_trace_is_reported(self):
        self.ledger["traces"].append(dict(self.ledger["traces"][0]))
        self.ledger["traces"].append({"module": "A", "class": "Unknown", "report": "docs/trace.md"})
        issues = self.report()["issues"]
        self.assertTrue(any("Duplicate" in issue for issue in issues))
        self.assertTrue(any("absent from catalog" in issue for issue in issues))

    def test_evidence_paths_cannot_escape_project(self):
        self.ledger["sources"][0]["path"] = "../outside.cs"
        with self.assertRaises(ValueError):
            self.report()


if __name__ == "__main__":
    unittest.main()
