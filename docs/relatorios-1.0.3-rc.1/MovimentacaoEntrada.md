# Movimentação entrada

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Apuração de ICMS

Classe: `ApuracaoIcms`. Identificador: `MovimentacaoEntrada.ApuracaoIcms`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Saidas.Cfop`, `Saidas.ValorContabil`, `Saidas.ValorBaseIcms`, `Saidas.ValorIcms`, `Saidas.ValorIsento`, `Saidas.ValorOutros`, `Saidas.TotalValorContabilSaida`, `Saidas.TotalValorBaseIcmsSaida`, `Saidas.TotalValorIcmsSaida`, `Saidas.TotalValorIsentoSaida`, `Saidas.TotalValorOutrosSaida`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IConhecimentoTransporteEletronicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IConhecimentoTransporteEletronicoRelatorioModuleService.ObterApuracoesIcms`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ApuracaoIcms.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ConhecimentoTransporteEletronico/ApuracaoIcms.cs) — SHA-256 `314d8a8aaf9e9be880b68a7bdcf90702cc5d8cc477e36c621f27e217e8ba5f81`
- [ApuracaoIcms.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ConhecimentoTransporteEletronico/ApuracaoIcms.Designer.cs) — SHA-256 `79eabcd722819e1f3cad3df0c150c7aa3ece72b45b4eefdafa1ef5cdbdf1f9e2`

## Apuração de ICMS por CT-e

Classe: `ApuracaoIcmsPorMovimentacao`. Identificador: `MovimentacaoEntrada.ApuracaoIcmsPorMovimentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; CFOP

**Campos apresentados (DTO):** `Saidas.Numero`, `Saidas.Cfop`, `Saidas.ValorContabil`, `Saidas.ValorBaseIcms`, `Saidas.ValorIcms`, `Saidas.ValorIsento`, `Saidas.ValorOutros`, `Saidas.TotalValorContabilSaida`, `Saidas.TotalValorBaseIcmsSaida`, `Saidas.TotalValorIcmsSaida`, `Saidas.TotalValorIsentoSaida`, `Saidas.TotalValorOutrosSaida`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorContabil])`
- `Sum([ValorIcms])`
- `Sum([ValorIsento])`
- `Sum([ValorOutros])`

**Contratos de serviço:**

- `IConhecimentoTransporteEletronicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IConhecimentoTransporteEletronicoRelatorioModuleService.ObterApuracoesIcmsPorMovimentacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ApuracaoIcmsPorMovimentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ConhecimentoTransporteEletronico/ApuracaoIcmsPorMovimentacao.cs) — SHA-256 `6ba3d7ffa4598e227dd7c092aab427f64d5a92cdab92904588929340eb2b262c`
- [ApuracaoIcmsPorMovimentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ConhecimentoTransporteEletronico/ApuracaoIcmsPorMovimentacao.Designer.cs) — SHA-256 `7e074513a56a1a2f6cfe99c9f8622794c89415999f4fc47ccdd53ec5ccd214d6`

## Apuração de PIS e COFINS

Classe: `ApuracaoPisCofins`. Identificador: `MovimentacaoEntrada.ApuracaoPisCofins`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Entradas.Cst`, `Entradas.ValorContabil`, `Entradas.ValorBasePis`, `Entradas.ValorPis`, `Entradas.ValorBaseCofins`, `Entradas.ValorCofins`, `Entradas.TotalValorContabilEntrada`, `Entradas.TotalValorBasePisEntrada`, `Entradas.TotalValorPisEntrada`, `Entradas.TotalValorBaseCofinsEntrada`, `Entradas.TotalValorCofinsEntrada`, `Saidas.Cst`, `Saidas.ValorContabil`, `Saidas.ValorBasePis`, `Saidas.ValorPis`, `Saidas.ValorBaseCofins`, `Saidas.ValorCofins`, `Saidas.TotalValorContabilSaida`, `Saidas.TotalValorBasePisSaida`, `Saidas.TotalValorPisSaida`, `Saidas.TotalValorBaseCofinsSaida`, `Saidas.TotalValorCofinsSaida`

**Expressões calculadas do layout:**

- `Sum([ValorContabil])`
- `Sum([ValorBasePis])`
- `Sum([ValorPis])`
- `Sum([ValorBaseCofins])`
- `Sum([ValorCofins])`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterApuracoesPisCofins`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ApuracaoPisCofins.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ApuracaoPisCofins.cs) — SHA-256 `2aac7d9297b9548a777f66ed3fa392d54d41b7f09e3f65c07821cd4470087dfc`
- [ApuracaoPisCofins.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ApuracaoPisCofins.Designer.cs) — SHA-256 `0b7ff6bac9a42f4de1e4364d9c9485498453a8e4ededa0a9536dad813622703f`

## Apuração de PIS e COFINS por itens comercializados

Classe: `ApuracaoPisCofinsItens`. Identificador: `MovimentacaoEntrada.ApuracaoPisCofinsItens`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; CFOP; Tributação federal; Tipo de data

**Campos apresentados (DTO):** `ValorCofins`, `ValorPis`, `TotalLiquido`, `NcmNbs`, `Descricao`, `TotalBruto`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterItensApuracaoPisCofins`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ApuracaoPisCofinsItens.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ApuracaoPisCofinsItens.cs) — SHA-256 `b335cd8166a52d82bba53a9e9febbb016abba3680b3732fb4668a0161de6dfa5`
- [ApuracaoPisCofinsItens.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ApuracaoPisCofinsItens.Designer.cs) — SHA-256 `dd0f5c3e25282de80b83fdc869799b75b74416ae25195fc14e4e1d2a0d40c276`

## Apuração de PIS e COFINS por itens comercializados - detalhado

Classe: `ApuracaoPisCofinsItensDetalhado`. Identificador: `MovimentacaoEntrada.ApuracaoPisCofinsItensDetalhado`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; CFOP; Tributação federal; Tipo de data

