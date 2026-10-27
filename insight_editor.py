"""MongoDB document administration: scoped sessions, previews and single-document writes."""
from __future__ import annotations

import hashlib
import hmac
import json
import secrets
import threading
import time
from typing import Literal

from bson import ObjectId
from bson.errors import InvalidDocument
from fastapi import APIRouter, Depends, Header, HTTPException, Request
from pydantic import BaseModel, ConfigDict, Field
from pymongo import ReturnDocument
from pymongo.read_preferences import ReadPreference
from pymongo.write_concern import WriteConcern
from pymongo.errors import DuplicateKeyError, OperationFailure, PyMongoError

import insight_store as store
from insight_editor_query import QueryRequest, compile_query, ejson, parse_json, fail, MAX_JSON

router = APIRouter(prefix='/api/editor')
_sessions = {}
_lock = threading.RLock()
_session_key = secrets.token_bytes(32)
SESSION_SECONDS = 600
PREVIEW_SECONDS = 300
SYSTEM_DATABASES = {'admin', 'config', 'local'}


def digest(text):
    return hashlib.sha256(text.encode('utf-8')).hexdigest()


def fingerprint():
    import app
    # Hash, never expose or journal, connection details and credential revision.
    from insight_connection import password_from_current
    revision = '|'.join(str(v) for v in [app.MONGO_URI, app.MONGO_USERNAME,
                  app.MONGO_AUTH_SOURCE, password_from_current()])
    return hmac.new(_session_key, revision.encode('utf-8'), hashlib.sha256).hexdigest()


def journal_owner():
    import app
    return digest(app.INSIGHT_USER or 'localhost')


def connection_identity():
    import app
    from pymongo.uri_parser import parse_uri
    parsed = parse_uri(app.MONGO_URI)
    identity = [sorted(parsed['nodelist']), app.MONGO_USERNAME or parsed.get('username'),
                app.MONGO_AUTH_SOURCE if app.MONGO_USERNAME else parsed.get('options', {}).get('authSource', 'admin')]
    return digest(json.dumps(identity, sort_keys=True))


def actor(request):
    return digest(request.headers.get('authorization', '') + '|' + (request.client.host if request.client else 'local'))


def same_origin(request: Request):
    import app
    from urllib.parse import urlsplit
    host = request.url.hostname
    if not (app.INSIGHT_USER and app.INSIGHT_PASSWORD) and host not in {'127.0.0.1', 'localhost', '::1'}:
        raise HTTPException(403, 'Sem autenticação, o editor só aceita acesso pelo endereço localhost.')
    origin = request.headers.get('origin')
    if origin:
        origin_url = urlsplit(origin)
        if (origin_url.scheme, origin_url.netloc) != (request.url.scheme, request.url.netloc):
            raise HTTPException(403, 'Origem não permitida para o editor.')


router.dependencies.append(Depends(same_origin))


def database_name(name):
    if not name or len(name) > 120 or any(c in name for c in '/\\. "$\x00'):
        fail('Nome de banco inválido.')
    return name


def collection_name(name, *, write=False):
    if not name or len(name) > 120 or '\x00' in name or '$' in name:
        fail('Nome de coleção inválido.')
    if write and name.startswith('system.'):
        raise HTTPException(403, 'Coleções de sistema não podem ser editadas.')
    return name


def selected_collection(client, database, collection, *, write=False):
    database_name(database); collection_name(collection, write=write)
    if write and database in SYSTEM_DATABASES:
        raise HTTPException(403, 'Bancos de sistema não podem ser editados.')
    db = client[database]
    if collection not in db.list_collection_names(maxTimeMS=5000):
        raise HTTPException(404, 'Coleção não encontrada ou sem acesso.')
    selected = db[collection]
    if write:
        selected = selected.with_options(read_preference=ReadPreference.PRIMARY, write_concern=WriteConcern(w=1, wtimeout=5000))
    return selected


def editor_client():
    import app
    return app.mongo_client(for_editor=True)


def grants(client):
    info = client.admin.command('connectionStatus', showPrivileges=True).get('authInfo', {})
    if not info.get('authenticatedUsers'):
        raise HTTPException(403, 'Edição exige uma conta autenticada no MongoDB com permissões de escrita.')
    return info.get('authenticatedUserPrivileges', [])


def permitted(privileges, database, collection, action):
    for grant in privileges:
        resource, actions = grant.get('resource', {}), grant.get('actions', [])
        if action not in actions and 'anyAction' not in actions:
            continue
        if resource.get('anyResource'):
            return True
        if resource.get('db') not in ('', database):
            continue
        if resource.get('collection') in ('', collection):
            return True
    return False


