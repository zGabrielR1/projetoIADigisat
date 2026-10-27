"""Editor grammar/security tests and real writes to a disposable, separate MongoDB.

The write suite starts its OWN mongod in a new workspace directory on a free
loopback port. It never uses app.mongo_client or the configured DigiSat database
for fixture setup, mutations or cleanup.
"""
from __future__ import annotations

import base64
from contextlib import closing
from copy import deepcopy
from datetime import datetime
import json
import os
from pathlib import Path
import secrets
import socket
import subprocess
import sys
import tempfile
import time
import unittest
import urllib.error
import urllib.request
from unittest.mock import patch, MagicMock

from bson import ObjectId, Int64, Decimal128, json_util
from fastapi import HTTPException
from pymongo import MongoClient
from pymongo.errors import AutoReconnect, ConnectionFailure, WTimeoutError

from insight_editor_query import QueryRequest, compile_query, parse_json, ejson
import insight_editor as editor

ROOT = Path(__file__).resolve().parent


class EditorQueryRules(unittest.TestCase):
    def sql(self, text):
        return compile_query(QueryRequest(database='Fixture', mode='sql', sql=text))

    def test_sql_compiles_filters_projection_sort_and_limits(self):
        result = self.sql("SELECT name, meta.active FROM `documents` WHERE quantity >= 2 AND name LIKE 'A%' ORDER BY quantity DESC LIMIT 20 OFFSET 5;")
        self.assertEqual(result['collection'], 'documents')
        self.assertEqual(result['filter']['$and'][0], {'quantity':{'$gte':2}})
        self.assertEqual(result['filter']['$and'][1]['name'], {'$regex':'^A.*$', '$options':'i'})
        self.assertEqual(result['projection'], {'name':1,'meta.active':1,'_id':1})
        self.assertEqual(result['sort'], [('quantity',-1),('_id',1)])
        self.assertEqual((result['limit'],result['offset']), (20,5))

    def test_sql_literals_do_not_become_code_or_identifiers(self):
        result = self.sql("SELECT * FROM documents WHERE name = 'O''Brien; DROP DATABASE prod' AND flag = TRUE AND deleted IS NULL AND quantity IN (1, 2, 3)")
        values = result['filter']['$and']
        self.assertEqual(values[0], {'name':{'$eq':"O'Brien; DROP DATABASE prod"}})
        self.assertEqual(values[1], {'flag':{'$eq':True}})
        self.assertEqual(values[2], {'deleted':{'$eq':None}})
        self.assertEqual(values[3], {'quantity':{'$in':[1,2,3]}})

    def test_sql_writes_functions_multi_statements_and_unsupported_syntax_rejected(self):
        for sql in ['DELETE FROM documents', 'UPDATE documents SET x=1', 'DROP DATABASE Fixture',
                    'SELECT * FROM documents; DELETE FROM documents', 'SELECT count(*) FROM documents',
                    'SELECT * FROM documents WHERE x=1 OR y=2', 'SELECT * FROM a JOIN b',
                    'SELECT * FROM documents LIMIT 201', 'SELECT * FROM documents OFFSET -1',
                    'SELECT * FROM documents WHERE x = 1e999', 'SELECT * FROM documents -- comment']:
            with self.subTest(sql=sql), self.assertRaises(HTTPException): self.sql(sql)

    def test_extended_json_retains_identifiers_dates_and_int64(self):
        ident = ObjectId()
        req = QueryRequest(database='Fixture', collection='documents', mode='json', filter_json=ejson({'_id':ident,'long':Int64(9007199254740993),'date':datetime(2026,10,27)}))
        result = compile_query(req)
        self.assertEqual(result['filter']['_id'],ident)
        self.assertIsInstance(result['filter']['long'],Int64)
        self.assertEqual(result['filter']['long'],9007199254740993)
        self.assertIsInstance(result['filter']['date'],datetime)
        with self.assertRaises(HTTPException): parse_json('{"$oid":"bad"}')

    def test_unsafe_or_unbounded_filters_rejected(self):
        for query in [{'$where':'return true'}, {'x':{'$function':{}}}, {'x':{'$expr':1}},
                      {'$or':[]}, {'x':{'$exists':'true'}}, {'x':{'$in':list(range(101))}},
                      {'x':{'$options':'i'}}, {'x':{'$regex':'a'*257}}, {'x':{'$ne':{'$code':'evil'}}}]:
            with self.subTest(query=query), self.assertRaises(HTTPException):
                compile_query(QueryRequest(database='Fixture',collection='documents',mode='json',filter_json=json.dumps(query)))

    def test_builder_contains_is_literal_and_paging_preserves_sort(self):
        req = QueryRequest(database='Fixture', collection='documents', conditions=[{'field':'name','operator':'contains','value_json':'"a.*"'}],sort_json='{"a":1,"b":1,"c":1,"d":1,"e":1}')
        compiled = compile_query(req)
        self.assertEqual(compiled['filter'],{'$and':[{'name':{'$regex':'a\\.\\*','$options':'i'}}]})
        paged = compile_query(QueryRequest(database='Fixture',collection='documents',mode='json',filter_json=ejson(compiled['filter']),sort_json=json.dumps(dict(compiled['sort'])),offset=50))
        self.assertEqual(paged['sort'],compiled['sort'])

    def test_projection_operators_and_mixed_modes_rejected(self):
        for projection in ['{"x":{"$slice":1}}','{"x":1,"y":0}']:
            with self.assertRaises(HTTPException): compile_query(QueryRequest(database='Fixture',collection='documents',projection_json=projection))

    def test_expired_wrong_owner_and_reconfigured_sessions_block_before_database_access(self):
        token = secrets.token_urlsafe(20)
        with patch.object(editor,'fingerprint',return_value='fixture'):
            editor._sessions[editor.digest(token)]={'database':'Fixture','actor':'owner','fingerprint':'fixture','expires':time.time()-1,'grants':[]}
            with self.assertRaises(HTTPException): editor.session(token,'owner','Fixture')
            editor._sessions[editor.digest(token)]['expires']=time.time()+100
            with self.assertRaises(HTTPException): editor.session(token,'another-user','Fixture')
            with patch.object(editor,'fingerprint',return_value='changed'), self.assertRaises(HTTPException): editor.session(token,'owner','Fixture')
        editor._sessions.pop(editor.digest(token),None)


