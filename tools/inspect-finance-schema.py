"""Record field names/types and counts only; never export finance documents."""
from collections import Counter, defaultdict
import json
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT))
import app


def shape(value, path='', found=None):
    found = found if found is not None else defaultdict(Counter)
    if path:
        found[path][type(value).__name__] += 1
    if isinstance(value, dict):
        for key, child in value.items():
            shape(child, f'{path}.{key}' if path else key, found)
    elif isinstance(value, list):
        for child in value[:5]:
            shape(child, path + '[]', found)
    return found


def main():
    result = {'evidence_type': 'read_only_field_shape_sample', 'collections': []}
    with app.mongo_client() as client:
        db = client[app.MONGO_DATABASE]
        known = set(db.list_collection_names())
        for name in ['Pagamentos', 'Recebimentos', 'Pessoas', 'Contas', 'PlanosConta', 'CentrosCusto']:
            fields = defaultdict(Counter)
            sampled = 0
            if name not in known:
                result['collections'].append({'name': name, 'exists': False})
                continue
            with db[name].find({}).limit(30).max_time_ms(20000) as cursor:
                for doc in cursor:
                    shape(doc, found=fields)
                    sampled += 1
            result['collections'].append({'name': name, 'exists': True, 'sampled': sampled,
                                          'fields': {k: dict(v) for k,v in sorted(fields.items())}})
    out = ROOT / 'docs/finance-schema-1.0.3-rc.1.json'
    out.write_text(json.dumps(result, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print(json.dumps([{'name': c['name'], 'exists': c['exists'], 'sampled': c.get('sampled', 0),
                      'fields': len(c.get('fields', {}))} for c in result['collections']]))


if __name__ == '__main__':
    main()
