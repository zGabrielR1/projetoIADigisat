# Turno

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Conferência de turno

Classe: `ConferenciaTurno`. Identificador: `Turno.ConferenciaTurno`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Turno; Mostrar itens; Mostrar detalhamento das parcelas recebidas; Mostrar detalhamento das parcelas nas vendas a prazo

**Campos apresentados (DTO):** `Usuario`, `DataHoraAbertura`, `DataHoraFechamento`, `TotalOutrasDespesasAplicado.TotalOutrasDespesas`, `TotalCanceladasTurno.Quantidade`, `TotalCanceladasTurno.TotalLiquido`, `TotalDescontoAplicado.TotalDesconto`, `TotalCanceladasTurno.TotalBruto`, `EspeciesRecebimento.Descricao`, `EspeciesRecebimento.ValorCalculado`, `EspeciesRecebimento.ValorInformado`, `EspeciesRecebimento.Diferenca`, `ParcelamentosReceber.Descricao`, `ParcelamentosReceber.ValorCalculado`, `ParcelamentosPagar.Descricao`, `ParcelamentosPagar.ValorCalculado`, `Movimentacoes.Descricao`, `Movimentacoes.ValorCalculado`, `TotalDescontosConvenios.TotalLiquido`, `TotalDescontosConvenios.TotalDescontoConvenio`, `TotalDescontosConvenios.TotalBruto`, `RecebimentosDeMovimentacoesAVista.Descricao`, `RecebimentosDeMovimentacoesAVista.ValorCalculado`, `Recebimentos.Descricao`, `Recebimentos.ValorCalculado`, `Pagamentos.Descricao`, `Pagamentos.ValorCalculado`, `MovimentacoesAPrazo.NumeroParcelas`, `MovimentacoesAPrazo.ValorVista`, `MovimentacoesAPrazo.ValorPrazo`, `MovimentacoesAPrazo.Total`, `MovimentacoesAPrazo.Numero`, `MovimentacoesAPrazo.Pessoa`, `MovimentacoesAPrazo.Parcelas.Descricao`, `MovimentacoesAPrazo.Parcelas.Valor`, `MovimentacoesAPrazo.Parcelas.Vencimento`, `MovimentacoesAPrazo.Parcelas.Ordem`, `MovimentacoesAPrazo.Parcelas.Numero`, `ChequesRecebidos.DataPreDatado`, `ChequesRecebidos.Valor`, `ChequesRecebidos.Numero`, `ChequesRecebidos.Conta`, `ChequesRecebidos.Agencia`, `ChequesRecebidos.Banco`, `ItensVendidos.Descricao`, `ItensVendidos.Barras`, `ItensVendidos.Codigo`, `ItensVendidos.Quantidade`, `ItensVendidos.Valor`, `ItensComprados.Valor`, `ItensComprados.Quantidade`, `ItensComprados.Codigo`, `ItensComprados.Barras`, `ItensComprados.Descricao`, `PagamentosOutrasContas.Descricao`, `PagamentosOutrasContas.ValorCalculado`, `Abastecimentos.TotalLiquidoFaturado`, `Abastecimentos.DescontoAplicado`, `Abastecimentos.Valor`, `Abastecimentos.Quantidade`, `Abastecimentos.NumeroBico`, `Abastecimentos.afericaoToText`, `Abastecimentos.Descricao`, `Encerrantes.Volume`, `Encerrantes.EncerranteFinal`, `Encerrantes.EncerranteInicial`, `Encerrantes.NumeroBico`, `EstoquesFechamento.Descricao`, `EstoquesFechamento.QuantidadeSaldo`, `RedeCartaoRecebidos.Valor`, `RedeCartaoRecebidos.Rede`, `ParcelasRecebidas.ValorQuitado`, `ParcelasRecebidas.ValorOriginal`, `ParcelasRecebidas.DataEmissao`, `ParcelasRecebidas.DataVencimento`, `ParcelasRecebidas.Pessoa`, `ParcelasRecebidas.Ordem`, `ParcelasRecebidas.Documento`, `ParcelasGeradasDeRecebimentosParciais.Documento`, `ParcelasGeradasDeRecebimentosParciais.Ordem`, `ParcelasGeradasDeRecebimentosParciais.Pessoa`, `ParcelasGeradasDeRecebimentosParciais.DataVencimento`, `ParcelasGeradasDeRecebimentosParciais.DataEmissao`, `ParcelasGeradasDeRecebimentosParciais.ValorOriginal`, `CarteiraDigitalPorTipo.Tipo`, `CarteiraDigitalPorTipo.Valor`

**Expressões calculadas do layout:**

