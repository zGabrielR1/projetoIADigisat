"""Native diagnostics preserve results and never infer remote server identity."""
from copy import deepcopy
from decimal import Decimal
import unittest
from unittest.mock import patch

import insight_native as native
from insight_upstream_issues import ANALYZED_SHA256, REPORT_CONTRACT, SHARED_ARRAY_METHODS, issues_for_call


class UpstreamIssueTests(unittest.TestCase):
    def call(self, method):
        return {'contract': REPORT_CONTRACT, 'method': method}

    def test_each_parallel_aggregation_is_a_build_unverified_candidate(self):
        for method in SHARED_ARRAY_METHODS:
            with self.subTest(method=method):
                issue, = issues_for_call(self.call(method), {})
                self.assertEqual(issue['remote_build_status'], 'unverified')
                self.assertEqual(issue['data_condition_status'], 'unverified')
                self.assertEqual(issue['applicability'], 'candidate')
                self.assertEqual(issue['analyzed_artifact']['sha256'], ANALYZED_SHA256)
                self.assertFalse(issue['results_modified'])

    def test_method_and_contract_must_both_match(self):
        self.assertEqual(issues_for_call(self.call('ObterItensVendidosPorPessoa')), [])
        self.assertEqual(issues_for_call({'method':'ObterItensVendidos','contract':'AnotherContract'}), [])
        self.assertEqual(issues_for_call({}), [])

    def test_fractional_issue_only_matches_actual_false(self):
        call = self.call('ObterItensVendidos')
        issue, = issues_for_call(call, {'agruparPorValorUnitario':False})
        self.assertEqual(issue['condition_status'], 'matched')
        self.assertEqual(issues_for_call(call, {'agruparPorValorUnitario':True}), [])
        for arguments in [None, {}, {'agruparPorValorUnitario':'false'}, {'agruparPorValorUnitario':0}]:
            with self.subTest(arguments=arguments):
                issue, = issues_for_call(call, arguments)
                self.assertEqual(issue['condition_status'], 'unknown')

    def test_local_hash_does_not_authenticate_remote_server(self):
        for local_hash in [None, '0' * 64, ANALYZED_SHA256]:
            call = self.call('ObterTotalMovimentacoesPorMesPorVendedor')
            call['sha256'] = local_hash
            call['installed_assemblies'] = [{'sha256':local_hash}]
            issue, = issues_for_call(call)
            self.assertEqual(issue['remote_build_status'], 'unverified')

    def test_returns_independent_registry_values(self):
        call = self.call('ObterItensVendidos')
        issues_for_call(call)[0]['analyzed_artifact']['sha256'] = 'changed'
        self.assertEqual(issues_for_call(call)[0]['analyzed_artifact']['sha256'], ANALYZED_SHA256)

    def test_only_supported_groupings_link_to_independent_audit(self):
        monthly = issues_for_call(self.call('ObterTotalMovimentacoesPorMesPorVendedor'))[0]
        self.assertEqual(monthly['independent_audit']['endpoint'], '/api/audit/monthly')
        prices = issues_for_call(self.call('ObterItensVendidos'))[0]
        self.assertEqual(prices['independent_audit']['endpoint'], '/api/audit/unit-prices')
        for method in SHARED_ARRAY_METHODS - {'ObterTotalMovimentacoesPorMesPorVendedor'}:
            self.assertIsNone(issues_for_call(self.call(method))[0]['independent_audit']['endpoint'])

    def test_catalog_attaches_candidates_without_mutating_cached_source(self):
        source = native.catalog()
        unchanged = deepcopy(source)
        with patch.object(native, 'catalog', return_value=source):
            response = native.native_catalog()
        reports = {report['id']:report for report in response['reports']}
        monthly = reports['MovimentacaoSaida.TotalMovimentacoesPorMesPorVendedor']['calls'][0]
        self.assertEqual(monthly['upstream_issues'][0]['remote_build_status'], 'unverified')
        fractional = reports['MovimentacaoSaida.ItensVendidos']['calls'][0]
        self.assertEqual(fractional['upstream_issues'][0]['condition_status'], 'unknown')
        self.assertEqual(source, unchanged)

    def invoke(self, report_id, arguments):
        data = [{'Quantidade':Decimal('0.5'), 'Preco':Decimal('10.00'), 'Total':Decimal('10.00')}]
        session = {'username':'fixture', 'password':'secret', 'company_id':'0'*24, 'port':12221}
        with patch.object(native, 'get_session', return_value=session), \
             patch.object(native, 'host', return_value={'ok':True, 'data':data}) as host:
            result = native.query(native.NativeQuery(report_id=report_id, arguments=arguments), 'opaque')
        self.assertEqual(host.call_count, 1)
        return result

    def test_query_fractional_warning_keeps_native_price_unchanged(self):
        arguments = {'dataInicial':'2026-01-01T00:00:00', 'dataFinal':'2026-12-31T23:59:59',
                     'tipo':0, 'consolidado':False, 'agruparPorValorUnitario':False}
        result = self.invoke('MovimentacaoSaida.ItensVendidos', arguments)
        self.assertEqual(result['rows'][0]['record']['Preco'], '10.00')
        self.assertEqual(result['upstream_issues'][0]['condition_status'], 'matched')
        self.assertTrue(any('fracionada' in issue['title'] for issue in result['upstream_issues']))
        self.assertTrue(any('servidor consultado não foi verificada' in warning for warning in result['warnings']))
        arguments['agruparPorValorUnitario'] = True
        result = self.invoke('MovimentacaoSaida.ItensVendidos', arguments)
        self.assertEqual(result['upstream_issues'], [])
        self.assertEqual(len(result['warnings']), 3)

    def test_query_monthly_warns_without_asserting_this_run_is_wrong(self):
        result = self.invoke('MovimentacaoSaida.TotalMovimentacoesPorMesPorVendedor', {
            'ano':2026, 'origem':{'Codigo':99,'Descricao':'Todas as saídas'},
            'situacao':{'Codigo':2,'Descricao':'Concluído'}, 'idVendedor':''})
        self.assertEqual(result['upstream_issues'][0]['remote_build_status'], 'unverified')
        self.assertEqual(len(result['warnings']), 4)

    def test_unaffected_query_has_no_issue_warning(self):
        result = self.invoke('OperacaoFiscal.ListaCadastroCfop', {})
        self.assertEqual(result['upstream_issues'], [])
        self.assertEqual(len(result['warnings']), 3)


if __name__ == '__main__':
    unittest.main()
