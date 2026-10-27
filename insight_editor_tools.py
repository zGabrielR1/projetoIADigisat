"""Collection inspection/creation and personal query presets for the editor."""
from __future__ import annotations

from collections import defaultdict
from datetime import datetime
import json
import time

from bson import ObjectId, Int64, Decimal128, Binary, Regex, Code
from fastapi import APIRouter, Depends, Header, HTTPException, Request
from pydantic import BaseModel, ConfigDict, Field, field_validator
from pymongo.errors import CollectionInvalid, OperationFailure, PyMongoError
from pymongo.write_concern import WriteConcern

import insight_editor as editor
import insight_store as store
from insight_editor_query import QueryRequest, compile_query, ejson, fail, MAX_JSON

router = APIRouter(prefix='/api/editor', dependencies=[Depends(editor.same_origin)])


def bson_type(value):
    for cls, name in [(ObjectId,'objectId'),(bool,'boolean'),(Int64,'int64'),(Decimal128,'decimal128'),
                      (datetime,'date'),(Code,'javascript'),(str,'string'),(dict,'object'),
                      (list,'array'),(Binary,'binary'),(bytes,'binary'),(Regex,'regex'),(float,'double'),(int,'integer')]:
        if isinstance(value, cls): return name
    return 'null' if value is None else type(value).__name__


def schema_sample(documents, limit=100):
    fields = {}
    count = size = 0
    truncated = False
    for document in documents:
        if count >= limit:
            truncated=True
            break
        encoded = len(ejson(document).encode('utf-8'))
        if encoded > MAX_JSON or size + encoded > 2 * MAX_JSON:
            truncated = True
            break
        size += encoded; count += 1
        seen = defaultdict(set)
        budget = [0]
        def visit(value, path, depth):
            nonlocal truncated
            budget[0] += 1
            if budget[0] > 5000 or depth > 8:
                truncated = True
                return
            if path:
                if path not in fields and path not in seen and len(fields.keys() | seen.keys()) >= 500:
                    truncated = True
                    return
                seen[path].add(bson_type(value))
            if isinstance(value, dict):
                for key, child in value.items():
                    visit(child, path+'.'+key if path else key, depth+1)
            elif isinstance(value, list):
                if len(value)>25: truncated=True
                for child in value[:25]: visit(child,path+'[]',depth+1)
        visit(document,'',0)
        for path, types in seen.items():
            entry=fields.setdefault(path,{'path':path,'types':set(),'present_in':0})
            entry['types'].update(types);entry['present_in']+=1
    return {'sampled_documents':count, 'truncated':truncated,
            'fields':[{'path':path,'types':sorted(entry['types']),'present_in':entry['present_in']} for path,entry in sorted(fields.items())]}


@router.get('/inspect')
def inspect_collection(database: str, collection: str):
    notes=[]
    try:
        with editor.editor_client() as client:
            coll=editor.selected_collection(client,database,collection)
            cursor=coll.find({}).sort('_id',1).limit(101).max_time_ms(5000)
            try: result=schema_sample(cursor)
            finally: cursor.close()
            try: estimate=coll.estimated_document_count(maxTimeMS=5000)
            except PyMongoError:
                estimate=None;notes.append('Contagem estimada indisponível.')
            indexes=[]
            try:
                for index in coll.list_indexes():
                    indexes.append({'name':index.get('name',''),'keys':list(index.get('key',{}).items()),
                                    'unique':bool(index.get('unique',False) or index.get('name')=='_id_'),'sparse':bool(index.get('sparse',False))})
            except PyMongoError: notes.append('Índices indisponíveis para esta conta.')
    except PyMongoError:
        raise HTTPException(422,'Não foi possível inspecionar a coleção no prazo. Confira o acesso e tente novamente.') from None
    return {'database':database,'collection':collection,'estimated_documents':estimate,'indexes':indexes,
            **result,'notes':notes,'method':'Primeiros 100 documentos por _id; até 25 elementos por array e oito níveis. A amostra não representa um esquema completo.'}


class QueryPreset(BaseModel):
    model_config=ConfigDict(extra='forbid')
    id: str | None=Field(default=None,min_length=10,max_length=100)
    title: str=Field(min_length=1,max_length=120)
    query: QueryRequest

    @field_validator('title')
    @classmethod
    def clean_title(cls,value):
        if not value.strip(): raise ValueError('Informe um título para a consulta.')
        return value.strip()