- `Iif([PreDatado]  == '01/01/0001 00:00:00', '', [PreDatado])`
- `Iif([Afericao], 'SIM' , 'NÃO' )`

**Contratos de serviço:**

- `ITurnoRelatorioModuleService`

**Chamadas catalogadas:**

- `ITurnoRelatorioModuleService.ObterConferenciaTurno`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ConferenciaTurno.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Turno/ConferenciaTurno.cs) — SHA-256 `a9503d0fb7021d7cde91823ae4f509aa7cb32b48b7730444fc75dd0dc4050176`
- [ConferenciaTurno.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Turno/ConferenciaTurno.Designer.cs) — SHA-256 `af89058a7f9369aeeeb08cbe096ca775fecc1fc18b3a2e17b42e4231cd257b24`

## Conferência de turno - 40 colunas

Classe: `ConferenciaTurno40Colunas`. Identificador: `Turno.ConferenciaTurno40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Turno; Mostrar itens

**Campos apresentados (DTO):** `Usuario`, `DataHoraAbertura`, `DataHoraFechamento`, `TotalOutrasDespesasAplicado.TotalOutrasDespesas`, `TotalCanceladasTurno.TotalLiquido`, `TotalDescontoAplicado.TotalDesconto`, `TotalCanceladasTurno.TotalBruto`, `TotalCanceladasTurno.Quantidade`, `EspeciesRecebimento.Descricao`, `EspeciesRecebimento.ValorCalculado`, `EspeciesRecebimento.ValorInformado`, `EspeciesRecebimento.Diferenca`, `ParcelamentosReceber.Descricao`, `ParcelamentosReceber.ValorCalculado`, `ParcelamentosPagar.Descricao`, `ParcelamentosPagar.ValorCalculado`, `Movimentacoes.Descricao`, `Movimentacoes.ValorCalculado`, `TotalDescontosConvenios.TotalLiquido`, `TotalDescontosConvenios.TotalDescontoConvenio`, `TotalDescontosConvenios.TotalBruto`, `RecebimentosDeMovimentacoesAVista.Descricao`, `RecebimentosDeMovimentacoesAVista.ValorCalculado`, `Recebimentos.Descricao`, `Recebimentos.ValorCalculado`, `Pagamentos.Descricao`, `Pagamentos.ValorCalculado`, `MovimentacoesAPrazo.NumeroParcelas`, `MovimentacoesAPrazo.ValorVista`, `MovimentacoesAPrazo.ValorPrazo`, `MovimentacoesAPrazo.Total`, `MovimentacoesAPrazo.Numero`, `MovimentacoesAPrazo.Pessoa`, `ChequesRecebidos.DataPreDatado`, `ChequesRecebidos.Valor`, `ChequesRecebidos.Numero`, `ChequesRecebidos.Conta`, `ChequesRecebidos.Agencia`, `ChequesRecebidos.Banco`, `ItensVendidos.Descricao`, `ItensVendidos.Codigo`, `ItensVendidos.Quantidade`, `ItensVendidos.Valor`, `ItensComprados.Quantidade`, `ItensComprados.Codigo`, `ItensComprados.Descricao`, `ItensComprados.Valor`, `PagamentosOutrasContas.Descricao`, `PagamentosOutrasContas.ValorCalculado`, `Abastecimentos.TotalLiquidoFaturado`, `Abastecimentos.DescontoAplicado`, `Abastecimentos.Valor`, `Abastecimentos.Quantidade`, `Abastecimentos.NumeroBico`, `Abastecimentos.afericaoToText`, `Abastecimentos.Descricao`, `Encerrantes.Volume`, `Encerrantes.EncerranteFinal`, `Encerrantes.EncerranteInicial`, `Encerrantes.NumeroBico`, `EstoquesFechamento.Descricao`, `EstoquesFechamento.QuantidadeSaldo`, `RedeCartaoRecebidos.Rede`, `RedeCartaoRecebidos.Valor`, `CarteiraDigitalPorTipo.Tipo`, `CarteiraDigitalPorTipo.Valor`

**Expressões calculadas do layout:**

- `Iif([PreDatado]  == '01/01/0001 00:00:00', '', [PreDatado])`
- `Iif([Afericao], 'SIM' , 'NÃO' )`

**Contratos de serviço:**

- `ITurnoRelatorioModuleService`

**Chamadas catalogadas:**

- `ITurnoRelatorioModuleService.ObterConferenciaTurno`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ConferenciaTurno40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Turno/ConferenciaTurno40Colunas.cs) — SHA-256 `e7d7f45c92475680778888c8347598e0fade254b828106a688920c808eb79e0b`
- [ConferenciaTurno40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Turno/ConferenciaTurno40Colunas.Designer.cs) — SHA-256 `89a231defa3236a0a2c2250eb05f3f5050ed721dbc5e83f6a5ed8f1dfde3a94d`

## Conferência de turno sintético

Classe: `ConferenciaTurnoSintetico`. Identificador: `Turno.ConferenciaTurnoSintetico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Turno; Mostrar itens; Mostrar lista de vendas a prazo

