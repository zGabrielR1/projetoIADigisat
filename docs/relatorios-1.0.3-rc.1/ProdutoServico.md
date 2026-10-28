# Produto ou Serviço

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Alterações de estoque MEI

Classe: `AlteracaoEstoqueMei`. Identificador: `ProdutoServico.AlteracaoEstoqueMei`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Produto

**Campos apresentados (DTO):** `QuantidadeInformada`, `NomeUsuario`, `DataAlteracao`, `QuantidadeAnterior`, `Quantidade`, `Descricao`

**Agrupamentos do layout:**

- `CodigoInterno`
- `CodigoBarras`
- `Descricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterAlteracoesEstoqueMei`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [AlteracaoEstoqueMei.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/AlteracaoEstoqueMei.cs) — SHA-256 `66e9d6b54cd665227f843dd46e6a3ed385e3d2be93950099c0affa19ae4e7331`
- [AlteracaoEstoqueMei.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/AlteracaoEstoqueMei.Designer.cs) — SHA-256 `c5ce3cee93f65b8c806afee42101c70a0f6972b75e5f31d532cf9cf1ef74e143`

## Cadastro de produtos e serviços

Classe: `CadastroProdutosServicos`. Identificador: `ProdutoServico.CadastroProdutosServicos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo item; NCM; Gênero item; Marca; SubGrupo

**Campos apresentados (DTO):** `Cest`, `CodigoGenero`, `CodigoInterno`, `Descricao`, `TipoItem`, `Ncm`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterCadastroProdutoServico`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [CadastroProdutosServicos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/CadastroProdutosServicos.cs) — SHA-256 `b149c1965f689b0ec61b25ef5867aadfc2c785b3b00f7c2aee1d02270a327d0c`
- [CadastroProdutosServicos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/CadastroProdutosServicos.Designer.cs) — SHA-256 `7cf0d7ca9b55148086919eff031b7831a25a6aff387bca415475b1c6651a72a1`

## Catálogo

Classe: `ListaProdutosCatalogo`. Identificador: `ProdutoServico.ListaProdutosCatalogo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Situação; Mostrar em quantas colunas; Tabela de preço; Marca; SubGrupo; Ordenação; Mostrar preço e quantidade

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Expressões calculadas do layout:**

- `Iif([Ativo]==True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosCatalogo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosCatalogo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosCatalogo.cs) — SHA-256 `6b02f837bc3296219e16b1b7ee95b44d828dd9d772ce8a3012940fb7d18bd0ce`
- [ListaProdutosCatalogo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosCatalogo.Designer.cs) — SHA-256 `6f1d28efd3e36baa55af57055c76b1de5f9bd5be6699b3ea9a94832e1d5c8f18`

## Comparativo anual de saídas por produto

Classe: `ComparativoAnualSaidasPorProduto`. Identificador: `ProdutoServico.ComparativoAnualSaidasPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ano A; Ano B; Produto

**Campos apresentados (DTO):** `DescricaoToText`, `Meses.DiferencaPercentualFormatada`, `Meses.DescritivoMes`, `Meses.QuantidadeAnoA`, `Meses.QuantidadeAnoB`, `Meses.Diferenca`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ComparativoAnualSaidasPorProduto`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [ComparativoAnualSaidasPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ComparativoAnualSaidasPorProduto.cs) — SHA-256 `f9d300efd3feabc337b12e4e0c316ee7ed858c4ad41630893c31196c8b4b46c9`
- [ComparativoAnualSaidasPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ComparativoAnualSaidasPorProduto.Designer.cs) — SHA-256 `70a76faf6e86a499d0b73b51b6af42ed6703fe0cf261b8b6153a551cb2829fe2`

## Componentes utilizados em montagens de composições

Classe: `ComponentesUtilizadosMontagemComposicao`. Identificador: `ProdutoServico.ComponentesUtilizadosMontagemComposicao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `Quantidade`, `Descricao`, `CodigoInterno`

**Contratos de serviço:**

- `IComposicaoModuleService`

**Chamadas catalogadas:**

- `IComposicaoModuleService.ObterComponentesUtilizadosPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [ComponentesUtilizadosMontagemComposicao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ComponentesUtilizadosMontagemComposicao.cs) — SHA-256 `a3833c2fcd7d6f988de6974f136add7b017e8185276bd0708067ffa43ed9e6ee`
- [ComponentesUtilizadosMontagemComposicao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ComponentesUtilizadosMontagemComposicao.Designer.cs) — SHA-256 `d4596a42714627b4f52f56e241bbf0ec97308f7902a137e3d40ee8bff049d506`

## Composição

Classe: `Composicao`. Identificador: `ProdutoServico.Composicao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Composição

**Campos apresentados (DTO):** `CodigoDescricao`, `PrecoVenda`, `PrecoCusto`, `Componentes.CustoTotal`, `Componentes.Quantidade`, `Componentes.VendaTotal`, `Componentes.PrecoCusto`, `Componentes.Codigo`, `Componentes.Descricao`, `Componentes.PrecoVenda`, `Componentes.QuantidadeDisponivel`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterComposicoes`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [Composicao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/Composicao.cs) — SHA-256 `e7b7eae98057ed62f3d6d26acffa3e4544cc6cbf71ad27be3c7b8122a5dd8b52`
- [Composicao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/Composicao.Designer.cs) — SHA-256 `8de225b96ab84581f92cd4b36eabb4c46b4e160d23f3d7495491925fddb6f514`

## Grade por produto

Classe: `ProdutoGrade`. Identificador: `ProdutoServico.ProdutoGrade`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** SubGrupo; Marca; Ordenação dos produtos; Ordenação da grade; Situação; Agrupar por marca; Agrupar por subgrupo; Linha/Coluna; Mostrar somente grades com quantidade disponível

