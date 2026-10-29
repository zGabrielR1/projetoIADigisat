"""Real MongoDB 3.4 integration on a new disposable loopback instance only."""
from datetime import date
import copy
import os
from pathlib import Path
import socket
import subprocess
import tempfile
import time
import unittest
from unittest.mock import patch

from bson import ObjectId, Decimal128
from fastapi import HTTPException
from pymongo import MongoClient
from pymongo.errors import AutoReconnect, ConnectionFailure

import app
import insight_finance as f
from test_finance import parcel, COMPANY, PERSON, ACCOUNT

ROOT = Path(__file__).resolve().parents[1]


class FinanceIsolatedMongo(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        binary=Path(os.environ.get('INSIGHT_TEST_MONGOD', r'C:\Digisat\SuiteG6\MongoDB\bin\mongod.exe'))
        if not binary.is_file():
            if os.environ.get('INSIGHT_FINANCE_REQUIRE_MONGO') == '1':
                raise RuntimeError('Release finance verification requires an isolated mongod')
            raise unittest.SkipTest('Finance integration requires a local test mongod.')
        cls.build=ROOT/'.release-build';cls.build.mkdir(exist_ok=True)
        cls.folder=tempfile.TemporaryDirectory(prefix='finance-isolated-',dir=cls.build)
        cls.path=Path(cls.folder.name).resolve()
        if not cls.path.is_relative_to(cls.build.resolve()):
            raise RuntimeError('Isolated Mongo path escaped workspace')
        with socket.socket() as sock:
            sock.bind(('127.0.0.1',0));cls.port=sock.getsockname()[1]
        if cls.port in {12220,12231}:
            raise RuntimeError('Refusing a SuiteG6 database port')
        cls.uri=f'mongodb://127.0.0.1:{cls.port}/'
        cls.client=None;cls.process=None
        try:
            cls.process=subprocess.Popen([str(binary),'--dbpath',str(cls.path),'--port',str(cls.port),
                '--bind_ip','127.0.0.1','--wiredTigerCacheSizeGB','0.25','--nojournal',
                '--logpath',str(cls.path/'mongo.log')],stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL,
                creationflags=subprocess.CREATE_NO_WINDOW if os.name=='nt' else 0)
            cls.client=MongoClient(cls.uri,serverSelectionTimeoutMS=300)
            deadline=time.monotonic()+60
            while True:
                try:
                    cls.client.admin.command('ping');break
                except ConnectionFailure:
                    if cls.process.poll() is not None or time.monotonic()>deadline:
                        raise RuntimeError('Isolated finance MongoDB did not start')
            actual=cls.client.admin.command('getCmdLineOpts')['parsed']['storage']['dbPath']
            if Path(actual).resolve()!=cls.path:
                raise RuntimeError('Refusing fixture writes: wrong Mongo storage path')
            cls.database='InsightFinanceFixture'
            cls.db=cls.client[cls.database]
            cls.other_company=ObjectId();cls.other_person=ObjectId();cls.seller=ObjectId();cls.other_seller=ObjectId();cls.consumer=ObjectId();cls.segment=ObjectId()
            cls.db.Pessoas.insert_many([
                {'_id':COMPANY,'Nome':'Empresa de teste'}, {'_id':cls.other_company,'Nome':'Empresa de teste'},
                {'_id':PERSON,'Nome':'Pessoa inativa com vendedor','Ativo':False,'Cliente':{'VendedorReferencia':cls.seller}},
                {'_id':cls.other_person,'Nome':'Pessoa segmentada','Cliente':{'VendedorReferencia':cls.other_seller},'Segmentacao':{'Tipo':{'_id':cls.segment}}},
                {'_id':cls.consumer,'_t':'Consumidor','Nome':'Consumidor','Cliente':{'VendedorReferencia':cls.seller}},
                {'_id':cls.seller,'Nome':'Vendedor'}])
            cls.db.Contas.insert_one({'_id':ACCOUNT,'Descricao':'Conta de teste'})
            cls.db.TiposSegmentacao.insert_one({'_id':cls.segment,'Descricao':'Segmento de teste'})
            def record(value, **opts):
                r=parcel(value=str(value),settled=opts.pop('settled',False));r['Ativo']=True;r['EmpresaReferencia']=COMPANY;r.update(opts)
                if r['Situacao']['Codigo']==3:
                    r['Historico'][-1]['Valor']=Decimal128(str(int(value)-20));r['Historico'][-1]['Desconto']=20
                return r
            a=record(100)
            b=record(200,settled=True)
            card=record(300,PessoaReferencia=cls.other_person,_t=['ParcelaRecebimento','ParcelaRecebimentoCartao'],
                        TaxaRede={'_t':'Debito','TipoTaxa':0,'PercentualTaxa':2},DescricaoRede='Rede B')
            other=record(400,EmpresaReferencia=cls.other_company,PessoaReferencia=cls.other_person)
            other['Historico'][0]['EmpresaReferencia']=cls.other_company
            inactive=record(500,Ativo=False)
            consumer=record(600,PessoaReferencia=cls.consumer)
            reversed_record=record(700,PessoaReferencia=cls.other_person)
            reversed_record['Situacao']['Codigo']=4
            reversed_record['Historico'].append({**reversed_record['Historico'][-1],'_t':'HistoricoEstornado','Valor':700})
            wallet=record(800,_t=['ParcelaRecebimento','ParcelaRecebimentoCarteiraDigital'],TaxaRede={'TipoTaxa':1,'PercentualTaxa':10})
            no_fee=record(50,_t=['ParcelaRecebimento','ParcelaRecebimentoCartao'],DescricaoRede='Rede A')
            no_fee['Historico'][-1]['EspeciePagamento']['TipoCartao']=1
            cls.db.Recebimentos.insert_many([a,b,card,other,inactive,consumer,reversed_record,wallet,no_fee])
            payment_segment=record(25,PessoaReferencia=cls.other_person)
            payments=copy.deepcopy([a,b,other,inactive,payment_segment])
            for p in payments:
                p['_id']=ObjectId();p['_t']='ParcelaPagamento'
            cls.db.Pagamentos.insert_many(payments)
        except Exception:
            cls.clean_up();raise

    @classmethod
    def clean_up(cls):
        if cls.client:
            try:cls.client.admin.command('shutdown',force=True)
            except (AutoReconnect,ConnectionFailure):pass
            cls.client.close()
        if cls.process:
            try:cls.process.wait(timeout=15)
            except subprocess.TimeoutExpired:cls.process.terminate();cls.process.wait(timeout=15)
        if Path(cls.folder.name).resolve().is_relative_to(cls.build.resolve()):
            cls.folder.cleanup()

    @classmethod
    def tearDownClass(cls):
        cls.clean_up()

    def setUp(self):
        self.connection=patch.object(app,'mongo_client',side_effect=lambda:MongoClient(self.uri,serverSelectionTimeoutMS=1000))
        self.database_patch=patch.object(app,'MONGO_DATABASE',self.database)
        self.connection.start();self.database_patch.start()
        self.addCleanup(self.connection.stop);self.addCleanup(self.database_patch.stop)

    def query(self,rid,**kw):
        data={'report_id':rid,'start':date(2026,1,1),'end':date(2026,1,31),'company_id':str(COMPANY),'page_size':200}
        data.update(kw)
        return f.execute(f.FinanceRequest(**data),reference_date=date(2026,2,1))

    def test_all_twenty_one_native_entries_have_working_data_queries(self):
        self.assertEqual(len(f.SPECS),21)
        for rid,spec in f.SPECS.items():
            with self.subTest(report=rid):
                kw={}
                if spec['global_scope']:kw['company_id']=None
                if spec['seller_filter']:kw['seller_id']=str(self.seller)
                if spec['segment_filter']:kw['segment_id']=str(self.segment)
                result=self.query(rid,**kw)
                self.assertGreater(result['record_count'],0)
                self.assertEqual(result['record_count'],len(result['rows']))
                self.assertTrue(result['provenance']['read_only'])

    def test_filter_totals_fees_and_pagination_are_independent_of_the_page(self):
        rid='Recebimento.PagamentosRecebimentos'
        first=self.query(rid,page_size=2)
        second=self.query(rid,page_size=2,offset=2)
        self.assertEqual(first['record_count'],7)
        self.assertEqual(first['totals']['value'],'2750.00')
        self.assertEqual(first['totals']['corrected'],'2734.00')
        self.assertEqual(first['totals']['fee'],'16.00')
        self.assertEqual(first['totals']['settled'],'180.00')
        self.assertEqual(first['totals'],second['totals'])
        self.assertFalse({r['id'] for r in first['rows']} & {r['id'] for r in second['rows']})

    def test_bi_analysis_uses_full_pending_set_and_person_drill(self):
        request=f.FinanceRequest(report_id='Recebimento.PagamentosRecebimentos',
            start=date(2026,1,1),end=date(2026,1,31),company_id=str(COMPANY),status='pending',page_size=1)
        result=f.execute(request,reference_date=date(2026,2,1),include_analysis=True)
        self.assertEqual(len(result['rows']),1)
        self.assertEqual(result['analysis']['count'],5)
        self.assertEqual(result['analysis']['total'],'1834.00')
        self.assertEqual(result['analysis']['overdue'],'1834.00')
        detail=f.execute(request,reference_date=date(2026,2,1),include_analysis=True,analysis_person_id=str(self.other_person))
        self.assertEqual(detail['record_count'],1)
        self.assertEqual(detail['analysis']['total'],'294.00')

    def test_bi_date_drill_pages_and_displayed_cent_totals(self):
        from insight_bi_finance import ParcelScope, parcels
        company=ObjectId()
        documents=[]
        for _ in range(55):
            row=parcel(value='1.005')
            row.update(Ativo=True,EmpresaReferencia=company)
            row['Historico'][0]['EmpresaReferencia']=company
            documents.append(row)
        ids=self.db.Recebimentos.insert_many(documents).inserted_ids
        try:
            scope=dict(start='2026-01-01',end='2026-01-31',side='receivable',
                       due_date='2026-01-01',company_id=str(company),person_id=str(PERSON))
            first=parcels(ParcelScope(**scope))
            second=parcels(ParcelScope(**scope,offset=50))
            self.assertEqual((len(first['rows']),len(second['rows'])),(50,5))
            self.assertEqual(first['record_count'],55)
            self.assertEqual(first['analysis']['total'],'55.55')
            self.assertEqual(second['analysis']['total'],'55.55')
            self.assertEqual((first['page_total'],second['page_total']),('50.50','5.05'))
            self.assertTrue(first['pagination']['has_more'])
            self.assertFalse(second['pagination']['has_more'])
            self.assertFalse({r['id'] for r in first['rows']} & {r['id'] for r in second['rows']})
        finally:
            self.db.Recebimentos.delete_many({'_id':{'$in':ids}})

    def test_seller_uses_customer_assignment_including_inactive_clients_but_excludes_consumer(self):
        result=self.query('Recebimento.RecebimentosPorVendedor',seller_id=str(self.seller))
        self.assertEqual(result['record_count'],4)
        self.assertEqual(result['totals']['value'],'1150.00')
        self.assertNotIn(str(self.consumer),{r['person_id'] for r in result['rows']})

    def test_segment_join_and_card_operation_postfilter(self):
        segment=self.query('Recebimento.RecebimentosPorSegmentacao',segment_id=str(self.segment))
        self.assertEqual(segment['record_count'],2)
        self.assertEqual(segment['totals']['value'],'1000.00')
        debit=self.query('Recebimento.RecebimentosCartao',operation='debit')
        self.assertEqual(debit['record_count'],1)
        self.assertEqual(debit['totals']['corrected'],'294.00')
        credit=self.query('Recebimento.RecebimentosCartao',operation='credit')
        self.assertEqual(credit['record_count'],1)
        self.assertEqual(credit['totals']['value'],'50.00')
        fee=self.query('Recebimento.RecebimentosTaxas',include_zero_fees=False)
        self.assertEqual(fee['record_count'],1)

    def test_consolidation_and_global_competence_do_not_mix_company_names(self):
        result=self.query('Recebimento.PagamentosRecebimentos',company_id=None,consolidated=True)
        self.assertEqual(result['record_count'],8)
        self.assertEqual(len(result['groups']),2)
        result=self.query('Recebimento.RecebimentosQuitadosPorDataCompetencia',company_id=None)
        self.assertEqual(result['record_count'],1)
        self.assertEqual(result['totals']['value'],'200.00')

    def test_guard_rejects_large_results_before_returning_partial_amounts(self):
        with patch.object(f,'MAX_PARCELS',1),self.assertRaises(HTTPException):
            self.query('Recebimento.PagamentosRecebimentos')


if __name__=='__main__':
    unittest.main()
