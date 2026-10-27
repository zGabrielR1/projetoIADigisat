# Comparação offline dos backups históricos

Leitura em 22/10/2026, sem restauração, conexão ou alteração de banco. ZIPs e BSON gzip foram lidos em memória. Evidência reproduzível em `evidencias-backups-historicos-caixa.json`; script em `engenharia/ferramentas/comparar_backups_caixa.py`.

## Resultado

Os backups `C:\DigiSat_Backup\DigisatServer20261015_110611.zip`, `20261015_141048.zip` e `20261016_074652.zip` não fornecem estados anteriores da base do caso. Identificam outra empresa (`6aa8432d45cd1e1e700a0119`), com apenas um turno aberto em 22/09/2026. Não há turno com identificador comum aos 123 turnos do ZIP do usuário, cuja empresa é `698603e435c0e8108464734e`.

| Coleção | Cada um dos três ZIPs locais | ZIP do usuário |
|---|---:|---:|
| Turnos | 1 | 123 |
| TurnosLancamentos | 109 | 1.585 |
| Recebimentos | 51 | 850 |
| Movimentacoes | 66 | 525 |
| ItensCreditoDebitoCliente | 0 | 40 |

Entre os três ZIPs locais, todas as coleções financeiras têm conteúdo BSON idêntico. As únicas coleções diferentes são Pessoas, TiposPersonalizado e Usuarios. Nenhum contém o turno `6ad0d1f435c0e8154819c2a7`. Portanto não permitem comparar o estado desse caixa antes e depois do estorno.

## Ampliação da busca monetária

Foram percorridas todas as coleções BSON do ZIP fornecido pelo usuário, recursivamente, inclusive listas e históricos, procurando valores numéricos absolutos exatamente iguais a 20,00, 355,80 e 411,80 em campos cujo nome contém valor, total, credito, desconto, acrescimo ou troco.

Encontradas apenas seis ocorrências: 411,80 em `Turnos.TurnoFechamento.CreditoLoja` do turno investigado, e cinco lançamentos com `Valor=20` nas datas 16/06, 14/08, 08/09, 24/09 e 08/10 de 2026. Nenhum desses lançamentos de 20 pertence a 15/10. Não foi encontrado 355,80.

## Limites

A busca procura valores exatos em campos monetários numéricos, não demonstra ausência de combinações de valores, de valores representados em texto ou de registros fisicamente removidos. Os ZIPs locais examinados não recuperam estados históricos da empresa investigada. Esses resultados ampliam a abrangência da conferência, mas não explicam o preenchimento de 411,80 na tela de fechamento.
