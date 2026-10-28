"""Attach native screen labels/literal choices to verified service parameters."""
import json
from pathlib import Path
import re

ROOT=Path(__file__).resolve().parents[1]


def balanced(text,start,opening='(',closing=')'):
    depth=0;quoted=False;escape=False
    for pos in range(start,len(text)):
        char=text[pos]
        if quoted:
            if escape:escape=False
            elif char=='\\':escape=True
            elif char=='"':quoted=False
        elif char=='"':quoted=True
        elif char==opening:depth+=1
        elif char==closing:
            depth-=1
            if depth==0:return text[start+1:pos]
    return ''


def split_arguments(text):
    parts=[];start=0;depth=0;quoted=False;escape=False
    for pos,char in enumerate(text):
        if quoted:
            if escape:escape=False
            elif char=='\\':escape=True
            elif char=='"':quoted=False
        elif char=='"':quoted=True
        elif char in '([{':depth+=1
        elif char in ')]}':depth-=1
        elif char==',' and depth==0:parts.append(text[start:pos].strip());start=pos+1
    parts.append(text[start:].strip())
    return parts


def enrich(result):
    cache={};labels=choices=0
    for report_id,report in result['reports'].items():
        texts=[]
        for source in report['sources']:
            path=ROOT/source['path']
            cache.setdefault(str(path),path.read_text(encoding='utf-8-sig'))
            texts.append(cache[str(path)])
        text='\n'.join(texts)
        for call in report['calls']:
            params=call.get('parameters',[])
            candidates=[split_arguments(balanced(text,match.end()-1)) for match in re.finditer(r'\b\w+\.'+re.escape(call['method'])+r'\s*\(',text)]
            args=next((a for a in candidates if len(a)==len(params)),[])
            for index,param in enumerate(params):
                expression=args[index] if index<len(args) else ''
                param['source_expression']=expression
                member=re.search(r'this\.(\w+)',expression)
                if member:
                    field=member.group(1)
                    initialization=re.search(r'this\.'+re.escape(field)+r'\s*=\s*new\s+\w+',text)
                    if initialization:
                        snippet=text[initialization.end():initialization.end()+4000].split(';',1)[0]
                        title=re.search(r'Titulo\s*=\s*("(?:[^"\\]|\\.)*")',snippet)
                        if title:
                            param['label']=json.loads(title[1]);labels+=1
                        for dto,lookup in [('ProdutoServicoEmpresaDto','company_products'),('ProdutoServicoDto','products'),('PessoaDto','people'),('ContaDto','accounts'),('TicketDto','tickets'),('GestaoContratoDto','contracts')]:
                            if dto in snippet:param['lookup']=lookup
                        default=re.search(r'\bValor\s*=\s*(true|false|\d+)\b',snippet)
                        if default:
                            param['default']=json.loads(default[1])
                        options=re.search(r'\bOpcoes\s*=\s*(\w+)',snippet)
                        if options:
                            declarations=list(re.finditer(r'\bList<CodigoDescricaoDto>\s+'+re.escape(options[1])+r'\s*=\s*new List<CodigoDescricaoDto>\s*\{',text[:initialization.start()]))
                            if declarations:
                                declaration=declarations[-1]
                                body=balanced(text,declaration.end()-1,'{','}')
                                literals=[]
                                for entry in re.finditer(r'new CodigoDescricaoDto\s*\{([^{}]*)\}',body):
                                    code=re.search(r'Codigo\s*=\s*(-?\d+)\b',entry[1]);description=re.search(r'Descricao\s*=\s*("(?:[^"\\]|\\.)*")',entry[1])
                                    if code and description:
                                        literals.append({'label':json.loads(description[1]),'value':{'Codigo':int(code[1]),'Descricao':json.loads(description[1])}})
                                if literals and len(literals)==len(re.findall(r'new CodigoDescricaoDto',body)):
                                    if param['kind'] in {'number','enum'}:
                                        literals=[{'label':v['label'],'value':v['value']['Codigo']} for v in literals]
                                    param['choices']=literals;param['choices_origin']='retained_ui_literals';choices+=1
                elif expression in {'true','false','""'} or re.fullmatch(r'-?\d+',expression):
                    param['default']=json.loads(expression)
                    param['fixed']=True
                param.setdefault('label',re.sub(r'(?<!^)(?=[A-Z])',' ',param['name']).replace('Referencia','').strip().capitalize())
                if param['kind']=='object' and 'CodigoDescricaoDto' in param['type'] and not param.get('choices'):
                    provider=None
                    if 'origem' in param['label'].lower():provider='OrigensPesquisaMovimentacao'
                    elif 'situa' in param['label'].lower() and 'movimenta' in param['label'].lower():provider='SituacoesMovimentacao'
                    values=result.get('value_choices',{}).get(provider,[])
                    if values:
                        if provider=='OrigensPesquisaMovimentacao' and 'MovimentacaoSaida' in report_id:
                            values=[v for v in values if any(i in v['interfaces'] for i in ['IOrigemSaida','IOrigemDocumentoFiscalSaida'])]
                        elif provider=='OrigensPesquisaMovimentacao' and 'MovimentacaoEntrada' in report_id:
                            values=[v for v in values if any(i in v['interfaces'] for i in ['IOrigemEntrada','IOrigemDocumentoFiscalEntrada'])]
                        param['choices']=[{'label':v['label'],'value':v['value']} for v in values]
                        param['choices_origin']='installed_value_objects'
    result['ui_enrichment']={'labels_from_retained_ui':labels,'literal_choice_lists':choices,
                             'limitations':'Labels/defaults/choices transcribed from retained UI sources; installed service parameter types are verified separately.'}
    return result


if __name__=='__main__':
    path=ROOT/'static/native-service-map.json'
    result=enrich(json.loads(path.read_text(encoding='utf-8')))
    path.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps(result['ui_enrichment'],ensure_ascii=False))
