# Posto

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Abastecimento por cliente/placa

Classe: `AbastecimentoPorClientePlaca`. Identificador: `Posto.AbastecimentoPorClientePlaca`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Pessoa

**Campos apresentados (DTO):** `Numero`, `Data`, `TotalItem`, `Quantidade`, `Quilometragem`, `DescricaoItem`, `Placa`

**Agrupamentos do layout:**

- `Placa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.AbastecimentoPorClientePlaca`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [AbastecimentoPorClientePlaca.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentoPorClientePlaca.cs) — SHA-256 `dd64fa6bbd0f9bdf18c482bbceff7bfd48ab42638f5835c7a751911665bf81dd`
- [AbastecimentoPorClientePlaca.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentoPorClientePlaca.Designer.cs) — SHA-256 `ed54ae5a84949461b2e86d7bb79c092ad0746bf59e894d4b061361c830c9cb6c`

## Abastecimentos por bico

Classe: `AbastecimentosPorBico`. Identificador: `Posto.AbastecimentosPorBico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Bico; Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Litros`, `Combustivel`, `NumeroDoBicoNoSoftware`

**Agrupamentos do layout:**

- `NumeroDoBicoNoSoftware`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterAbastecimentosPorBico`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [AbastecimentosPorBico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentosPorBico.cs) — SHA-256 `69df031b09a5ba3f9c3b8031c9808e6293ea916a4c87c0cb79cc8090a8979b09`
- [AbastecimentosPorBico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentosPorBico.Designer.cs) — SHA-256 `90fd0a427f03b1bbcb5128521c7b4c70cf2e2ad1d7eb89a3c94e60f930308db7`

## Abastecimentos por frentista

Classe: `AbastecimentosPorFrentista`. Identificador: `Posto.AbastecimentosPorFrentista`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Frentista; Bico; Data/Hora inicial; Data/Hora final; Mostrar aferições

**Campos apresentados (DTO):** `Total`, `ValorPago`, `Desconto`, `Preco`, `Litros`, `SituacaoAbastecimento`, `EncerranteFinal`, `EncerranteInicial`, `DataHora`, `Combustivel`, `NumeroDoBicoNoSoftware`, `Frentista`

**Agrupamentos do layout:**

- `Frentista`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterAbastecimentosPorFrentista`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [AbastecimentosPorFrentista.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentosPorFrentista.cs) — SHA-256 `77287410b2e429d10b07a2001cfe377708eaadfe521eddb531f4a4eef643ac7d`
- [AbastecimentosPorFrentista.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentosPorFrentista.Designer.cs) — SHA-256 `ebb3967b1aa5da95684f14240a367561250e9c0f1902d7f003e6ecdf94e01206`

## Abastecimentos por frentista - 40 colunas

Classe: `AbastecimentosPorFrentista40Colunas`. Identificador: `Posto.AbastecimentosPorFrentista40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Frentista; Bico; Data/Hora inicial; Data/Hora final; Mostrar abastecimentos; Mostrar aferições

**Campos apresentados (DTO):** `ValorPago`, `Desconto`, `SituacaoAbastecimento`, `DataHora`, `Total`, `Preco`, `Litros`, `Combustivel`, `NumeroDoBicoNoSoftware`, `Frentista`

**Agrupamentos do layout:**

- `Frentista`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterAbastecimentosPorFrentista`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [AbastecimentosPorFrentista40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentosPorFrentista40Colunas.cs) — SHA-256 `341ae27874b8796e6c77736b5fdc4f2a227954a477e21798ccd05a02c7bfbc9b`
- [AbastecimentosPorFrentista40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/AbastecimentosPorFrentista40Colunas.Designer.cs) — SHA-256 `3c338433217d7b59807011f0b42f0c676d32d5475bca8eb984664e702760bdc3`

## Aferições

Classe: `Afericoes`. Identificador: `Posto.Afericoes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Bico; Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Abastecimentos.Usuario`, `Abastecimentos.Litros`, `Abastecimentos.DataHora`, `Abastecimentos.Combustivel`, `Abastecimentos.NumeroDoBicoNoSoftware`

**Agrupamentos do layout:**

- `Combustivel`
- `NumeroDoBicoNoSoftware`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterAfericoes`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [Afericoes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/Afericoes.cs) — SHA-256 `25ce5e43ea3c02c0fb5aa0a39bb8b969c19b99a90ada687b8bfff6a26e1d07d2`
- [Afericoes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/Afericoes.Designer.cs) — SHA-256 `12c277e57300eac6dc5aee4e8d7d13b34d3d9bb917ab7cd36fd7cfcc14c9da18`

## Cadastro de tanques, bombas e bicos

Classe: `TanquesBombasBicos`. Identificador: `Posto.TanquesBombasBicos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `CapacidadeNominal`, `Numero`, `Descricao`, `Bombas.Modelo`, `Bombas.Numero`, `Bombas.Bicos.PosicaoRetorno`, `Bombas.Bicos.BicoConcentrador`, `Bombas.Bicos.ProdutoServicoEmpresa.ProdutoServico.Descricao`, `Bombas.Bicos.Numero`

**Agrupamentos do layout:**

- `Numero`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterTanques`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [TanquesBombasBicos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/TanquesBombasBicos.cs) — SHA-256 `21a74a12b3d81a115a6073e28080b32e86f807458bc04e0dc2c49d1d5d2f6021`
- [TanquesBombasBicos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/TanquesBombasBicos.Designer.cs) — SHA-256 `709b1898e184406ce568a6e3ee6c0baf4401f7fa55850de93185f26a9677f9a7`

## Movimentações por abastecimento

Classe: `MovimentacoesPorAbastecimento`. Identificador: `Posto.MovimentacoesPorAbastecimento`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final; Bico

**Campos apresentados (DTO):** `Abastecimento.EncerranteFinal`, `Abastecimento.EncerranteInicial`, `Abastecimento.Preco`, `Abastecimento.Litros`, `Abastecimento.DataHora`, `Abastecimento.DescricaoProduto`, `Abastecimento.NumeroDoBicoNoSoftware`, `Movimentacoes.Situacao`, `Movimentacoes.Valor`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Tipo`, `Status`

