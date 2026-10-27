# Rotinas para novas funcionalidades da 1.0.1-rc.1

Pesquisa estática de 27/10/2026: **35 declarações em 13 arquivos locais**, cobrindo Financeiro (pagamentos e recebimentos), Estoque por lote e Controle de entrega. As rotinas abaixo ainda não foram implementadas no Insight.

O catálogo existente tem 358 entradas e 122 continuações que cobrem as entradas antes pendentes; sua verificação encontrou 249 fontes íntegras e zero entradas sem continuação. Este trabalho aprofunda regras para implementação e não aumenta artificialmente a cobertura do catálogo. Não houve nova descompilação, execução de assembly nem consulta ao banco.

As fontes de aplicação, projeção e WCF vêm da pasta preservada `2.0.26273.6`; domínio, repositório e tela vêm da árvore `codigo-descompilado`. O manifesto [routines-1.0.1-rc.1.json](routines-1.0.1-rc.1.json) registra caminho, SHA-256 e linha de cada declaração. Esses hashes identificam arquivos C# locais; não comprovam por si só a identidade dos assemblies instalados nem compatibilidade entre as duas árvores.

## Ordem proposta para a release

| Prioridade | Funcionalidade | Pergunta que deve responder | Dependência antes de publicar |
|---|---|---|---|
| 1 | Lotes vencidos e próximos do vencimento | “Quais lotes com estoque vencem nos próximos 30 dias?” | Conferir a serialização dos lotes e o vínculo produto/empresa; homologar a fronteira de hoje |
| 2 | Pendências de entrega por produto | “O que ainda falta entregar deste produto?” | Conferir coleção, itens e situações; reproduzir devoluções e data por ObjectId |
| 3 | Contas a receber/pagar com vencimentos e atraso | “Quanto tenho vencido e o que vence nesta semana?” | Reconstituir histórico financeiro, juros/multa e quitação parcial; não usar total de vendas como saldo |

As três propostas são consultas de leitura com filtros tipados, paginação e exportação no workspace. O classificador atual é específico de vendas: os novos módulos precisam de planos e executores próprios, com seleção explícita de módulo antes de ampliar a interpretação de perguntas. Não reutilizar silenciosamente `Movimentacoes` para tudo.

## 1. Estoque: vencimento de lotes

Cadeia confirmada: `ListaProdutosLotesValidade.ObterRelatorio` → `ProdutoServicoRelatorioModuleService.ObterProdutosComValidadeEmXDias` → `ProdutoServicoRelatorioAppService.ObterProdutosComValidadeEmXDias`.

Fontes principais: [tela](../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLotesValidade.Designer.cs), linhas 27–44 e 78; [serviço](../engenharia/assemblies-descompilados/2.0.26273.6/DigisatServer.Application.Main/DigisatServer.Application.Main/AuxiliaresModulo/Services/ProdutoServicoRelatorioAppService.cs), linha 1440; ponte no manifesto `expiry_bridge`.

- Parte de produtos ativos, por identidade de empresa, restringindo a `ProdutoEmpresa` com estoque do tipo `EstoqueLote`.
- Sem a opção de vencidos, considera `DataValidade.Date > hoje` e `<= hoje + dias`. **Um lote que vence hoje não entra nesse modo.**
- Com vencidos habilitados, considera toda validade `<= hoje + dias`, sem limite inferior. Não significa apenas lotes vencidos: também inclui os futuros dentro da janela.
- O argumento chamado `exibirItemSemQuantidade` tem nome enganoso: o título da tela é **“Exibir somente itens com quantidade em estoque”**. Quando verdadeiro, mantém lotes com quantidade positiva e remove produtos cuja soma dos lotes selecionados não seja positiva. A tela e a implementação são consistentes; não inverter o booleano por causa do nome.
- Exibe código interno, descrição, preço de venda vigente, número do lote, fabricação, validade e quantidade. Preço vigente não é custo histórico nem perda realizada.
- O nativo filtra os lotes em memória após carregar produtos. No Insight, projetar apenas campos necessários e paginar no servidor, mantendo os resultados equivalentes.

Contrato proposto: empresa obrigatória; dias de horizonte; incluir vencidos; somente saldo positivo; data de referência visível. O perfil “compatível com nativo” deve preservar a exclusão de hoje. Se adicionarmos “inclui vencimentos de hoje”, deve ser uma opção explícita e testada.

