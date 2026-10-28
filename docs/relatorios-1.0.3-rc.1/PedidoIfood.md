# Pedidos do iFood

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Pedidos iFood por período

Classe: `PedidosIfoodPorPeriodo`. Identificador: `PedidoIfood.PedidosIfoodPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Situação; Data/Hora inicial; Data/Hora final; Exibir detalhes dos itens; Exibir quantidade de registros por situação

**Campos apresentados (DTO):** `Movimentacoes.Situacao`, `Movimentacoes.Total`, `Movimentacoes.TotalProdutos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Total`, `ItensAcumulados.Quantidade`, `ItensAcumulados.Descricao`, `ResumoSituacoes.Situacao`, `ResumoSituacoes.Quantidade`

**Contratos de serviço:**

- `IPedidoIfoodModuleService`

**Chamadas catalogadas:**

- `IPedidoIfoodModuleService.PedidoIfoodPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Period is the iFood order creation date
- Totals come from the iFood payload, not local item recalculation
- displayId parse may fail for non-numeric ids

**Fontes da ficha:**

- [PedidosIfoodPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PedidoIfood/PedidosIfoodPorPeriodo.cs) — SHA-256 `c8fd5f89d48d632529522ebee188d2e6ac6cac265cbc1e57ce96ed6a3e6b2c77`
- [PedidosIfoodPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PedidoIfood/PedidosIfoodPorPeriodo.Designer.cs) — SHA-256 `714ee99b6e2f68c372d00970fdd50fe9f82acf1959183f4e1254c28dd527f4d6`

