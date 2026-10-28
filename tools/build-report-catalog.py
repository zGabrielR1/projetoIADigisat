"""Build reviewable report documentation from retained SuiteG6 evidence.

This indexes static evidence; it does not translate C# into executable queries.
"""
from __future__ import annotations

import argparse
from collections import Counter, defaultdict
import hashlib
import json
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT))
sys.path.insert(0, str(ROOT / 'engenharia/ferramentas'))
from coverage_report import catalog_entries, build_report
from insight_finance import SPECS as FINANCE_REPORTS

DEST = ROOT / 'static/report-catalog.json'
GUIDE = ROOT / 'docs/relatorios-1.0.3-rc.1'
BASE = ROOT / 'engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios'
FALLBACK = ROOT / 'engenharia/codigo-descompilado/Modulo.Relatorios'
LABELS = {
    'Pessoa': 'Pessoa', 'ProdutoServico': 'Produto ou Serviço', 'Grupo': 'Grupo',
    'RelacaoVendedorCliente': 'Relação vendedor clientes', 'MovimentoConta': 'Movimento Conta',
    'Pagamento': 'Contas a pagar', 'Recebimento': 'Contas a receber',
    'MovimentacaoEntrada': 'Movimentação entrada', 'MovimentacaoSaida': 'Movimentação saída',
    'ConhecimentoTransporteEletronico': 'CT-e', 'ManifestoEletronicoDocumentoFiscal': 'MDF-e',
    'Delivery': 'Delivery', 'Ctrc': 'Movimentações CTRC', 'Turno': 'Turno',
    'Cheque': 'Cheques', 'Transferencia': 'Transferências entre contas',
    'Promocao': 'Promoção', 'GestaoContrato': 'Gestão de contratos', 'Kit': 'Montagem de Kits',
    'Agendamento': 'Agendamentos', 'Boletos': 'Gestão de Boletos',
    'PromocaoScanntech': 'Clube de Promoção', 'Locacao': 'Locações',
    'RomaneioCarga': 'Romaneios de carga',
    'RomaneioConhecimentoTransporteEletronico': 'Romaneios de CT-e',
    'GerenciadorTicket': 'Gerenciador de ticket', 'MenuRapido': 'Menus',
    'OperacaoFiscal': 'Operação fiscal', 'Restaurante': 'Restaurante',
    'PedidoIfood': 'Pedidos do iFood', 'Industria': 'Indústria', 'Devolucao': 'Devoluções',
    'ControleEntrega': 'Controle de entregas', 'ManifestacaoDestinatario': 'Manifestação do destinatário',
}


def unique(values):
    return list(dict.fromkeys(values))


def extract(text):
    """Literal UI evidence only. A DTO binding is never a Mongo field mapping."""
    return {
        'parameters': unique(re.findall(r'\bTitulo\s*=\s*"([^"\r\n]+)"', text)),
        'bindings': unique(re.findall(r'new XRBinding\("Text",\s*null,\s*"([^"\r\n]+)"', text)),
        'expressions': unique(re.findall(r'\.Expression\s*=\s*"((?:[^"\\]|\\.)*)"', text)),
        'groups': unique(re.findall(r'new GroupField\("([^"\r\n]+)"', text)),
        'summaries': unique(re.findall(r'\.Func\s*=\s*SummaryFunc\.(\w+)', text)),
    }