**Campos apresentados (DTO):** `Descricao`, `UnidadeVenda`, `PrecoVenda`, `CodigoInterno`, `Grades.BarrasGrade`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Quantidade`, `ultimaCompra`, `ultimaVenda`, `SubGrupoDescricao`, `Marca`

**Agrupamentos do layout:**

- `Marca`
- `SubGrupoDescricao`

**Expressões calculadas do layout:**

- `Iif([DataUltimaCompra] == '01/01/0001 00:00:00', '-', [DataUltimaCompra])`
- `Iif([DataUltimaVenda] == '01/01/0001 00:00:00', '-', [DataUltimaVenda])`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutosGrade`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ProdutoGrade.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ProdutoGrade.cs) — SHA-256 `a9a3b4d60174a5c933f87bcfb6b4744b293cb66316d3cd6e68019d4c65605fe2`
- [ProdutoGrade.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ProdutoGrade.Designer.cs) — SHA-256 `00d347eb4597a6f71a419e2420d0b33760e776f944a9bf3a00d2cac1f398dedf`

## GTIN inválido

Classe: `ListaProdutosGtinInvalido`. Identificador: `ProdutoServico.ListaProdutosGtinInvalido`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Situação; Ordenação

**Campos apresentados (DTO):** `calculatedField1`, `CodigoInterno`, `CodigoBarras`, `Descricao`, `Quantidade`, `Ativo`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif([Ativo]==True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosBarrasInvalido`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosGtinInvalido.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosGtinInvalido.cs) — SHA-256 `c3f6c266b5a2b58a3f8cdf52ba6a7e0a1b9136b59b6869aaf4edac9ba82bc511`
- [ListaProdutosGtinInvalido.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosGtinInvalido.Designer.cs) — SHA-256 `e5279a8b78d90a94f9264d2ca3481245a93a55e383dd0a4bccc94c94bcf3ff5f`

## Histórico de movimentação por lote

Classe: `HistoricoMovimentacaoLote`. Identificador: `ProdutoServico.HistoricoMovimentacaoLote`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Número do lote; Ordenação

**Campos apresentados (DTO):** `DataValidade`, `DataFabricacao`, `Descricao`, `UnidadeVenda`, `CodigoBarras`, `CodigoInterno`, `Movimentacoes.Quantidade`, `Movimentacoes.NomePessoa`, `Movimentacoes.Cfop`, `Movimentacoes.Serie`, `Movimentacoes.Modelo`, `Movimentacoes.NumeroDocumento`, `Movimentacoes.Origem`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterHistoricoMovimentacaoLote`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [HistoricoMovimentacaoLote.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/HistoricoMovimentacaoLote.cs) — SHA-256 `c160430b3f4f11d5ad11c5c3ad881cbb6df29eea538a1f6bde0dc37eed5ddc76`
- [HistoricoMovimentacaoLote.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/HistoricoMovimentacaoLote.Designer.cs) — SHA-256 `11a4674493c170768b876f14a491d9354013197aed27ce4ae65ec0c68540893a`

## Histórico de movimentação por serial

Classe: `HistoricoMovimentacaoSerial`. Identificador: `ProdutoServico.HistoricoMovimentacaoSerial`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Número do serial; Ordenação

**Campos apresentados (DTO):** `Descricao`, `UnidadeVenda`, `CodigoBarras`, `CodigoInterno`, `Movimentacoes.NomePessoa`, `Movimentacoes.Cfop`, `Movimentacoes.Serie`, `Movimentacoes.Modelo`, `Movimentacoes.NumeroDocumento`, `Movimentacoes.Origem`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterHistoricoMovimentacaoSerial`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [HistoricoMovimentacaoSerial.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/HistoricoMovimentacaoSerial.cs) — SHA-256 `9305ebd75835880adb0b4e20ac65b907b0837eea22daf6e0d4634abf61edf719`
- [HistoricoMovimentacaoSerial.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/HistoricoMovimentacaoSerial.Designer.cs) — SHA-256 `b8a5961039149b90319a824fab2ba173f40303fe1be3b642cc11fa47e59e1c6c`

## Integridade do estoque

Classe: `IntegridadeEstoque`. Identificador: `ProdutoServico.IntegridadeEstoque`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Somente produtos e serviços com chave inválida

**Campos apresentados (DTO):** `chaveValidaToText`, `ativoToText`, `Descricao`, `Codigo`

**Expressões calculadas do layout:**

- `Iif([Ativo] == True, 'Sim', 'Não')`
- `Iif([ChaveValida] == True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterDadosRelatorioIntegridadeEstoque`

**Pesquisa da regra:** Rastreado até regra da chave PAF.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [IntegridadeEstoque.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/IntegridadeEstoque.cs) — SHA-256 `215f2e5315fed9a8c453b590a4aff211a09c7bfc3ac4ce68d588ad9ad8b0d6e2`
- [IntegridadeEstoque.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/IntegridadeEstoque.Designer.cs) — SHA-256 `7535b1a3bb45d11de77fe884a9abb820434c50c1df8bc55c645c076bdd19b35d`

## Lista de preços consolidada

Classe: `ListaPrecosProdutoServicoConsolidado`. Identificador: `ProdutoServico.ListaPrecosProdutoServicoConsolidado`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** SubGrupo; Ordenação

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaPrecosProdutoServicoConsolidado`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaPrecosProdutoServicoConsolidado.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaPrecosProdutoServicoConsolidado.cs) — SHA-256 `195bd59c691d527ad99f823a2c114f6556fbabf3a6cded839de477be8354fd20`
- [ListaPrecosProdutoServicoConsolidado.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaPrecosProdutoServicoConsolidado.Designer.cs) — SHA-256 `5e16c54cf3430201eff1082fd799a7965f40f103a8a322de27b38485d0084d27`

## Lista de produtos

Classe: `ListaProdutos`. Identificador: `ProdutoServico.ListaProdutos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Situação; Tabela de preço; Ordenação; Quanto a opção vendável mostrar itens; Fornecedor; Quantidade em estoque/saldo; Mostrar preços de custo; Quanto a opção pesável mostrar itens; Qual custo mostrar

**Campos apresentados (DTO):** `PrecoCustoMedio`, `CustoMedioTotal`, `PrecoCusto`, `CustoTotal`, `dataUltmVenda`, `dataUltCompra`, `calculatedField1`, `QuantidadeMaxima`, `QuantidadeMinima`, `VendaTotal`, `Quantidade`, `PrecoVenda`, `UnidadeMedida`, `Descricao`, `CodigoBarras`, `CodigoInterno`, `CamposPersonalizados.Informacao`, `CamposPersonalizados.Descricao`

**Expressões calculadas do layout:**

- `Iif([Ativo]==True, 'Sim', 'Não')`
- `Iif(GetYear([DataUltimaCompra])==0001,''  ,[DataUltimaCompra] )`
- `Iif(GetYear([DataUltimaVenda])==0001, '' ,[DataUltimaVenda] )`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutos`

**Pesquisa da regra:** Rastreado até filtros, projeção e pós-processamento.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [ListaProdutos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutos.cs) — SHA-256 `34de24d4ae72b3ba4e5a6937ea60fcf4bbd452a717331137892a590e568a263b`
- [ListaProdutos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutos.Designer.cs) — SHA-256 `b78b5884b76e2f05735c73953cba10b908d7fd928500c7940e72cb924e5825fb`

## Lista de produtos com tributos

Classe: `ListaProdutosTributos`. Identificador: `ProdutoServico.ListaProdutosTributos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Unidade da federação; Tipo do cálculo; Situação; Ordenação

**Campos apresentados (DTO):** `ClassificacaoTributariaIbsCbs`, `Cfop`, `PercentualCbs`, `PercentualIbs`, `Cest`, `CstCofins`, `CstPis`, `CstIpi`, `PercentualIcms`, `BaseCalculoToText`, `CstToText`, `PercentualCofins`, `PercentualPis`, `PercentualIpi`, `Ncm`, `CodigoInterno`, `AtivoInativoTodos`, `Descricao`

**Expressões calculadas do layout:**

- `Iif([Ativo]==True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosComTributos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosTributos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributos.cs) — SHA-256 `5085c7175a07eb70e40e4938e4b30805452f61e4b92979e84ad2f10edbbc0b80`
- [ListaProdutosTributos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributos.Designer.cs) — SHA-256 `63ad8bbd86d683c06b76207e5ca6e6c5da6436cffa873c7cc85f9a931b7a6dfe`

## Lista de produtos com tributos estaduais

Classe: `ListaProdutosTributosEstaduais`. Identificador: `ProdutoServico.ListaProdutosTributosEstaduais`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Unidade da federação; Tipo do cálculo; Situação; Ordenação

**Campos apresentados (DTO):** `PercentualBaseIbsCbs`, `CstIbsCbs`, `PercentualCbs`, `PercentualIbs`, `Cest`, `CodigoBeneficioFiscal`, `PercentualIcms`, `PercentualBaseIcms`, `Cst`, `Ncm`, `CodigoInterno`, `AtivoInativoTodos`, `Descricao`, `ClassificacaoTributariaIbsCbs`

**Expressões calculadas do layout:**

- `Iif([Ativo]==True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosComTributos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosTributosEstaduais.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributosEstaduais.cs) — SHA-256 `45863da2660c0468b2548ee84527450d076794935199b652c3fa000ba7ae2970`
- [ListaProdutosTributosEstaduais.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributosEstaduais.Designer.cs) — SHA-256 `c6c8164b002d721b524fd8e7ad6aff283ee347c5dc5b9f6383c2f687f38a4603`

## Lista de produtos por índice do preço de venda

Classe: `ListaProdutosPorIndicePrecoVendaMarcaSubgrupo`. Identificador: `ProdutoServico.ListaProdutosPorIndicePrecoVendaMarcaSubgrupo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Índice do preço de venda; Marca; SubGrupo; Ordenação

**Campos apresentados (DTO):** `PercentualLucroToText`, `Lucro`, `Venda`, `DescricaoSubgrupo`, `Custo`, `CodigoInterno`, `Descricao`, `DescricaoMarca`, `DescricaoIndicePrecoVenda`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutoServico`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosPorIndicePrecoVendaMarcaSubgrupo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosPorIndicePrecoVendaMarcaSubgrupo.cs) — SHA-256 `667d86be9bb843add12ca593cd3927d7682cfbaad95868e81735b8e4b26ffbb3`
- [ListaProdutosPorIndicePrecoVendaMarcaSubgrupo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosPorIndicePrecoVendaMarcaSubgrupo.Designer.cs) — SHA-256 `08c5498d85278af10ef00b3b59b23ab8a31ce9f3ed589fbeea1d7fd5f577ccda`

## Lista de produtos por linha de produto

Classe: `ListaProdutosLinhas`. Identificador: `ProdutoServico.ListaProdutosLinhas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Linha de produto; Ordenação; Exibir 'Código do fabricante'

**Campos apresentados (DTO):** `Linha`, `Produtos.VendaTotal`, `Produtos.CustoTotal`, `Produtos.QuantidadeMaxima`, `Produtos.QuantidadeMinima`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoFabricante`, `Produtos.CodigoInterno`, `Produtos.CamposPersonalizados.Informacao`, `Produtos.CamposPersonalizados.Descricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosPorLinha`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosLinhas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLinhas.cs) — SHA-256 `9574177f479818324df42804b9b912b726daf0e66f92eb417d2b5195d9919ecf`
- [ListaProdutosLinhas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLinhas.Designer.cs) — SHA-256 `48713032db158ed3414ca75ab50bf7e8d9519c1bc873554a714d898da13dbc39`

## Lista de produtos por marca

Classe: `ListaProdutosMarcas`. Identificador: `ProdutoServico.ListaProdutosMarcas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Marca; Ordenação; Exibir 'Código do fabricante'

**Campos apresentados (DTO):** `Marca`, `Produtos.VendaTotal`, `Produtos.CustoTotal`, `Produtos.QuantidadeMaxima`, `Produtos.QuantidadeMinima`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoFabricante`, `Produtos.CodigoInterno`, `Produtos.CamposPersonalizados.Informacao`, `Produtos.CamposPersonalizados.Descricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosPorMarca`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosMarcas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosMarcas.cs) — SHA-256 `ff0352773b21baf83622f1ec91222d8d782dd4e5c24f6c69edb141e79aa1c92a`
- [ListaProdutosMarcas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosMarcas.Designer.cs) — SHA-256 `bf76cc5cb96071716a2404b2feadb8d99a02bf1555933b38f6aacaef1d933696`

## Lista de produtos por tributação estadual

Classe: `ListaProdutosTributacoesEstaduais`. Identificador: `ProdutoServico.ListaProdutosTributacoesEstaduais`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Tributação estadual; Ordenação

**Campos apresentados (DTO):** `Tributacao`, `Produtos.VendaTotal`, `Produtos.CustoTotal`, `Produtos.QuantidadeMaxima`, `Produtos.QuantidadeMinima`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoInterno`, `Produtos.CamposPersonalizados.Informacao`, `Produtos.CamposPersonalizados.Descricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosTributacaoEstadual`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosTributacoesEstaduais.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesEstaduais.cs) — SHA-256 `8519066c182f8d8b96c4a3840d2bc97aa7c5e31b55ffa352bb1d1460ed45722f`
- [ListaProdutosTributacoesEstaduais.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesEstaduais.Designer.cs) — SHA-256 `47c0ab19bc293ad816ff6a9481e2a5cbd94d5f0395f81e7ef40cb308f37dfe68`

