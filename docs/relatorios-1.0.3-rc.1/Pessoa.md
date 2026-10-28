# Pessoa

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Aniversariantes

Classe: `Aniversariantes`. Identificador: `Pessoa.Aniversariantes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `NomeProprietario`, `Telefone`, `DataNascimento`, `Idade`, `Nome`

**Expressões calculadas do layout:**

- `DateDiffYear([DataNascimento],LocalDateTimeNow())`

**Contratos de serviço:**

- `IAnimalModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterAniversariantes`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [Aniversariantes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Animal/Aniversariantes.cs) — SHA-256 `f0f0aba64f99332ea48dc1401e6a714b74df8a7ccba7a5cb1ca231213cbac66f`
- [Aniversariantes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Animal/Aniversariantes.Designer.cs) — SHA-256 `5aedcad8d5f64c8923cd561625d442c0218606437f30749678df80a6b12db04c`

## Clientes com cashback disponível

Classe: `ClientesCashbackDisponivel`. Identificador: `Pessoa.ClientesCashbackDisponivel`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ordenação

**Campos apresentados (DTO):** `Valor`, `PessoaNome`, `PessoaTelefone`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterItensCreditoDebitoCashback`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesCashbackDisponivel.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesCashbackDisponivel.cs) — SHA-256 `ca7466df78a81ef95064893dcb91823eeb2b48a6b1add8ec24df78bb49dea6c3`
- [ClientesCashbackDisponivel.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesCashbackDisponivel.Designer.cs) — SHA-256 `be078dad1f3e41ae7e265091de7b16abdb19262762db6457ccff13f2c31ee2ec`

## Clientes com crédito disponível em serviços

Classe: `ClientesCreditoServico`. Identificador: `Pessoa.ClientesCreditoServico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `PessoaNome`, `Creditos.Servico.ProdutoServico.Descricao`, `Creditos.QuantidadeCredito`, `Telefone`, `PessoaDocumento`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPessoasComCreditoServicos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesCreditoServico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesCreditoServico.cs) — SHA-256 `07607cb7c5d07bcb516732f0e63bb2eacb1eec0ebabe2a306adf0b77adfa0ea1`
- [ClientesCreditoServico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesCreditoServico.Designer.cs) — SHA-256 `77f4e01f8a3f736170c9113026e14815ee3f4e1ac036dab2222afbac95ec03b7`

## Clientes inadimplentes

Classe: `ClientesInadimplentes`. Identificador: `Pessoa.ClientesInadimplentes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ordenação

**Campos apresentados (DTO):** `Celular`, `Telefone`, `Email`, `participacao`, `ValorAtrasado`, `UltimaVenda`, `Nome`

**Expressões calculadas do layout:**

- `([ValorAtrasado] * 100)/[].Sum([ValorAtrasado])`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterClientesInadimplentes`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesInadimplentes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesInadimplentes.cs) — SHA-256 `916ab06a66209805853badfce35350c87613baf5feecdf5c8ab8003df875e6ee`
- [ClientesInadimplentes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesInadimplentes.Designer.cs) — SHA-256 `6c47b2fa8d4d3a5af88c7fa0410c52f55181781a03cc6077f83a5b5c56070cc7`

## Clientes por prescritor

Classe: `ClientesPorPrescritor`. Identificador: `Pessoa.ClientesPorPrescritor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data de emissão inicial; Data de emissão final; Prescritor

**Campos apresentados (DTO):** `PessoaNome`, `DataEmissao`, `PrescritorNome`

**Agrupamentos do layout:**

- `PrescritorNome`

**Expressões calculadas do layout:**

- `[Pessoa.Celulares.Numero]`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPrescricoesOticas`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesPorPrescritor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesPorPrescritor.cs) — SHA-256 `2fa848b3cedd2281863d4f0c3e7d8c05759ed857af91aa5614176eb382dd842c`
- [ClientesPorPrescritor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesPorPrescritor.Designer.cs) — SHA-256 `6ed6bcf17db585006ee954b66e724a03746ae331ca806d257a96bb6e6535404d`

## Clientes que deixaram de comprar

Classe: `ClientesQueDeixaramDeComprar`. Identificador: `Pessoa.ClientesQueDeixaramDeComprar`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Mínimo de dias desde a última compra; Ordenação

**Campos apresentados (DTO):** `CpfCnpj`, `Bairro`, `Municipio`, `Telefone`, `Nome`, `ultimaVenda`, `tituloDocumento`

**Expressões calculadas do layout:**

- `Substring([TituloCpfCnpj], 0, Len([TituloCpfCnpj])-1)`
- `Iif([DataUltimaVenda] == '01/01/0001 00:00:00', '-', [DataUltimaVenda])`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ClientesQueDeixaramDeComprar`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesQueDeixaramDeComprar.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesQueDeixaramDeComprar.cs) — SHA-256 `82b1cdeaddfce2f2df1f9532f8b5a0b0ecb23bba2c828808cad843aa70bd728e`
- [ClientesQueDeixaramDeComprar.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesQueDeixaramDeComprar.Designer.cs) — SHA-256 `57bb8236d5488d21f07dedea297562fcdf497ff58b48225813e51325cba9d43c`