class UnlockRequest(BaseModel):
    model_config = ConfigDict(extra='forbid')
    database: str = Field(min_length=1, max_length=120)
    confirmation: str = Field(max_length=160)


def unlock(req, owner):
    database_name(req.database)
    if req.database in SYSTEM_DATABASES:
        raise HTTPException(403, 'Bancos de sistema não podem ser editados.')
    if req.confirmation != 'EDITAR ' + req.database:
        fail('Digite EDITAR seguido do nome exato do banco para habilitar a sessão.')
    try:
        with editor_client() as client:
            if not client[req.database].list_collection_names(maxTimeMS=5000):
                raise HTTPException(404, 'Banco vazio, não encontrado ou sem acesso.')
            privileges = grants(client)
            # Collection-scoped grants are checked again for each preview.
            relevant = [g for g in privileges if g.get('resource', {}).get('anyResource') or g.get('resource', {}).get('db') in ('', req.database)]
            if not any(set(g.get('actions', [])) & {'insert', 'update', 'remove', 'anyAction'} for g in relevant):
                raise HTTPException(403, 'A conta atual não possui permissões de escrita neste banco.')
    except PyMongoError:
        raise HTTPException(403, 'Não foi possível verificar permissões de escrita no MongoDB.') from None
    token = secrets.token_urlsafe(32)
    with _lock:
        now = time.time()
        for key in list(_sessions):
            if _sessions[key]['expires'] < now:
                del _sessions[key]
        if len(_sessions) >= 100:
            raise HTTPException(429, 'Muitas sessões de edição abertas. Aguarde a expiração.')
        _sessions[digest(token)] = {'database': req.database, 'actor': owner, 'fingerprint': fingerprint(),
                                  'connection_identity':connection_identity(),
                                  'expires': now + SESSION_SECONDS, 'grants': privileges}
    store.event('editor', 'Write session enabled; database=' + req.database)
    return {'token': token, 'database': req.database, 'expires_in': SESSION_SECONDS}


def session(token, owner, database):
    with _lock:
        found = _sessions.get(digest(token or ''))
        if not found or found['expires'] < time.time() or found['actor'] != owner:
            raise HTTPException(403, 'Sessão de edição ausente ou expirada. Habilite a edição novamente.')
        if found['database'] != database or found['fingerprint'] != fingerprint():
            raise HTTPException(409, 'A conexão ou o banco mudou. Abra uma nova sessão de edição.')
        return found.copy()


def identifier(text):
    value = parse_json(text, maximum=4096)
    if isinstance(value, list):
        fail('Identificador de documento inválido.')
    return value


class DocumentRequest(BaseModel):
    model_config = ConfigDict(extra='forbid')
    database: str = Field(min_length=1, max_length=120)
    collection: str = Field(min_length=1, max_length=120)
    id_json: str = Field(min_length=1, max_length=4096)


class ChangeRequest(BaseModel):
    model_config = ConfigDict(extra='forbid')
    database: str = Field(min_length=1, max_length=120)
    collection: str = Field(min_length=1, max_length=120)
    operation: Literal['insert', 'update', 'delete']
    id_json: str | None = Field(default=None, max_length=4096)
    original_json: str | None = Field(default=None, max_length=MAX_JSON)
    document_json: str | None = Field(default=None, max_length=MAX_JSON)


def validate_document(document, depth=0):
    if depth > 30:
        fail('Documento com mais de 30 níveis não é suportado pelo editor.')
    if isinstance(document, dict):
        for key, value in document.items():
            if key.startswith('$') or '.' in key or '\x00' in key:
                fail('Campos com ponto, NUL ou prefixo $ não podem ser gravados neste editor.')
            validate_document(value, depth + 1)
    elif isinstance(document, list):
        for value in document:
            validate_document(value, depth + 1)


def same_document(left, right):
    # MongoDB moves _id to the front on insert. Root JSON key order is not an
    # edit conflict; individual BSON values/types (including nested order) are.
    return isinstance(left, dict) and isinstance(right, dict) and left.keys() == right.keys() and all(ejson(left[k]) == ejson(right[k]) for k in left)


def put_change(key, body):
    store.write('INSERT OR REPLACE INTO editor_changes VALUES(?,?,?)', (key, store.dump(body), body['created']))


def get_change(key):
    rows = store.read('SELECT body FROM editor_changes WHERE id=?', (key,))
    return json.loads(rows[0]['body']) if rows else None


