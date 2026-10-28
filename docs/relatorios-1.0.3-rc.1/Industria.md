# Indústria

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Colaboradores

Classe: `Colaboradores`. Identificador: `Industria.Colaboradores`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data de adimissão inicial; Data adimissão final; Tipo; Ordenação

**Campos apresentados (DTO):** `Nome`, `ColaboradorIndustria.DataAdmissao`, `ColaboradorIndustria.Salario`, `textProducao`

**Expressões calculadas do layout:**

- `Iif([ColaboradorIndustria.Producao] == True, 'Sim', 'Não')`

**Contratos de serviço:**

- `IPessoaRelatorioModuleService`

**Chamadas catalogadas:**

- `IPessoaRelatorioModuleService.ObterColaboradoresIndustria`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [Colaboradores.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/Colaboradores.cs) — SHA-256 `a58ceb32ac53d39516c45525780d576a256df59f9a7aea4a3acef78a70ee00b5`
- [Colaboradores.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/Colaboradores.Designer.cs) — SHA-256 `9130f5b094c652fa77f30e0210e22e149fe7e9ae2f3f2abd60418b666c1a44a6`

## Custos diretos

Classe: `CustosDiretos`. Identificador: `Industria.CustosDiretos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Situação; Exibir incidências; Ordenação

**Campos apresentados (DTO):** `TotalIncidencias`, `PercentualLucro`, `DataInclusao`, `Descricao`, `Incidencias.Percentual`, `Incidencias.Descricao`

**Contratos de serviço:**

- `ICustoDiretoRelatorioModuleService`

**Chamadas catalogadas:**

- `ICustoDiretoRelatorioModuleService.ObterCustos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [CustosDiretos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/CustosDiretos.cs) — SHA-256 `eabd5bba4e8d40b5024ef7f1b406930a3c8662124a9deaf8093ef1d19bbb927d`
- [CustosDiretos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/CustosDiretos.Designer.cs) — SHA-256 `3e4867a1374206200f235c2139126ad36b2ee5a36fe49ca6120af9247c74f0fa`

## Custos indiretos

Classe: `CustosIndiretos`. Identificador: `Industria.CustosIndiretos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mês e ano de referência incial; Mês e ano de referência final; Situação; Exibir incidências; Ordenação

**Campos apresentados (DTO):** `TotalIncidencias`, `SalarioAdministrativo`, `DataReferencia`, `Incidencias.Valor`, `Incidencias.Descricao`

**Contratos de serviço:**

- `ICustoIndiretoRelatorioModuleService`

**Chamadas catalogadas:**

- `ICustoIndiretoRelatorioModuleService.ObterCustosIndiretos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [CustosIndiretos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/CustosIndiretos.cs) — SHA-256 `6109d6b6bdae38d7c99af93917e655640f6e13af6a9d7eee64facc258106dbda`
- [CustosIndiretos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/CustosIndiretos.Designer.cs) — SHA-256 `7da3a861b5ad184b994af73b228493b787aa67c4c801fbc4116857ce1208a225`

## Desmontagens

Classe: `Desmontagens`. Identificador: `Industria.Desmontagens`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto produzido; Ordem de produção; Tipo da data; Data inicial; Data final; Exibir detalhes; Ordenação

**Campos apresentados (DTO):** `ProdutoProduzido.QuantidadeDesmontada`, `ProdutoProduzido.Quantidade`, `ProdutoProduzido.ProdutoDescricao`, `DataHoraFim`, `DataHoraInicio`, `NumeroOrdemProducao`, `Itens.QuantidadeDesmontada`, `Itens.Quantidade`, `Itens.ProdutoDescricao`

**Contratos de serviço:**

- `IOrdemProducaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IOrdemProducaoRelatorioModuleService.ObterDesmontagensAsync`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [Desmontagens.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/Desmontagens.cs) — SHA-256 `5749ece6a7cff84f36c11bfa5b5b5ea1936ce65542faf8fc093202a9acf33d51`
- [Desmontagens.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/Desmontagens.Designer.cs) — SHA-256 `4b9fb4e74369edd0bc6059ea5521986f8f8f3ff55752584d868b5eac2b381958`

