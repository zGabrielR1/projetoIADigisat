# DigiSat Insight 1.0.1-rc.1

Primeiro módulo novo derivado do [rastreio das rotinas](modulos-1.0.1-rc.1.md): **Validade de lotes**, separado dos relatórios de vendas.

- Empresa obrigatória, referência de data e horizonte de até 366 dias.
- Lotes vencidos/a vencer, filtro de saldo positivo, busca por produto/código/lote e paginação.
- Quantidade física reconstruída pelos movimentos assinados cujo `EstoqueReferencia` é o identificador do lote, nas coleções reais `Estoques`, `ProdutosServicosEmpresa` e `ProdutosServicos`.
- Consulta somente de produtos ativos da empresa selecionada; valores de estoque não são confundidos com faturamento.
- CSV e Excel da página original, com origem, filtros e avisos; consultas completas de vendas não são aplicadas aos planos de estoque.
- Limites explícitos: 20.000 lotes/20 segundos; falhas recusam o resultado em vez de entregar saldos parciais.

O esquema local foi conferido em leitura: lotes em `Estoques.Itens`, movimentos assinados em `Estoques.Quantidades`, ligação por `ProdutosServicosEmpresa.EstoqueReferencia` e cadastro por `ProdutoServicoReferencia`. A amostra possui dez estoques de lote. Datas de validade amostradas têm hora 00:00 no valor retornado pelo driver. Isso não homologa todos os formatos ou fusos de outras instalações.

A regra nativa exclui o vencimento da própria data quando “incluir vencidos” está desativado. A quantidade é física, sem desconto de reservas. Lotes compostos por grade ou serial são recusados nesta RC; não são tratados como saldo zero. Contas a pagar/receber e pendências de entrega continuam no backlog de pesquisa, sem funcionalidades anunciadas como concluídas.

Testes repetíveis: `python -m unittest test_stock` e `python -m unittest test_e2e_reports.ApiE2E.test_stock_expiry_matches_independent_lot_balances`. O primeiro usa dados sintéticos; o segundo exige a base local de homologação e só lê o MongoDB. Exportações e snapshots usam o SQLite/diretório local do Insight.

Validação: 97 testes de cálculo, interpretação, workspace, estabilidade e estoque aprovados, mais 24 testes E2E (121 no total). A suíte HTTP completa passou em 182,402 segundos e inclui a conferência independente de lotes. O navegador automatizado não estava disponível nesta sessão, portanto a conferência visual da nova tela permanece pendente.

O [relatório do executável](verification-1.0.1-rc.1.json) registra os gates aprovados e o SHA-256 do binário verificado (`0fc27f71837896f39d7343e727b717e8248db30088f805fd8da38a2bc03a63c0`). O ZIP preparado passou na lista explícita de conteúdo, na conferência do PE Windows x64 e em todos os hashes do manifesto.

**Limitação encontrada na rodada ampliada:** o [teste do ZIP isolado](stability-1.0.1-rc.1.json) aprovou configuração/reinício, backup/restauração, rollback/upgrade e três consultas concorrentes. Investigação diária e faturamento mensal/anual passaram nos orçamentos aplicados. Duas investigações anuais e a consulta anual de documentos retornaram HTTP 422; a etapa posterior de cancelamento da exportação anual não foi concluída. Portanto, a rodada ampliada terminou com falha e não deve ser descrita como estabilidade integral aprovada. Consultas anuais de investigação/documentos precisam de revisão adicional; o módulo novo de lotes foi validado separadamente.

Permanecem pendentes a homologação em Windows limpo, a aceitação dos operadores e as medições no volume de destino. Esta entrega continua sendo candidata à homologação.
