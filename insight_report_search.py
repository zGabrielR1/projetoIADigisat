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
    hits=[(len(alias),module) for module,aliases in ALIASES.items() for alias in aliases.split('|') if ' '+alias+' ' in q]
    if not hits:return set()
    longest=max(n for n,_ in hits)
    return {m for n,m in hits if n==longest}

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
        params={'report':report['id']}
        if period:params.update(period)
        ranked.append((score,{'id':report['id'],'title':report['title'],'module':report['module_label'],
            'available':available,'url':'/native-reports?'+urlencode(params),
            'matched_terms':matched,'parameter_count':len(next((c['parameters'] for c in native.get('calls',[]) if c.get('available')),[]))}))
    ranked.sort(key=lambda item:(-item[0],item[1]['title'],item[1]['id']))
    matches=[r for _,r in ranked[:limit]]
    return {'matches':matches,'total_matches':len(ranked),'period':period,'period_error':period_error,
        'message':'Escolha o relatório e confirme os filtros antes de consultar o SuiteG6.' if matches else 'Não encontrei um relatório correspondente. Tente informar o módulo e o assunto.',
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
