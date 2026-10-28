# Hospedagem

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Fechamento de quartos

Classe: `FechamentoQuartos`. Identificador: `Hospedagem.FechamentoQuartos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `Quarto`, `NomeHospedes`, `DataInicial`, `DataFinal`, `TotalPrePagamento`, `TotalFinanceiro`, `TotalDiaria`, `TotalQuantidadeDiarias`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensPorDataCheckout`

**Pesquisa da regra:** Rastreado até serviço e ordenação da tela.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [FechamentoQuartos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/FechamentoQuartos.cs) — SHA-256 `3bb260644cb0d0d45e8f8d8669385a6fd117e4090264901ab583d29b84b48ec1`
- [FechamentoQuartos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/FechamentoQuartos.Designer.cs) — SHA-256 `55ec63b75c45a8776af43bc1b2697f742d4a772d7a0d83f7044069acaf1f15a2`

## Histórico de manutenções

Classe: `HistoricoManutencoes`. Identificador: `Hospedagem.HistoricoManutencoes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `MotivoManutencao`, `Quarto`, `DataFinal`, `DataInicial`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHistoricoManutencoes`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-hospedagem.md](../../docs/continuacao-hospedagem.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Period boundaries excluded by strict comparisons
- Maintenance history selected by description text

**Fontes da ficha:**

- [HistoricoManutencoes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/HistoricoManutencoes.cs) — SHA-256 `a704783362e864712dd8520b037ac471d0d4a0d75eaec7f075c34c53cc27f948`
- [HistoricoManutencoes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/HistoricoManutencoes.Designer.cs) — SHA-256 `ee5d5e13b0c5fddfc2d44930f07f8c3f05ba32d26e6a27228167d82ec0948299`

## Hospedagens canceladas

Classe: `HospedagensCanceladas`. Identificador: `Hospedagem.HospedagensCanceladas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Agrupamento

**Campos apresentados (DTO):** `Quarto`, `Situacao`, `NomeHospedes`, `DataInicial`, `DataFinal`, `DataCancelamento`, `UsuarioCancelamento`, `Observacao`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensCanceladas`

**Pesquisa da regra:** Rastreado até filtros e projeção do serviço.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [HospedagensCanceladas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/HospedagensCanceladas.cs) — SHA-256 `d6fea00187723d0c88326ba501a852b04da0c8d9e5e4ad36c023df792dec2984`
- [HospedagensCanceladas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/HospedagensCanceladas.Designer.cs) — SHA-256 `79b4a15d334e152726cc8ab82832d5fa3fd17844301bf40a9a0d339a05e10ca8`

## Hospedagens finalizadas

Classe: `HospedagensFinalizadas`. Identificador: `Hospedagem.HospedagensFinalizadas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Pessoa; Agrupamento

**Campos apresentados (DTO):** `Quarto`, `Situacao`, `NomeHospedes`, `DataInicial`, `DataFinal`, `Observacao`, `TotalHospedes`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensPorDataRelatorio`

**Pesquisa da regra:** Rastreado até filtros e projeção do serviço.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [HospedagensFinalizadas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/HospedagensFinalizadas.cs) — SHA-256 `f4e2cfddf94539c597be56aa7d4c8e3286315fbf703b7bc373903c8bd8dbd482`
- [HospedagensFinalizadas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/HospedagensFinalizadas.Designer.cs) — SHA-256 `9427e992d482f69583deea86de6cf6d817834015603c05c72da3b943517eeca1`

## Lista de quartos em atividade

Classe: `ListaQuartos`. Identificador: `Hospedagem.ListaQuartos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Situação; Agrupamento

**Campos apresentados (DTO):** `Quarto`, `Situacao`, `NomeHospedes`, `DataInicial`, `DataFinal`, `TotalPrePagamento`, `Observacao`, `TotalHospedes`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensPorDataRelatorio`

**Pesquisa da regra:** Rastreado até filtros e projeção do serviço.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaQuartos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/ListaQuartos.cs) — SHA-256 `b99dfab6bc653b4bfca7033e25618d76c8689792b859f4a3b4b0aa17d9aa31aa`
- [ListaQuartos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/ListaQuartos.Designer.cs) — SHA-256 `6a29d8bf124c0dd28e7ebc1d3a9ec97b1d9baa7b531d6c7572283cb0cd9e18e1`

