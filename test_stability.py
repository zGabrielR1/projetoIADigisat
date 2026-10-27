"""Exploratory release regressions, isolated from customer data and configuration."""
from contextlib import closing
from datetime import date
import json
import os
from pathlib import Path
import tempfile
import unittest
from unittest.mock import MagicMock, patch

from fastapi import HTTPException
from pydantic import ValidationError
import app
import insight_store as store
from insight_plans import Plan, infer_plan
from insight_conversation import ConversationRequest, reply

ROOT=Path(__file__).parent


class IsolatedState(unittest.TestCase):
    def setUp(self):
        self.folder=tempfile.TemporaryDirectory(dir=ROOT/'.release-build')
        self.environment=patch.dict(os.environ,{'INSIGHT_DATA_DIR':self.folder.name,'AI_ENDPOINT':'','INSIGHT_SCHEDULER':'0'})
        self.environment.start()
    def tearDown(self):
        self.environment.stop();self.folder.cleanup()

    @staticmethod
    def fake_result(plan):
        return {'plan':plan.model_dump(mode='json'),'documents':0,'rows':[],
                'interpretation':{'kind':'documents','kind_label':'Documentos'}}


class StableConversationTests(IsolatedState):
    def test_projection_retains_formula_inputs_and_avoids_path_collisions(self):
        projection=app.computed_projection('@item_total','DataHoraEmissao','ItensBase','Empresa.Nome')
        self.assertIn('ItensBase',projection)
        self.assertNotIn('ItensBase.PrecoUnitario',projection)
        self.assertIn('Empresa.Nome',projection)
        narrow=app.computed_projection('@item_total','DataHoraEmissao','ItensBase[].ProdutoServico.Descricao')
        self.assertIn('ItensBase.Percentual.ValorSt',narrow)
        self.assertIn('ItensBase.ProdutoServico.Descricao',narrow)
    def test_missing_seller_keeps_explicit_date_after_answer(self):
        with patch('insight_conversation.execute_plan',side_effect=self.fake_result) as execute:
            question=reply(ConversationRequest(message='Em 05/10/2026, qual foi a diferença do vendedor?',
                filters={'start':'2026-09-01','end':'2026-09-30'}))
            self.assertEqual(question['status'],'clarification')
            response=reply(ConversationRequest(conversation_id=question['conversation_id'],answer='2'))
        self.assertEqual(response['plan']['start'],'2026-10-05')
        self.assertEqual(response['plan']['end'],'2026-10-05')
        self.assertEqual(execute.call_count,1)

    def test_date_clarification_keeps_seller_from_original_question(self):
        with patch('insight_conversation.execute_plan',side_effect=self.fake_result):
            question=reply(ConversationRequest(message='Investigue vendedor 2 no dia 5'))
            self.assertEqual(question['clarification']['field'],'period')
            response=reply(ConversationRequest(conversation_id=question['conversation_id'],answer='05/10/2026'))
        self.assertEqual(response['plan']['seller_query'],'2')

    def test_oversized_period_is_a_clarification_not_an_unhandled_exception(self):
        with patch('insight_conversation.execute_plan') as execute:
            response=reply(ConversationRequest(message='Investigue de 01/01/2024 a 31/12/2026'))
            self.assertEqual(response['status'],'clarification')
            execute.assert_not_called()

    def test_answer_cannot_bypass_period_limit(self):
        first=reply(ConversationRequest(message='O mesmo vendedor naquele dia'))
        second=reply(ConversationRequest(conversation_id=first['conversation_id'],answer='2'))
        with patch('insight_conversation.execute_plan',side_effect=self.fake_result) as execute:
            response=reply(ConversationRequest(conversation_id=second['conversation_id'],answer='01/01/2024 a 31/12/2026'))
            self.assertEqual(response['status'],'clarification')
            execute.assert_not_called()

    def test_native_quality_intent_is_preserved_by_language_parser(self):
        result,prompt=infer_plan('Verifique a qualidade dos dados no perfil nativo em 05/10/2026',
            Plan(start='2026-10-01',end='2026-10-26'),False)
        self.assertIsNone(prompt)
        self.assertEqual(result.kind,'quality')

    def test_company_only_comparison_keeps_original_period(self):
        result,prompt=infer_plan('Compare com a empresa "Empresa B"',
            Plan(start='2026-10-05',end='2026-10-05',company_value='Empresa A',kind='investigation'),True)
        self.assertIsNone(prompt)
        self.assertEqual(result.company_value,'Empresa A')
        self.assertEqual(result.comparison.company_value,'Empresa B')

    def test_anonymous_same_day_is_not_guessed_from_month_context(self):
        result,prompt=infer_plan('Investigue naquele dia',
            Plan(start='2026-10-01',end='2026-10-26'),True)
        self.assertIsNotNone(prompt)
        self.assertEqual(prompt['field'],'period')