## Clientes que não compraram no período

Classe: `ClientesNaoComprarPorPeriodo`. Identificador: `Pessoa.ClientesNaoComprarPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Data inicial; Data final; UF destino; Município; Bairro; Ordenação; Mostrar clientes que nunca compraram

**Campos apresentados (DTO):** `CpfCnpj`, `Bairro`, `Municipio`, `Telefone`, `Nome`, `ultimaVenda`, `tituloDocumento`, `Uf`

**Expressões calculadas do layout:**

- `Substring([TituloCpfCnpj], 0, Len([TituloCpfCnpj])-1)`
- `Iif([DataUltimaVenda] == '01/01/0001 00:00:00', '-', [DataUltimaVenda])`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ClientesQueDeixaramDeComprarPeriodo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesNaoComprarPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesNaoComprarPorPeriodo.cs) — SHA-256 `fbf4ec0694a65728138709348321291ea476064e546f96f7df67a4a889f801f6`
- [ClientesNaoComprarPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesNaoComprarPorPeriodo.Designer.cs) — SHA-256 `d27ee5d2142518a249d88f3f9c0e3c748784a167fd3cd8c7974018f221076dda`

## Clientes que possuem crédito

Classe: `ClientesCreditoDisponivel`. Identificador: `Pessoa.ClientesCreditoDisponivel`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar valores; Tipo de segmentação; Ordenação

**Campos apresentados (DTO):** `Valor`, `PessoaNome`, `PessoaTelefone`, `DescricaoTipoSegmentacao`

**Agrupamentos do layout:**

- `DescricaoTipoSegmentacao`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterItensCreditoDebito`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesCreditoDisponivel.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesCreditoDisponivel.cs) — SHA-256 `b6658b88c18e0153aa442487c4595a102abc123ef384635a9e08980b9221cf13`
- [ClientesCreditoDisponivel.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesCreditoDisponivel.Designer.cs) — SHA-256 `d9cde919499270ff23f407a30bcd8b5761a4f4411b365b34eb1b9b315137c06c`

## Comissões por colaborador

Classe: `ComissoesPorColaborador`. Identificador: `Pessoa.ComissoesPorColaborador`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pessoa; Situação; Data/Hora inicial; Data/Hora final; Filtrar por data de recebimento da parcela; Ocultar total (Base cálculo - valor comissões); Ocultar detalhamento

**Campos apresentados (DTO):** `Colaborador.Nome`, `regiaoAtuacao`, `percentualComissao`, `tipoComissao`, `Comissoes.Valor`, `Comissoes.BaseCalculo`, `Comissoes.DataInclusao`, `Comissoes.dataRecebimentoField`, `Comissoes.documentoOrigem`, `Comissoes.Situacao`, `Comissoes.Percentual`

**Expressões calculadas do layout:**

- `Concat([TipoComissao.Descricao],': ' ,'[DocumentoReferencia]','-','[Pessoa.Nome]')`
- `Iif([Colaborador.Fisica.Vendedor.IsVendedor], [Colaborador.Fisica.Vendedor.RegiaoAtuacao], [Colaborador.Fisica.Tecnico.RegiaoAtuacao])`
- `Iif([Colaborador.Fisica.Vendedor.IsVendedor], [Colaborador.Fisica.Vendedor.PercentualComissao], [Colaborador.Fisica.Tecnico.PercentualComissao])`
- `Iif([Colaborador.Fisica.Vendedor.IsVendedor], [Colaborador.Fisica.Vendedor.Comissao.Descricao], [Colaborador.Fisica.Tecnico.Comissao.Descricao])`
- `Iif([DataRecebimento]== '01/01/0001 00:00:00', '', [DataRecebimento])`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterComissaoPorPeriodoColaboradorESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ComissoesPorColaborador.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ComissoesPorColaborador.cs) — SHA-256 `3c964e847e190ce7916ac19b472131e494b6636a0ba9d13b821fbc6059ddbbad`
- [ComissoesPorColaborador.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ComissoesPorColaborador.Designer.cs) — SHA-256 `10da4096022513c2d8c8a6bfcd6a300f1d4e9774cd17d4ffad7fdff66e901d3b`

## Curva ABC de clientes

Classe: `ClientesCurvaAbc`. Identificador: `Pessoa.ClientesCurvaAbc`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Acumulado`, `ABC`, `PercentualAcumulado`, `Total`, `UltimaVenda`, `Telefone`, `Uf`, `Cidade`, `Nome`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterCurvaAbcClientes`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesCurvaAbc.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesCurvaAbc.cs) — SHA-256 `4e8d06cc78a453efe47d00e813fbe996288a96a00bfc1918e1db81aebba64ca8`
- [ClientesCurvaAbc.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesCurvaAbc.Designer.cs) — SHA-256 `426e09af2a72919eeb3c4407a3a7da2a8cb8db8e8d6a240cf1894e20d236c2da`

