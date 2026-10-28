# Movimento Conta

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Fluxo da conta

Classe: `FluxoConta`. Identificador: `MovimentoConta.FluxoConta`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Conta; Data inicial; Data final

**Campos apresentados (DTO):** `SaldoFinal`, `Receber`, `Pagar`, `SaldoAtual`, `Recebimentos.ValorTotal`, `Recebimentos.DataVencimento`, `Recebimentos.Pessoa.Nome`, `Recebimentos.documentoOrdem`, `Pagamentos.documentoOrdemPagar`, `Pagamentos.Pessoa.Nome`, `Pagamentos.DataVencimento`, `Pagamentos.ValorTotal`

**Expressões calculadas do layout:**

- `Concat([Documento], ' - ', [Ordem])`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterFluxoConta`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [FluxoConta.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/FluxoConta.cs) — SHA-256 `709f1506b9d359f980071962b640bdf1c7ccefbde9df34f4e68da74b213d49f2`
- [FluxoConta.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/FluxoConta.Designer.cs) — SHA-256 `4ea1ba94e9ba7cd1e755fa22350c591c3d55adc142c34aef170dc1aeb3538019`

## Livro caixa

Classe: `LivroCaixa`. Identificador: `MovimentoConta.LivroCaixa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Conta; Plano de conta; Centro de custo; Ordenação; Quanto às estornadas

**Campos apresentados (DTO):** `Conta`, `SaldoAtual`, `SaldoFinal`, `SaldoInicial`, `MovimentosConta.saida`, `MovimentosConta.entrada`, `MovimentosConta.Descricao`, `MovimentosConta.DataHoraCompetencia`, `MovimentosConta.pessoaNome`, `MovimentosConta.Saldo`, `MovimentosConta.CentroCusto.ToText`, `MovimentosConta.PlanoConta.ToText`

**Expressões calculadas do layout:**

- `Iif([CreditoDebitoItemSelecionado] == 1, [Valor], 0)`
- `Iif([CreditoDebitoItemSelecionado] == 1, 0, [Valor])`
- `Iif([Pessoa.Nome] != '', Concat('Pessoa: ', [Pessoa.Nome]), '')`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterLivroCaixa`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [LivroCaixa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixa.cs) — SHA-256 `6ebfa3a36d7923370edaff8fd22ef562815f250ca8efea6d7efdeb4c9e73348f`
- [LivroCaixa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixa.Designer.cs) — SHA-256 `cca3c9572e55a5c7f7786c1a01a01bd6d6f0eeb6167a388efa81dd8678e4210a`

## Livro caixa - 40 Colunas

Classe: `LivroCaixa40Colunas`. Identificador: `MovimentoConta.LivroCaixa40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Conta; Plano de conta; Centro de custo; Ordenação; Tipo; Quanto às estornadas

**Campos apresentados (DTO):** `Conta`, `SaldoAtual`, `SaldoFinal`, `SaldoInicial`, `MovimentosConta.saida`, `MovimentosConta.entrada`, `MovimentosConta.Descricao`, `MovimentosConta.DataHoraCompetencia`, `MovimentosConta.pessoaNome`, `MovimentosConta.Saldo`, `MovimentosConta.CentroCusto.ToText`, `MovimentosConta.PlanoConta.ToText`

**Expressões calculadas do layout:**

- `Iif([CreditoDebitoItemSelecionado] == 1, [Valor], 0)`
- `Iif([CreditoDebitoItemSelecionado] == 1, 0, [Valor])`
- `Iif([Pessoa.Nome] != '', [Pessoa.Nome], '')`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterLivroCaixa`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [LivroCaixa40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixa40Colunas.cs) — SHA-256 `bb02f89c21b710ba56bd47cf75124bbafb511c9a715006615d040935badb996b`
- [LivroCaixa40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixa40Colunas.Designer.cs) — SHA-256 `0eb7d3240c8eb3859057f75c845f849e53aa9d209e69650560d28a7908440c27`

## Livro caixa com dados da espécie de pagamento

Classe: `LivroCaixaComDadosEspeciePagamento`. Identificador: `MovimentoConta.LivroCaixaComDadosEspeciePagamento`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Conta; Ordenação; Quanto às estornadas

**Campos apresentados (DTO):** `Conta`, `SaldoAtual`, `SaldoFinal`, `SaldoInicial`, `MovimentosConta.saida`, `MovimentosConta.entrada`, `MovimentosConta.Descricao`, `MovimentosConta.DataHoraCompetencia`, `MovimentosConta.NumeroCheque`, `MovimentosConta.Especie`, `MovimentosConta.pessoaNome`, `MovimentosConta.Saldo`

**Expressões calculadas do layout:**

- `Iif([CreditoDebitoItemSelecionado] == 1, [Valor], 0)`
- `Iif([CreditoDebitoItemSelecionado] == 1, 0, [Valor])`
- `Iif([Pessoa.Nome] != '', Concat('Pessoa: ', [Pessoa.Nome]), '')`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterLivroCaixaComDadosEspeciePagamento`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [LivroCaixaComDadosEspeciePagamento.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixaComDadosEspeciePagamento.cs) — SHA-256 `fd6cdc96013973f4fac1757c944f2e799b48426f703329f57f4c9b69aa63249a`
- [LivroCaixaComDadosEspeciePagamento.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixaComDadosEspeciePagamento.Designer.cs) — SHA-256 `bfb0b5e9f570f28f4ed9d412efe01dcf91fcf8947960b57bc7ffb4e8328dc9d0`

