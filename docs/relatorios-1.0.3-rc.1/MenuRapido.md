# Menus

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Grupo de complementos

Classe: `ListaGrupoMenu`. Identificador: `MenuRapido.ListaGrupoMenu`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto contido nos adicionais do grupo

**Campos apresentados (DTO):** `TabelaPreco`, `DescricaoGrupo`, `Tipo`, `QuantidadeMinima`, `QuantidadeMaxima`, `ItensGrupo.QuantidadeMinima`, `ItensGrupo.QuantidadeMaxima`, `ItensGrupo.Descricao`

**Agrupamentos do layout:**

- `Numero`

**Contratos de serviço:**

- `IMenuRapidoModuleService`

**Chamadas catalogadas:**

- `IMenuRapidoModuleService.ObterGruposMenuRelatorio`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No company predicate
- Product filter is applied in memory

**Fontes da ficha:**

- [ListaGrupoMenu.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/MenuRapido/ListaGrupoMenu.cs) — SHA-256 `f39632e466d68740fd8d02643312310a22b71f359d190b794773c065c03ec4b6`
- [ListaGrupoMenu.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/MenuRapido/ListaGrupoMenu.Designer.cs) — SHA-256 `74196c2dc5db1cfd3d4bf589e1b313520cbe279fd92f346ac01304254ad5e2f9`

## Menus

Classe: `ListaMenus`. Identificador: `MenuRapido.ListaMenus`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto contido nos menus

**Campos apresentados (DTO):** `Descricao`, `ItensMenuRapido.ProdutoServicoEmpresa.ProdutoServico.Id`, `ItensMenuRapido.ProdutoServicoEmpresa.Preco.PrecoVendaVigente`, `ItensMenuRapido.ProdutoServicoEmpresa.ProdutoServico.Descricao`, `ItensMenuRapido.ProdutoServicoEmpresa.ProdutoServico.CodigoInterno`, `ItensMenuRapido.DescricaoTela`, `ItensMenuRapido.GruposMenuRapidoRelatorio.TabelaPrecoToText`, `ItensMenuRapido.GruposMenuRapidoRelatorio.QuantidadeMinima`, `ItensMenuRapido.GruposMenuRapidoRelatorio.QuantidadeMaxima`, `ItensMenuRapido.GruposMenuRapidoRelatorio.DescricaoGrupo`, `ItensMenuRapido.GruposMenuRapidoRelatorio.ItensGrupo.QuantidadeMinima`, `ItensMenuRapido.GruposMenuRapidoRelatorio.ItensGrupo.Descricao`, `ItensMenuRapido.GruposMenuRapidoRelatorio.ItensGrupo.QuantidadeMaxima`

**Agrupamentos do layout:**

- `Numero`
- `DescricaoTela`

**Contratos de serviço:**

- `IMenuRapidoModuleService`

**Chamadas catalogadas:**

- `IMenuRapidoModuleService.ObterMenusRelatorio`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Product filter is applied in memory
- Version cutoff is TimeSpan.Zero

**Fontes da ficha:**

- [ListaMenus.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/MenuRapido/ListaMenus.cs) — SHA-256 `14719edd9078b6c6831c75d4342dfad9a708d7c255ac3e9bc01e1285e2ce275e`
- [ListaMenus.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/MenuRapido/ListaMenus.Designer.cs) — SHA-256 `c8208f233084df2ab8ce7ca60fe9bab58d3d376ebdf3716680ce683b4730def2`