def build():
    verified = build_report()
    if verified['issues']:
        raise ValueError('; '.join(verified['issues']))
    traces = {(t['module'], t['class']): t for t in verified['continuation_traces']}
    files = defaultdict(list)
    for base in (BASE, FALLBACK):
        for path in sorted(base.rglob('*.cs')):
            files[path.name].append(path)
    docs = []
    for path in sorted((ROOT / 'docs').glob('*.md')):
        if path.name.startswith(('continuacao-', 'engenharia-reversa-')):
            docs.append((path, path.read_text(encoding='utf-8')))
    catalog_text = (ROOT / 'docs/catalogo-relatorios.md').read_text(encoding='utf-8')
    service_calls = {}
    for line in catalog_text.splitlines():
        cells = [c.strip() for c in line.strip('|').split('|')]
        if len(cells) == 6 and cells[2].startswith('`'):
            service_calls[(cells[0], cells[2].strip('`'))] = re.findall(r'`([^`]+)`', cells[3])
    entries = catalog_entries(catalog_text)
    native_path=ROOT/'static/native-service-map.json'
    native_reports=json.loads(native_path.read_text(encoding='utf-8'))['reports'] if native_path.exists() else {}
    for item in entries:
        name = item['class']
        chosen = []
        for suffix in ('.cs', '.Designer.cs'):
            candidates = files.get(name + suffix, [])
            if candidates:
                chosen.append(candidates[0])
        texts = [p.read_text(encoding='utf-8-sig') for p in chosen]
        combined = '\n'.join(texts)
        item.update(extract(combined))
        item['id'] = item['module'] + '.' + name
        item['module_label'] = LABELS.get(item['module'], item['module'])
        item['service_calls'] = service_calls.get((item['module'], name), [])
        item['sources'] = [{'path': p.relative_to(ROOT).as_posix(),
                            'sha256': hashlib.sha256(p.read_bytes()).hexdigest()}
                           for p in chosen]
        item['services'] = unique(re.findall(r'Canal<(I\w+ModuleService)', combined))
        trace = traces.get((item['module'], name))
        references = [p.relative_to(ROOT).as_posix() for p, content in docs
                      if re.search(r'(?<!\w)' + re.escape(name) + r'(?!\w)', content)]
        if trace:
            references.insert(0, trace['report'])
            item['coverage'] += ' Rastreamento complementar disponível; runtime não homologado.'
        item['references'] = unique(references)
        item['limitations'] = (trace or {}).get('limitations', [])
        item['execution'] = {'status': 'pending',
                             'reason': 'Consulta nativa ainda não implementada e homologada no Insight.'}
        if item['id'] in FINANCE_REPORTS:
            item['execution'] = {'status': 'implemented_data_view',
                                 'reason': 'Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.',
                                 'url': '/finance-reports?report=' + item['id']}
            item['references'] = unique(['docs/financeiro-1.0.3.md', 'docs/finance-artifacts-1.0.3-rc.1.json'] + item['references'])
        native=native_reports.get(item['id'])
        if native and any(c.get('available') for c in native['calls']):
            if item['id'] not in FINANCE_REPORTS:
                item['execution']={'status':'native_service_available',
                    'reason':'Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.',
                    'url':'/native-reports?report='+item['id']}
            item['native_url']='/native-reports?report='+item['id']
            item['references']=unique(['docs/relatorios-nativos-1.0.3.md']+item['references'])
        if not chosen:
            item['execution']={'status':'unavailable_in_installed_build','reason':'Classe não localizada nos assemblies da instalação. Sem comportamento nativo para reconstruir ou executar nesta versão.'}
    counts = Counter(item['module'] for item in entries)
    return {'schema_version': 1, 'target_release': '1.0.3-rc.1',
            'status': 'in_progress', 'evidence_type': 'retained_local_decompiled_source',
            'limitations': 'As fontes retidas incluem o build 2.0.26273.6. A identidade do build instalado e a equivalência de execução precisam ser reconfirmadas. Campos abaixo são bindings de DTO, não caminhos MongoDB. Inventário estático não comprova disponibilidade por licença, segmento ou permissão.',
            'summary': {'entries': len(entries), 'distinct_types': len({e['class'] for e in entries}),
                        'modules': len(counts), 'native_queries_implemented':sum(bool(e.get('native_url')) for e in entries),
                        'native_print_layouts_reproduced':0,
                        'data_views_implemented': sum(e['execution']['status'] == 'implemented_data_view' for e in entries),
                        'native_service_entries':sum(bool(e.get('native_url')) for e in entries),
                        'unavailable_entries':sum(e['execution']['status']=='unavailable_in_installed_build' for e in entries),
                        'pending_entries':sum(e['execution']['status']=='pending' for e in entries)},
            'modules': [{'id': m, 'label': LABELS.get(m, m), 'count': n,
                         'shown_in_reference': m in LABELS} for m, n in sorted(counts.items())],
            'reports': entries}


