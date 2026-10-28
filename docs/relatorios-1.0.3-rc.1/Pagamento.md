# Contas a pagar

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de pagamentos

Classe: `PagamentosRecebimentos`. Identificador: `Pagamento.PagamentosRecebimentos`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Filtrar por tipo de data; Situação; Conta; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `Valor`, `NossoNumero`, `Situacao`, `Especie`, `ValorQuitado`, `ValorCorrigido`, `dataQuitado`, `DataVencimento`, `DataEmissao`, `PessoaToText`, `Ordem`, `Documento`, `ContaDescricao`, `EmpresaCnpj`, `Empresa`

**Agrupamentos do layout:**

- `ContaReferencia`
- `EmpresaCnpj`

**Expressões calculadas do layout:**

- `Iif([DataQuitacao]  ==  '01/01/0001 00:00:00', '', [DataQuitacao])`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaPagamentos, IPagamentoRecebimentoRelatorioModuleService.ObterListaRecebimentos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosRecebimentos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PagamentoRecebimento/PagamentosRecebimentos.cs) — SHA-256 `4337be632dd9d3aaa8342598014c94a7756d50a416b72d2adc45878cd3a00ff6`
- [PagamentosRecebimentos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PagamentoRecebimento/PagamentosRecebimentos.Designer.cs) — SHA-256 `a0bbbf17e239ea96b28bb614cf191892dd0196386c2cc70e51675f3a03e1d5b6`

## Lista de pagamentos agrupados por dia de vencimento

Classe: `PagamentosPorDiaVencimento`. Identificador: `Pagamento.PagamentosPorDiaVencimento`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Filtrar por tipo de data; Situação; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `PessoaNome`, `Situacao`, `DataRecebimento`, `EspeciePagamento`, `ValorQuitado`, `Valor`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `DataVencimento`, `EmpresaDocumento`, `EmpresaNome`

**Agrupamentos do layout:**

- `EmpresaDocumento`
- `DataVencimento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDePagamentosPorPessoaESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosPorDiaVencimento.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pagamento/PagamentosPorDiaVencimento.cs) — SHA-256 `a399a2736c0dde525e6a177703ea8c71cb972dba225f02c57387700453404e1f`
- [PagamentosPorDiaVencimento.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pagamento/PagamentosPorDiaVencimento.Designer.cs) — SHA-256 `ceb0f13232df084eb3a73fba43d12381e505dadd9e21f346f4de423d530210ee`

## Lista de pagamentos em aberto

Classe: `PagamentosEmAberto`. Identificador: `Pagamento.PagamentosEmAberto`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data de vencimento inicial; Data de vencimento final; Espécie; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `EspeciePagamento`, `ValorTotal`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `DiasAtraso`, `PessoaNome`, `PessoaDocumento`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`, `EmpresaNome`, `EmpresaDocumento`

**Agrupamentos do layout:**

- `PessoaDocumento`
- `EmpresaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDePagamentosEmAberto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosEmAberto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pagamento/PagamentosEmAberto.cs) — SHA-256 `527af6033fb2ce356968afe852b56cf8fc5c7f2c7b748ce611066d4ce81f85b6`
- [PagamentosEmAberto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pagamento/PagamentosEmAberto.Designer.cs) — SHA-256 `86f35786b40be9bc8068297b01560905e1422e307c693cf2a6f406f9a6a8d826`

## Lista de pagamentos por pessoa

Classe: `PagamentosPorPessoa`. Identificador: `Pagamento.PagamentosPorPessoa`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Pessoa; Data inicial; Data final; Plano de conta; Centro de custo; Filtrar por tipo de data; Situação; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `Situacao`, `Desconto`, `DataRecebimento`, `EspeciePagamento`, `ValorQuitado`, `Valor`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`, `PessoaDocumento`, `EmpresaDocumento`, `EmpresaNome`

**Agrupamentos do layout:**

- `PessoaNome`
- `EmpresaDocumento`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDePagamentosPorPessoaESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pagamento/PagamentosPorPessoa.cs) — SHA-256 `81e9c1e03ec6f9b6827f1f0c0eadf2cc0ab61a22d6a59e01b26348e7bc635f77`
- [PagamentosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pagamento/PagamentosPorPessoa.Designer.cs) — SHA-256 `d9cd9718a14031c066d60cd79494ab9b6df46b959e484da38bd8b31a846dfb43`

