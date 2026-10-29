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

    def test_drill_windows_cover_every_band_boundary(self):
        reference=date(2026,10,29)
        cases={'upcoming':('2026-10-30','2026-12-31'),'today':('2026-10-29','2026-10-29'),
               '1_7':('2026-10-22','2026-10-28'),'8_30':('2026-09-29','2026-10-21'),
               '31_60':('2026-08-30','2026-09-28'),'61_90':('2026-07-31','2026-08-29'),
               '90_plus':('2026-01-01','2026-07-30')}
        for band,(start,end) in cases.items():
            with self.subTest(band=band),patch('insight_bi_finance.date') as clock,patch('insight_bi_finance.execute',return_value={'rows':[{'corrected':'10.01'},{'corrected':'20.02'}]}) as run:
                clock.today.return_value=reference
                result=parcels(ParcelScope(start='2026-01-01',end='2026-12-31',side='receivable',band=band,reference_date=reference,offset=50))
                query=run.call_args.args[0]
                self.assertEqual((query.start.isoformat(),query.end.isoformat()),(start,end))
                self.assertEqual(query.offset,50)
                self.assertEqual(result['page_total'],'30.03')
                self.assertTrue(run.call_args.kwargs['include_analysis'])

    def test_drill_keeps_original_date_limits(self):
        with patch('insight_bi_finance.date') as clock,patch('insight_bi_finance.execute',return_value={}) as run:
            clock.today.return_value=date(2026,10,29)
            parcels(ParcelScope(start='2026-10-01',end='2026-10-15',side='payable',band='8_30',account_id='000000000000000000000002'))
            query=run.call_args.args[0]
            self.assertEqual((query.start,query.end),(date(2026,10,1),date(2026,10,15)))
            self.assertEqual(query.account_id,'000000000000000000000002')

    def test_agenda_drill_selects_one_date(self):
        with patch('insight_bi_finance.execute',return_value={}) as run:
            result=parcels(ParcelScope(start='2026-10-01',end='2026-11-30',side='receivable',due_date='2026-11-13'))
            query=run.call_args.args[0]
            self.assertEqual(query.start,query.end)
            self.assertEqual(result['drill_scope']['due_date'],'2026-11-13')

    def test_stale_or_impossible_drill_does_not_query(self):
        cases=[{'reference_date':'2000-01-01'},{'due_date':'2025-01-01'}, {'due_date':'2026-10-01','band':'1_7'}]
        for extra in cases:
            with self.subTest(extra=extra),patch('insight_bi_finance.execute') as run:
                with self.assertRaises(HTTPException):
                    parcels(ParcelScope(start='2026-10-01',end='2026-10-30',side='receivable',**extra))
                run.assert_not_called()