## Lista de produtos por tributação estadual com NCM e CEST

Classe: `ListaProdutosTributacoesEstaduaisComCestENcm`. Identificador: `ProdutoServico.ListaProdutosTributacoesEstaduaisComCestENcm`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Tributação estadual; Ordenação

**Campos apresentados (DTO):** `Tributacao`, `Produtos.CodigoEspecificadorSubstituicaoTributaria`, `Produtos.NcmNbs`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosTributacaoEstadual`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosTributacoesEstaduaisComCestENcm.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesEstaduaisComCestENcm.cs) — SHA-256 `ee3178bdae3af20994254d0e3138fc80efb510276780a828b445801d57f80679`
- [ListaProdutosTributacoesEstaduaisComCestENcm.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesEstaduaisComCestENcm.Designer.cs) — SHA-256 `d354d83c128c1b994ca8d7622b2a03bee39c98845eee61fe3640e7ebfab35391`

## Lista de produtos por tributação federal

Classe: `ListaProdutosTributacoesFederais`. Identificador: `ProdutoServico.ListaProdutosTributacoesFederais`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Tributação federal; Ordenação

**Campos apresentados (DTO):** `Tributacao`, `Produtos.VendaTotal`, `Produtos.CustoTotal`, `Produtos.QuantidadeMaxima`, `Produtos.QuantidadeMinima`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoInterno`, `Produtos.CamposPersonalizados.Informacao`, `Produtos.CamposPersonalizados.Descricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosTributacaoFederal`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosTributacoesFederais.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesFederais.cs) — SHA-256 `106bdfa4a429bcff8e3f2f44a38c727c823812ecf9531ded41191c5d7dfdde95`
- [ListaProdutosTributacoesFederais.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesFederais.Designer.cs) — SHA-256 `5cc1c024ccf7eb4b9331a4ecbad695e22cdc1e5d0340dbd4c42b73ff08fe93d6`

