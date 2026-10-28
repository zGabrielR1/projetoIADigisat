# DigiSat Insight 1.0.3 RC3

A RC3 prioriza busca e filtros inteligentes, preservando a interface e os recursos da RC2.

- Sugestões de situação, base de data, operação de cartão e consolidação, extraídas das opções documentadas do próprio relatório e validadas pelo backend. Não se inventam códigos, pessoas, empresas ou referências.
- Pedidos como **contas a receber atrasadas deste mês** sugerem **Atrasadas**, **Data de vencimento** e o período reconhecido. O usuário pode revisar e alterar os filtros.
- Pedidos com pagar/receber ou situações conflitantes são sinalizados. Relatórios de situações incompatíveis deixam de competir com o pedido específico.
- Quando uma classe possui chamadas auxiliares e uma consulta parametrizada, a tela prioriza a chamada parametrizada, mantendo a seleção manual dos conjuntos.
- Quadro de sugestões, ação para removê-las e revisão dos filtros reais antes da consulta. Buscar e revisar não executam o serviço de dados.
- Links de sugestões aceitam somente datas, booleanos e opções catalogadas; referências cadastrais e objetos livres são rejeitados.
- A extração das opções financeiras compartilhadas foi corrigida para resolver os ramos de pagamento/recebimento das fontes retidas, com conferência dos hashes antes da leitura. Tipos e assinaturas continuam validados separadamente contra as bibliotecas instaladas.

## Verificação e limites

Seis testes de filtros inteligentes cobrem situação e base de data, cartão, ambiguidade, chamada principal, rejeição de sugestões arbitrárias e validação das propostas para todas as entradas disponíveis. Permanecem os testes de descoberta das 358 identidades, ponte nativa e regressão do workspace. Os gates do executável verificam a prévia sem execução, a rejeição de referências por links, autenticação, exportações e integridade do pacote.

A automação de navegador voltou a informar inventário vazio durante a conferência desta RC3. Os fluxos da RC2 foram conferidos anteriormente, mas a nova revisão visual da RC3 não é declarada aprovada.

Continuam disponíveis 356 entradas nativas e 358 fichas em 40 módulos. Duas fichas cadastrais estão ausentes do assembly. A busca não responde perguntas arbitrárias sobre toda a base e não executa relatórios automaticamente; a consulta exige seleção, filtros e login normal SuiteG6. Filtros/cálculos locais das classes cliente e layouts de impressão continuam fora da ponte.
