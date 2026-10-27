# Apuração do crédito loja — turno de 15/10/2026

Análise em 22/10/2026. Leitura do MongoDB ativo e leitura offline do ZIP fornecido; nenhuma alteração no banco ou restauração de backup.

## Resultado

A origem documental dos R$ 20,00 adicionais NÃO foi localizada. O valor comprovado é um excesso no campo informado do fechamento: R$ 411,80 − R$ 335,80 − R$ 56,00 = R$ 20,00. Não há evidência suficiente para atribuí-lo a uma venda, cliente, retirada, fraude ou erro de digitação específico.

O achado decisivo é que a hipótese de estorno posterior ao fechamento é contrariada pelo histórico: a parcela 474/1 foi estornada em 15/10 às 15:09:45.275 pelo usuário SIMONE, antes do fechamento das 18:27:09.192. O usuário registrado identifica a conta utilizada; não prova a identidade física do operador.

## Valores reconciliados

| Componente | Valor |
|---|---:|
| Crédito efetivamente quitado e não cancelado: venda 476/2 | R$ 56,00 |
| Crédito da venda 474/1, estornado antes do fechamento | R$ 335,80 |
| Soma das duas operações, incluindo o estorno | R$ 391,80 |
| Campo `TurnoFechamento.CreditoLoja`, gravado no turno | R$ 411,80 |
| Excesso sobre a soma bruta | R$ 20,00 |
| Diferença informado menos calculado válido | R$ 355,80 |

Os R$ 20,00 são uma decomposição da divergência; o banco não os apresenta como um terceiro pagamento de crédito loja desse turno.

## Linha do tempo comprovada

- 10:49:17: quitação da parcela 474/1 por R$ 335,80 em crédito loja.
- 15:02–15:05: criação, quitação e estorno de recebimento manual CRE15102026 por R$ 335,80 em dinheiro. Estorno às 15:05:59.346, usuário SIMONE.
- 15:09:45.275: estorno da parcela 474/1, usuário SIMONE.
- 15:09–15:13: registros substitutos da parcela e do recebimento manual passam por ajustes para R$ 0,10 e ficam inativos. Há observação `erro de lancamento de credito duplicado` no substituto manual. Esses registros explicam tentativas de correção; não os R$ 20,00.
- 16:14:40: venda 476/2 quita R$ 56,00 em crédito loja.
- 18:27:09.192: fechamento registra R$ 411,80 em crédito loja.

A soma dos 40 registros de crédito/débito de cliente deixa R$ 335,80 para Renata. Há crédito de retorno com referência de devolução zerada; esse registro, isoladamente, não comprova devolução física ou fiscal. Não é adequado zerar o saldo por esta auditoria sem esclarecer a operação comercial e a correção pretendida.

## O que o projeto esclarece sobre o relatório

Leitura estática dos fontes descompilados já preservados, sem nova validação de binário ou execução por REA:

- `engenharia/codigo-descompilado/DigisatServer.Application.Main/AuxiliaresModulo/Services/TurnoAppService.cs`, método `FecharTurno`, linhas 111–174: o servidor recebe `turnoDto.Fechamento.TotalCreditoLoja` e o transfere ao fechamento; esse trecho não recalcula o total a partir dos pagamentos.
- `engenharia/codigo-descompilado/DigisatServer.Application.Main/AuxiliaresModulo/Services/TurnoRelatorioAppService.cs`, linhas 321–343 e 395–408: o calculado considera lançamentos quitados ativos e não cancelados e soma `ValorQuitacao`, com sinal conforme a origem.
- Mesmo serviço, método `ObterValorInformadoNoTurno`, linhas 969–974: o informado de crédito loja é lido de `TurnoFechamento.CreditoLoja`.

Portanto o resumo de fechamento NÃO demonstra que o sistema calculava R$ 411,80 às 18:27. Demonstra que esse valor foi enviado e gravado como fechamento. Não foi rastreada a tela cliente que preenche o DTO: digitação, preenchimento automático, valor antigo na tela e eventual defeito continuam possibilidades, não conclusões. As ferramentas REA não estavam disponíveis nesta sessão; as referências acima são evidência estática local.

## Abrangência e reprodução

ZIP: `D:\Documents\pasta coisas\DigisatServer20261016_104950.zip`. A leitura BSON foi feita em memória, sem extração/restauração. A comparação integral por documento encontrou os mesmos conteúdos na base ativa e no ZIP nas seis coleções: Turnos (123), TurnosLancamentos (1.585), Recebimentos (850), ItensCreditoDebitoCliente (40), MovimentosConta (712) e Movimentacoes (525). Isso compara dois estados iguais, não um estado anterior ao estorno com outro posterior.

O arquivo `evidencias-caixa-1510.json` guarda hash do ZIP, fechamento, lançamentos de crédito do turno, históricos relevantes e ocorrências dos valores 20,00 / 355,80 / 411,80 em campos monetários dessas coleções. A busca inclui documentos inativos e históricos, mas não recupera documentos fisicamente apagados nem um histórico completo das edições do fechamento.

Reprodução, na raiz do projeto:

```powershell
.\.venv\Scripts\python.exe engenharia/ferramentas/auditar_caixa_1510.py 'D:\Documents\pasta coisas\DigisatServer20261016_104950.zip' --output docs/evidencias-caixa-1510.json
```

## Encaminhamento objetivo para resolver o restante

Consultar a memória/comprovante usado para informar crédito loja no fechamento e perguntar como foi obtido R$ 411,80. Pedir ao suporte o rastreamento da tela que preenche `Fechamento.TotalCreditoLoja`, com o turno `6ad0d1f435c0e8154819c2a7`, e verificar se mantém valores anteriores a estornos ou permite edição manual. Para comprovar a causa dos R$ 20,00 é necessário esse registro da entrada do valor ou outra evidência histórica; repetir a mesma conferência de caixa não recupera essa informação.

Texto para o suporte: “Turno SIMONE de 15/10/2026, abertura 10:15:32 e fechamento 18:27:09: CreditoLoja informado 411,80; calculado válido 56,00. Parcela 474/1 de 335,80 foi estornada às 15:09:45, antes do fechamento. Mesmo incluindo-a, o total é 391,80, restando 20,00 sem lançamento correspondente no turno. Como a tela originou Fechamento.TotalCreditoLoja=411,80? Há log do valor enviado, edição do fechamento ou reaproveitamento de valor anterior ao estorno?”