## Lista de produtos por tributação federal com NCM e CEST

Classe: `ListaProdutosTributacoesFederaisComCestENcm`. Identificador: `ProdutoServico.ListaProdutosTributacoesFederaisComCestENcm`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Tributação federal; Ordenação

**Campos apresentados (DTO):** `Tributacao`, `Produtos.CodigoEspecificadorSubstituicaoTributaria`, `Produtos.NcmNbs`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosTributacaoFederal`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosTributacoesFederaisComCestENcm.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesFederaisComCestENcm.cs) — SHA-256 `829a3183121e4dff5c497558de751db0668f47fc7191bdd5e4e4dc0e8d16a913`
- [ListaProdutosTributacoesFederaisComCestENcm.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesFederaisComCestENcm.Designer.cs) — SHA-256 `97d1a36249d9d4e68b6824d66039387b6e696edd9859f9e6a84e7b6cbbb953f0`

## Lista de produtos por tributação IBS/CBS com NCM e CEST

Classe: `ListaProdutosTributacoesIbsCbsComCestENcm`. Identificador: `ProdutoServico.ListaProdutosTributacoesIbsCbsComCestENcm`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Tributação IBS/CBS; Ordenação

**Campos apresentados (DTO):** `Tributacao`, `Produtos.CodigoEspecificadorSubstituicaoTributaria`, `Produtos.NcmNbs`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosTributacaoIbsCbs`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosTributacoesIbsCbsComCestENcm.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesIbsCbsComCestENcm.cs) — SHA-256 `f8163c4fe2435db78436cd30bb4e9531327ce572044281bba966ccc37347a116`
- [ListaProdutosTributacoesIbsCbsComCestENcm.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosTributacoesIbsCbsComCestENcm.Designer.cs) — SHA-256 `fcd1fbcb406bfcd8b5b55f623e3022f43039310554c9d83b7d93682746b169e7`