## 2. Entregas: saldo por item e por produto

Cadeia confirmada: `ControleEntregaModuleService` → `ControleEntregaAppService.ObterControlesEntregaPorPeriodo` ou `ObterControlesEntregaPorProduto` → especificação e projeção. Fontes: [aplicação](../engenharia/assemblies-descompilados/2.0.26273.6/DigisatServer.Application.Main/DigisatServer.Application.Main/MovimentacoesModulo/ControleEntregaAppService.cs), linhas 146 e 158; [especificação](../engenharia/codigo-descompilado/DigisatServer.Domain.Main/DigisatServer.Domain.Main/Aggregates/MovimentacoesAgg/Saidas/ControlesEntrega/ControleEntregaSpecification.cs), linhas 17–73; [item](../engenharia/codigo-descompilado/DigisatServer.Domain.Main/DigisatServer.Domain.Main/Aggregates/MovimentacoesAgg/Saidas/ControlesEntrega/ControleEntregaItem.cs), linha 501 e helpers 849–903.

- `PorDataInclusao` constrói limites de **ObjectId** e compara `Id`; não faz uma comparação direta com o campo `DataInclusao`. O DTO apresenta `DataInclusao`, portanto filtro e data exibida não são necessariamente intercambiáveis em dados importados.
- A especificação usa `ObjectId.GenerateNewId` nas fronteiras, não sentinelas de primeiro/último ObjectId do segundo. Horário, conversão para UTC e documentos no segundo de fronteira exigem conferência antes de alegar equivalência exata.
- O relatório por período parte de uma especificação verdadeira; fora do consolidado acrescenta empresa. Esse caminho não acrescenta explicitamente `Ativo`. O relatório por produto usa `AtivoPorEmpresa`, período e produto; depois filtra novamente os itens do documento pelo produto. Não presumir que ambos os relatórios tenham o mesmo conjunto de documentos.
- No filtro de situação, código **4** significa `Situacao != null`; os demais códigos comparam `Situacao.Codigo`. Não traduzir 4 como uma situação operacional específica.
- O getter de quantidade pendente, resolvido pelos helpers ofuscados, equivale a:

  `QuantidadePendente = Quantidade − QuantidadeEntregue + QuantidadeDevolvida`

  Exemplo: 10 pedidos, 7 entregues e 2 devolvidos deixam **5 pendentes**, porque a devolução volta a compor o saldo. Subtrair devolução produziria incorretamente 1.
- O DTO por produto apresenta quantidade, devolvida, entregue e pendente, além de código, descrição, marca, cliente e data. Não misturar unidades de produtos diferentes em um único indicador de quantidade.

Contrato proposto: empresa, período de criação, produto e situação; tabela por controle/item; filtro de saldo pendente positivo; números de entregue/devolvido/pendente lado a lado. Manter operações de entrega e devolução fora do executor de consultas.

## 3. Financeiro: vencimentos, atraso e quitação

Rotinas de entrada no [serviço financeiro](../engenharia/assemblies-descompilados/2.0.26273.6/DigisatServer.Application.Main/DigisatServer.Application.Main/AuxiliaresModulo/Services/PagamentoRecebimentoRelatorioAppService.cs): pagamentos em aberto (85), pagamentos quitados (100), recebimentos em aberto (164) e recebimentos quitados (189). As pontes WCF injetam a identidade do usuário; não recebem uma empresa arbitrária da pergunta.

Regras confirmadas:

