# Devoluções

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Devoluções por período

Classe: `DevolucoesPorPeriodo`. Identificador: `Devolucao.DevolucoesPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa; Data inicial; Data final; Tipo da devolução; Vendedor

**Campos apresentados (DTO):** `NomeUsuario`, `MovimentacaoDataHoraEmissao`, `MovimentacaoDescricao`, `DataInclusao`, `Total`, `calculatedField1`, `PessoaNome`, `Itens.Estoque.Quantidade`, `Itens.Codigo`, `Itens.Descricao`, `Itens.EstoqueVenda.Quantidade`, `Itens.Unidade`, `Itens.Total`

**Agrupamentos do layout:**

- `PessoaReferencia`

**Expressões calculadas do layout:**

- `Iif([IsDevolucaoDav], ' ', 'Data venda')`

**Contratos de serviço:**

- `IDevolucaoModuleService`

**Chamadas catalogadas:**

- `IDevolucaoModuleService.DevolucoesPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Date filter is record inclusion, not sale date
- Seller filter is applied in memory and trims items
- DAV rows hide the sale date

**Fontes da ficha:**

- [DevolucoesPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Devolucao/DevolucoesPorPeriodo.cs) — SHA-256 `988535b7d8adf22356cb40fcff003df68d5e8dfbf03f79e58ec118a3371f15d3`
- [DevolucoesPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Devolucao/DevolucoesPorPeriodo.Designer.cs) — SHA-256 `648f3cdb11f049b31b9e7aec16dc4a98a2f210218e8682549f1d812abe678d03`