def visible_change(key, token, owner):
    body = get_change(key)
    if not body:
        raise HTTPException(404, 'Alteração não encontrada.')
    access = session(token, owner, body['database'])
    if body.get('journal_owner') != journal_owner() or body.get('connection_identity') != access.get('connection_identity'):
        raise HTTPException(403, 'Esta cópia pertence a outra conexão ou usuário do painel.')
    if not permitted(access['grants'], body['database'], body['collection'], 'find'):
        raise HTTPException(403, 'A conta não tem permissão de leitura para esta coleção.')
    return {k:body[k] for k in ('id','database','collection','operation','id_json','before_json','after_json','created','status')}


def preview(req, token, owner):
    access = session(token, owner, req.database)
    action = {'insert':'insert', 'update':'update', 'delete':'remove'}[req.operation]
    if not permitted(access['grants'], req.database, req.collection, action):
        raise HTTPException(403, 'A conta não tem permissão para esta operação/coleção.')
    before, after, ident = None, None, None
    try:
        with editor_client() as client:
            coll = selected_collection(client, req.database, req.collection, write=True)
            if req.operation != 'insert':
                if req.id_json is None or req.original_json is None:
                    fail('Abra o documento completo antes de editar ou excluir.')
                ident = identifier(req.id_json)
                before = coll.find_one({'_id': {'$eq': ident}}, max_time_ms=5000)
                if before is None:
                    raise HTTPException(404, 'O documento não existe mais.')
                if not same_document(before, parse_json(req.original_json)):
                    raise HTTPException(409, 'O documento mudou desde a leitura. Recarregue antes de editar.')
            if req.operation != 'delete':
                after = parse_json(req.document_json or '')
                if not isinstance(after, dict):
                    fail('Informe um documento JSON do tipo objeto.')
                validate_document(after)
                if req.operation == 'insert':
                    after.setdefault('_id', ObjectId())
                    ident = after['_id']
                    after = {'_id':ident, **{k:v for k,v in after.items() if k != '_id'}}
                    if coll.find_one({'_id': {'$eq': ident}}, {'_id':1}, max_time_ms=5000) is not None:
                        raise HTTPException(409, 'Já existe um documento com este _id.')
                elif '_id' not in after or ejson(after['_id']) != ejson(ident):
                    fail('O campo _id não pode ser alterado ou removido.')
            # Force BSON validation before persisting a preview, not during apply.
            from bson import BSON
            if after is not None:
                BSON.encode(after)
                if len(ejson(after).encode('utf-8')) > MAX_JSON:
                    fail('Documento acima de 1 MiB após serialização BSON/JSON.')
            if before is not None and len(ejson(before).encode('utf-8')) > MAX_JSON:
                fail('Documento acima de 1 MiB; este editor não pode alterá-lo.')
    except (InvalidDocument, OverflowError):
        fail('O documento contém um tipo ou número não suportado pelo BSON.')
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível preparar a alteração. Nenhuma escrita foi enviada.') from None
    if before is not None and after is not None and same_document(before, after):
        fail('Não há alterações para aplicar.')
    key = store.identifier()
    body = {'id': key, 'database': req.database, 'collection': req.collection, 'operation': req.operation,
            'id_json': ejson(ident), 'before_json': ejson(before), 'after_json': ejson(after),
            'created': store.now(), 'expires': time.time() + PREVIEW_SECONDS,
            'status':'preview', 'session_hash':digest(token), 'actor':owner, 'fingerprint':access['fingerprint'],
            'journal_owner':journal_owner(), 'connection_identity':access.get('connection_identity', connection_identity())}
    # Durable original/plan exists before any MongoDB mutation can be attempted.
    put_change(key, body)
    return {k:body[k] for k in ('id','database','collection','operation','id_json','before_json','after_json','expires','status')}


def guarded_filter(before, after=None):
    clauses = [{field:{'$eq':value}} for field, value in before.items()]
    if after is not None:
        clauses.extend({field:{'$exists':False}} for field in after if field not in before)
    return {'$and': clauses}