## Histórico do estoque físico

Classe: `HistoricoProdutosPorEstoqueFisico`. Identificador: `Industria.HistoricoProdutosPorEstoqueFisico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto; Data inicial; Data final; Tipo do histórico; Ordenação

**Campos apresentados (DTO):** `Descricao`, `CodigoInterno`, `QuantidadesEstoqueFisico.Quantidade`, `QuantidadesEstoqueFisico.EstoqueFisico`, `QuantidadesEstoqueFisico.Historicos.Tipo`, `QuantidadesEstoqueFisico.Historicos.Quantidade`, `QuantidadesEstoqueFisico.Historicos.DataInclusao`

**Contratos de serviço:**

- `IEstoqueFisicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IEstoqueFisicoRelatorioModuleService.ObterHistoricoProdutoPorEstoqueFisicoAsync`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [HistoricoProdutosPorEstoqueFisico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/HistoricoProdutosPorEstoqueFisico.cs) — SHA-256 `5759dbce456c6b1587446b9075853a6454cba54c6fa21fd1aab4ad13476a975f`
- [HistoricoProdutosPorEstoqueFisico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/HistoricoProdutosPorEstoqueFisico.Designer.cs) — SHA-256 `8413a42003b3d3ef0d67d3be960a5f48636d68836ae85969673e47a7242e90ed`

## Insumos consumidos

Classe: `InsumosConsumidos`. Identificador: `Industria.InsumosConsumidos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Exibir informações do estoque consumido; Ordenação; Mostrar resumo por insumo

**Campos apresentados (DTO):** `UnidadeMedida`, `QuantidadeUtilizada`, `ProdutoDescricao`, `ProdutoCodigoInterno`, `NumeroOrdemProducao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterItensConsumidos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [InsumosConsumidos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/InsumosConsumidos.cs) — SHA-256 `c292662ede891cf8db07b07690b42d444912f82db36647cf5cd1f61df868125a`
- [InsumosConsumidos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/InsumosConsumidos.Designer.cs) — SHA-256 `46bec5770d4e44ac4e957abbe7619608d7afcd95e616f8e9a99b592ea32cd92d`

## Insumos necessários - Ordens de produção

Classe: `InsumosNecessariosOrdensProducao`. Identificador: `Industria.InsumosNecessariosOrdensProducao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data de emissão inicial; Data de emissão final; Situação

**Campos apresentados (DTO):** `QuantidadeNecessaria`, `UnidadeMedida`, `Descricao`, `Localizacao`

**Contratos de serviço:**

- `IProgramacaoIndustriaRelatorioModuleService`

**Chamadas catalogadas:**

- `IProgramacaoIndustriaRelatorioModuleService.ObterMateriasPrimasOrdensProducao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [InsumosNecessariosOrdensProducao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/InsumosNecessariosOrdensProducao.cs) — SHA-256 `8ca6a6c8111bb2d9d7c35002ec9dce3356621b6df88351cfb0903aca7811eaef`
- [InsumosNecessariosOrdensProducao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/InsumosNecessariosOrdensProducao.Designer.cs) — SHA-256 `5c194fccc81f0122af168d877658ce8f06e601b5c9f6637d95c0b2cc04c1400e`

## Insumos necessários - Programações

Classe: `InsumosNecessariosProgramacoes`. Identificador: `Industria.InsumosNecessariosProgramacoes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo de data; Situação

**Campos apresentados (DTO):** `QuantidadeNecessaria`, `UnidadeMedida`, `Descricao`, `Localizacao`

**Contratos de serviço:**

- `IProgramacaoIndustriaRelatorioModuleService`

**Chamadas catalogadas:**

- `IProgramacaoIndustriaRelatorioModuleService.ObterProgramacoes`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [InsumosNecessariosProgramacoes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/InsumosNecessariosProgramacoes.cs) — SHA-256 `b1f2a89d014762d7417393eee55f08959c4e064cb18c8eafb2abf24722744fc3`
- [InsumosNecessariosProgramacoes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/InsumosNecessariosProgramacoes.Designer.cs) — SHA-256 `c16c12b4c131667d5284202975ec104b2c4861fa632f15670405bd7df39bc327`