## Lista de serviços

Classe: `ListaServicos`. Identificador: `ProdutoServico.ListaServicos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Situação; Tabela de preço; Ordenação

**Campos apresentados (DTO):** `calculatedField1`, `PrecoVenda`, `PrecoCusto`, `UnidadeMedida`, `Descricao`, `CodigoInterno`, `CamposPersonalizados.Informacao`, `CamposPersonalizados.Descricao`

**Expressões calculadas do layout:**

- `Iif([Ativo]==True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaServicos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaServicos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaServicos.cs) — SHA-256 `db80f8a646ab7116ee26de5e0a89833115af5ba94d435ca1fff18de7d4195fbe`
- [ListaServicos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaServicos.Designer.cs) — SHA-256 `aeacab89f3c2237c30a882b46e80e0bb4b09aa37d3132da21e0e64a180eb631c`

## Lista de tabelas de preço

Classe: `ProdutosServicosTabelaPreco`. Identificador: `ProdutoServico.ProdutosServicosTabelaPreco`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo; Tipo item; Tabela de preço; Ordenação

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterRelatorioProdutosServicosTabelaPreco`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ProdutosServicosTabelaPreco.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ProdutosServicosTabelaPreco.cs) — SHA-256 `629828a6ca59408a305f8613a11913c0a897aecc908ad9532dadc661f577709e`
- [ProdutosServicosTabelaPreco.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ProdutosServicosTabelaPreco.Designer.cs) — SHA-256 `a9ad17b1c85d6cd3b57bed0a0525eafb2f8a323fd87af24ceb3818df6f3b2341`

## Lista de tabelas de preço com quantidade

Classe: `ProdutosServicosTabelaPrecoComQuantidade`. Identificador: `ProdutoServico.ProdutosServicosTabelaPrecoComQuantidade`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo; Tipo item; Tabela de preço; Ordenação

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterRelatorioProdutosServicosTabelaPreco`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ProdutosServicosTabelaPrecoComQuantidade.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ProdutosServicosTabelaPrecoComQuantidade.cs) — SHA-256 `a09ed536d04b210ac829a1043ebf988473c0b6c87d76823b3dced44cd05fb016`
- [ProdutosServicosTabelaPrecoComQuantidade.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ProdutosServicosTabelaPrecoComQuantidade.Designer.cs) — SHA-256 `da5206c94db4a4852b9cfa7d8213f3be225cffdbb82a02670790c5742fbe35e6`

## Lista do saldo ICMS ST retido por produto

Classe: `ListaSaldoIcmsStRetidoPorProduto`. Identificador: `ProdutoServico.ListaSaldoIcmsStRetidoPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tributação estadual; Ordenação

**Campos apresentados (DTO):** `PrecoCusto`, `ValorStRetido`, `BaseCalculoStRetido`, `Total`, `PrecoUnitario`, `Quantidade`, `Ncm`, `Descricao`, `CodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterSaldoStRetidoPorProduto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaSaldoIcmsStRetidoPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaSaldoIcmsStRetidoPorProduto.cs) — SHA-256 `ae1a079d7b575dc939589a805b274c971c4833686a5c05fec0e2401d0093de2c`
- [ListaSaldoIcmsStRetidoPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaSaldoIcmsStRetidoPorProduto.Designer.cs) — SHA-256 `c75dfee9e9bb5b6deab1186e4e3e99472ac24c90985becebbe360d071becb0cf`

## Log de alterações da composição