- Em aberto usa **vencimento** e situação pendente; quitado usa **data de quitação** e situação quitada. Ambos incluem os dias inicial e final pelas especificações. “Recebido no mês” e “com vencimento no mês” são consultas diferentes.
- Sem consolidado aplica ativo por empresa; consolidado usa ativos sem a restrição de uma empresa. No Insight, consolidação deve exigir seleção explícita das empresas autorizadas, pois ainda não há o contexto de permissões do Suite.
- Recebimentos aceitam conta; espécie **99** significa não restringir espécie. O filtro de espécie do nativo é aplicado em memória. Pagamentos em aberto verificam o limite nativo de 50 mil registros antes do recorte por vencimento; esse limite não é uma meta de desempenho do Insight.
- A [especificação de recebimentos](../engenharia/codigo-descompilado/DigisatServer.Domain.Main/DigisatServer.Domain.Main/Aggregates/FinanceirosAgg/PagamentosRecebimentos/ParcelaRecebimentoSpecifications.cs), linhas 112 e 123, separa pendentes em dia (`Vencimento >= hoje`) e atrasados (`Vencimento < hoje`). Vencimento de hoje não está atrasado.
- No [repositório](../engenharia/codigo-descompilado/DigisatServer.Infrastructure.Data.Main/DigisatServer.Infrastructure.Data.Main/Repositories/ParcelaRepositoryRead.cs), `ObterTotais` (43) chama `SomarUltimoValorHistoricoPorSituacao` (51): agrupa por `Situacao.Codigo` e soma o **último `Historico.Valor`**, usando `$arrayElemAt(..., -1)`. Não soma todos os históricos nem o valor inicial de cada título.
- `PreencherTotalAtrasado` (77) seleciona código 1 e vencimento anterior a hoje; soma o valor do último histórico. Para recebimentos calcula separadamente o atrasado com juros/multa a partir de `ValorTotal`.
- Na [projeção](../engenharia/assemblies-descompilados/2.0.26273.6/DigisatServer.Application.Core/DigisatServer.Application.Core/Projections/FinanceirosProjection/PagamentosRecebimentos/PagamentoRecebimentoProjection.cs), método 234, último histórico `HistoricoQuitadoParcial` muda `Valor` e `ValorCorrigido` para `ValorQuitacao` e o rótulo para “Quitada parcial”. Portanto, somar a coluna de um relatório sem entender o histórico não garante saldo em aberto.
- `ObterTotalPorSituacao` converte a soma para `double` e arredonda com `Math.Round`. Isso não é a mesma regra `AwayFromZero` já usada nos itens de venda. Registrar a política de arredondamento própria do módulo antes de definir equivalência financeira.

Contrato proposto: direção pagar/receber; eixo de data vencimento/quitação; situação; empresa; conta e espécie quando suportadas. Exibir saldo principal, corrigido e quitado em colunas distintas, com data de referência. Faixas 1–30, 31–60 e 61+ dias seriam uma **extensão do Insight**, não uma rotina nativa já comprovada por este rastreio.

Bloqueio específico antes de implementar valores financeiros: rastrear os getters de `ValorTotal`, juros, multa, desconto e os tipos de histórico até seus cálculos; conferir a coleção e os discriminadores reais, incluindo quitação parcial. Não consultar o banco nem reproduzir cobrança nesta etapa de pesquisa.

## Casos mínimos de aceitação para implementação

| Módulo | Entrada controlada | Resultado esperado |
|---|---|---|
| Lotes | Hoje, amanhã e dia 30; horizonte 30; sem vencidos | Amanhã e dia 30 entram; hoje não |
| Lotes | Ontem e dia 31; horizonte 30; com vencidos | Ontem entra; dia 31 não |
| Lotes | Lote zero e lote positivo; somente estoque positivo | Apenas o positivo entra |
| Lotes | Mesma mercadoria em duas empresas | Empresa selecionada controla os lotes |
| Entregas | Pedido 10, entregue 7, devolvido 2 | Pendente 5 |
| Entregas | Controle com produtos A e B; filtro A | Somente os itens de A entram |
| Entregas | Mesmo horário em `DataInclusao`, ObjectIds com datas distintas | Exercitar a diferença entre filtro nativo por Id e data apresentada |
| Financeiro | Pendente com vencimento ontem e outro hoje | Só o de ontem está atrasado |
| Financeiro | Histórico de valores 100 e 60 | Total por situação usa 60, não 160 nem 100 |
| Financeiro | Quitação no mês seguinte ao vencimento | Resultado depende do eixo de data escolhido |
| Financeiro | Último histórico de quitação parcial | Conferir valor exibido, quitado e saldo separadamente |
| Financeiro | Espécies diferentes e filtro 99 | Todas as espécies do conjunto elegível entram |

## Reproduzir a verificação das fontes

Executar `.\.venv\Scripts\python.exe tools/research-module-routines.py`. A verificação falha se faltar fonte, se uma declaração deixar de ser única ou se conteúdo/linha divergir do manifesto. `--write` atualiza o manifesto apenas após revisão intencional. O comando não chama REA, DigiSat, MongoDB ou serviços externos.

Esta entrega é pesquisa e backlog implementável para a próxima RC. A versão e o comportamento do aplicativo permanecem na linha 1.0.0-rc.3.