## Curva ABC de clientes por origem de movimentação

Classe: `ClientesCurvaAbcPorTipoMovimentacao`. Identificador: `Pessoa.ClientesCurvaAbcPorTipoMovimentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Origem das movimentações; Data inicial; Data final

**Campos apresentados (DTO):** `Acumulado`, `ABC`, `PercentualAcumulado`, `Total`, `UltimaVenda`, `Telefone`, `Uf`, `Cidade`, `Nome`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterCurvaAbcClientesPorTipoTributacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesCurvaAbcPorTipoMovimentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesCurvaAbcPorTipoMovimentacao.cs) — SHA-256 `4538e9163c3285ff936815cc691487bfd74f9e3e77fd79eedd18e21a4802bb7e`
- [ClientesCurvaAbcPorTipoMovimentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesCurvaAbcPorTipoMovimentacao.Designer.cs) — SHA-256 `b26528a8c33cd3b9482f2da4b9b7d9e687593fdd20ba3293a558695c854b5663`

## Curva ABC de clientes por produto

Classe: `ClientesCurvaAbcPorProduto`. Identificador: `Pessoa.ClientesCurvaAbcPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Produto ou serviço; Marca; Subgrupo; Origem das movimentações; Ordenar por

**Campos apresentados (DTO):** `Nome`, `Telefone`, `Email`, `UltimaCompra`, `Quantidade`, `Total`, `Compras`, `PercentualAcumulado`, `ABC`, `Documento`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterCurvaAbcClientesPorProduto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ClientesCurvaAbcPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ClientesCurvaAbcPorProduto.cs) — SHA-256 `197f0c2c56a2d1a89e2b24f06ff6b46ad890249e5e66aeb32e5ad3c1c753184e`
- [ClientesCurvaAbcPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ClientesCurvaAbcPorProduto.Designer.cs) — SHA-256 `704cf700d04ac93e1f4a0739ca6f439b8b10125e98c2aaaeff32759509af7f6d`

## FichaCadastral

Classe: `FichaCadastral`. Identificador: `Pessoa.FichaCadastral`.

**Execução no Insight:** Classe não localizada nos assemblies da instalação. Sem comportamento nativo para reconstruir ou executar nesta versão.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Chamadas catalogadas:**

- `Não identificada na classe de entrada`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- Report class absent from the installed assemblies
- Entry point, service, DTO and bindings unknown
- No runtime observation

**Fontes da ficha:**

- Classe não localizada nas fontes retidas.

## FichaCadastral40Colunas

Classe: `FichaCadastral40Colunas`. Identificador: `Pessoa.FichaCadastral40Colunas`.

**Execução no Insight:** Classe não localizada nos assemblies da instalação. Sem comportamento nativo para reconstruir ou executar nesta versão.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Chamadas catalogadas:**

- `Não identificada na classe de entrada`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- Report class absent from the installed assemblies
- Entry point, service, DTO and bindings unknown
- No runtime observation

**Fontes da ficha:**

- Classe não localizada nas fontes retidas.

## Histórico por pessoa

