"""Compara backups BSON em memoria; nao restaura nem escreve em banco."""
import gzip, hashlib, json, zipfile
from pathlib import Path
from datetime import datetime
import bson

PATHS = [Path(r'C:\DigiSat_Backup') / ('DigisatServer'+s+'.zip') for s in
         ['20261015_110611', '20261015_141048', '20261016_074652']]
PATHS.append(Path(r'D:\Documents\pasta coisas\DigisatServer20261016_104950.zip'))
TARGET = '6ad0d1f435c0e8154819c2a7'

def digest(x):
    return hashlib.sha256(bson.BSON.encode(x)).hexdigest()

def hits(x, path=''):
    result=[]
    if isinstance(x, dict):
        for k,v in x.items():
            p=path+'.'+k
            if isinstance(v,(int,float)) and not isinstance(v,bool) and abs(v) in (20,355.8,411.8) and any(w in k.lower() for w in ('valor','total','credito','desconto','acrescimo','troco')):
                result.append([p,v])
            elif isinstance(v,(dict,list)): result.extend(hits(v,p))
    elif isinstance(x,list):
        for i,v in enumerate(x): result.extend(hits(v,path+f'[{i}]'))
    return result

def scan(p):
    summary={'backup':str(p),'sha256':hashlib.sha256(p.read_bytes()).hexdigest(), 'colecoes':{},'valores':[]}
    with zipfile.ZipFile(p) as z:
        for name in z.namelist():
            if not name.endswith('.bson.gz'): continue
            n=name[:-8]; raw=gzip.decompress(z.read(name)); ds=bson.decode_all(raw)
            summary['colecoes'][n]={'quantidade':len(ds),'sha256_bson':hashlib.sha256(raw).hexdigest()}
            if n=='Turnos':
                summary['turnos']=[{'id':str(d['_id']),'empresa':str(d.get('EmpresaReferencia')), 'abertura':d.get('TurnoAbertura',{}).get('DataHora'),'fechamento':d.get('TurnoFechamento',{}).get('DataHora')} for d in ds]
                target=next((d for d in ds if str(d['_id'])==TARGET),None)
                summary['turno_alvo']=None if target is None else {
                    'id':str(target['_id']), 'empresa':str(target.get('EmpresaReferencia')),
                    'abertura':target.get('TurnoAbertura',{}).get('DataHora'),
                    'fechamento':target.get('TurnoFechamento',{}).get('DataHora'),
                    'credito_loja_fechamento':target.get('TurnoFechamento',{}).get('CreditoLoja')}
            for d in ds:
                h=hits(d)
                if h: summary['valores'].append({'colecao':n,'id':str(d.get('_id')),'data':d.get('DataHora',d.get('DataEmissao',d.get('DataHoraEmissao'))),'campos':h})
    return summary

results=[scan(p) for p in PATHS]
reference=results[-1]
for r in results[:-1]:
    r['comparacao_usuario']={
        'colecoes_iguais':[n for n,v in r['colecoes'].items() if v==reference['colecoes'].get(n)],
        'colecoes_diferentes':[n for n,v in r['colecoes'].items() if v!=reference['colecoes'].get(n)],
        'ids_turnos_comuns': sorted(set(t['id'] for t in r['turnos']) & set(t['id'] for t in reference['turnos']))}
Path('docs/evidencias-backups-historicos-caixa.json').write_text(json.dumps(results,default=str,ensure_ascii=False,indent=2),encoding='utf-8')
for r in results:
    print(json.dumps({'backup':r['backup'],'turnos':len(r['turnos']),'empresas':sorted(set(t['empresa'] for t in r['turnos'])),'alvo_presente':r['turno_alvo'] is not None,'valores_quantidade':len(r['valores']),'contagens':{n:r['colecoes'][n]['quantidade'] for n in ['TurnosLancamentos','Recebimentos','Movimentacoes','ItensCreditoDebitoCliente']},'turnos_comuns':r.get('comparacao_usuario',{}).get('ids_turnos_comuns')},ensure_ascii=False))
