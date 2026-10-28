# Agendamentos

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de agendamentos

Classe: `ListaAgendamentos`. Identificador: `Agendamento.ListaAgendamentos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Pessoa; Usuário; Motivo; Status

**Campos apresentados (DTO):** `PessoaNome`, `AnimalNomeToText`, `Telefone`, `DataHoraInicial`, `DataHoraFinal`, `Assunto`, `Observacao`, `Recorrencias.DataHora`, `Recorrencias.Descricao`, `Recorrencias.NomeUsuario`

**Contratos de serviço:**

- `IAgendamentoModuleService`

**Chamadas catalogadas:**

- `IAgendamentoModuleService.ObterListaAgendamentos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-agendamentos.md](../../docs/continuacao-agendamentos.md)

**Limites registrados na pesquisa:**

- Repository restrictions unknown
- Specification version identity not reconfirmed
- No runtime observation

**Fontes da ficha:**

- [ListaAgendamentos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Agendamento/ListaAgendamentos.cs) — SHA-256 `dda63be4f6cb67b85ed274d44a1cf16a6acc9aa334dfd2c01069f43910fbe023`
- [ListaAgendamentos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Agendamento/ListaAgendamentos.Designer.cs) — SHA-256 `ffd525cda271d389748e133f2100b8843404fed463b949e00f9fcc220bcc0992`

