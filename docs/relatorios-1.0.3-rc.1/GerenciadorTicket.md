# Gerenciador de ticket

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Resumo do ticket

Classe: `ResumoDoTicket`. Identificador: `GerenciadorTicket.ResumoDoTicket`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ticket

**Campos apresentados (DTO):** `TicketDetalheToText`, `PeriodoVigenciaFinal`, `Situacao.Descricao`, `DataEmissao`, `Descricao`, `Referencia`, `Lancamentos.SituacaoLancamentoTicket.Codigo`, `Lancamentos.ReferenciaTicketGerado`, `Lancamentos.Observacao`, `Lancamentos.NomeUsuario`, `Lancamentos.DataHora`, `Lancamentos.SituacaoLancamentoTicket.Descricao`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `DateDiffYear([DataNascimento],LocalDateTimeNow())`

**Contratos de serviço:**

- `ITicketModuleService`

**Chamadas catalogadas:**

- `ITicketModuleService.ObterResumoTicket`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No company predicate
- Missing ticket returns null after validation

**Fontes da ficha:**

- [ResumoDoTicket.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Ticket/ResumoDoTicket.cs) — SHA-256 `e82e7d3103111c182dd9b98256f3fcf0621b2940da6b3b4df6f59c5ac540395d`
- [ResumoDoTicket.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Ticket/ResumoDoTicket.Designer.cs) — SHA-256 `35dd85b7111ce8a63db25816e8b3f1e96ae542baab770a127ac2cf699ad97cf0`

## Tickets

Classe: `TicketsPorSituacao`. Identificador: `GerenciadorTicket.TicketsPorSituacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora emissão inicial; Data/Hora emissão final; Situação; Mostrar somente vencidos; Ordenação

**Campos apresentados (DTO):** `Valor`, `PeriodoVigenciaFinal`, `Situacao.Descricao`, `DataEmissao`, `Descricao`, `Referencia`, `Validade`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `DateDiffYear([DataNascimento],LocalDateTimeNow())`

**Contratos de serviço:**

- `ITicketModuleService`

**Chamadas catalogadas:**

- `ITicketModuleService.ObterTicketsPorSituacao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No company predicate
- Date uses the raw end instant chosen on screen
- Ordering is applied in memory

**Fontes da ficha:**

- [TicketsPorSituacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Ticket/TicketsPorSituacao.cs) — SHA-256 `a4f5935788072c10cbdf6bef5b590d47a04e13cc04a60d0681ccb789e530853f`
- [TicketsPorSituacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Ticket/TicketsPorSituacao.Designer.cs) — SHA-256 `c8580225394baa73765871deb58e284d5c5dc9c94119c3e9fb90663536f7b80c`

