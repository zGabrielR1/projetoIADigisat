# Movimentações CTRC

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Conhecimentos de transporte rodoviário de cargas por período

Classe: `CtrcsPorPeriodo`. Identificador: `Ctrc.CtrcsPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Emissor; Data inicial; Data final

**Campos apresentados (DTO):** `ChaveCte`, `SituacaoToText`, `OperacaoFiscal.Cfop.Codigo`, `DataEmissao`, `Serie`, `Modelo.Modelo`, `Numero`, `Emissor.EnderecoPrincipal.ToText`, `Emissor.TelefonePrincipal.ToText`, `Emissor.CpfCnpjFormatado`, `Emissor.Nome`, `Total`

**Agrupamentos do layout:**

- `Emissor.Nome`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterMovimentacoesCtrc`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [CtrcsPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/CtrcsPorPeriodo.cs) — SHA-256 `46aefb13647dcad9d215621e5d61459701881fc62027546e6756e445de012e39`
- [CtrcsPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/CtrcsPorPeriodo.Designer.cs) — SHA-256 `7361974b4b242ed42a7bc49499b457d1f3afac5615e8d48d0f829956e5fcb126`

