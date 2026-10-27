"""Index retained C# routines for 1.0.1 RC1; never execute assemblies or access MongoDB."""
from __future__ import annotations

import argparse
import hashlib
import json
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
SNAPSHOT = ROOT / 'docs/routines-1.0.1-rc.1.json'
VERSIONED = 'engenharia/assemblies-descompilados/2.0.26273.6/'
APP = VERSIONED + 'DigisatServer.Application.Main/DigisatServer.Application.Main/'
CORE = VERSIONED + 'DigisatServer.Application.Core/DigisatServer.Application.Core/'
WCF = VERSIONED + 'DigisatServer.Distributed.Wcf.Main/DigisatServer.Distributed.Wcf.Main/Services/'
DOMAIN = 'engenharia/codigo-descompilado/DigisatServer.Domain.Main/DigisatServer.Domain.Main/Aggregates/'
SOURCES = [
    ('finance_app', 'finance', APP + 'AuxiliaresModulo/Services/PagamentoRecebimentoRelatorioAppService.cs',
     ['ObterListaDeRecebimentosEmAberto', 'ObterListaDeRecebimentosQuitados',
      'ObterListaDePagamentosEmAberto', 'ObterListaDePagamentosQuitados']),
    ('finance_repository', 'finance', 'engenharia/codigo-descompilado/DigisatServer.Infrastructure.Data.Main/DigisatServer.Infrastructure.Data.Main/Repositories/ParcelaRepositoryRead.cs',
     ['ObterTotais', 'SomarUltimoValorHistoricoPorSituacao', 'ObterTotalPorSituacao', 'PreencherTotalAtrasado']),
    ('receivable_filters', 'finance', DOMAIN + 'FinanceirosAgg/PagamentosRecebimentos/ParcelaRecebimentoSpecifications.cs',
     ['PorSituacaoPendenteMasNaoAtrasada', 'PorSituacaoAtrasada', 'PorVencimento', 'PorQuitacao']),
    ('payable_filters', 'finance', DOMAIN + 'FinanceirosAgg/PagamentosRecebimentos/ParcelaPagamentoSpecifications.cs',
     ['PorVencimento', 'PorQuitacao']),
    ('finance_projection', 'finance', CORE + 'Projections/FinanceirosProjection/PagamentosRecebimentos/PagamentoRecebimentoProjection.cs',
     ['ProjectedAsSituacaoRelatorioDto', 'ProjectedAsSituacaoRelatorioCollectionDto']),
    ('finance_bridge', 'finance', WCF + 'AuxiliaresModulo/PagamentoRecebimentoRelatorioModuleService.cs',
     ['ObterListaDeRecebimentosEmAberto', 'ObterListaDePagamentosEmAberto']),
    ('expiry_app', 'stock', APP + 'AuxiliaresModulo/Services/ProdutoServicoRelatorioAppService.cs',
     ['ObterProdutosComValidadeEmXDias']),
    ('expiry_ui', 'stock', 'engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLotesValidade.Designer.cs',
     ['ObterRelatorio']),
    ('expiry_bridge', 'stock', WCF + 'AuxiliaresModulo/ProdutoServicoRelatorioModuleService.cs',
     ['ObterProdutosComValidadeEmXDias']),
    ('delivery_app', 'delivery', APP + 'MovimentacoesModulo/ControleEntregaAppService.cs',
     ['ObterControlesEntregaPorPeriodo', 'ObterControlesEntregaPorProduto']),
    ('delivery_filters', 'delivery', DOMAIN + 'MovimentacoesAgg/Saidas/ControlesEntrega/ControleEntregaSpecification.cs',
     ['PorDataInclusao', 's0XKMrjMrn9', 'ObterSituacao', 'PorProdutoEmpresaReferencia']),
    ('delivery_quantity', 'delivery', DOMAIN + 'MovimentacoesAgg/Saidas/ControlesEntrega/ControleEntregaItem.cs',
     ['QuantidadePendente', 'kYTTxvEeq0kOtHd0lXn7', 'Mww4pVEer5aX4f5dafxm',
      'PkeYoeEeeooBoOatcA3m', 'JKBdFiEe5oSYikA8uCLq', 'Mq8YG4EeTFYiVhjMq56g']),
    ('delivery_bridge', 'delivery', WCF + 'SaidasModulo/ControleEntregaModuleService.cs',
     ['ObterControlesEntregaPorPeriodo', 'ObterControlesEntregaPorProduto']),
]


def inventory():
    sources = []
    for key, module, relative, members in SOURCES:
        path = (ROOT / relative).resolve()
        if not path.is_relative_to(ROOT):
            raise ValueError('Source outside workspace')
        raw = path.read_bytes()
        content = raw.decode('utf-8-sig')
        locations = []
        for member in members:
            pattern = r'^[\t ]*(?:public|private|internal|protected)[^\r\n]*\b' + re.escape(member) + r'(?:\s*\(|[\t ]*\r?$)'
            matches = list(re.finditer(pattern, content, re.MULTILINE))
            if len(matches) != 1:
                raise ValueError(f'{key}/{member}: expected one declaration, found {len(matches)}')
            line = content[:matches[0].start()].count('\n') + 1
            locations.append({'member': member, 'line': line})
        sources.append({'id': key, 'module': module, 'path': relative,
                        'sha256': hashlib.sha256(raw).hexdigest(), 'members': locations})
    return {'schema_version': 1, 'target': '1.0.1-rc.1',
            'evidence_type': 'retained_local_decompiled_source',
            'limitations': 'Static source trace only; mixed retained source trees. No new decompilation, assembly identity verification, runtime, database, or feature implementation is implied.',
            'sources': sources}


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--write', action='store_true', help='Explicitly record the current local source identities')
    args = parser.parse_args()
    try:
        actual = inventory()
        if args.write:
            SNAPSHOT.write_text(json.dumps(actual, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
        elif json.loads(SNAPSHOT.read_text(encoding='utf-8')) != actual:
            raise ValueError('Retained sources or routine locations changed; review before refreshing the snapshot')
    except (OSError, ValueError) as exc:
        parser.exit(1, f'Research verification failed: {exc}\n')
    count = sum(len(source['members']) for source in actual['sources'])
    print(f'Verified {len(actual["sources"])} retained files and {count} routine declarations across 3 modules.')
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
