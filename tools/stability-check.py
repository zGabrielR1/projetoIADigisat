"""Isolated ZIP installation, recovery and bounded read-only performance checks."""
from __future__ import annotations
from concurrent.futures import ThreadPoolExecutor
from contextlib import closing
from datetime import datetime
from pathlib import Path
import base64
import hashlib
import json
import math
import os
import platform
import secrets
import shutil
import socket
import sqlite3
import statistics
import subprocess
import sys
import time
import urllib.error
import urllib.request
import zipfile
import argparse

from dotenv import dotenv_values
from pymongo import MongoClient
from pymongo.uri_parser import parse_uri

ROOT=Path(__file__).resolve().parent.parent
sys.path.insert(0,str(ROOT))
from insight_version import VERSION
QA=ROOT/'.release-build'/('stability-'+datetime.now().strftime('%Y%m%d-%H%M%S')+'-'+secrets.token_hex(3))
INSTALL=QA/'instalação portátil com espaços'
INSTALL.mkdir(parents=True)
parser=argparse.ArgumentParser()
parser.add_argument('--archive',type=Path,default=ROOT/'release'/f'DigiSatInsight-{VERSION}-win64.zip')
archive=parser.parse_args().archive.resolve()
with zipfile.ZipFile(archive) as package:
    assert set(package.namelist())=={'DigiSatInsight.exe','config.env','VERSION.json','LEIA-ME.md','SHA256SUMS'}
    package.extractall(INSTALL)
EXE=INSTALL/'DigiSatInsight.exe'
exe_hash=hashlib.sha256(EXE.read_bytes()).hexdigest()
environment=os.environ.copy()
for name in list(environment):
    if name.startswith(('MONGO_','BACKUP_MONGO_','AI_')) or name in {'DIGISAT_MODE','PYTHONPATH','PYTHONHOME','VIRTUAL_ENV','INSIGHT_REQUIRE_READ_ONLY'}:
        environment.pop(name,None)
environment.update(PATH=str(Path(os.environ['WINDIR'])/'System32'),INSIGHT_USER='stability-test',
    INSIGHT_PASSWORD=secrets.token_urlsafe(24),INSIGHT_DATA_DIR=str(QA/'data'),INSIGHT_SCHEDULER='0',INSIGHT_HOST='127.0.0.1',
    AI_ENDPOINT='',AI_MODEL='',AI_API_KEY='',PYTHONUTF8='1')
with socket.socket() as free:
    free.bind(('127.0.0.1',0));port=free.getsockname()[1]
BASE=f'http://127.0.0.1:{port}'
AUTH='Basic '+base64.b64encode((environment['INSIGHT_USER']+':'+environment['INSIGHT_PASSWORD']).encode()).decode()
log=(QA/'server.log').open('w',encoding='utf-8')
process=None
report={'version':VERSION,'exe_sha256':exe_hash,'platform':platform.platform(),'functional':{},'performance':[],
    'clean_os_tested':False,'windows_sandbox_available':(Path(os.environ['WINDIR'])/'System32/WindowsSandbox.exe').exists(),
    'scope':'Read-only tests against the existing database; isolated application files and SQLite state.'}


def request(path,body=None,raw=False,auth=True):
    headers={'Content-Type':'application/json'}
    if auth:headers['Authorization']=AUTH
    call=urllib.request.Request(BASE+path,data=json.dumps(body).encode() if body is not None else None,headers=headers)
    try:
        with urllib.request.urlopen(call,timeout=65) as response:
            value=response.read();return response.status,value if raw else json.loads(value)
    except urllib.error.HTTPError as exc:return exc.code,json.loads(exc.read())


def ok(path,body=None):
    status,value=request(path,body)
    if status!=200:raise AssertionError(f'{path}: HTTP {status}')
    return value


def start():
    global process
    process=subprocess.Popen([str(EXE),'--no-browser','--port',str(port)],cwd=QA,env=environment,
        stdout=log,stderr=subprocess.STDOUT,creationflags=subprocess.CREATE_NO_WINDOW)
    deadline=time.monotonic()+60
    while time.monotonic()<deadline:
        if process.poll() is not None:raise AssertionError('Isolated executable exited')
        try:
            if request('/api/status')[0]==200:return
        except OSError:pass
        time.sleep(.25)
    raise AssertionError('Isolated startup timed out')