**Campos apresentados (DTO):** `CodigoInterno`, `PercentualIcms`, `Cst`, `PercentualCofins`, `PercentualPis`, `ValorCofins`, `ValorPis`, `TotalLiquido`, `NcmNbs`, `Descricao`, `TotalBruto`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterItensApuracaoPisCofinsDetalhado`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ApuracaoPisCofinsItensDetalhado.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ApuracaoPisCofinsItensDetalhado.cs) — SHA-256 `52958c3bb5e7c1988c0124358eb754bb08ca473ed4350118053a870e8f6de0f4`
- [ApuracaoPisCofinsItensDetalhado.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ApuracaoPisCofinsItensDetalhado.Designer.cs) — SHA-256 `4fbb4bef6d4a682a856782a06bfaa242969c1fed80b8fd9dad54e9c1874ae437`

## Compras por fornecedor

Classe: `ComprasPorFornecedor`. Identificador: `MovimentacaoEntrada.ComprasPorFornecedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo de data; Ordenação

**Campos apresentados (DTO):** `Participacao`, `Total`, `DataUltimaMovimentacao`, `Nome`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ComprasPorFornecedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ComprasPorFornecedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ComprasPorFornecedor.cs) — SHA-256 `85d46e6e6320435c04aaaea3585beaaf252656d283f10c3f6b8bf2f0f6c8b10f`
- [ComprasPorFornecedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ComprasPorFornecedor.Designer.cs) — SHA-256 `bff0582b992f3759c2d322f60bdf422f46b2154b13f9b0f2261740b9192d9f82`

## Entradas com substituição tributária por produto

Classe: `EntradasSubstituicaoTributariaPorProduto`. Identificador: `MovimentacaoEntrada.EntradasSubstituicaoTributariaPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Mostrar detalhes; Ordenação

**Campos apresentados (DTO):** `BaseCalculoIcmsEfetivo`, `ValorIcmsEfetivo`, `ValorStRetido`, `BaseCalculoStRetido`, `ValorSt`, `ValorBaseCalculoSt`, `TotalLiquido`, `Quantidade`, `Ncm`, `Descricao`, `CodigoInterno`, `ItensMovimentacoes.PercentualBaseCalculoIcmsEfetivo`, `ItensMovimentacoes.BaseCalculoIcmsEfetivo`, `ItensMovimentacoes.PercentualIcmsEfetivo`, `ItensMovimentacoes.ValorIcmsEfetivo`, `ItensMovimentacoes.ValorStRetido`, `ItensMovimentacoes.BaseCalculoStRetido`, `ItensMovimentacoes.ValorSt`, `ItensMovimentacoes.ValorBaseCalculoSt`, `ItensMovimentacoes.TotalLiquido`, `ItensMovimentacoes.Quantidade`, `ItensMovimentacoes.Pessoa`, `ItensMovimentacoes.NumeroMovimentacao`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterEntradasSubstituicaoTributariaPorProduto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [EntradasSubstituicaoTributariaPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/EntradasSubstituicaoTributariaPorProduto.cs) — SHA-256 `f365ff098807171bb2e040d6393982977cfa844817dab8de540fb914c6631b7c`
- [EntradasSubstituicaoTributariaPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/EntradasSubstituicaoTributariaPorProduto.Designer.cs) — SHA-256 `15fc31598f887aee38267cd71d3780b7497b5ce97a2cecb176a1e573d93aed88`

## EntradasSubstituicaoTributariaPorProdutoCfopCstCsosn

Classe: `EntradasSubstituicaoTributariaPorProdutoCfopCstCsosn`. Identificador: `MovimentacaoEntrada.EntradasSubstituicaoTributariaPorProdutoCfopCstCsosn`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Mostrar detalhes; Ordenação

**Campos apresentados (DTO):** `CstCsosn`, `Cfop`, `PercentualMva`, `PercentualIcmsSt`, `ValorSt`, `ValorBaseCalculoSt`, `TotalLiquido`, `Quantidade`, `Ncm`, `Descricao`, `CodigoInterno`, `ItensMovimentacoes.CstCsosn`, `ItensMovimentacoes.Cfop`, `ItensMovimentacoes.PercentualIcmsEfetivo`, `ItensMovimentacoes.ValorIcmsEfetivo`, `ItensMovimentacoes.PercentualMva`, `ItensMovimentacoes.PercentualIcmsSt`, `ItensMovimentacoes.ValorSt`, `ItensMovimentacoes.ValorBaseCalculoSt`, `ItensMovimentacoes.TotalLiquido`, `ItensMovimentacoes.Quantidade`, `ItensMovimentacoes.Pessoa`, `ItensMovimentacoes.NumeroMovimentacao`, `ValorIcmsEfetivo`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterEntradasSubstituicaoTributariaPorProduto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [EntradasSubstituicaoTributariaPorProdutoCfopCstCsosn.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/EntradasSubstituicaoTributariaPorProdutoCfopCstCsosn.cs) — SHA-256 `16d66c0c56797ffde11f59506327c4053a4c56f03111db0976caac8cd6fe5c1f`

## Itens comprados

Classe: `ItensCompradosPorFornecedor`. Identificador: `MovimentacaoEntrada.ItensCompradosPorFornecedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pessoa; Produto; Tipo item; Data inicial; Data final; Tipo de data; Ordenação

**Campos apresentados (DTO):** `CodigoInterno`, `UnitarioLiquido`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `CpfCnpjPessoa`, `NomePessoa`, `Empresa`, `CnpjEmpresa`, `Seriais.Serial`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Grades.Seriais.Serial`, `Lotes.Quantidade`, `Lotes.Validade`, `Lotes.Lote`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `CpfCnpjPessoa`
- `NomePessoa`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterItensComprados`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensCompradosPorFornecedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorFornecedor.cs) — SHA-256 `e19869ace03e6529b2c07368f7d6cdb0fb84fa6815a430a940f5fbea2528d810`
- [ItensCompradosPorFornecedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorFornecedor.Designer.cs) — SHA-256 `61ad38a3dca3d648d1ee55357006ac86b4bb47680a643f5fd2581bd18c2bf2e1`