Classe: `HistoricoPessoa`. Identificador: `Pessoa.HistoricoPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa; Gerar movimentações; Origem; Data de emissão inicial das movimentações; Data de emissão final das movimentações; Mostrar itens das movimentações (se houver); Gerar recebimentos pendentes; Gerar recebimentos quitados; Gerar pagamentos pendentes; Gerar pagamentos quitados

**Campos apresentados (DTO):** `Ie`, `Endereco.Municipio.UF.Sigla`, `Endereco.Bairro`, `Celular.Numero`, `Rg`, `Endereco.Municipio.Nome`, `Endereco.Numero`, `Telefone.Numero`, `CpfCnpj`, `nascimento`, `Endereco.Complemento`, `Endereco.Logradouro`, `Email`, `Nome`, `TituloCpfCnpj`, `Movimentacoes.Desconto`, `Movimentacoes.Total`, `Movimentacoes.ValorItens`, `Movimentacoes.DataEmissao`, `Movimentacoes.Serie`, `Movimentacoes.Modelo`, `Movimentacoes.Numero`, `Movimentacoes.Situacao.Descricao`, `Movimentacoes.Origem`, `Movimentacoes.Itens.Desconto`, `Movimentacoes.Itens.Codigo`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.ValorUnitario`, `Movimentacoes.Itens.Total`, `Movimentacoes.Itens.Cancelado`, `Movimentacoes.Objetos.DataGarantia`, `Movimentacoes.Objetos.Objeto.Descricao`, `Movimentacoes.Objetos.Objeto.ToTextPlacaSerial`, `Movimentacoes.Objetos.Defeito`, `Movimentacoes.Objetos.Diagnostico`, `Movimentacoes.Objetos.Objeto.Marca`, `Movimentacoes.Objetos.Objeto.Modelo`, `Movimentacoes.Objetos.Objeto.Renavam`, `Movimentacoes.Objetos.Objeto.Cor`, `Movimentacoes.Objetos.Objeto.AnoFabricacao`, `Movimentacoes.Objetos.Objeto.Chassi`, `RecebimentosQuitados.ValorTotal`, `RecebimentosQuitados.DataVencimento`, `RecebimentosQuitados.Documento`, `RecebimentosQuitados.Ordem`, `RecebimentosQuitados.Descricao`, `RecebimentosQuitados.DataInclusao`, `RecebimentosQuitados.DataQuitacao`, `RecebimentosQuitados.ValorQuitacao`, `RecebimentosPendentes.DiasAtraso`, `RecebimentosPendentes.Documento`, `RecebimentosPendentes.Ordem`, `RecebimentosPendentes.Descricao`, `RecebimentosPendentes.DataInclusao`, `RecebimentosPendentes.DataVencimento`, `RecebimentosPendentes.ValorTotal`, `PagamentosQuitados.ValorTotal`, `PagamentosQuitados.Documento`, `PagamentosQuitados.Ordem`, `PagamentosQuitados.Descricao`, `PagamentosQuitados.DataEmissao`, `PagamentosQuitados.DataVencimento`, `PagamentosQuitados.DataQuitacao`, `PagamentosQuitados.ValorQuitacao`, `PagamentosPendentes.DiasAtraso`, `PagamentosPendentes.Documento`, `PagamentosPendentes.Ordem`, `PagamentosPendentes.Descricao`, `PagamentosPendentes.DataEmissao`, `PagamentosPendentes.DataVencimento`, `PagamentosPendentes.ValorTotal`, `Movimentacoes.Objetos.Objeto.Cilindrada`, `Movimentacoes.Objetos.Objeto.Potencia`, `Movimentacoes.Objetos.Objeto.Motor`, `Movimentacoes.Objetos.Objeto.Segmento`

**Expressões calculadas do layout:**

- `Iif([DataNascimento] == '01/01/0001 00:00:00', '', [DataNascimento])`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterHistoricoPessoa`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [HistoricoPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/HistoricoPessoa.cs) — SHA-256 `50152e8b003ae95166c7471388dde9e5d9a7fa2d0f55a73434c91f4743bce962`
- [HistoricoPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/HistoricoPessoa.Designer.cs) — SHA-256 `5ada74e83f11486fd3bc10a3905d56453799f185ffb8c712f4ffb8ec96aa0255`

## Histórico por pessoa - 40 colunas

Classe: `HistoricoPessoa40Colunas`. Identificador: `Pessoa.HistoricoPessoa40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa; Gerar movimentações; Origem; Data de emissão inicial das movimentações; Data de emissão final das movimentações; Mostrar itens das movimentações (se houver); Gerar recebimentos pendentes; Gerar recebimentos quitados; Gerar pagamentos pendentes; Gerar pagamentos quitados

**Campos apresentados (DTO):** `Endereco.Logradouro`, `Ie`, `Endereco.Municipio.UF.Sigla`, `Endereco.Bairro`, `Celular.Numero`, `Rg`, `Endereco.Municipio.Nome`, `Endereco.Numero`, `Telefone.Numero`, `CpfCnpj`, `nascimento`, `Endereco.Complemento`, `Email`, `Nome`, `TituloCpfCnpj`, `Movimentacoes.Desconto`, `Movimentacoes.Total`, `Movimentacoes.ValorItens`, `Movimentacoes.DataEmissao`, `Movimentacoes.Modelo`, `Movimentacoes.Numero`, `Movimentacoes.Situacao.Descricao`, `Movimentacoes.Origem`, `Movimentacoes.Itens.Desconto`, `Movimentacoes.Itens.Codigo`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.ValorUnitario`, `Movimentacoes.Itens.Total`, `Movimentacoes.Itens.Cancelado`, `Movimentacoes.Objetos.DataGarantia`, `Movimentacoes.Objetos.Objeto.Descricao`, `Movimentacoes.Objetos.Objeto.ToTextPlacaSerial`, `Movimentacoes.Objetos.Defeito`, `Movimentacoes.Objetos.Diagnostico`, `Movimentacoes.Objetos.Objeto.Marca`, `Movimentacoes.Objetos.Objeto.Modelo`, `Movimentacoes.Objetos.Objeto.Renavam`, `Movimentacoes.Objetos.Objeto.Cor`, `Movimentacoes.Objetos.Objeto.AnoFabricacao`, `Movimentacoes.Objetos.Objeto.Chassi`, `RecebimentosQuitados.DataVencimento`, `RecebimentosQuitados.Documento`, `RecebimentosQuitados.Ordem`, `RecebimentosQuitados.DataInclusao`, `RecebimentosQuitados.DataQuitacao`, `RecebimentosQuitados.ValorQuitacao`, `RecebimentosPendentes.DiasAtraso`, `RecebimentosPendentes.Documento`, `RecebimentosPendentes.Ordem`, `RecebimentosPendentes.DataInclusao`, `RecebimentosPendentes.DataVencimento`, `RecebimentosPendentes.ValorTotal`, `PagamentosQuitados.Documento`, `PagamentosQuitados.Ordem`, `PagamentosQuitados.DataEmissao`, `PagamentosQuitados.DataVencimento`, `PagamentosQuitados.DataQuitacao`, `PagamentosQuitados.ValorQuitacao`, `PagamentosPendentes.DiasAtraso`, `PagamentosPendentes.Documento`, `PagamentosPendentes.Ordem`, `PagamentosPendentes.DataEmissao`, `PagamentosPendentes.DataVencimento`, `PagamentosPendentes.ValorTotal`, `Movimentacoes.Objetos.Objeto.Cilindrada`, `Movimentacoes.Objetos.Objeto.Potencia`, `Movimentacoes.Objetos.Objeto.Motor`, `Movimentacoes.Objetos.Objeto.Segmento`

**Expressões calculadas do layout:**

- `Iif([DataNascimento] == '01/01/0001 00:00:00', '', [DataNascimento])`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterHistoricoPessoa`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [HistoricoPessoa40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/HistoricoPessoa40Colunas.cs) — SHA-256 `9b081e1c8053b89e4315851ff4a3e9aaef3084757cd5865542bf08ac78e15ed7`
- [HistoricoPessoa40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/HistoricoPessoa40Colunas.Designer.cs) — SHA-256 `c4a14cb28a5b04b29b027a9e460c567f8bb218c271b5795dc0ccc2c426579dcb`

