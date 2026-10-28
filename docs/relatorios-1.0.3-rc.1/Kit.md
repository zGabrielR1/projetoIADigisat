# Montagem de Kits

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Itens por kit

Classe: `ItensPorKit`. Identificador: `Kit.ItensPorKit`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Kit; Tipo preço custo

**Campos apresentados (DTO):** `Itens.CustoUltimaCompraTotal`, `Itens.CustoMedioTotal`, `Itens.CustoTotal`, `Itens.totalDoItem`, `Itens.ProdutoEmpresa.Preco.PrecoVendaVigente`, `Itens.Quantidade`, `Itens.ProdutoEmpresa.ProdutoServico.Descricao`, `Itens.ProdutoEmpresa.ProdutoServico.Produto.CodigoBarras`, `Itens.ProdutoEmpresa.ProdutoServico.CodigoInterno`, `Descricao`

**Agrupamentos do layout:**

- `Id`

**Expressões calculadas do layout:**

- `Iif([Credito] == True, 'Crédito', 'Débito')`
- `Iif([Credito] == True, [Valor], 0)`
- `Iif([Credito] == True, 0, [Valor])`
- `[Quantidade]*[ProdutoEmpresa.Preco.PrecoVendaVigente]`

**Contratos de serviço:**

- `IKitModuleService`

**Chamadas catalogadas:**

- `IKitModuleService.ObterKit`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Empty kit lists all active kits
- Price/cost type is screen-only

**Fontes da ficha:**

- [ItensPorKit.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Kit/ItensPorKit.cs) — SHA-256 `b5cceb60cb92a712e3cfc7dec992f1cd3b6cc280c90310cddf652085d9a0f839`
- [ItensPorKit.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Kit/ItensPorKit.Designer.cs) — SHA-256 `16f32bfe25b04e1ab818919946f03015a6f4c2141c3cbe573af4bc8173805b56`