## Itens comprados agrupados por segmentação

Classe: `ItensCompradosPorSegmentacao`. Identificador: `MovimentacaoEntrada.ItensCompradosPorSegmentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo da segmentação; Data inicial; Data final; Tipo; Situação das movimentações; Natureza da operação; Pessoa; Produto; Ordenação; Agrupar por Produto

**Campos apresentados (DTO):** `TipoMovimentacao`, `Total`, `DataHoraSaida`, `NomePessoa`, `DataHora`, `Numero`, `CodigoBarras`, `Quantidade`, `CodigoInterno`, `Descricao`, `Segmentacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `Segmentacao`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterItensCompradosComDadosPessoa, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensCompradosPorSegmentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorSegmentacao.cs) — SHA-256 `da41062b7e820d9a111540bc6f63bb10d2e1320fd239e213e19ae829ed0d7eab`
- [ItensCompradosPorSegmentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorSegmentacao.Designer.cs) — SHA-256 `9e17dbb17b202d12af11b10dc4e978fbfedcfce67a1b13b7d645ae42305501de`

## Itens comprados por linha de produto

Classe: `ItensCompradosPorLinha`. Identificador: `MovimentacaoEntrada.ItensCompradosPorLinha`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pessoa; Linha de produto; Data inicial; Data final; Tipo de data; Ordenação; Exibir 'Código do fabricante'

**Campos apresentados (DTO):** `CodigoInterno`, `UnitarioLiquido`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `CodigoFabricante`, `CpfCnpjPessoa`, `NomePessoa`, `Empresa`, `CnpjEmpresa`, `Seriais.Serial`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Grades.Seriais.Serial`, `Lotes.Quantidade`, `Lotes.Validade`, `Lotes.Lote`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `CpfCnpjPessoa`
- `NomePessoa`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterItensCompradosPorLinha`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensCompradosPorLinha.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorLinha.cs) — SHA-256 `c137e1c5129acbb572d5c35be0b795c7fd8a7f593f7794340d76ff5f7f577b57`
- [ItensCompradosPorLinha.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorLinha.Designer.cs) — SHA-256 `2cb1cb161c84492de859a960fc0a0992221840361cc88f100724149bd12e1c03`

## Itens comprados por marca

Classe: `ItensCompradosPorMarca`. Identificador: `MovimentacaoEntrada.ItensCompradosPorMarca`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pessoa; Marca; Data inicial; Data final; Tipo de data; Ordenação; Exibir 'Código do fabricante'

**Campos apresentados (DTO):** `CodigoInterno`, `UnitarioLiquido`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `CodigoFabricante`, `CpfCnpjPessoa`, `NomePessoa`, `Empresa`, `CnpjEmpresa`, `Seriais.Serial`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Grades.Seriais.Serial`, `Lotes.Quantidade`, `Lotes.Validade`, `Lotes.Lote`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `CpfCnpjPessoa`
- `NomePessoa`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterItensCompradosPorMarca`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensCompradosPorMarca.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorMarca.cs) — SHA-256 `7ed2ef5ba0a2c9b82162c83ee90262dca5507ab82994469bcf63bdefc74c7efa`
- [ItensCompradosPorMarca.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorMarca.Designer.cs) — SHA-256 `e43f9773ab886d5cf8cb4f2db17f4ebeb5020344691f78f77bdc110230eafeff`

## Itens comprados por pessoa

Classe: `ItensCompradosPorPessoa`. Identificador: `MovimentacaoEntrada.ItensCompradosPorPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final; Pessoa; Tipo; Origem; Situação das movimentações; Natureza da operação; Produto ou serviço; Ordenação

**Campos apresentados (DTO):** `OutrasDespesas`, `CodigoInterno`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ObterItensCompradosPorPessoa, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensCompradosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorPessoa.cs) — SHA-256 `62652da1ac062678e55737495130ed853c761a8864c98505319ffd89661b13cf`
- [ItensCompradosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ItensCompradosPorPessoa.Designer.cs) — SHA-256 `f3b3acccb777c24c1ae45ad0919ce8d660157472f413724cb6934dd77c5789ae`

## Itens movimentados com dados de tributação do FCP

Classe: `ItensVendidosComTributacaoFcp`. Identificador: `MovimentacaoEntrada.ItensVendidosComTributacaoFcp`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Origem; Ordenação

**Campos apresentados (DTO):** `ValorIcms`, `BaseCalculoFcp`, `Cfop`, `ValorFcp`, `PercentualFcp`, `NumeroMovimentacao`, `CodigoInterno`, `Total`, `Descricao`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosComFcp`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosComTributacaoFcp.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ItensVendidosComTributacaoFcp.cs) — SHA-256 `a6972a51c5552b3c183ad0e707c6aa6074143e6d08af64fba542296b50d83d69`
- [ItensVendidosComTributacaoFcp.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ItensVendidosComTributacaoFcp.Designer.cs) — SHA-256 `2a8af0f69ca8925de9a0791e409d91fd7cdf0e84fdb4c60501b161470001b75d`

## Itens movimentados por origem

Classe: `ItensMovimentadosPorOrigem`. Identificador: `MovimentacaoEntrada.ItensMovimentadosPorOrigem`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Situação das movimentações; Origem; Tributação; Natureza da operação; Produto ou serviço; Ordenação; Exibir pesos bruto e líquido; Tipo preço custo; Tipo; Mostrar itens com movimentação física de estoque; Mostrar itens que geraram movimentação financeira; Marca; SubGrupo