## Informações óticas por pessoa

Classe: `InformacoesOticasPorPessoa`. Identificador: `Pessoa.InformacoesOticasPorPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa

**Campos apresentados (DTO):** `DescricaoLenteDireita`, `InformacaoOticaPertoEsquerdo.Esferico`, `InformacaoOticaPertoEsquerdo.Cilindrico`, `InformacaoOticaPertoEsquerdo.Eixo`, `InformacaoOticaPertoEsquerdo.Dnp`, `InformacaoOticaPertoEsquerdo.Adicao`, `InformacaoOticaPertoEsquerdo.Altura`, `InformacaoOticaLongeEsquerdo.Esferico`, `InformacaoOticaLongeEsquerdo.Cilindrico`, `InformacaoOticaLongeEsquerdo.Eixo`, `InformacaoOticaLongeEsquerdo.Dnp`, `InformacaoOticaLongeEsquerdo.Adicao`, `InformacaoOticaLongeEsquerdo.Altura`, `PrescritorNome`, `FormatoAro.Descricao`, `MaterialAro.Descricao`, `TiposTratamento`, `ObservacaoPerto`, `ObservacaoLonge`, `Armacao.Observacao`, `Laboratorio`, `DataEmissao`, `Armacao.DistanciaVertice`, `Armacao.Diagonal`, `Armacao.Horizontal`, `Armacao.Ponte`, `Armacao.Vertical`, `InformacaoOticaLongeDireito.Altura`, `InformacaoOticaPertoDireito.Altura`, `InformacaoOticaLongeDireito.Adicao`, `InformacaoOticaPertoDireito.Adicao`, `InformacaoOticaLongeDireito.Dnp`, `InformacaoOticaPertoDireito.Dnp`, `InformacaoOticaLongeDireito.Eixo`, `InformacaoOticaPertoDireito.Eixo`, `InformacaoOticaLongeDireito.Cilindrico`, `InformacaoOticaPertoDireito.Cilindrico`, `InformacaoOticaLongeDireito.Esferico`, `InformacaoOticaPertoDireito.Esferico`, `ArmacaoDescricao`, `DescricaoLenteEsquerda`

**Expressões calculadas do layout:**

- `[Pessoa.Celulares.Numero]`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterInformacoesOticas`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [InformacoesOticasPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/InformacoesOticasPorPessoa.cs) — SHA-256 `2423fb40a9e744937f5b10e479aae1a7734d5ecb79b2dd2b82975f81f5e958d0`
- [InformacoesOticasPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/InformacoesOticasPorPessoa.Designer.cs) — SHA-256 `d49ba85a40c2fc4287d6bfb45c185f3913764bc7cf5594040a67a59aa0fb51ab`

## Objetos por pessoa

