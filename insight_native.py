"""Bounded access to installed SuiteG6 report services through its normal login.

The fixed map is verified against the installed service contracts. No arbitrary
contract, method, assembly, server address, or executable comes from an API user.
"""
from __future__ import annotations
from datetime import datetime
from decimal import Decimal
import hashlib
import json
import os
from pathlib import Path
import re
import secrets
import subprocess
import threading
from time import monotonic
from typing import Any

from fastapi import APIRouter, HTTPException, Header
from pydantic import BaseModel, ConfigDict, Field, SecretStr, field_validator
from bson import ObjectId
from pymongo.errors import PyMongoError
from insight_upstream_issues import issues_for_call

ROOT=Path(__file__).resolve().parent
MAP=ROOT/'static/native-service-map.json'
SUITE=Path(os.environ.get('INSIGHT_NATIVE_SUITE_PATH',r'C:\Digisat\SuiteG6\Sistema'))
PORT=int(os.environ.get('INSIGHT_NATIVE_PORT','12221'))
HOST=ROOT/'native/NativeReportHost.exe'
if not HOST.is_file():
    HOST=ROOT/'.release-build/native-host/NativeReportHost.exe'
MAX_OUTPUT=16*1024*1024
MAX_ROWS=50000
SESSION_SECONDS=20*60
_sessions={}
_lock=threading.RLock()
_workers=threading.BoundedSemaphore(2)
router=APIRouter(prefix='/api/native',tags=['SuiteG6 native reports'])

class ReportSearch(BaseModel):
    model_config=ConfigDict(extra='forbid')
    question:str=Field(min_length=2,max_length=500)
    limit:int=Field(default=8,ge=1,le=20)

@router.post('/search')
def search_reports(req:ReportSearch):
    from insight_report_search import discover
    return discover(req.question,req.limit)

class FilterPreview(BaseModel):
    model_config=ConfigDict(extra='forbid')
    report_id:str=Field(min_length=1,max_length=160)
    call_index:int=Field(default=0,ge=0,le=20)
    arguments:dict[str,Any]=Field(default_factory=dict)

@router.post('/preview')
def preview_filters(req:FilterPreview):
    report=catalog()['reports'].get(req.report_id)
    if not report or req.call_index>=len(report['calls']):raise HTTPException(404,'Relatório não localizado.')
    call=report['calls'][req.call_index]
    if not call.get('available'):raise HTTPException(422,'Conjunto indisponível.')
    if len(json.dumps(req.arguments).encode())>8192:raise HTTPException(422,'Sugestões excedem o limite permitido.')
    parameters={p['name']:p for p in call['parameters']}
    validated={};summary=[]
    for name,value in req.arguments.items():
        p=parameters.get(name)
        if not p or p.get('fixed') or p['kind'] not in {'boolean','datetime','number','enum','object'}:
            raise HTTPException(422,'Sugestão não permitida para este filtro: '+name)
        if p['kind']=='object' and not p.get('choices'):raise HTTPException(422,'Objeto livre não permitido nas sugestões.')
        clean=validate_value(value,p)
        choices=p.get('choices') or []
        selected=next((c for c in choices if c['value']==clean),None)
        if choices and not selected:raise HTTPException(422,'Opção fora do catálogo permitido: '+name)
        validated[name]=clean
        summary.append({'name':name,'label':p.get('label') or name,'display':selected['label'] if selected else str(clean)})
    return {'report_id':req.report_id,'call_index':req.call_index,'arguments':validated,'summary':summary,'executed':False}


class ConnectRequest(BaseModel):
    model_config=ConfigDict(extra='forbid')
    username: str=Field(min_length=1,max_length=100)
    password: SecretStr
    company_id: str=Field(pattern=r'^[0-9a-fA-F]{24}$')

    @field_validator('password')
    @classmethod
    def password_length(cls,value):
        if not 1<=len(value.get_secret_value())<=256:
            raise ValueError('Informe uma senha válida do SuiteG6.')
        return value


class NativeQuery(BaseModel):
    model_config=ConfigDict(extra='forbid')
    report_id: str=Field(min_length=1,max_length=160)
    call_index: int=Field(default=0,ge=0,le=20)
    arguments: dict[str,Any]=Field(default_factory=dict)
    dataset: str=Field(default='$',max_length=250)
    offset: int=Field(default=0,ge=0,le=MAX_ROWS)
    page_size: int=Field(default=50,ge=1,le=200)


