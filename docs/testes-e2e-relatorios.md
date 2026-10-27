# Testes ponta a ponta dos relatórios e do diagnóstico

Data: 2026-10-23. Arquivo de testes: `test_e2e_reports.py`. Ferramenta exercitada: `engenharia/ferramentas/diagnostico_relatorios.py`.

## O que a suíte cobre

A suíte sobe um processo **uvicorn real** em uma porta livre de loopback e conversa por HTTP com a API. Ela também lê a base configurada no `.env` para calcular os totais de forma independente e comparar com a resposta da API. Nada é gravado no MongoDB.

- **Superfície**: `/api/status`, `/api/collections`, `/api/schema`, `/api/values`, a página inicial e o `app.js`.
- **Sete relatórios**: faturamento, produtos, diário, empresas, documentos, formas de pagamento e ticket médio.
- **Todos os parâmetros de requisição**: cada um tem um teste que mostra o efeito esperado e uma explicação em caso de erro.
- **Perguntas em linguagem natural**: cada tipo de pergunta é classificado e devolve o mesmo contrato do relatório.
- **Conferência**: aritmética de itens × pagamentos, totais por tipo, contagem de documentos e truncamento.
- **Autenticação**: um segundo servidor com Basic Auth confirma 401 sem credenciais, 200 com credenciais e que a página inicial continua pública.
- **CLI**: `--help`, `--explain`, `--year`, `--company`, `--date`, `--seller`, `--seller-ref` e os erros de entrada.
- **Documentação**: os testes falham se um campo dos modelos ou uma flag da CLI deixar de estar documentado neste arquivo.

## Como executar

```powershell
# suíte completa (HTTP + banco + CLI)
.\.venv\Scripts\python.exe test_e2e_reports.py -v

# apenas os testes unitários que não precisam de banco
.\.venv\Scripts\python.exe -m unittest test_e2e_reports.ToolUnitTests -v
.\.venv\Scripts\python.exe -m unittest test_e2e_reports.DocumentationTests -v
```

Os testes de integração descobrem um mês com vendas na base atual para as comparações de relatórios. O CLI descobre ano, data e vendedor presentes na conexão. Perguntas com datas explícitas continuam verificando a interpretação dessas datas, mesmo sem movimentos no período. O caso histórico de 05/10/2026 fica separado e é **skipped** quando o vendedor de referência não existe. Se o MongoDB não estiver acessível, as classes de integração são **skipped**, não falham. `test_live_workspace.py` verifica o workspace com o conjunto atual, incluindo totais independentes, paginação, perfis, exportação e deduplicação de alertas.

## Parâmetros da API de relatório

Todos aparecem em `ReportRequest`; `AskRequest` reutiliza os mesmos campos e adiciona `question`; `ReconciliationRequest` usa o subconjunto indicado.

| Parâmetro | Tipo | Obrigatório | O que significa e o que o teste verifica |
|---|---|---|---|
| `collection` | string | sim | Coleção MongoDB consultada. O teste usa `Movimentacoes`; a API devolve 404 para coleção inexistente. |
| `kind` | string | sim | Um de `revenue`, `top_products`, `daily`, `companies`, `documents`, `payments`, `ticket`. O teste percorre todos e rejeita valor desconhecido com 422. |
| `start` | data | sim | Início inclusivo do período. O teste usa um único dia e confirma que o total é menor ou igual ao do mês. |
| `end` | data | sim | Fim inclusivo; a API soma até o fim do dia. `start > end` devolve 400 com mensagem sobre a data final. |
| `date_field` | string | sim | Campo de data da coleção. O teste usa `DataHoraEmissao`; caractere inválido devolve 400. |
| `value_field` | string/null | depende | Campo de valor ou pseudo-campo `@item_total`. Obrigatório em `revenue` e `ticket`; o teste confirma o 400 e a mensagem "valor". |
| `product_field` | string/null | depende | Campo de produto usado em `top_products`; obrigatório nesse tipo. |
| `company_field` | string/null | depende | Campo de empresa. Obrigatório em `companies` e quando `company_value` é usado; o teste confirma o 400. |
| `company_value` | string/null | não | Filtra uma empresa. O teste compara com o total sem filtro e confirma zero para empresa inexistente. |
| `only_valid` | bool | não | Exclui `Cancelado`, `Rejeitado` e `Inutilizado`. O teste compara com `only_valid=false` e confere a diferença pelo `count_documents` independente. |
| `only_sales` | bool | não | Compatibilidade: exclui compras (`$nin` dos tipos de compra). O teste principal usa `movement_group`; o campo continua aceito. |
| `movement_group` | string/null | não | Escopo explícito: `fiscal_sales`, `all_sales`, `nfce`, `nfe`, `manual`, `registro_venda`, `purchases`, `all`. O teste confirma a ordem `nfce ≤ fiscal_sales ≤ all_sales ≤ all` e que compras têm documentos. |
| `limit` | int | não | Máximo de linhas (1..`MAX_REPORT_ROWS`, padrão 500). O teste confirma o corte em 3 linhas e rejeita 0 e 100000 com 422. |
| `question` | string | só em `/api/ask` | Pergunta em português. O teste confirma a classificação de cada um dos sete tipos, o resumo e o período explícito. Pergunta vazia devolve 400. |

### Campos específicos de `/api/reconciliation`

`reconciliation` usa `collection`, `start`, `end`, `date_field`, `company_field`, `company_value`, `only_valid`, `only_sales`, `movement_group` e `limit`. O teste verifica:

