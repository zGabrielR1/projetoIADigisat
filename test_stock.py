"""Synthetic lot balances, boundary dates, bounded reads and snapshot exports."""
from copy import deepcopy
from datetime import date, datetime
from decimal import Decimal
import csv
import io
import os
from pathlib import Path
import tempfile
import unittest
from unittest.mock import MagicMock, patch

from bson import ObjectId, Decimal128
from fastapi import HTTPException
from pydantic import ValidationError

import insight_stock as stock
import insight_store as store
from insight_exports import ExportRequest, start_export

COMPANY = str(ObjectId())
LOT = ObjectId()


def record(day=28, quantities=(10, -3)):
    return {'link_id': ObjectId(), 'product': {'Descricao': 'Produto A', 'CodigoInterno': '001', 'UnidadeMedida': {'Sigla': 'UN'}},
            'stock': {'Itens': [{'_id': LOT, '_t': ['ItemEstoque', 'ItemEstoqueLote'], 'Numero': 'L-1', 'DataValidade': datetime(2026, 10, day)}],
                      'Quantidades': [{'EstoqueReferencia': LOT, 'Quantidade': q} for q in quantities]}}


def request(**changes):
    return stock.ExpiryRequest(**{'company_id': COMPANY, 'reference_date': date(2026, 10, 27), **changes})


class LotRules(unittest.TestCase):
    def test_signed_physical_balance_and_other_lots(self):
        row = record(quantities=(Decimal128('10.5'), -3, Decimal('0.25')))
        row['stock']['Quantidades'].append({'EstoqueReferencia': ObjectId(), 'Quantidade': 999})
        row['stock']['QuantidadesReservadas'] = [{'EstoqueReferencia': LOT, 'Quantidade': 4}]
        result = stock.stock_rows([row], request())
        self.assertEqual(result[0]['Quantidade'], 7.75)
        self.assertEqual(result[0]['Unidade'], 'UN')

    def test_reference_day_is_excluded_without_expired(self):
        self.assertEqual(stock.stock_rows([record(27)], request()), [])
        self.assertEqual(stock.stock_rows([record(26)], request()), [])
        self.assertEqual(len(stock.stock_rows([record(28)], request(horizon_days=1))), 1)
        self.assertEqual(stock.stock_rows([record(29)], request(horizon_days=1)), [])

    def test_include_expired_also_includes_today_and_future(self):
        rows = stock.stock_rows([record(28), record(27), record(26)], request(include_expired=True))
        self.assertEqual([r['SituacaoLote'] for r in rows], ['Vencido', 'Vence hoje', 'A vencer'])
        self.assertEqual([r['DiasParaVencer'] for r in rows], [-1, 0, 1])

    def test_zero_and_negative_balances_are_optional(self):
        for quantities in [(), (2, -2), (1, -2)]:
            with self.subTest(quantities=quantities):
                self.assertEqual(stock.stock_rows([record(quantities=quantities)], request()), [])
                self.assertEqual(len(stock.stock_rows([record(quantities=quantities)], request(positive_only=False))), 1)

    def test_search_is_case_insensitive_literal_text(self):
        for text in ['produto a', '001', 'l-1']:
            self.assertEqual(len(stock.stock_rows([record()], request(search=text))), 1)
        for text in ['.*', '$where', 'no match']:
            self.assertEqual(stock.stock_rows([record()], request(search=text)), [])

    def test_invalid_quantities_dates_and_unsupported_variants_fail_closed(self):
        for quantity in ['bad', float('nan'), float('inf'), None]:
            with self.subTest(quantity=quantity), self.assertRaises(HTTPException):
                stock.stock_rows([record(quantities=(quantity,))], request())
        broken = record(); broken['stock']['Itens'][0]['DataValidade'] = None
        with self.assertRaises(HTTPException): stock.stock_rows([broken], request())
        for variant in ['ItemEstoqueLoteGrade', 'ItemEstoqueLoteSerial']:
            broken = record(); broken['stock']['Itens'][0]['_t'] = [variant]
            with self.assertRaises(HTTPException): stock.stock_rows([broken], request())

    def test_bounds_prevent_partial_results(self):
        with patch.object(stock, 'MAX_LOTS', 1), self.assertRaises(HTTPException):
            stock.stock_rows([record(), record()], request())
        with self.assertRaises(HTTPException): stock.stock_rows([record()], request(), deadline=0)

    def test_operator_company_and_unknown_fields_rejected(self):
        for changes in [{'company_id': '$where'}, {'company_id': {'$ne': ''}}, {'horizon_days': -1},
                        {'horizon_days': 367}, {'page_size': 201}, {'offset': -1}, {'collection': 'Pessoas'}]:
            with self.subTest(changes=changes), self.assertRaises(ValidationError): request(**changes)


