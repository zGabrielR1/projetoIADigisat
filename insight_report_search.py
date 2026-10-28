"""Local, explainable report discovery. Never generates or executes database code."""
from datetime import date
from difflib import SequenceMatcher
from functools import lru_cache
import json
from pathlib import Path
import re
import unicodedata
from urllib.parse import urlencode

ROOT=Path(__file__).resolve().parent
STOP=set('a ao aos as o os um uma de da das do dos em no nos na nas por para com e que quais qual quero ver mostrar mostre liste listar lista relatorio relatorios consulta consultar preciso saber gere gerar me meu minha este esta esse essa mes meses ano anos hoje ontem ultimo ultimos ultima ultimas dia dias janeiro fevereiro marco abril maio junho julho agosto setembro outubro novembro dezembro'.split())
ALIASES={
 'Pagamento':'contas a pagar|pagamentos|pagar|fornecedores a pagar',
 'Recebimento':'contas a receber|recebimentos|receber|clientes devedores',
 'Pessoa':'pessoas|clientes|fornecedores|aniversariantes|inadimplentes|ficha cadastral',
 'ProdutoServico':'produtos|servicos|estoque|inventario|precos|mercadorias',
 'Grupo':'por grupo|por subgrupo|grupos|subgrupos',
 'RelacaoVendedorCliente':'relacao vendedor cliente|clientes por vendedor',
 'MovimentoConta':'movimento de conta|extrato de conta|fluxo de caixa',
 'MovimentacaoEntrada':'compras|entradas|notas de entrada',
 'MovimentacaoSaida':'vendas|saidas|notas de saida',
 'ConhecimentoTransporteEletronico':'ct-e|cte|conhecimento de transporte',
 'ManifestoEletronicoDocumentoFiscal':'mdf-e|mdfe|manifesto eletronico',
 'Delivery':'delivery|entrega de pedidos', 'Ctrc':'ctrc|fretes',
 'Turno':'turnos|fechamento de turno', 'Cheque':'cheques',
 'Transferencia':'transferencias entre contas|transferencias',
 'Promocao':'promocoes|promocao', 'GestaoContrato':'contratos|contrato',
 'Kit':'kits|montagem de kits', 'Agendamento':'agendamentos|agenda',
 'Boletos':'boletos|boleto', 'PromocaoScanntech':'scanntech|clube de promocao',
 'Locacao':'locacoes|alugueis|locacao', 'RomaneioCarga':'romaneios de carga',
 'RomaneioConhecimentoTransporteEletronico':'romaneios de ct-e',
 'GerenciadorTicket':'tickets|ticket de atendimento', 'Menu':'menus',
 'MenuRapido':'menus|menu', 'OperacaoFiscal':'cfop|operacoes fiscais|operacao fiscal',
 'Restaurante':'restaurante|comandas|mesas', 'PedidoIfood':'ifood|pedidos ifood',
 'Industria':'industria|producao|ordens de producao',
 'Devolucao':'devolucoes|devolucao', 'ControleEntrega':'controle de entregas',
 'ManifestacaoDestinatario':'manifestacao do destinatario',
 'Hospedagem':'hospedagem|hospedes|reservas', 'AgrupamentoQuartos':'quartos|hunit',
 'Animais':'animais|veterinaria', 'Auxiliares':'backups|backup restaurado',
 'Farmacia':'farmacia|medicamentos', 'Posto':'posto|combustivel|abastecimentos',
}
SYNONYMS={'vencido':'atrasado','vencidos':'atrasado','vencidas':'atrasado','vencida':'atrasado',
 'abertas':'aberto','abertos':'aberto','pendentes':'aberto','pendente':'aberto',
 'pagas':'quitado','pagos':'quitado','quitadas':'quitado','quitados':'quitado',
 'recebidas':'quitado','recebidos':'quitado','receber':'recebimento','pagar':'pagamento',
 'comprar':'comprado','compras':'comprado','vendidos':'vendido','precos':'preco',
 'estoques':'estoque','mercadorias':'produto','clientes':'cliente','fornecedores':'fornecedor'}

def normal(value):
    value=re.sub(r'([a-z])([A-Z])',r'\1 \2',str(value))
    return ''.join(c for c in unicodedata.normalize('NFD',value.casefold()) if unicodedata.category(c)!='Mn')

def words(value):
    return [SYNONYMS.get(t,t) for t in re.findall(r'[a-z0-9]+',normal(value)) if len(t)>1 and not t.isdigit() and t not in STOP]