**Agrupamentos do layout:**

- `Abastecimento.IdProduto`
- `Abastecimento.DataHora`
- `Abastecimento.NumeroDoBicoNoSoftware`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterMovimentacoesPorAbastecimento`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [MovimentacoesPorAbastecimento.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/MovimentacoesPorAbastecimento.cs) — SHA-256 `ef4d018056c18749ad690ae01f8cf32395023a968e31d00d5c3898af7beec570`
- [MovimentacoesPorAbastecimento.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/MovimentacoesPorAbastecimento.Designer.cs) — SHA-256 `2ddb273f9ba420dde803cb32e720f4edddec2cca30604d8183a96bc0dfe59085`

## Resumo de perdas e ganhos

Classe: `ResumoPerdasGanhos`. Identificador: `Posto.ResumoPerdasGanhos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Itens.EstoqueFechamento`, `Itens.PerdasGanhos`, `Itens.DescricaoCombustivel`, `Itens.Data`, `Itens.NumeroTanque`, `Itens.EstoqueEscritural`, `TotalGanhos`, `TotalPerdas`, `Itens.PercentualPerdaGanho`, `Itens.SomaPercentuais`

**Expressões calculadas do layout:**

- `([].Sum([PerdasGanhos]) * 100)  / [].Sum([EstoqueFechamento])`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterResumoPerdasGanhos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [ResumoPerdasGanhos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/ResumoPerdasGanhos.cs) — SHA-256 `45920a67d90eac3efd4ea8b7cfdec1646021036c3f82c4720a21d49c51abb484`
- [ResumoPerdasGanhos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/ResumoPerdasGanhos.Designer.cs) — SHA-256 `05ccd9b3c5ee6d3ebbf1daddcc1ea90c227b34e8de0977b191c965c64fb33dc9`

## Resumo dos tanques

Classe: `ResumoTanques`. Identificador: `Posto.ResumoTanques`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `NivelPercentualEstoque`, `QuantidadeEstoque`, `NivelPercentual`, `Quantidade`, `CapacidadeNominal`, `Descricao`, `Bombas.Numero`

**Agrupamentos do layout:**

- `Numero`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterTanques`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [ResumoTanques.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/ResumoTanques.cs) — SHA-256 `aa0169e60c3fec6532c5ff16cc321f26dca615a3f0ed70e9b4c4433032f803fd`
- [ResumoTanques.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/ResumoTanques.Designer.cs) — SHA-256 `187cbb093cb615c9b2fd7376c66cc0171ee69060e4067fb9b7077d58f98265a7`

## Resumo dos volumes por período

Classe: `ResumoPeriodo`. Identificador: `Posto.ResumoPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Litros`, `SituacaoAbastecimento`, `EncerranteFinal`, `EncerranteInicial`, `DataHora`, `Combustivel`, `NumeroDoBicoNoSoftware`, `Frentista`

**Agrupamentos do layout:**

- `Frentista`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterAbastecimentosPorFrentista, IPostoCombustivelRelatorioModuleService.ObterTanques`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [ResumoPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/ResumoPeriodo.cs) — SHA-256 `9854f50a504794bf44b597355d1d937f34f7bea212988c4087316388116864c9`
- [ResumoPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/ResumoPeriodo.Designer.cs) — SHA-256 `c8d859988cc6df12810795cac059fc90edc9c4a356818bed00e8347000798444`

## Volumes registrados e faturados

Classe: `VolumesRegistradosEFaturados`. Identificador: `Posto.VolumesRegistradosEFaturados`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final; Mostrar detalhes por status do abastecimento

**Campos apresentados (DTO):** `VolumeAfericao`, `DiferencaVolume`, `VolumeFaturado`, `VolumeRegistrado`, `NomeUsuario`, `DescricaoProduto`, `Itens.VolumeAfericao`, `Itens.ItemDiferencaVolume`, `Itens.VolumeFaturado`, `Itens.VolumeRegistrado`, `Itens.StatusDoAbastecimento`

**Agrupamentos do layout:**

- `NomeUsuario`
- `DescricaoProduto`

**Expressões calculadas do layout:**

- `[VolumeRegistrado] - [VolumeFaturado] - [VolumeAfericao]`

**Contratos de serviço:**

- `IPostoCombustivelRelatorioModuleService`

**Chamadas catalogadas:**

- `IPostoCombustivelRelatorioModuleService.ObterVolumesRegistradosEFaturados`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-posto-combustivel.md](../../docs/continuacao-posto-combustivel.md)

**Limites registrados na pesquisa:**

- No runtime observation
- StatusDoAbastecimento numeric values not in retained sources
- Plate and odometer parsed from free-text observation

**Fontes da ficha:**

- [VolumesRegistradosEFaturados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PostoCombustivel/VolumesRegistradosEFaturados.cs) — SHA-256 `1654f85b8bf00f13e8091f4706738f90cb49b2b5770bfe108d4cb0c15ac7986d`
- [VolumesRegistradosEFaturados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PostoCombustivel/VolumesRegistradosEFaturados.Designer.cs) — SHA-256 `d6bb471c011e5c5d46c0f25b28c8df175dced11542aba4080291165754da241d`