def stop():
    escaped=str(EXE).replace("'","''")
    command=f"Get-Process DigiSatInsight -ErrorAction SilentlyContinue | Where-Object {{ $_.Path -eq '{escaped}' }} | Stop-Process -Force"
    subprocess.run([str(Path(os.environ['WINDIR'])/'System32/WindowsPowerShell/v1.0/powershell.exe'),'-NoProfile','-Command',command],capture_output=True)
    if process:
        try:process.wait(timeout=10)
        except subprocess.TimeoutExpired:process.terminate()


def replace_exe(content):
    for attempt in range(20):
        try:EXE.write_bytes(content);return
        except PermissionError:
            if attempt==19:raise
            time.sleep(.5)


def query(kind,start,end):
    before=time.perf_counter()
    value=ok('/api/workspace/conversation',{'action':'refine','filters':{'kind':kind,'start':start,'end':end,'page_size':50}})
    return {'seconds':round(time.perf_counter()-before,3),'documents':value['result']['documents'],
        'engine_seconds':round(value['result']['provenance']['duration_ms']/1000,3),'response':value}


try:
    start()
    assert request('/api/workspace/saved',auth=False)[0]==401
    assert not ok('/api/status')['connected']
    config=dotenv_values(ROOT/'.env')
    uri=config.get('MONGO_URI','mongodb://127.0.0.1:12220/')
    parsed=parse_uri(uri)
    host,db_port=parsed['nodelist'][0]
    password=parsed.get('password')
    if config.get('MONGO_PASSWORD_FILE'):
        secret=Path(config['MONGO_PASSWORD_FILE'])
        if not secret.is_absolute():secret=ROOT/secret
        lines=[line.strip() for line in secret.read_text(encoding='utf-8-sig').splitlines() if line.strip()]
        assert len(lines) in {1,2}
        password=lines[-1]
    settings={'host':host,'port':db_port,'database':config.get('MONGO_DATABASE') or parsed.get('database'),
        'username':config.get('MONGO_USERNAME') or parsed.get('username') or '',
        'auth_source':config.get('MONGO_AUTH_SOURCE','admin'),'password':password,'require_read_only':False}
    check=ok('/api/workspace/connection/test',settings)
    strict_status=request('/api/workspace/connection/test',{**settings,'require_read_only':True})[0]
    assert strict_status==(200 if check['security']['verified_read_only'] else 422)
    initial_config=(INSTALL/'config.env').read_bytes()
    assert request('/api/workspace/connection/save',{**settings,'password':'deliberately-incorrect-stability-password'})[0]==422
    assert (INSTALL/'config.env').read_bytes()==initial_config
    assert ok('/api/workspace/connection/save',settings)['saved']
    assert len(list(INSTALL.glob('mongo-*.local.key')))==1
    assert not any(name in ok('/api/workspace/connection') for name in ['password','secret'])
    stop();start()
    assert ok('/api/status')['connected']
    report['functional']['isolated_zip_setup_restart_and_secret_handling']=True

    first=ok('/api/workspace/conversation',{'message':'Em 05/10/2026, qual foi a diferença do vendedor?'})
    assert first['clarification']['field']=='seller_query'
    answered=ok('/api/workspace/conversation',{'conversation_id':first['conversation_id'],'answer':'2'})
    assert answered['plan']['start']=='2026-10-05' and answered['result']['totals']['Outros']==104.96
    bad_range=ok('/api/workspace/conversation',{'message':'Investigue vendedor 2 de 01/01/2024 a 31/12/2026'})
    assert bad_range['status']=='clarification'
    assert ok('/api/workspace/conversation',{'conversation_id':bad_range['conversation_id'],'answer':'01/01/2024 a 31/12/2026'})['status']=='clarification'
    assert ok('/api/workspace/conversation',{'conversation_id':bad_range['conversation_id'],'answer':'05/10/2026'})['result']['documents']==37
    native_quality=ok('/api/workspace/conversation',{'message':'Qualidade dos dados no perfil nativo em 05/10/2026'})
    assert native_quality['plan']['kind']=='quality'
    report['functional']['clarification_date_range_and_native_quality_regressions']=True

    saved=ok('/api/workspace/saved',{'run_id':answered['run_id'],'title':'Isolated stability evidence'})
    backup_status,backup=request('/api/workspace/state/backup',{},raw=True)
    assert backup_status==200
    restored=QA/'restored-data';restored.mkdir()
    (restored/'insight.sqlite3').write_bytes(backup)
    with closing(sqlite3.connect(restored/'insight.sqlite3')) as db:assert db.execute('PRAGMA integrity_check').fetchone()[0]=='ok'
    stop();environment['INSIGHT_DATA_DIR']=str(restored);start()
    assert any(row['id']==saved['id'] for row in ok('/api/workspace/saved')['saved'])
    snapshot=ok('/api/workspace/conversation/resume',{'run_id':saved['run_id']})
    assert snapshot['result']['totals']['Outros']==104.96
    report['functional']['backup_restore_preserves_evidence']=True

    old_archive=ROOT/'release/DigiSatInsight-1.0.0-rc.1-win64.zip'
    if VERSION!='1.0.0-rc.1' and old_archive.exists():
        new_binary=EXE.read_bytes()
        with zipfile.ZipFile(old_archive) as old:old_binary=old.read('DigiSatInsight.exe')
        stop();replace_exe(old_binary);start()
        assert ok('/api/status')['version']=='1.0.0-rc.1'
        assert any(row['id']==saved['id'] for row in ok('/api/workspace/saved')['saved'])
        stop();replace_exe(new_binary);start()
        assert ok('/api/status')['version']==VERSION
        assert ok('/api/workspace/runs/'+saved['run_id'])['result']['totals']['Outros']==104.96
        report['functional']['rc2_to_rc1_rollback_and_upgrade_preserves_data']=True

    options={'serverSelectionTimeoutMS':3000,'connectTimeoutMS':3000}
    if settings['username']:options.update(username=settings['username'],password=password,authSource=settings['auth_source'])
    with MongoClient(uri,**options) as client:
        coll=client[settings['database']].Movimentacoes
        report['dataset']={'total_movements':coll.count_documents({},maxTimeMS=10000),
            'movements_2026':coll.count_documents({'DataHoraEmissao':{'$gte':datetime(2026,1,1),'$lt':datetime(2027,1,1)}},maxTimeMS=10000)}
    cases=[('daily investigation','investigation','2026-10-05','2026-10-05',20,3),
           ('monthly revenue','revenue','2026-09-01','2026-09-30',5,3),
           ('year-to-date revenue','revenue','2026-01-01','2026-10-26',20,2),
           ('year-to-date investigation','investigation','2026-01-01','2026-10-26',20,2)]
    for label,kind,a,b,budget,repeats in cases:
        samples=[];docs=0;errors=[]
        for _ in range(repeats):
            try:
                result=query(kind,a,b);samples.append(result['seconds']);docs=result['documents']
            except Exception as exc:errors.append(type(exc).__name__)
        measured={'case':label,'samples_seconds':samples,'documents':docs,'budget_seconds':budget,'errors':errors,
                  'median_seconds':round(statistics.median(samples),3) if samples else None,'max_seconds':max(samples) if samples else None,
                  'passed':not errors and bool(samples) and max(samples)<=budget}
        report['performance'].append(measured)
        print(json.dumps(measured),flush=True)
    before=time.perf_counter()
    with ThreadPoolExecutor(max_workers=3) as pool:
        futures=[pool.submit(query,k,'2026-10-05','2026-10-05') for k in ['revenue','investigation','payments']]
        concurrent=[future.result() for future in futures]
    report['concurrency']={'users':3,'seconds':round(time.perf_counter()-before,3),
        'request_seconds':[r['seconds'] for r in concurrent],'passed':all(r['seconds']<20 for r in concurrent)}
    report['functional']['concurrent_independent_conversations']=True

    year=ok('/api/workspace/conversation',{'action':'refine','filters':{'kind':'documents','start':'2026-01-01','end':'2026-10-26'}})
    export=ok('/api/workspace/exports',{'run_id':year['run_id'],'format':'csv','complete':True})
    ok('/api/workspace/exports/'+export['id']+'/cancel',{})
    deadline=time.monotonic()+60
    while time.monotonic()<deadline:
        job=ok('/api/workspace/exports/'+export['id'])
        if job['status'] in {'cancelled','failed','succeeded'}:break
        time.sleep(.2)
    assert job['status']=='cancelled'
    assert request('/api/workspace/exports/'+export['id']+'/download')[0]==404
    report['functional']['large_export_cancellation_no_partial_download']=True
    report['functional_passed']=True
    report['performance_passed']=all(row['passed'] for row in report['performance']) and report['concurrency']['passed']
except Exception as exc:
    report['functional_passed']=False
    report['failure']=type(exc).__name__+': '+str(exc)
    raise
finally:
    stop();log.close()
    (QA/'results.json').write_text(json.dumps(report,indent=2,ensure_ascii=False),encoding='utf-8')
    (ROOT/'.release-build/latest-stability-path').write_text(str(QA),encoding='utf-8')
    print('STABILITY_REPORT='+str(QA/'results.json'),flush=True)
