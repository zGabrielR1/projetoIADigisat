"""Independent serial reference, exact decimals, scope transparency and API wiring."""
import asyncio
from copy import deepcopy
from decimal import Decimal
import json
import unittest
from unittest.mock import patch

from fastapi import FastAPI, HTTPException
from pydantic import ValidationError
import insight_calculation_audit as audit


def item(seller='A', total='1.01', **extra):
    return {'seller_id': seller, 'total_liquido': total, 'cancelled': False,
            'auxiliary_document': False, 'moves_stock': True, 'moves_finance': True, **extra}


def request(documents=None, **extra):
    return audit.MonthlyAudit.model_validate({'year': 2026, 'company_reference': 'fixture',
        'repository_scope_description': 'Explicit preselected normalized test input',
        'documents': documents if documents is not None else [{'document_id': 'one', 'month': 1, 'items': [item()]}], **extra})


class CalculationAuditTests(unittest.TestCase):
    def test_exact_serial_totals_all_months_and_explained_filters(self):
        docs = [{'document_id': str(month), 'month': month, 'items': [
            item(), item('B', '2.02'), item('', '-0.01'),
            item(total='999', cancelled=True), item(total='999', auxiliary_document=True, moves_stock=False),
            item(total='999', auxiliary_document=True, moves_finance=False),
            item(total='0.10', auxiliary_document=True)]} for month in range(1, 13)]
        result = audit.monthly_audit(request(docs))
        rows = {r['seller_id']: r for r in result['rows']}
        self.assertEqual([Decimal(x) for x in rows['A']['months']], [Decimal('1.11')] * 12)
        self.assertEqual(Decimal(rows['A']['year_total']), Decimal('13.32'))
        self.assertEqual(rows['B']['months'], ['2.02'] * 12)
        self.assertEqual(result['counts']['items_included'], 48)
        self.assertEqual(result['counts']['cancelled'], 12)
        self.assertEqual(result['counts']['auxiliary_without_stock'], 12)
        self.assertEqual(result['counts']['auxiliary_without_finance'], 12)
        self.assertEqual(result['explanations'][0]['contributions']['A'], '1.11')

    def test_seller_filter_runs_per_item_not_per_whole_document(self):
        doc = {'document_id': 'shared', 'month': 6, 'items': [item(), item('B', '100'), item('', '200')]}
        result = audit.monthly_audit(request([doc], seller_id='A'))
        self.assertEqual(len(result['rows']), 1)
        self.assertEqual(result['rows'][0]['months'][5], '1.01')
        self.assertEqual(result['counts']['other_seller'], 2)

    def test_input_identity_and_result_independent_of_document_and_item_order(self):
        docs = [{'document_id': str(n), 'month': n % 12 + 1, 'items': [item(), item('B', '-.01')]} for n in range(501)]
        first = audit.monthly_audit(request(docs, explanation_limit=0))
        reordered = deepcopy(docs)[::-1]
        for doc in reordered:
            doc['items'].reverse()
        second = audit.monthly_audit(request(reordered, explanation_limit=10, declared_complete=True))
        self.assertEqual(first['rows'], second['rows'])
        self.assertEqual(first['provenance']['snapshot_sha256'], second['provenance']['snapshot_sha256'])
        self.assertTrue(first['explanations_truncated'])

    def test_native_variation_and_difference_does_not_assert_cause(self):
        observed = [{'label': 'bad', 'rows': [{'seller_id': 'A', 'months': ['1'] + ['0'] * 11}]},
                    {'label': 'same', 'rows': [{'seller_id': 'A', 'months': ['1.01'] + ['0'] * 11}]}]
        result = audit.monthly_audit(request(native_runs=observed))
        self.assertTrue(result['native_runs_vary'])
        self.assertFalse(result['comparisons'][0]['matches_snapshot_reference'])
        self.assertEqual(result['comparisons'][0]['differences'][0]['native_minus_reference'], '-0.01')
        self.assertTrue(result['comparisons'][1]['matches_snapshot_reference'])
        self.assertIn('não comprova', ' '.join(result['findings']))
        self.assertFalse(result['coverage']['remote_build_verified'])

    def test_missing_rows_are_not_converted_to_zero(self):
        result = audit.monthly_audit(request([], native_runs=[{'label': 'zero-row', 'rows': [
            {'seller_id': 'A', 'months': ['0'] * 12}]}]))
        self.assertFalse(result['comparisons'][0]['matches_snapshot_reference'])
        self.assertEqual(result['comparisons'][0]['differences'][0]['kind'], 'missing_in_reference')

    def test_limited_explanations_do_not_hide_failed_comparison(self):
        result = audit.monthly_audit(request(difference_limit=0, native_runs=[{'label': 'none', 'rows': []}]))
        comparison = result['comparisons'][0]
        self.assertFalse(comparison['matches_snapshot_reference'])
        self.assertEqual(comparison['difference_count'], 1)
        self.assertEqual(comparison['differences'], [])
        self.assertTrue(comparison['differences_truncated'])

    def test_scope_and_completeness_are_claims_not_verification(self):
        result = audit.monthly_audit(request(declared_complete=True))
        self.assertTrue(result['coverage']['declared_complete'])
        for key in ['independently_verified_complete', 'raw_bson_mapping_verified', 'remote_build_verified']:
            self.assertFalse(result['coverage'][key])
        self.assertFalse(result['provenance']['production_assembly_executed'])

    def test_duplicate_documents_and_sellers_are_rejected(self):
        doc = {'document_id': 'one', 'month': 1, 'items': [item()]}
        with self.assertRaises(ValidationError):
            request([doc, doc])
        with self.assertRaises(ValidationError):
            request(native_runs=[{'label': 'x', 'rows': [{'seller_id': s, 'months': ['0'] * 12} for s in ['A', 'a']]}])

    def test_invalid_and_overflow_decimals_fail_without_partial_totals(self):
        for value in ['NaN', 'Infinity', str(2**96), '0.' + '0' * 28 + '1']:
            with self.subTest(value=value), self.assertRaises(ValidationError):
                request([{'document_id': 'one', 'month': 1, 'items': [item(total=value)]}])
        big = str(2**96 - 1)
        with self.assertRaises(HTTPException):
            audit.monthly_audit(request([{'document_id': 'one', 'month': 1, 'items': [item(total=big), item(total='1')]}]))

    def test_no_rounding_or_float_loss_of_decimal_cents(self):
        docs = [{'document_id': 'one', 'month': 1, 'items': [item(total='0.1'), item(total='0.2')]}]
        self.assertEqual(audit.monthly_audit(request(docs))['rows'][0]['months'][0], '0.3')

    def test_zero_negative_and_fractional_unit_prices(self):
        result = audit.audit_unit_prices(audit.UnitPriceAudit(rows=[
            {'reference': 'fractional', 'quantity': '.5', 'net_total': '10', 'reported_unit_price': '10'},
            {'reference': 'zero', 'quantity': '0', 'net_total': '10'},
            {'reference': 'negative', 'quantity': '-.5', 'net_total': '-10', 'reported_unit_price': '20'},
            {'reference': 'third', 'quantity': '3', 'net_total': '-1', 'reported_unit_price': '-.33'}]))
        a, b, c, d = result['rows']
        self.assertEqual(a['reference_unit_price'], '20.00')
        self.assertEqual(a['status'], 'different')
        self.assertTrue(a['matches_recovered_defective_formula'])
        self.assertEqual(b['status'], 'undefined_zero_quantity')
        self.assertIsNone(b['reference_unit_price'])
        self.assertEqual(c['status'], 'matches')
        self.assertEqual(d['status'], 'matches')
        self.assertFalse(result['results_modified'])

    def test_http_routes_validate_and_do_not_touch_database(self):
        async def post(payload):
            application = FastAPI()
            application.include_router(audit.router)
            messages = []
            consumed = False
            async def receive():
                nonlocal consumed
                if consumed:
                    await asyncio.Event().wait()
                consumed = True
                return {'type': 'http.request', 'body': json.dumps(payload).encode(), 'more_body': False}
            async def send(message):
                messages.append(message)
            await application({'type': 'http', 'asgi': {'version': '3.0'}, 'http_version': '1.1',
                'method': 'POST', 'scheme': 'http', 'path': '/api/audit/monthly', 'raw_path': b'/api/audit/monthly',
                'query_string': b'', 'root_path': '', 'headers': [(b'content-type', b'application/json')],
                'client': ('127.0.0.1', 1234), 'server': ('127.0.0.1', 80)}, receive, send)
            return messages[0]['status'], json.loads(b''.join(m.get('body', b'') for m in messages[1:]))
        import app
        self.assertIn('/api/audit/monthly', [r.path for r in app.app.routes])
        self.assertIn('/api/audit/unit-prices', [r.path for r in app.app.routes])
        with patch.object(app, 'mongo_client', side_effect=AssertionError('No database access')):
            status, result = asyncio.run(post(request().model_dump(mode='json')))
            self.assertEqual(status, 200)
            self.assertEqual(result['rows'][0]['months'][0], '1.01')
            self.assertEqual(asyncio.run(post({'year': 2026}))[0], 422)


if __name__ == '__main__':
    unittest.main()
