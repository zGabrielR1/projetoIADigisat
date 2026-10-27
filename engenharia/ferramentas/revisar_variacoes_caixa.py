import zipfile,gzip,json
from pathlib import Path
from datetime import datetime
from decimal import Decimal
import bson

def nums(x,p=''):
    out={}
    if isinstance(x,dict):
        for k,v in x.items():out.update(nums(v,p+'.'+k))
    elif isinstance(x,list):
        for i,v in enumerate(x):out.update(nums(v,p+f'[{i}]'))
    elif isinstance(x,(int,float)) and not isinstance(x,bool):out[p]=x
    return out

def dated(x):
    if isinstance(x,datetime):return x.date().isoformat()=='2026-10-15'
    if isinstance(x,dict):return any(dated(v) for k,v in x.items() if k not in ['Pessoa','Cliente','Fornecedor','ProdutosServicos'])
    if isinstance(x,list):return any(dated(v) for v in x)
    return False

def histories(x,p=''):
    out=[]
    if isinstance(x,dict):
        for k,v in x.items():
            if isinstance(v,list) and 'historic' in k.lower():
                for a,b in zip(v,v[1:]):
                    na,nb=nums(a),nums(b)
                    changes=[{'campo':key,'antes':na[key],'depois':nb[key],'delta':str(Decimal(str(nb[key]))-Decimal(str(na[key])))} for key in na.keys() & nb.keys() if na[key]!=nb[key]]
                    if changes:out.append({'caminho':p+'.'+k,'data_antes':a.get('Data'),'data_depois':b.get('Data'),'mudancas':changes})
            out.extend(histories(v,p+'.'+k))
    elif isinstance(x,list):
        for i,v in enumerate(x):out.extend(histories(v,p+f'[{i}]'))
    return out

result={'ocorrencias_numericas_exatas':[],'documentos_dia_numeros':[],'historicos_com_mudancas':[],'venda474':[]}
with zipfile.ZipFile(r'D:\Documents\pasta coisas\DigisatServer20261016_104950.zip') as z:
    for name in z.namelist():
        if not name.endswith('.bson.gz'):continue
        n=name[:-8]
        for d in bson.decode_all(gzip.decompress(z.read(name))):
            nd=nums(d);base={'colecao':n,'id':str(d.get('_id'))}
            hit={k:v for k,v in nd.items() if abs(v) in (20,355.8,411.8)}
            if hit:result['ocorrencias_numericas_exatas'].append(dict(base,campos=hit))
            if dated(d):
                # Exclui entidades embutidas para não exportar detalhes pessoais nem fiscais sem relação.
                money={k:v for k,v in nd.items() if not any(q in k for q in ['.Pessoa.','.Produto.','.ProdutosServicos.','.Emitente.','.Destinatario.']) and any(q in k.lower() for q in ['valor','total','credito','dinheiro','cartao','saldo','troco','desconto','acrescimo','cheque','duplicata','debito','convenio','deposito','pix'])}
                if money:result['documentos_dia_numeros'].append(dict(base,campos=money))
                h=histories(d)
                if h:result['historicos_com_mudancas'].append(dict(base,historicos=h))
            if n=='Movimentacoes' and (str(d.get('Numero'))=='474' or str(d.get('Documento'))=='474'):
                result['venda474'].append(dict(base,numeros=nd,campos_raiz=list(d.keys())))
Path('docs/evidencias-variacoes-caixa.json').write_text(json.dumps(result,default=str,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({'historicos_com_mudancas':result['historicos_com_mudancas'],'exatos':result['ocorrencias_numericas_exatas'],'venda474':result['venda474']},default=str,ensure_ascii=False))
