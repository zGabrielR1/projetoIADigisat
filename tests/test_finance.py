"""Finance rules exercised with adversarial history and boundary fixtures."""
from datetime import date, datetime, timedelta
from decimal import Decimal
import copy
import json
from pathlib import Path
import unittest

from bson import ObjectId, Decimal128
from fastapi import HTTPException
from pydantic import ValidationError

import insight_finance as f


COMPANY = ObjectId('000000000000000000000001')
PERSON = ObjectId('000000000000000000000002')
ACCOUNT = ObjectId('000000000000000000000003')


def parcel(*, settled=False, value='100.00'):
    pending = {'_t': 'HistoricoPendente', 'Valor': Decimal128(value), 'Data': datetime(2026, 1, 1),
               'EmpresaReferencia': COMPANY, 'ContaReferencia': ACCOUNT,
               'EspeciePagamento': {'Codigo': 1, 'Descricao': 'Dinheiro'}}
    record = {'_id': ObjectId(), '_t': 'ParcelaRecebimento', 'Situacao': {'Codigo': 3 if settled else 1},
              'Historico': [pending], 'Vencimento': datetime(2026, 1, 1), 'PessoaReferencia': PERSON,
              'DataEmissao': datetime(2025, 12, 20), 'DataQuitacao': datetime(2026, 1, 5)}
    if settled:
        record['Historico'].append({**pending, '_t': 'HistoricoQuitado', 'Valor': Decimal128('95'),
                                    'Desconto': Decimal128('5'), 'Data': datetime(2026, 1, 5)})
    return record


def request(report_id='Recebimento.RecebimentosPorPessoa', **kwargs):
    return f.FinanceRequest(report_id=report_id, start=date(2026, 1, 1), end=date(2026, 1, 31),
                            company_id=str(COMPANY), **kwargs)