**Campos apresentados (DTO):** `Usuario`, `DataHoraAbertura`, `DataHoraFechamento`, `TotalOutrasDespesasAplicado.TotalOutrasDespesas`, `TotalCanceladasTurno.Quantidade`, `TotalCanceladasTurno.TotalLiquido`, `TotalDescontoAplicado.TotalDesconto`, `TotalCanceladasTurno.TotalBruto`, `ParcelamentosReceber.Descricao`, `ParcelamentosReceber.ValorCalculado`, `MovimentacoesPorTipo.Descricao`, `MovimentacoesPorTipo.ValorCalculado`, `RecebimentosDeMovimentacoesAVista.Descricao`, `RecebimentosDeMovimentacoesAVista.ValorCalculado`, `Recebimentos.Descricao`, `Recebimentos.ValorCalculado`, `Pagamentos.ContaLivroCaixaToText`, `Pagamentos.Descricao`, `Pagamentos.ValorCalculado`, `MovimentacoesAPrazo.NumeroParcelas`, `MovimentacoesAPrazo.ValorVista`, `MovimentacoesAPrazo.ValorPrazo`, `MovimentacoesAPrazo.Total`, `MovimentacoesAPrazo.Numero`, `MovimentacoesAPrazo.Pessoa`, `ItensVendidos.Descricao`, `ItensVendidos.Barras`, `ItensVendidos.Codigo`, `ItensVendidos.Quantidade`, `ItensVendidos.Valor`, `TotalEmCaixa`, `LancamentosManuais.DataHoraCompetencia`, `LancamentosManuais.Historico`, `LancamentosManuais.Descricao`, `LancamentosManuais.Documento`, `LancamentosManuais.Valor`

**Expressões calculadas do layout:**

- `Iif([PreDatado]  == '01/01/0001 00:00:00', '', [PreDatado])`
- `Iif([Afericao], 'SIM' , 'NÃO' )`
- `Iif([ContaLivroCaixa], 'Saiu do livro caixa' , '')`

**Contratos de serviço:**

- `ITurnoRelatorioModuleService`

**Chamadas catalogadas:**

- `ITurnoRelatorioModuleService.ObterConferenciaTurnoSintetico`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ConferenciaTurnoSintetico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Turno/ConferenciaTurnoSintetico.cs) — SHA-256 `db448f755024031dbaebd881f661f2e2d61c91ce9d601181ecce12665180a85c`
- [ConferenciaTurnoSintetico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Turno/ConferenciaTurnoSintetico.Designer.cs) — SHA-256 `12b22501e59facf7174392d2f56323cf0c4b0e4548f00c395532e2f276018e1a`

## Conferência faltas e sobras por período