def similar(a,b):
    return a==b or (min(len(a),len(b))>=4 and (a.rstrip('s')==b.rstrip('s') or SequenceMatcher(None,a,b).ratio()>=.86))

@lru_cache(maxsize=1)
def index():
    catalog=json.loads((ROOT/'static/report-catalog.json').read_text(encoding='utf-8'))
    native=json.loads((ROOT/'static/native-service-map.json').read_text(encoding='utf-8'))['reports']
    for r in catalog['reports']:
        r['_title_words']=words(r['title']+' '+r['class'])
        r['_module_words']=words(r['module_label'])
    return [(r,native.get(r['id'],{})) for r in catalog['reports']]

def module_hits(question):
    q=' '+re.sub(r'[^a-z0-9-]+',' ',normal(question)).strip()+' '
    if re.search(r'\b(?:pagar|pagamentos)\b',q) and re.search(r'\b(?:receber|recebimentos)\b',q):
        return {'Pagamento','Recebimento'}
    hits=[(len(alias),module) for module,aliases in ALIASES.items() for alias in aliases.split('|') if ' '+alias+' ' in q]
    if not hits:return set()
    longest=max(n for n,_ in hits)
    return {m for n,m in hits if n==longest}

def primary_call(native):
    """Prefer a parameterized report call over preceding lookup-only calls."""
    candidates=[(i,c) for i,c in enumerate(native.get('calls',[])) if c.get('available')]
    return max(candidates,key=lambda pair:(any(p['kind']=='datetime' for p in pair[1]['parameters']),len(pair[1]['parameters'])),default=(0,{'parameters':[]}))

def requested_status(question):
    q=normal(question)
    patterns={'overdue':r'\b(?:atrasad\w*|vencid\w*)\b','open':r'\bem aberto\b|\bpendentes?\b',
        'settled':r'\b(?:quitad\w*|pagos?|pagas?|recebidas?|recebidos?)\b','reversed':r'\bestornad\w*\b',
        'future':r'\ba vencer\b'}
    return [name for name,pattern in patterns.items() if re.search(pattern,q)]

def suggested_filters(question,native,period):
    call_index,call=primary_call(native)
    q=normal(question);statuses=requested_status(question);suggestions=[];warnings=[]
    if len(statuses)>1:warnings.append('Foram pedidas situações diferentes. Escolha a situação no formulário.')
    status_patterns={'overdue':r'^(?:atrasad|vencid)','open':r'em aberto','settled':r'quitad|recebid|^pag[oa]s?$','reversed':r'estornad','future':r'^a vencer$'}
    for p in call['parameters']:
        if p.get('fixed'):continue
        name=p['name'];n=normal(name);value=None;reason=None;label=None
        choices=p.get('choices') or []
        pattern=None
        if p['kind']=='datetime' and period:
            if re.search(r'inicial|inicio',n):value=period['start']+'T00:00:00';reason='Início do período informado'
            elif re.search(r'final|fim',n):value=period['end']+'T23:59:59.999';reason='Fim do período informado'
        elif p['kind']=='boolean' and n=='consolidado' and re.search(r'todas as empresas|consolidad',q):
            value=True;reason='Consolidação solicitada; permissões continuam valendo'
        elif n=='situacao' and len(statuses)==1:pattern=status_patterns[statuses[0]];reason='Situação mencionada no pedido'
        elif n=='tipo filtro data':
            for word in ['vencimento','emissao','inclusao','quitacao','recebimento','pagamento']:
                if re.search(r'(?:data de |por |pelo )'+word,q):pattern=word;reason='Base de data mencionada no pedido';break
            if pattern is None and any(s in statuses for s in ['overdue','open','future']):pattern='vencimento';reason='Vencimento sugerido para a situação pedida; confirme'
        elif n=='tipo filtro operacao':
            credit=bool(re.search(r'\bcredito\b',q));debit=bool(re.search(r'\bdebito\b',q))
            if credit!=debit:pattern='^credito$' if credit else '^debito$';reason='Operação de cartão mencionada'
            elif credit and debit:pattern='^todas$';reason='Crédito e débito solicitados'
        elif n=='especie':
            methods=[m for m in ['dinheiro','pix','boleto','cheque','cartao'] if re.search(r'\b'+m+r'\b',q)]
            if len(methods)==1:pattern='^'+methods[0]+'$';reason='Espécie mencionada no pedido'
        if pattern:
            found=[c for c in choices if re.search(pattern,normal(c['label']))]
            if len(found)==1:value=found[0]['value'];label=found[0]['label']
        if value is not None:
            suggestions.append({'name':name,'label':p.get('label') or name,'value':value,'display':label if label else str(value),'reason':reason})
    if not period and any(p['kind']=='datetime' for p in call['parameters']):warnings.append('Período não informado: confira as datas antes de consultar.')
    return {'call_index':call_index,'suggestions':suggestions,'warnings':warnings}

