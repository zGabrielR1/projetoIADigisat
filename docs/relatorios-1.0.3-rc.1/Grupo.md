# Grupo

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Itens comprados por subgrupo

Classe: `ItensCompradosPorSubGrupo`. Identificador: `Grupo.ItensCompradosPorSubGrupo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; SubGrupo; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `CodigoDescricao`, `CustoTotal`, `PrecoCusto`, `Quantidade`, `SubGrupo`

**Agrupamentos do layout:**

- `SubGrupo`

**Contratos de serviço:**

- `IGrupoRelatorioModuleService`

**Chamadas catalogadas:**

- `IGrupoRelatorioModuleService.ObterItensCompradosPorSubGrupo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensCompradosPorSubGrupo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/ItensCompradosPorSubGrupo.cs) — SHA-256 `ff4cbfb03d80a2706a277b1bbda5ba84d9b96abe666e37673d6a14b18aee81fc`
- [ItensCompradosPorSubGrupo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/ItensCompradosPorSubGrupo.Designer.cs) — SHA-256 `96f90261635e27568c6d1042f1c7a7ac9789c1f9bc0a39490a80f75c06178d6f`

## Itens vendidos por subgrupo

Classe: `ItensVendidosPorSubGrupo`. Identificador: `Grupo.ItensVendidosPorSubGrupo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; SubGrupo; Vendedor; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `CodigoDescricao`, `CustoTotal`, `MargemContribuicao`, `Markup`, `PercentualCusto`, `PercentualMargem`, `PercentualTotal`, `PrecoCusto`, `PrecoVenda`, `Quantidade`, `VendaTotal`, `SubGrupo`

**Agrupamentos do layout:**

- `SubGrupo`

**Contratos de serviço:**

- `IGrupoRelatorioModuleService`

**Chamadas catalogadas:**

- `IGrupoRelatorioModuleService.ObterItensVendidosPorSubGrupo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosPorSubGrupo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/ItensVendidosPorSubGrupo.cs) — SHA-256 `ee599604f1cf48041d5c6938d42dacdd901adf7599152e80db54682b839f1581`
- [ItensVendidosPorSubGrupo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/ItensVendidosPorSubGrupo.Designer.cs) — SHA-256 `c50ac0ac1c641f3771214692c155b4c5aa9d910f9c22a89e158107a03bd9849c`

## Lista de preços por subgrupo

Classe: `ListaPrecoProdutosSubGrupos`. Identificador: `Grupo.ListaPrecoProdutosSubGrupos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** SubGrupo; Ordenação

**Campos apresentados (DTO):** `Subgrupo`, `Produtos.PrecoVenda`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosSubgrupos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaPrecoProdutosSubGrupos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/ListaPrecoProdutosSubGrupos.cs) — SHA-256 `490ab62a8d685279b4a2afee2162c7ad0605bb87eef7eecc3a133a8faeeedc1c`
- [ListaPrecoProdutosSubGrupos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/ListaPrecoProdutosSubGrupos.Designer.cs) — SHA-256 `2c8ec73a28841e770514f5b250ba8bce028098e205a03f07038fdf3014dac295`

## Lista de produtos por grupo

Classe: `ListaProdutosGrupos`. Identificador: `Grupo.ListaProdutosGrupos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; Grupo; Ordenação

**Campos apresentados (DTO):** `Grupo`, `SubGrupos.Produtos.VendaTotal`, `SubGrupos.Produtos.CustoTotal`, `SubGrupos.Subgrupo`, `SubGrupos.Produtos.PercentualLucro`, `SubGrupos.Produtos.QuantidadeMaxima`, `SubGrupos.Produtos.QuantidadeMinima`, `SubGrupos.Produtos.Quantidade`, `SubGrupos.Produtos.PrecoVenda`, `SubGrupos.Produtos.PrecoCusto`, `SubGrupos.Produtos.UnidadeMedida`, `SubGrupos.Produtos.Descricao`, `SubGrupos.Produtos.CodigoBarras`, `SubGrupos.Produtos.CodigoInterno`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosGrupos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosGrupos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/ListaProdutosGrupos.cs) — SHA-256 `7d9e416c1879e496b300c7b0e90a0a79e63dc6ccb2bbf6695bb718fa21d0ff23`
- [ListaProdutosGrupos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/ListaProdutosGrupos.Designer.cs) — SHA-256 `22c6ef5de203903439ee88bc16468321524553a01b3bc945b4d35305669d6c2e`

## Lista de produtos por subgrupo