## Livro caixa resumo mensal

Classe: `LivroCaixaResumoMensal`. Identificador: `MovimentoConta.LivroCaixaResumoMensal`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Conta

**Campos apresentados (DTO):** `MesAno`, `UltimoDiaMes`, `PrimeiroDiaMes`, `SaldoFinal`, `SaldoInicial`, `Conta`, `SaldoAtual`

**Agrupamentos do layout:**

- `Conta`

**Expressões calculadas do layout:**

- `Iif([CreditoDebitoItemSelecionado] == 1, [Valor], 0)`
- `Iif([CreditoDebitoItemSelecionado] == 1, 0, [Valor])`
- `Iif([Pessoa.Nome] != '', Concat('Pessoa: ', [Pessoa.Nome]), '')`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterResumoMensalLivroCaixa`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [LivroCaixaResumoMensal.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixaResumoMensal.cs) — SHA-256 `f5302688dbe3d50ff023e700af6d5f7475730d42f82571b2cb7cbc42414228a2`
- [LivroCaixaResumoMensal.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/LivroCaixaResumoMensal.Designer.cs) — SHA-256 `a1784db121f108be00841acea283a36fb4f6db8878e181a3c7ffba62b5f3f052`

## Movimentações por conta

Classe: `ContaMovimentacao`. Identificador: `MovimentoConta.ContaMovimentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Conta; Data inicial; Data final; Quanto às estornadas; Tipo de movimento

**Campos apresentados (DTO):** `Movimentos.Pessoa.NomeDocumentoToText`, `Movimentos.PlanoConta.Descricao`, `Movimentos.CentroCusto.Descricao`, `Movimentos.NomeEmpresa`, `Movimentos.Data`, `Movimentos.origem`, `Movimentos.HistoricoToText`, `Movimentos.entrada`, `Movimentos.saída`, `Saldo`, `SaldoPeriodo`, `SaldoAnterior`, `DebitoPeriodo`, `CreditoPeriodo`

**Expressões calculadas do layout:**

- `Iif([Credito] == True, 'Crédito', 'Débito')`
- `Iif([Credito] == True, [Valor], 0)`
- `Iif([Credito] == True, 0, [Valor])`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterContaMovimentacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ContaMovimentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/ContaMovimentacao.cs) — SHA-256 `1f0d960d47a2777e9f68d24edb8c25779c944e338bdc52adbab1924655912dd9`
- [ContaMovimentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/ContaMovimentacao.Designer.cs) — SHA-256 `be6c33f563e55acb54854d0203bed7de1005f3316b1bb1adf05721c5e34894e5`

## Resumo diário

Classe: `ResumoDiario`. Identificador: `MovimentoConta.ResumoDiario`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data; Tipo

**Campos apresentados (DTO):** `Movimentacoes.Total`, `Movimentacoes.Numero`, `Movimentacoes.NomePessoa`, `Movimentacoes.TipoMovimentacao`, `Movimentacoes.TotalAPrazo`, `Movimentacoes.TotalAVista`, `Movimentacoes.TipoMovimento`, `Movimentacoes.finalidadeMovimentacao`, `Recebimentos.ValorQuitado`, `Recebimentos.DataVencimento`, `Recebimentos.DataInclusao`, `Recebimentos.Pessoa`, `Recebimentos.Ordem`, `Recebimentos.Documento`, `Pagamentos.Documento`, `Pagamentos.Ordem`, `Pagamentos.Pessoa.Nome`, `Pagamentos.DataEmissao`, `Pagamentos.DataVencimento`, `Pagamentos.ValorQuitacao`

**Agrupamentos do layout:**

- `TipoMovimentacao`
- `TipoMovimento`
- `finalidadeMovimentacao`

**Expressões calculadas do layout:**