def discover(question,limit=8,anchor=None):
    query=normal(question);tokens=list(dict.fromkeys(words(question)))
    modules=module_hits(question)
    period=None;period_error=None
    from insight_plans import explicit_period
    from fastapi import HTTPException
    try:
        dates=explicit_period(question,anchor or date.today())
        if dates:period={'start':dates[0].isoformat(),'end':dates[1].isoformat()}
    except (HTTPException,ValueError) as exc:
        period_error=str(exc.detail if isinstance(exc,HTTPException) else 'Confira as datas informadas.')
    ranked=[]
    entries=index()
    exact_entries=[(r,n) for r,n in entries if normal(r['id'])==query or normal(r['class'])==query]
    if exact_entries:entries=exact_entries
    for report,native in entries:
        title=report['_title_words']
        module=report['_module_words']
        matched=[t for t in tokens if any(similar(t,x) for x in title)]
        module_matched=[t for t in tokens if any(similar(t,x) for x in module)]
        explicit=report['module'] in modules
        exact=normal(report['id'])==query or normal(report['class'])==query
        if modules and not explicit and not exact:continue
        if not exact and not explicit and (not tokens or len(matched)/len(tokens)<.45):continue
        if not (exact or explicit or matched):continue
        score=100 if exact else (30 if explicit else 0)+6*len(matched)+2*len(module_matched)+10*len(matched)/max(len(tokens),1)
        if normal(report['title']) in query:score+=18
        available=any(c.get('available') for c in native.get('calls',[]))
        proposed=suggested_filters(question,native,period)
        statuses=requested_status(question)
        if len(statuses)==1:
            report_status=requested_status(report['title'])
            if report_status and statuses[0] not in report_status and not (statuses[0]=='overdue' and report_status==['open']):continue
            if statuses[0] in report_status:score+=22
            if any(s['name']=='situacao' for s in proposed['suggestions']):score+=15
        params={'report':report['id']}
        if period:params.update(period)
        if available:params['call']=proposed['call_index']
        safe_values={s['name']:s['value'] for s in proposed['suggestions'] if not any(k in normal(s['name']) for k in ['inicial','final','inicio','fim'])}
        if safe_values:params['preset']=json.dumps(safe_values,ensure_ascii=False,separators=(',',':'))
        ranked.append((score,{'id':report['id'],'title':report['title'],'module':report['module_label'],
            'available':available,'url':'/native-reports?'+urlencode(params),
            'matched_terms':matched,'parameter_count':len(primary_call(native)[1]['parameters']),'proposed_filters':proposed}))
    ranked.sort(key=lambda item:(-item[0],item[1]['title'],item[1]['id']))
    matches=[r for _,r in ranked[:limit]]
    ambiguous=len(modules)>1 or len(requested_status(question))>1
    return {'matches':matches,'total_matches':len(ranked),'period':period,'period_error':period_error,'needs_clarification':ambiguous,
        'message':('O pedido contém módulos ou situações diferentes. Escolha o relatório e confira os filtros.' if ambiguous else 'Escolha o relatório e confirme os filtros antes de consultar o SuiteG6.') if matches else 'Não encontrei um relatório correspondente. Tente informar o módulo e o assunto.',
        'execution':'selection_required','coverage':{'catalog':358,'native_available':356},
        'limitations':'Esta busca identifica relatórios do catálogo; não consulta dados nem responde perguntas arbitrárias sobre toda a base.'}

def should_discover(question):
    q=normal(question)
    if re.search(r'\b(?:catalogo|nativo|nativos)\b|(?:buscar|encontrar|encontre) (?:um )?relatorio',q):return True
    modules=module_hits(question)
    # Existing sales conversation and seller follow-ups retain their original path.
    if modules-{'MovimentacaoSaida','ProdutoServico'}:return True
    if 'ProdutoServico' in modules and re.search(r'estoque|inventario|preco|cadastro|serial|integridade',q):return True
    if re.search(r'\brelatorios?\b',q):
        from insight_plans import classify
        return classify(question) is None
    return False
