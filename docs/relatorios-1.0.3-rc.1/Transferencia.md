# Transferências entre contas

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de transferências entre contas

Classe: `Transferencias`. Identificador: `Transferencia.Transferencias`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data de competência inicial; Data de competência final; Conta de origem; Conta de destino; Situação

**Campos apresentados (DTO):** `Situacao.Descricao`, `Documento`, `Observacao`, `Valor`, `DataHoraCompetencia`, `Descricao`, `Empresa.Nome`, `Empresa.CpfCnpjFormatado`

**Agrupamentos do layout:**

- `Empresa.CpfCnpjFormatado`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `ITransferenciaRelatorioModuleService`

**Chamadas catalogadas:**

- `ITransferenciaRelatorioModuleService.ObterListaDeTransferencias`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [Transferencias.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Transferencia/Transferencias.cs) — SHA-256 `a23c850875c8d120888a80fef575b2e40f00e3c6cea7bd43f65f692254398b19`
- [Transferencias.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Transferencia/Transferencias.Designer.cs) — SHA-256 `dcf8b31258c848214cc7b2d555c59448a41cd9d0dcaffd3654a20e473612247d`