def apply_change(key, token, owner, confirmation):
    body = get_change(key)
    if not body:
        raise HTTPException(404, 'Prévia não encontrada.')
    session(token, owner, body['database'])
    if body['session_hash'] != digest(token) or body['actor'] != owner or body['fingerprint'] != fingerprint():
        raise HTTPException(403, 'A prévia pertence a outra sessão.')
    if confirmation != body['collection']:
        fail('Confirme o nome exato da coleção antes de aplicar.')
    with store.connect() as db:
        db.execute('BEGIN IMMEDIATE')
        stored = db.execute('SELECT body FROM editor_changes WHERE id=?', (key,)).fetchone()
        current = json.loads(stored['body'])
        if current['status'] != 'preview':
            raise HTTPException(409, 'Esta alteração já foi enviada. Recarregue o documento; não repita a aplicação.')
        if current['expires'] < time.time():
            raise HTTPException(409, 'A prévia expirou. Prepare uma nova alteração.')
        body.update(status='applying', submitted_at=store.now())
        db.execute('UPDATE editor_changes SET body=? WHERE id=?', (store.dump(body), key))
    before, after = parse_json(body['before_json']), parse_json(body['after_json'])
    try:
        with editor_client() as client:
            coll = selected_collection(client, body['database'], body['collection'], write=True)
            if body['operation'] == 'insert':
                coll.insert_one(after)
            elif body['operation'] == 'update':
                # Patch only changed top-level fields: concurrent additions to other
                # fields survive. Nested changes compare the entire original parent.
                changes = {k:v for k,v in after.items() if k != '_id' and (k not in before or ejson(v) != ejson(before[k]))}
                removed = {k:'' for k in before if k != '_id' and k not in after}
                update = {}
                if changes: update['$set'] = changes
                if removed: update['$unset'] = removed
                actual = coll.find_one_and_update(guarded_filter(before, after), update, return_document=ReturnDocument.BEFORE, maxTimeMS=5000)
                if actual is None:
                    raise HTTPException(409, 'Conflito: o documento mudou ou foi removido. Nenhuma alteração foi aplicada.')
                body['before_json'] = ejson(actual)
                result = dict(actual)
                result.update(changes)
                for field in removed: result.pop(field, None)
                body['after_json'] = ejson(result)
            else:
                actual = coll.find_one_and_delete(guarded_filter(before), maxTimeMS=5000)
                if actual is None:
                    raise HTTPException(409, 'Conflito: o documento mudou ou foi removido. Nada foi excluído.')
                body['before_json'] = ejson(actual)
        body.update(status='applied', completed_at=store.now())
        put_change(key, body)
        store.event('editor', f'{body["operation"]} applied; database={body["database"]}; collection={body["collection"]}; change={key}')
        return {'id':key,'status':'applied','operation':body['operation'], 'before_json':body['before_json'], 'after_json':body['after_json']}
    except DuplicateKeyError as exc:
        body.update(status='failed', error=type(exc).__name__)
        put_change(key, body)
        raise HTTPException(409, 'O MongoDB recusou a escrita por chave duplicada. Recarregue antes de tentar novamente.') from None
    except OperationFailure as exc:
        # Write concern/time-limit errors may arrive after a mutation committed.
        definite_rejection = exc.code in {2, 9, 13, 14, 18, 20, 66, 73, 121}
        body.update(status='failed' if definite_rejection else 'unknown', error=type(exc).__name__)
        put_change(key, body)
        if definite_rejection:
            raise HTTPException(409, 'O MongoDB recusou a escrita por permissão, validação ou formato. Recarregue antes de tentar novamente.') from None
        raise HTTPException(503, 'O MongoDB não confirmou o resultado da escrita. Não repita a aplicação: recarregue e confira a cópia local.') from None
    except HTTPException:
        body.update(status='failed', error='conflict_or_access')
        put_change(key, body)
        raise
    except Exception:
        # A transport/local-journal failure can occur after the server committed.
        # Never automatically retry an uncertain write.
        body.update(status='unknown', error='outcome_requires_reload')
        try: put_change(key, body)
        except Exception: pass
        raise HTTPException(503, 'Resultado da escrita incerto. Não repita a aplicação: recarregue o documento e confira a cópia local.') from None


@router.get('/databases')
def databases():
    import app
    from insight_connection import current_settings
    try:
        with editor_client() as client:
            try:
                names = sorted(client.list_database_names())
            except OperationFailure as exc:
                # MongoDB 3.4 does not grant cluster-wide listDatabases to a
                # database-scoped readWrite user. The configured DB is sufficient.
                if exc.code != 13 or not app.MONGO_DATABASE:
                    raise
                if not client[app.MONGO_DATABASE].list_collection_names(maxTimeMS=5000):
                    raise HTTPException(404, 'O banco configurado está vazio ou não está acessível.')
                names = [app.MONGO_DATABASE]
        config = current_settings()
        return {'databases':names, 'selected':app.MONGO_DATABASE, 'host':config['host'], 'port':config['port'],
                'username':config['username'], 'session_seconds':SESSION_SECONDS}
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível listar os bancos disponíveis à conta atual.') from None


