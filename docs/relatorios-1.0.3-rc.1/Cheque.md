# Cheques

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de cheques emitidos

Classe: `ChequesEmitidosPorSituacao`. Identificador: `Cheque.ChequesEmitidosPorSituacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Situação dos cheques; Filtro por data; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `BancoToText`, `Conta`, `Numero`, `Emissao`, `DataCompensacao`, `Valor`, `PessoaVinculadaAoCheque`, `DataDevolucao`, `DataPredatado`, `Agencia`, `EmpresaCnpj`, `Empresa`, `SituacaoToText`

**Agrupamentos do layout:**

- `Empresa.CpfCnpjFormatado`
- `Pessoa.Nome`
- `Situacao`

**Expressões calculadas do layout:**

- `Iif([Devolucao]=='01/01/0001 00:00:00', '' , [Devolucao])`
- `Concat([DocumentoEmissor], ' ', [NomeEmissor])`
- `Iif([Compensacao]=='01/01/0001 00:00:00', '' , [Compensacao])`
- `Iif([PreDatado]=='01/01/0001 00:00:00', '' , [PreDatado])`
- `Concat([Banco.Codigo],' ', [Banco.Descricao])`
- `Concat([PessoaCpfCnpj], ' ', [Pessoa])`

**Contratos de serviço:**

- `IChequeModuleService`

**Chamadas catalogadas:**

- `IChequeModuleService.ObterChequesEmitidos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ChequesEmitidosPorSituacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Cheque/ChequesEmitidosPorSituacao.cs) — SHA-256 `ac8701745a9072f38f0da4bb53f23e00dd4421b180012e41d474a28617aad23c`
- [ChequesEmitidosPorSituacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Cheque/ChequesEmitidosPorSituacao.Designer.cs) — SHA-256 `6c1bd8015a38e41506116b3ccf6ad0ba430abf101eef14e1a911a14aee33c52e`

## Lista de cheques recebidos

Classe: `ChequesRecebidosPorSituacao`. Identificador: `Cheque.ChequesRecebidosPorSituacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Situação dos cheques; Filtro por data; Data inicial; Data final; Pessoa; Imprimir histórico do cheque; Ordenação

**Campos apresentados (DTO):** `HistoricoToText`, `BancoToText`, `Conta`, `Numero`, `Emissao`, `DataCompensacao`, `Valor`, `EmissorToText`, `DataDevolucao`, `DataPredatado`, `Agencia`, `EmpresaCnpj`, `Empresa`, `SituacaoToText`

**Agrupamentos do layout:**

- `Empresa.CpfCnpjFormatado`
- `Pessoa.Nome`
- `Situacao`

**Expressões calculadas do layout:**

- `Iif([Devolucao]=='01/01/0001 00:00:00', '' , [Devolucao])`
- `Concat([DocumentoEmissor], ' ', [NomeEmissor])`
- `Iif([Compensacao]=='01/01/0001 00:00:00', '' , [Compensacao])`
- `Iif([PreDatado]=='01/01/0001 00:00:00', '' , [PreDatado])`
- `Concat([Banco.Codigo],' ', [Banco.Descricao])`
- `Concat([PessoaCpfCnpj], ' ', [Pessoa])`

**Contratos de serviço:**

- `IChequeModuleService`

**Chamadas catalogadas:**

- `IChequeModuleService.ObterChequesRecebidos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ChequesRecebidosPorSituacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Cheque/ChequesRecebidosPorSituacao.cs) — SHA-256 `27506d2d4dab1b122dcdd807b3ff1b60aa13420156c396b5d31793e1e991ace8`
- [ChequesRecebidosPorSituacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Cheque/ChequesRecebidosPorSituacao.Designer.cs) — SHA-256 `06332699516b72845c94de0b49db7dd81e3001d263171ff3ede3b3ea71e22d01`

## Resumo dos cheques por situação

Classe: `ResumoChequesPorSituacao`. Identificador: `Cheque.ResumoChequesPorSituacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data de emissão inicial; Data de emissão final

**Campos apresentados (DTO):** `SituacaoToText`, `Valor`, `EmitidoRecebido`, `EmpresaCnpj`, `Empresa`

**Agrupamentos do layout:**

- `Situacao`
- `Empresa.CpfCnpjFormatado`
- `IsChequeEmitido`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif([Devolucao]=='01/01/0001 00:00:00', '' , [Devolucao])`
- `Concat([DocumentoEmissor], ' ', [NomeEmissor])`
- `Iif([Compensacao]=='01/01/0001 00:00:00', '' , [Compensacao])`
- `Iif([PreDatado]=='01/01/0001 00:00:00', '' , [PreDatado])`
- `Concat([Banco.Codigo],' ', [Banco.Descricao])`
- `Concat([PessoaCpfCnpj], ' ', [Pessoa])`
- `Iif([IsChequeEmitido],  'Cheques emitidos', 'Cheques recebidos')`

**Contratos de serviço:**

- `IChequeModuleService`

**Chamadas catalogadas:**

- `IChequeModuleService.ObterResumoCheques`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoChequesPorSituacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Cheque/ResumoChequesPorSituacao.cs) — SHA-256 `66af44c9e8e60cf08676fc234719c278e07ab90cd6c859ea4924da1f68f253bc`
- [ResumoChequesPorSituacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Cheque/ResumoChequesPorSituacao.Designer.cs) — SHA-256 `f39bba2cfccd2cba7ec7adf802d0861dc2211f1391a1b561d12c04639daf9488`