## Itens produzidos

Classe: `ItensProduzidos`. Identificador: `Industria.ItensProduzidos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ordem de produção; Colaborador; Data inicial; Data final; Exibir informações do estoque produzido; Ordenação

**Campos apresentados (DTO):** `UnidadeMedida`, `DuracaoString`, `DataHoraFimProducao`, `DataHoraInicioProducao`, `QuantidadeProduzida`, `ProdutoDescricao`, `ProdutoCodigoInterno`, `NumeroOrdemProducao`, `count`

**Expressões calculadas do layout:**

- `[].Count()`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterItensProduzidos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [ItensProduzidos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/ItensProduzidos.cs) — SHA-256 `2a2b884f75285f6715149784000d0827f74a64ed213ee845b1fa9c3597d1a0a5`
- [ItensProduzidos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/ItensProduzidos.Designer.cs) — SHA-256 `4113a2c20cd0439fa96734134e602fefe04e9f8f9256f84920dbcdc7a88c94ee`

## Itens produzidos com insumos utilizados

Classe: `ItensProduzidosComInsumosUtilizados`. Identificador: `Industria.ItensProduzidosComInsumosUtilizados`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ordem de produção; Data inicial; Data final; Exibir informações dos estoques; Ordenação

**Campos apresentados (DTO):** `QuantidadePrevista`, `UnidadeMedida`, `NumeroOrdemProducao`, `QuantidadeProduzida`, `ProdutoCodigoInterno`, `ProdutoDescricao`, `MateriasPrimasUtilizadas.Padrao`, `MateriasPrimasUtilizadas.ProdutoSubstituidoDescricao`, `MateriasPrimasUtilizadas.PercentualPerdaPrevista`, `MateriasPrimasUtilizadas.QuantidadeUtilizada`, `MateriasPrimasUtilizadas.QuantidadePrevista`, `MateriasPrimasUtilizadas.UnidadeMedida`, `MateriasPrimasUtilizadas.ProdutoDescricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterItensProduzidosComInsumosUtilizados`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [ItensProduzidosComInsumosUtilizados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/ItensProduzidosComInsumosUtilizados.cs) — SHA-256 `ce8868b76a2ea9b966da6117057fed38402686f39ec9295bd5bd23a097694bb2`
- [ItensProduzidosComInsumosUtilizados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/ItensProduzidosComInsumosUtilizados.Designer.cs) — SHA-256 `478815dc52943c766d8579940e068fb66c9b916a074dff6ccbc6346501536f21`

## Movimentações internas

Classe: `MovimentacoesInternas`. Identificador: `Industria.MovimentacoesInternas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto; Tipo do estoque físico na movimentação interna; Estoque físico; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `Observacao`, `Colaborador`, `EstoqueFisicoOrigem`, `EstoqueFisicoDestino`, `Quantidade`, `NomeUsuario`, `Data`, `ProdutoCodigoInterno`, `ProdutoDescricao`

**Agrupamentos do layout:**

- `ProdutoCodigoInterno`

**Contratos de serviço:**

- `IEstoqueFisicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IEstoqueFisicoRelatorioModuleService.ObterMovimentacoesInternasAsync`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [MovimentacoesInternas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/MovimentacoesInternas.cs) — SHA-256 `e0f633fc031ddd707e6fe59159d88edc5b4c4b70df86b7b2dade7e9090cd4558`
- [MovimentacoesInternas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/MovimentacoesInternas.Designer.cs) — SHA-256 `feeb4fdc9eac823902241cfd6df6e41b0d18154e916dfafbe12e2d27cd41d75a`

## Orçamentos

