"""Native-service map, typed arguments, isolation, redaction, and hierarchy tests."""
from decimal import Decimal
import hashlib
import json
from pathlib import Path
import unittest
from unittest.mock import patch

from fastapi import HTTPException
import insight_native as n

ROOT=Path(__file__).resolve().parents[1]


class NativeTests(unittest.TestCase):
    def test_all_installed_report_entries_have_verified_contract_bindings(self):
        catalog=n.catalog()
        self.assertEqual(len(catalog['reports']),358)
        available={rid for rid,r in catalog['reports'].items() if any(c.get('available') for c in r['calls'])}
        self.assertEqual(len(available),356)
        self.assertEqual(set(catalog['reports'])-available,{'Pessoa.FichaCadastral','Pessoa.FichaCadastral40Colunas'})
        self.assertEqual(len(catalog['installed_assemblies']),4)
        self.assertTrue(catalog['report_type_inventory_complete'])
        self.assertEqual({rid for rid,r in catalog['reports'].items() if r['class_presence']=='absent'},set(catalog['reports'])-available)
        kinds={'text','number','boolean','datetime','array','object','enum'}
        for report in catalog['reports'].values():
            for call in report['calls']:
                self.assertTrue(call['available'])
                for parameter in call['parameters']:
                    self.assertIn(parameter['kind'],kinds)
                    for child in parameter.get('properties') or []:self.assertIn(child['kind'],kinds)

    def test_argument_types_reject_code_injection_and_nonfinite_numbers(self):
        for value,parameter in [({'$type':'Evil'},{'name':'filter','kind':'object','type':'SafeDto','properties':[]}),
            ('2026-10-28T00:00:00Z',{'name':'date','kind':'datetime'}),
            (float('inf'),{'name':'value','kind':'number','type':'System.Decimal'}),
            (True,{'name':'value','kind':'number','type':'System.Int32'}),
            ('true',{'name':'flag','kind':'boolean'})]:
            with self.subTest(value=value),self.assertRaises(HTTPException):n.validate_value(value,parameter)
        self.assertEqual(n.validate_value('12345678901234567890.12',{'name':'v','kind':'number','type':'System.Decimal'}),'12345678901234567890.12')

    def test_native_fixed_arguments_and_enum_choices_cannot_be_overridden(self):
        with self.assertRaises(HTTPException):n.validate_value(True,{'name':'consolidado','kind':'boolean','fixed':True,'default':False})
        with self.assertRaises(HTTPException):n.validate_value(99,{'name':'state','kind':'enum','type':'Enum','choices':[{'value':1,'label':'One'}]})
        self.assertEqual(n.validate_value(['1','2'],{'name':'cfops','kind':'array','type':'List<System.Int32>'}),[1,2])

    def test_recursive_redaction_keeps_business_fields_and_exact_decimals(self):
        value={'Pessoa':{'Nome':'Cliente','Senha':'secret','Api_Key':'key'},'Valor':Decimal('123.45'),
               'Documentos':[{'ChaveAcesso':'fiscal-access-key','TokenSessao':'secret','Numero':2}]}
        clean=n.sanitize(value)
        self.assertEqual(clean,{'Pessoa':{'Nome':'Cliente'},'Valor':'123.45','Documentos':[{'ChaveAcesso':'fiscal-access-key','Numero':2}]})

    def test_child_datasets_preserve_parent_company_context(self):
        values=n.datasets({'Total':30,'Empresas':[{'Nome':'A','Itens':[{'Valor':10}]},{'Nome':'B','Itens':[{'Valor':20}]}]})
        rows=values['$.Empresas[].Itens']
        self.assertEqual([r['record']['Valor'] for r in rows],[10,20])
        self.assertEqual([r['context']['$.Empresas[].Nome'] for r in rows],['A','B'])
        self.assertEqual(len(values['$']),1)

    def test_hierarchical_results_are_bounded_without_partial_rows(self):
        with patch.object(n,'MAX_ROWS',2),self.assertRaises(HTTPException):n.datasets([1,2,3])

    def test_connections_expire_and_only_opaque_tokens_leave_the_api(self):
        with patch.object(n,'host',return_value={'ok':True}),patch.object(n,'_sessions',{}):
            response=n.connect(n.ConnectRequest(username='fixture',password='local-fixture-secret',company_id='0'*24))
            self.assertNotIn('password',response)
            self.assertNotIn('local-fixture-secret',json.dumps(response))
            stored=n.get_session(response['session'])
            self.assertEqual(stored['username'],'fixture')
            key=hashlib.sha256(response['session'].encode()).hexdigest()
            n._sessions[key]['expires']=0
            with self.assertRaises(HTTPException):n.get_session(response['session'])

    def test_query_invokes_only_allowlisted_call_and_returns_a_page(self):
        fake={'ok':True,'data':[{'Codigo':i,'Valor':Decimal('10.20'),'Password':'hidden'} for i in range(4)]}
        with patch.object(n,'host',return_value=fake) as host,patch.object(n,'get_session',return_value={'username':'fixture','password':'fixture-secret','company_id':'0'*24,'port':12221}):
            response=n.query(n.NativeQuery(report_id='OperacaoFiscal.ListaCadastroCfop',page_size=2,offset=1),x_native_session='opaque')
            self.assertEqual([r['record']['Codigo'] for r in response['rows']],[1,2])
            self.assertEqual(response['pagination']['total'],4)
            self.assertTrue(response['pagination']['has_more'])
            self.assertNotIn('Password',response['rows'][0]['record'])
            self.assertEqual(response['rows'][0]['record']['Valor'],'10.20')
            payload=host.call_args.args[0]
            self.assertEqual(payload['report_id'],'OperacaoFiscal.ListaCadastroCfop')
            self.assertNotIn('method',payload)
            with self.assertRaises(HTTPException):n.query(n.NativeQuery(report_id='unknown'),x_native_session='opaque')


if __name__=='__main__':unittest.main()
