# Operação fiscal

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Itens movimentados por CFOP

Classe: `ItensMovimentadosPorCfop`. Identificador: `OperacaoFiscal.ItensMovimentadosPorCfop`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; CFOP; Data inicial; Data final

**Campos apresentados (DTO):** `PercentualIcms`, `CstCsosn`, `Preco`, `ValorIpi`, `OutraDespesa`, `Desconto`, `ValorIcmsSt`, `Total`, `ValorIcms`, `Quantidade`, `CodigoDescricao`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `CodigoDescricao`

**Contratos de serviço:**

- `ICfopRelatorioModuleService`

**Chamadas catalogadas:**

- `ICfopRelatorioModuleService.ObterItensMovimentadosPorCfop`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensMovimentadosPorCfop.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Cfop/ItensMovimentadosPorCfop.cs) — SHA-256 `028a461f6e6a7d79b7b285639e70738beb15ea08e86a7fb15d8849e8b44c94af`
- [ItensMovimentadosPorCfop.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Cfop/ItensMovimentadosPorCfop.Designer.cs) — SHA-256 `a0a033eb12bd09b5845f5473bb497fa867b546d241bd83ab2e72e0da8225e55b`

## Lista de cadastro de CFOP

Classe: `ListaCadastroCfop`. Identificador: `OperacaoFiscal.ListaCadastroCfop`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `Cfop.Codigo`, `Cfop.Descricao`, `movEstoque`, `movFinanceiro`, `compoeTotal`, `geraCredito`, `comissao`

**Expressões calculadas do layout:**

- `Iif([MovimentaFinanceiro]==false, 'Não', 'Sim')`
- `Iif([MovimentaEstoque]==false, 'Não', 'Sim')`
- `Iif([CompoeTotalNota]==false, 'Não', 'Sim')`
- `Iif([GeraComissao]==false, 'Não', 'Sim')`
- `Iif([GeraCreditoPisCofins]==false, 'Não', 'Sim')`

**Contratos de serviço:**

- `ICfopRelatorioModuleService`

**Chamadas catalogadas:**

- `ICfopRelatorioModuleService.ObterListaCadastroCfop`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No company, active or period predicate

**Fontes da ficha:**

- [ListaCadastroCfop.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Cfop/ListaCadastroCfop.cs) — SHA-256 `525f329e1cba8c27b7007a49715d2ed192f3cdcf2b1444f5a7cfddefa8fc93be`
- [ListaCadastroCfop.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Cfop/ListaCadastroCfop.Designer.cs) — SHA-256 `2eb48d17b4cc1e4d8d337b90170978b623d1229498e8437e2bd20c7bee9785ea`

## Totalização por CFOP

Classe: `TotalizacaoPorCfop`. Identificador: `OperacaoFiscal.TotalizacaoPorCfop`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo de movimento; Data inicial; Data final

**Campos apresentados (DTO):** `Total`, `TotalCofins`, `Quantidade`, `Desconto`, `OutrasDespesas`, `TotalIcms`, `TotalIcmsSt`, `TotalIpi`, `TotalPis`, `Descricao`, `Cfop`

**Agrupamentos do layout:**

- `Cfop`
- `Descricao`

**Contratos de serviço:**

- `ICfopRelatorioModuleService`

**Chamadas catalogadas:**

- `ICfopRelatorioModuleService.ObterTotalizacaoPorCfop`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [TotalizacaoPorCfop.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Cfop/TotalizacaoPorCfop.cs) — SHA-256 `df8727f71215d4688c870a011d94d103684fd7d3b273d36e3a572a5c733176fe`
- [TotalizacaoPorCfop.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Cfop/TotalizacaoPorCfop.Designer.cs) — SHA-256 `7b2ebe98408ec158ea864f349151bf3f0bdee4cb2bea8d38240c98ff0840de64`