Classe: `ObjetosPorPessoa`. Identificador: `Pessoa.ObjetosPorPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa

**Campos apresentados (DTO):** `Modelo`, `ToTextPlacaSerial`, `Marca`, `Descricao`, `TipoCombustivel.Descricao`, `Renavam`, `Chassi`, `Cor`, `AnoFabricacao`, `PessoaNome`, `Cilindrada`, `Potencia`, `Motor`, `Segmento`

**Agrupamentos do layout:**

- `PessoaReferencia`

**Expressões calculadas do layout:**

- `[Pessoa.Celulares.Numero]`

**Contratos de serviço:**

- `IObjetoModuleService`

**Chamadas catalogadas:**

- `IObjetoModuleService.ObterObjetosPorPessoa`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ObjetosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ObjetosPorPessoa.cs) — SHA-256 `76b6fd5c94b65fcc52a57a9cce95c665c32642d6a9e4d369d6a3f53e1a719ca8`
- [ObjetosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ObjetosPorPessoa.Designer.cs) — SHA-256 `9b21ca7ad6d093ae8a0428d99ab609886aaeb02b6e5c37769ba631cb893e69a2`

## Perfil do usuário

Classe: `PerfilUsuario`. Identificador: `Pessoa.PerfilUsuario`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Usuário

**Campos apresentados (DTO):** `Perfis.Perfil.Descricao`, `Perfis.Perfil.Restricoes.Modulo.Descricao`, `Perfis.Perfil.Restricoes.Direitos.Descricao`

**Expressões calculadas do layout:**

- `Iif([DataNascimento] == '01/01/0001 00:00:00', '', [DataNascimento])`

**Contratos de serviço:**

- `IUsuarioModuleService`

**Chamadas catalogadas:**

- `IUsuarioModuleService.ObterUsuarioPeloNome`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PerfilUsuario.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PerfilUsuario.cs) — SHA-256 `d61aa6ae5ba3e4be6ed12f0ba075598b4abf419d4169b0a96d290b0f29d9b2e1`
- [PerfilUsuario.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PerfilUsuario.Designer.cs) — SHA-256 `8e57a6abc4b749766fb98b96afb8c79b33c71076fd6c8f86c1b614ee75af033f`

## Pessoas por bairro

Classe: `PessoasPorBairro`. Identificador: `Pessoa.PessoasPorBairro`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Registros Ativos/Inativos; Tipo de cadastro; Bairro

**Campos apresentados (DTO):** `Rg`, `Telefone`, `Celular`, `Uf`, `Complemento`, `Municipio`, `Bairro`, `Numero`, `Endereco`, `Email`, `Nome`, `Cep`, `Ie`, `CpfCnpj`, `TituloCpfCnpj`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPessoasPorBairroETipoCadastro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PessoasPorBairro.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PessoasPorBairro.cs) — SHA-256 `268f2da2537763cc433e86becdba04bbeba3412c90fcd185160dc4bbcc312794`
- [PessoasPorBairro.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PessoasPorBairro.Designer.cs) — SHA-256 `8cb481c5ef61b046123ce3a49152f55cacf3c2f90d7ae01b7622f6384fb1408f`

## Pessoas por informação em campo personalizado

Classe: `PessoasPorCampoPesquisa`. Identificador: `Pessoa.PessoasPorCampoPesquisa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pesquisar pela informação em campo personalizado

**Campos apresentados (DTO):** `Rg`, `Telefone`, `Celular`, `Uf`, `Complemento`, `Municipio`, `Bairro`, `Numero`, `Endereco`, `Email`, `Nome`, `Ie`, `CpfCnpj`, `TituloCpfCnpj`, `CamposPersonalizados.Campos.ToText`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.PesquisarInformacaoCampoPersonalizado`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PessoasPorCampoPesquisa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PessoasPorCampoPesquisa.cs) — SHA-256 `de889f2f2a95703ff6fa6520cc75bbe1a02321c0986056072ee7b347b621fb71`
- [PessoasPorCampoPesquisa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PessoasPorCampoPesquisa.Designer.cs) — SHA-256 `5dcbcd6592d47b5916a8c5a0a7d2c572da897d35fb6c23bc22cbda53d3e26713`

## Pessoas por município

Classe: `PessoasPorMunicipio`. Identificador: `Pessoa.PessoasPorMunicipio`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Registros Ativos/Inativos; Tipo de cadastro; Município

**Campos apresentados (DTO):** `Rg`, `Telefone`, `Celular`, `Uf`, `Complemento`, `Municipio`, `Bairro`, `Numero`, `Endereco`, `Email`, `Nome`, `Cep`, `Ie`, `CpfCnpj`, `TituloCpfCnpj`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPessoasPorMunicipioETipoCadastro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PessoasPorMunicipio.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PessoasPorMunicipio.cs) — SHA-256 `43c1add64b8dd1df253fb1f03108fc420ef805827ad07bf561007ee273a7eaf5`
- [PessoasPorMunicipio.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PessoasPorMunicipio.Designer.cs) — SHA-256 `1dccc4e0d331befe9231ed6dc63a7eaf74954a72823c53e7a4bbf50384e90bd0`

## Pessoas por tabela de preço

Classe: `PessoasPorTabelaPreco`. Identificador: `Pessoa.PessoasPorTabelaPreco`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Registros Ativos/Inativos; Tabela de preço

**Campos apresentados (DTO):** `Rg`, `Telefone`, `Celular`, `Uf`, `Complemento`, `Municipio`, `Bairro`, `Numero`, `Endereco`, `Email`, `Nome`, `Cep`, `Ie`, `CpfCnpj`, `TituloCpfCnpj`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPessoasPorTabelaPrecoETipoCadastro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PessoasPorTabelaPreco.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PessoasPorTabelaPreco.cs) — SHA-256 `3aa497abb8efab58a6a7cb60bc57ba17e1ea02cc6802a589d70828ead407fd2e`
- [PessoasPorTabelaPreco.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PessoasPorTabelaPreco.Designer.cs) — SHA-256 `5f848d9116e4baf3d6ba38927c0f00f3a29af738392bdee28169d8260a99afec`

## Pessoas por tipo de cadastro

Classe: `PessoasPorTipoCadastro`. Identificador: `Pessoa.PessoasPorTipoCadastro`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Registros Ativos/Inativos; Tipo de cadastro; Tipo de pessoa; Classificação; Indicador de operação; Ordenação

**Campos apresentados (DTO):** `Rg`, `LimiteCredito`, `TituloCpfCnpj`, `CpfCnpj`, `Ie`, `Cep`, `Nome`, `Email`, `Endereco`, `Numero`, `Bairro`, `Municipio`, `Complemento`, `Uf`, `Celular`, `Telefone`, `CamposPersonalizados.CamposToText`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPessoasPorTipoCadastro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PessoasPorTipoCadastro.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PessoasPorTipoCadastro.cs) — SHA-256 `13cab97411f654530d3cdc8ae08776a87fc9733205804d79f598f252dc95e26e`
- [PessoasPorTipoCadastro.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PessoasPorTipoCadastro.Designer.cs) — SHA-256 `51b6ae21e876b0a94b223345ad6d06bdaaa9a0d9ebcc4700cb09598f51bc5fa6`

## Pessoas por tipo de segmentação

Classe: `PessoasPorSegmentacao`. Identificador: `Pessoa.PessoasPorSegmentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Registros Ativos/Inativos; Tipo de cadastro; Tipo de segmentação

