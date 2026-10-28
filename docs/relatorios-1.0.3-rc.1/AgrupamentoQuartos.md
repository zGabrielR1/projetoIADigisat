# AgrupamentoQuartos

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Agrupamentos de quartos

Classe: `ListaAgrupamentosQuarto`. Identificador: `AgrupamentoQuartos.ListaAgrupamentosQuarto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Agrupamento

**Campos apresentados (DTO):** `Descricao`, `QuartosToText`, `PeriodosPrecos.PrecoPorQuantidadeHospede.QuantidadeHospede`, `PeriodosPrecos.Descricao`, `PeriodosPrecos.DataHoraVigenciaInicial`, `PeriodosPrecos.DataHoraVigenciaFinal`, `PeriodosPrecos.PrecoPorQuantidadeHospede.PrecoPeriodo`

**Agrupamentos do layout:**

- `Id`

**Contratos de serviço:**

- `IAgrupamentoQuartoModuleService`

**Chamadas catalogadas:**

- `IAgrupamentoQuartoModuleService.ObterAgrupamentosRelatorio`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-agrupamentos-hunit.md](../../docs/continuacao-agrupamentos-hunit.md)

**Limites registrados na pesquisa:**

- No runtime observation
- External service rules or additional repository restrictions remain unknown

**Fontes da ficha:**

- [ListaAgrupamentosQuarto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaAgrupamentosQuarto.cs) — SHA-256 `a190fe78ff28fcb1c4ca56c6bc7d9d59c470ea227221e57eb6fbd4cf77f2ad48`
- [ListaAgrupamentosQuarto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaAgrupamentosQuarto.Designer.cs) — SHA-256 `aeb19bd96327ce38a9f6a422106075b95e9d1152f8d8e970601900637d7418db`

## Pacotes pendentes de atualização HUNIT

Classe: `ListaPacotesPendentesAtualizacaoHunit`. Identificador: `AgrupamentoQuartos.ListaPacotesPendentesAtualizacaoHunit`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `Status`, `PortalId`, `CreateDateTime`, `Id`, `PortalName`

**Contratos de serviço:**

- `IHunitModuleService`

**Chamadas catalogadas:**

- `IHunitModuleService.ObterPendenciasAtualizacaoHunit`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-agrupamentos-hunit.md](../../docs/continuacao-agrupamentos-hunit.md)

**Limites registrados na pesquisa:**

- No runtime observation
- External service rules or additional repository restrictions remain unknown

**Fontes da ficha:**

- [ListaPacotesPendentesAtualizacaoHunit.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaPacotesPendentesAtualizacaoHunit.cs) — SHA-256 `11713664334189cac33be0fc60be5cc5699d807cab64a699139f972d18ff87cc`
- [ListaPacotesPendentesAtualizacaoHunit.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaPacotesPendentesAtualizacaoHunit.Designer.cs) — SHA-256 `bd828d2f749a99a0e8251deb95e1d12a5644bc9eb7c02dc8aaec155efb4ba1b3`

## Portais do HUNIT

Classe: `ListaPortaisIntegradosHunit`. Identificador: `AgrupamentoQuartos.ListaPortaisIntegradosHunit`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `calculatedField1`, `Id`, `Name`

**Expressões calculadas do layout:**

- `Iif([IsActive], 'Sim' , 'Não')`

**Contratos de serviço:**

- `IHunitModuleService`

**Chamadas catalogadas:**

- `IHunitModuleService.ObterPortaisIntegradosHunit`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-agrupamentos-hunit.md](../../docs/continuacao-agrupamentos-hunit.md)

**Limites registrados na pesquisa:**

- No runtime observation
- External service rules or additional repository restrictions remain unknown

**Fontes da ficha:**

- [ListaPortaisIntegradosHunit.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaPortaisIntegradosHunit.cs) — SHA-256 `759a558af3734c68cd44e3fefaa44fd2ea735f404147fb020a24fa6b95df81fd`
- [ListaPortaisIntegradosHunit.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaPortaisIntegradosHunit.Designer.cs) — SHA-256 `a2d79ac15b48c529c061a1d2ec9aa37de815f40f035d59db9edc2ffac411033d`

## Quartos do HUNIT

Classe: `ListaQuartosSincronizadosHunit`. Identificador: `AgrupamentoQuartos.ListaQuartosSincronizadosHunit`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `calculatedField1`, `Id`, `Name`

**Expressões calculadas do layout:**

- `Iif([IsActive], 'Sim', 'Não')`

**Contratos de serviço:**

- `IHunitModuleService`

**Chamadas catalogadas:**

- `IHunitModuleService.ObterQuartosSincronizadosHunit`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-agrupamentos-hunit.md](../../docs/continuacao-agrupamentos-hunit.md)

**Limites registrados na pesquisa:**

- No runtime observation
- External service rules or additional repository restrictions remain unknown

**Fontes da ficha:**

- [ListaQuartosSincronizadosHunit.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaQuartosSincronizadosHunit.cs) — SHA-256 `f39eb51f6c6866e8c5f10a388ad8ab075041592e2b5fbbaac87b63b1e8133541`
- [ListaQuartosSincronizadosHunit.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/AgrupamentoQuarto/ListaQuartosSincronizadosHunit.Designer.cs) — SHA-256 `b76f1539c1cb573f5eb287bfbf98195b59937b99bae07c7598742ded007ddb08`