class StableJobsTests(IsolatedState):
    def test_progress_cannot_erase_cancellation_flag(self):
        from insight_exports import put_job,cancel_job,update_progress,get_job
        key=store.identifier()
        put_job(key,{'id':key,'status':'running','records':0})
        cancel_job(key)
        with self.assertRaises(HTTPException) as caught:update_progress(key,100)
        self.assertEqual(caught.exception.status_code,499)
        self.assertTrue(get_job(key)['cancel_requested'])

    def test_queued_cancel_does_not_start_a_database_query(self):
        from insight_exports import put_job,build_export,get_job,ExportRequest
        key=store.identifier()
        put_job(key,{'id':key,'status':'queued','format':'csv','records':0,'cancel_requested':True})
        run={'created':store.now(),'plan':Plan(start='2026-10-05',end='2026-10-05').model_dump(mode='json'),
             'result':{'rows':[]}}
        with patch('insight_exports.datasets',return_value=iter([])) as datasets:
            build_export(key,ExportRequest(run_id='test-run-identifier',format='csv',complete=True),run)
            datasets.assert_not_called()
        self.assertEqual(get_job(key)['status'],'cancelled')

    def test_new_day_same_amount_still_generates_new_alert(self):
        import insight_schedule as schedule
        from insight_schedule import ScheduleRequest
        original=Plan(start='2026-10-05',end='2026-10-05',kind='investigation').model_dump(mode='json')
        result={'plan':original,'investigation':{},'documents':2,'totals':{'Diferenca':100}}
        # The actual create API requires an investigation with details.
        result['investigation']={'findings':['test']}
        run_id=store.save_run(original,result)
        created=schedule.create(ScheduleRequest(run_id=run_id,title='Test',period_policy='today',threshold=99))
        def fake(plan):
            return {**result,'plan':plan.model_dump(mode='json'),'period':{'start':str(plan.start),'end':str(plan.end)}}
        class First(date):
            @classmethod
            def today(cls):return date(2026,10,5)
        class Next(date):
            @classmethod
            def today(cls):return date(2026,10,6)
        with patch('insight_schedule.execute_plan',side_effect=fake),patch('insight_schedule.date',First):
            schedule.run_once(created['id']);schedule.run_once(created['id'])
        with patch('insight_schedule.execute_plan',side_effect=fake),patch('insight_schedule.date',Next):
            schedule.run_once(created['id'])
        self.assertEqual(len(store.read('SELECT id FROM alerts')),2)

    def test_empty_database_is_not_a_successful_setup(self):
        from insight_connection import ConnectionSettings,test_connection
        client=MagicMock()
        client.__enter__.return_value=client
        client.__getitem__.return_value.list_collection_names.return_value=[]
        with patch('pymongo.MongoClient',return_value=client),patch('insight_connection.privileges',return_value={'verified_read_only':True}):
            with self.assertRaises(HTTPException):test_connection(ConnectionSettings(database='DoesNotExist',username='reader'))


if __name__=='__main__':unittest.main(verbosity=2)
