"""Auditoria offline: lê BSON diretamente do ZIP, sem restaurar ou escrever no MongoDB."""
import argparse, gzip, hashlib, json, zipfile
from datetime import datetime
from decimal import Decimal
from pathlib import Path
import bson

def audit(path):
    names = ['Turnos','TurnosLancamentos','Recebimentos','ItensCreditoDebitoCliente','MovimentosConta','Movimentacoes']
    with zipfile.ZipFile(path) as z:
        data={n:bson.decode_all(gzip.decompress(z.read(n+'.bson.gz'))) for n in names}
    turn=next(d for d in data['Turnos'] if str(d['_id'])=='6ad0d1f435c0e8154819c2a7')
    begin=turn['TurnoAbertura']['DataHora'];end=turn['TurnoFechamento']['DataHora']
    rows=[d for d in data['TurnosLancamentos'] if begin<=d.get('DataHora',datetime.min)<=end and d.get('UsuarioReferencia')==turn['UsuarioReferencia']]
    credit=[d for d in rows if d.get('EspecieRecebimento',{}).get('_t')=='CreditoLoja' or d.get('EspeciePagamento',{}).get('EspecieRecebimento',{}).get('_t')=='CreditoLoja']
    def compact(d):
        return {k:d[k] for k in ['_id','_t','DataHora','Documento','Ordem','Valor','ValorQuitacao','Ativo','Cancelado','Versao','OrigemReferencia','Observacao'] if k in d}
    history=[]
    for d in data['Recebimentos']:
        if d.get('Documento') in ['474','CRE15102026']:
            history.append({'id':d['_id'],'documento':d['Documento'],'ordem':d['Ordem'],'ativo':d['Ativo'],'situacao':d['Situacao'],'observacao':d.get('Observacao'),'historico':[{k:h[k] for k in ['_t','Data','NomeUsuario','Valor','Observacao'] if k in h} for h in d['Historico']]})
    # Busca somente campos monetários, incluindo histórico e documentos inativos.
    def money_hits(x,p=''):
        hits=[]
        if isinstance(x,dict):
            for k,v in x.items():
                q=p+'.'+k
                if isinstance(v,(float,int)) and not isinstance(v,bool) and abs(v) in (20,355.8,411.8) and any(t in k.lower() for t in ['valor','total','credito','desconto','acrescimo','troco']):hits.append([q,v])
                elif isinstance(v,(dict,list)):hits+=money_hits(v,q)
        elif isinstance(x,list):
            for i,v in enumerate(x):hits+=money_hits(v,p+f'[{i}]')
        return hits
    hits=[]
    for n,docs in data.items():
        for d in docs:
            h=money_hits(d)
            if h:hits.append({'colecao':n,'id':d['_id'],'documento':d.get('Documento',d.get('Numero')),'data':d.get('DataHora',d.get('DataEmissao',d.get('DataHoraEmissao'))),'ocorrencias':h})
    valid=sum((Decimal(str(d.get('ValorQuitacao',0))) for d in credit if d.get('Ativo') and not d.get('Cancelado') and 'TurnoFinanceiroQuitado' in d['_t']),Decimal(0))
    return {'backup':str(path),'sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'contagens':{n:len(v) for n,v in data.items()},'turno':turn,'credito_turno':[compact(d) for d in credit],'calculado_credito':str(valid),'historicos':history,'busca_valores_monetarios':hits,'saldo_renata':str(sum((Decimal(str(d['Valor'])) for d in data['ItensCreditoDebitoCliente'] if str(d['PessoaReferencia'])=='6a1f3d3735c0e8235cef8041'),Decimal(0)))}
if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('backup',type=Path);p.add_argument('--output',type=Path,required=True);a=p.parse_args()
    result=audit(a.backup);a.output.write_text(json.dumps(result,default=str,ensure_ascii=False,indent=2),encoding='utf-8')
    print(json.dumps({k:result[k] for k in ['contagens','calculado_credito','saldo_renata','busca_valores_monetarios']},default=str,ensure_ascii=False,indent=2))