def saved_query(key):
    rows=store.read('SELECT body FROM editor_queries WHERE id=?',(key,))
    if not rows: raise HTTPException(404,'Consulta salva não encontrada.')
    body=json.loads(rows[0]['body'])
    if body['owner']!=editor.journal_owner() or body['connection']!=editor.connection_identity():
        raise HTTPException(404,'Consulta salva não encontrada nesta conexão.')
    return body


@router.get('/queries')
def query_presets():
    return {'queries':store.read("""SELECT id,updated,json_extract(body,'$.title') AS title,
        json_extract(body,'$.query.database') AS database,json_extract(body,'$.query.collection') AS collection,
        json_extract(body,'$.query.mode') AS mode FROM editor_queries
        WHERE json_extract(body,'$.owner')=? AND json_extract(body,'$.connection')=?
        ORDER BY updated DESC,rowid DESC LIMIT 100""",(editor.journal_owner(),editor.connection_identity()))}


@router.get('/queries/{key}')
def load_query(key: str):
    body=saved_query(key)
    return {'id':key,'title':body['title'],'query':body['query']}


@router.post('/queries')
def save_query(req: QueryPreset):
    editor.database_name(req.query.database)
    compiled=compile_query(req.query)
    editor.collection_name(compiled['collection'])
    if req.id: saved_query(req.id)
    key=req.id or store.identifier()
    body={'title':req.title,'query':req.query.model_dump(mode='json'),'owner':editor.journal_owner(),'connection':editor.connection_identity()}
    store.write('INSERT OR REPLACE INTO editor_queries VALUES(?,?,?)',(key,store.dump(body),store.now()))
    return {'id':key,'title':req.title}


@router.delete('/queries/{key}')
def remove_query(key: str):
    saved_query(key)
    store.write('DELETE FROM editor_queries WHERE id=?',(key,))
    return {'removed':True}


class CreateCollection(BaseModel):
    model_config=ConfigDict(extra='forbid')
    database: str=Field(min_length=1,max_length=120)
    collection: str=Field(min_length=1,max_length=120)
    confirmation: str=Field(max_length=120)


def create_empty_collection(req,token,owner):
    editor.database_name(req.database);editor.collection_name(req.collection,write=True)
    if req.database in editor.SYSTEM_DATABASES: raise HTTPException(403,'Bancos de sistema não podem ser editados.')
    access=editor.session(token,owner,req.database)
    if not editor.permitted(access['grants'],req.database,req.collection,'createCollection'):
        raise HTTPException(403,'A conta não tem permissão para criar coleções neste banco.')
    if req.confirmation!=req.collection: fail('Confirme o nome exato da nova coleção.')
    key=store.identifier()
    body={'id':key,'database':req.database,'collection':req.collection,'operation':'create_collection',
          'id_json':'null','before_json':'null','after_json':ejson({'collection':req.collection,'empty_on_creation':True}),
          'created':store.now(),'status':'applying','session_hash':editor.digest(token),'actor':owner,
          'fingerprint':access['fingerprint'],'journal_owner':editor.journal_owner(),'connection_identity':access['connection_identity']}
    try:
        with editor.editor_client() as client:
            db=client[req.database]
            if req.collection in db.list_collection_names(maxTimeMS=5000):
                raise HTTPException(409,'A coleção já existe. Seu conteúdo foi preservado.')
            editor.put_change(key,body)
            db.create_collection(req.collection,write_concern=WriteConcern(w=1,wtimeout=5000))
        body.update(status='applied',completed_at=store.now());editor.put_change(key,body)
        return {'id':key,'status':'applied','database':req.database,'collection':req.collection}
    except CollectionInvalid:
        body.update(status='failed',error='collection_exists');editor.put_change(key,body)
        raise HTTPException(409,'A coleção já existe. Seu conteúdo foi preservado.') from None
    except OperationFailure as exc:
        definite=exc.code in {2,9,13,18,48,73}
        body.update(status='failed' if definite else 'unknown',error=type(exc).__name__);editor.put_change(key,body)
        raise HTTPException(409 if definite else 503,'O MongoDB recusou a criação.' if definite else 'Resultado incerto. Atualize a árvore e confira o histórico antes de tentar novamente.') from None
    except HTTPException: raise
    except Exception:
        body.update(status='unknown',error='outcome_requires_reload')
        try: editor.put_change(key,body)
        except Exception: pass
        raise HTTPException(503,'Criação não confirmada. Atualize a árvore e confira o histórico antes de tentar novamente.') from None


@router.post('/collection/create')
def create_collection(req: CreateCollection,request: Request,x_insight_editor_session: str=Header(default='')):
    return create_empty_collection(req,x_insight_editor_session,editor.actor(request))
