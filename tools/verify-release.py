"""Exercise the shipped PE with a minimal PATH and isolated persistent data."""
from pathlib import Path
from datetime import date, datetime, timedelta
import base64
from contextlib import closing
import hashlib
import json
import os
import secrets
import socket
import sqlite3
import subprocess
import time
import urllib.request
import urllib.error
import sys
import shutil

from dotenv import dotenv_values
from openpyxl import load_workbook
from pypdf import PdfReader

ROOT=Path(__file__).resolve().parent.parent
sys.path.insert(0,str(ROOT))
from insight_version import VERSION
BUILD=Path((ROOT/'.release-build/latest-build-path').read_text(encoding='utf-8'))
EXE=BUILD/'dist/DigiSatInsight.exe'
QA=BUILD/'verification'
QA.mkdir(exist_ok=True)
QA_DATA=QA/('data-'+secrets.token_hex(6))
environment=os.environ.copy()
configuration=dotenv_values(ROOT/'.env')
for name in ['MONGO_URI','MONGO_DATABASE','MONGO_USERNAME','MONGO_AUTH_SOURCE','MONGO_PASSWORD_FILE','DIGISAT_MODE','BACKUP_MONGO_URI']:
    if configuration.get(name) is not None:environment[name]=configuration[name]
password_file=environment.get('MONGO_PASSWORD_FILE','')
if password_file and not Path(password_file).is_absolute():environment['MONGO_PASSWORD_FILE']=str(ROOT/password_file)
environment.update(INSIGHT_DATA_DIR=str(QA_DATA),INSIGHT_SCHEDULER='0',INSIGHT_HOST='127.0.0.1',
                   INSIGHT_USER='release-qa',INSIGHT_PASSWORD=secrets.token_urlsafe(24),INSIGHT_REQUIRE_READ_ONLY='0',
                   AI_ENDPOINT='',AI_MODEL='',AI_API_KEY='',PATH=str(Path(os.environ['WINDIR'])/'System32'),PYTHONUTF8='1')
for name in ['PYTHONPATH','PYTHONHOME','VIRTUAL_ENV']:environment.pop(name,None)
with socket.socket() as sock:
    sock.bind(('127.0.0.1',0));port=sock.getsockname()[1]
BASE=f'http://127.0.0.1:{port}'
AUTH='Basic '+base64.b64encode((environment['INSIGHT_USER']+':'+environment['INSIGHT_PASSWORD']).encode()).decode()
log=(QA/'server.log').open('w',encoding='utf-8')
process=None


def request(path,body=None,authenticated=True,raw=False,method=None):
    headers={'Content-Type':'application/json'}
    if authenticated:headers['Authorization']=AUTH
    req=urllib.request.Request(BASE+path,data=json.dumps(body).encode() if body is not None else None,headers=headers,method=method)
    try:
        with urllib.request.urlopen(req,timeout=60) as response:
            value=response.read()
            return response.status,value if raw else json.loads(value)
    except urllib.error.HTTPError as exc:
        return exc.code,json.loads(exc.read())


def ok(path,body=None):
    status,value=request(path,body)
    assert status==200,(path,status,value)
    return value


def start():
    global process
    process=subprocess.Popen([str(EXE),'--no-browser','--port',str(port)],cwd=QA,env=environment,
                             stdout=log,stderr=subprocess.STDOUT,creationflags=subprocess.CREATE_NO_WINDOW)
    deadline=time.monotonic()+120
    while time.monotonic()<deadline:
        if process.poll() is not None:raise AssertionError('Executable exited; inspect verification/server.log')
        try:
            if request('/api/status')[0]==200:return
        except OSError:pass
        time.sleep(.25)
    raise AssertionError('Executable startup timed out')


def stop():
    escaped=str(EXE).replace("'","''")
    command=f"Get-Process DigiSatInsight -ErrorAction SilentlyContinue | Where-Object {{ $_.Path -eq '{escaped}' }} | Stop-Process -Force"
    subprocess.run([str(Path(os.environ['WINDIR'])/'System32/WindowsPowerShell/v1.0/powershell.exe'),'-NoProfile','-Command',command],capture_output=True)
    if process:
        try:process.wait(timeout=10)
        except subprocess.TimeoutExpired:process.terminate()


