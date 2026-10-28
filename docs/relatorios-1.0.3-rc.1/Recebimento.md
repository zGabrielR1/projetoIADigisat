# Contas a receber

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de pagamentos

Classe: `PagamentosRecebimentos`. Identificador: `Recebimento.PagamentosRecebimentos`.

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

## Lista de pagamentos sintético

Classe: `PagamentosRecebimentosSintetico`. Identificador: `Recebimento.PagamentosRecebimentosSintetico`.

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

## Lista de recebimentos (cartão crédito/débito)

Classe: `RecebimentosCartao`. Identificador: `Recebimento.RecebimentosCartao`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Administradora de cartão; Filtrar por tipo de data; Data inicial; Data final; Filtrar por operação; Ordenação

**Campos apresentados (DTO):** `Operacao`, `Situacao`, `DescricaoRede`, `EspeciePagamento`, `ValorTotal`, `ValorTaxa`, `Valor`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaFantasia`, `PessoaDocumento`, `PessoaNome`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosCartao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosCartao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosCartao.cs) — SHA-256 `1335978e56a9e80b34d1b9671d2cb2fc4954852c12469f4902ffef725928ed1a`
- [RecebimentosCartao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosCartao.Designer.cs) — SHA-256 `f8adaa386a0c661fee5a1ce8924d3450118d349a8da9a576e5523d9427c8ffbd`

## Lista de recebimentos com taxa (cartão crédito/débito)

Classe: `RecebimentosTaxas`. Identificador: `Recebimento.RecebimentosTaxas`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Administradora de cartão; Filtrar por tipo de data; Data inicial; Data final; Filtrar por operação; Imprimir registros com valor de taxa zerado; Ordenação

**Campos apresentados (DTO):** `Situacao`, `EspeciePagamento`, `ValorTaxa`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `Operacao`, `DescricaoRede`

**Agrupamentos do layout:**

- `DescricaoRede`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosCartao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosTaxas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosTaxas.cs) — SHA-256 `8e7f8473409b8a04df55f6b51d311a654c7c73207121be249741d34b568d15ce`
- [RecebimentosTaxas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosTaxas.Designer.cs) — SHA-256 `93954c1fff1dbfdb64b197488c0dab92c35b0cb09bdf76a9dd8c9e3ead4ed1f8`

## Lista de recebimentos em aberto

Classe: `RecebimentosEmAberto`. Identificador: `Recebimento.RecebimentosEmAberto`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data de vencimento inicial; Data de vencimento final; Espécie; Conta; Ordenação; Município; Mostrar observação

**Campos apresentados (DTO):** `NossoNumero`, `DiasAtraso`, `EspeciePagamento`, `ValorTotal`, `Multa`, `Juro`, `Valor`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaFantasia`, `PessoaEndereco`, `PessoaTelefone`, `PessoaDocumento`, `PessoaNome`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosEmAberto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosEmAberto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosEmAberto.cs) — SHA-256 `9eb9c5117c1fec714a6d3e9f16b2d0a85f8ab4c0b94a98a9144e28573b5bedeb`
- [RecebimentosEmAberto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosEmAberto.Designer.cs) — SHA-256 `607d08d3b462ce851c5742d20425e3cbd3472cd642b444f081419c71021510fa`

## Lista de recebimentos em aberto sintético

Classe: `RecebimentosEmAbertoSintetico`. Identificador: `Recebimento.RecebimentosEmAbertoSintetico`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data de vencimento inicial; Data de vencimento final; Espécie; Conta; Município

**Campos apresentados (DTO):** `ValorTotal`, `Multa`, `PessoaDocumento`, `PessoaNome`, `Juro`, `Valor`, `PessoaTelefone`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosEmAberto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosEmAbertoSintetico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosEmAbertoSintetico.cs) — SHA-256 `76bb5ea6d860910ecdfc13dcb277ce3ef71c250b38366f4a01e4d067c818db50`
- [RecebimentosEmAbertoSintetico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosEmAbertoSintetico.Designer.cs) — SHA-256 `45286cf99880530597afee0ee0e7955a63b050ab81c0d2e276bfd01fc5607195`

## Lista de recebimentos por pessoa

