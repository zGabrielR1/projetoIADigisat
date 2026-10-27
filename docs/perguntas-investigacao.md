# Investigação de vendas por pergunta

`POST /api/ask` reconhece a intenção de investigar uma diferença e extrai data, vendedor e valor mencionado. A extração local prevalece sobre uma classificação genérica do provedor opcional. O modelo pode escolher `investigation`, mas não cria consultas, campos ou código.

`POST /api/investigation` permite repetir a investigação com filtros editados. Parâmetros: `collection`, `start`, `end`, `date_field`, `company_field`, `company_value`, `only_valid`, `only_sales`, `movement_group`, `limit`, `seller_query` e `target_amount`. Usa Movimentacoes/DataHoraEmissao e períodos de até 367 dias. O limite controla a listagem; os totais são calculados antes desse limite. Não há escrita em MongoDB.

Vendedor é resolvido contra Pessoas.Nome ou referência, com participação de itens não cancelados no período quando há vários candidatos. Ambiguidade restante ou vendedor inexistente produz HTTP 422; não substitui o vendedor por “todos”. Os totais dos documentos incluem os itens de todos os vendedores, enquanto o total próprio inclui apenas os itens do vendedor selecionado. Pagamentos são do documento inteiro, preferindo ItensPagamentos e usando Pagamentos como alternativa.

Exemplo verificado em 26/10/2026: “Existe uma diferença aparecendo no dia 5/10 no vendedor 2 de cerca de R$100,00 que não consegui descobrir.” Resultou em 37 vendas, R$ 2.145,11 de itens, R$ 2.040,15 do vendedor e R$ 104,96 dos demais/sem vendedor. Pagamentos: R$ 2.145,11. A diferença de atribuição aparece nas vendas 001/217242 (R$ 64,98) e 001/217322 (R$ 39,98). Resultados dependem da base e dos filtros, sem valores ou referências desse caso fixados na aplicação.

Validação: `python -m unittest test_question_investigation` cobre interpretação, datas inválidas, ambiguidade, limite sem mudança nos totais e cálculo independente no MongoDB. Os testes permanecem fora da entrega.
