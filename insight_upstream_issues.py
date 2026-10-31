"""Evidence-scoped notices about the original SuiteG6 report calculations.

These notices never change native results. The client installation and catalog
cannot authenticate the binary loaded by the remote WCF server, so a matching
method is a candidate for review, never proof that this server is affected.
"""
from __future__ import annotations

from typing import Any


ANALYZED_ASSEMBLY = 'DigisatServer.Application.Main.dll'
ANALYZED_SHA256 = '471908afa8e8ae43aaab13722e7820006c4b7a221600983abc42e9c3b9039af4'
REPORT_CONTRACT = 'IMovimentacaoSaidaRelatorioModuleService'
SHARED_ARRAY_METHODS = frozenset({
    'ObterTotalMovimentacoesPorMesPorVendedor',
    'TotalMovimentadoPorVendedor',
    'ObterTotalMovimentacoesPorMesPorTipo',
})


def issues_for_call(call: dict[str, Any], arguments: dict[str, Any] | None = None) -> list[dict[str, Any]]:
    """Return method/argument candidates without asserting remote build identity.

    None denotes catalog discovery, where conditional arguments are unknown.
    Only the actual boolean False activates the fractional-quantity condition.
    """
    if call.get('contract') != REPORT_CONTRACT:
        return []
    method = call.get('method')
    if method in SHARED_ARRAY_METHODS:
        issue = {
            'id': 'digisat-shared-monthly-array',
            'condition_status': 'matched',
            'condition': {'parallel_partitions_share_a_seller_or_type': True},
            'title': 'Possível divergência na totalização paralela',
            'warning': (
                'Na versão analisada do SuiteG6, esta rotina modifica totais compartilhados entre tarefas '
                'paralelas e pode retornar somas divergentes mesmo com os mesmos dados. '
                'A versão do servidor consultado não foi verificada; este aviso identifica uma rotina '
                'a conferir e não confirma que o resultado desta consulta esteja incorreto.'
            ),
        }
    elif method == 'ObterItensVendidos':
        value = (arguments or {}).get('agruparPorValorUnitario')
        if value is True:
            return []
        issue = {
            'id': 'digisat-fractional-unit-price',
            'condition_status': 'matched' if value is False else 'unknown',
            'condition': {'agruparPorValorUnitario': False, 'aggregate_quantity': '0 < quantidade < 1'},
            'title': 'Possível preço unitário incorreto para quantidade fracionada',
            'warning': (
                'Na versão analisada do SuiteG6, com o agrupamento por preço desativado, '
                'quantidades totais entre zero e uma unidade podem produzir preço unitário incorreto. '
                'A versão do servidor consultado não foi verificada; confira os preços dessas linhas '
                'com os documentos de origem. Os valores retornados não foram recalculados pelo Insight.'
            ),
        }
    else:
        return []
    endpoint = ('/api/audit/monthly' if method == 'ObterTotalMovimentacoesPorMesPorVendedor' else
                '/api/audit/unit-prices' if method == 'ObterItensVendidos' else None)
    return [{
        **issue,
        'applicability': 'candidate',
        'remote_build_status': 'unverified',
        'data_condition_status': 'unverified',
        'analyzed_artifact': {'name': ANALYZED_ASSEMBLY, 'sha256': ANALYZED_SHA256},
        'method': method,
        'independent_audit': {
            'endpoint': endpoint,
            'input': 'normalized preselected snapshot' if endpoint == '/api/audit/monthly' else
                     'explicit quantity, total and reported unit price' if endpoint else 'grouping adapter not implemented',
            'scope': 'explicit input only; no automatic remote data capture' if endpoint else
                     'this aggregation requires a separate verified grouping adapter',
        },
        'results_modified': False,
    }]