Classe: `LogAlteracaoComposicao`. Identificador: `ProdutoServico.LogAlteracaoComposicao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Composição

**Campos apresentados (DTO):** `DataHora`, `Quantidade`, `ComponenteDescricao`, `NomeUsuario`, `TipoAcaoLogComponenteAlterado`

**Agrupamentos do layout:**

- `Descricao`
- `DataInclusao`
- `ProdutoEmpresaReferencia`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterLogAlteracaoComposicao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [LogAlteracaoComposicao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/LogAlteracaoComposicao.cs) — SHA-256 `f3152e7fa5e0c7faaa5877b93cbcf960d264a8d1700f016b3611bc0c567f25ad`
- [LogAlteracaoComposicao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/LogAlteracaoComposicao.Designer.cs) — SHA-256 `d6980391f8e3f5476a383dfd5c93165418e4e5144e9f958a93c105a81066259f`

## Log de alterações da consulta de produtos (IMendes)

Classe: `ConsultaProdutoIMendesLog`. Identificador: `ProdutoServico.ConsultaProdutoIMendesLog`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Unidade de federação; Produto

**Campos apresentados (DTO):** `AliquotaIbsApi`, `AliquotaIbsBase`, `ReducaoIbsApi`, `ReducaoIbsBase`, `ReducaoIsBase`, `ReducaoIsApi`, `AliquotaIsBase`, `AliquotaIsApi`, `CstIbsBase`, `ClassificacaoTributariaIbsBase`, `CstIbsApi`, `ClassificacaoTributariaIbsApi`, `ReducaoCbsBase`, `ReducaoCbsApi`, `AliquotaCbsBase`, `AliquotaCbsApi`, `ClassificacaoTributariaCbsBase`, `ClassificacaoTributariaCbsApi`, `CstCbsBase`, `CstCbsApi`, `CodigoBeneficioFiscalUfApi`, `CodigoBeneficioFiscalUfBase`, `NomeUsuario`, `CodigoEnquadramentoIpiApi`, `CodigoEnquadramentoIpiBase`, `AliquotaIpiApi`, `AliquotaIpiBase`, `TributacaoEcfApi`, `TributacaoEcfBase`, `AliquotaIcmsEcfApi`, `AliquotaIcmsEcfBase`, `AliquotaMvaApi`, `AliquotaMvaBase`, `AliquotaReducaoBaseCalculoIcmsStApi`, `AliquotaReducaoBaseCalculoIcmsStBase`, `AliquotaReducaoBaseCalculoIcmsApi`, `AliquotaReducaoBaseCalculoIcmsBase`, `Uf`, `CfopApi`, `CfopBase`, `DataInclusao`, `AliquotaCofinsApi`, `AliquotaCofinsBase`, `CstCofinsBase`, `CstCofinsApi`, `AliquotaPisApi`, `AliquotaPisBase`, `CstPisBase`, `CstPisApi`, `AliquotaIcmsApi`, `AliquotaIcmsBase`, `CstIcmsApi`, `CstIcmsBase`, `CestApi`, `CestBase`, `NcmApi`, `NcmBase`, `Descricao`, `CodigoInterno`, `CodigoBarras`

**Agrupamentos do layout:**

- `Descricao`
- `DataInclusao`
- `ProdutoEmpresaReferencia`

**Contratos de serviço:**

- `IIMendesModuleService`

**Chamadas catalogadas:**

- `IIMendesModuleService.ObterConsultaProdutoLog`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [ConsultaProdutoIMendesLog.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ConsultaProdutoIMendesLog.cs) — SHA-256 `37ebf901bf8837e08c5a3b5766158c2f3f7c7306e33f34f19d26cd094c60acbc`
- [ConsultaProdutoIMendesLog.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ConsultaProdutoIMendesLog.Designer.cs) — SHA-256 `b382168e9971d4a88f3c7a07cb9b3e99b4c7880b8ee21a8a4616145959295018`

## Log de alterações da consulta de produtos (Parametriza Soft/Adv Integra)

Classe: `ConsultaProdutoLog`. Identificador: `ProdutoServico.ConsultaProdutoLog`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Produto

**Campos apresentados (DTO):** `DataInclusao`, `AliquotaCofinsApi`, `AliquotaCofinsBase`, `CstCofinsBase`, `CstCofinsApi`, `AliquotaPisApi`, `AliquotaPisBase`, `CstPisBase`, `CstPisApi`, `AliquotaIcmsApi`, `AliquotaIcmsBase`, `CstIcmsApi`, `CstIcmsBase`, `CestApi`, `CestBase`, `NcmApi`, `NcmBase`, `Descricao`, `CodigoBarras`, `CfopBase`, `CfopApi`

**Agrupamentos do layout:**

- `Descricao`
- `DataInclusao`
- `IdProdutoServicoEmpresa`

**Contratos de serviço:**

- `IParametrizaSoftModuleService`

**Chamadas catalogadas:**

- `IParametrizaSoftModuleService.ObterConsultaProdutoLog`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [ConsultaProdutoLog.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ConsultaProdutoLog.cs) — SHA-256 `16a1eb5f592e57b5e0083776c69b60cbac0d3b2e1c848d0fc3d0c04759d38c00`
- [ConsultaProdutoLog.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ConsultaProdutoLog.Designer.cs) — SHA-256 `c5144bca47f5d849eb45f0a9ee19bce23caa25a7b08aa0ce7c574ccf13e8363e`

## Lucro movimentação por produto

Classe: `LucroMovimentacaoProduto`. Identificador: `ProdutoServico.LucroMovimentacaoProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `CodigoDescricao`, `CustoTotal`, `EstoqueFinal`, `Lucro`, `PrecoCustoMedio`, `PrecoVendaMedio`, `QuantidadeEntrada`, `QuantidadeSaida`, `VendaTotal`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterLucroMovimentacaoPorProduto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [LucroMovimentacaoProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/LucroMovimentacaoProduto.cs) — SHA-256 `4937d1d0d5faabfdc5c5b5ab4f899e7e28254f8f8da7463a6e26965cccbc81b1`
- [LucroMovimentacaoProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/LucroMovimentacaoProduto.Designer.cs) — SHA-256 `de09b522cfbb0c0daeeafbb6efeedcfb8c5b0ff13074f9b1db7f5f5dcd047ba7`

## Medicamentos controlados

Classe: `MedicamentosControlados`. Identificador: `ProdutoServico.MedicamentosControlados`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Classe terapêutica; Ordenação; Mostrar produtos

**Campos apresentados (DTO):** `DataFabricacao`, `Quantidade`, `DataValidade`, `numeroLote`, `PrecoVenda`, `RegistroMinisterioSaude`, `Descricao`, `CodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterMedicamentosControlados`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MedicamentosControlados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/MedicamentosControlados.cs) — SHA-256 `4a5a6d104f860d4b435b4ad3a97686aa49784666f2c067b1477a15c0173683c7`
- [MedicamentosControlados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/MedicamentosControlados.Designer.cs) — SHA-256 `251a54d850f7ce59a13b5304458fcea8bff019a6bab09e22005bfc48ce248e2a`