Classe: `RecebimentosPorPessoa`. Identificador: `Recebimento.RecebimentosPorPessoa`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Pessoa; Data inicial; Data final; Conta; Plano de conta; Centro de custo; Filtrar por tipo de data; Situação; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `Valor`, `Situacao`, `Desconto`, `DataRecebimento`, `EspeciePagamento`, `ValorQuitado`, `Multa`, `Juro`, `ValorCorrigido`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaDocumento`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`, `JuroMulta`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`
- `[Juro] + [Multa]`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosPorPessoaESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorPessoa.cs) — SHA-256 `ba6235e982cefa6365bdece28a5ce6f3bf4582f82ab4ec7158a5f2401ab56cd0`
- [RecebimentosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorPessoa.Designer.cs) — SHA-256 `b3ff518affd2fc3736735c66937360b3e73e63d1682ca288b9dfb2318e02b2d9`

## Lista de recebimentos por pessoa - 40 colunas

Classe: `RecebimentosPorPessoa40Colunas`. Identificador: `Recebimento.RecebimentosPorPessoa40Colunas`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Pessoa; Data inicial; Data final; Conta; Plano de conta; Centro de custo; Filtrar por tipo de data; Situação; Ordenação

**Campos apresentados (DTO):** `ValorCorrigido`, `DataVencimento`, `ToDocumentoOrdem`, `PessoaNome`, `PessoaDocumento`, `PessoaFantasia`, `ValorQuitado`, `JuroMulta`, `Desconto`, `Multa`, `Juro`, `DataEmissao`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`
- `[Juro] + [Multa]`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosPorPessoaESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosPorPessoa40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorPessoa40Colunas.cs) — SHA-256 `b56b03e39d41bd66634872fd654df94a35a355d67f4bb52318df0409637aeff3`
- [RecebimentosPorPessoa40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorPessoa40Colunas.Designer.cs) — SHA-256 `69e35eec0bbaaeeb0825ae23eb7edc3af780803888b51ed0d8e669d647e55ba4`

## Lista de recebimentos por tipo de segmentação

Classe: `RecebimentosPorSegmentacao`. Identificador: `Recebimento.RecebimentosPorSegmentacao`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Tipo de segmentação; Data inicial; Data final; Conta; Plano de conta; Centro de custo; Filtrar por tipo de data; Situação; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `Valor`, `Situacao`, `Desconto`, `DataRecebimento`, `EspeciePagamento`, `ValorQuitado`, `Multa`, `Juro`, `ValorCorrigido`, `DataVencimento`, `DataEmissao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaDocumento`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`, `JuroMulta`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`
- `[Juro] + [Multa]`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosPorPessoaESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosPorSegmentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorSegmentacao.cs) — SHA-256 `fd788d0b133f15adb497174a4a08a9ba55d912ff849810850ea4f51a9255ea0c`
- [RecebimentosPorSegmentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorSegmentacao.Designer.cs) — SHA-256 `93b599345829a664826e440d87360db24b9b3715e17ac5871c9c3261e106524c`

## Lista de recebimentos por tipo de segmentação - 40 colunas

Classe: `RecebimentosPorSegmentacao40Colunas`. Identificador: `Recebimento.RecebimentosPorSegmentacao40Colunas`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Tipo de segmentação; Data inicial; Data final; Conta; Plano de conta; Centro de custo; Filtrar por tipo de data; Situação; Ordenação

