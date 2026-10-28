# Promoção

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Itens por promoção

Classe: `ItensPorPromocao`. Identificador: `Promocao.ItensPorPromocao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Promoção; Ordenação

**Campos apresentados (DTO):** `Itens.Preco`, `Itens.CodigoInterno`, `Itens.CodigoBarras`, `Itens.Descricao`, `Itens.PrecoPromocional`

**Expressões calculadas do layout:**

- `Iif([Credito] == True, 'Crédito', 'Débito')`
- `Iif([Credito] == True, [Valor], 0)`
- `Iif([Credito] == True, 0, [Valor])`

**Contratos de serviço:**

- `IPromocaoModuleService`

**Chamadas catalogadas:**

- `IPromocaoModuleService.ObterPromocao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Promotion read has no company predicate
- Missing price reference can raise NullReferenceException

**Fontes da ficha:**

- [ItensPorPromocao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Promocao/ItensPorPromocao.cs) — SHA-256 `6e01785dab127d0e53567875b0613943a1b52215b27dba7a7a398b42561f2fd7`
- [ItensPorPromocao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Promocao/ItensPorPromocao.Designer.cs) — SHA-256 `6708cd84e6a1b8f42a96164ba9d944099a42d9bcd09fa26df5b2e66b8e03ecf0`