- `Iif([finalidade] == '', 'Normal', [finalidade])`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ResumoDiario`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoDiario.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/ResumoDiario.cs) — SHA-256 `3207670abe8e55243b91b32e801c8f4037f2da2e058e6a19a7b8d17e1f77c88c`
- [ResumoDiario.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/ResumoDiario.Designer.cs) — SHA-256 `e4eef7484dc7b1e338649d9eaee31f41b67fdd1fc3d2a25e2e1ff915de9b351a`

## Resumo financeiro

Classe: `ResumoFinanceiro`. Identificador: `MovimentoConta.ResumoFinanceiro`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo

**Campos apresentados (DTO):** `Outras`, `Frete`, `Juro`, `Multa`, `Produto`, `Servico`, `Desconto`, `Seguro`, `Ipi`, `Descricao`, `Total`, `Itens.Descricao`, `Itens.Credito`, `Itens.Debito`, `Itens.Total`, `Itens.Tipo`, `Itens.PlanosDeConta.Descricao`, `Itens.PlanosDeConta.Credito`, `Itens.PlanosDeConta.Debito`, `Itens.CentrosDeCusto.Descricao`, `Itens.CentrosDeCusto.Credito`, `Itens.CentrosDeCusto.Debito`, `Origens.Descricao`, `Origens.Total`, `Origens.Tipo`, `Origens.Itens.Descricao`, `Origens.Itens.Total`, `Origens.Itens.Tipo`, `Naturezas.Descricao`, `Naturezas.Total`, `Naturezas.Tipo`, `Naturezas.Itens.Descricao`, `Naturezas.Itens.Total`, `Naturezas.Itens.Tipo`

**Agrupamentos do layout:**

- `ordem`
- `Tipo`
- `ordemOrigemItens`
- `ordemNaturezaItens`

**Expressões calculadas do layout:**

- `Iif([Tipo]== 'Forma de Pagamento', 1 , \r\n Iif([Tipo]== 'Espécies', 2 ,\r\n  Iif([Tipo]== 'Contas', 3 ,  \r\n   Iif([Tipo] == 'Plano de Contas', 4 , \r\n    Iif([Tipo] == 'Centro de Custos', 5 , 0 ) ) ) ) )`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterResumoFinanceiro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoFinanceiro.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/ResumoFinanceiro.cs) — SHA-256 `8ae980a873437d93f58676a4d608d873a6cdc088f871c542b5fa0b479758eca2`
- [ResumoFinanceiro.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/ResumoFinanceiro.Designer.cs) — SHA-256 `a8aced0e649260e4a65b22cda64c6a7d9298c59f28271f997d3aa44e6a454401`

## Resumo financeiro - 40 colunas

Classe: `ResumoFinanceiro40Colunas`. Identificador: `MovimentoConta.ResumoFinanceiro40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo

**Campos apresentados (DTO):** `Outras`, `Frete`, `Juro`, `Multa`, `Produto`, `Servico`, `Desconto`, `Seguro`, `Ipi`, `Descricao`, `Total`, `Itens.Descricao`, `Itens.Credito`, `Itens.Debito`, `Itens.Total`, `Itens.Tipo`, `Itens.PlanosDeConta.Descricao`, `Itens.PlanosDeConta.Credito`, `Itens.PlanosDeConta.Debito`, `Itens.CentrosDeCusto.Descricao`, `Itens.CentrosDeCusto.Credito`, `Itens.CentrosDeCusto.Debito`, `Origens.Descricao`, `Origens.Total`, `Origens.Tipo`, `Origens.Itens.Descricao`, `Origens.Itens.Total`, `Origens.Itens.Tipo`, `Naturezas.Descricao`, `Naturezas.Total`, `Naturezas.Tipo`, `Naturezas.Itens.Descricao`, `Naturezas.Itens.Total`, `Naturezas.Itens.Tipo`

**Agrupamentos do layout:**

- `ordem`
- `Tipo`
- `ordemOrigemItens`
- `ordemNaturezaItens`

**Expressões calculadas do layout:**

- `Iif([Tipo]== 'Forma de Pagamento', 1 , \r\n Iif([Tipo]== 'Espécies', 2 ,\r\n  Iif([Tipo]== 'Contas', 3 ,  \r\n   Iif([Tipo] == 'Plano de Contas', 4 , \r\n    Iif([Tipo] == 'Centro de Custos', 5 , 0 ) ) ) ) )`

**Contratos de serviço:**

- `IContaMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IContaMovimentacaoRelatorioModuleService.ObterResumoFinanceiro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoFinanceiro40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ContaMovimentacao/ResumoFinanceiro40Colunas.cs) — SHA-256 `e7c820afbfc34497859613e61725e3ea153004133fd141b984bb6ed3d54a54ce`
- [ResumoFinanceiro40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ContaMovimentacao/ResumoFinanceiro40Colunas.Designer.cs) — SHA-256 `dff17b8fa200d6dbfcfbb5fc133699ec83b5a5db1fdbd839d1bda3d2c6407cf5`