**Campos apresentados (DTO):** `ValorCorrigido`, `DataVencimento`, `ToDocumentoOrdem`, `PessoaNome`, `PessoaDocumento`, `PessoaFantasia`, `ValorQuitado`, `JuroMulta`, `Desconto`, `Multa`, `Juro`, `DataEmissao`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`
- `Iif([DataQuitacao]=='01/01/0001 00:00:00', '' , [DataQuitacao])`
- `[Juro] + [Multa]`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosPorPessoaESituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosPorSegmentacao40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorSegmentacao40Colunas.cs) — SHA-256 `b3bcbdabdfd09bfeae96f427cc62bb0f8db0a1972826259f9ba1e2515130b73e`
- [RecebimentosPorSegmentacao40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorSegmentacao40Colunas.Designer.cs) — SHA-256 `bcd17b6d6d376c00b87bf293b8a8d284e7bd7d3caf8bcec7cb830bffaad764b9`

## Lista de recebimentos quitados

Classe: `RecebimentosQuitados`. Identificador: `Recebimento.RecebimentosQuitados`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Consolidado; Data de quitação inicial; Data de quitação final; Tipo de recebimento; Conta; Ordenação; Mostrar observação

**Campos apresentados (DTO):** `Desconto`, `DataQuitacao`, `EspeciePagamento`, `ValorQuitado`, `Multa`, `Juro`, `Valor`, `DataVencimento`, `DataInclusao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaDocumento`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosQuitados`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosQuitados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosQuitados.cs) — SHA-256 `c316d432293cd8bd3d2d8ce79d2a99268315638afc425f04a7fde5eed8afd73e`
- [RecebimentosQuitados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosQuitados.Designer.cs) — SHA-256 `da57e6cb23aa168feafb7881212e6e2a85aad3558134b9264163e1ba43a1b1d9`

## Lista de recebimentos quitados por data de competência

Classe: `RecebimentosQuitadosPorDataCompetencia`. Identificador: `Recebimento.RecebimentosQuitadosPorDataCompetencia`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Data inicial; Data final; Mostrar observação

**Campos apresentados (DTO):** `Desconto`, `DataUltimoHistorico`, `DataQuitacao`, `EspeciePagamento`, `ValorQuitado`, `Multa`, `Juro`, `Valor`, `DataVencimento`, `DataInclusao`, `Descricao`, `Ordem`, `Documento`, `PessoaNome`, `PessoaDocumento`, `PessoaTelefone`, `PessoaEndereco`, `PessoaFantasia`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Iif(DateDiffDay([DataVencimento],LocalDateTimeNow()) > 0, DateDiffDay([DataVencimento],LocalDateTimeNow()),0)`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterListaDeRecebimentosQuitadosPorDataCompetencia`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosQuitadosPorDataCompetencia.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosQuitadosPorDataCompetencia.cs) — SHA-256 `074c89c6ae9bdbe15f29f7eff63323b77a1bacdda1fd9396860d80386baceaec`
- [RecebimentosQuitadosPorDataCompetencia.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosQuitadosPorDataCompetencia.Designer.cs) — SHA-256 `3d794a2f3baae8356e5eda93fdb8bbb667586fae8dc7a6a382c22adb6592af6e`

## Recebimentos por vendedor

Classe: `RecebimentosPorVendedor`. Identificador: `Recebimento.RecebimentosPorVendedor`.

**Execução no Insight:** Consulta financeira disponível no Insight; regras rastreadas no build instalado. Homologação visual do relatório nativo pendente.

**Parâmetros nativos:** Vendedor; Conta; Data inicial; Data final; Filtrar por tipo de data; Situação; Ordenação

**Campos apresentados (DTO):** `DocumentoToText`, `Descricao`, `DataInclusao`, `DataVencimento`, `DataQuitacao`, `Valor`, `Juro`, `Multa`, `ValorQuitado`, `Situacao`, `PessoaNome`

**Agrupamentos do layout:**

- `PessoaNome`
- `PessoaDocumento`

**Expressões calculadas do layout:**

- `Concat([Documento], '/', [Ordem])`

**Contratos de serviço:**

- `IPagamentoRecebimentoRelatorioModuleService`

**Chamadas catalogadas:**

- `IPagamentoRecebimentoRelatorioModuleService.ObterRelatorioRecebimentosPorVendedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: financeiro-1.0.3.md](../../docs/financeiro-1.0.3.md)
- [Rastreamento: finance-artifacts-1.0.3-rc.1.json](../../docs/finance-artifacts-1.0.3-rc.1.json)

**Fontes da ficha:**

- [RecebimentosPorVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorVendedor.cs) — SHA-256 `deb19945ff514918e42f4a43abda4cf7449d67d1f7e9a5de841e73338fc56305`
- [RecebimentosPorVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Recebimento/RecebimentosPorVendedor.Designer.cs) — SHA-256 `f34cad935691a890cdd8b175662c6b5b4a95582903553065a4cd22c81c18af99`