def markdown(catalog):
    result = {}
    index = ['# Relatórios SuiteG6 — preparação da 1.0.3-rc.1', '',
             '**Estado: em desenvolvimento.** Esta documentação não certifica que os relatórios nativos já executem no Insight.', '',
             catalog['limitations'], '',
             'As consultas financeiras implementadas podem ser abertas pelo catálogo do Insight. Os sete relatórios existentes de vendas continuam na tela principal. Disponibilidade de uma consulta de dados e homologação do layout nativo são estados separados.', '',
             '## Módulos', '']
    for module in catalog['modules']:
        mid = module['id']
        index.append(f'- [{module["label"]} ({module["count"]})]({mid}.md)')
        lines = [f'# {module["label"]}', '', '[Todos os módulos](README.md)', '',
                 'Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.', '']
        for e in catalog['reports']:
            if e['module'] != mid:
                continue
            lines.extend([f'## {e["title"]}', '', f'Classe: `{e["class"]}`. Identificador: `{e["id"]}`.', '',
                          '**Execução no Insight:** ' + e['execution']['reason'], '',
                          '**Parâmetros nativos:** ' + ('; '.join(e['parameters']) or 'Não recuperados na classe de apresentação.'), '',
                          '**Campos apresentados (DTO):** ' + (', '.join('`' + x + '`' for x in e['bindings']) or 'Não recuperados por extração literal.'), ''])
            for label, key in [('Agrupamentos do layout', 'groups'), ('Resumos do layout', 'summaries'), ('Expressões calculadas do layout', 'expressions'), ('Contratos de serviço', 'services'), ('Chamadas catalogadas', 'service_calls')]:
                if e[key]:
                    lines.extend([f'**{label}:**', ''] + ['- `' + x.replace('`', '\\`') + '`' for x in e[key]] + [''])
            lines.extend(['**Pesquisa da regra:** ' + e['coverage'] + '.', ''])
            for ref in e['references']:
                lines.extend([f'- [Rastreamento: {Path(ref).name}](../../{ref})'])
            if e['limitations']:
                lines.extend(['', '**Limites registrados na pesquisa:**', ''] + ['- ' + x for x in e['limitations']])
            lines.extend(['', '**Fontes da ficha:**', ''])
            for src in e['sources']:
                lines.append(f'- [{Path(src["path"]).name}](../../{src["path"]}) — SHA-256 `{src["sha256"]}`')
            if not e['sources']:
                lines.append('- Classe não localizada nas fontes retidas.')
            lines.append('')
        result[GUIDE / (mid + '.md')] = '\n'.join(lines) + '\n'
    result[GUIDE / 'README.md'] = '\n'.join(index) + '\n'
    return result


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--check', action='store_true')
    args = parser.parse_args()
    catalog = build()
    outputs = {DEST: json.dumps(catalog, ensure_ascii=False, indent=2) + '\n', **markdown(catalog)}
    for path, content in outputs.items():
        if args.check:
            if not path.exists() or path.read_text(encoding='utf-8') != content:
                raise SystemExit(f'Conteúdo desatualizado: {path.relative_to(ROOT)}')
        else:
            path.parent.mkdir(parents=True, exist_ok=True)
            path.write_text(content, encoding='utf-8')
    print(json.dumps(catalog['summary']))


if __name__ == '__main__':
    main()