**Campos apresentados (DTO):** `PrecoCusto`, `QuantidadeRestanteImportacaoParcial`, `QuantidadeImportadaImportacaoParcial`, `PesoLiquido`, `PesoBruto`, `Preco`, `Total`, `Desconto`, `Quantidade`, `Unidade`, `Subgrupo`, `Descricao`, `CodigoInterno`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterItensMovimentadosPorOrigem, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [ItensMovimentadosPorOrigem.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ItensMovimentadosPorOrigem.cs) — SHA-256 `b261faf373258b0b1c7ec9980cbda3dd47fcde8e094a5c7b0810ed93cb15fbdb`
- [ItensMovimentadosPorOrigem.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ItensMovimentadosPorOrigem.Designer.cs) — SHA-256 `59f50bc7d10064d75f40773a473b50bdd768f5bbd7cbcfa525a73130021ef50e`

## Lista de movimentações

Classe: `ListaMovimentacoesPorOrigemETipo`. Identificador: `MovimentacaoEntrada.ListaMovimentacoesPorOrigemETipo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Origem das movimentações; Situação das movimentações; Natureza da operação; Data/Hora inicial; Data/Hora final; Tipo de data; Pessoa; Vendedor; Mostrar detalhes da movimentação; Mostrar itens; Mostrar informação de movimentações referenciadas; Mostrar resumo por origem e situação; Mostrar resumo por dia; Ordenação; Mostrar observação da movimentação; Mostrar observação do item; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro; Mostrar resumo por pessoa; Mostrar resumo por forma de pagamento

**Campos apresentados (DTO):** `calculatedField1`, `Observacao`, `EspeciesPagamentoToText`, `FormaPagamento`, `Situacao`, `Finalidade`, `Total`, `DataHoraEntradaSaidaCF`, `DataHoraEmissao`, `ChaveAcesso`, `NomePessoa`, `Serie`, `Numero`, `Empresa`, `CnpjEmpresa`, `NaturezaOperacao`, `TipoMovimento`, `TipoMovimentacao`, `Itens.Quantidade`, `Itens.TotalLiquidoParaSum`, `Itens.Cfop`, `Itens.TotalLiquido`, `Itens.TotalBruto`, `Itens.TotalDesconto`, `Itens.PercentualIcms`, `Itens.PrecoUnitario`, `Itens.UnidadeMedida`, `Itens.Descricao`, `Itens.CodigoInterno`, `Itens.InformacaoEstoque`, `Itens.Observacao`, `DocumentosFiscaisReferenciados.InformacaoRelatorioToText`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Resumos do layout:**

- `Count`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`
- `Iif([DataHoraPrevisao]  == '01/01/0001 00:00:00', '', [DataHoraPrevisao])`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterMovimentacoesPorTipoMovimentacao, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [ListaMovimentacoesPorOrigemETipo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipo.cs) — SHA-256 `28b047ced8962fceac2923dd88df1ed0614e0989ae091aaa7af959f7553a6029`
- [ListaMovimentacoesPorOrigemETipo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipo.Designer.cs) — SHA-256 `7da2a27834704b829381822a7c9cf036ad3014775c3eea7ea1c3e3d80aff3453`

## Lista de movimentações com impostos

Classe: `ListaMovimentacoesPorOrigemETipoComImpostos`. Identificador: `MovimentacaoEntrada.ListaMovimentacoesPorOrigemETipoComImpostos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pessoa; Data/Hora inicial; Data/Hora final; Tipo de data; Origem das movimentações; Situação das movimentações; Natureza da operação; Ordenação; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro

**Campos apresentados (DTO):** `TotalSubstituicaoTributaria`, `TotalImpostoImportacao`, `TotalIpi`, `TotalIcms`, `TotalIss`, `TotalCofins`, `TotalPis`, `TotalAproximadoDosImpostos`, `TotalSeguro`, `TotalFrete`, `TotalOutrasDespesas`, `TotalDesconto`, `TotalServicos`, `TotalProdutos`, `Modelo`, `UfPessoa`, `NomePessoa`, `Situacao`, `Finalidade`, `Total`, `DataHoraEntradaSaidaCF`, `DataHoraEmissao`, `ChaveAcesso`, `CpfCnpjPessoa`, `Serie`, `Numero`, `CnpjEmpresa`, `Empresa`, `NaturezaOperacao`, `TipoMovimento`, `TipoMovimentacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterMovimentacoesPorTipoMovimentacao, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaMovimentacoesPorOrigemETipoComImpostos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipoComImpostos.cs) — SHA-256 `542a23efb4c449459710c972e4e07a06f86c56a88f7164eeeff658a5a74e51d4`
- [ListaMovimentacoesPorOrigemETipoComImpostos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipoComImpostos.Designer.cs) — SHA-256 `2199c777c1bffd528482ddfba3315b0f5521f4a41bfb59765b37f80d96ca108f`

## Lista de movimentações por informação de pesquisa