def catalog():
    try:
        result=json.loads(MAP.read_text(encoding='utf-8'))
    except (OSError,ValueError):
        raise HTTPException(503,'Mapa dos relatórios nativos indisponível. Recompile a integração.') from None
    return result


def host(payload):
    if not HOST.is_file():
        raise HTTPException(503,'O componente local de relatórios ainda não foi compilado.')
    if not _workers.acquire(blocking=False):
        raise HTTPException(429,'Duas consultas nativas estão em andamento. Aguarde e tente novamente.')
    process=None
    try:
        process=subprocess.Popen([str(HOST),str(SUITE),str(MAP)],stdin=subprocess.PIPE,stdout=subprocess.PIPE,stderr=subprocess.PIPE,
            cwd=str(ROOT),creationflags=subprocess.CREATE_NO_WINDOW if os.name=='nt' else 0)
        chunks=[];errors=[];oversize=threading.Event()
        def read_bounded(stream,target,limit):
            total=0
            while True:
                chunk=stream.read(32768)
                if not chunk:break
                total+=len(chunk)
                if total>limit:
                    oversize.set();process.kill();break
                target.append(chunk)
        readers=[threading.Thread(target=read_bounded,args=(process.stdout,chunks,MAX_OUTPUT),daemon=True),
                 threading.Thread(target=read_bounded,args=(process.stderr,errors,65536),daemon=True)]
        for reader in readers:reader.start()
        process.stdin.write(json.dumps(payload,ensure_ascii=False).encode('utf-8'));process.stdin.close()
        process.wait(timeout=65)
        for reader in readers:reader.join(timeout=3)
        if oversize.is_set():
            raise HTTPException(422,'O relatório excedeu o limite de resposta. Reduza o período ou os filtros.')
        try:
            response=json.loads(b''.join(chunks).decode('utf-8-sig'),parse_float=Decimal)
        except (ValueError,UnicodeError):
            raise HTTPException(502,'O componente nativo não retornou uma resposta válida.') from None
        if not response.get('ok'):
            message=str(response.get('error') or 'O SuiteG6 recusou a consulta.')
            password=payload.get('password')
            if password:message=message.replace(password,'[redigido]')
            raise HTTPException(422,message[:1200])
        return response
    except subprocess.TimeoutExpired:
        raise HTTPException(504,'O serviço nativo excedeu o prazo. Reduza o período e tente novamente.') from None
    except OSError:
        raise HTTPException(503,'Não foi possível iniciar o componente .NET do SuiteG6.') from None
    finally:
        if process:
            if process.poll() is None:process.kill()
            try:process.wait(timeout=5)
            except subprocess.TimeoutExpired:pass
            for stream in [process.stdin,process.stdout,process.stderr]:
                if stream:
                    try:stream.close()
                    except OSError:pass
        _workers.release()


def get_session(token):
    if not token or len(token)>150:
        raise HTTPException(401,'Conecte um usuário SuiteG6 para consultar relatórios nativos.')
    key=hashlib.sha256(token.encode()).hexdigest()
    with _lock:
        value=_sessions.get(key)
        if not value or value['expires']<=monotonic():
            _sessions.pop(key,None)
            raise HTTPException(401,'A conexão com o SuiteG6 expirou. Entre novamente.')
        return dict(value)


@router.get('/catalog')
def native_catalog():
    result=catalog()
    return {'reports':[{**{k:v for k,v in report.items() if k!='sources'},'id':rid,
                        'calls':[{**call,'upstream_issues':issues_for_call(call)} for call in report['calls']]}
                       for rid,report in result['reports'].items()],
            'installed_assemblies':result.get('installed_assemblies',[]),
            'available':sum(any(c.get('available') for c in r['calls']) for r in result['reports'].values()),
            'limitations':result['limitations']}


@router.post('/connect')
def connect(req:ConnectRequest):
    payload={'operation':'login','username':req.username,'password':req.password.get_secret_value(),
             'company_id':req.company_id,'port':PORT}
    host(payload)
    token=secrets.token_urlsafe(32)
    with _lock:
        now=monotonic()
        for key in [k for k,v in _sessions.items() if v['expires']<=now]:_sessions.pop(key,None)
        if len(_sessions)>=8:
            _sessions.pop(min(_sessions,key=lambda k:_sessions[k]['expires']),None)
        _sessions[hashlib.sha256(token.encode()).hexdigest()]={**payload,'expires':now+SESSION_SECONDS}
    return {'session':token,'username':req.username,'company_id':req.company_id,'expires_in_seconds':SESSION_SECONDS}