## Lista de quartos por dia

Classe: `ListaQuartosPorDia`. Identificador: `Hospedagem.ListaQuartosPorDia`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data; Situação; Agrupamento; Ordenação

**Campos apresentados (DTO):** `HospedagemRelatorio.Quarto`, `HospedagemRelatorio.Situacao`, `HospedagemRelatorio.NomeHospedes`, `HospedagemRelatorio.DataInicialToText`, `HospedagemRelatorio.DataFinalToText`, `HospedagemRelatorio.Observacao`, `TotalPorSituacao.Descricao`, `TotalPorSituacao.Valor`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensRelatorioTotalPorSituacao`

**Pesquisa da regra:** Rastreado até composição e totais por situação.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaQuartosPorDia.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/ListaQuartosPorDia.cs) — SHA-256 `ad03a7cb8013f406fc255f9a55f2c069e030bde135c1419581d94da191abd39e`
- [ListaQuartosPorDia.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/ListaQuartosPorDia.Designer.cs) — SHA-256 `84c9423153908e6d43639399dbd4a96600e89d4c4e018def2fa2a8b6a253c5b0`

## Ocupação diária

Classe: `OcupacaoDiaria`. Identificador: `Hospedagem.OcupacaoDiaria`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `TotalQuantidadeDiarias`, `TotalFinanceiro`, `TotalDiaria`, `TotalPrePagamento`, `Quarto`, `Situacao`, `NomeHospedes`, `Observacao`, `DataInicial`, `DataFinal`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensPorPeriodoOcupacaoDiario`

**Pesquisa da regra:** Rastreado até filtros, cálculo e ordenação.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [OcupacaoDiaria.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/OcupacaoDiaria.cs) — SHA-256 `08b95bf8e33eff0e1df53bdec5516f77ab9c07e479885fad1d7131f85fc654d8`
- [OcupacaoDiaria.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/OcupacaoDiaria.Designer.cs) — SHA-256 `c1538bb8de0930b06181a4d629532f43245b680718fbc12cf822cad200d9b6e8`

## Ocupação diária resumido

Classe: `OcupacaoDiariaResumido`. Identificador: `Hospedagem.OcupacaoDiariaResumido`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Quarto`, `TotalQuantidadeDiarias`, `TotalPrePagamento`, `TotalFinanceiro`, `TotalDiaria`

**Agrupamentos do layout:**

- `Quarto`

**Resumos do layout:**

- `Avg`
- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensPorPeriodoOcupacaoDiario`

**Pesquisa da regra:** Rastreado até filtros, cálculo e ordenação.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [OcupacaoDiariaResumido.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/OcupacaoDiariaResumido.cs) — SHA-256 `418a141f1ce8e3378121643898007a8a310ab5f3fc94c0c77c0cc95bbe614b38`
- [OcupacaoDiariaResumido.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/OcupacaoDiariaResumido.Designer.cs) — SHA-256 `dad78e86b8ede41cf18a238a6b31cfb717c4fef291167d17f0e3fe9eae632ba4`

## Previsões das refeições