Classe: `ConferenciaFaltasSobrasPeriodo`. Identificador: `Turno.ConferenciaFaltasSobrasPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Operador; Data/Hora inicial; Data/Hora final; Mostrar somente conferências com diferenças

**Campos apresentados (DTO):** `DataHoraAbertura`, `EspeciesRecebimento.Descricao`, `EspeciesRecebimento.ValorCalculado`, `EspeciesRecebimento.ValorInformado`, `EspeciesRecebimento.Diferenca`

**Expressões calculadas do layout:**

- `Iif([PreDatado]  == '01/01/0001 00:00:00', '', [PreDatado])`
- `Iif([Afericao], 'SIM' , 'NÃO' )`

**Contratos de serviço:**

- `ITurnoRelatorioModuleService`

**Chamadas catalogadas:**

- `ITurnoRelatorioModuleService.ObterListaConferenciaTurno`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ConferenciaFaltasSobrasPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Turno/ConferenciaFaltasSobrasPeriodo.cs) — SHA-256 `b5d35e8a2f082b77e8e64c9753cc649244cfeff9b178c54d8d98b07d3397a48a`
- [ConferenciaFaltasSobrasPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Turno/ConferenciaFaltasSobrasPeriodo.Designer.cs) — SHA-256 `22e89602c50288b22cec75246ad0112f70b63a28acb53322daac60c7eaf935aa`

## Resumo da Jornada

Classe: `ResumoJornada`. Identificador: `Turno.ResumoJornada`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Jornada

**Campos apresentados (DTO):** `DataFechamento`, `DataAbertura`, `ItensVendidos.Total`, `ItensVendidos.Desconto`, `ItensVendidos.Preco`, `ItensVendidos.QuantidadeVendida`, `ItensVendidos.Descricao`, `ItensVendidos.CodigoInterno`, `Turnos.DataHoraFechamento`, `Turnos.DataHoraAbertura`, `Turnos.Usuario`, `Pagamentos.EspeciePagamento`, `Pagamentos.ValorQuitado`, `Pagamentos.Valor`, `Pagamentos.DataQuitado`, `Pagamentos.Vencimento`, `Pagamentos.Ordem`, `Pagamentos.Numero`, `Recebimentos.Numero`, `Recebimentos.Ordem`, `Recebimentos.Vencimento`, `Recebimentos.DataQuitado`, `Recebimentos.Valor`, `Recebimentos.ValorQuitado`, `Recebimentos.EspeciePagamento`, `PagamentosERecebimentos.Valor`, `PagamentosERecebimentos.ValorQuitado`, `EspeciesRecebimento.Valor`, `EspeciesRecebimento.Especie`, `Encerrantes.EncerranteInicialJornada`, `Encerrantes.EncerranteFinalJornada`, `Encerrantes.EncerranteInicial`, `Encerrantes.EncerranteFinal`, `Encerrantes.TotalLitrosJornada`, `Encerrantes.TotalLitros`, `Encerrantes.Afericao`, `Encerrantes.Preco`, `Encerrantes.Combustivel`, `Encerrantes.Bico`, `Encerrantes.Bomba`, `Encerrantes.Tanque`, `ParcelamentosPagar.Vencimento`, `ParcelamentosPagar.Valor`, `ParcelamentosPagar.Ordem`, `ParcelamentosPagar.EspeciePagamento`, `ParcelamentosPagar.Numero`, `ParcelamentosReceber.Numero`, `ParcelamentosReceber.Ordem`, `ParcelamentosReceber.EspeciePagamento`, `ParcelamentosReceber.Vencimento`, `ParcelamentosReceber.Valor`

**Contratos de serviço:**

- `IJornadaRelatorioModuleService`

**Chamadas catalogadas:**

- `IJornadaRelatorioModuleService.ObterResumoJornada`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoJornada.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Turno/ResumoJornada.cs) — SHA-256 `c39deb9bbcbfee8f201cad161aeb675a32f198363e60deaaa61b11337f5ba940`
- [ResumoJornada.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Turno/ResumoJornada.Designer.cs) — SHA-256 `c7f50c98994274c567b75351902e382793e937a59975c9d847598b34a92bb436`

## Sangrias e Suprimentos

Classe: `SangriasSuprimentosPeriodo`. Identificador: `Turno.SangriasSuprimentosPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Observacao`, `DataHora`, `Valor`, `Especie`, `Tipo`, `Usuario`

**Agrupamentos do layout:**

- `Usuario`
- `Tipo`

**Expressões calculadas do layout:**

- `Iif([PreDatado]  == '01/01/0001 00:00:00', '', [PreDatado])`

**Contratos de serviço:**

- `ITurnoRelatorioModuleService`

**Chamadas catalogadas:**

- `ITurnoRelatorioModuleService.ObterSangriasSuprimentosPeriodo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [SangriasSuprimentosPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Turno/SangriasSuprimentosPeriodo.cs) — SHA-256 `c7da3a5dd973d4b99b85ba5b357101b2723515808165c4dc9cab779744f46d31`
- [SangriasSuprimentosPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Turno/SangriasSuprimentosPeriodo.Designer.cs) — SHA-256 `a148a7c76e609c97749fac26b007574124f3b0acd00ac58e16473efcae10ef97`

## Sangrias e Suprimentos - 40 colunas

Classe: `SangriasSuprimentosPeriodo40Colunas`. Identificador: `Turno.SangriasSuprimentosPeriodo40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Valor`, `Especie`, `Observacao`, `DataHora`, `Usuario`, `Tipo`

**Agrupamentos do layout:**

- `Usuario`
- `Tipo`

**Contratos de serviço:**

- `ITurnoRelatorioModuleService`

**Chamadas catalogadas:**

- `ITurnoRelatorioModuleService.ObterSangriasSuprimentosPeriodo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [SangriasSuprimentosPeriodo40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Turno/SangriasSuprimentosPeriodo40Colunas.cs) — SHA-256 `b00699634fe6b9de92518fd319cdc2838665e1c3450efffb25a2ebc9fa8032fb`
- [SangriasSuprimentosPeriodo40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Turno/SangriasSuprimentosPeriodo40Colunas.Designer.cs) — SHA-256 `7c515f1d591b2d6d001731cd26bc03df0aee4dc8ea39af0090ec9ad9371cefab`