@router.get('/collections')
def collections(database: str):
    database_name(database)
    try:
        with editor_client() as client:
            return {'database':database, 'collections':sorted(client[database].list_collection_names(maxTimeMS=5000))}
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível listar as coleções.') from None


@router.post('/query')
def query(req: QueryRequest):
    database_name(req.database)
    plan = compile_query(req)
    documents, size, more = [], 0, False
    try:
        with editor_client() as client:
            coll = selected_collection(client, req.database, plan['collection'])
            cursor = coll.find(plan['filter'], plan['projection'] or None).sort(plan['sort']).skip(plan['offset']).limit(plan['limit'] + 1).max_time_ms(5000)
            try:
                for document in cursor:
                    text = ejson(document)
                    if len(text.encode('utf-8')) > MAX_JSON:
                        fail('Um documento excede 1 MiB. Use projeção para selecionar menos campos.')
                    if len(documents) >= plan['limit'] or size + len(text.encode('utf-8')) > 2 * MAX_JSON:
                        more = True
                        break
                    documents.append({'id_json':ejson(document['_id']) if '_id' in document else None, 'json':text})
                    size += len(text.encode('utf-8'))
            finally:
                cursor.close()
    except PyMongoError:
        raise HTTPException(422, 'Consulta recusada ou tempo limite de 5 segundos excedido. Confira tipos, campos e filtros.') from None
    return {'database':req.database, 'collection':plan['collection'], 'documents':documents,
            'offset':plan['offset'], 'limit':plan['limit'], 'has_more':more,
            'translated':{'filter':ejson(plan['filter']), 'projection':ejson(plan['projection']), 'sort':plan['sort']}}


@router.post('/document')
def document(req: DocumentRequest):
    ident = identifier(req.id_json)
    try:
        with editor_client() as client:
            found = selected_collection(client, req.database, req.collection).find_one({'_id':{'$eq':ident}}, max_time_ms=5000)
    except PyMongoError:
        raise HTTPException(503, 'Não foi possível abrir o documento.') from None
    if found is None:
        raise HTTPException(404, 'Documento não encontrado.')
    text = ejson(found)
    if len(text.encode('utf-8')) > MAX_JSON:
        fail('Este documento excede o limite de edição de 1 MiB.')
    return {'id_json':ejson(found['_id']), 'json':text}


@router.post('/unlock')
def enable_editing(req: UnlockRequest, request: Request):
    return unlock(req, actor(request))


@router.post('/lock')
def lock_editing(request: Request, x_insight_editor_session: str = Header(default='')):
    owner = actor(request)
    with _lock:
        key = digest(x_insight_editor_session)
        if _sessions.get(key, {}).get('actor') == owner:
            _sessions.pop(key)
    return {'locked':True}


@router.get('/history')
def history(database: str, request: Request, x_insight_editor_session: str = Header(default='')):
    owner = actor(request)
    access = session(x_insight_editor_session, owner, database)
    rows = store.read("""SELECT id,created,json_extract(body,'$.collection') AS collection,
        json_extract(body,'$.operation') AS operation,json_extract(body,'$.status') AS status
        FROM editor_changes WHERE json_extract(body,'$.database')=? AND json_extract(body,'$.journal_owner')=?
        AND json_extract(body,'$.connection_identity')=? AND json_extract(body,'$.status')!='preview'
        ORDER BY created DESC,rowid DESC LIMIT 50""", (database,journal_owner(),access['connection_identity']))
    return {'changes':[row for row in rows if permitted(access['grants'], database, row['collection'], 'find')]}


@router.get('/history/{key}')
def history_copy(key: str, request: Request, x_insight_editor_session: str = Header(default='')):
    return visible_change(key, x_insight_editor_session, actor(request))


@router.post('/preview')
def prepare_change(req: ChangeRequest, request: Request, x_insight_editor_session: str = Header(default='')):
    return preview(req, x_insight_editor_session, actor(request))


class ApplyRequest(BaseModel):
    model_config = ConfigDict(extra='forbid')
    confirmation: str = Field(max_length=120)


@router.post('/apply/{key}')
def commit_change(key: str, req: ApplyRequest, request: Request, x_insight_editor_session: str = Header(default='')):
    return apply_change(key, x_insight_editor_session, actor(request), req.confirmation)
