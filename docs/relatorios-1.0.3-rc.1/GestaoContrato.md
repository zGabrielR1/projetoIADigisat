# Gestão de contratos

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de contratos

Classe: `ListaContratos`. Identificador: `GestaoContrato.ListaContratos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pessoa; Técnico; Tipo do contrato; Filtrar por data; Data inicial; Data final; Tipo do faturamento; Status; Ordenação

**Campos apresentados (DTO):** `NumeroContrato`, `Servicos`, `DataInicial`, `DataFinal`, `TipoFaturamento`, `Valor`, `Status`, `PessoaEmail`, `PessoaNome`, `PessoaTelefone`, `PessoaEndereco`, `PessoaDocumento`

**Agrupamentos do layout:**

- `EmpresaDocumento`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`

**Contratos de serviço:**

- `IGestaoContratoModuleService`

**Chamadas catalogadas:**

- `IGestaoContratoModuleService.ObterListaContratos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- End-of-day expanded
- Status is a validity window, not a contract situation code
- Billing type outside 1/2 does not filter

**Fontes da ficha:**

- [ListaContratos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/GestaoContrato/ListaContratos.cs) — SHA-256 `3e36faf0c528e00822ff7c0c5bbe639d0f4d39314ea7b0d5d831ba40aa4a0023`
- [ListaContratos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/GestaoContrato/ListaContratos.Designer.cs) — SHA-256 `6d7851ede7e476a93c3fd572a83803e0e0deb0e7322fd4804bd8a7c91052a296`

## Lista de contratos por intervalo de vencimento

Classe: `ListaContratosPorVencimento`. Identificador: `GestaoContrato.ListaContratosPorVencimento`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Dia de vencimento financeiro inicial; Dia de vencimento financeiro final; Tipo do contrato; Tipo do faturamento; Status; Ordenação

**Campos apresentados (DTO):** `DiaVencimentoFinanceiro`, `PessoaTelefone`, `PessoaDocumento`, `Valor`, `Status`, `TipoFaturamento`, `PessoaNome`, `NumeroContrato`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`

**Contratos de serviço:**

- `IGestaoContratoModuleService`

**Chamadas catalogadas:**

- `IGestaoContratoModuleService.ObterListaContratosPorVencimento`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Filters the due day of month, not a date range
- End-of-day expansion does not apply

**Fontes da ficha:**

- [ListaContratosPorVencimento.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/GestaoContrato/ListaContratosPorVencimento.cs) — SHA-256 `ae14a5f2dfea29691245521db91df421cab0a6ff11b23b7ce6f9ab396bf0eb30`
- [ListaContratosPorVencimento.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/GestaoContrato/ListaContratosPorVencimento.Designer.cs) — SHA-256 `1741c58438cc4832d0b92bf8bb872005f232bcac06e824bb4619e4c9f6f65ff6`

## Lista de contratos sintético

Classe: `ListaContratosSintetico`. Identificador: `GestaoContrato.ListaContratosSintetico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Filtrar por data; Data inicial; Data final; Situação; Tipo do contrato; Tipo do faturamento; Status; Ordenação

**Campos apresentados (DTO):** `DiaVencimentoFinanceiro`, `PessoaTelefone`, `PessoaDocumento`, `Valor`, `Status`, `TipoFaturamento`, `PessoaNome`, `NumeroContrato`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`

**Contratos de serviço:**

- `IGestaoContratoModuleService`

**Chamadas catalogadas:**

- `IGestaoContratoModuleService.ObterListaContratosSintetico`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Active/inactive changes the company predicate
- Extra codigoDescricaoDto parameter is unused

**Fontes da ficha:**

- [ListaContratosSintetico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/GestaoContrato/ListaContratosSintetico.cs) — SHA-256 `e4bbf0e27192b7cc2eed2801635c3cbc82e5174c01dbe87682e17849cb64b346`
- [ListaContratosSintetico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/GestaoContrato/ListaContratosSintetico.Designer.cs) — SHA-256 `a2adb9cd24bb3d9f5141889815c915795ff6ea9e5501dde34922952ca462fb85`

