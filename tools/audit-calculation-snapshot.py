"""Offline CLI for the same independent audits exposed by /api/audit.

python tools/audit-calculation-snapshot.py monthly snapshot.json
python tools/audit-calculation-snapshot.py unit-prices prices.json
"""
from pathlib import Path
import json
import sys

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from insight_calculation_audit import MonthlyAudit, UnitPriceAudit, monthly_audit, audit_unit_prices

mode, path = sys.argv[1:]
payload = json.loads(Path(path).read_text(encoding='utf-8'), parse_float=str)
if mode == 'monthly':
    result = monthly_audit(MonthlyAudit.model_validate(payload))
elif mode == 'unit-prices':
    result = audit_unit_prices(UnitPriceAudit.model_validate(payload))
else:
    raise SystemExit('Choose monthly or unit-prices')
print(json.dumps(result, ensure_ascii=False, indent=2))