class FinanceRulesTests(unittest.TestCase):
    def test_arithmetic_matches_independent_csharp_reconstruction(self):
        oracle = json.loads((Path(__file__).parent / 'fixtures/finance-calculation-oracle.json').read_text(encoding='utf-8'))
        self.assertFalse(oracle['target_executed'])
        for vector in oracle['vectors']:
            start, end = date.fromisoformat(vector['start']), date.fromisoformat(vector['end'])
            with self.subTest(start=start, end=end):
                self.assertEqual(f.pro_rata_period(start,end), Decimal(vector['period']))
                for kind, expected in [('JuroSimples',vector['simple']),('JuroComposto',vector['compound'])]:
                    record=parcel();record['Vencimento']=datetime.combine(start,datetime.min.time())
                    record['Juro']={'_t':kind,'Percentual':oracle['percent'],'DiasCarencia':0}
                    self.assertEqual(f.charges(record,Decimal(oracle['principal']),end)[0],Decimal(expected))

    def test_settled_value_comes_from_last_pending_not_last_payment(self):
        amounts = f.projected_amounts(parcel(settled=True), date(2026, 2, 1))
        self.assertEqual(amounts['value'], Decimal('100'))
        self.assertEqual(amounts['corrected'], Decimal('100'))
        self.assertEqual(amounts['settled'], Decimal('95'))
        self.assertEqual(amounts['discount'], Decimal('5'))

    def test_partial_override_uses_last_settlement_without_summing_previous_payments(self):
        record = parcel(settled=True)
        record['Historico'] += [{**record['Historico'][-1], '_t': ['HistoricoQuitado', 'HistoricoQuitadoParcial'], 'Valor': 30}]
        amounts = f.projected_amounts(record, date(2026, 2, 1))
        self.assertEqual(amounts['value'], 30)
        self.assertEqual(amounts['corrected'], 30)
        self.assertEqual(amounts['settled'], 30)

    def test_settlement_subclass_with_scalar_discriminator_is_still_a_settlement(self):
        record=parcel(settled=True)
        record['Historico'][-1]['_t']='HistoricoQuitadoComRetornoDePagar'
        self.assertEqual(f.projected_amounts(record,date(2026,2,1))['settled'],Decimal('95'))

    def test_partial_interface_sibling_does_not_trigger_partial_class_override(self):
        record=parcel(settled=True)
        record['Historico'][-1]['_t']='HistoricoQuitadoParcialExtratoBancario'
        self.assertEqual(f.projected_amounts(record,date(2026,2,1))['value'],Decimal('100'))

    def test_reversal_uses_current_history_and_does_not_count_prior_payment(self):
        record = parcel(settled=True)
        record['Situacao']['Codigo'] = 4
        record['Historico'].append({**record['Historico'][-1], '_t': 'HistoricoEstornado', 'Valor': 77})
        amounts = f.projected_amounts(record, date(2026, 2, 1))
        self.assertEqual(amounts['value'], 77)
        self.assertEqual(amounts['settled'], 0)
        self.assertEqual(amounts['discount'], 0)

    def test_grace_is_strict_and_charges_are_not_applied_after_settlement(self):
        record = parcel()
        record['Juro'] = {'_t':'JuroSimples', 'Percentual': 2, 'DiasCarencia': 5}
        record['Multa'] = {'Percentual': 3, 'DiasCarencia': 5}
        self.assertEqual(f.charges(record, Decimal('100'), date(2026, 1, 6)), (0, 0))
        interest, penalty = f.charges(record, Decimal('100'), date(2026, 2, 1))
        self.assertEqual((interest, penalty), (Decimal('2.00'), Decimal('3.00')))
        record['Situacao']['Codigo'] = 3
        self.assertEqual(f.charges(record, Decimal('100'), date(2026, 2, 1)), (0, 0))

    def test_period_uses_month_fractions_not_days_divided_by_thirty(self):
        self.assertEqual(f.pro_rata_period(date(2026, 1, 1), date(2026, 2, 1)), Decimal('1'))
        self.assertEqual(f.pro_rata_period(date(2026, 1, 15), date(2026, 2, 15)), Decimal('1.0484'))
        self.assertEqual(f.pro_rata_period(date(2024, 2, 1), date(2024, 3, 1)), Decimal('1'))

    def test_compound_interest_and_away_from_zero_rounding(self):
        record = parcel()
        record['Juro'] = {'_t':'JuroComposto', 'Percentual': 2, 'DiasCarencia': 0}
        self.assertEqual(f.charges(record, Decimal('100'), date(2026, 3, 1))[0], Decimal('4.04'))
        self.assertEqual(f.rounded(Decimal('-1.005')), Decimal('-1.01'))
        self.assertEqual(f.rounded(Decimal('1.005')), Decimal('1.01'))

    def test_fee_override_ignores_interest_and_fixed_fee_requires_value_greater_than_fee(self):
        record = parcel()
        record['_t'] = ['ParcelaRecebimento', 'ParcelaRecebimentoCartao']
        record['TaxaRede'] = {'TipoTaxa':0, 'PercentualTaxa':2.5}
        record['Juro'] = {'_t':'JuroSimples', 'Percentual':5, 'DiasCarencia':0}
        amounts = f.projected_amounts(record, date(2026, 2, 1))
        self.assertEqual(amounts['corrected'], Decimal('97.50'))
        self.assertEqual(amounts['fee'], Decimal('2.50'))
        record['TaxaRede'] = {'TipoTaxa':1, 'PercentualTaxa':100}
        self.assertEqual(f.projected_amounts(record, date(2026, 2, 1))['fee'], 0)

    def test_query_date_and_overdue_reference_are_independent(self):
        query = f.query_filter(request(status='overdue'), date(2026, 2, 5))['$and']
        self.assertIn({'Vencimento': {'$gte': datetime(2026,1,1), '$lt': datetime(2026,2,1)}}, query)
        self.assertIn({'Vencimento': {'$lt': datetime(2026,2,5)}}, query)
        self.assertIn({'EmpresaReferencia': COMPANY}, query)

    def test_settlement_date_and_incompatible_status_are_both_applied(self):
        query = f.query_filter(request(date_basis='settled', status='pending'), date(2026,2,1))['$and']
        self.assertIn({'Situacao.Codigo':3},query)
        self.assertIn({'Situacao.Codigo':1},query)

    def test_competence_matches_any_history_and_omits_company(self):
        req = f.FinanceRequest(report_id='Recebimento.RecebimentosQuitadosPorDataCompetencia',
                               start=date(2026,1,1), end=date(2026,1,31))
        query = f.query_filter(req, date(2026,2,1))['$and']
        self.assertIn({'Historico': {'$elemMatch': {'Data': {'$gte': datetime(2026,1,1), '$lt': datetime(2026,2,1)}}}},query)
        self.assertFalse(any('EmpresaReferencia' in clause for clause in query))

    def test_plan_and_cost_center_can_match_different_history_entries(self):
        query = f.query_filter(request(plan_code='1', cost_center_code='2'), date(2026,2,1))['$and']
        self.assertIn({'Historico.PlanoContaCodigoUnico':'1'},query)
        self.assertIn({'Historico.CentroCustoCodigoUnico':'2'},query)

    def test_unsafe_and_silently_ignored_parameters_are_rejected(self):
        for kwargs in [{'company_id':None}, {'person_id':'$where'}, {'consolidated':True}, {'species':1},
                       {'report_id':'Recebimento.Unimplemented'}, {'unexpected':1}]:
            payload = request().model_dump()
            payload.update(kwargs)
            with self.assertRaises(ValidationError):
                f.FinanceRequest(**payload)
        with self.assertRaises(ValidationError):
            request('Pagamento.PagamentosPorPessoa', account_id=str(ACCOUNT))

    def test_missing_history_and_nonfinite_amounts_do_not_produce_partial_totals(self):
        for history in [[], None, [None]]:
            record=parcel();record['Historico']=history
            with self.assertRaises(HTTPException):
                f.projected_amounts(record, date(2026,2,1))
        with self.assertRaises(HTTPException):
            f.number(float('nan'))
        with self.assertRaises(HTTPException):
            f.types({'_t':[{'not':'a type'}]})

    def test_inclusion_outside_objectid_timestamp_range_is_a_validation_error(self):
        for start,end in [(date(1969,1,1),date(1969,2,1)),(date(2106,1,1),date(2106,2,1))]:
            with self.assertRaises(ValidationError):
                f.FinanceRequest(report_id='Recebimento.PagamentosRecebimentos',start=start,end=end,
                                  consolidated=True,date_basis='inclusion')

    def test_group_totals_preserve_company_identity_even_when_names_match(self):
        r=f.project(parcel(), {COMPANY:{'Nome':'Empresa'},PERSON:{'Nome':'Pessoa'}},{},date(2026,2,1))
        other=copy.deepcopy(r);other['company_id']=str(ObjectId())
        totals,groups=f.summarize([r,other],'person')
        self.assertEqual(len(groups),2)
        self.assertEqual(totals['value'],200)
        self.assertEqual(f.json_money(totals)['value'],'200.00')


if __name__ == '__main__':
    unittest.main()
