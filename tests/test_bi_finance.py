import unittest
from datetime import date, timedelta
from decimal import Decimal
from unittest.mock import patch

from fastapi import HTTPException
from insight_bi_finance import FinancialScope, ParcelScope, analyze_rows, overview, parcels


class FinancialBITests(unittest.TestCase):
    def row(self, days, amount='10.005', person='one'):
        return {'due_at': (date(2026, 10, 29)-timedelta(days=days)).isoformat(),
                'corrected': Decimal(amount), 'person_id': person, 'person': 'Same name'}

    def test_bands_reconcile_displayed_cents_and_homonyms(self):
        rows=[self.row(days, person=str(i)) for i,days in enumerate([-1,0,1,7,8,30,31,60,61,90,91])]
        result=analyze_rows(rows,date(2026,10,29))
        self.assertEqual(result['total'],Decimal('110.11'))
        self.assertEqual([x['count'] for x in result['aging']],[1,1,2,2,2,2,1])
        self.assertEqual(sum(x['amount'] for x in result['aging']),result['total'])
        self.assertEqual(result['overdue'],Decimal('90.09'))
        self.assertEqual(len(result['people']),8)
        self.assertEqual(len({p['id'] for p in result['people']}),8)

    def test_empty_is_zero_and_missing_date_fails(self):
        self.assertEqual(analyze_rows([],date.today())['total'],0)
        row=self.row(0);row['due_at']=None
        with self.assertRaises(HTTPException):analyze_rows([row],date.today())

    def test_invalid_scope_rejected_before_read(self):
        for values in [dict(start='2026-10-30',end='2026-10-01'),dict(start='2026-10-01',end='2026-10-30',company_id='bad')]:
            with patch('insight_bi_finance.execute') as run:
                with self.assertRaises(HTTPException) as raised:overview(FinancialScope(**values))
                self.assertEqual(raised.exception.status_code,422);run.assert_not_called()

    def test_partial_source_never_becomes_zero(self):
        with patch('insight_bi_finance.execute',side_effect=[{'analysis':{'total':'10.00'},'provenance':{}},HTTPException(503,'Unavailable')]) as run:
            result=overview(FinancialScope(start='2026-10-01',end='2026-10-30'))
            self.assertEqual(result['sides']['receivable']['analysis']['total'],'10.00')
            self.assertEqual(result['sides']['payable'],{'error':'Unavailable'})
            self.assertTrue(run.call_args_list[0].kwargs['include_analysis'])

    def test_parcel_filters_reach_engine(self):
        with patch('insight_bi_finance.execute',return_value={}) as run:
            parcels(ParcelScope(start='2026-10-01',end='2026-10-30',side='receivable',person_id='000000000000000000000001',offset=50))
            req=run.call_args.args[0]
            self.assertEqual(req.offset,50);self.assertEqual(req.status,'pending')
            self.assertEqual(run.call_args.kwargs['analysis_person_id'],'000000000000000000000001')