**Campos apresentados (DTO):** `Rg`, `Telefone`, `Celular`, `Uf`, `Complemento`, `Municipio`, `Bairro`, `Numero`, `Endereco`, `Email`, `Nome`, `Cep`, `Ie`, `CpfCnpj`, `TituloCpfCnpj`, `DescricaoTipoSegmentacao`

**Agrupamentos do layout:**

- `DescricaoTipoSegmentacao`

**Resumos do layout:**

- `Count`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPessoasPorTipoSegmentacaoETipoCadastro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PessoasPorSegmentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PessoasPorSegmentacao.cs) — SHA-256 `6102509dc91e89cc5d9eb84cb4ca6757cc2b24398c484faab913b74c826bdfa9`
- [PessoasPorSegmentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PessoasPorSegmentacao.Designer.cs) — SHA-256 `7d91b6322d1e4d9e9258c89a2315d2f79b7e4619ac95f9f0d42b899ef23e8a79`

## Pessoas por UF

Classe: `PessoasPorUf`. Identificador: `Pessoa.PessoasPorUf`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Registros Ativos/Inativos; Tipo de cadastro; Estado; Ordenação

**Campos apresentados (DTO):** `Rg`, `Telefone`, `Celular`, `Uf`, `Complemento`, `Municipio`, `Bairro`, `Numero`, `Endereco`, `Email`, `Nome`, `Cep`, `Ie`, `CpfCnpj`, `TituloCpfCnpj`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPessoasPorUfETipoCadastro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [PessoasPorUf.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/PessoasPorUf.cs) — SHA-256 `48e4a0bcfa3f892255f31b53a25179876dac4ddc89482d32abf2704552f9fbe4`
- [PessoasPorUf.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/PessoasPorUf.Designer.cs) — SHA-256 `2748326873be158efd99ef356291ba782a7250cef5f6c8a1602665fa0e6ce78b`

## Registro para aviamento de receituário óptico