Classe: `Orcamentos`. Identificador: `Industria.Orcamentos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa; Data inicial; Data final; Tipo de data; Situação; Exibir itens

**Campos apresentados (DTO):** `Total`, `PessoaDocumento`, `PessoaNome`, `Numero`, `DataHoraValidade`, `DataHoraEmissao`, `Itens.TotalMateriasPrimas`, `Itens.ProdutoProduzidoValorTotal`, `Itens.ProdutoProduzidoPrecoVenda`, `Itens.ProdutoProduzidoPrecoCusto`, `Itens.ProdutoProduzidoQuantidade`, `Itens.ProdutoProduzidoDescricao`, `Situacao`

**Contratos de serviço:**

- `IOrcamentoIndustriaRelatorioModuleService`

**Chamadas catalogadas:**

- `IOrcamentoIndustriaRelatorioModuleService.ObterOrcamentosIndustria`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [Orcamentos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/Orcamentos.cs) — SHA-256 `e36af85f52e9f74f4ad743f3eafdb975ea2605a197e99bfad9792ad7c48ed405`
- [Orcamentos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/Orcamentos.Designer.cs) — SHA-256 `94b1721ffa60b251c967ddfdaf7829804c5215596ba6306542fb2ed9e2d5944d`

## Ordens de produção

Classe: `OrdensProducao`. Identificador: `Industria.OrdensProducao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto produzido; Data inicial; Data final; Situação; Exibir detalhes; Ordenação

**Campos apresentados (DTO):** `QuantidadeProduzida`, `DataHoraEmissao`, `Descricao`, `Numero`, `ProdutoProduzidoDescricao`, `Quantidade`, `Situacao`, `Etapas.DuracaoPrevista`, `Etapas.DataHoraFim`, `Etapas.DataHoraInicio`, `Etapas.Descricao`, `Etapas.MateriasPrimas.QuantidadeUtilizada`, `Etapas.MateriasPrimas.QuantidadePrevista`, `Etapas.MateriasPrimas.Descricao`, `Etapas.Colaboradores.PessoaNome`, `Etapas.Colaboradores.Tempo`

**Contratos de serviço:**

- `IOrdemProducaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IOrdemProducaoRelatorioModuleService.ObterOrdensProducaoAsync`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [OrdensProducao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/OrdensProducao.cs) — SHA-256 `be2d237ddfd6bee67d291fe4f4719555c5cf272f18291cfac851308b04853171`
- [OrdensProducao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/OrdensProducao.Designer.cs) — SHA-256 `aebbdd1b7c25c08364659df8e2c9d4616af7ac1d986722b664f86ed0ff8fe4c8`

## Produtos por estoque físico

Classe: `ProdutosPorEstoqueFisico`. Identificador: `Industria.ProdutosPorEstoqueFisico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Estoque físico; Ordenação

**Campos apresentados (DTO):** `Descricao`, `CodigoBarras`, `CodigoInterno`, `QuantidadesEstoqueFisico.Quantidade`, `QuantidadesEstoqueFisico.EstoqueFisico`

**Contratos de serviço:**

- `IEstoqueFisicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IEstoqueFisicoRelatorioModuleService.ObterProdutosPorEstoqueFisicoAsync`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [ProdutosPorEstoqueFisico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/ProdutosPorEstoqueFisico.cs) — SHA-256 `2fed3fb79ede601af66daa73aa33a830c24784f7ce8041bb1ffff90b2dbff272`
- [ProdutosPorEstoqueFisico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/ProdutosPorEstoqueFisico.Designer.cs) — SHA-256 `1db9d41531c6d32c541afd451b1e9ae6c92de6228f3de439ff9247addb163d48`

## Produtos por matéria-prima

Classe: `ProdutosPorMateriaPrima`. Identificador: `Industria.ProdutosPorMateriaPrima`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Matéria-prima; Ordenação

**Campos apresentados (DTO):** `Quantidade`, `DescricaoProdutoProduzido`, `CodigoInternoProdutoProduzido`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterProdutosProduzidosPorMateriaPrimaUtilizada`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [ProdutosPorMateriaPrima.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/ProdutosPorMateriaPrima.cs) — SHA-256 `10a7642153081de7e84fe63385b798573f9fea022729b82fc6a826648509f12c`
- [ProdutosPorMateriaPrima.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/ProdutosPorMateriaPrima.Designer.cs) — SHA-256 `8ac6157c7eede674586ded356d39ecdc2f964f42c52763c85b6de4d8e374db97`