Classe: `ListaMovimentacoesPorInformacaoPesquisa`. Identificador: `MovimentacaoEntrada.ListaMovimentacoesPorInformacaoPesquisa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Pesquisar pela informação de pesquisa; Origem das movimentações; Situação das movimentações; Natureza da operação; Data/Hora inicial; Data/Hora final; Tipo de data; Vendedor; Mostrar itens; Mostrar resumo por origem e situação; Mostrar resumo por dia; Ordenação; Mostrar observação da movimentação; Mostrar observação do item; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro

**Campos apresentados (DTO):** `Observacao`, `EspeciesPagamentoToText`, `FormaPagamento`, `Situacao`, `Finalidade`, `Total`, `DataHoraEntradaSaidaCF`, `DataHoraEmissao`, `ChaveAcesso`, `NomePessoa`, `Serie`, `Numero`, `Empresa`, `CnpjEmpresa`, `NaturezaOperacao`, `TipoMovimento`, `TipoMovimentacao`, `Itens.Quantidade`, `Itens.TotalLiquido`, `Itens.Cfop`, `Itens.TotalBruto`, `Itens.TotalDesconto`, `Itens.PercentualIcms`, `Itens.PrecoUnitario`, `Itens.UnidadeMedida`, `Itens.Descricao`, `Itens.CodigoInterno`, `Itens.InformacaoEstoque`, `Itens.Observacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterMovimentacoesPorTipoMovimentacao, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [ListaMovimentacoesPorInformacaoPesquisa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorInformacaoPesquisa.cs) — SHA-256 `4df617fae0d20a81bea97ae10584fe5f9404431c49c4861419db0ffcd0bf7187`
- [ListaMovimentacoesPorInformacaoPesquisa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorInformacaoPesquisa.Designer.cs) — SHA-256 `0c1865abc32ed45a0aa84983852ecf3c6afd6bad7945d8d74f2b8c1ac28992b1`

## Lista de movimentações por tipo de segmentação

Classe: `ListaMovimentacoesPorOrigemTipoESegmentacao`. Identificador: `MovimentacaoEntrada.ListaMovimentacoesPorOrigemTipoESegmentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Origem das movimentações; Situação das movimentações; Natureza da operação; Data/Hora inicial; Data/Hora final; Tipo de data; Tipo de segmentação; Vendedor; Mostrar itens; Mostrar resumo por origem e situação; Mostrar resumo por dia; Ordenação; Mostrar observação da movimentação; Mostrar observação do item; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro

**Campos apresentados (DTO):** `Observacao`, `EspeciesPagamentoToText`, `FormaPagamento`, `Situacao`, `Finalidade`, `Total`, `DataHoraEntradaSaidaCF`, `DataHoraEmissao`, `ChaveAcesso`, `NomePessoa`, `Serie`, `Numero`, `Empresa`, `CnpjEmpresa`, `NaturezaOperacao`, `TipoMovimento`, `TipoMovimentacao`, `Itens.Quantidade`, `Itens.TotalLiquido`, `Itens.Cfop`, `Itens.TotalBruto`, `Itens.TotalDesconto`, `Itens.PercentualIcms`, `Itens.PrecoUnitario`, `Itens.UnidadeMedida`, `Itens.Descricao`, `Itens.CodigoInterno`, `Itens.InformacaoEstoque`, `Itens.Observacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterMovimentacoesPorTipoMovimentacao, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [ListaMovimentacoesPorOrigemTipoESegmentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemTipoESegmentacao.cs) — SHA-256 `387544db85dfd2afe908801975dbcad584888f632827d2606cc6b3cd4c5a6daf`
- [ListaMovimentacoesPorOrigemTipoESegmentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemTipoESegmentacao.Designer.cs) — SHA-256 `0c8f054f632b8eda4d393eef2b6258f92f7713d8b06bac0ca4eb78474879d352`

## Lista de movimentações simplificado

Classe: `ListaMovimentacoesPorOrigemETipoSimplificado`. Identificador: `MovimentacaoEntrada.ListaMovimentacoesPorOrigemETipoSimplificado`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Origem das movimentações; Situação das movimentações; Natureza da operação; Data/Hora inicial; Data/Hora final; Tipo de data; Pessoa; Vendedor; Ordenação; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro; Agrupar por série

**Campos apresentados (DTO):** `NomePessoa`, `Total`, `DataHoraEmissao`, `ChaveAcesso`, `Numero`, `Serie`, `Empresa`, `CnpjEmpresa`, `NaturezaOperacao`, `TipoMovimento`, `TipoMovimentacao`

**Agrupamentos do layout:**

- `Serie`
- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterMovimentacoesPorTipoMovimentacao, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaMovimentacoesPorOrigemETipoSimplificado.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipoSimplificado.cs) — SHA-256 `06b7cc4b262a91e341aa64f4bb4995388b0e34fc8c80dd5c1d18ed5d88d683d9`
- [ListaMovimentacoesPorOrigemETipoSimplificado.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipoSimplificado.Designer.cs) — SHA-256 `b4fabf5f6a4b921b65b697fc586afb3b737e50dc1de465017165166410796e87`

## Lista de pedidos de compra

Classe: `ListaPedidosDeCompra`. Identificador: `MovimentacaoEntrada.ListaPedidosDeCompra`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Situação; Data/Hora inicial; Data/Hora final; Pessoa; Mostrar itens; Ordenação

**Campos apresentados (DTO):** `Situacao`, `Total`, `DataHoraPrevisaoCF`, `DataHoraEmissao`, `NomePessoa`, `Numero`, `Empresa`, `CnpjEmpresa`, `TipoMovimento`, `TipoMovimentacao`, `Itens.TotalLiquido`, `Itens.TotalBruto`, `Itens.TotalDesconto`, `Itens.PercentualIcms`, `Itens.PrecoUnitario`, `Itens.Quantidade`, `Itens.UnidadeMedida`, `Itens.Descricao`, `Itens.CodigoInterno`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`
- `Iif([DataHoraPrevisao]=='01/01/0001 00:00:00', '' , [DataHoraPrevisao])`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterMovimentacoesPorTipoMovimentacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [ListaPedidosDeCompra.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ListaPedidosDeCompra.cs) — SHA-256 `2e842ba18b4e40eea14ec07547deadb735d13f027d348890626d81d5bb4509fb`
- [ListaPedidosDeCompra.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ListaPedidosDeCompra.Designer.cs) — SHA-256 `6514b1d913edd7eb8b9067c1e11f16cc6c957e326eb1975d5e18a026c385aceb`

## Medicamentos controlados comprados

Classe: `MedicamentosControladosComprados`. Identificador: `MovimentacaoEntrada.MedicamentosControladosComprados`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo de data; Classe terapêutica; Ordenação

**Campos apresentados (DTO):** `Origem`, `Numero`, `Serie`, `DataVenda`, `Codigo`, `RegistroMinisterioSaude`, `Descricao`, `Lote`, `Quantidade`, `CnpjEmpresa`, `Empresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.MedicamentosControladosComprados`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MedicamentosControladosComprados.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/MedicamentosControladosComprados.cs) — SHA-256 `139f1bfc678457360c0a1725c633409bc749f4bac80dd530477f1607eaeaf13e`
- [MedicamentosControladosComprados.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/MedicamentosControladosComprados.Designer.cs) — SHA-256 `3487b86b302f92d019b936156d16e5dd2a007fffacac61c9f9ee4c0890d6c18f`

## Produtos comercializados por fornecedor

Classe: `ProdutosComercializadosPorFornecedor`. Identificador: `MovimentacaoEntrada.ProdutosComercializadosPorFornecedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Fornecedor; Data inicial; Data final; Exibir gráfico; Ordenação

