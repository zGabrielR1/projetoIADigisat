# Locações

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Locações por data

Classe: `LocacoesPorData`. Identificador: `Locacao.LocacoesPorData`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa; Data inicial; Data final; Situação

**Campos apresentados (DTO):** `PessoaDocumento`, `ObjetoDescricao`, `Situacao`, `PessoaNome`, `Valor`, `DataFinal`, `DataInicial`

**Agrupamentos do layout:**

- `PessoaDocumento`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `ILocacaoModuleService`

**Chamadas catalogadas:**

- `ILocacaoModuleService.ObterLocacoesPorDataRelatorio`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-locacao.md](../../docs/continuacao-locacao.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification version identity not reconfirmed
- Additional repository restrictions unknown

**Fontes da ficha:**

- [LocacoesPorData.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Locacao/LocacoesPorData.cs) — SHA-256 `5ad1009a7b8fe10253a88b6932dbbdb6a616d471ec122f19d88f88344ff4d0b0`
- [LocacoesPorData.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Locacao/LocacoesPorData.Designer.cs) — SHA-256 `23b5ce5289e1d6717f1b1786e9972f3dfc2e360ad86ac8335316108a38b4dbea`

## Locações por objeto

Classe: `LocacoesPorObjeto`. Identificador: `Locacao.LocacoesPorObjeto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Objeto; Data inicial; Data final; Situação

**Campos apresentados (DTO):** `PessoaDocumento`, `Situacao`, `PessoaNome`, `Valor`, `DataFinal`, `DataInicial`

**Agrupamentos do layout:**

- `PessoaDocumento`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `ILocacaoModuleService`

**Chamadas catalogadas:**

- `ILocacaoModuleService.ObterLocacoesPorObjeto`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-locacao.md](../../docs/continuacao-locacao.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification version identity not reconfirmed
- Additional repository restrictions unknown

**Fontes da ficha:**

- [LocacoesPorObjeto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Locacao/LocacoesPorObjeto.cs) — SHA-256 `afe19390ef21385bb878c8f7f20021a8ca5a6c308cd8fa502d44530c40e4c396`
- [LocacoesPorObjeto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Locacao/LocacoesPorObjeto.Designer.cs) — SHA-256 `dab5949a1d25b124d3424a8dd833df08a30162f377a5600d39c49fbda3a8766a`

## Previsões para devoluções de locações.

Classe: `PrevisaoDevolucaoLocacao`. Identificador: `Locacao.PrevisaoDevolucaoLocacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `ObjetoDescricao`, `Situacao`, `Valor`, `DataInicial`, `PessoaNome`, `PessoaDocumento`, `DataFinal`

**Agrupamentos do layout:**

- `DataFinal`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `ILocacaoModuleService`

**Chamadas catalogadas:**

- `ILocacaoModuleService.ObterPrevisaoDevolucaoLocacao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-locacao.md](../../docs/continuacao-locacao.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification version identity not reconfirmed
- Additional repository restrictions unknown

**Fontes da ficha:**

- [PrevisaoDevolucaoLocacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Locacao/PrevisaoDevolucaoLocacao.cs) — SHA-256 `6816196ae8ec07dacc84c617497c57bc3a1285f13e0275aff676ea61d9619ed2`
- [PrevisaoDevolucaoLocacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Locacao/PrevisaoDevolucaoLocacao.Designer.cs) — SHA-256 `c0ea9923e1fcc6ae28fa53cf67c35f330e216d509ecac2e51e562fbea8968fb4`

## Previsões para inicio de locações

Classe: `PrevisaoInicioLocacao`. Identificador: `Locacao.PrevisaoInicioLocacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `PessoaNome`, `PessoaDocumento`, `DataInicial`, `Situacao`, `ObjetoDescricao`, `Valor`

**Agrupamentos do layout:**

- `DataInicial`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `ILocacaoModuleService`

**Chamadas catalogadas:**

- `ILocacaoModuleService.ObterPrevisaoInicioLocacao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-locacao.md](../../docs/continuacao-locacao.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification version identity not reconfirmed
- Additional repository restrictions unknown

**Fontes da ficha:**

- [PrevisaoInicioLocacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Locacao/PrevisaoInicioLocacao.cs) — SHA-256 `ca407588027b097394144898a839255d5e631e595d316113c20952dbf74666bb`
- [PrevisaoInicioLocacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Locacao/PrevisaoInicioLocacao.Designer.cs) — SHA-256 `ae11a82ffbe2c209a720d8444f45ab1380edd9033f46840ea220e42f6f829260`

