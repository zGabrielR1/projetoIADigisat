# Auditorias independentes — RC6

O Insight pode conferir cálculos sem executar o SuiteG6 nem alterar valores retornados por ele. Nesta RC, as auditorias são recursos de API e CLI; não há nova tela nem captura automática de MongoDB/WCF.

## Mensal por vendedor

`POST /api/audit/monthly` recebe documentos normalizados que já foram selecionados pelo exportador. Cada documento informa referência única, mês de emissão e itens; cada item informa vendedor, `total_liquido`, cancelamento e os indicadores de documento auxiliar, movimentação de estoque e financeiro.

O Insight soma por vendedor/mês com decimal e execução serial, explica exclusões e compara resultados nativos opcionais. Totais, diferenças e hashes do input são retornados como evidência. Ausência de linha é distinta de uma linha com zero. Limites de exibição não alteram totais ou contagem de divergências.

Os campos obrigatórios evitam assumir silenciosamente flags ou regras ausentes. Um snapshot normalizado não é um dump BSON bruto: o exportador precisa validar seleção de empresa/ano/origem/situação/finalidade, tipos de item e cálculo de `TotalLiquido`. Completude declarada e identidade do servidor não são tratadas como comprovadas. A API aceita até 5.000 documentos/50.000 itens e rejeita duplicatas/valores inválidos.

## Preço unitário

`POST /api/audit/unit-prices` recebe linhas com `reference`, `quantity`, `net_total` e, opcionalmente, `reported_unit_price`. Para quantidade 0,5 e total 10, a referência é 20,00 por unidade. Quantidade zero é explicitamente indefinida; o Insight não a troca silenciosamente por 1. Os preços são truncados em duas casas em direção a zero.

## Uso e limites

As APIs usam a autenticação normal do Insight. Os esquemas completos estão na especificação OpenAPI servida pela aplicação. Os módulos novos são incluídos no executável portátil; não é necessário Python para consultar as APIs.

No checkout do código-fonte, com as dependências Python instaladas:

```sh
python tools/audit-calculation-snapshot.py monthly snapshot.json
python tools/audit-calculation-snapshot.py unit-prices prices.json
```

Esses comandos usam as mesmas funções das APIs. Não abrem banco, não executam código nativo e não corrigem os números informados. Uma divergência precisa primeiro de escopo e entrada equivalentes; não prova isoladamente bug, perda de dados ou versão do servidor.

## Validação desta distribuição

A RC6 é compilada e executada em Windows x64 por CI, com PATH mínimo, autenticação e dados sintéticos. O arquivo `verification.json` publicado registra a cobertura. O perfil é **windows-isolated**: chamadas ao SuiteG6 instalado, paridade com a base do cliente, CRUD Mongo e revisão visual completa não são alegados. O verificador integral anterior permanece separado para a homologação no ambiente de destino.