**Campos apresentados (DTO):** `Itens.CodigoInterno`, `Itens.Descricao`, `Itens.QuantidadeAtualNoEstoque`, `Itens.QuantidadeComprada`, `Itens.QuantidadeVendida`, `Itens.PrecoCusto`, `Itens.PrecoVenda`, `Itens.CustoTotal`, `Itens.VendaTotal`, `Itens.Participacao`, `Forncedor`

**Contratos de serviço:**

- `IMovimentacaoEntradaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoEntradaRelatorioModuleService.ProdutosComercializadosPorFornecedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ProdutosComercializadosPorFornecedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/ProdutosComercializadosPorFornecedor.cs) — SHA-256 `44febb1bf27a72e5f1cdfdc0bde948607c2015170848834e04cfa8b24453a5b0`
- [ProdutosComercializadosPorFornecedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/ProdutosComercializadosPorFornecedor.Designer.cs) — SHA-256 `889e1e927a660db3bce66b888de60b868140e32cdbf8dba7defb51ca989e193b`

## Ranking de movimentações por produto

Classe: `RankingMovimentacoesPorProduto`. Identificador: `MovimentacaoEntrada.RankingMovimentacoesPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Origem das movimentações; Situação das movimentações; Ordenação

**Campos apresentados (DTO):** `CodigoInterno`, `Descricao`, `Quantidade`, `ValorMovimentado`, `Participacao`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterRankingMovimentacoesPorProdutos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-entrada.md](../../docs/continuacao-movimentacao-entrada.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [RankingMovimentacoesPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/RankingMovimentacoesPorProduto.cs) — SHA-256 `683e5452f60b67a1433aec8f5007546ef1a91e6a6a57d637b61995001b5b3850`
- [RankingMovimentacoesPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/RankingMovimentacoesPorProduto.Designer.cs) — SHA-256 `9b1bb85a9b2d0c3273b582c3c6c6fb66bc54995d1bb0c8adaa1ad4abbecec5ab`

## Relação de Notas de Entrada

Classe: `RelacaoNotasEntrada`. Identificador: `MovimentacaoEntrada.RelacaoNotasEntrada`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo de data; Data inicial; Data final

**Campos apresentados (DTO):** `Destinatario`, `Numero`, `Data`, `ValorBaseIcms`, `ValorIcms`, `ValorBaseIcmsSt`, `ValorIcmsSt`, `ValorTotal`, `TotalValorBaseIcms`, `TotalValorIcms`, `TotalBaseIcmsSt`, `TotalValorIcmsSt`, `Total`

**Expressões calculadas do layout:**

- `Sum([ValorBaseIcms])`
- `Sum([ValorIcms])`
- `Sum([ValorBaseIcmsSt])`
- `Sum([ValorIcmsSt])`
- `Sum([ValorTotal])`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterRelacaoNotasEntrada`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-entrada.md](../../docs/continuacao-movimentacao-entrada.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [RelacaoNotasEntrada.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Entrada/RelacaoNotasEntrada.cs) — SHA-256 `ba4537a332a03590d60ffcbd42830173420bd48a458b595254f37a94d6c91c94`
- [RelacaoNotasEntrada.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Entrada/RelacaoNotasEntrada.Designer.cs) — SHA-256 `55d31d01bdda17f75cba3e05681bd155878f5997d12269d0a3989a0c5a11e061`

## Relação de Notas Fiscais

Classe: `RelacaoDeNotasFiscais`. Identificador: `MovimentacaoEntrada.RelacaoDeNotasFiscais`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo de data; Data inicial; Data final

**Campos apresentados (DTO):** `DataHoraEmissao`, `Icms`, `Outros`, `NomePessoa`, `ValorContabil`, `Isento`, `Numero`, `ValorBaseIcms`, `DataHoraMovimento`, `TotalValorBaseIcms`, `TotalIsento`, `TotalOutros`, `TotalValorContabil`, `TotalIcms`

**Expressões calculadas do layout:**

- `Sum([ValorContabil])`
- `Sum([ValorBaseIcms])`
- `Sum([Icms])`
- `Sum([Isento])`
- `Sum([Outros])`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterRelacaoDeNotasFiscais`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-entrada.md](../../docs/continuacao-movimentacao-entrada.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [RelacaoDeNotasFiscais.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/RelacaoDeNotasFiscais.cs) — SHA-256 `c1769d2c0183ae2d43ae5f988e3b15ff9f8f734926ba2c44ee6623e215b290ac`
- [RelacaoDeNotasFiscais.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/RelacaoDeNotasFiscais.Designer.cs) — SHA-256 `7dc637eef389f4de3cb43a8b80e145682f8323464f37b0245e88d2a69727ff70`

## Resumo diário por CFOP e CST do COFINS

Classe: `ResumoDiarioPorCfopCstCofins`. Identificador: `MovimentacaoEntrada.ResumoDiarioPorCfopCstCofins`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Origem; Data inicial; Data final; Tipo de data

**Campos apresentados (DTO):** `ItensResumoDiarioPorCfopCofins.Total`, `ItensResumoDiarioPorCfopCofins.CstCsosn`, `ItensResumoDiarioPorCfopCofins.Cfop`, `ItensResumoDiarioPorCfopCofins.Data`

**Agrupamentos do layout:**

- `Data`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterTotalizacaoCfopPorDia`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoDiarioPorCfopCstCofins.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoDiarioPorCfopCstCofins.cs) — SHA-256 `04b7e1bb9bf5f1c5c369b7f28fc86d7457979699bc181eff2d38ea9f38317eda`
- [ResumoDiarioPorCfopCstCofins.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoDiarioPorCfopCstCofins.Designer.cs) — SHA-256 `c567dcb81851560d56d9c2ae1fc94650829edbbb884f7b980e84683d210c2a2b`

## Resumo diário por CFOP e CST do PIS

Classe: `ResumoDiarioPorCfopCstPis`. Identificador: `MovimentacaoEntrada.ResumoDiarioPorCfopCstPis`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Origem; Data inicial; Data final; Tipo de data

**Campos apresentados (DTO):** `ItensResumoDiarioPorCfopPis.Total`, `ItensResumoDiarioPorCfopPis.CstCsosn`, `ItensResumoDiarioPorCfopPis.Cfop`, `ItensResumoDiarioPorCfopPis.Data`

**Agrupamentos do layout:**

- `Data`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterTotalizacaoCfopPorDia`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoDiarioPorCfopCstPis.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoDiarioPorCfopCstPis.cs) — SHA-256 `d15cf8e157461341c154bf0fd1412482f19973d907ebfb4762e237d7bb43cba3`
- [ResumoDiarioPorCfopCstPis.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoDiarioPorCfopCstPis.Designer.cs) — SHA-256 `2a17fe5e6427729c70eb41b809de02996ec1f9aad6aeaaebb4a657bc6c27443a`

