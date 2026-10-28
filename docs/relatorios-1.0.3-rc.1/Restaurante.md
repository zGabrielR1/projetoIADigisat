# Restaurante

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Cardápios

Classe: `ListaCardapios`. Identificador: `Restaurante.ListaCardapios`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto contido nos cardápios

**Campos apresentados (DTO):** `Descricao`, `ItensCardapio.Id`, `ItensCardapio.ProdutoServicoEmpresa.Preco.PrecoVendaVigente`, `ItensCardapio.ProdutoServicoEmpresa.ProdutoServico.Descricao`, `ItensCardapio.ProdutoServicoEmpresa.ProdutoServico.CodigoInterno`, `ItensCardapio.DescricaoTela`, `ItensCardapio.Adicionais.ProdutoEmpresaReferencia`, `ItensCardapio.Adicionais.DescricaoTela`

**Agrupamentos do layout:**

- `Numero`
- `DescricaoTela`

**Contratos de serviço:**

- `ICardapioModuleService`

**Chamadas catalogadas:**

- `ICardapioModuleService.ObterCardapiosRelatorio`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-restaurante.md](../../docs/continuacao-restaurante.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [ListaCardapios.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/ListaCardapios.cs) — SHA-256 `c9c2062d565175277ead1846e5b69a617bd2935693e536b6bd5a882e96410fd4`
- [ListaCardapios.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/ListaCardapios.Designer.cs) — SHA-256 `bdd8177cf39e878fc9331e96a59eb0a71695c5ccbaebc3e9d96d05e64a153501`

## HistoricoBloqueioMesaConta

Classe: `HistoricoBloqueioMesaConta`. Identificador: `Restaurante.HistoricoBloqueioMesaConta`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Não recuperados na classe de apresentação.

**Campos apresentados (DTO):** `NomeUsuario`, `DataHora`, `calculatedField1`, `Numero`

**Agrupamentos do layout:**

- `Numero`

**Expressões calculadas do layout:**

- `Iif([Bloqueado], 'Bloqueio efetuado', 'Desbloqueio efetuado')`

**Contratos de serviço:**

- `IBloqueioMesaContaClienteModuleService`

**Chamadas catalogadas:**

- `IBloqueioMesaContaClienteModuleService.ObterRelatorio`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-restaurante.md](../../docs/continuacao-restaurante.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [HistoricoBloqueioMesaConta.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/HistoricoBloqueioMesaConta.cs) — SHA-256 `f7ce775047af0b33f5c66135fdc9e183140248c4e6a635d24c0ed6e59e97bc3a`
- [HistoricoBloqueioMesaConta.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/HistoricoBloqueioMesaConta.Designer.cs) — SHA-256 `ba1d06882906b03e20a4c72e7b806ff942645f13163c36a58618287f129badaf`

## Itens cancelados no período

Classe: `ItensCancelados`. Identificador: `Restaurante.ItensCancelados`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `DataHoraCanceladoDocumentoFiscalAntesFinalizacaoSemRetornoItensMesaConta`, `Total`, `DataHora`, `ValorUnitario`, `Quantidade`, `Descricao`, `Codigo`, `Garcom`, `NomeUsuarioAutorizouCancelamento`

**Agrupamentos do layout:**

- `DataHora`
- `NomeUsuarioAutorizouCancelamento`

**Expressões calculadas do layout:**

- `Iif([Cancelado] == True, 0 , [TotalAdicionais])`
- `Iif([Cancelado] == True, 0 , [ValorComissao])`
- `GetTimeOfDay([DataHoraFechamento])`
- `Iif([Cancelado] == True, 0 , [Quantidade])`
- `[Qtd] * [ValorUnitario]`
- `Iif([NumeroMesaContaPedirMenu] > 0, [NumeroMesaContaPedirMenu] ,'-' )`

**Contratos de serviço:**

- `IRestauranteModuleService`

**Chamadas catalogadas:**

- `IRestauranteModuleService.ObterInformacoesItensCancelados`

**Pesquisa da regra:** Rastreado até especificação e projeção.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [ItensCancelados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/ItensCancelados.cs) — SHA-256 `da2c3c45285616bba422f0ad3b18422d5f7ac668e076a25840c85ba90938692c`
- [ItensCancelados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/ItensCancelados.Designer.cs) — SHA-256 `d9f0620f08d3dff166e79e8585118e3cdfa0e52462078de8a3ffa465d45dfa98`

## MesasContasClientePorPeriodo

Classe: `MesasContasClientePorPeriodo`. Identificador: `Restaurante.MesasContasClientePorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final; Garçom; Cardápio; Imprimir observações; Mostrar histórico de transferência dos itens; Mostrar somente comandas do Pedir Menu

**Campos apresentados (DTO):** `Itens.Adicional`, `Itens.Comissao`, `Itens.Total`, `Itens.DataHora`, `Itens.ValorUnitario`, `Itens.Quantidade`, `Itens.Descricao`, `Itens.Codigo`, `Itens.Garcom`, `Itens.Observacoes`, `Itens.PessoaCelular`, `Itens.PessoaNome`, `Itens.PessoaCpf`, `Itens.NumeroMesa`, `Itens.NumeroMesaConta`, `Itens.Situacao`, `Itens.Qtd`, `Itens.tituloPedirMenu`, `Itens.NumeroMesaContaPedirMenuVisivel`, `Itens.Ocorrencias.TransferenciaToText`, `ItensAgrupados.Codigo`, `ItensAgrupados.Descricao`, `ItensAgrupados.Quantidade`, `ItensAgrupados.Total`

**Agrupamentos do layout:**

- `DataHora`
- `NumeroMesaConta`
- `mesaFechada`
- `Situacao`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif([Cancelado] == True, 0 , [TotalAdicionais])`
- `Iif([Cancelado] == True, 0 , [ValorComissao])`
- `GetTimeOfDay([DataHoraFechamento])`
- `Iif([Cancelado] == True, 0 , [Quantidade])`
- `[Qtd] * [ValorUnitario]`
- `Iif([NumeroMesaContaPedirMenu] > 0, [NumeroMesaContaPedirMenu] ,'' )`
- `Iif([NumeroMesaContaPedirMenu] > 0, 'Pedir menu:' ,'' )`

**Contratos de serviço:**

- `IRestauranteModuleService`

**Chamadas catalogadas:**

- `IRestauranteModuleService.ObterMesasPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-restaurante.md](../../docs/continuacao-restaurante.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [MesasContasClientePorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/MesasContasClientePorPeriodo.cs) — SHA-256 `e36b6925cd7421c6b0b5543fc433a23b08f8bdf778e8fe8f7fad88ad0a9106fd`
- [MesasContasClientePorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/MesasContasClientePorPeriodo.Designer.cs) — SHA-256 `b099e4dc47c33b41dac258a79da8e6d95d422d3b909828aa6b98592c1b9739f5`

## MesasContasClientePorPeriodoEHorario

Classe: `MesasContasClientePorPeriodoEHorario`. Identificador: `Restaurante.MesasContasClientePorPeriodoEHorario`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Hora inicial do intervalo de horário; Hora final do intervalo de horário; Garçom; Cardápio; Imprimir observações; Mostrar histórico de transferência dos itens; Mostrar somente comandas do Pedir Menu

**Campos apresentados (DTO):** `Itens.Adicional`, `Itens.Comissao`, `Itens.Total`, `Itens.DataHora`, `Itens.ValorUnitario`, `Itens.Quantidade`, `Itens.Descricao`, `Itens.Codigo`, `Itens.Garcom`, `Itens.Observacoes`, `Itens.PessoaCelular`, `Itens.PessoaNome`, `Itens.PessoaCpf`, `Itens.NumeroMesa`, `Itens.NumeroMesaConta`, `Itens.Situacao`, `Itens.Qtd`, `Itens.NumeroMesaContaPedirMenuVisivel`, `Itens.Ocorrencias.TransferenciaToText`, `ItensAgrupados.Codigo`, `ItensAgrupados.Descricao`, `ItensAgrupados.Quantidade`, `ItensAgrupados.Total`

**Agrupamentos do layout:**

- `DataHora`
- `NumeroMesaConta`
- `mesaFechada`
- `Situacao`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif([Cancelado] == True, 0 , [TotalAdicionais])`
- `Iif([Cancelado] == True, 0 , [ValorComissao])`
- `GetTimeOfDay([DataHoraFechamento])`
- `Iif([Cancelado] == True, 0 , [Quantidade])`
- `[Qtd] * [ValorUnitario]`
- `Iif([NumeroMesaContaPedirMenu] > 0, [NumeroMesaContaPedirMenu] ,'-' )`

**Contratos de serviço:**

- `IRestauranteModuleService`

**Chamadas catalogadas:**

- `IRestauranteModuleService.ObterMesasPorPeriodoEHorario`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-restaurante.md](../../docs/continuacao-restaurante.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [MesasContasClientePorPeriodoEHorario.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/MesasContasClientePorPeriodoEHorario.cs) — SHA-256 `056eb81998712ea3f5e484cbb9372417ad93c60ed90fe2abc15fd0268f3a9402`
- [MesasContasClientePorPeriodoEHorario.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/MesasContasClientePorPeriodoEHorario.Designer.cs) — SHA-256 `9766428fe3beff7820490f2cf7838a2ba7a95f066a851c17af347396124734e2`

## MesasContasClientePorSituacao

Classe: `MesasContasClientePorSituacao`. Identificador: `Restaurante.MesasContasClientePorSituacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Total`, `DataHora`, `PessoaCelular`, `PessoaCpf`, `NumeroMesa`, `NumeroMesaConta`, `PessoaNome`, `calculatedField1`, `Situacao`

**Agrupamentos do layout:**

- `DataHora`
- `Situacao`

**Resumos do layout:**

- `Count`

**Contratos de serviço:**

- `IRestauranteModuleService`

**Chamadas catalogadas:**

- `IRestauranteModuleService.ObterMesasPorSituacao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-restaurante.md](../../docs/continuacao-restaurante.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [MesasContasClientePorSituacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/MesasContasClientePorSituacao.cs) — SHA-256 `06145011a731902cc09d50234b5f3e2c817c1c17dcec2eafb4ffc634806d7bbb`
- [MesasContasClientePorSituacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/MesasContasClientePorSituacao.Designer.cs) — SHA-256 `90fcbbfef1478c82d98676e89841afdee489b8de4980d08ae4a5a1f10d721c1f`

## Tempo de permanência/consumo

Classe: `TempoDePermanenciaEConsumo`. Identificador: `Restaurante.TempoDePermanenciaEConsumo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final

**Campos apresentados (DTO):** `Permanencia`, `DataHoraFechamento`, `Total`, `DataHoraAbertura`, `PessoaCelular`, `PessoaCpf`, `NumeroMesaConta`, `PessoaNome`

**Agrupamentos do layout:**

- `DataHora`

**Resumos do layout:**

- `Avg`
- `Count`

**Contratos de serviço:**

- `IRestauranteModuleService`

**Chamadas catalogadas:**

- `IRestauranteModuleService.ObterTempoDePermanenciaEConsumo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-restaurante.md](../../docs/continuacao-restaurante.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [TempoDePermanenciaEConsumo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/TempoDePermanenciaEConsumo.cs) — SHA-256 `ae3865b2fc7956ec1d3b7b968e03f0f29b52a092ca8ceb0cea57706a930b78bd`
- [TempoDePermanenciaEConsumo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/TempoDePermanenciaEConsumo.Designer.cs) — SHA-256 `b2710a346001cf2e3656c47521e5ece0c3dad0ff673e42da356ee8eb1e0fbe53`

## Vendas por cardápio

Classe: `VendasPorCardapio`. Identificador: `Restaurante.VendasPorCardapio`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final; Cardápio; Mostrar detalhes

**Campos apresentados (DTO):** `ItemVendido.Total`, `ItemVendido.Desconto`, `ItemVendido.Quantidade`, `ItemVendido.NumeroItensVendidos`, `ItemVendido.PrecoVenda`, `ItemVendido.Descricao`, `ItemVendido.CodigoInterno`, `DetalhesMovimentacao`, `ItemVendido.Cardapio`, `Adicionais.CodigoInterno`, `Adicionais.Descricao`, `Adicionais.PrecoVenda`, `Adicionais.NumeroItensVendidos`, `Adicionais.Quantidade`, `Adicionais.Desconto`, `Adicionais.Total`, `TotalGeral`

**Agrupamentos do layout:**

- `Descricao`
- `Cardapio`
- `CardapioReferencia`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosPorCardapio`

**Pesquisa da regra:** Rastreado até consulta, agrupamento e detalhes.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasPorCardapio.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Restaurante/VendasPorCardapio.cs) — SHA-256 `f3899421dd1c9d675b435a0e79ed7c521847b05099042e276133b66c827af0bb`
- [VendasPorCardapio.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Restaurante/VendasPorCardapio.Designer.cs) — SHA-256 `a229b047c271ff3a8ed7cac79f7aa6b79b6e20a8aa8a992574a0340d9f31b08a`

