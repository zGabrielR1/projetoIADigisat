from datetime import date
from unittest.mock import patch
import unittest
from insight_report_search import discover,index,should_discover,ALIASES

class ReportSearchTests(unittest.TestCase):
    def test_every_catalog_identity_is_discoverable_including_absent_entries(self):
        for report,native in index():
            result=discover(report['id'],anchor=date(2026,10,28))
            self.assertEqual(result['matches'][0]['id'],report['id'])
            self.assertEqual(result['matches'][0]['available'],bool(report.get('native_url')))
        self.assertTrue({r['module'] for r,_ in index()}<=set(ALIASES))

    def test_portuguese_intents_rank_module_and_report_without_execution(self):
        cases={'contas a pagar em aberto deste mês':'Pagamento.PagamentosEmAberto',
            'clientes inadimplentes':'Pessoa.ClientesInadimplentes',
            'cadastro de CFOP':'OperacaoFiscal.ListaCadastroCfop',
            'boletos vencidos':'Boletos.ListaBoletos','agendamentos':'Agendamento.ListaAgendamentos'}
        for question,expected in cases.items():
            with self.subTest(question=question):self.assertEqual(discover(question)['matches'][0]['id'],expected)

    def test_period_prefill_is_explicit_and_does_not_invent_customer_references(self):
        result=discover('contas a pagar em aberto deste mês',anchor=date(2026,10,28))
        self.assertEqual(result['period'],{'start':'2026-10-01','end':'2026-10-28'})
        self.assertIn('start=2026-10-01',result['matches'][0]['url'])
        self.assertEqual(result['execution'],'selection_required')
        self.assertNotIn('company_id',result)

    def test_unknown_questions_and_invalid_dates_do_not_silently_run_sales(self):
        self.assertEqual(discover('previsão meteorológica de Marte')['matches'],[])
        self.assertTrue(discover('relatório de CFOP 2026-02-31')['period_error'])
        self.assertTrue(should_discover('contas a pagar'))
        self.assertFalse(should_discover('faturamento deste mês'))
        self.assertFalse(should_discover('relatório de faturamento deste mês'))
        self.assertTrue(should_discover('estoque de produtos'))
        self.assertFalse(should_discover('e o vendedor 3?'))

    def test_conversation_discovery_never_executes_a_data_plan(self):
        import insight_conversation as conversation
        session={'plan':None,'pending':None,'messages':[]}
        with patch.object(conversation.store,'get_conversation',return_value=('fixture',session)),patch.object(conversation.store,'put_conversation') as save,patch.object(conversation,'execute_plan') as execute:
            result=conversation.reply(conversation.ConversationRequest(message='contas a pagar em aberto'))
            self.assertEqual(result['status'],'native_reports')
            self.assertEqual(result['native_reports']['matches'][0]['id'],'Pagamento.PagamentosEmAberto')
            execute.assert_not_called();save.assert_called_once()

    def test_search_api_and_selection_schema_are_bounded(self):
        from insight_native import ReportSearch,search_reports
        from pydantic import ValidationError
        with self.assertRaises(ValidationError):ReportSearch(question='x'*501)
        with self.assertRaises(ValidationError):ReportSearch(question='CFOP',limit=21)
        self.assertLessEqual(len(search_reports(ReportSearch(question='produtos',limit=3))['matches']),3)

if __name__=='__main__':unittest.main()