report={}
try:
    regression=subprocess.run([sys.executable,'-m','unittest','test_stability','test_live_workspace','-q'],cwd=ROOT,
        stdout=log,stderr=subprocess.STDOUT,timeout=60)
    assert regression.returncode==0,'Stability regression tests failed; inspect verification/server.log'
    finance_environment=os.environ.copy()
    finance_environment['INSIGHT_FINANCE_REQUIRE_MONGO']='1'
    finance_checks=subprocess.run([sys.executable,'-m','unittest','discover','-s','tests','-p','test_finance*.py','-q'],
        cwd=ROOT,env=finance_environment,stdout=log,stderr=subprocess.STDOUT,timeout=180)
    assert finance_checks.returncode==0,'Finance rules or isolated MongoDB checks failed; inspect verification/server.log'
    for pattern in ['test_native.py','test_report_catalog.py','test_report_search.py','test_smart_filters.py']:
        checked=subprocess.run([sys.executable,'-m','unittest','discover','-s','tests','-p',pattern,'-q'],cwd=ROOT,stdout=log,stderr=subprocess.STDOUT,timeout=90)
        assert checked.returncode==0,pattern+' failed; inspect verification/server.log'
    # CRUD is verified only against the test suite's OWN authenticated mongod,
    # new dbpath and random loopback port. Never send writes to the DigiSat base.
    editor_environment=os.environ.copy()
    editor_environment['INSIGHT_EDITOR_TEST_EXE']=str(EXE)
    editor_checks=subprocess.run([sys.executable,'-m','unittest','test_editor','-q'],cwd=ROOT,
        env=editor_environment,stdout=log,stderr=subprocess.STDOUT,timeout=600)
    assert editor_checks.returncode==0,'Packaged editor CRUD tests failed; inspect verification/server.log'
    node_path=shutil.which('node')
    assert node_path,'Node.js is required for the page-export JavaScript verification'
    export_environment=os.environ.copy()
    export_environment['INSIGHT_EDITOR_EXPORT_JS']=str(BUILD/'static/editor-data.js')
    export_checks=subprocess.run([node_path,str(ROOT/'tests/editor-data.test.cjs')],cwd=ROOT,
        env=export_environment,stdout=log,stderr=subprocess.STDOUT,timeout=30)
    assert export_checks.returncode==0,'Packaged page-export serializer failed'
    version=subprocess.run([str(EXE),'--version'],env=environment,capture_output=True,text=True,timeout=90)
    assert version.returncode==0 and VERSION in version.stdout
    start()
    assert request('/api/workspace/profiles',authenticated=False)[0]==401
    assert request('/api/editor/databases',authenticated=False)[0]==401
    for editor_asset in ['/database-editor','/static/editor.js','/static/editor.css','/static/editor-data.js']:
        assert request(editor_asset,raw=True)[0]==200
    for asset in ['editor.js','editor.css','editor-data.js']:
        assert hashlib.sha256(request('/static/'+asset,raw=True)[1]).digest()==hashlib.sha256((BUILD/'static'/asset).read_bytes()).digest()
    assert request('/api/finance/reports',authenticated=False)[0]==401
    assert request('/api/native/catalog',authenticated=False)[0]==401
    native_metadata=ok('/api/native/catalog')
    assert len(native_metadata['reports'])==358
    assert native_metadata['available']==356
    for assembly in native_metadata['installed_assemblies']:
        assembly_root=Path(r'C:\Digisat\SuiteG6\Sistema')
        path=assembly_root/('Modulos' if assembly['name']=='Modulo.Relatorios' else '')/(assembly['name']+'.dll')
        assert hashlib.sha256(path.read_bytes()).hexdigest()==assembly['sha256'],'Installed native contract changed: '+assembly['name']
    assert request('/api/native/search',{'question':'contas a pagar em aberto'},authenticated=False)[0]==401
    native_search=ok('/api/native/search',{'question':'contas a pagar em aberto deste mês'})
    assert native_search['matches'][0]['id']=='Pagamento.PagamentosEmAberto'
    assert native_search['execution']=='selection_required'
    smart_match=ok('/api/native/search',{'question':'contas a receber atrasadas deste mês'})['matches'][0]
    smart=smart_match['proposed_filters']
    proposed={s['name']:s['value'] for s in smart['suggestions']}
    assert proposed['situacao']['Codigo']==4 and proposed['tipoFiltroData']['Codigo']==2
    preview=ok('/api/native/preview',{'report_id':smart_match['id'],'call_index':smart['call_index'],'arguments':proposed})
    assert preview['executed'] is False
    assert request('/api/native/preview',{'report_id':smart_match['id'],'arguments':{'contaReferencia':'0'*24}})[0]==422
    native_chat=ok('/api/workspace/conversation',{'message':'cadastro de CFOP'})
    assert native_chat['status']=='native_reports' and native_chat['native_reports']['matches'][0]['id']=='OperacaoFiscal.ListaCadastroCfop'
    assert request('/api/native/query',{'report_id':'OperacaoFiscal.ListaCadastroCfop'})[0]==401
    for native_asset in ['native.js','native.css','native-service-map.json','rc2.css','rc3.css','workspace.js']:
        assert hashlib.sha256(request('/static/'+native_asset,raw=True)[1]).digest()==hashlib.sha256((BUILD/'static'/native_asset).read_bytes()).digest()
    assert request('/native-reports',raw=True)[0]==200
    assert (BUILD/'native/NativeReportHost.exe').is_file()
    from insight_finance import SPECS as finance_specs, FinanceRequest, execute as execute_finance
    finance_metadata=ok('/api/finance/reports')
    assert {r['id'] for r in finance_metadata['reports']}==set(finance_specs)
    assert request('/api/finance/query',{'report_id':'$where'})[0]==422
    for asset in ['/reports','/finance-reports','/static/finance.js','/static/finance.css','/static/report-catalog.json']:
        assert request(asset,raw=True)[0]==200
    for asset in ['finance.js','finance.css','report-catalog.json']:
        assert hashlib.sha256(request('/static/'+asset,raw=True)[1]).digest()==hashlib.sha256((BUILD/'static'/asset).read_bytes()).digest()
    status=ok('/api/status');assert status['connected'] and status['version']==VERSION
    import app
    finance_query_verified=False
    with app.mongo_client() as source:
        source_db=source[app.MONGO_DATABASE]
        dated_finance=source_db.Recebimentos.find_one({'Ativo':True,'Vencimento':{'$type':9}},sort=[('Vencimento',-1)])
    if dated_finance:
        finance_end=dated_finance['Vencimento'].date()
        finance_req=FinanceRequest(report_id='Recebimento.PagamentosRecebimentos',
            start=finance_end.replace(day=1),end=finance_end,consolidated=True,page_size=2)
        source_finance=execute_finance(finance_req)
        packaged_finance=ok('/api/finance/query',finance_req.model_dump(mode='json'))
        assert packaged_finance['record_count']==source_finance['record_count']
        assert packaged_finance['totals']==source_finance['totals']
        finance_query_verified=True
    assert request('/api/workspace/stock/companies',authenticated=False)[0]==401
    stock_companies=ok('/api/workspace/stock/companies')['companies']
    assert stock_companies,'Packaged stock verification requires a product company'
    stock_result=ok('/api/workspace/stock/expiry',{'company_id':stock_companies[0]['id'],
        'include_expired':True,'positive_only':False,'horizon_days':366,'page_size':2})
    assert stock_result['result']['module']=='stock_expiry'
    assert stock_result['result']['total_lots']>=len(stock_result['result']['rows'])
    assert request('/api/workspace/stock/expiry',{'company_id':'$where'})[0]==422
    assert request('/api/workspace/exports',{'run_id':stock_result['run_id'],'complete':True})[0]==422
    assert 'password' not in ok('/api/workspace/connection')
    # Discover a populated month; no fixed customer identities, dates or totals.
    import app
    from test_e2e_reports import scope_match, item_rows, payment_total
    with app.mongo_client() as source:
        db=source[app.MONGO_DATABASE]
        latest=db.Movimentacoes.find_one({'DataHoraEmissao':{'$type':9},'_t':{'$in':app.MOVEMENT_GROUPS['all_sales']},'Situacao.Descricao':{'$nin':app.STATUS_EXCLUDED}},sort=[('DataHoraEmissao',-1)])
        assert latest,'Verification requires at least one dated sale in Movimentacoes'
        period_end=latest['DataHoraEmissao'].date();period_start=period_end.replace(day=1)
        match=scope_match(period_start,period_end)
        expected_documents=db.Movimentacoes.count_documents(match,maxTimeMS=5000)
        expected_items=round(sum(float(row['total'] or 0) for row in item_rows(db,match)),2)
        expected_paid=payment_total(db,match)
        native_match=scope_match(period_start,period_end,group='nfce');native_match.update({'Situacao.Descricao':'Concluído','FinalidadeEmissaoNfe.Codigo':1})
        expected_native=round(sum(float(row['total'] or 0) for row in item_rows(db,native_match)),2)
        profile_totals={}
        for label,completed_only in [('fiscal',False),('completed',True)]:
            scoped=scope_match(period_start,period_end,group='fiscal_sales')
            if completed_only:scoped['Situacao.Descricao']='Concluído'
            profile_totals[label]=round(sum(float(row['total'] or 0) for row in item_rows(db,scoped)),2)
    filters={'start':str(period_start),'end':str(period_end),'kind':'investigation','page_size':10}
    example=ok('/api/workspace/conversation',{'action':'refine','filters':filters})
    assert example['result']['documents']==expected_documents
    assert abs(example['result']['totals']['Itens']-expected_items)<.011
    assert abs(example['result']['totals']['Pagamentos']-expected_paid)<.011
    conversation_id=example['conversation_id']
    follow=ok('/api/workspace/conversation',{'conversation_id':conversation_id,'message':'E no dia seguinte?'})
    assert follow['plan']['start']==str(period_start+timedelta(days=1))
    compared=ok('/api/workspace/conversation',{'conversation_id':conversation_id,'message':'Compare com o dia anterior'})
    assert compared['plan']['comparison']['seller_query']==follow['plan']['seller_query']
    initial=ok('/api/workspace/conversation',{'message':'E ontem?'})
    assert initial['status']=='clarification'
    answered=ok('/api/workspace/conversation',{'conversation_id':initial['conversation_id'],'answer':'documents'})
    assert answered['status']=='result'
    pending=ok('/api/workspace/conversation',{'message':'O mesmo vendedor naquele dia'})
    assert pending['clarification']['field']=='seller_query'
    pending=ok('/api/workspace/conversation',{'conversation_id':pending['conversation_id'],'answer':'2'})
    assert pending['clarification']['field']=='period'
    status,bad=request('/api/workspace/conversation',{'message':'investigue em 05/10/2026','filters':{'company_field':'$where','company_value':'return true'}})
    assert status==400
    paged=ok('/api/workspace/conversation',{'conversation_id':example['conversation_id'],'action':'refine','filters':{
        **example['plan'],'page_size':1,'offset':1,'comparison':None}})
    assert len(paged['result']['rows'])==min(1,max(0,expected_documents-1))
    assert paged['result']['totals']==example['result']['totals']
    saved=ok('/api/workspace/saved',{'title':'Packaged verification','run_id':example['run_id']})
    saved_path='/api/workspace/saved/'+saved['id']
    renamed=request(saved_path,{'title':'  RC3 conferência  '},method='PATCH')
    assert renamed[0]==200 and renamed[1]['title']=='RC3 conferência'
    assert request(saved_path,{'title':'   '},method='PATCH')[0]==422
    assert request('/api/workspace/saved/missing',{'title':'Missing'},method='PATCH')[0]==404
    resumed=ok('/api/workspace/conversation/resume',{'run_id':saved['run_id']})
    assert resumed['result']['provenance']['source']=='saved_snapshot'
    quality=ok('/api/workspace/conversation',{'action':'refine','filters':{**filters,'kind':'quality','profile':'native_nfce'}})
    assert quality['plan']['kind']=='quality'
    assert abs(quality['result']['totals']['Itens']-expected_native)<.011
    assert all(v>=0 for v in quality['result']['quality']['counts'].values())
    fiscal=ok('/api/workspace/conversation',{'action':'refine','filters':{**filters,'profile':'fiscal_valid'}})
    completed=ok('/api/workspace/conversation',{'action':'refine','filters':{**filters,'profile':'fiscal_valid','status_filter':'completed'}})
    assert abs(fiscal['result']['totals']['Itens']-profile_totals['fiscal'])<.011
    assert abs(completed['result']['totals']['Itens']-profile_totals['completed'])<.011
    for fmt in ['xlsx','pdf','csv']:
        job=ok('/api/workspace/exports',{'run_id':paged['run_id'],'format':fmt,'complete':True})
        deadline=time.monotonic()+60
        while time.monotonic()<deadline:
            progress=ok('/api/workspace/exports/'+job['id'])
            if progress['status'] in {'succeeded','failed'}:break
            time.sleep(.15)
        assert progress['status']=='succeeded',progress
        assert progress['records']==expected_documents
        response,content=request(progress['download'],raw=True)
        assert response==200
        output=QA/('evidence.'+fmt);output.write_bytes(content)
        if fmt=='xlsx':
            book=load_workbook(output,read_only=True,data_only=True);rows=list(book['Base - vendas'].values)
            assert len(rows)==expected_documents+1
            index=rows[0].index('Itens do documento')
            assert abs(sum(float(row[index]) for row in rows[1:])-expected_items)<.011
            book.close()
        if fmt=='pdf':
            text='\n'.join(page.extract_text() for page in PdfReader(output).pages)
            assert text.strip() and '-0,00' not in text
            assert str(example['result']['rows'][0]['Numero']) in text
    schedule=ok('/api/workspace/schedules',{'run_id':example['run_id'],'title':'Verification','period_policy':'fixed','metric':'Diferenca','threshold':0})
    assert not schedule['enabled']
    run=ok('/api/workspace/schedules/'+schedule['id']+'/run',{})
    assert run['last_status']=='succeeded'
    assert abs(run['last_value']-abs(example['result']['totals']['Diferenca']))<.011
    ok('/api/workspace/schedules/'+schedule['id']+'/run',{})
    assert len(ok('/api/workspace/alerts')['alerts'])==int(run['last_value']>0)
    backup_status,backup=request('/api/workspace/state/backup',{},raw=True)
    assert backup_status==200
    (QA/'backup.sqlite3').write_bytes(backup)
    with closing(sqlite3.connect(QA/'backup.sqlite3')) as db:assert db.execute('PRAGMA integrity_check').fetchone()[0]=='ok'
    duplicate=subprocess.run([str(EXE),'--no-browser','--port',str(port)],env=environment,capture_output=True,timeout=90)
    assert duplicate.returncode==0
    stop()
    environment['INSIGHT_REQUIRE_READ_ONLY']='1'
    start()
    blocked=ok('/api/status')
    from insight_connection import privileges
    with app.mongo_client() as source: broad=not privileges(source)['verified_read_only']
    assert blocked['connected']==(not broad),'Strict read-only mode must follow actual account grants'
    restored=ok('/api/workspace/conversations/'+pending['conversation_id'])
    assert restored['pending']['clarification']['field']=='period'
    persisted=ok('/api/workspace/saved')['saved']
    assert any(row['id']==saved['id'] and row['title']=='RC3 conferência' and row['run_id']==example['run_id'] for row in persisted)
    report={'dataset_discovery':True,'reference_period':{'start':str(period_start),'end':str(period_end)},'reference_documents':expected_documents,'version':VERSION,'minimal_path':True,'authentication':True,'operator_fields_rejected':True,
            'followups':True,'clarifications':True,'restart_persistence':True,'native_quality':True,
            'paging_and_complete_exports':True,'pdf_excel_csv':True,'schedule_alert_deduplication':True,'profile_status_intersection':True,
            'sqlite_backup_integrity':True,'strict_read_only_rejects_broad_account':True,
            'stability_regressions':True,'saved_rename_persistence':True,'stock_expiry':True,
            'database_editor_write_crud':True,'database_editor_query_modes':True,'database_editor_history':True,
            'database_editor_collection_create':True,'database_editor_schema':True,
            'database_editor_saved_queries':True,'database_editor_page_export':True,
            'finance_isolated_rules':True,'finance_data_views':len(finance_specs),'finance_live_query':finance_query_verified,
            'native_service_entries':356,'native_contract_and_argument_tests':True,'native_assets_and_authentication':True,
            'native_natural_language_discovery':True,'native_discovery_in_conversation':True,
            'native_smart_filter_preview':True,'native_preset_reference_injection_rejected':True,
            'exe_sha256':hashlib.sha256(EXE.read_bytes()).hexdigest()}
    (QA/'verification.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
    print(json.dumps(report,indent=2),flush=True)
finally:
    stop();log.close()
