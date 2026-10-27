"""Read-only report checks discovered from the connected DigiSat dataset.

Historical incident tests remain separate; these checks require no fixed seller,
document number, month or financial total. Only local temporary SQLite is written.
"""
from copy import deepcopy
import os
from pathlib import Path
import tempfile
import time
import unittest
from unittest.mock import patch

import app
import insight_store as store
from insight_engine import execute_plan
from insight_plans import Plan
from test_e2e_reports import scope_match, item_rows, payment_total


class LiveWorkspace(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.client=app.mongo_client()
        cls.db=cls.client[app.MONGO_DATABASE]
        try:
            latest=cls.db.Movimentacoes.find_one({'DataHoraEmissao':{'$type':9},
                '_t':{'$in':app.MOVEMENT_GROUPS['all_sales']},
                'Situacao.Descricao':{'$nin':app.STATUS_EXCLUDED}},sort=[('DataHoraEmissao',-1)])
        except Exception:
            cls.client.close();raise
        if not latest:
            cls.client.close();raise unittest.SkipTest('No dated sales available')
        cls.end=latest['DataHoraEmissao'].date();cls.start=cls.end.replace(day=1)

    @classmethod
    def tearDownClass(cls):cls.client.close()

    def setUp(self):
        self.folder=tempfile.TemporaryDirectory(dir=Path(__file__).parent/'.release-build')
        self.env=patch.dict(os.environ,{'INSIGHT_DATA_DIR':self.folder.name,'INSIGHT_SCHEDULER':'0'})
        self.env.start()
        self.addCleanup(self.folder.cleanup);self.addCleanup(self.env.stop)

    def plan(self,**changes):
        return Plan(**{'start':self.start,'end':self.end,'kind':'investigation','page_size':2,**changes})

    def test_totals_match_independent_source_aggregation(self):
        result=execute_plan(self.plan())
        match=scope_match(self.start,self.end)
        self.assertEqual(result['documents'],self.db.Movimentacoes.count_documents(match,maxTimeMS=5000))
        expected=sum(float(r['total'] or 0) for r in item_rows(self.db,match))
        self.assertAlmostEqual(result['totals']['Itens'],expected,places=2)
        self.assertAlmostEqual(result['totals']['Pagamentos'],payment_total(self.db,match),places=2)

    def test_paging_preserves_totals_and_handles_empty_tail(self):
        first=execute_plan(self.plan(page_size=1));second=execute_plan(self.plan(page_size=1,offset=1))
        self.assertEqual(first['totals'],second['totals'])
        self.assertEqual(len(second['rows']),min(1,max(0,first['documents']-1)))
        if second['rows']:self.assertNotEqual(first['rows'],second['rows'])
        tail=execute_plan(self.plan(offset=first['documents']))
        self.assertEqual(tail['rows'],[]);self.assertEqual(tail['totals'],first['totals'])

    def test_quality_filter_count_matches_unfiltered_findings(self):
        full=execute_plan(self.plan(kind='quality'))
        unpaid=execute_plan(self.plan(kind='quality',issue='unpaid'))
        self.assertEqual(unpaid['documents'],full['quality']['counts']['unpaid'])
        self.assertTrue(all(abs(r['Pagamentos'])<.01 for r in unpaid['rows']))

    def test_profiles_match_source_status_intersection(self):
        for profile,group,completed in [('native_nfce','nfce',True),('fiscal_valid','fiscal_sales',False),('fiscal_valid','fiscal_sales',True)]:
            with self.subTest(profile=profile,completed=completed):
                match=scope_match(self.start,self.end,group=group)
                if completed:match['Situacao.Descricao']='Concluído'
                if profile=='native_nfce':match['FinalidadeEmissaoNfe.Codigo']=1
                expected=sum(float(r['total'] or 0) for r in item_rows(self.db,match))
                actual=execute_plan(self.plan(profile=profile,status_filter='completed' if completed else 'valid'))
                self.assertAlmostEqual(actual['totals']['Itens'],expected,places=2)

    def test_conversation_followup_and_saved_snapshot(self):
        from insight_conversation import reply,ConversationRequest
        from insight_workspace import resume,ResumeRequest
        from datetime import timedelta
        first=reply(ConversationRequest(action='refine',filters=self.plan().model_dump(mode='json')))
        self.assertEqual(first['status'],'result')
        follow=reply(ConversationRequest(conversation_id=first['conversation_id'],message='E no dia seguinte?'))
        self.assertEqual(follow['plan']['start'],str(self.start+timedelta(days=1)))
        with patch('insight_conversation.execute_plan') as query:
            restored=resume(ResumeRequest(run_id=first['run_id']));query.assert_not_called()
        self.assertEqual(restored['result']['totals'],first['result']['totals'])

    def test_complete_export_matches_live_count_and_total(self):
        from insight_exports import ExportRequest,start_export,get_job
        from openpyxl import load_workbook
        result=execute_plan(self.plan());key=store.save_run(result['plan'],result)
        job=start_export(ExportRequest(run_id=key,format='xlsx',complete=True))
        deadline=time.monotonic()+60
        while time.monotonic()<deadline:
            current=get_job(job['id'])
            if current['status'] in {'succeeded','failed'}:break
            time.sleep(.1)
        self.assertEqual(current['status'],'succeeded',current)
        self.assertEqual(current['records'],result['documents'])
        book=load_workbook(store.data_dir()/'exports'/current['filename'],read_only=True,data_only=True)
        try:
            rows=list(book['Base - vendas'].values);index=rows[0].index('Itens do documento')
            self.assertEqual(len(rows)-1,result['documents'])
            self.assertAlmostEqual(sum(float(row[index]) for row in rows[1:]),result['totals']['Itens'],places=2)
        finally:book.close()

    def test_alert_deduplication_does_not_require_customer_discrepancies(self):
        from insight_schedule import ScheduleRequest,create,run_once
        result=execute_plan(self.plan());key=store.save_run(result['plan'],result)
        schedule=create(ScheduleRequest(run_id=key,title='Deterministic alert',period_policy='fixed',metric='Diferenca',threshold=5))
        evidence=deepcopy(result);evidence['totals']['Diferenca']=10
        with patch('insight_schedule.execute_plan',return_value=evidence):
            self.assertEqual(run_once(schedule['id'])['last_value'],10)
            run_once(schedule['id'])
        self.assertEqual(store.read('SELECT count(*) n FROM alerts')[0]['n'],1)


if __name__=='__main__':unittest.main()
