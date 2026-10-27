"""Read-only catalog coverage and retained-source verification (no REA calls)."""
from __future__ import annotations

import argparse
from collections import Counter
import hashlib
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]


def catalog_entries(text):
    entries = []
    for line in text.splitlines():
        cells = [cell.strip() for cell in line.strip().strip("|").split("|")]
        if not line.startswith("|") or len(cells) != 6 or not cells[2].startswith("`"):
            continue
        entries.append({"module": cells[0], "title": cells[1],
                        "class": cells[2].strip("`"), "coverage": cells[5]})
    if not entries:
        raise ValueError("No six-column catalog entries found")
    return entries


def inside_root(root, relative):
    if Path(relative).is_absolute():
        raise ValueError(f"Expected relative evidence path: {relative}")
    path = (root / relative).resolve()
    if not path.is_relative_to(root.resolve()):
        raise ValueError(f"Evidence path outside project: {relative}")
    return path


def build_report(root=ROOT):
    entries = catalog_entries((root / "docs/catalogo-relatorios.md").read_text(encoding="utf-8"))
    ledger = json.loads((root / "engenharia/ferramentas/continuation_coverage.json").read_text(encoding="utf-8"))
    known = {(item["module"], item["class"]) for item in entries}
    seen = set()
    issues = []
    for item in ledger["traces"]:
        key = (item["module"], item["class"])
        if key in seen:
            issues.append(f"Duplicate continuation: {key}")
        seen.add(key)
        if key not in known:
            issues.append(f"Continuation absent from catalog: {key}")
        if not inside_root(root, item["report"]).is_file():
            issues.append(f"Missing report: {item['report']}")
    for source in ledger["sources"]:
        path = inside_root(root, source["path"])
        if not path.is_file():
            issues.append(f"Missing source: {source['path']}")
        elif hashlib.sha256(path.read_bytes()).hexdigest() != source["sha256"]:
            issues.append(f"Source changed since trace: {source['path']}")
    pending = [item for item in entries
               if item["coverage"].startswith("Entrada catalogada;")
               and (item["module"], item["class"]) not in seen]
    return {
        "catalog_entries": len(entries),
        "distinct_types": len({item["class"] for item in entries}),
        "catalog_status_counts": dict(Counter(item["coverage"] for item in entries)),
        "continuation_traces": ledger["traces"],
        "pending_without_continuation": pending,
        "partial_with_pending_detail": [item for item in entries
            if not item["coverage"].startswith("Entrada catalogada;")
            and "pendente" in item["coverage"].casefold()],
        "source_files_checked": len(ledger["sources"]),
        "issues": issues,
        "limitations": "Continuation traces do not promote catalog status. Source hashes identify local decompiled files, not original assemblies or REA Evidence. Runtime behavior remains unverified.",
    }


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--json", action="store_true", help="Print complete machine-readable report")
    parser.add_argument("--pending", type=int, default=10, help="Number of pending entries to display")
    args = parser.parse_args()
    if args.pending < 0:
        parser.error("--pending must be non-negative")
    try:
        result = build_report()
    except (OSError, ValueError, KeyError, TypeError) as exc:
        parser.exit(2, f"Coverage check failed: {exc}\n")
    if args.json:
        print(json.dumps(result, ensure_ascii=False, indent=2))
    else:
        print(f"Catalog: {result['catalog_entries']} entries / {result['distinct_types']} types")
        for status, count in result["catalog_status_counts"].items():
            print(f"  {count}: {status}")
        print(f"Continuation traces: {len(result['continuation_traces'])}")
        print(f"Sources verified: {result['source_files_checked']}")
        print(f"Pending without continuation: {len(result['pending_without_continuation'])}")
        for item in result["pending_without_continuation"][:args.pending]:
            print(f"  {item['module']} / {item['class']}: {item['title']}")
        for issue in result["issues"]:
            print(f"ERROR: {issue}")
        print(result["limitations"])
    return 1 if result["issues"] else 0


if __name__ == "__main__":
    raise SystemExit(main())