@router.post('/disconnect')
def disconnect(x_native_session:str|None=Header(default=None)):
    if x_native_session:
        with _lock:_sessions.pop(hashlib.sha256(x_native_session.encode()).hexdigest(),None)
    return {'disconnected':True}


@router.get('/lookup')
def lookup(kind:str,q:str,x_native_session:str|None=Header(default=None)):
    import app
    session=get_session(x_native_session)
    sources={'people':('Pessoas','Nome'),'accounts':('Contas','Descricao'),
             'products':('ProdutosServicos','Descricao'),'tickets':('Tickets','Descricao'),
             'contracts':('GestaoContratos','Descricao')}
    if kind not in {*sources,'company_products'} or not 2<=len(q.strip())<=100:
        raise HTTPException(422,'Informe de 2 a 100 caracteres para pesquisar um cadastro permitido.')
    try:
        with app.mongo_client() as client:
            db=client[app.MONGO_DATABASE]
            if kind=='company_products':
                match={'EmpresaReferencia':ObjectId(session['company_id'])}
                pipeline=[{'$match':match},{'$lookup':{'from':'ProdutosServicos','localField':'ProdutoServicoReferencia','foreignField':'_id','as':'product'}},
                          {'$unwind':'$product'},{'$match':{'product.Descricao':{'$regex':re.escape(q.strip()),'$options':'i'}}},
                          {'$limit':51},{'$project':{'_id':1,'label':'$product.Descricao'}}]
                records=list(db.ProdutosServicosEmpresa.aggregate(pipeline,maxTimeMS=10000,allowDiskUse=False))
            else:
                collection,field=sources[kind]
                match={'_id':ObjectId(q)} if ObjectId.is_valid(q) else {field:{'$regex':re.escape(q.strip()),'$options':'i'}}
                records=[{'_id':r['_id'],'label':r.get(field) or str(r['_id'])} for r in db[collection].find(match,{field:1}).limit(51).max_time_ms(10000)]
            return {'items':[{'id':str(r['_id']),'label':str(r['label'])} for r in records[:50]],'has_more':len(records)>50}
    except PyMongoError:
        raise HTTPException(503,'Não foi possível pesquisar o cadastro na conexão de dados do Insight.') from None


def validate_value(value,parameter,depth=0):
    if depth>4:raise HTTPException(422,'Parâmetro nativo excede a profundidade permitida.')
    kind=parameter['kind'];name=parameter['name']
    if parameter.get('fixed') and value!=parameter.get('default'):
        raise HTTPException(422,'Este filtro é fixo no relatório nativo: '+name)
    if kind=='text':
        if not isinstance(value,str) or len(value)>2000:raise HTTPException(422,'Texto inválido: '+name)
        return value
    if kind=='boolean':
        if not isinstance(value,bool):raise HTTPException(422,'Opção inválida: '+name)
        return value
    if kind=='datetime':
        try:
            parsed=datetime.fromisoformat(value)
            if parsed.tzinfo is not None:raise ValueError()
        except (ValueError,TypeError):raise HTTPException(422,'Data e hora local inválidas: '+name) from None
        return value
    if kind in {'number','enum'}:
        try:
            numeric=Decimal(str(value))
            if isinstance(value,bool) or not numeric.is_finite() or abs(numeric)>Decimal('1e28'):raise ValueError()
            if parameter['type'] not in {'System.Decimal','System.Double','System.Single'} and numeric!=int(numeric):raise ValueError()
        except (ValueError,ArithmeticError):raise HTTPException(422,'Número inválido: '+name) from None
        if kind=='enum' and parameter.get('choices') and int(numeric) not in {c['value'] for c in parameter['choices']}:
            raise HTTPException(422,'Opção fora do catálogo nativo: '+name)
        return str(numeric) if parameter['type']=='System.Decimal' else int(numeric) if numeric==int(numeric) else float(numeric)
    if kind=='array':
        if not isinstance(value,list) or len(value)>1000:raise HTTPException(422,'Lista inválida: '+name)
        element={'name':name,'kind':'number' if 'System.Int32' in parameter['type'] else 'text',
                 'type':'System.Int32' if 'System.Int32' in parameter['type'] else 'System.String'}
        return [validate_value(v,element,depth+1) for v in value]
    if kind=='object':
        if not isinstance(value,dict):raise HTTPException(422,'Opções inválidas: '+name)
        properties={p['name']:p for p in parameter.get('properties') or []}
        if any(k not in properties for k in value):raise HTTPException(422,'Campo nativo desconhecido: '+name)
        return {k:validate_value(v,properties[k],depth+1) for k,v in value.items()}
    raise HTTPException(422,'Tipo de parâmetro nativo ainda não suportado: '+name)