- `items_total − payments_total = difference` (com arredondamento de centavos);
- `documents_with_payment + documents_without_payment = documents`;
- a soma de `documents`, `items` e `payments` em `by_type` bate com os totais;
- `truncated` é verdadeiro apenas quando há mais diferenças que linhas;
- `start > end` devolve 400.

## O que cada tipo de relatório devolve

| `kind` | Linhas | Verificação independente |
|---|---|---|
| `revenue` | uma linha com `total` e `items` | total e itens iguais à agregação `$sum` do `@item_total` no Mongo; `payments_total` igual à soma de `ItensPagamentos`/`Pagamentos`. |
| `top_products` | produto, itens e total | soma dos totais igual ao total do período; soma dos itens igual ao número de itens; produto agrupado por `ItensBase[].ProdutoServico.Descricao`. |
| `daily` | dia, itens, total e documentos | soma dos totais igual ao período; soma de `documents` igual ao total de documentos; dias em ordem crescente. |
| `companies` | empresa, itens e total | soma dos totais igual ao período; empresas agrupadas por `Empresa.Nome`. |
| `documents` | data, número, série, empresa, produto, valor e situação | quantidade limitada por `limit`; datas dentro do período; nenhum documento completo é exposto. |
| `payments` | espécie, total, lançamentos e participação | soma dos totais igual aos pagamentos; participação soma 100%; documentos com pagamento não excedem o escopo. |
| `ticket` | dia, documentos, total e ticket | soma dos totais igual ao período; soma de documentos igual ao total; ticket = total ÷ documentos. |

## Parâmetros da CLI de diagnóstico

| Parâmetro | O que faz | Como o teste valida |
|---|---|---|
| `--year` | Ano do relatório mensal (01/01 a 31/12), NFC-e concluída com finalidade normal. | Compara o `TOTAL` impresso com a agregação independente do ano. |
| `--company` | Filtra `Empresa.Nome` no mensal; sem ele, todas as empresas. | Confere que o total filtrado é menor ou igual e que uma empresa inexistente dá zero. |
| `--date` | Dia do relatório de documentos compartilhados; data inclusiva. | Reproduz 05/10/2026 e os documentos 001/217242 e 001/217322. |
| `--seller` | Nome ou parte do vendedor; resolve pelo cadastro. Se houver mais de uma pessoa, escolhe a de mais movimentações; empate pede `--seller-ref`. | Resolve "VENDEDORA 2" e confere a referência `6496ee…`. |
| `--seller-ref` | ObjectId exato do vendedor (24 hex). | Usa a referência direta no caso de 05/10. Referência inexistente devolve erro com "not found". |
| `--explain` | Explica as duas bases de cálculo, cada parâmetro e as colunas de saída, sem consultar a base. | Confere os textos essenciais e todas as flags. |

### Colunas da saída de documentos compartilhados

- `documento`: total do documento inteiro (todos os vendedores).
- `VENDEDORA`: total somente dos itens do vendedor escolhido.
- `outros`: `documento − VENDEDORA`, os itens dos outros vendedores.
- `pago`: soma de `ItensPagamentos[].Valor` ou `Pagamentos[].Valor`.

## Como a independência é garantida

Os testes **não chamam** `run_report` escondido para conferir a resposta. Eles montam um pipeline Mongo próprio (`scope_match`, `item_rows`, `payment_total`) com a fórmula do item duplicada de propósito. Assim:

- um erro no filtro de escopo aparece como diferença de contagem;
- um erro na fórmula aparece como diferença de centavos;
- um erro no agrupamento aparece como diferença de soma por dia/produto/empresa;
- um erro na paginação aparece no limite de linhas e no `truncated`.

A fórmula duplicada é intencional: se ela e a implementação mudarem juntas, o teste de documentação e o teste de arredondamento ainda cobrem o contrato.

## Resultado da execução (2026-10-23)

- `test_e2e_reports.py`: **23 testes, OK**, ~285 s, incluindo dois servidores uvicorn reais (normal e com Basic Auth) e a CLI.
- Suítes existentes: `test_company_interpretation.py` 4/4, `test_payments_report.py` 10/10, `test_revenue_formula.py` 7/7, `test_ticket_report.py` 9/9, `test_coverage_report.py` 5/5.
- `coverage_report.py`: 122 continuações, 249 fontes verificadas, 0 pendências, sem problemas de integridade.
- O processo uvicorn é encerrado no `tearDownClass`; as portas 8090/8091 permaneceram livres após a execução.

## Limites e interpretação de falhas

- A suíte é **somente leitura**; ela não altera coleções, não executa assemblies e não chama a IA externa.
- A base é a configurada em `.env`/`app.MONGO_DATABASE`; os testes de integração são skipped quando o Mongo não responde.
- Setembro/2026 e 05/10/2026 são usados como períodos históricos. Se a base for trocada por uma sem esses dados, os testes de relatório falham por total zero — confira o banco antes de tratar como regressão de código.
- O ZIP de backup não é restaurado pela suíte; use a base ativa ou restaure o ZIP e aponte `MONGO_DATABASE` para a instância restaurada.
- Uma falha em `test_every_api_parameter_is_documented` significa que um campo novo entrou nos modelos e este documento precisa ser atualizado.
- Uma falha em `test_only_valid...` significa que a diferença entre incluir e excluir cancelados não bateu com o `count_documents`; verifique se a situação mudou no período.