class StockSnapshots(unittest.TestCase):
    def setUp(self):
        self.folder = tempfile.TemporaryDirectory()
        self.env = patch.dict(os.environ, {'INSIGHT_DATA_DIR': self.folder.name, 'INSIGHT_SCHEDULER': '0'})
        self.env.start()

    def tearDown(self):
        self.env.stop(); self.folder.cleanup()

    def test_pagination_and_snapshot_exports_preserve_original_page(self):
        import app
        records = [record(28), record(29), record(30)]
        cursor = MagicMock(); cursor.__iter__.return_value = iter(records)
        db = MagicMock(); db.list_collection_names.return_value = ['Estoques', 'ProdutosServicosEmpresa', 'ProdutosServicos']
        db.Estoques.aggregate.return_value = cursor
        client = MagicMock(); client.__enter__.return_value.__getitem__.return_value = db
        with patch.object(app, 'mongo_client', return_value=client):
            response = stock.query_expiry(request(page_size=1, offset=1))
        result = response['result']
        self.assertEqual(result['total_lots'], 3)
        self.assertEqual(result['rows'][0]['Validade'], '2026-10-29')
        self.assertTrue(result['pagination']['has_more'])
        self.assertEqual(store.get_run(response['run_id'])['result'], result)
        kwargs = db.Estoques.aggregate.call_args.kwargs
        self.assertFalse(kwargs['allowDiskUse']); self.assertEqual(kwargs['maxTimeMS'], 20000)
        cursor.close.assert_called_once()
        with self.assertRaises(HTTPException) as blocked:
            start_export(ExportRequest(run_id=response['run_id'], complete=True))
        self.assertEqual(blocked.exception.status_code, 422)
        from insight_exports import build_export, put_job, get_job
        from openpyxl import load_workbook
        for format in ['csv', 'xlsx']:
            key = store.identifier()
            put_job(key, {'id':key, 'status':'queued', 'records':0})
            build_export(key, ExportRequest(run_id=response['run_id'], format=format), store.get_run(response['run_id']))
            job = get_job(key)
            self.assertEqual(job['status'], 'succeeded', job)
            self.assertEqual(job['records'], 1)
            path = Path(self.folder.name)/'exports'/job['filename']
            if format == 'xlsx':
                book = load_workbook(path, read_only=True)
                try:
                    sheet = book['Base - lotes']; values = list(sheet.values)
                    self.assertIn('2026-10-29', values[1]); self.assertNotIn('2026-10-28', values[1])
                finally: book.close()
            else:
                text = path.read_text(encoding='utf-8-sig')
                # Metadata contains the real export date, which can equal a lot
                # date. Check the exported lot rows rather than that header.
                table = text[text.index('Codigo;Produto;Lote;Validade;'):]
                rows = list(csv.DictReader(io.StringIO(table), delimiter=';'))
                self.assertEqual(len(rows), 1)
                self.assertEqual(rows[0]['Validade'], '2026-10-29')


if __name__ == '__main__':
    unittest.main()
