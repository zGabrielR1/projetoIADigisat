from datetime import date
import unittest
from unittest.mock import patch
from fastapi import HTTPException
import insight_native as native
from insight_report_search import discover,index,primary_call

class SmartFilterTests(unittest.TestCase):
    def test_overdue_intent_uses_verified_native_status_and_date_basis(self):
        result=discover('contas a receber atrasadas deste mês',anchor=date(2026,10,28))
        match=result['matches'][0]
        proposed=match['proposed_filters']
        values={s['name']:s['value'] for s in proposed['suggestions']}
        self.assertEqual(values['situacao']['Codigo'],4)
        self.assertEqual(values['tipoFiltroData']['Codigo'],2)
        self.assertIn('preset=',match['url'])
        with patch.object(native,'host') as host:
            preview=native.preview_filters(native.FilterPreview(report_id=match['id'],call_index=proposed['call_index'],arguments=values))
            self.assertFalse(preview['executed']);host.assert_not_called()

    def test_mixed_modules_and_statuses_are_not_silently_narrowed(self):
        both=discover('contas a pagar e receber deste mês',limit=20)
        self.assertTrue(both['needs_clarification'])
        self.assertEqual({m['id'].split('.')[0] for m in both['matches']},{'Pagamento','Recebimento'})
        conflict=discover('contas a pagar quitadas e em aberto')
        self.assertTrue(conflict['needs_clarification'])
        for match in conflict['matches']:
            self.assertFalse(any(s['name']=='situacao' for s in match['proposed_filters']['suggestions']))

    def test_primary_report_call_skips_auxiliary_lookup(self):
        report=next(n for r,n in index() if r['id']=='MovimentacaoSaida.ItensMovimentadosPorMarca')
        call,_=primary_call(report)
        self.assertEqual(call,1)

    def test_card_operation_is_resolved_from_declared_options(self):
        match=discover('recebimentos de cartão crédito por data de vencimento deste mês')['matches'][0]
        values={s['name']:s['value'] for s in match['proposed_filters']['suggestions']}
        self.assertEqual(values['tipoFiltroOperacao']['Codigo'],1)
        self.assertEqual(values['tipoFiltroData']['Codigo'],2)

    def test_shared_urls_cannot_supply_customer_ids_or_arbitrary_code(self):
        for arguments in [{'contaReferencia':'0'*24},{'situacao':{'Codigo':999,'Descricao':'invented'}},{'$type':'Evil'}]:
            with self.subTest(arguments=arguments),self.assertRaises(HTTPException):
                native.preview_filters(native.FilterPreview(report_id='Recebimento.PagamentosRecebimentos',arguments=arguments))

    def test_every_proposal_is_valid_for_its_report_and_missing_period_is_visible(self):
        for report,metadata in index():
            result=discover(report['id'],limit=1,anchor=date(2026,10,28))
            if not result['matches'] or not result['matches'][0]['available']:continue
            p=result['matches'][0]['proposed_filters']
            values={s['name']:s['value'] for s in p['suggestions']}
            validated=native.preview_filters(native.FilterPreview(report_id=report['id'],call_index=p['call_index'],arguments=values))
            self.assertFalse(validated['executed'])
        match=discover('contas a pagar em aberto')['matches'][0]
        self.assertTrue(match['proposed_filters']['warnings'])

if __name__=='__main__':unittest.main()