def free_port():
    with socket.socket() as sock:
        sock.bind(('127.0.0.1',0))
        return sock.getsockname()[1]


class EditorIsolatedMongo(unittest.TestCase):
    database = 'InsightEditorTest'
    collection = 'documents'

    @classmethod
    def setUpClass(cls):
        binary = Path(os.environ.get('INSIGHT_TEST_MONGOD',r'C:\Digisat\SuiteG6\MongoDB\bin\mongod.exe'))
        if not binary.is_file():
            if os.environ.get('INSIGHT_EDITOR_TEST_EXE'):
                raise RuntimeError('Packaged write verification requires an isolated mongod; CRUD cannot be skipped')
            raise unittest.SkipTest('Isolated write tests require an explicit local mongod executable')
        build = ROOT/'.release-build';build.mkdir(exist_ok=True)
        cls.folder = tempfile.TemporaryDirectory(prefix='editor-isolated-',dir=build)
        cls.path = Path(cls.folder.name).resolve()
        if not cls.path.is_relative_to(build.resolve()): raise RuntimeError('Test path escaped workspace')
        cls.dbpath = cls.path/'mongo';cls.dbpath.mkdir()
        cls.mongo_port,cls.http_port = free_port(),free_port()
        cls.password = secrets.token_urlsafe(24)
        cls.read_password = secrets.token_urlsafe(24)
        cls.mongo_process=cls.api_process=cls.client=None
        cls.api_log=(cls.path/'api.log').open('w',encoding='utf-8')
        cls.flags = subprocess.CREATE_NO_WINDOW if os.name=='nt' else 0
        cls.binary = binary
        try:
            cls.start_mongo(auth=False)
            bootstrap = MongoClient(f'mongodb://127.0.0.1:{cls.mongo_port}/',serverSelectionTimeoutMS=1000)
            # Verify the storage directory belongs to THIS new process before any write.
            actual = bootstrap.admin.command('getCmdLineOpts')['parsed']['storage']['dbPath']
            if Path(actual).resolve()!=cls.dbpath: raise RuntimeError('Refusing writes: wrong MongoDB storage path')
            bootstrap.admin.command('createUser','editor-fixture-root',pwd=cls.password,roles=[{'role':'root','db':'admin'}])
            bootstrap.admin.command('createUser','editor-fixture-reader',pwd=cls.read_password,roles=[{'role':'read','db':cls.database}])
            bootstrap.admin.command('createUser','editor-fixture-writer',pwd=cls.password,roles=[{'role':'readWrite','db':cls.database}])
            try: bootstrap.admin.command('shutdown',force=True)
            except (AutoReconnect,ConnectionFailure): pass
            bootstrap.close();cls.mongo_process.wait(timeout=20)
            cls.start_mongo(auth=True)
            cls.client=MongoClient(f'mongodb://127.0.0.1:{cls.mongo_port}/',username='editor-fixture-root',password=cls.password,authSource='admin',serverSelectionTimeoutMS=2000)
            actual=cls.client.admin.command('getCmdLineOpts')['parsed']['storage']['dbPath']
            if Path(actual).resolve()!=cls.dbpath: raise RuntimeError('Wrong isolated server after authenticated restart')
            cls.client[cls.database][cls.collection].insert_one({'fixture':True})
            key=cls.path/'test-mongo.local.key';key.write_text(cls.password,encoding='utf-8')
            cls.http_user='editor-http-fixture';cls.http_password=secrets.token_urlsafe(24)
            env=os.environ.copy();env.update(MONGO_URI=f'mongodb://127.0.0.1:{cls.mongo_port}/?directConnection=true',MONGO_DATABASE=cls.database,MONGO_USERNAME='editor-fixture-root',MONGO_AUTH_SOURCE='admin',MONGO_PASSWORD_FILE=str(key),INSIGHT_USER=cls.http_user,INSIGHT_PASSWORD=cls.http_password,INSIGHT_HOST='127.0.0.1',INSIGHT_REQUIRE_READ_ONLY='1',INSIGHT_SCHEDULER='0',INSIGHT_DATA_DIR=str(cls.path/'app-data'),AI_ENDPOINT='',AI_MODEL='',AI_API_KEY='')
            candidate=os.environ.get('INSIGHT_EDITOR_TEST_EXE')
            cls.packaged=bool(candidate)
            if candidate:
                exe=Path(candidate).resolve()
                if not exe.is_file() or not exe.is_relative_to(build.resolve()): raise RuntimeError('Packaged test executable must be inside .release-build')
                command=[str(exe),'--no-browser','--port',str(cls.http_port)]
                env['PATH']=str(Path(os.environ['WINDIR'])/'System32')
                for key in ['PYTHONPATH','PYTHONHOME','VIRTUAL_ENV']: env.pop(key,None)
            else:
                command=[sys.executable,'-m','uvicorn','app:app','--host','127.0.0.1','--port',str(cls.http_port),'--log-level','warning']
            cls.api_process=subprocess.Popen(command,cwd=ROOT,env=env,stdout=cls.api_log,stderr=subprocess.STDOUT,creationflags=cls.flags)
            cls.base=f'http://127.0.0.1:{cls.http_port}'
            deadline=time.monotonic()+40
            while time.monotonic()<deadline:
                if cls.api_process.poll() is not None: raise RuntimeError('Isolated API exited: '+(cls.path/'api.log').read_text(encoding='utf-8')[-2000:])
                try:
                    with urllib.request.urlopen(cls.base+'/database-editor',timeout=2) as response:
                        if response.status==200: return
                except OSError: time.sleep(.2)
            raise RuntimeError('Isolated API startup timed out')
        except Exception:
            cls.clean_up()
            raise

    @classmethod
    def start_mongo(cls,auth):
        args=[str(cls.binary),'--dbpath',str(cls.dbpath),'--port',str(cls.mongo_port),'--bind_ip','127.0.0.1','--logpath',str(cls.path/'mongo.log'),'--logappend','--wiredTigerCacheSizeGB','0.25']
        if auth: args.append('--auth')
        cls.mongo_process=subprocess.Popen(args,stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL,creationflags=cls.flags)
        probe=MongoClient(f'mongodb://127.0.0.1:{cls.mongo_port}/',serverSelectionTimeoutMS=400)
        try:
            deadline=time.monotonic()+30
            while time.monotonic()<deadline:
                if cls.mongo_process.poll() is not None: raise RuntimeError('Isolated mongod exited: '+(cls.path/'mongo.log').read_text(encoding='utf-8',errors='replace')[-2000:])
                try:
                    probe.admin.command('ping')
                    return
                except ConnectionFailure: time.sleep(.15)
            raise RuntimeError('Isolated mongod startup timed out')
        finally: probe.close()

    @classmethod
    def clean_up(cls):
        if cls.api_process is not None and cls.api_process.poll() is None:
            if os.name=='nt' and getattr(cls,'packaged',False):
                subprocess.run([str(Path(os.environ['WINDIR'])/'System32'/'taskkill.exe'),'/PID',str(cls.api_process.pid),'/T','/F'],capture_output=True)
            else:
                cls.api_process.terminate()
            try: cls.api_process.wait(timeout=10)
            except subprocess.TimeoutExpired: cls.api_process.kill();cls.api_process.wait(timeout=10)
        if os.name=='nt' and getattr(cls,'packaged',False):
            # One-file launchers may leave their extracted child alive briefly.
            # Match only this candidate's exact path, never the installed app.
            target=str(Path(os.environ['INSIGHT_EDITOR_TEST_EXE']).resolve()).replace("'","''")
            command="Get-Process DigiSatInsight -ErrorAction SilentlyContinue | Where-Object { $_.Path -eq '"+target+"' } | Stop-Process -Force"
            subprocess.run([str(Path(os.environ['WINDIR'])/'System32/WindowsPowerShell/v1.0/powershell.exe'),'-NoProfile','-Command',command],capture_output=True,creationflags=cls.flags)
        if cls.mongo_process is not None and cls.mongo_process.poll() is None:
            if cls.client is not None:
                try: cls.client.admin.command('shutdown',force=True)
                except (AutoReconnect,ConnectionFailure): pass
            try: cls.mongo_process.wait(timeout=10)
            except subprocess.TimeoutExpired: cls.mongo_process.terminate();cls.mongo_process.wait(timeout=10)
        if cls.client is not None: cls.client.close()
        cls.api_log.close()
        for attempt in range(20):
            try:
                cls.folder.cleanup()
                break
            except PermissionError:
                if attempt==19: raise
                time.sleep(.25)

    @classmethod
    def tearDownClass(cls): cls.clean_up()

    def setUp(self):
        # This client was constructed only with the isolated port and independently
        # verified storage path. No configured DigiSat connection is used here.
        self.coll=self.client[self.database][self.collection]
        self.coll.delete_many({})
        self.ident=ObjectId()
        self.original={'_id':self.ident,'name':'Alpha','quantity':5,'meta':{'active':True},'long':Int64(9007199254740993),'money':Decimal128('12.34'),'date':datetime(2026,10,27)}
        self.coll.insert_many([deepcopy(self.original),{'name':'Beta','quantity':2}])
        self.token=None

    def call(self,path,body=None,*,auth=True,origin=None):
        headers={}
        if auth: headers['Authorization']='Basic '+base64.b64encode((self.http_user+':'+self.http_password).encode()).decode()
        if body is not None: headers['Content-Type']='application/json'
        if self.token: headers['X-Insight-Editor-Session']=self.token
        if origin: headers['Origin']=origin
        req=urllib.request.Request(self.base+'/api/editor/'+path,data=json.dumps(body).encode() if body is not None else None,headers=headers)
        try:
            with urllib.request.urlopen(req,timeout=20) as response: return response.status,json.loads(response.read())
        except urllib.error.HTTPError as exc:
            try: return exc.code,json.loads(exc.read())
            finally: exc.close()

    def ok(self,path,body=None):
        code,data=self.call(path,body)
        self.assertEqual(code,200,data)
        return data

    def unlock(self):
        self.token=self.ok('unlock',{'database':self.database,'confirmation':'EDITAR '+self.database})['token']

    def change(self,operation='update',after=None,original=None):
        if not self.token: self.unlock()
        return self.ok('preview',{'database':self.database,'collection':self.collection,'operation':operation,
            'id_json':ejson(self.ident) if operation!='insert' else None,
            'original_json':ejson(self.original if original is None else original) if operation!='insert' else None,
            'document_json':ejson(after) if after is not None else None})

    def test_tree_query_sql_projection_and_bson_roundtrip(self):
        self.assertIn(self.database,self.ok('databases')['databases'])
        self.assertIn(self.collection,self.ok('collections?database='+self.database)['collections'])
        result=self.ok('query',{'database':self.database,'collection':self.collection,'mode':'builder','conditions':[{'field':'quantity','operator':'gte','value_json':'2'}],'limit':1})
        self.assertEqual(len(result['documents']),1);self.assertTrue(result['has_more'])
        sql=self.ok('query',{'database':self.database,'mode':'sql','sql':"SELECT name, quantity FROM documents WHERE name LIKE 'Al%' ORDER BY quantity DESC LIMIT 2"})
        self.assertEqual(parse_json(sql['documents'][0]['json'])['name'],'Alpha')
        found=self.ok('document',{'database':self.database,'collection':self.collection,'id_json':ejson(self.ident)})
        restored=parse_json(found['json'])
        self.assertIsInstance(restored['long'],Int64);self.assertEqual(restored['long'],9007199254740993)
        self.assertIsInstance(restored['money'],Decimal128)

    def test_insert_requires_preview_and_applies_once(self):
        preview=self.change('insert',{'name':'Inserted','quantity':9})
        ident=parse_json(preview['id_json'])
        self.assertIsNone(self.coll.find_one({'_id':ident}))
        result=self.ok('apply/'+preview['id'],{'confirmation':self.collection})
        self.assertEqual(result['status'],'applied');self.assertEqual(self.coll.find_one({'_id':ident})['quantity'],9)
        self.assertEqual(self.call('apply/'+preview['id'],{'confirmation':self.collection})[0],409)

    def test_insert_result_can_be_edited_without_reopening_document(self):
        preview=self.change('insert',{'name':'New baseline','quantity':3})
        applied=self.ok('apply/'+preview['id'],{'confirmation':self.collection})
        after=parse_json(applied['after_json']);after['quantity']=4
        edited=self.ok('preview',{'database':self.database,'collection':self.collection,'operation':'update',
            'id_json':preview['id_json'],'original_json':applied['after_json'],'document_json':ejson(after)})
        self.ok('apply/'+edited['id'],{'confirmation':self.collection})
        self.assertEqual(self.coll.find_one({'_id':parse_json(preview['id_json'])})['quantity'],4)

    def test_update_preserves_int64_and_concurrent_new_field(self):
        after=deepcopy(self.original);after['quantity']=7;after['meta']={'active':False};after.pop('name')
        preview=self.change(after=after)
        self.coll.update_one({'_id':self.ident},{'$set':{'externalNew':'preserved'}})
        result=self.ok('apply/'+preview['id'],{'confirmation':self.collection})
        actual=self.coll.find_one({'_id':self.ident})
        self.assertEqual(actual['quantity'],7);self.assertFalse(actual['meta']['active'])
        self.assertNotIn('name',actual);self.assertEqual(actual['externalNew'],'preserved')
        self.assertIsInstance(actual['long'],Int64);self.assertEqual(actual['long'],9007199254740993)
        self.assertEqual(parse_json(result['before_json'])['externalNew'],'preserved')
        self.assertEqual(ejson(parse_json(result['after_json'])),ejson(actual))

    def test_conflict_does_not_overwrite_external_edit(self):
        after=deepcopy(self.original);after['quantity']=7
        preview=self.change(after=after)
        self.coll.update_one({'_id':self.ident},{'$set':{'quantity':99}})
        self.assertEqual(self.call('apply/'+preview['id'],{'confirmation':self.collection})[0],409)
        self.assertEqual(self.coll.find_one({'_id':self.ident})['quantity'],99)

    def test_stale_read_is_rejected_before_preview(self):
        self.unlock();after=deepcopy(self.original);after['quantity']=7
        self.coll.update_one({'_id':self.ident},{'$set':{'quantity':99}})
        code,_=self.call('preview',{'database':self.database,'collection':self.collection,'operation':'update','id_json':ejson(self.ident),'original_json':ejson(self.original),'document_json':ejson(after)})
        self.assertEqual(code,409)

    def test_delete_is_single_document_confirmed_and_original_is_journaled(self):
        preview=self.change('delete')
        self.assertEqual(self.call('apply/'+preview['id'],{'confirmation':'wrong'})[0],422)
        self.assertEqual(self.coll.count_documents({}),2)
        result=self.ok('apply/'+preview['id'],{'confirmation':self.collection})
        self.assertEqual(self.coll.count_documents({}),1);self.assertIsNone(self.coll.find_one({'_id':self.ident}))
        self.assertEqual(ejson(parse_json(result['before_json'])),ejson(self.original))
        import sqlite3
        with closing(sqlite3.connect(self.path/'app-data'/'insight.sqlite3')) as local:
            body=json.loads(local.execute('SELECT body FROM editor_changes WHERE id=?',(preview['id'],)).fetchone()[0])
        self.assertEqual(body['status'],'applied');self.assertEqual(ejson(parse_json(body['before_json'])),ejson(self.original))

    def test_duplicate_insert_race_is_rejected(self):
        wanted=ObjectId();preview=self.change('insert',{'_id':wanted,'name':'ours'})
        self.coll.insert_one({'_id':wanted,'name':'external'})
        self.assertEqual(self.call('apply/'+preview['id'],{'confirmation':self.collection})[0],409)
        self.assertEqual(self.coll.find_one({'_id':wanted})['name'],'external')

    def test_history_exposes_recovery_copy_only_with_matching_unlocked_connection(self):
        preview=self.change('delete')
        self.ok('apply/'+preview['id'],{'confirmation':self.collection})
        history=self.ok('history?database='+self.database)['changes']
        self.assertTrue(any(row['id']==preview['id'] and row['status']=='applied' for row in history))
        copy=self.ok('history/'+preview['id'])
        self.assertEqual(ejson(parse_json(copy['before_json'])),ejson(self.original))
        self.assertNotIn('fingerprint',copy);self.assertNotIn('session_hash',copy)
        # The recovered document is only written after a NEW ordinary preview/apply.
        restored=self.change('insert',parse_json(copy['before_json']))
        self.assertIsNone(self.coll.find_one({'_id':self.ident}))
        self.ok('apply/'+restored['id'],{'confirmation':self.collection})
        self.assertEqual(ejson(self.coll.find_one({'_id':self.ident})),ejson(self.original))
        self.ok('lock',{})
        self.assertEqual(self.call('history/'+preview['id'])[0],403)

    def test_workspace_backup_contains_editor_recovery_journal(self):
        after=deepcopy(self.original);after['quantity']=8
        preview=self.change(after=after)
        self.ok('apply/'+preview['id'],{'confirmation':self.collection})
        auth='Basic '+base64.b64encode((self.http_user+':'+self.http_password).encode()).decode()
        req=urllib.request.Request(self.base+'/api/workspace/state/backup',data=b'{}',headers={'Authorization':auth,'Content-Type':'application/json'})
        with urllib.request.urlopen(req,timeout=10) as response:
            self.assertEqual(response.status,200);data=response.read()
        backup=self.path/'recovery-check.sqlite3';backup.write_bytes(data)
        import sqlite3
        with closing(sqlite3.connect(backup)) as restored:
            self.assertEqual(restored.execute('PRAGMA integrity_check').fetchone()[0],'ok')
            copy=json.loads(restored.execute('SELECT body FROM editor_changes WHERE id=?',(preview['id'],)).fetchone()[0])
        self.assertEqual(copy['status'],'applied')
        self.assertEqual(ejson(parse_json(copy['before_json'])),ejson(self.original))

    def test_auth_origin_locked_session_and_system_database_guards(self):
        self.assertEqual(self.call('databases',auth=False)[0],401)
        self.assertEqual(self.call('unlock',{'database':self.database,'confirmation':'EDITAR '+self.database},origin='https://untrusted.invalid')[0],403)
        self.assertEqual(self.call('preview',{'database':self.database,'collection':self.collection,'operation':'insert','document_json':'{"x":1}'})[0],403)
        self.assertEqual(self.call('unlock',{'database':'admin','confirmation':'EDITAR admin'})[0],403)
        self.unlock();self.ok('lock',{})
        self.assertEqual(self.call('preview',{'database':self.database,'collection':self.collection,'operation':'insert','document_json':'{"x":1}'})[0],403)

    def test_read_only_mongo_user_cannot_unlock(self):
        reader=MongoClient(f'mongodb://127.0.0.1:{self.mongo_port}/',username='editor-fixture-reader',password=self.read_password,authSource='admin',serverSelectionTimeoutMS=2000)
        try:
            with patch.object(editor,'editor_client',return_value=reader):
                with self.assertRaises(HTTPException) as refused:
                    editor.unlock(editor.UnlockRequest(database=self.database,confirmation='EDITAR '+self.database),'fixture-owner')
            self.assertEqual(refused.exception.status_code,403)
        finally: reader.close()

    def test_database_scoped_writer_can_unlock(self):
        import app
        uri=f'mongodb://127.0.0.1:{self.mongo_port}/'
        def writer(): return MongoClient(uri,username='editor-fixture-writer',password=self.password,authSource='admin',serverSelectionTimeoutMS=2000)
        with patch.dict(os.environ,{'INSIGHT_DATA_DIR':str(self.path/'writer-state')}),patch.object(editor,'editor_client',side_effect=writer),patch.multiple(app,MONGO_URI=uri,MONGO_DATABASE=self.database,MONGO_USERNAME='editor-fixture-writer',MONGO_PASSWORD_FILE='',MONGO_AUTH_SOURCE='admin'):
                self.assertIn(self.database,editor.databases()['databases'])
                self.assertIn(self.collection,editor.collections(self.database)['collections'])
                unlocked=editor.unlock(editor.UnlockRequest(database=self.database,confirmation='EDITAR '+self.database),'writer-fixture-owner')
                self.assertEqual(unlocked['database'],self.database)
                preview=editor.preview(editor.ChangeRequest(database=self.database,collection=self.collection,operation='insert',document_json='{"name":"Scoped writer"}'),unlocked['token'],'writer-fixture-owner')
                result=editor.apply_change(preview['id'],unlocked['token'],'writer-fixture-owner',self.collection)
                self.assertEqual(result['status'],'applied')
                self.assertIsNotNone(self.coll.find_one({'_id':parse_json(preview['id_json'])}))
                editor._sessions.pop(editor.digest(unlocked['token']),None)

    def test_dangerous_queries_and_immutable_identifier_rejected(self):
        self.assertEqual(self.call('query',{'database':self.database,'collection':self.collection,'mode':'json','filter_json':'{"$where":"return true"}'})[0],422)
        self.assertEqual(self.call('query',{'database':self.database,'mode':'sql','sql':'DELETE FROM documents'})[0],422)
        self.unlock();after=deepcopy(self.original);after['_id']=ObjectId()
        code,_=self.call('preview',{'database':self.database,'collection':self.collection,'operation':'update','id_json':ejson(self.ident),'original_json':ejson(self.original),'document_json':ejson(after)})
        self.assertEqual(code,422);self.assertEqual(self.coll.count_documents({}),2)