## Montagens e desmontagens por composição

Classe: `MontagemDesmontagemComposicao`. Identificador: `ProdutoServico.MontagemDesmontagemComposicao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Composição; Data inicial; Data final

**Campos apresentados (DTO):** `Montagens.Quantidade`, `Montagens.Data`, `Desmontagens.Data`, `Desmontagens.Quantidade`

**Contratos de serviço:**

- `IComposicaoModuleService`

**Chamadas catalogadas:**

- `IComposicaoModuleService.ObterMontagensDesmontagens`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-produto-servico.md](../../docs/continuacao-produto-servico.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Company scoping differs between the traced methods
- External product consultation services not called

**Fontes da ficha:**

- [MontagemDesmontagemComposicao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/MontagemDesmontagemComposicao.cs) — SHA-256 `dc891ac37b0abe74a46ebf1098800ea2ac4e54c122cfa8301281c09a7ed60687`
- [MontagemDesmontagemComposicao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/MontagemDesmontagemComposicao.Designer.cs) — SHA-256 `4e072094f017161a9d44122b559980ba80085754dd48ac80ad0ac876ad91da12`

## Movimentações por produto ou serviço

Classe: `MovimentacaoProdutoServico`. Identificador: `ProdutoServico.MovimentacaoProdutoServico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Produto ou serviço; Data inicial; Data final; Tipo de movimento

**Campos apresentados (DTO):** `DataCadastroProduto`, `Subgrupo`, `QuantidadeEmEstoque`, `Unidade`, `Descricao`, `CodigoInterno`, `Itens.Saldo`, `Itens.Cfop`, `Itens.DataEfetivacao`, `Itens.DataEntradaSaida`, `Itens.TotalLiquido`, `Itens.Total`, `Itens.Desconto`, `Itens.Preco`, `Itens.Quantidade`, `Itens.Pessoa`, `Itens.DataEmissao`, `Itens.NumeroDocumento`, `Itens.TipoMovimento`, `Grades.Posicao`, `Grades.Linha`, `Grades.Coluna`, `Grades.CodigoBarras`, `Grades.Quantidade`, `Grades.Total`, `TotalEntrada`, `TotalSaida`, `Diferenca`, `Saldo`, `QuantidadeInicial`, `QuantidadeSaida`, `QuantidadeEntrada`

**Expressões calculadas do layout:**

- `([QuantidadeInicial] + [QuantidadeEntrada]) - [QuantidadeSaida]`
- `[Saldo] - [QuantidadeEmEstoque]`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterMovimentacoesProdutoServico`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MovimentacaoProdutoServico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/MovimentacaoProdutoServico.cs) — SHA-256 `fcfce31d68eefef4f82414f596baed4b4ced43a66820084607d67b45860b41e8`
- [MovimentacaoProdutoServico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/MovimentacaoProdutoServico.Designer.cs) — SHA-256 `f0a1215809de78a91cd46377168b2e996a0634027ef28439eec379d121d49cff`

## NCM inválido

Classe: `ListaProdutosNcmInvalido`. Identificador: `ProdutoServico.ListaProdutosNcmInvalido`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Situação; Ordenação

**Campos apresentados (DTO):** `calculatedField1`, `CodigoInterno`, `CodigoBarras`, `Descricao`, `Quantidade`, `Ativo`, `CodigoNcm`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif([Ativo]==True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosNcmInvalido`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosNcmInvalido.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosNcmInvalido.cs) — SHA-256 `0c0dc4a082f05652f41fdcfd5bf6fc85c5cb2a096075240339abf1cd2e2a541f`
- [ListaProdutosNcmInvalido.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosNcmInvalido.Designer.cs) — SHA-256 `d2ec69803b97cf59400e3c9290cc5ad92dcab222c3b97d08347c0c3f44d28b7f`

## Preços alterados no período

Classe: `ListaProdutosAlteracaoPreco`. Identificador: `ProdutoServico.ListaProdutosAlteracaoPreco`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final; Ordenação

**Campos apresentados (DTO):** `CodigoInterno`, `CodigoBarras`, `Descricao`, `ItensPreco.PrecoVenda`, `ItensPreco.NomeUsuario`, `ItensPreco.Data`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterHistoricoAlteracaoPrecos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosAlteracaoPreco.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosAlteracaoPreco.cs) — SHA-256 `a4880a4bc98c48e01d413fc95acc142089a24dc8f96bf4d0e7503ba6967e5e47`
- [ListaProdutosAlteracaoPreco.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosAlteracaoPreco.Designer.cs) — SHA-256 `28397b17d636663343cdbddf59a06413d150570bec03bff911d75dac3c76bc25`

## Produto curva ABC

Classe: `ProdutoCurvaAbc`. Identificador: `ProdutoServico.ProdutoCurvaAbc`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; SubGrupo

**Campos apresentados (DTO):** `Subgrupo`, `CodigoInterno`, `Descricao`, `QuantidadeVendida`, `TotalVenda`, `TotalCusto`, `TotalLucro`, `PercentualLucro`, `PercentualAcumulado`, `ABC`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutosCurvaAbc`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ProdutoCurvaAbc.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ProdutoCurvaAbc.cs) — SHA-256 `b820cf2346044c7bdecdeb9c8a6f436085db7ebd222fdae7c716f2464306da18`
- [ProdutoCurvaAbc.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ProdutoCurvaAbc.Designer.cs) — SHA-256 `5adb36af6db1b4f9c3d4e7c255aa3130636629772aba4d3c18f181b38349584d`

## Produtos com controle de lote

Classe: `ListaProdutosLotes`. Identificador: `ProdutoServico.ListaProdutosLotes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos

**Campos apresentados (DTO):** `Descricao`, `CodigoInterno`, `PrecoVenda`, `Lotes.Quantidade`, `Lotes.DataFabricacao`, `Lotes.NumeroLote`, `Lotes.DataValidade`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutosComControleLote`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosLotes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLotes.cs) — SHA-256 `c4255f2511e625ccfb33a665b88f8656025ff898b4fa3496a02dfd9fb564dfa0`
- [ListaProdutosLotes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLotes.Designer.cs) — SHA-256 `d2c1c42cad6253a827f57ea2305c7995033834275c70b20f2f1d74b0611ecf13`

## Produtos com seriais em estoque

Classe: `ListaProdutosSeriais`. Identificador: `ProdutoServico.ListaProdutosSeriais`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto ou serviço

**Campos apresentados (DTO):** `Quantidade`, `SeriaisToText`, `Descricao`, `CodigoInterno`, `PrecoVenda`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutosComControleSerial`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosSeriais.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosSeriais.cs) — SHA-256 `5f7c7312f57d7b433a9cb8d3a54441942e0b7156a91a9013573ca6dfb9352581`
- [ListaProdutosSeriais.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosSeriais.Designer.cs) — SHA-256 `ecaf1aa99288485e9ff34012178fa85d961c3492dd876707c7882a3a331a0800`

## Produtos e serviços com condição especial

Classe: `ListaProdutoServicosCondicaoEspecial`. Identificador: `ProdutoServico.ListaProdutoServicosCondicaoEspecial`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ordenação

**Campos apresentados (DTO):** `CodigoInterno`, `Descricao`, `PrecoVenda`, `Condicao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutoComCondicaoEspecial`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutoServicosCondicaoEspecial.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutoServicosCondicaoEspecial.cs) — SHA-256 `9b31f462fc5419cecff4cc34fb20cb276c27b901175fd93f976f38bced5c4227`
- [ListaProdutoServicosCondicaoEspecial.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutoServicosCondicaoEspecial.Designer.cs) — SHA-256 `6ac3e36cc3acbdecc1439813c0397801b0a3f07cd6f94b4a2393f4339969ff34`

## Produtos vencidos ou a vencer

Classe: `ListaProdutosLotesValidade`. Identificador: `ProdutoServico.ListaProdutosLotesValidade`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Dias que faltam para expirar a data de validade; Exibir somente itens com quantidade em estoque; Exibir lotes vencidos

**Campos apresentados (DTO):** `Descricao`, `CodigoInterno`, `PrecoVenda`, `Lotes.Quantidade`, `Lotes.DataFabricacao`, `Lotes.NumeroLote`, `Lotes.DataValidade`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutosComValidadeEmXDias`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosLotesValidade.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLotesValidade.cs) — SHA-256 `77670966939a4ef42fd1177b719e86c807809fb1e5e45efe5574b2216d97e521`
- [ListaProdutosLotesValidade.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ListaProdutosLotesValidade.Designer.cs) — SHA-256 `c367037443a980724606bfaa3e94cfead47792edb41fab16ebcc727c73d12c47`

## Relação de produtos por fornecedor

Classe: `ProdutosPorFornecedor`. Identificador: `ProdutoServico.ProdutosPorFornecedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Fornecedor; Mostrar produtos; Ordenação

**Campos apresentados (DTO):** `QuantidadeSaldo`, `PrecoCustoMedio`, `PrecoCusto`, `PrecoVenda`, `ProdutoEmpresa`, `CodigoBarras`, `CodigoFornecedor`, `CodigoInterno`, `telefone`, `FornecedorEndereco`, `email`, `FornecedorCnpj`, `Fornecedor`

**Agrupamentos do layout:**

- `Fornecedor`
- `FornecedorCnpj`
- `FornecedorEndereco`
- `telefone`
- `email`

**Expressões calculadas do layout:**

- `Iif([FornecedorTelefonePrincipal] == '', '', Concat('Telefone: ', [FornecedorTelefonePrincipal]))`
- `Iif([FornecedorEmailPrincipal] == '', '', Concat('E-mail: ', [FornecedorEmailPrincipal]))`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutosPorFornecedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ProdutosPorFornecedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/ProdutosPorFornecedor.cs) — SHA-256 `b0d31804dab93b209bb6802da6a354a6d5d0b6c0a1aa923fc75cef7b25285afb`
- [ProdutosPorFornecedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/ProdutosPorFornecedor.Designer.cs) — SHA-256 `47ef489fd3c971fb8cbf5c0c01319472c6fb068e760ae52edd52b5f5fcc2880a`

## Rotatividade

Classe: `Rotatividade`. Identificador: `ProdutoServico.Rotatividade`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Marca; SubGrupo; Fornecedor; Ordenação; Situação; Mostrar total de entrada de estoque

**Campos apresentados (DTO):** `Subgrupo`, `Marca`, `CodigoInterno`, `Saldo`, `TotalSaida`, `TotalEntrada`, `QuantidadeSaldo`, `QuantidadeSaida`, `QuantidadeEntrada`, `QuantidadeInicial`, `Descricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterRotatividadeProdutos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [Rotatividade.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ProdutoServico/Rotatividade.cs) — SHA-256 `b3ff2796830e5059850ed4667b59d8c5e907fa390aa5a64a3fc90e803be7c9b9`
- [Rotatividade.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ProdutoServico/Rotatividade.Designer.cs) — SHA-256 `8b556950d7dabfb6fa26993292a9a5d43f5dd3318f0c4daa3882e31987e0c585`

