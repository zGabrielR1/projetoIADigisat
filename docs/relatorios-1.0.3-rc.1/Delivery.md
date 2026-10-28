# Delivery

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Entregas efetuadas por entregador

Classe: `EntregasPorEntregador`. Identificador: `Delivery.EntregasPorEntregador`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final; Entregador; Ordenação

**Campos apresentados (DTO):** `DescricaoTipoPagamentoDelivery`, `DataEntregaEfetuada`, `Frete`, `Total`, `DataSaidaEntrega`, `DataEmissaoDav`, `Bairro`, `Pessoa`, `NumeroDav`, `EntregadorToText`, `Empresa`, `CpfCnpjEmpresa`

**Agrupamentos do layout:**

- `EntregadorToText`
- `CpfCnpjEmpresa`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Concat([CpfCnpjEntregador], ' ', [Entregador])`
- `Concat([CpfCnpjPessoa], ' ', [Pessoa])`

**Contratos de serviço:**

- `IDeliveryRelatorioModuleService`

**Chamadas catalogadas:**

- `IDeliveryRelatorioModuleService.ObterEntregasPorEntregador`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [EntregasPorEntregador.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Delivery/EntregasPorEntregador.cs) — SHA-256 `0b177a1aa1958183ffa5159c031119e2287ba0d0cf20a8d096d0cb327e660fca`
- [EntregasPorEntregador.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Delivery/EntregasPorEntregador.Designer.cs) — SHA-256 `67dada0c574b0a29d7fe6de2fbfbd4bac919a6b68ca47f563d82356359ee84b9`

## Itens lançados no Delivery

Classe: `ItensLancadosNoDelivery`. Identificador: `Delivery.ItensLancadosNoDelivery`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final; Situação das movimentações; Ordenação

**Campos apresentados (DTO):** `OutrasDespesas`, `CodigoInterno`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`, `Seriais.Serial`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Grades.Seriais.Serial`, `Lotes.Quantidade`, `Lotes.Validade`, `Lotes.Lote`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IDeliveryRelatorioModuleService`

**Chamadas catalogadas:**

- `IDeliveryRelatorioModuleService.ObterItensLancadosNoDelivery`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensLancadosNoDelivery.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Delivery/ItensLancadosNoDelivery.cs) — SHA-256 `0e389504d1f90447b36dbbc6169d055e7cdd359beb3341f9e15eb60813e4bcde`
- [ItensLancadosNoDelivery.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Delivery/ItensLancadosNoDelivery.Designer.cs) — SHA-256 `f9fd9fff4b4aff302f79798a5b22c592a2b15589a7216220fbaee804b6a65b5a`

## Itens por ambiente de produção

Classe: `EntregasProgramadas`. Identificador: `Delivery.EntregasProgramadas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial entrega programada; Data/Hora final entrega programada; Situação da entrega

**Campos apresentados (DTO):** `PessoaNome`, `DataHoraEntregaRetirada`, `CodigoInternoItem`, `Numero`, `Quantidade`, `DescricaoItem`, `Empresa`, `CpfCnpjEmpresa`, `DescricaoAmbienteProducao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `DescricaoAmbienteProducao`
- `CodigoInternoItem`

**Resumos do layout:**

- `Count`

**Contratos de serviço:**

- `IDeliveryRelatorioModuleService`

**Chamadas catalogadas:**

- `IDeliveryRelatorioModuleService.ObterItensPorAmbienteProducao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [EntregasProgramadas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Delivery/EntregasProgramadas.cs) — SHA-256 `bc63f5e491dc7e1310df8fde8b97bda88882f63203cb8346eabddbfeed2ef2a8`
- [EntregasProgramadas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Delivery/EntregasProgramadas.Designer.cs) — SHA-256 `6e5b60c952df9f99b2efc5643bcd4e7bdc2e130c2fb8c36de378fdceb28f83cd`

## Resumo das entregas efetuadas

Classe: `ResumoEntregasPorEntregador`. Identificador: `Delivery.ResumoEntregasPorEntregador`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Frete`, `Total`, `EntregadorToText`, `Empresa`, `CpfCnpjEmpresa`

**Agrupamentos do layout:**

- `EntregadorToText`
- `CpfCnpjEmpresa`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Concat([CpfCnpjEntregador], ' ', [Entregador])`
- `Concat([CpfCnpjPessoa], ' ', [Pessoa])`

**Contratos de serviço:**

- `IDeliveryRelatorioModuleService`

**Chamadas catalogadas:**

- `IDeliveryRelatorioModuleService.ObterEntregasPorEntregador`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoEntregasPorEntregador.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Delivery/ResumoEntregasPorEntregador.cs) — SHA-256 `5ca7d9983cdfbc0cca35e200dbbd276b8dd0c832120f73dd0b5a661ccd860efc`
- [ResumoEntregasPorEntregador.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Delivery/ResumoEntregasPorEntregador.Designer.cs) — SHA-256 `42e9f8b77ec66734a83ddece9477048bac855ba7542deca2d3f1cfb378d99ff1`