## Lista de pagamentos por tipo de segmentação

Classe: `PagamentosPorSegmentacao`. Identificador: `Pagamento.PagamentosPorSegmentacao`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Tipo de segmentação; Data inicial; Data final; Plano de conta; Centro de custo; Filtrar por tipo de data; Situação; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `Situacao`, `Desconto`, `DataRecebimento`, `EspeciePagamento`, `ValorQuitado`, `Valor`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`, `PessoaDocumento`, `EmpresaDocumento`, `EmpresaNome`

**Agrupamentos do layout:**

- `PessoaNome`
- `EmpresaDocumento`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDePagamentosPorPessoaESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosPorSegmentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pagamento/PagamentosPorSegmentacao.cs) — SHA-256 `198afd36ced4581bd92017b63c43a965e3ed0f53610e9f609a3c51d8e461781c`
- [PagamentosPorSegmentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pagamento/PagamentosPorSegmentacao.Designer.cs) — SHA-256 `c6d5dba4b7d5d4229bde65a51baf0975cd645221bbdab14e66f1452f2bfa0b76`

## Lista de pagamentos quitados

Classe: `PagamentosQuitados`. Identificador: `Pagamento.PagamentosQuitados`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data de quitação inicial; Data de quitação final; Tipo de pagamento; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `Desconto`, `DataQuitacao`, `EspeciePagamento`, `ValorQuitado`, `Valor`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`, `PessoaDocumento`, `EmpresaDocumento`, `EmpresaNome`

**Agrupamentos do layout:**

- `PessoaNome`
- `EmpresaDocumento`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDePagamentosQuitados`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosQuitados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pagamento/PagamentosQuitados.cs) — SHA-256 `794a14d44a4823e930ea779134fc73171c59a3a2df9275c602792bbcb6dd983b`
- [PagamentosQuitados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pagamento/PagamentosQuitados.Designer.cs) — SHA-256 `6127a07c4966baf52fbc6f5758ce40938b4cf890fe9665290e2a42962f3d6e06`

## Lista de pagamentos quitados por data de competência

Classe: `PagamentosQuitadosPorDataCompetencia`. Identificador: `Pagamento.PagamentosQuitadosPorDataCompetencia`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Data inicial; Data final; Mostrar observação

**Campos apresentados (DTO):** `DataUltimoHistorico`, `DataQuitacao`, `EspeciePagamento`, `ValorQuitado`, `Valor`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`, `PessoaDocumento`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDePagamentosQuitadosPorDataCompetencia`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosQuitadosPorDataCompetencia.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Pagamento/PagamentosQuitadosPorDataCompetencia.cs) — SHA-256 `f4679d6216e55a274f445a529e9a3e3f04d84ad0ef6a4c8587359e92bb159b56`
- [PagamentosQuitadosPorDataCompetencia.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Pagamento/PagamentosQuitadosPorDataCompetencia.Designer.cs) — SHA-256 `cc11517c282c9f19b376281a634fbcf4b29ac0016f569bc161aca720407887da`

## Lista de pagamentos sintético

Classe: `PagamentosRecebimentosSintetico`. Identificador: `Pagamento.PagamentosRecebimentosSintetico`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Filtrar por tipo de data; Situação; Conta; Ordenação

**Campos apresentados (DTO):** `ValorCorrigido`, `DataVencimento`, `PessoaToText`, `Ordem`, `Documento`, `ContaDescricao`, `EmpresaCnpj`, `Situacao`, `ValorQuitado`

**Agrupamentos do layout:**

- `ContaReferencia`
- `EmpresaCnpj`

**Expressões calculadas do layout:**

- `Iif([DataQuitacao]  ==  '01/01/0001 00:00:00', '', [DataQuitacao])`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaPagamentos, IPagamentoRecebimentoRelatorioModuleService.ObterListaRecebimentos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [PagamentosRecebimentosSintetico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PagamentoRecebimento/PagamentosRecebimentosSintetico.cs) — SHA-256 `7d43291cafc7df9ab7b514c0d68b03612f1fdf6154d12a986d54b4a6390d110f`
- [PagamentosRecebimentosSintetico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PagamentoRecebimento/PagamentosRecebimentosSintetico.Designer.cs) — SHA-256 `31f9d8232a1eee267527ebcf687282b30f4cc0c2c579065119cd595eb23c17c2`

