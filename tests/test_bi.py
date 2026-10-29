"""BI arithmetic, scope propagation, local models, and bounded query contracts."""
from datetime import date
import copy
import os
import tempfile
import unittest
from unittest.mock import patch
from fastapi import HTTPException
from pydantic import ValidationError
import app
import insight_bi as bi


MAPPING = {'collection': 'Movimentacoes', 'date_field': 'DataHoraEmissao', 'value_field': '@item_total',
           'company_field': 'Empresa.Nome', 'product_field': 'ItensBase[].ProdutoServico.Descricao'}


def query(**values):
    return bi.BIQuery(mapping=MAPPING, start=date(2026, 10, 1), end=date(2026, 10, 29), **values)


def fake_report(req):
    previous = req.start < date(2026, 10, 1)
    if req.kind == 'revenue':
        return {'rows': [{'total': 80 if previous else 100.05}], 'documents': 4 if previous else 5,
                'payments_total': 75 if previous else 90, 'warnings': [],
                'scope': {'movement_group': req.movement_group, 'excluded': 2}}
    if req.kind == 'daily':
        return {'rows': [{'_id': str(req.start), 'total': 80 if previous else 100.05, 'documents': 4 if previous else 5, 'items': 7}], 'documents': 4 if previous else 5, 'warnings': []}
    return {'rows': [{'_id': 'Empresa teste' if req.kind == 'companies' else 'Produto teste', 'total': 60.02, 'items': 2}], 'documents': 5, 'warnings': []}