Classe: `PrevisaoRefeicao`. Identificador: `Hospedagem.PrevisaoRefeicao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `TotalHospedes`, `TotalCheckOutDia`, `Criancas`, `Cafe`, `Almoco`, `Janta`, `Adultos`, `DataInicial`

**Agrupamentos do layout:**

- `DataInicial`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterPrevisaoRefeicao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-hospedagem.md](../../docs/continuacao-hospedagem.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Period boundaries excluded by strict comparisons
- Maintenance history selected by description text

**Fontes da ficha:**

- [PrevisaoRefeicao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/PrevisaoRefeicao.cs) — SHA-256 `1d98135b7d40ec900af32bc96215e615c6306fd21e8956cb9a001650601e0982`
- [PrevisaoRefeicao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/PrevisaoRefeicao.Designer.cs) — SHA-256 `44d6d6d39513ffcab5815d08dc139fbc349acfd3af6a8f5ddf7805c1f263b938`

## Previsões de check-in

Classe: `PrevisaoCheckIn`. Identificador: `Hospedagem.PrevisaoCheckIn`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `TotalPrePagamento`, `SequenciaNumeracao`, `Situacao`, `TotalHospedes`, `DataFinal`, `NomeHospedes`, `Quarto`, `DataInicialDate`

**Agrupamentos do layout:**

- `DataInicialDate`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterPrevisaoCheckIn`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-hospedagem.md](../../docs/continuacao-hospedagem.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Period boundaries excluded by strict comparisons
- Maintenance history selected by description text

**Fontes da ficha:**

- [PrevisaoCheckIn.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/PrevisaoCheckIn.cs) — SHA-256 `e117de92c6d53af274d3ae6e66a01d9839ec17506b86975faa90d218c465ac67`
- [PrevisaoCheckIn.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/PrevisaoCheckIn.Designer.cs) — SHA-256 `c12a0bbe73d8e57a071999c8787cb118dfb63a320ee9ee75f2a2fe09bee1ad13`

## Previsões de check-out

Classe: `PrevisaoCheckOut`. Identificador: `Hospedagem.PrevisaoCheckOut`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Quarto`, `TotalPrePagamento`, `SequenciaNumeracao`, `Situacao`, `TotalHospedes`, `DataInicial`, `NomeHospedes`, `DataFinalDate`

**Agrupamentos do layout:**

- `DataFinalDate`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterPrevisaoCheckOut`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-hospedagem.md](../../docs/continuacao-hospedagem.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Period boundaries excluded by strict comparisons
- Maintenance history selected by description text

**Fontes da ficha:**

- [PrevisaoCheckOut.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/PrevisaoCheckOut.cs) — SHA-256 `f30cfe0750ccaa9d7074f872490409e010d1fd9f8bf03994bf172c613a1cf8d4`
- [PrevisaoCheckOut.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/PrevisaoCheckOut.Designer.cs) — SHA-256 `77c747f249b197a253b512fe92b720101ef5a4098b96c3aae08b21b0cc5e9672`

## Previsões de refeição por dia

Classe: `PrevisaoRefeicaoDiaria`. Identificador: `Hospedagem.PrevisaoRefeicaoDiaria`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data

**Campos apresentados (DTO):** `Hospedes.Quarto`, `Hospedes.NomeHospede`, `Hospedes.Almoco`, `Hospedes.Cafe`, `Hospedes.Janta`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterPrevisaoPorDiaReferencia`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-hospedagem.md](../../docs/continuacao-hospedagem.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Period boundaries excluded by strict comparisons
- Maintenance history selected by description text

**Fontes da ficha:**

- [PrevisaoRefeicaoDiaria.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/PrevisaoRefeicaoDiaria.cs) — SHA-256 `3cea864d1da9548f9f245f904b8401d1a0d1203160be91baab3f7d04b8d183bf`
- [PrevisaoRefeicaoDiaria.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/PrevisaoRefeicaoDiaria.Designer.cs) — SHA-256 `28f59d9329eb3b8c69c2faff631897a7f161454378de87c189919f138d5bc031`

## Quartos em aberto por pessoa

Classe: `QuartosAbertosPorPessoa`. Identificador: `Hospedagem.QuartosAbertosPorPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa; Data inicial; Data final; Situação

**Campos apresentados (DTO):** `TotalConsumido`, `Situacao`, `NomeHospedes`, `TotalDiaria`, `Quarto`, `DataFinal`, `DataInicial`, `TotalHospedes`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IHospedagemModuleService`

**Chamadas catalogadas:**

- `IHospedagemModuleService.ObterHospedagensEmAbertoPorPessoa`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-hospedagem.md](../../docs/continuacao-hospedagem.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Period boundaries excluded by strict comparisons
- Maintenance history selected by description text

**Fontes da ficha:**

- [QuartosAbertosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Hospedagem/QuartosAbertosPorPessoa.cs) — SHA-256 `8ca4d72ebf56f61d5a5004d5aea11559e183592b422d600bb76477e0362aa6d3`
- [QuartosAbertosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Hospedagem/QuartosAbertosPorPessoa.Designer.cs) — SHA-256 `e982e1f8f74903e9ba575cf51879d4f3ca302962e100a7e7a3403982a54e627f`