class EditorUncertainWrites(unittest.TestCase):
    def test_uncertain_writes_keep_original_and_are_never_retried(self):
        import insight_store as store
        for failure in [AutoReconnect('fixture disconnect'),WTimeoutError('fixture write concern timeout',code=64)]:
            with self.subTest(failure=type(failure).__name__), tempfile.TemporaryDirectory() as folder:
                token=secrets.token_urlsafe(24);owner='fixture-owner';ident=ObjectId()
                original={'_id':ident,'name':'Before'};after={'_id':ident,'name':'After'}
                collection=MagicMock();collection.find_one.return_value=original
                collection.find_one_and_update.side_effect=failure
                editor._sessions[editor.digest(token)]={'database':'Fixture','actor':owner,'fingerprint':'isolated','expires':time.time()+100,'grants':[{'resource':{'anyResource':True},'actions':['anyAction']}]}
                try:
                    with patch.dict(os.environ,{'INSIGHT_DATA_DIR':folder}),patch.object(editor,'fingerprint',return_value='isolated'),patch.object(editor,'editor_client',return_value=MagicMock()),patch.object(editor,'selected_collection',return_value=collection):
                        preview=editor.preview(editor.ChangeRequest(database='Fixture',collection='documents',operation='update',id_json=ejson(ident),original_json=ejson(original),document_json=ejson(after)),token,owner)
                        with self.assertRaises(HTTPException) as uncertain: editor.apply_change(preview['id'],token,owner,'documents')
                        self.assertEqual(uncertain.exception.status_code,503)
                        journal=editor.get_change(preview['id'])
                        self.assertEqual(journal['status'],'unknown');self.assertEqual(parse_json(journal['before_json']),original)
                        with self.assertRaises(HTTPException) as duplicate: editor.apply_change(preview['id'],token,owner,'documents')
                        self.assertEqual(duplicate.exception.status_code,409)
                        self.assertEqual(collection.find_one_and_update.call_count,1)
                finally: editor._sessions.pop(editor.digest(token),None)


if __name__=='__main__': unittest.main()