## Resumo diário por CFOP e CST/CSOSN

Classe: `ResumoDiarioPorCfopCst`. Identificador: `MovimentacaoEntrada.ResumoDiarioPorCfopCst`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Origem; Data inicial; Data final; Tipo de data

**Campos apresentados (DTO):** `ItensResumoDiarioPorCfop.Total`, `ItensResumoDiarioPorCfop.CstCsosn`, `ItensResumoDiarioPorCfop.Cfop`, `ItensResumoDiarioPorCfop.Data`

**Agrupamentos do layout:**

- `Data`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterTotalizacaoCfopPorDia`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoDiarioPorCfopCst.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoDiarioPorCfopCst.cs) — SHA-256 `65d0e3d03fe001e8cec904f64cd187711abe9e33d6c4f1effb61d02bd4135471`
- [ResumoDiarioPorCfopCst.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoDiarioPorCfopCst.Designer.cs) — SHA-256 `3742e1bec5436db99ba6d114761935dcdfc5531d48375dc6c43ec24786b24397`

## Resumo PIS e COFINS

Classe: `ResumoPisCofins`. Identificador: `MovimentacaoEntrada.ResumoPisCofins`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** CST PIS COFINS ; Natureza da operação; Origem; Data inicial; Data final; Tipo de data; Mostrar itens

**Campos apresentados (DTO):** `ItensPis.TotalPisCofins`, `ItensPis.TotalBase`, `ItensPis.Codigo`, `ItensPis.Descricao`, `ItensPis.Percentual`, `ItensPis.Itens.ValorPis`, `ItensPis.Itens.TotalLiquido`, `ItensPis.Itens.PrecoUnitario`, `ItensPis.Itens.Quantidade`, `ItensPis.Itens.Descricao`, `ItensPis.Itens.CodigoInterno`, `ItensCofins.TotalBase`, `ItensCofins.TotalPisCofins`, `ItensCofins.Percentual`, `ItensCofins.Descricao`, `ItensCofins.Codigo`, `ItensCofins.Itens.CodigoInterno`, `ItensCofins.Itens.Descricao`, `ItensCofins.Itens.Quantidade`, `ItensCofins.Itens.PrecoUnitario`, `ItensCofins.Itens.TotalLiquido`, `ItensCofins.Itens.ValorCofins`, `ItensPis.Total`, `ItensCofins.Total`

**Agrupamentos do layout:**

- `Pis.CstPISCOFINS.Codigo`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterItensMovimentadosPorTipoMovimento, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoPisCofins.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoPisCofins.cs) — SHA-256 `320e3f29a7dac60d8b0c0b1c9b909eedd00064128c545ba2b8f5d414c674506c`
- [ResumoPisCofins.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoPisCofins.Designer.cs) — SHA-256 `752b8604007867d0f8037c0d249f2186e4bf80bb3a2e930b13f5c54fef127830`

## Resumo PIS e COFINS diário

Classe: `ResumoPisCofinsDiario`. Identificador: `MovimentacaoEntrada.ResumoPisCofinsDiario`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** CST PIS COFINS ; Natureza da operação; Data inicial; Data final; Tipo de data

**Campos apresentados (DTO):** `ItensPis.Codigo`, `ItensPis.Descricao`, `ItensPis.Percentual`, `ItensPis.Total`, `ItensPis.TotalBase`, `ItensPis.TotalPisCofins`, `ItensCofins.Codigo`, `ItensCofins.Descricao`, `ItensCofins.Percentual`, `ItensCofins.Total`, `ItensCofins.TotalBase`, `ItensCofins.TotalPisCofins`, `ItensPis.Data`, `ItensCofins.Data`

**Agrupamentos do layout:**

- `Data`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterResumoPisCofinsDiario, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoPisCofinsDiario.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoPisCofinsDiario.cs) — SHA-256 `863caf1ca83e92b3603231a3adb37640b5b00ed1b33b84aad425e7dd58a6a342`
- [ResumoPisCofinsDiario.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoPisCofinsDiario.Designer.cs) — SHA-256 `0aab9467cfc7619506e492a3e35caec7927baa4e188c6b60585d0dd32591b059`

## Resumo PIS e COFINS por movimentação

Classe: `ResumoPisCofinsMovimentacao`. Identificador: `MovimentacaoEntrada.ResumoPisCofinsMovimentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** CST PIS COFINS ; Natureza da operação; Origem; Data inicial; Data final; Tipo de data