Classe: `ListaProdutosSubGrupos`. Identificador: `Grupo.ListaProdutosSubGrupos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Mostrar produtos; SubGrupo; Ordenação

**Campos apresentados (DTO):** `Subgrupo`, `Produtos.PercentualLucro`, `Produtos.VendaTotal`, `Produtos.CustoTotal`, `Produtos.QuantidadeMaxima`, `Produtos.QuantidadeMinima`, `Produtos.Quantidade`, `Produtos.PrecoVenda`, `Produtos.PrecoCusto`, `Produtos.UnidadeMedida`, `Produtos.Descricao`, `Produtos.CodigoBarras`, `Produtos.CodigoInterno`, `Produtos.CamposPersonalizados.Informacao`, `Produtos.CamposPersonalizados.Descricao`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterListaProdutosSubgrupos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaProdutosSubGrupos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/ListaProdutosSubGrupos.cs) — SHA-256 `17a5a367fc9a018537092e8238272e00b74e255b61c5a2d4c596803c5d6c1488`
- [ListaProdutosSubGrupos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/ListaProdutosSubGrupos.Designer.cs) — SHA-256 `af3ea9781e95ea7148588ee83f3d035dbc719962f82dabd63f16be88f6a29500`

## SubGrupo curva ABC

Classe: `GrupoCurvaAbc`. Identificador: `Grupo.GrupoCurvaAbc`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final

**Campos apresentados (DTO):** `Subgrupo`, `QuantidadeVendida`, `TotalVenda`, `TotalCusto`, `TotalLucro`, `PercentualLucro`, `PercentualAcumulado`, `ABC`

**Contratos de serviço:**

- `IProdutoServicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IProdutoServicoRelatorioModuleService.ObterGruposCurvaAbc`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [GrupoCurvaAbc.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/GrupoCurvaAbc.cs) — SHA-256 `6806d6c51291d42865a3509dcbd2861875f452d16a25b131217c927d54f8cca4`
- [GrupoCurvaAbc.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/GrupoCurvaAbc.Designer.cs) — SHA-256 `20a075839448cae0e95dbc1ad2f242379d284b992f9d480688ce62eacb77b569`

## Vendas por dia da semana e subgrupo

Classe: `VendasPorDiaSemanaSubgrupo`. Identificador: `Grupo.VendasPorDiaSemanaSubgrupo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Subgrupo; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `DiaSemana`, `Quantidade`, `QuantidadeVendas`, `Total`, `PercentualTotal`, `OcorrenciasDia`, `TotalMedioPorDia`, `GrupoSubgrupo`, `MelhorDia`

**Agrupamentos do layout:**

- `Grupo`
- `Subgrupo`

**Contratos de serviço:**

- `IGrupoRelatorioModuleService`

**Chamadas catalogadas:**

- `IGrupoRelatorioModuleService.ObterVendasPorDiaSemanaSubgrupo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasPorDiaSemanaSubgrupo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/VendasPorDiaSemanaSubgrupo.cs) — SHA-256 `2f42ffaf930a0399aeb0cb67390b1717aa67521f9da7a022cafc6b948ac3ee57`
- [VendasPorDiaSemanaSubgrupo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/VendasPorDiaSemanaSubgrupo.Designer.cs) — SHA-256 `bb67c5dffc6ca2e2600171f4035ef55bf5292a4cab4ae1f4b6b70133b51443a2`

## Vendas por grupo

Classe: `VendasPorGrupo`. Identificador: `Grupo.VendasPorGrupo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Vendedor; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `PercentualMargem`, `MargemContribuicao`, `Markup`, `PercentualCusto`, `CustoTotal`, `PercentualTotal`, `Total`, `Quantidade`, `Grupo`

**Contratos de serviço:**

- `IGrupoRelatorioModuleService`

**Chamadas catalogadas:**

- `IGrupoRelatorioModuleService.ObterVendasPorGrupo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasPorGrupo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/VendasPorGrupo.cs) — SHA-256 `bcee745430a20ba6b8f32f89685949680afce6c4db4c2bddb3e869d4edc954bc`
- [VendasPorGrupo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/VendasPorGrupo.Designer.cs) — SHA-256 `521773cfb23008f4252571739f2f2ea6bd15e8761f5bdbc92ce93573d57a03de`

## Vendas totais por grupo

Classe: `VendasPorSubgrupo`. Identificador: `Grupo.VendasPorSubgrupo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Grupo; Data inicial; Data final; Tipo preço custo; Ordenação

**Campos apresentados (DTO):** `Subgrupo`, `Quantidade`, `Total`, `PercentualTotal`, `CustoTotal`, `PercentualCusto`, `Markup`, `MargemContribuicao`, `PercentualMargem`, `Grupo`

**Agrupamentos do layout:**

- `Grupo`

**Contratos de serviço:**

- `IGrupoRelatorioModuleService`

**Chamadas catalogadas:**

- `IGrupoRelatorioModuleService.ObterVendasPorSubgrupo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasPorSubgrupo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Grupo/VendasPorSubgrupo.cs) — SHA-256 `40ca8b0ad905a9f4f201d197bced7ed16618927bf2919fe5faf9c9e4c11e3993`
- [VendasPorSubgrupo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Grupo/VendasPorSubgrupo.Designer.cs) — SHA-256 `63ba0662434b6adbc0d9374b030712c4a91e8f823d692022d712c87b5c6e582f`