## Produtos produzidos com insumos utilizados

Classe: `ProdutosProduzidosComInsumosUtilizados`. Identificador: `Industria.ProdutosProduzidosComInsumosUtilizados`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto produzido; Data inicial; Data final

**Campos apresentados (DTO):** `QuantidadePrevista`, `UnidadeMedida`, `QuantidadeProduzida`, `ProdutoCodigoInterno`, `ProdutoDescricao`, `MateriasPrimasUtilizadas.QuantidadeUtilizada`, `MateriasPrimasUtilizadas.UnidadeMedida`, `MateriasPrimasUtilizadas.ProdutoDescricao`

**Agrupamentos do layout:**

- `ProdutoCodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterItensProduzidosComInsumosUtilizadosPorProdutoProduzido`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [ProdutosProduzidosComInsumosUtilizados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/ProdutosProduzidosComInsumosUtilizados.cs) — SHA-256 `968a06b19841bd88d92a0630353873491b2e581481bbf16249979fdb1d3b33d8`
- [ProdutosProduzidosComInsumosUtilizados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/ProdutosProduzidosComInsumosUtilizados.Designer.cs) — SHA-256 `41ebc604135b8b44200204c0577e5953aad62161d1460e42ff85f9aa9cba83ae`

## Programações

Classe: `Programacoes`. Identificador: `Industria.Programacoes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo de data; Situação; Exibir ordens de produção; Exibir interrupções

**Campos apresentados (DTO):** `DataHoraFinal`, `DataHoraInicial`, `Descricao`, `Ordens.Duracao`, `Ordens.DataHoraFinal`, `Ordens.DataHoraInicial`, `Ordens.Descricao`, `IntervaloOrdensTexto`, `Interrupcoes.Duracao`, `Interrupcoes.DataHoraFinal`, `Interrupcoes.DataHoraInicial`, `Interrupcoes.Motivo`, `Ordens.ProdutoProduzido`, `Ordens.Quantidade`

**Contratos de serviço:**

- `IProgramacaoIndustriaRelatorioModuleService`

**Chamadas catalogadas:**

- `IProgramacaoIndustriaRelatorioModuleService.ObterProgramacoes`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [Programacoes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/Programacoes.cs) — SHA-256 `877b22deabfe21e3326c6c74398adefbb6efa0ee3e269e344480672f957da8aa`
- [Programacoes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/Programacoes.Designer.cs) — SHA-256 `9d303a72b9a3c10f9796263c80716de8240b90389b2c4785832d6833082d2c7d`

## Tempo das ordens de produção

Classe: `TempoOrdensProducao`. Identificador: `Industria.TempoOrdensProducao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Colaborador; Data inicial; Data final; Situação; Exibir etapas; Exibir totais das etapas (colaborador)

**Campos apresentados (DTO):** `Colaborador`, `Ordens.Numero`, `Ordens.Descricao`, `Ordens.DataEmissao`, `Ordens.Situacao`, `Ordens.TempoTotal`, `Total`, `Ordens.Etapas.Nome`, `Ordens.Etapas.Tempo`, `TotaisPorEtapa.Nome`, `TotaisPorEtapa.Tempo`

**Agrupamentos do layout:**

- `Colaborador`

**Contratos de serviço:**

- `IOrdemProducaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IOrdemProducaoRelatorioModuleService.ObterOrdensProducaoAsync`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-industria.md](../../docs/continuacao-industria.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Period predicates based on ObjectId generation ranges; driver translation unknown
- Null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [TempoOrdensProducao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Industria/TempoOrdensProducao.cs) — SHA-256 `4bfe1253cb25cdbc3c19f1829e3faa438ae0c266a1e9cbe2627cbf684608febd`
- [TempoOrdensProducao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Industria/TempoOrdensProducao.Designer.cs) — SHA-256 `bcf843091fe35fa51747fce0104dbdeeee6f0e60794cbde98f9f8cbf6ebaf629`