Classe: `LivroRegistroAviamentoReceituarioOptico`. Identificador: `Pessoa.LivroRegistroAviamentoReceituarioOptico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Prescritor; Tipo

**Campos apresentados (DTO):** `TotalDavOsGerado`, `PrescritorCbo`, `PrescritorNome`, `PessoaEndereco`, `PessoaNome`, `NumeroDavOsGerado`, `DataEmissao`, `DescricaoLenteEsquerda`, `InformacaoOticaPertoDireito.Esferico`, `InformacaoOticaLongeDireito.Esferico`, `InformacaoOticaLongeDireito.Cilindrico`, `InformacaoOticaPertoDireito.Eixo`, `InformacaoOticaLongeDireito.Eixo`, `InformacaoOticaLongeEsquerdo.Altura`, `InformacaoOticaLongeEsquerdo.Adicao`, `InformacaoOticaLongeEsquerdo.Dnp`, `InformacaoOticaLongeEsquerdo.Eixo`, `InformacaoOticaLongeEsquerdo.Cilindrico`, `InformacaoOticaLongeEsquerdo.Esferico`, `InformacaoOticaPertoEsquerdo.Altura`, `InformacaoOticaPertoEsquerdo.Adicao`, `InformacaoOticaPertoEsquerdo.Dnp`, `InformacaoOticaPertoEsquerdo.Eixo`, `InformacaoOticaPertoEsquerdo.Cilindrico`, `InformacaoOticaPertoEsquerdo.Esferico`, `DescricaoLenteDireita`, `InformacaoOticaPertoDireito.Cilindrico`, `InformacaoOticaLongeDireito.Altura`, `InformacaoOticaLongeDireito.Adicao`, `InformacaoOticaLongeDireito.Dnp`, `InformacaoOticaPertoDireito.Dnp`, `InformacaoOticaPertoDireito.Adicao`, `InformacaoOticaPertoDireito.Altura`, `Laboratorio`, `ObservacaoLonge`, `ObservacaoPerto`

**Expressões calculadas do layout:**

- `[Pessoa.Celulares.Numero]`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterPrescricoesOticas`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [LivroRegistroAviamentoReceituarioOptico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/LivroRegistroAviamentoReceituarioOptico.cs) — SHA-256 `a24192fa3d1ba469386d2b3549153d27f7189eb15840a24e03bc5fa80fed1873`
- [LivroRegistroAviamentoReceituarioOptico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/LivroRegistroAviamentoReceituarioOptico.Designer.cs) — SHA-256 `d2c6ed3348661357cba57fd71fb7f779764b6ab7634d75cc273d100d014d7069`

## Serviços por técnico

Classe: `ServicosPorTecnico`. Identificador: `Pessoa.ServicosPorTecnico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Técnico; Data inicial; Data final; Origem; Situação das movimentações

**Campos apresentados (DTO):** `Itens.Numero`, `Itens.CodigoInterno`, `Itens.Descricao`, `Itens.Tipo`, `Itens.UnidadeMedida`, `Itens.Quantidade`, `Itens.Total`, `Nome`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ServicosTecnico`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ServicosPorTecnico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ServicosPorTecnico.cs) — SHA-256 `3a1507968ea25b670478ff9cea6f5ab7aadbc2d38b7d3f6535828c38d868962e`
- [ServicosPorTecnico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ServicosPorTecnico.Designer.cs) — SHA-256 `7d6d00eebd4343895f6add7971684660eed41ab656a9e3719276edac5e69588f`

## Serviços por técnico com dados de comissão

Classe: `ServicosPorTecnicoComComissao`. Identificador: `Pessoa.ServicosPorTecnicoComComissao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Técnico; Data inicial; Data final; Origem; Situação das movimentações

**Campos apresentados (DTO):** `Itens.Numero`, `Itens.ValorComissao`, `Itens.PercentualComissao`, `Itens.CodigoInterno`, `Itens.Descricao`, `Itens.Tipo`, `Itens.UnidadeMedida`, `Itens.Quantidade`, `Itens.Total`, `Percentual`, `Nome`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ServicosTecnico`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ServicosPorTecnicoComComissao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ServicosPorTecnicoComComissao.cs) — SHA-256 `3befd918b9c6599a1a853adc05f8ba540ce46f4f56eaef3dff324d009b4298d9`
- [ServicosPorTecnicoComComissao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ServicosPorTecnicoComComissao.Designer.cs) — SHA-256 `955a7dd1176e9303366f2859155001ef280f367f36beb8f586b60bde596ba397`

## Total de comissões por colaborador - 40 colunas

Classe: `ComissoesPorColaborador40Colunas`. Identificador: `Pessoa.ComissoesPorColaborador40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pessoa; Situação; Data/Hora inicial; Data/Hora final; Filtrar por data de recebimento da parcela; Ocultar total (Base cálculo - valor comissões)

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Expressões calculadas do layout:**

- `Concat([TipoComissao.Descricao],': ' ,'[DocumentoReferencia]','-','[Pessoa.Nome]')`
- `Iif([Colaborador.Fisica.Vendedor.IsVendedor], [Colaborador.Fisica.Vendedor.RegiaoAtuacao], [Colaborador.Fisica.Tecnico.RegiaoAtuacao])`
- `Iif([Colaborador.Fisica.Vendedor.IsVendedor], [Colaborador.Fisica.Vendedor.PercentualComissao], [Colaborador.Fisica.Tecnico.PercentualComissao])`
- `Iif([Colaborador.Fisica.Vendedor.IsVendedor], [Colaborador.Fisica.Vendedor.Comissao.Descricao], [Colaborador.Fisica.Tecnico.Comissao.Descricao])`
- `Iif([DataRecebimento]== '01/01/0001 00:00:00', '', [DataRecebimento])`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterComissaoPorPeriodoColaboradorESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ComissoesPorColaborador40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pessoa/ComissoesPorColaborador40Colunas.cs) — SHA-256 `9afef4829725085e80c820d8721e95b834da370e5732e3fae7dbcc55c0bbf19d`
- [ComissoesPorColaborador40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pessoa/ComissoesPorColaborador40Colunas.Designer.cs) — SHA-256 `6558e82b278da180763bd35be0d5ce5a6915016fa92f72eda333ecd8b8806fde`