**Campos apresentados (DTO):** `ItensPis.Descricao`, `ItensPis.TotalPisCofins`, `ItensPis.TotalBase`, `ItensPis.Total`, `ItensPis.Percentual`, `ItensPis.Codigo`, `ItensPis.Itens.BasePis`, `ItensPis.Itens.DataMovimentacao`, `ItensPis.Itens.TotalLiquido`, `ItensPis.Itens.ValorPis`, `ItensPis.Itens.Serie`, `ItensPis.Itens.NumeroMovimentacao`, `ItensPis.Itens.Origem`, `ItensCofins.TotalPisCofins`, `ItensCofins.TotalBase`, `ItensCofins.Total`, `ItensCofins.Percentual`, `ItensCofins.Descricao`, `ItensCofins.Codigo`, `ItensCofins.Itens.BaseCofins`, `ItensCofins.Itens.ValorCofins`, `ItensCofins.Itens.TotalLiquido`, `ItensCofins.Itens.Serie`, `ItensCofins.Itens.NumeroMovimentacao`, `ItensCofins.Itens.DataMovimentacao`, `ItensCofins.Itens.Origem`, `Itens.CodigoPis`, `Itens.CodigoInterno`, `Itens.Descricao`, `Itens.Quantidade`, `Itens.PrecoUnitario`, `Itens.ValorPis`, `Itens.ValorCofins`

**Agrupamentos do layout:**

- `Origem`
- `Serie`
- `NumeroMovimentacao`
- `CodigoInterno`
- `Descricao`
- `PrecoUnitario`
- `CodigoCofins`
- `CodigoPis`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterItensMovimentadosPorTipoMovimento, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoPisCofinsMovimentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoPisCofinsMovimentacao.cs) — SHA-256 `d4f5d403fbeac714353871f468c34ecc2d62aa35680756917b4e68b0d3b7c50e`
- [ResumoPisCofinsMovimentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoPisCofinsMovimentacao.Designer.cs) — SHA-256 `c85e131afde0c55b6ee222725ed1c5cb1caf0721acd345e84cd7d434716fa78f`

## Resumo por CFOP

Classe: `ResumoPorCfop`. Identificador: `MovimentacaoEntrada.ResumoPorCfop`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; CFOP; Data inicial; Data final; Tipo de data

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterTotalizacaoCfop`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoPorCfop.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoPorCfop.cs) — SHA-256 `36e352da6c0d369422995600acd71be54b29a5a55451fb12d08b9a0cde968d98`
- [ResumoPorCfop.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoPorCfop.Designer.cs) — SHA-256 `914e43ae6c2dfa21d4972941c9233f2009c2afe9d1e78e914a36bfdfa1b0ac60`

## Resumo por movimentação

Classe: `ResumoPorMovimentacao`. Identificador: `MovimentacaoEntrada.ResumoPorMovimentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Movimentação; Mostrar itens; Mostrar observação da movimentação; Tipo preço custo

**Campos apresentados (DTO):** `Observacao`, `EspeciesPagamentoToText`, `FormaPagamento`, `Situacao`, `Finalidade`, `Total`, `DataHoraEntradaSaidaCF`, `DataHoraEmissao`, `ChaveAcesso`, `NomePessoa`, `Serie`, `Numero`, `Empresa`, `CnpjEmpresa`, `NaturezaOperacao`, `TipoMovimento`, `TipoMovimentacao`, `Itens.PrecoCustoTotal`, `Itens.Quantidade`, `Itens.PrecoVendaVigente`, `Itens.Cfop`, `Itens.TotalLiquido`, `Itens.TotalBruto`, `Itens.TotalDesconto`, `Itens.PercentualIcms`, `Itens.PrecoUnitario`, `Itens.UnidadeMedida`, `Itens.Descricao`, `Itens.CodigoInterno`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterResumoPorMovimentacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ResumoPorMovimentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ResumoPorMovimentacao.cs) — SHA-256 `584c3e8cd8a031cebe485e0fc40e396dc578b43aac469d00a08b6bad00611af5`
- [ResumoPorMovimentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ResumoPorMovimentacao.Designer.cs) — SHA-256 `02dc7c5a7f46f260164ad75c152f1ed9c8f2804873c5590e4493b05deef3de0e`

## Totalização por NCM

Classe: `TotalizacaoPorNcm`. Identificador: `MovimentacaoEntrada.TotalizacaoPorNcm`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo de data

**Campos apresentados (DTO):** `Total`, `TotalCofins`, `Quantidade`, `Desconto`, `OutrasDespesas`, `TotalIcms`, `TotalIcmsSt`, `TotalIpi`, `TotalPis`, `Descricao`, `Ncm`

**Agrupamentos do layout:**

- `Cfop`
- `Descricao`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterTotalizacaoPorNcm`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [TotalizacaoPorNcm.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/TotalizacaoPorNcm.cs) — SHA-256 `84b51cb7168861e20500bcfd0a3cd24e786569f7d047aa6594e2d4594d07b875`
- [TotalizacaoPorNcm.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/TotalizacaoPorNcm.Designer.cs) — SHA-256 `ff1a215398da895d8f72aa43469bcc698f384cd2ba96b67ac31a246f62b4503e`

