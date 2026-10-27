"""Exercise the shipped PE with a minimal PATH and isolated persistent data."""
from pathlib import Path
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
    deadline=time.monotonic()+60
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
    regression=subprocess.run([sys.executable,'-m','unittest','test_stability','-q'],cwd=ROOT,
        stdout=log,stderr=subprocess.STDOUT,timeout=60)
    assert regression.returncode==0,'Stability regression tests failed; inspect verification/server.log'
    # CRUD is verified only against the test suite's OWN authenticated mongod,
    # new dbpath and random loopback port. Never send writes to the DigiSat base.
    editor_environment=os.environ.copy()
    editor_environment['INSIGHT_EDITOR_TEST_EXE']=str(EXE)
    editor_checks=subprocess.run([sys.executable,'-m','unittest','test_editor','-q'],cwd=ROOT,
        env=editor_environment,stdout=log,stderr=subprocess.STDOUT,timeout=240)
    assert editor_checks.returncode==0,'Packaged editor CRUD tests failed; inspect verification/server.log'
    version=subprocess.run([str(EXE),'--version'],env=environment,capture_output=True,text=True,timeout=20)
    assert version.returncode==0 and VERSION in version.stdout
    start()
    assert request('/api/workspace/profiles',authenticated=False)[0]==401
    assert request('/api/editor/databases',authenticated=False)[0]==401
    for editor_asset in ['/database-editor','/static/editor.js','/static/editor.css']:
        assert request(editor_asset,raw=True)[0]==200
    status=ok('/api/status');assert status['connected'] and status['version']==VERSION
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
    example=ok('/api/workspace/conversation',{'message':'Existe uma diferença no vendedor 2 em 05/10/2026 de R$100,00'})
    assert example['result']['documents']==37
    assert example['result']['totals']['Outros']==104.96
    conversation_id=example['conversation_id']
    follow=ok('/api/workspace/conversation',{'conversation_id':conversation_id,'message':'E o vendedor 3?'})
    assert follow['plan']['start']=='2026-10-05' and follow['result']['totals']['Vendedor']==986.1
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
        **example['plan'],'page_size':10,'offset':10,'comparison':None}})
    assert len(paged['result']['rows'])==10 and paged['result']['totals']['Outros']==104.96
    saved=ok('/api/workspace/saved',{'title':'Packaged verification','run_id':example['run_id']})
    saved_path='/api/workspace/saved/'+saved['id']
    renamed=request(saved_path,{'title':'  RC3 conferência  '},method='PATCH')
    assert renamed[0]==200 and renamed[1]['title']=='RC3 conferência'
    assert request(saved_path,{'title':'   '},method='PATCH')[0]==422
    assert request('/api/workspace/saved/missing',{'title':'Missing'},method='PATCH')[0]==404
    resumed=ok('/api/workspace/conversation/resume',{'run_id':saved['run_id']})
    assert resumed['result']['provenance']['source']=='saved_snapshot'
    quality=ok('/api/workspace/conversation',{'action':'refine','filters':{'start':'2026-10-05','end':'2026-10-05','kind':'quality','profile':'native_nfce'}})
    assert quality['plan']['kind']=='quality' and quality['result']['quality']['counts']['shared']>=2
    fiscal=ok('/api/workspace/conversation',{'action':'refine','filters':{'start':'2026-10-01','end':'2026-10-26','kind':'investigation','profile':'fiscal_valid'}})
    completed=ok('/api/workspace/conversation',{'action':'refine','filters':{'start':'2026-10-01','end':'2026-10-26','kind':'investigation','profile':'fiscal_valid','status_filter':'completed'}})
    assert abs(fiscal['result']['totals']['Itens']-completed['result']['totals']['Itens']-97.93)<.001
    for fmt in ['xlsx','pdf','csv']:
        job=ok('/api/workspace/exports',{'run_id':paged['run_id'],'format':fmt,'complete':True})
        deadline=time.monotonic()+60
        while time.monotonic()<deadline:
            progress=ok('/api/workspace/exports/'+job['id'])
            if progress['status'] in {'succeeded','failed'}:break
            time.sleep(.15)
        assert progress['status']=='succeeded',progress
        assert progress['records']==37
        response,content=request(progress['download'],raw=True)
        assert response==200
        output=QA/('evidence.'+fmt);output.write_bytes(content)
        if fmt=='xlsx':
            book=load_workbook(output,read_only=True,data_only=True);rows=list(book['Base - vendas'].values)
            assert len(rows)==38
            index=rows[0].index('Outros/sem vendedor')
            assert abs(sum(float(row[index]) for row in rows[1:])-104.96)<.001
            book.close()
        if fmt=='pdf':
            text='\n'.join(page.extract_text() for page in PdfReader(output).pages)
            assert '104,96' in text and '217242' in text and '-0,00' not in text
    schedule=ok('/api/workspace/schedules',{'run_id':example['run_id'],'title':'Verification','period_policy':'fixed','metric':'Outros','threshold':100})
    assert not schedule['enabled']
    run=ok('/api/workspace/schedules/'+schedule['id']+'/run',{})
    assert run['last_status']=='succeeded' and run['last_value']==104.96
    ok('/api/workspace/schedules/'+schedule['id']+'/run',{})
    assert len(ok('/api/workspace/alerts')['alerts'])==1
    backup_status,backup=request('/api/workspace/state/backup',{},raw=True)
    assert backup_status==200
    (QA/'backup.sqlite3').write_bytes(backup)
    with closing(sqlite3.connect(QA/'backup.sqlite3')) as db:assert db.execute('PRAGMA integrity_check').fetchone()[0]=='ok'
    duplicate=subprocess.run([str(EXE),'--no-browser','--port',str(port)],env=environment,capture_output=True,timeout=20)
    assert duplicate.returncode==0
    stop()
    environment['INSIGHT_REQUIRE_READ_ONLY']='1'
    start()
    blocked=ok('/api/status')
    assert not blocked['connected'],'Strict read-only mode must reject the current broad-privilege account'
    restored=ok('/api/workspace/conversations/'+pending['conversation_id'])
    assert restored['pending']['clarification']['field']=='period'
    persisted=ok('/api/workspace/saved')['saved']
    assert any(row['id']==saved['id'] and row['title']=='RC3 conferência' and row['run_id']==example['run_id'] for row in persisted)
    report={'version':VERSION,'minimal_path':True,'authentication':True,'operator_fields_rejected':True,
            'followups':True,'clarifications':True,'restart_persistence':True,'native_quality':True,
            'paging_and_complete_exports':True,'pdf_excel_csv':True,'schedule_alert_deduplication':True,'profile_status_intersection':True,
            'sqlite_backup_integrity':True,'strict_read_only_rejects_broad_account':True,
            'stability_regressions':True,'saved_rename_persistence':True,'stock_expiry':True,
            'database_editor_write_crud':True,'database_editor_query_modes':True,'database_editor_history':True,
            'exe_sha256':hashlib.sha256(EXE.read_bytes()).hexdigest()}
    (QA/'verification.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
    print(json.dumps(report,indent=2),flush=True)
finally:
    stop();log.close()