class BITests(unittest.TestCase):
    def setUp(self):
        self.tmp = tempfile.TemporaryDirectory()
        self.env = patch.dict(os.environ, {'INSIGHT_DATA_DIR': self.tmp.name})
        self.env.start()
        self.engine = patch.object(app, 'run_report', side_effect=fake_report)
        self.run = self.engine.start()

    def tearDown(self):
        self.engine.stop()
        self.env.stop()
        self.tmp.cleanup()

    def test_comparison_uses_equal_inclusive_period_and_actual_total(self):
        data = bi.overview(query(company='Loja matriz'))
        self.assertEqual(data['previous_period'], {'start': '2026-09-02', 'end': '2026-09-30'})
        self.assertEqual(data['metrics']['revenue'], 100.05)
        self.assertEqual(data['metrics']['ticket'], 20.01)
        self.assertEqual(data['deltas']['revenue'], {'absolute': 20.05, 'percent': 25.06})
        self.assertNotEqual(data['metrics']['revenue'], sum(row['total'] for row in data['products']))
        for call in self.run.call_args_list:
            request = call.args[0]
            self.assertEqual(request.company_value, 'Loja matriz')
            self.assertEqual(request.movement_group, 'all_sales')
            self.assertTrue(request.only_valid)
            self.assertLessEqual(request.limit, app.MAX_ROWS)

    def test_zero_and_missing_are_different_and_no_infinite_growth(self):
        self.assertEqual(bi.totals({'rows': [], 'documents': 0, 'payments_total': None}),
                         {'revenue': 0.0, 'documents': 0, 'ticket': 0.0, 'payments': None})
        change = bi.deltas({'revenue': 10, 'payments': None}, {'revenue': 0, 'payments': 0})
        self.assertEqual(change['revenue'], {'absolute': 10.0, 'percent': None})
        self.assertIsNone(change['payments'])
        self.assertEqual(bi.totals({'rows': [{'total': 0}], 'documents': 1, 'payments_total': 0})['payments'], 0)

    def test_partial_failure_preserves_metrics_and_names_failed_block(self):
        def failed(req):
            if req.kind == 'daily':
                raise HTTPException(422, 'Tempo de consulta excedido')
            return fake_report(req)
        self.run.side_effect = failed
        data = bi.overview(query(compare=False))
        self.assertEqual(data['metrics']['revenue'], 100.05)
        self.assertEqual(data['errors']['daily'], 'Tempo de consulta excedido')
        self.assertEqual(data['daily'], [])
        self.assertIsNone(data['previous'])
        self.assertIsNone(data['previous_period'])

    def test_primary_failure_is_not_converted_to_zero(self):
        self.run.side_effect = HTTPException(503, 'Base indisponível')
        with self.assertRaises(HTTPException) as error:
            bi.overview(query())
        self.assertEqual(error.exception.status_code, 503)

    def test_invalid_period_and_unsafe_models_fail_before_query(self):
        for extra in [{'end': date(2026, 9, 30)}, {'end': date(2027, 10, 2)}, {'end': date.max}]:
            with self.subTest(extra=extra), self.assertRaises(ValidationError):
                bi.BIQuery(mapping=MAPPING, **{'start': date(2026, 10, 1), 'end': date(2026, 10, 29), **extra})
        for extra in [{'date_field': '$where'}, {'value_field': {'$function': 'evil'}},
                      {'company_field': 'Empresas[].Nome'}, {'value_field': 'Total'},
                      {'product_field': 'Outros[].Nome'}]:
            with self.subTest(extra=extra), self.assertRaises(ValidationError):
                bi.DataMapping(**{**MAPPING, **extra})
        self.run.assert_not_called()

    def test_model_is_local_and_bound_to_database_and_server(self):
        schema = {'sampled': 30, 'fields': [{'name': field} for field in MAPPING.values() if field != '@item_total'],
                  'computed': [{'name': '@item_total'}]}
        with patch.object(app, 'schema', return_value=schema):
            saved = bi.save_model(bi.ModelSave(name=' Vendas fiscais ', mapping=MAPPING))
        self.assertEqual(saved['name'], 'Vendas fiscais')
        self.assertEqual(len(bi.models()['models']), 1)
        request = bi.BIQuery(model_id=saved['id'], start=date(2026, 10, 1), end=date(2026, 10, 29))
        self.assertEqual(bi.resolve_mapping(request).collection, 'Movimentacoes')
        for field, value in [('MONGO_DATABASE', 'OutraBase'), ('MONGO_URI', 'mongodb://another-host:12220')]:
            with patch.object(app, field, value), self.assertRaises(HTTPException) as error:
                bi.resolve_mapping(request)
            self.assertEqual(error.exception.status_code, 409)
        self.assertNotIn('mongodb://', str(saved))
        bi.delete_model(saved['id'])
        self.assertEqual(bi.models()['models'], [])

    def test_model_save_rejects_fields_not_observed_in_schema(self):
        with patch.object(app, 'schema', return_value={'sampled': 0, 'fields': [], 'computed': []}), self.assertRaises(HTTPException):
            bi.save_model(bi.ModelSave(name='Modelo', mapping=MAPPING))
        self.assertEqual(bi.models()['models'], [])

    def test_drill_scope_reaches_engine_and_limit_is_explicit(self):
        request = bi.ExploreQuery(mapping=MAPPING, start=date(2026, 10, 1), end=date(2026, 10, 1),
                                 company='Filial 2', dimension='documents', limit=1, movement_group='nfce')
        data = bi.explore(request)
        called = self.run.call_args.args[0]
        self.assertEqual((called.start, called.end), (date(2026, 10, 1), date(2026, 10, 1)))
        self.assertEqual(called.company_value, 'Filial 2')
        self.assertEqual(called.movement_group, 'nfce')
        self.assertTrue(data['limit_reached'])

    def test_unmapped_dimension_and_company_filter_fail_clearly(self):
        mapping = {**MAPPING, 'company_field': None}
        request = bi.ExploreQuery(mapping=mapping, start=date(2026, 10, 1), end=date(2026, 10, 2), dimension='companies')
        with self.assertRaises(HTTPException):
            bi.explore(request)
        request.dimension = 'daily'
        request.company = 'Loja 1'
        with self.assertRaises(HTTPException):
            bi.explore(request)

    def test_money_rejects_nonfinite_values(self):
        for value in ['NaN', 'Infinity', '-Infinity']:
            with self.assertRaises(HTTPException):
                bi.money(value)

    def test_model_rejects_text_as_date_or_amount(self):
        schema = {'sampled': 1, 'fields': [{'name': value, 'types': ['str']} for value in MAPPING.values()],
                  'computed': [{'name': '@item_total'}]}
        with patch.object(app, 'schema', return_value=schema), self.assertRaises(HTTPException) as error:
            bi.save_model(bi.ModelSave(name='Campos incorretos', mapping=MAPPING))
        self.assertEqual(error.exception.status_code, 422)

    def test_optional_dimensions_do_not_block_revenue(self):
        mapping = {**MAPPING, 'company_field': None, 'product_field': None}
        request = bi.BIQuery(mapping=mapping, start=date(2026, 10, 1), end=date(2026, 10, 29), compare=False)
        data = bi.overview(request)
        self.assertEqual(data['metrics']['revenue'], 100.05)
        self.assertEqual(data['companies'], [])
        self.assertEqual(data['products'], [])
        self.assertEqual({call.args[0].kind for call in self.run.call_args_list}, {'revenue', 'daily'})

    def test_truncated_daily_series_is_not_reported_as_complete(self):
        def many_days(req):
            if req.kind == 'daily':
                return {'rows': [{'_id': str(date(2026, 1, day)), 'total': 1} for day in range(1, 4)], 'warnings': []}
            return fake_report(req)
        self.run.side_effect = many_days
        with patch.object(app, 'MAX_ROWS', 3):
            data = bi.overview(query(compare=False))
        self.assertFalse(data['daily_complete'])
        self.assertTrue(any('limite de linhas' in warning for warning in data['warnings']))

    def test_export_scope_metadata_matches_the_server_request(self):
        data = bi.overview(query(company='Filial 3', movement_group='fiscal_sales', only_valid=False))
        self.assertEqual(data['applied_filters'], {'company': 'Filial 3', 'movement_group': 'fiscal_sales', 'only_valid': False})

    def test_previous_daily_preserves_its_dates_and_scope(self):
        data = bi.overview(query(company='Loja matriz'))
        self.assertEqual(data['daily'][0]['_id'], '2026-10-01')
        self.assertEqual(data['previous_daily'][0]['_id'], '2026-09-02')
        self.assertTrue(data['previous_daily_complete'])
        previous = [call.args[0] for call in self.run.call_args_list if call.args[0].kind == 'daily' and call.args[0].end < date(2026, 10, 1)]
        self.assertEqual(previous[0].company_value, 'Loja matriz')

    def test_previous_daily_failure_does_not_remove_kpi_comparison(self):
        def partially_failed(req):
            if req.kind == 'daily' and req.start < date(2026, 10, 1):
                raise HTTPException(422, 'Série anterior indisponível')
            return fake_report(req)
        self.run.side_effect = partially_failed
        data = bi.overview(query())
        self.assertIsNotNone(data['deltas']['revenue'])
        self.assertFalse(data['previous_daily_complete'])
        self.assertEqual(data['errors']['previous_daily'], 'Série anterior indisponível')

    def test_relative_view_periods_follow_calendar_and_inclusive_days(self):
        today = date(2024, 3, 1)
        start, end = date(2020, 1, 1), date(2020, 1, 2)
        self.assertEqual(bi.view_period('previous', start, end, today), (date(2024, 2, 1), date(2024, 2, 29)))
        self.assertEqual(bi.view_period('month', start, end, today), (today, today))
        self.assertEqual(bi.view_period('7', start, end, today), (date(2024, 2, 24), today))
        self.assertEqual(bi.view_period('30', start, end, today), (date(2024, 2, 1), today))
        self.assertEqual(bi.view_period('fixed', start, end, today), (start, end))

    def test_saved_view_keeps_filter_and_drill_path_without_running_reports(self):
        saved = bi.save_view(bi.ViewSave(name='Notas de uma filial', query=query(), tab='explore',
                                       dimension='documents', drill_company='Filial 2', drill_day=date(2026, 10, 5),
                                       search='NF-e', sort='Valor', sort_direction=-1))
        self.run.assert_not_called()
        restored = bi.get_view(saved['id'])
        self.assertEqual(restored['config']['query']['mapping'], MAPPING)
        self.assertEqual(restored['config']['drill_company'], 'Filial 2')
        self.assertEqual(restored['config']['drill_day'], '2026-10-05')
        self.assertEqual(restored['config']['sort'], 'Valor')
        self.assertEqual(restored['config']['search'], 'NF-e')
        self.assertEqual(restored['config']['sort_direction'], -1)
        self.assertEqual(len(bi.views()['views']), 1)

    def test_saved_view_survives_deletion_of_named_model(self):
        schema = {'sampled': 30, 'fields': [{'name': field} for field in MAPPING.values() if field != '@item_total'],
                  'computed': [{'name': '@item_total'}]}
        with patch.object(app, 'schema', return_value=schema):
            model = bi.save_model(bi.ModelSave(name='Vendas', mapping=MAPPING))
        req = bi.BIQuery(model_id=model['id'], start=date(2026, 10, 1), end=date(2026, 10, 29))
        saved = bi.save_view(bi.ViewSave(name='Acompanhamento', query=req, period_preset='month'))
        bi.delete_model(model['id'])
        restored = bi.get_view(saved['id'])
        self.assertIsNone(restored['config']['query']['model_id'])
        self.assertEqual(restored['config']['query']['mapping'], MAPPING)
        self.assertEqual(restored['config']['query']['end'], str(date.today()))
        bi.BIQuery(**restored['config']['query'])

    def test_saved_views_are_bound_to_source_and_deletion_preserves_models(self):
        saved = bi.save_view(bi.ViewSave(name='Acompanhamento', query=query()))
        with patch.object(app, 'MONGO_URI', 'mongodb://different-source:12220'):
            self.assertFalse(bi.views()['views'][0]['available'])
            with self.assertRaises(HTTPException) as error:
                bi.get_view(saved['id'])
            self.assertEqual(error.exception.status_code, 409)
        self.assertNotIn('mongodb://', str(saved))
        bi.delete_view(saved['id'])
        self.assertEqual(bi.views()['views'], [])
        with self.assertRaises(HTTPException):
            bi.get_view(saved['id'])

    def test_invalid_saved_view_cannot_create_ambiguous_dynamic_day(self):
        for extra in [{'period_preset': 'month', 'drill_day': date(2026, 10, 5)},
                      {'drill_day': date(2026, 9, 30)}, {'name': '  '},
                      {'sort': '$where'}, {'limit': 100000}, {'search': 'x' * 201}]:
            with self.subTest(extra=extra), self.assertRaises(ValidationError):
                bi.ViewSave(**{'name': 'Visão', 'query': query(), **extra})
        self.assertEqual(bi.views()['views'], [])

    def test_document_drill_sums_embedded_amounts_without_mutating_raw_result(self):
        raw = {'rows': [{'Numero': 1, 'Valor': [1.1, '2.20', None]}, {'Numero': 2, 'Valor': []}], 'documents': 2}
        self.run.return_value = raw
        self.run.side_effect = None
        request = bi.ExploreQuery(mapping={**MAPPING, 'value_field': 'ItensBase[].TotalLiquido'},
                                  start=date(2026, 10, 1), end=date(2026, 10, 1), dimension='documents')
        data = bi.explore(request)
        self.assertEqual([row['Valor'] for row in data['rows']], [3.3, 0.0])
        self.assertEqual(raw['rows'][0]['Valor'], [1.1, '2.20', None])

    def test_invalid_embedded_amount_is_not_silently_exposed_as_zero(self):
        self.run.return_value = {'rows': [{'Valor': ['not-a-number']}], 'documents': 1}
        self.run.side_effect = None
        request = bi.ExploreQuery(mapping={**MAPPING, 'value_field': 'ItensBase[].TotalLiquido'},
                                  start=date(2026, 10, 1), end=date(2026, 10, 1), dimension='documents')
        with self.assertRaises(HTTPException) as error:
            bi.explore(request)
        self.assertEqual(error.exception.status_code, 422)


if __name__ == '__main__':
    unittest.main()