SENSITIVE=re.compile(r'senha|password|secret|credential|token|xmlkey|apikey|api_key|privatekey|chaveprivada|connectionstring|autenticacao',re.I)


def sanitize(value,depth=0):
    if depth>32:raise HTTPException(422,'A resposta nativa excede a profundidade permitida.')
    if isinstance(value,dict):return {k:sanitize(v,depth+1) for k,v in value.items() if not SENSITIVE.search(k)}
    if isinstance(value,list):return [sanitize(v,depth+1) for v in value]
    if isinstance(value,Decimal):return str(value)
    return value


def datasets(data):
    """Expose original DTO collections, preserving parent scalar context."""
    if data is None:return {'$':[]}
    result={}
    budget=[0]
    def walk(value,path,parents,depth):
        if depth>8:return
        if isinstance(value,list):
            rows=result.setdefault(path,[])
            for item in value:
                budget[0]+=1
                if budget[0]>MAX_ROWS:
                    raise HTTPException(422,'O relatório excede 50.000 registros hierárquicos; reduza o período.')
                rows.append({'context':parents,'record':item})
                walk(item,path+'[]',parents,depth+1)
        elif isinstance(value,dict):
            context={**parents,**{path+'.'+k:v for k,v in value.items() if not isinstance(v,(list,dict))}}
            for key,child in value.items():
                if isinstance(child,(dict,list)):walk(child,path+'.'+key,context,depth+1)
    if isinstance(data,dict):result['$']=[{'context':{},'record':data}]
    elif not isinstance(data,list):result['$']=[{'context':{},'record':{'Resultado':data}}]
    walk(data,'$',{},0)
    return result or {'$':[]}


@router.post('/query')
def query(req:NativeQuery,x_native_session:str|None=Header(default=None)):
    session=get_session(x_native_session)
    report=catalog()['reports'].get(req.report_id)
    if not report or req.call_index>=len(report['calls']):raise HTTPException(404,'Relatório nativo não localizado nesta instalação.')
    call=report['calls'][req.call_index]
    if not call.get('available'):raise HTTPException(422,'Esta operação não está no contrato instalado.')
    parameters={p['name']:p for p in call['parameters']}
    if set(req.arguments)!=set(parameters):raise HTTPException(422,'Preencha todos os parâmetros do conjunto de dados selecionado.')
    arguments={k:validate_value(v,parameters[k]) for k,v in req.arguments.items()}
    if len(json.dumps(arguments,ensure_ascii=False).encode('utf-8'))>256*1024:
        raise HTTPException(422,'Os parâmetros do relatório excedem 256 KiB.')
    started=monotonic()
    response=host({k:session[k] for k in ['username','password','company_id','port']}|
                  {'operation':'invoke','report_id':req.report_id,'call_index':req.call_index,'arguments':arguments})
    clean=None if response.get('is_null') else sanitize(response.get('data'))
    collections=datasets(clean)
    if req.dataset not in collections:raise HTTPException(422,'Escolha uma coleção de dados disponível neste resultado.')
    rows=collections[req.dataset]
    page=rows[req.offset:req.offset+req.page_size]
    upstream_issues=issues_for_call(call,arguments)
    return {'report_id':req.report_id,'title':report['title'],'rows':page,
            'upstream_issues':upstream_issues,
            'datasets':[{'path':path,'count':len(values)} for path,values in collections.items()],
            'pagination':{'offset':req.offset,'page_size':req.page_size,'has_more':req.offset+len(page)<len(rows),'total':len(rows)},
            'provenance':{'source':'SuiteG6 native WCF','company_id':session['company_id'],'username':session['username'],
                          'duration_ms':round((monotonic()-started)*1000,1),'queried_at':datetime.now().isoformat()},
            'warnings':['Valores e regras do conjunto retornados pelo serviço nativo. Filtros locais da classe de relatório e layout de impressão não são executados por esta conexão.',
                        'Cada consulta ou página refaz a leitura no SuiteG6. O CSV contém somente a página exibida.',
                        'Campos de autenticação e segredos de integração são removidos da resposta.']+
                       [issue['warning'] for issue in upstream_issues]}
