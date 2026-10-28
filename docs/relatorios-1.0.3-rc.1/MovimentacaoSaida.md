# Movimentação saída

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Apuração de ICMS

Classe: `ApuracaoIcms`. Identificador: `MovimentacaoSaida.ApuracaoIcms`.

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

Classe: `ApuracaoIcmsPorMovimentacao`. Identificador: `MovimentacaoSaida.ApuracaoIcmsPorMovimentacao`.

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

Classe: `ApuracaoPisCofins`. Identificador: `MovimentacaoSaida.ApuracaoPisCofins`.

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

Classe: `ApuracaoPisCofinsItens`. Identificador: `MovimentacaoSaida.ApuracaoPisCofinsItens`.

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

Classe: `ApuracaoPisCofinsItensDetalhado`. Identificador: `MovimentacaoSaida.ApuracaoPisCofinsItensDetalhado`.

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

## Apuração Sintegra

Classe: `ApuracaoSintegra`. Identificador: `MovimentacaoSaida.ApuracaoSintegra`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** ECF; Data inicial; Data final

**Campos apresentados (DTO):** `Data`, `NumeroEcf`, `Crz`, `ValorVendaBruta`, `ValorDesconto`, `ValorCancelamento`, `ValorVendaLiquida`, `ValorBaseIcms`, `ValorIcmsSubstituicao`, `ValorIsento`, `ValorNaoTributado`, `calculatedField1`, `calculatedField3`, `calculatedField2`, `calculatedField4`, `calculatedField5`, `calculatedField6`, `calculatedField7`, `TotalValorNaoTributado`

**Agrupamentos do layout:**

- `Data`
- `NumeroEcf`

**Expressões calculadas do layout:**

- `Sum([ValorVendaBruta])`
- `Sum([ValorCancelamento])`
- `Sum([ValorDesconto])`
- `Sum([ValorVendaLiquida])`
- `Sum([ValorBaseIcms])`
- `Sum([ValorIcmsSubstituicao])`
- `Sum([ValorIsento])`
- `Sum([ValorNaoTributado])`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterApuracoesSintegra`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Values come from PAF-ECF R02/R03 records, not from Movimentacoes

**Fontes da ficha:**

- [ApuracaoSintegra.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ApuracaoSintegra.cs) — SHA-256 `999b4926b5a84a5e1cdf2361333da9c123f9b94a3ef3d3bdd2ea915299ad8fa5`
- [ApuracaoSintegra.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ApuracaoSintegra.Designer.cs) — SHA-256 `6960b279abe549d9a67507daf5f4446d25fa1ff690bfebbcef482285ba86c0e3`

## Cartas de correção

Classe: `RelatorioCartasCorrecao`. Identificador: `MovimentacaoSaida.RelatorioCartasCorrecao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Situação

**Campos apresentados (DTO):** `DataInclusao`, `ChaveAcessoDocumentoFiscal`, `Sequencia`, `Descricao`, `Motivo`, `count`

**Expressões calculadas do layout:**

- `[].Count()`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterCartasCorrecao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Company restricted by CNPJ rather than by empresa reference

**Fontes da ficha:**

- [RelatorioCartasCorrecao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/RelatorioCartasCorrecao.cs) — SHA-256 `be32ccca5e1da06de287a96b86264bfc3786cfbdb25a22ab3e392ac1b2e3b53a`
- [RelatorioCartasCorrecao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/RelatorioCartasCorrecao.Designer.cs) — SHA-256 `402f1695a092d6105374af5e0d6da07aea608661573dbe0b24c6260f87f7011c`

## Componentes das composições vendidas

Classe: `ComponentesComposicoesVendidas`. Identificador: `MovimentacaoSaida.ComponentesComposicoesVendidas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `Quantidade`, `Descricao`, `CodigoInterno`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterComponentesVendidosPorPeriodo`

**Pesquisa da regra:** Rastreado em profundidade; WCF acrescenta a empresa da identidade ao serviço de aplicação.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ComponentesComposicoesVendidas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ComponentesComposicoesVendidas.cs) — SHA-256 `8c298c88d5e58880a9f7fac6858850af9eb61e9dd0b626bfd3eec0f3387e9148`
- [ComponentesComposicoesVendidas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ComponentesComposicoesVendidas.Designer.cs) — SHA-256 `22ea34ee14d83023e2ea1d194732eced833676f1b70eb80ed4638268788ca85d`

## Composições vendidas

Classe: `ComposicoesVendidas`. Identificador: `MovimentacaoSaida.ComposicoesVendidas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `Total`, `Desconto`, `Preco`, `Quantidade`, `Codigo`, `Descricao`, `Compontes.Quantidade`, `Compontes.Codigo`, `Compontes.Descricao`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterComposicoesVendidas`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ComposicoesVendidas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ComposicoesVendidas.cs) — SHA-256 `e7df7a2ad8bf19ce099c0cb38ebc95dbdec7f5ecd1382e5bf4ca5e8c7f613d62`
- [ComposicoesVendidas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ComposicoesVendidas.Designer.cs) — SHA-256 `43f5f5235a24e10c4e1c26c489aa9cefea423f694ca55a6c1e90a08f4f1ed87e`

## Crédito presumido sintético - Sistemática atacadista

Classe: `RelatorioCreditoPresumidoSistematicaAtacadista`. Identificador: `MovimentacaoSaida.RelatorioCreditoPresumidoSistematicaAtacadista`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `CreditoPresumido2`, `CreditoPresumido1`, `ValorAgregado`, `Agregado`, `RecolhimentoReferente2`, `RecolhimentoReferente1`, `IcmsCreditado`, `ValorContabil`, `Cfop`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterApuracoesSistematicaAtacadista`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Fixed 35%/1.1%/6% percentages; output block repeated per row

**Fontes da ficha:**

- [RelatorioCreditoPresumidoSistematicaAtacadista.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/RelatorioCreditoPresumidoSistematicaAtacadista.cs) — SHA-256 `c3738c2606b649390bff611cf7a3514d8d8e9274a72f68a373471567316bed98`
- [RelatorioCreditoPresumidoSistematicaAtacadista.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/RelatorioCreditoPresumidoSistematicaAtacadista.Designer.cs) — SHA-256 `c9ad4369f99bf7532afd98a66db59c110e61cdabb10ed824660f9b952bd3bbe8`

## DAVs por numerações específicas

Classe: `DavsEspecificasPorNumero`. Identificador: `MovimentacaoSaida.DavsEspecificasPorNumero`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Numerações específicas; Ordenação; Ordenação itens agrupados; Exibir detalhes dos itens; Exibir quantidade de registros por situação

**Campos apresentados (DTO):** `Movimentacoes.DataPrevisao`, `Movimentacoes.Pagamento`, `Movimentacoes.Situacao`, `Movimentacoes.Total`, `Movimentacoes.TotalProdutos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Total`, `ItensAcumulados.Quantidade`, `ItensAcumulados.Total`, `ItensAcumulados.Descricao`, `ItensAcumulados.CodigoInterno`, `ResumoPagamentos.Total`, `ResumoPagamentos.Descricao`, `ResumoSituacoes.Situacao`, `ResumoSituacoes.Quantidade`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.DavOuPreVendaOuOrcamentoPorNumero`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No Ativo() or situation predicate in the analysed method

**Fontes da ficha:**

- [DavsEspecificasPorNumero.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/DavsEspecificasPorNumero.cs) — SHA-256 `66bbecbe4efc0d6dcfe8ee64d017cbabef71321fca523f562f1e001f5621dfbf`
- [DavsEspecificasPorNumero.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/DavsEspecificasPorNumero.Designer.cs) — SHA-256 `3686bc2bef71129f624d770c461ae9843846f978a9730daffd56d0d2b8082172`

## DAVs por período

Classe: `DavsPorPeriodo`. Identificador: `MovimentacaoSaida.DavsPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Situação das movimentações; Pessoa; Vendedor; Marca; Tipo do filtro de data; Data inicial; Data final; Ordenação; Ordenação itens agrupados; Exibir detalhes dos itens; Exibir quantidade de registros por situação; Exibir 'Agrupado por item concluído'; Exibir 'Agrupado por item cancelado'

**Campos apresentados (DTO):** `Movimentacoes.DataPrevisao`, `Movimentacoes.Pagamento`, `Movimentacoes.Situacao`, `Movimentacoes.Total`, `Movimentacoes.TotalProdutos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Total`, `ItensAcumulados.Quantidade`, `ItensAcumulados.Total`, `ItensAcumulados.Descricao`, `ItensAcumulados.CodigoInterno`, `ResumoPagamentos.Total`, `ResumoPagamentos.Descricao`, `ResumoSituacoes.Situacao`, `ResumoSituacoes.Quantidade`, `ItensAcumuladosCancelados.CodigoInterno`, `ItensAcumuladosCancelados.Descricao`, `ItensAcumuladosCancelados.Total`, `ItensAcumuladosCancelados.Quantidade`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.DavOuPreVendaOuOrcamentoPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Date-type codes follow the screen convention (1 = previsao), not TiposFiltroData

**Fontes da ficha:**

- [DavsPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/DavsPorPeriodo.cs) — SHA-256 `5d6a5cbf811e6362ece5ddfce424796f4db663dddf85466d894cf104b2681439`
- [DavsPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/DavsPorPeriodo.Designer.cs) — SHA-256 `8ba0ceb0b93c336042b07386b5e9eb38d462a7c6962e99989e3e44f91e6c82fd`

## DAVs-OS por período

Classe: `DavsOsPorPeriodo`. Identificador: `MovimentacaoSaida.DavsOsPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo DAV-OS; Situação das movimentações; Pessoa; Vendedor; Tipo do filtro de data; Data inicial; Data final; Ordenação; Ordenação itens agrupados; Tipo do filtro de status; Atendimento; Exibir detalhes dos itens; Exibir quantidade de registros por situação e status; Exibir defeito do objeto; Exibir 'Agrupado por item concluído'; Exibir 'Agrupado por item cancelado'

**Campos apresentados (DTO):** `Movimentacoes.PlacaSerialObjeto`, `Movimentacoes.TotalProdutos`, `Movimentacoes.DataPrevisao`, `Movimentacoes.Vendedor`, `Movimentacoes.Atendimento`, `Movimentacoes.Status`, `Movimentacoes.Total`, `Movimentacoes.TotalServicos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.Vendedor`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Total`, `Movimentacoes.DefeitosObjeto.Descricao`, `ItensAcumulados.Quantidade`, `ItensAcumulados.Total`, `ItensAcumulados.Descricao`, `ItensAcumulados.CodigoInterno`, `ResumoPagamentos.Total`, `ResumoPagamentos.Descricao`, `ResumoSituacoes.Situacao`, `ResumoSituacoes.Quantidade`, `ResumoStatus.Situacao`, `ResumoStatus.Quantidade`, `ItensAcumuladosCancelados.CodigoInterno`, `ItensAcumuladosCancelados.Descricao`, `ItensAcumuladosCancelados.Total`, `ItensAcumuladosCancelados.Quantidade`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.DavOsPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Termino filter is applied in memory after the query

**Fontes da ficha:**

- [DavsOsPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/DavsOsPorPeriodo.cs) — SHA-256 `559b71eb81df1b979e7fc49529cb347218fbeafadc1b1425e5ba266582a59ee8`
- [DavsOsPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/DavsOsPorPeriodo.Designer.cs) — SHA-256 `a9c5b371760d52b8bfd293845bdc4b5dbee761e0d08298b22c06806373ab069d`

## Histórico do objeto

Classe: `HistoricoObjeto`. Identificador: `MovimentacaoSaida.HistoricoObjeto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Objeto; Data inicial; Data final; Tipo do filtro de data; Situação da movimentação; Técnico; Ocultar valores monetários

**Campos apresentados (DTO):** `PlacaSerial`, `Descricao`, `Veiculo.TipoCombustivel`, `Veiculo.AnoFabricacao`, `Veiculo.Cor`, `Veiculo.Marca`, `Veiculo.Chassi`, `Veiculo.Renavam`, `Veiculo.Modelo`, `DavsOs.Status`, `DavsOs.DataHoraEmissao`, `DavsOs.Defeito`, `DavsOs.Diagnostico`, `DavsOs.DataFinalGarantia`, `DavsOs.Situacao`, `DavsOs.DataHoraEntrega`, `DavsOs.DataHoraPrevisao`, `DavsOs.DataHoraTermino`, `DavsOs.DataHoraRecebimento`, `DavsOs.Prioridade`, `DavsOs.Tecnico`, `DavsOs.Total`, `DavsOs.TotalServicos`, `DavsOs.TotalProdutos`, `DavsOs.Cliente`, `DavsOs.TipoAtendimento`, `DavsOs.Numero`, `DavsOs.Itens.Total`, `DavsOs.Itens.Preco`, `DavsOs.Itens.Quantidade`, `DavsOs.Itens.Produto`, `CamposPersonalizados.Informacao`, `CamposPersonalizados.Descricao`, `Veiculo.Cilindrada`, `Veiculo.Potencia`, `Veiculo.Segmento`, `Veiculo.Motor`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterHistoricoObjeto`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Repository materialises Objeto and Movimentacao collections in memory
- Null person and cast failures are static reading, not reproduced

**Fontes da ficha:**

- [HistoricoObjeto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/HistoricoObjeto.cs) — SHA-256 `e5b36b46ddff924ea3e3ee1d28397ccab7dd5fddc7021007780f50ed01653c20`
- [HistoricoObjeto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/HistoricoObjeto.Designer.cs) — SHA-256 `c15f0abd1964536229eb259daa457b80974b65bcb1100c2fdd0f3512ef8dda72`

## Itens movimentados com dados de tributação do FCP

Classe: `ItensVendidosComTributacaoFcp`. Identificador: `MovimentacaoSaida.ItensVendidosComTributacaoFcp`.

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

## Itens movimentados por departamento

Classe: `ItensMovimentadosPorDepartamento`. Identificador: `MovimentacaoSaida.ItensMovimentadosPorDepartamento`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Departamento; Data/Hora inicial; Data/Hora final; Situação; Origem; Tributação; Natureza da operação; Ordenação; Exibir pesos bruto e líquido; Marca; SubGrupo

**Campos apresentados (DTO):** `PesoLiquido`, `PesoBruto`, `Preco`, `Total`, `Desconto`, `Quantidade`, `Unidade`, `Subgrupo`, `Descricao`, `CodigoInterno`, `DepartamentoDescricao`, `Empresa`, `CnpjEmpresa`, `Marca`

**Agrupamentos do layout:**

- `DepartamentoReferencia`
- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterItensMovimentadosPorDepartamento, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Brand and subgroup filters run in the screen, after the service call

**Fontes da ficha:**

- [ItensMovimentadosPorDepartamento.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorDepartamento.cs) — SHA-256 `b87844066388ec0e635c6ae81a836da2043a09cd4f0feae2457b58fc39c42b1d`
- [ItensMovimentadosPorDepartamento.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorDepartamento.Designer.cs) — SHA-256 `e9eaef2dbae2f205ff0d53c937062d648a6beb523bf1005f2b85dc941223533f`

## Itens movimentados por linha de produto

Classe: `ItensMovimentadosPorLinha`. Identificador: `MovimentacaoSaida.ItensMovimentadosPorLinha`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo do filtro de data; Origem; Situação das movimentações; Linha de produto; Vendedor; Ordenação; Natureza da operação; Mostrar dados da pessoa; Exibir 'Código do fabricante'

**Campos apresentados (DTO):** `NomePessoa`, `CodigoInterno`, `Descricao`, `Quantidade`, `Preco`, `Desconto`, `Total`, `CodigoFabricante`, `Linha`, `CnpjEmpresa`, `Empresa`, `Seriais.Serial`, `Grades.Linha`, `Grades.Coluna`, `Grades.Quantidade`, `Grades.Seriais.Serial`, `Lotes.Lote`, `Lotes.Validade`, `Lotes.Quantidade`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Linha`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosPorLinha, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensMovimentadosPorLinha.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorLinha.cs) — SHA-256 `55bf018f4252a7a51dad3b0fdc01fbcf7467315f38f1db39a32fcbc49337e56f`
- [ItensMovimentadosPorLinha.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorLinha.Designer.cs) — SHA-256 `dd4fb3110be6906ab43685140341bcd98a7cdeabcb425d722ba673efab6353e7`

## Itens movimentados por marca

Classe: `ItensMovimentadosPorMarca`. Identificador: `MovimentacaoSaida.ItensMovimentadosPorMarca`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo do filtro de data; Origem; Situação das movimentações; Marca; Vendedor; Ordenação; Natureza da operação; Mostrar dados da pessoa; Exibir 'Código do fabricante'

**Campos apresentados (DTO):** `NomePessoa`, `CodigoInterno`, `Descricao`, `Quantidade`, `Preco`, `Desconto`, `Total`, `CodigoFabricante`, `Marca`, `CnpjEmpresa`, `Empresa`, `Seriais.Serial`, `Grades.Linha`, `Grades.Coluna`, `Grades.Quantidade`, `Grades.Seriais.Serial`, `Lotes.Lote`, `Lotes.Validade`, `Lotes.Quantidade`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Marca`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosPorMarca, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensMovimentadosPorMarca.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorMarca.cs) — SHA-256 `e520615050b94a143f064e97a6c2509856eeb0e7683c48869a79c88b603cbaf0`
- [ItensMovimentadosPorMarca.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorMarca.Designer.cs) — SHA-256 `6c3f4d851bdc677f8f91484a7162a005e5fb7541825fa40b3c5003ee17eba4d1`

## Itens movimentados por marca - agrupado por subgrupo

Classe: `ItensMovimentadosPorMarcaAgrupadoPorSubgrupo`. Identificador: `MovimentacaoSaida.ItensMovimentadosPorMarcaAgrupadoPorSubgrupo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo do filtro de data; Origem; Situação das movimentações; Marca; Vendedor; Ordenação; Natureza da operação; Mostrar dados da pessoa; Exibir 'Código do fabricante'

**Campos apresentados (DTO):** `CodigoInterno`, `Descricao`, `Quantidade`, `Preco`, `Desconto`, `Total`, `Subgrupo`, `Seriais.Serial`, `Grades.Linha`, `Grades.Coluna`, `Grades.Quantidade`, `Grades.Seriais.Serial`, `Lotes.Lote`, `Lotes.Validade`, `Lotes.Quantidade`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`, `CnpjEmpresa`, `Empresa`, `Marca`, `NomePessoa`, `CodigoFabricante`

**Agrupamentos do layout:**

- `Subgrupo`
- `CnpjEmpresa`
- `Marca`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosPorMarca, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensMovimentadosPorMarcaAgrupadoPorSubgrupo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorMarcaAgrupadoPorSubgrupo.cs) — SHA-256 `57c284d257b6aa945d49b2601ddbb64dd29d395c146a4de6ed1c3f490edc47c2`
- [ItensMovimentadosPorMarcaAgrupadoPorSubgrupo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorMarcaAgrupadoPorSubgrupo.Designer.cs) — SHA-256 `983fa9d60d604c39d23070fd10698ee2f6cda6a3c4229eb037033484fe8958fe`

## Itens movimentados por origem

Classe: `ItensMovimentadosPorOrigem`. Identificador: `MovimentacaoSaida.ItensMovimentadosPorOrigem`.

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

## Itens movimentados por promoção

Classe: `ItensMovimentadosPorPromocao`. Identificador: `MovimentacaoSaida.ItensMovimentadosPorPromocao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Promoção; Data/Hora inicial; Data/Hora final; Tipo do filtro de data; Origem; Situação das movimentações; Vendedor; Ordenação

**Campos apresentados (DTO):** `CodigoInterno`, `Descricao`, `Quantidade`, `Preco`, `Desconto`, `Total`, `CnpjEmpresa`, `Empresa`, `Seriais.Serial`, `Grades.Linha`, `Grades.Coluna`, `Grades.Quantidade`, `Grades.Seriais.Serial`, `Lotes.Lote`, `Lotes.Validade`, `Lotes.Quantidade`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Marca`

**Contratos de serviço:**

- `IPromocaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosPorPromocao, IPromocaoModuleService.ObterPeriodoPromocao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensMovimentadosPorPromocao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorPromocao.cs) — SHA-256 `59b85b6f9bfff2bba734496806edd8fdfbb5fd2990ad6f86cd3a60453e11aed0`
- [ItensMovimentadosPorPromocao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorPromocao.Designer.cs) — SHA-256 `07149c5b2dbffbbff982385d469da503637a8071c6fbedd56b6493488535dea1`

## Itens movimentados por vendedor

Classe: `ItensMovimentadosPorVendedor`. Identificador: `MovimentacaoSaida.ItensMovimentadosPorVendedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Data e hora inicial; Data e hora final; Origem; Situação

**Campos apresentados (DTO):** `TotalLiquido`, `Outras`, `Desconto`, `PrecoUnitario`, `Quantidade`, `Codigo`, `Descricao`, `VendedorNome`, `NaturezaOperacao`, `Origem`, `Pessoa`, `Serie`, `Modelo`, `NumeroMovimentacao`, `Data`

**Agrupamentos do layout:**

- `VendedorNome`
- `VendedorId`
- `Origem`
- `NaturezaOperacao`
- `NumeroMovimentacao`
- `Data`
- `Modelo`
- `Serie`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensMovimentadosPorVendedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensMovimentadosPorVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorVendedor.cs) — SHA-256 `88d41323095e4e335c42b80cb57480c1a51ad431ce590483b4be305bd1e666b0`
- [ItensMovimentadosPorVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensMovimentadosPorVendedor.Designer.cs) — SHA-256 `df1e3fc1fd13ab7b588fb1d2ee0dd43f4c842729674413da5475823508b49aac`

## Itens totalizados e agrupados por pessoa

Classe: `ItensVendidosTotalizadosPorPessoa`. Identificador: `MovimentacaoSaida.ItensVendidosTotalizadosPorPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Pessoa; Origem das movimentações; Situação das movimentações; Natureza da operação; Ordenação

**Campos apresentados (DTO):** `Total`, `Preco`, `CodigoBarras`, `Quantidade`, `CodigoInterno`, `Descricao`, `NomePessoa`, `CpfCnpjPessoa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `CpfCnpjPessoa`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosTotalizadosPorPessoa, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosTotalizadosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosTotalizadosPorPessoa.cs) — SHA-256 `9b30ee4313851a37a650a51361090394b41b3b32d10ed5712d73b265ac59aa7e`
- [ItensVendidosTotalizadosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosTotalizadosPorPessoa.Designer.cs) — SHA-256 `15843b70daff814152b7e50d914778839d057953e850137fc7ab3c980f20fe02`

## Itens vendidos

Classe: `ItensVendidos`. Identificador: `MovimentacaoSaida.ItensVendidos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final; Tipo; Ordenação; Agrupar itens vendidos com base no preço de venda

**Campos apresentados (DTO):** `Saldo`, `PercentualDesconto`, `OutrasDespesas`, `QuantidadeMovimentacoes`, `CodigoInterno`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`, `Seriais.Serial`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Grades.Seriais.Serial`, `Lotes.Quantidade`, `Lotes.Validade`, `Lotes.Lote`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`, `Componentes.QuantidadeVendida`, `Componentes.QuantidadeComposicao`, `Componentes.Descricao`, `Abastecimentos.Volume`, `Abastecimentos.Bico`, `Abastecimentos.Bomba`, `Abastecimentos.Tanque`, `GradesGroup.Linha`, `GradesGroup.Coluna`, `GradesGroup.Quantidade`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidos.cs) — SHA-256 `c480de1a4c5a12258457f3dec6bb54ce9400b81255da515b72267dcd219e3a1e`
- [ItensVendidos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidos.Designer.cs) — SHA-256 `eae6983d19cde1df2c54ad42a3179a3fdf2206d4d198925ada596756b8e066c7`

## Itens vendidos - 40 colunas

Classe: `ItensVendidos40Colunas`. Identificador: `MovimentacaoSaida.ItensVendidos40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final; Tipo; Ordenação; Agrupar itens vendidos com base no preço de venda

**Campos apresentados (DTO):** `QuantidadeMovimentacoes`, `CodigoInterno`, `Total`, `Preco`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`, `OutrasDespesas`, `Desconto`, `Seriais.Serial`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Grades.Seriais.Serial`, `Lotes.Quantidade`, `Lotes.Validade`, `Lotes.Lote`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`, `Componentes.QuantidadeVendida`, `Componentes.QuantidadeComposicao`, `Componentes.Descricao`, `Abastecimentos.Volume`, `Abastecimentos.Bico`, `Abastecimentos.Bomba`, `Abastecimentos.Tanque`, `GradesGroup.Linha`, `GradesGroup.Coluna`, `GradesGroup.Quantidade`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidos40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidos40Colunas.cs) — SHA-256 `d7b68effe786b38807fafaa0bf384b700e396a387bcbbff6519298fd0380a585`
- [ItensVendidos40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidos40Colunas.Designer.cs) — SHA-256 `323136e75da31df1d3cb45705af5b0b290d177b4d1d792ee6c199b0afed69a17`

## Itens vendidos agrupados por pessoa

Classe: `ItensVendidosAgrupadosPorPessoa`. Identificador: `MovimentacaoSaida.ItensVendidosAgrupadosPorPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo; Mostrar somente vendas para pessoas identificadas; Origem das movimentações; Situação das movimentações; Natureza da operação; Ordenação

**Campos apresentados (DTO):** `DataHora`, `Numero`, `CodigoBarras`, `Quantidade`, `CodigoInterno`, `Descricao`, `NomePessoa`, `MunicipioPessoa`, `UfPessoa`, `CpfCnpjPessoa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `CpfCnpjPessoa`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosComDadosPessoa, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosAgrupadosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosAgrupadosPorPessoa.cs) — SHA-256 `9202c4f7f01b6caf4e90baab0ab9b5c74149632ead05bf5fd2a05ba4b57f4b19`
- [ItensVendidosAgrupadosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosAgrupadosPorPessoa.Designer.cs) — SHA-256 `05141c91f979123a6459ca0d7059ed5ffc320e509ff28b17db5d8280ec26e2f1`

## Itens vendidos agrupados por segmentação

Classe: `ItensVendidosPorSegmentacao`. Identificador: `MovimentacaoSaida.ItensVendidosPorSegmentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo da segmentação; Data inicial; Data final; Tipo; Situação das movimentações; Natureza da operação; Pessoa; Vendedor; Produto; Ordenação; Agrupar por Produto

**Campos apresentados (DTO):** `TipoMovimentacao`, `Total`, `DataHoraSaida`, `NomePessoa`, `DataHora`, `Numero`, `CodigoBarras`, `Quantidade`, `CodigoInterno`, `Descricao`, `Segmentacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `Segmentacao`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosComDadosPessoa, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosPorSegmentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorSegmentacao.cs) — SHA-256 `0773db8c75cbfd90f625f3593d421ebe85a1cdcdf9defeb7b53591a39e166c00`
- [ItensVendidosPorSegmentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorSegmentacao.Designer.cs) — SHA-256 `259bdb80df408145f40c00758690da394b671ef7a4f32c2dcf81fd1ce7d9f9c7`

## Itens vendidos com dados de tributação

Classe: `ItensVendidosComTributacao`. Identificador: `MovimentacaoSaida.ItensVendidosComTributacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo; Ordenação

**Campos apresentados (DTO):** `Cfop`, `CodigoEspecificadorSubstituicaoTributaria`, `CodigoNcmNbs`, `DataHora`, `CodigoBarras`, `CodigoInterno`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `MesEmissao`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`
- `MesEmissao`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosComTributacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosComTributacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosComTributacao.cs) — SHA-256 `0cfe2eb9c3127346bb1505cad07d62ef4dc9c1a566086a82564000b79d430d5b`
- [ItensVendidosComTributacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosComTributacao.Designer.cs) — SHA-256 `8c04ed2d75ba5aba04782991f88c4d9239170c924e927d3be97f5816e5ccde07`

## Itens vendidos com dados de tributação do PIS e COFINS

Classe: `ItensVendidosComTributacaoPisCofins`. Identificador: `MovimentacaoSaida.ItensVendidosComTributacaoPisCofins`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo; Ordenação

**Campos apresentados (DTO):** `CstCsosn`, `CstCofins`, `CstPis`, `Cfop`, `CodigoEspecificadorSubstituicaoTributaria`, `CodigoNcmNbs`, `DataHora`, `CodigoBarras`, `CodigoInterno`, `Total`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosComTributacaoPisCofins`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosComTributacaoPisCofins.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosComTributacaoPisCofins.cs) — SHA-256 `a183806ccc5b6eb8d2266fe3d9b3b3f08ab7e639a19d676caef5a8bfb233e90d`
- [ItensVendidosComTributacaoPisCofins.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosComTributacaoPisCofins.Designer.cs) — SHA-256 `7445a9a52dd88c4d1f205b3dab68c5b1ca39120cd7bc2a974b362ecc3cb52955`

## Itens vendidos com dados do comprador

Classe: `ItensVendidosComDadosPessoa`. Identificador: `MovimentacaoSaida.ItensVendidosComDadosPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Tipo; Origem das movimentações; Situação das movimentações; Natureza da operação; Ordenação

**Campos apresentados (DTO):** `DataHoraSaida`, `DataHora`, `Numero`, `NomePessoa`, `MunicipioPessoa`, `CodigoBarras`, `Quantidade`, `CodigoInterno`, `UfPessoa`, `Descricao`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosComDadosPessoa, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosComDadosPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosComDadosPessoa.cs) — SHA-256 `1eed09f1ea905f2511a56ff8c9d6b64463f3f13e7d688fe60e262e11c9c46ea2`
- [ItensVendidosComDadosPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosComDadosPessoa.Designer.cs) — SHA-256 `fc2074c867e17932cc9907b976f72366ce876ac2798c8baf7eb7152d2b1471b4`

## Itens vendidos por fornecedor

Classe: `ItensVendidosPorFornecedor`. Identificador: `MovimentacaoSaida.ItensVendidosPorFornecedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Fornecedor; Data/Hora inicial; Data/Hora final; Ordenação

**Campos apresentados (DTO):** `CodigoBarras`, `CodigoFornecedor`, `PercentualDesconto`, `OutrasDespesas`, `QuantidadeMovimentacoes`, `CodigoInterno`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`, `Seriais.Serial`, `Grades.Quantidade`, `Grades.Coluna`, `Grades.Linha`, `Grades.Seriais.Serial`, `Lotes.Quantidade`, `Lotes.Validade`, `Lotes.Lote`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`, `GradesGroup.Linha`, `GradesGroup.Coluna`, `GradesGroup.Quantidade`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ProdutosVendidosPorFornecedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosPorFornecedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorFornecedor.cs) — SHA-256 `1e7ed4bb4999c31a28bbc7d1dac588f01db740be5730184922ba9c007998f8f1`
- [ItensVendidosPorFornecedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorFornecedor.Designer.cs) — SHA-256 `d80441d01c737e7167347db6434afbef263199485e747c7550172f65029ea3d1`

## Itens vendidos por localização

Classe: `ItensVendidosPorLocalizacao`. Identificador: `MovimentacaoSaida.ItensVendidosPorLocalizacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Localização

**Campos apresentados (DTO):** `Descricao`, `Quantidade`, `Preco`, `Desconto`, `Total`, `CnpjEmpresa`, `Empresa`, `Seriais.Serial`, `Grades.Linha`, `Grades.Coluna`, `Grades.Quantidade`, `Grades.Seriais.Serial`, `Lotes.Lote`, `Lotes.Validade`, `Lotes.Quantidade`, `Lotes.Seriais.Serial`, `Lotes.Grades.Linha`, `Lotes.Grades.Coluna`, `Lotes.Grades.Quantidade`, `Lotes.Grades.Seriais.Serial`, `PrecoVenda`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosPorLocalizacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosPorLocalizacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorLocalizacao.cs) — SHA-256 `3d92c756217ff50a06ca0d95f2781a056002da9faf0e3dd4721c164210102ee5`
- [ItensVendidosPorLocalizacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorLocalizacao.Designer.cs) — SHA-256 `022d4375292db9780d4ecb3759f6f2377e9b77f301aa16b715b5058716e0e44c`

## Itens vendidos por pessoa

Classe: `ItensVendidosPorPessoa`. Identificador: `MovimentacaoSaida.ItensVendidosPorPessoa`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final; Pessoa; Tipo; Origem; Situação das movimentações; Natureza da operação; Produto ou serviço; Ordenação

**Campos apresentados (DTO):** `OutrasDespesas`, `CodigoInterno`, `Desconto`, `Total`, `Preco`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterItensVendidosPorPessoa, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ItensVendidosPorPessoa.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorPessoa.cs) — SHA-256 `d7cee4fdce3604816505d84aabf558cc0a49476f2c3bfeda31ef518a7505ef21`
- [ItensVendidosPorPessoa.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ItensVendidosPorPessoa.Designer.cs) — SHA-256 `bf8f8ebc6ebbe6a789723a666032d039fa79c243fc86fa2c6bef55f943fc8c2e`

## Lista de movimentações

Classe: `ListaMovimentacoesPorOrigemETipo`. Identificador: `MovimentacaoSaida.ListaMovimentacoesPorOrigemETipo`.

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

## Lista de movimentações - 40 colunas

Classe: `ListaMovimentacoesPorOrigemTipo40Colunas`. Identificador: `MovimentacaoSaida.ListaMovimentacoesPorOrigemTipo40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Origem das movimentações; Situação das movimentações; Natureza da operação; Data/Hora inicial; Data/Hora final; Tipo de data; Pessoa; Vendedor; Mostrar resumo por origem e situação; Ordenação; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro

**Campos apresentados (DTO):** `Total`, `DataHoraEmissao`, `Serie`, `Numero`, `TipoMovimentacao`, `NaturezaOperacao`, `NomePessoa`, `Vendedor`, `CnpjEmpresa`, `Empresa`, `Situacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NomePessoa`
- `Vendedor`
- `Empresa`
- `NaturezaOperacao`
- `Situacao`

**Resumos do layout:**

- `Count`

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

- [ListaMovimentacoesPorOrigemTipo40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemTipo40Colunas.cs) — SHA-256 `073f495736c8b34f185c33b8fc40b55d2c72f6f94c7f4f937703cf6dbd967b05`
- [ListaMovimentacoesPorOrigemTipo40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemTipo40Colunas.Designer.cs) — SHA-256 `757f242f1b24f49a256edb600bdedd4ce0cfb48b9ba68cd6db4f0405465fda81`

## Lista de movimentações com DIFAL

Classe: `ListaMovimentacoesPorOrigemETipoDifal`. Identificador: `MovimentacaoSaida.ListaMovimentacoesPorOrigemETipoDifal`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data/Hora inicial; Data/Hora final; UF destino; Natureza da operação; Tipo de data; Mostrar resumo por origem e situação; Mostrar resumo por dia; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro

**Campos apresentados (DTO):** `TotalFundoCombatePobrezaUfDestinatario`, `TotalIcmsUfDestinatario`, `Situacao`, `Finalidade`, `Total`, `DataHoraEntradaSaidaCF`, `DataHoraEmissao`, `NomePessoa`, `Serie`, `Numero`, `Empresa`, `CnpjEmpresa`, `NaturezaOperacao`, `TipoMovimento`, `TipoMovimentacao`

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

- `INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Only DocumentoFiscalEletronico documents are selected

**Fontes da ficha:**

- [ListaMovimentacoesPorOrigemETipoDifal.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipoDifal.cs) — SHA-256 `fdea3083520c425951934397e5683c8cc7a24c9af63501b750f3cb6aeb3833ef`
- [ListaMovimentacoesPorOrigemETipoDifal.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/ListaMovimentacoesPorOrigemETipoDifal.Designer.cs) — SHA-256 `78506415a3325ab9e35ed0793a26233162b3a5f17ee8f0ced7b5aa51e7c1737c`

## Lista de movimentações com impostos

Classe: `ListaMovimentacoesPorOrigemETipoComImpostos`. Identificador: `MovimentacaoSaida.ListaMovimentacoesPorOrigemETipoComImpostos`.

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

Classe: `ListaMovimentacoesPorInformacaoPesquisa`. Identificador: `MovimentacaoSaida.ListaMovimentacoesPorInformacaoPesquisa`.

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

Classe: `ListaMovimentacoesPorOrigemTipoESegmentacao`. Identificador: `MovimentacaoSaida.ListaMovimentacoesPorOrigemTipoESegmentacao`.

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

Classe: `ListaMovimentacoesPorOrigemETipoSimplificado`. Identificador: `MovimentacaoSaida.ListaMovimentacoesPorOrigemETipoSimplificado`.

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

## Lucro por movimentação

Classe: `MovimentacaoLucro`. Identificador: `MovimentacaoSaida.MovimentacaoLucro`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Pessoa; Vendedor; Origem das movimentações; Situação das movimentações; Natureza da operação; Tipo preço custo; Exibir itens das movimentações; Ordenação

**Campos apresentados (DTO):** `Origem`, `ValorLucro`, `PercentualLucro`, `CustoTotal`, `VendaTotal`, `Data`, `Serie`, `Modelo`, `Pessoa`, `Numero`, `Itens.PercentualLucro`, `Itens.ValorLucro`, `Itens.CustoTotal`, `Itens.VendaTotal`, `Itens.Preco`, `Itens.Quantidade`, `Itens.Descricao`, `Itens.Codigo`, `calculatedField1`

**Expressões calculadas do layout:**

- `([].Sum([VendaTotal] - [CustoTotal]) * 100)  / [].Sum([CustoTotal])`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.LucroMovimentacao, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MovimentacaoLucro.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacaoLucro.cs) — SHA-256 `899a6a22342f4ab71fe80960ed8f8130852f9950c8c93c00928d1be5a4512a10`
- [MovimentacaoLucro.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacaoLucro.Designer.cs) — SHA-256 `72ed105502b01b4d54113a3104d490f247e75179bd265eb8be4c9a6f16994e00`

## Lucro por produto comercializado

Classe: `VendasComLucroProduto`. Identificador: `MovimentacaoSaida.VendasComLucroProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto ou serviço; Data inicial; Data final; Tipo preço custo; Ordenação; Tipo; Marca; SubGrupo

**Campos apresentados (DTO):** `PrecoUnitarioCusto`, `PrecoUnitarioVenda`, `CodigoInterno`, `PercentualLucro`, `ValorLucro`, `CustoTotal`, `VendaTotal`, `Quantidade`, `Descricao`, `calculatedField1`, `PercentualLucroBruto`, `calculatedField2`

**Resumos do layout:**

- `Avg`

**Expressões calculadas do layout:**

- `[].Sum([ValorLucro])/[].Sum([CustoTotal])*100`
- `[].Sum([ValorLucro]) / [].Sum([VendaTotal]) * 100`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.LucroProdutoVendido`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasComLucroProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasComLucroProduto.cs) — SHA-256 `056f4235a958c8b23c0bca7567d38489c46b58db1b38710142c531e230c8df2b`
- [VendasComLucroProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasComLucroProduto.Designer.cs) — SHA-256 `34631db69622c108012abb2d5f9773a72b7c3b19185301ffd4d563187c2b33c9`

## Lucro por venda

Classe: `VendasComLucro`. Identificador: `MovimentacaoSaida.VendasComLucro`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Pessoa; Tipo preço custo; Ordenação

**Campos apresentados (DTO):** `Numero`, `PercentualLucro`, `ValorLucro`, `CustoTotal`, `VendaTotal`, `Data`, `Pessoa`, `Serie`, `Modelo`, `Origem`, `calculatedField1`

**Agrupamentos do layout:**

- `Origem`

**Resumos do layout:**

- `Custom`

**Expressões calculadas do layout:**

- `[].Sum([ValorLucro])/[].Sum([CustoTotal])*100`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.VendasComLucro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasComLucro.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasComLucro.cs) — SHA-256 `638820c569b5f6e3b516ec19e561d92858dbcd0a87f153993a01929d5675347c`
- [VendasComLucro.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasComLucro.Designer.cs) — SHA-256 `b2226deda4ec3ee46c2615c64e82c3a77e5ff9ccea99604bebe240629e908060`

## Medicamentos controlados vendidos

Classe: `MedicamentosControladosVendidos`. Identificador: `MovimentacaoSaida.MedicamentosControladosVendidos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Classe terapêutica; Ordenação

**Campos apresentados (DTO):** `Quantidade`, `Lote`, `Descricao`, `RegistroMinisterioSaude`, `Codigo`, `DataVenda`, `Serie`, `Numero`, `Origem`, `CnpjEmpresa`, `Empresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.MedicamentosControladosVendidos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Therapeutic class is matched against current and historical class in different places

**Fontes da ficha:**

- [MedicamentosControladosVendidos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MedicamentosControladosVendidos.cs) — SHA-256 `b0824e56d96513cc4a802608993e0b9677ffa636ff983f6ed44494f471bcf938`
- [MedicamentosControladosVendidos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MedicamentosControladosVendidos.Designer.cs) — SHA-256 `2160f37fb6ebab3f3cd16b0e15009ff30e27fdada81d745f583d07f9a6e770b8`

## Movimentações com desconto de cashback

Classe: `MovimentacoesComDescontoCashback`. Identificador: `MovimentacaoSaida.MovimentacoesComDescontoCashback`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pessoa; Data inicial; Data final; Vendedor

**Campos apresentados (DTO):** `TotalCashback`, `TipoMovimentacao`, `Vendedor`, `DataHoraEmissao`, `Numero`, `PessoaNome`

**Agrupamentos do layout:**

- `CpfCnpjPessoa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.MovimentacoesComDescontoCashback`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No company predicate in the analysed method
- Period applies to ObjectId generation, not to a business date

**Fontes da ficha:**

- [MovimentacoesComDescontoCashback.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesComDescontoCashback.cs) — SHA-256 `586ef6cf111ebd6302c6ec1d1ea9b2f70304a68e6d23b33dd73841dd7c9c8541`
- [MovimentacoesComDescontoCashback.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesComDescontoCashback.Designer.cs) — SHA-256 `71214a40065a8af2504fa5a37432e623d93e2712d476f39519d03b5ce2ed7543`

## Movimentações de medicamentos controlados sem receita lançada no módulo SNGPC

Classe: `MovimentacoesSemReceita`. Identificador: `MovimentacaoSaida.MovimentacoesSemReceita`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `Total`, `TipoMovimentacao`, `TipoMovimento`, `Empresa`, `CnpjEmpresa`, `Numero`, `NomePessoa`, `DataHoraEmissao`, `DataHoraPrevisaoCF`, `Situacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.MovimentacoesSemReceita`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Receipt complement is computed with Except over company-wide receipts

**Fontes da ficha:**

- [MovimentacoesSemReceita.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesSemReceita.cs) — SHA-256 `6523360b71ca8a8801f74f7c1df8bf74303b78ef788a36b544cd7e2442fdbf96`
- [MovimentacoesSemReceita.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesSemReceita.Designer.cs) — SHA-256 `d322cd2c23d17ee172b32bacf9f8e811309c917a7916f6f9288f67d6b7068766`

## Movimentações por convênio

Classe: `MovimentacoesPorConvenio`. Identificador: `MovimentacaoSaida.MovimentacoesPorConvenio`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Convênio; Data inicial; Data final; Exibir itens

**Campos apresentados (DTO):** `Telefone`, `Cliente`, `Movimentacoes.TipoMovimentacao`, `Movimentacoes.Total`, `Movimentacoes.TotalDesconto`, `Movimentacoes.TotalBruto`, `Movimentacoes.DescontoConvenio`, `Movimentacoes.DataHoraEmissao`, `Movimentacoes.Serie`, `Movimentacoes.Modelo`, `Movimentacoes.Numero`, `Movimentacoes.Itens.TotalLiquido`, `Movimentacoes.Itens.TotalDesconto`, `Movimentacoes.Itens.PrecoUnitario`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Empresa`, `Movimentacoes.CnpjEmpresa`, `ConvenioDocumento`, `Convenio`

**Agrupamentos do layout:**

- `CpfCnpjPessoa`
- `Empresa`
- `ConvenioDocumento`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterMovimentacoesPorConvenio`

**Pesquisa da regra:** Rastreado em profundidade; projeção comum recuperada na DLL `DigisatServer.Application.Core.dll`.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MovimentacoesPorConvenio.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorConvenio.cs) — SHA-256 `dc401c6ad263eabb50102b4cf6c4cfd163bab48547349bd4bd1f8c55bd707c14`
- [MovimentacoesPorConvenio.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorConvenio.Designer.cs) — SHA-256 `3b3dd21db6ce919af591988724900813d7ad12ada01ca6b5930283cddd926da6`

## Movimentações por mês

Classe: `TotalMovimentacoesPorMesPorVendedor`. Identificador: `MovimentacaoSaida.TotalMovimentacoesPorMesPorVendedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Ano; Origem; Situação; Ordenação

**Campos apresentados (DTO):** `acumulado`, `VendedorNome`, `Dezembro`, `Novembro`, `Outubro`, `Setembro`, `Agosto`, `Julho`, `Junho`, `Maio`, `Abril`, `Marco`, `Fevereiro`, `Janeiro`

**Expressões calculadas do layout:**

- `[Janeiro] + [Fevereiro] + [Marco] + [Abril] + [Maio] + [Junho] + [Julho] + [Agosto] + [Setembro] + [Outubro] + [Novembro] + [Dezembro]`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterTotalMovimentacoesPorMesPorVendedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [TotalMovimentacoesPorMesPorVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentacoesPorMesPorVendedor.cs) — SHA-256 `ab9e052604f3ec4574aca8db08febf42df7621ce0536a796f70a0f0d8a874705`
- [TotalMovimentacoesPorMesPorVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentacoesPorMesPorVendedor.Designer.cs) — SHA-256 `f5334eeb1cd11a65041ec1e40e70839b735c95d5e79bc4c489e3e34baa8c0425`

## Movimentações por mês e tipo

Classe: `TotalMovimentacoesPorMesPorTipo`. Identificador: `MovimentacaoSaida.TotalMovimentacoesPorMesPorTipo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ano; Ordenação

**Campos apresentados (DTO):** `acumulado`, `Tipo`, `Dezembro`, `Novembro`, `Outubro`, `Setembro`, `Agosto`, `Julho`, `Junho`, `Maio`, `Abril`, `Marco`, `Fevereiro`, `Janeiro`

**Expressões calculadas do layout:**

- `[Janeiro] + [Fevereiro] + [Marco] + [Abril] + [Maio] + [Junho] + [Julho] + [Agosto] + [Setembro] + [Outubro] + [Novembro] + [Dezembro]`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterTotalMovimentacoesPorMesPorTipo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [TotalMovimentacoesPorMesPorTipo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentacoesPorMesPorTipo.cs) — SHA-256 `49c6484284d8c7db41c46fde000b1d213bf5ee857d6231db018e669aa912a24a`
- [TotalMovimentacoesPorMesPorTipo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentacoesPorMesPorTipo.Designer.cs) — SHA-256 `d8abc78092829a045d819d9b263f2d6360bd56848b4d6ef1367dbd993b28ff2f`

## Movimentações por Município

Classe: `MovimentacoesPorMunicipio`. Identificador: `MovimentacaoSaida.MovimentacoesPorMunicipio`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Data inicial; Data final; Origem; Situação das movimentações

**Campos apresentados (DTO):** `Total`, `DataUltimaMovimentacao`, `Nome`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.MovimentacoesPorMunicipio`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MovimentacoesPorMunicipio.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorMunicipio.cs) — SHA-256 `de1c8e6ce99e74d1c97f7697261cf7a1076f0f6a896f474a344df847d7290185`
- [MovimentacoesPorMunicipio.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorMunicipio.Designer.cs) — SHA-256 `5cd0689857111e543fcbc6223c6ae7cdcc855993d1ec735311cc1209edce3d19`

## Movimentações por operador

Classe: `MovimentacoesPorOperador`. Identificador: `MovimentacaoSaida.MovimentacoesPorOperador`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Operador; Data inicial; Data final; Exibir detalhes dos itens; Situação das movimentações; Origem

**Campos apresentados (DTO):** `Movimentacoes.Situacao`, `Movimentacoes.Total`, `Movimentacoes.TotalDesconto`, `Movimentacoes.TotalProdutos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Desconto`, `Movimentacoes.Itens.Total`, `Movimentacoes.Origem`

**Agrupamentos do layout:**

- `Origem`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.MovimentacoesPorOperador`

**Pesquisa da regra:** Rastreado em profundidade; corpo confirmado na DLL instalada `2.0.26273.6`.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MovimentacoesPorOperador.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorOperador.cs) — SHA-256 `98bc7870fb62ab1dea170690a35700130069a6dd4cf5c36eb55707af0f948d0f`
- [MovimentacoesPorOperador.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorOperador.Designer.cs) — SHA-256 `0587ea1b69c3b517f304368570445ba1427e898a8eedb84aa6426b3d90c1be80`

## Movimentações por vendedor

Classe: `MovimentacoesPorVendedor`. Identificador: `MovimentacaoSaida.MovimentacoesPorVendedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Vendedor; Data/Hora inicial; Data/Hora final; Origem

**Campos apresentados (DTO):** `DataHoraEmissao`, `NomePessoa`, `TipoMovimentacao`, `Serie`, `Modelo`, `Numero`, `Itens.TotalLiquido`, `Itens.TotalDesconto`, `Itens.PrecoUnitario`, `Itens.Quantidade`, `Itens.Descricao`, `Itens.CodigoInterno`, `Total`, `CnpjEmpresa`, `Empresa`, `NaturezaOperacao`, `VendedorToText`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `Finalidade`
- `Vendedor`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterMovimentacoesPorVendedor`

**Pesquisa da regra:** Rastreado em profundidade; projeção comum recuperada na DLL `DigisatServer.Application.Core.dll`.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [MovimentacoesPorVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorVendedor.cs) — SHA-256 `727e809e3838ef7c165600890406461f4d858c3e16029288c9bf6b3d3536e7ad`
- [MovimentacoesPorVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/MovimentacoesPorVendedor.Designer.cs) — SHA-256 `d873e0c14e18a4ece626b648dc76c44f4f86d7fde6f110f3e13ada2f65f3dbcf`

## Movimentações sem XML

Classe: `DocumentosFiscaisEletronicosSemXml`. Identificador: `MovimentacaoSaida.DocumentosFiscaisEletronicosSemXml`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `ValorTotal`, `Serie`, `Pessoa`, `DataEmissao`, `Numero`, `Origem`, `xmlAutorizado`, `xmlCancelado`, `Situacao`

**Agrupamentos do layout:**

- `Origem`

**Expressões calculadas do layout:**

- `Iif([Situacao] == 'Concluído', Iif([PossuiXmlAutorizado] == True, 'Ok', 'Não encontrado'), '-')`
- `Iif([Situacao] == 'Cancelado', Iif([PossuiXmlCancelado] == True, 'Ok', 'Não encontrado'), '-')`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.NotasFiscaisConsumidorEletronicaSemXml`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Only the folder relevant to the document situation is checked

**Fontes da ficha:**

- [DocumentosFiscaisEletronicosSemXml.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/DocumentosFiscaisEletronicosSemXml.cs) — SHA-256 `a64d3d1cdf23013491257cfba1d19f9cdc85c2fef44a3e9c1b12e067474388a9`
- [DocumentosFiscaisEletronicosSemXml.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/DocumentosFiscaisEletronicosSemXml.Designer.cs) — SHA-256 `8a48da0cf6ca52cb9a6042bd0d47151f36d3a196a0c530b3aa03e94572bf883e`

## Objetos por data de garantia

Classe: `ObjetosPorDataGarantia`. Identificador: `MovimentacaoSaida.ObjetosPorDataGarantia`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Objeto.Modelo`, `Objeto.ToTextPlacaSerial`, `Objeto.Marca`, `Objeto.Descricao`, `Objeto.Renavam`, `Objeto.Chassi`, `Objeto.Cor`, `Objeto.AnoFabricacao`, `Objeto.Personalizado.CamposToText`, `Itens.NumeroDavOs`, `Itens.DataGarantia`, `Itens.PessoaNome`, `Itens.Telefones`

**Expressões calculadas do layout:**

- `[Pessoa.Celulares.Numero]`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterObjetosPorDataGarantia`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Cast and person null-safety findings are static reading, not reproduced failures

**Fontes da ficha:**

- [ObjetosPorDataGarantia.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ObjetosPorDataGarantia.cs) — SHA-256 `84bbf5c24077292247de9f1da07ceea13fec20752e45dc45cb6c5394023c0817`
- [ObjetosPorDataGarantia.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ObjetosPorDataGarantia.Designer.cs) — SHA-256 `ab0f0ed2d2ae729176ddc13045561c3994ea1937b9fdebe430d0d35cda9297a0`

## Orçamentos por período

Classe: `OrcamentosPorPeriodo`. Identificador: `MovimentacaoSaida.OrcamentosPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Situação das movimentações; Pessoa; Vendedor; Data inicial; Data final; Ordenação itens agrupados; Exibir detalhes dos itens; Exibir quantidade de registros por situação

**Campos apresentados (DTO):** `Movimentacoes.Situacao`, `Movimentacoes.Total`, `Movimentacoes.TotalProdutos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Total`, `ItensAcumulados.Quantidade`, `ItensAcumulados.Total`, `ItensAcumulados.Descricao`, `ItensAcumulados.CodigoInterno`, `ResumoSituacoes.Situacao`, `ResumoSituacoes.Quantidade`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.DavOuPreVendaOuOrcamentoPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Type 14 and date code 2 are passed as literals

**Fontes da ficha:**

- [OrcamentosPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/OrcamentosPorPeriodo.cs) — SHA-256 `86a26fa9378a8fede0b57608ccc536b0eb79ad6ce0af60e0e245136a75b2edb4`
- [OrcamentosPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/OrcamentosPorPeriodo.Designer.cs) — SHA-256 `e1e0b010318512cc7fc8373bac344d6b642c503df620290d6889370949eaba23`

## Pré-vendas por período

Classe: `PreVendasPorPeriodo`. Identificador: `MovimentacaoSaida.PreVendasPorPeriodo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Situação das movimentações; Pessoa; Vendedor; Data inicial; Data final; Exibir detalhes dos itens; Exibir quantidade de registros por situação

**Campos apresentados (DTO):** `Movimentacoes.Situacao`, `Movimentacoes.Total`, `Movimentacoes.TotalProdutos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Total`, `ItensAcumulados.Quantidade`, `ItensAcumulados.Descricao`, `ItensAcumulados.CodigoInterno`, `ResumoSituacoes.Situacao`, `ResumoSituacoes.Quantidade`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.DavOuPreVendaOuOrcamentoPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Type 7 and date code 2 are passed as literals

**Fontes da ficha:**

- [PreVendasPorPeriodo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/PreVendasPorPeriodo.cs) — SHA-256 `7b1169498700db42f5c583468ae216766fd51284978258274d78275ff4f0d608`
- [PreVendasPorPeriodo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/PreVendasPorPeriodo.Designer.cs) — SHA-256 `39c0ad033a905b1cc02b7bb6912e9ce6322e1048581321127098973264838148`

## Ranking de movimentações por produto

Classe: `RankingMovimentacoesPorProduto`. Identificador: `MovimentacaoSaida.RankingMovimentacoesPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Origem das movimentações; Situação das movimentações; Ordenação

**Campos apresentados (DTO):** `CodigoInterno`, `Descricao`, `Quantidade`, `ValorMovimentado`, `Participacao`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterRankingMovimentacoesPorProdutos`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)
- [Rastreamento: continuacao-movimentacao-entrada.md](../../docs/continuacao-movimentacao-entrada.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Repository sums all items without cancelled or stock/finance checks

**Fontes da ficha:**

- [RankingMovimentacoesPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/RankingMovimentacoesPorProduto.cs) — SHA-256 `683e5452f60b67a1433aec8f5007546ef1a91e6a6a57d637b61995001b5b3850`
- [RankingMovimentacoesPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/RankingMovimentacoesPorProduto.Designer.cs) — SHA-256 `9b1bb85a9b2d0c3273b582c3c6c6fb66bc54995d1bb0c8adaa1ad4abbecec5ab`

## Relação de Notas Fiscais

Classe: `RelacaoDeNotasFiscais`. Identificador: `MovimentacaoSaida.RelacaoDeNotasFiscais`.

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
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)
- [Rastreamento: continuacao-movimentacao-entrada.md](../../docs/continuacao-movimentacao-entrada.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- The same class serves the entry and exit variants (types 5 and 10)
- PorData ignores the previsao branch

**Fontes da ficha:**

- [RelacaoDeNotasFiscais.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/RelacaoDeNotasFiscais.cs) — SHA-256 `c1769d2c0183ae2d43ae5f988e3b15ff9f8f734926ba2c44ee6623e215b290ac`
- [RelacaoDeNotasFiscais.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/RelacaoDeNotasFiscais.Designer.cs) — SHA-256 `7dc637eef389f4de3cb43a8b80e145682f8323464f37b0245e88d2a69727ff70`

## Resumo de vendas - 40 colunas

Classe: `ResumoVendas40Colunas`. Identificador: `MovimentacaoSaida.ResumoVendas40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Origem

**Campos apresentados (DTO):** `Produtos.Total`, `Produtos.AliquotaToText`, `Servicos.Total`, `Servicos.AliquotaToText`, `ResumosVendas.Valor`, `ResumosVendas.Descricao`, `Totais.TotalOutrasDespesas`, `Totais.TotalLiquido`, `Totais.TotalBruto`, `Totais.TotalDesconto`

**Agrupamentos do layout:**

- `AliquotaToText`
- `Descricao`
- `TotalBruto`
- `TotalLiquido`
- `TotalDesconto`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ResumoVendas`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Movements are fetched twice, once for payments and once for items
- Acrescimo and Desconto are not filled in this path

**Fontes da ficha:**

- [ResumoVendas40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/ResumoVendas40Colunas.cs) — SHA-256 `68a49c21f6fecb68077da6c8fdaa2cf1c00ef8d86169a3ae4488703d9693cda4`
- [ResumoVendas40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/ResumoVendas40Colunas.Designer.cs) — SHA-256 `3aa31d316e3876f65a326d4e4eb7f0c5056c49c108dfc03e175897e223ae9f37`

## Resumo diário por CFOP e CST do COFINS

Classe: `ResumoDiarioPorCfopCstCofins`. Identificador: `MovimentacaoSaida.ResumoDiarioPorCfopCstCofins`.

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

Classe: `ResumoDiarioPorCfopCstPis`. Identificador: `MovimentacaoSaida.ResumoDiarioPorCfopCstPis`.

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

Classe: `ResumoDiarioPorCfopCst`. Identificador: `MovimentacaoSaida.ResumoDiarioPorCfopCst`.

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

Classe: `ResumoPisCofins`. Identificador: `MovimentacaoSaida.ResumoPisCofins`.

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

Classe: `ResumoPisCofinsDiario`. Identificador: `MovimentacaoSaida.ResumoPisCofinsDiario`.

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

Classe: `ResumoPisCofinsMovimentacao`. Identificador: `MovimentacaoSaida.ResumoPisCofinsMovimentacao`.

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

Classe: `ResumoPorCfop`. Identificador: `MovimentacaoSaida.ResumoPorCfop`.

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

Classe: `ResumoPorMovimentacao`. Identificador: `MovimentacaoSaida.ResumoPorMovimentacao`.

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

## Saídas com substituição tributária por produto

Classe: `SaidasSubstituicaoTributariaPorProduto`. Identificador: `MovimentacaoSaida.SaidasSubstituicaoTributariaPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Origem; Mostrar detalhes; Ordenação

**Campos apresentados (DTO):** `ValorIcmsEfetivo`, `BaseCalculoIcmsEfetivo`, `CodigoInterno`, `Descricao`, `Ncm`, `Quantidade`, `TotalLiquido`, `ValorBaseCalculoSt`, `ValorSt`, `BaseCalculoStRetido`, `ValorStRetido`, `ItensMovimentacoes.PercentualIcmsEfetivo`, `ItensMovimentacoes.PercentualBaseCalculoIcmsEfetivo`, `ItensMovimentacoes.ValorIcmsEfetivo`, `ItensMovimentacoes.BaseCalculoIcmsEfetivo`, `ItensMovimentacoes.NumeroMovimentacao`, `ItensMovimentacoes.Pessoa`, `ItensMovimentacoes.Quantidade`, `ItensMovimentacoes.TotalLiquido`, `ItensMovimentacoes.ValorBaseCalculoSt`, `ItensMovimentacoes.ValorSt`, `ItensMovimentacoes.BaseCalculoStRetido`, `ItensMovimentacoes.ValorStRetido`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterSaidaSubstituicaoTributariaPorProduto`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- NCM is read without a null guard

**Fontes da ficha:**

- [SaidasSubstituicaoTributariaPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/SaidasSubstituicaoTributariaPorProduto.cs) — SHA-256 `5eb7f6c7a522a7c1dead2a5d4b2eb3419d21c31603e1e4eb8600eca7c8e1b8cd`
- [SaidasSubstituicaoTributariaPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/SaidasSubstituicaoTributariaPorProduto.Designer.cs) — SHA-256 `6a8e39f365fb8c9acff52682ce5b22bfe60c0ee2e48f15f88775d6acd74cf739`

## Saldo de estoque e entregas futuras

Classe: `SaldoEstoqueEntregasFuturas`. Identificador: `MovimentacaoSaida.SaldoEstoqueEntregasFuturas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** CFOPs de venda para entrega futura; Natureza da operação; Filtrar por data de emissão da venda; Data de emissão inicial; Data de emissão final; Produto; Grupo; Situação da cobertura; Exibir documentos e separar por clientes

**Campos apresentados (DTO):** `ClienteTexto`, `CodigoInterno`, `DescricaoProduto`, `Unidade`, `QuantidadeVendida`, `QuantidadeEntregue`, `SaldoLinha`, `EstoqueDisponivel`, `SaldoCobertura`, `DataEmissao`, `Documento`, `TipoMovimento`, `Cfop`, `Quantidade`, `SaldoCorrente`

**Agrupamentos do layout:**

- `NomePessoa`
- `PessoaReferencia`
- `CodigoInternoNumerico`
- `ProdutoServicoEmpresaReferencia`

**Resumos do layout:**

- `Max`

**Contratos de serviço:**

- `INaturezaOperacaoModuleService`
- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterSaldoEstoqueEntregasFuturas, INaturezaOperacaoModuleService.Obter`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Estoque.QuantidadeSaldo semantics not confirmed

**Fontes da ficha:**

- [SaldoEstoqueEntregasFuturas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/SaldoEstoqueEntregasFuturas.cs) — SHA-256 `74b7c29496d6af42aca816790808ba5d571608c287397d45c3cd18df1d5f66b0`
- [SaldoEstoqueEntregasFuturas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/SaldoEstoqueEntregasFuturas.Designer.cs) — SHA-256 `6ebbc519da080d9027005b3f32f377d603bfebd7949a3706b7049fa888eec466`

## Ticket médio

Classe: `TicketMedioPorOrigemETipo`. Identificador: `MovimentacaoSaida.TicketMedioPorOrigemETipo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Origem das movimentações; Situação das movimentações; Data/Hora inicial; Data/Hora final; Pessoa; Vendedor; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro

**Campos apresentados (DTO):** `QuantidadeMovimentacoes`, `Total`, `TicketMedio`, `DataEmissao`, `Empresa`, `CnpjEmpresa`, `TipoMovimentacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `Não identificada na classe de entrada`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)
- [Rastreamento: continuacao-ticket-medio.md](../../docs/continuacao-ticket-medio.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Date type, natureza and segmentacao are hardcoded in the call

**Fontes da ficha:**

- [TicketMedioPorOrigemETipo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TicketMedioPorOrigemETipo.cs) — SHA-256 `cfdb383899b6dfdb13be797fa1ef8acd1635e6aafafb43f2f536ccaffefacdd5`
- [TicketMedioPorOrigemETipo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TicketMedioPorOrigemETipo.Designer.cs) — SHA-256 `c9b974e9d8e68868f6d34a17dd2f76bde1b2f72c25c7a646488e73e28a7ce7f0`

## Ticket médio dos produtos

Classe: `TicketMedioPorOrigemETipoItens`. Identificador: `MovimentacaoSaida.TicketMedioPorOrigemETipoItens`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Origem das movimentações; Situação das movimentações; Data/Hora inicial; Data/Hora final; Pessoa; Vendedor; Mostrar somente com movimentação física de estoque e financeiro; Não mostrar NFC-e de item financeiro; Ordenação

**Campos apresentados (DTO):** `Unidade`, `Descricao`, `Quantidade`, `Total`, `TicketMedio`, `CodigoInterno`, `Empresa`, `CnpjEmpresa`, `TipoMovimentacao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `Não identificada na classe de entrada`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)
- [Rastreamento: continuacao-ticket-medio.md](../../docs/continuacao-ticket-medio.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Date type, natureza and segmentacao are hardcoded in the call

**Fontes da ficha:**

- [TicketMedioPorOrigemETipoItens.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TicketMedioPorOrigemETipoItens.cs) — SHA-256 `0f47bf45107f1f2105862ab1bf3b1cdbda9e9aecabd94d50061e4e3272a28de7`
- [TicketMedioPorOrigemETipoItens.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TicketMedioPorOrigemETipoItens.Designer.cs) — SHA-256 `3c9ceba2641a73f6265ce5c00c87d99c5a1a6aec2f4e5bf3dc0e6150ecbbb93f`

## Total movimentado por CFOP

Classe: `TotalMovimentadoPorCfop`. Identificador: `MovimentacaoSaida.TotalMovimentadoPorCfop`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Origem; Situação

**Campos apresentados (DTO):** `DetalhesMovimentacao.Data`, `DetalhesMovimentacao.Numero`, `DetalhesMovimentacao.Origem`, `DetalhesMovimentacao.Serie`, `DetalhesMovimentacao.ValorTotal`, `DetalhesMovimentacao.DetalhesCfop.Cfop`, `DetalhesMovimentacao.DetalhesCfop.Valor`, `DetalhesCfop.Cfop`, `DetalhesCfop.Valor`, `DetalhesCfop.Descricao`

**Agrupamentos do layout:**

- `Cfop`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterDadosRelatorioTotalMovimentadoPorCfop`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [TotalMovimentadoPorCfop.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/TotalMovimentadoPorCfop.cs) — SHA-256 `d88ebacd09cc2b99e105c11e450fadd722f0b793d5b2f33c5a0d6a9a3e1e9346`
- [TotalMovimentadoPorCfop.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/TotalMovimentadoPorCfop.Designer.cs) — SHA-256 `167781ad90007a2ca29841d82ac9281d0bf04056b0656ebecf38d0d298a844a9`

## Total movimentado por tributação

Classe: `TotalMovimentadoPorTributacao`. Identificador: `MovimentacaoSaida.TotalMovimentadoPorTributacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Operador; Data inicial; Data final; Origem; Situação

**Campos apresentados (DTO):** `Produtos.Acrescimo`, `Produtos.Desconto`, `Produtos.CST`, `Produtos.Tributacao`, `Produtos.Aliquota`, `Produtos.Total`, `Servicos.Desconto`, `Servicos.Acrescimo`, `Servicos.CST`, `Servicos.Tributacao`, `Servicos.Aliquota`, `Servicos.Total`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.TotalVendidoPorTributacao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Operator filter uses movement history user names, not the item seller

**Fontes da ficha:**

- [TotalMovimentadoPorTributacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentadoPorTributacao.cs) — SHA-256 `3a4be82b7b2ce7385d17861bd0d3e76e2725825b46efe948cb66246566ea4570`
- [TotalMovimentadoPorTributacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentadoPorTributacao.Designer.cs) — SHA-256 `b135025538dc9e6901a8cdebe9672fb65e546b45898191146d9729ccae0c2a6a`

## Total movimentado por vendedor

Classe: `TotalMovimentadoPorVendedor`. Identificador: `MovimentacaoSaida.TotalMovimentadoPorVendedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Data inicial; Data final; Origem; Situação

**Campos apresentados (DTO):** `calculatedField1`, `Total`, `VendedorNome`

**Expressões calculadas do layout:**

- `([Total]/[].Sum([Total]))*100`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.TotalMovimentadoPorVendedor`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Monthly buckets are computed and then discarded by the DTO

**Fontes da ficha:**

- [TotalMovimentadoPorVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentadoPorVendedor.cs) — SHA-256 `25e3799db86f019bf2292acef0d9b2d08b9cb370e4c7e6c46dd73c00e150eeab`
- [TotalMovimentadoPorVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalMovimentadoPorVendedor.Designer.cs) — SHA-256 `c0603a4d0eba61bd46ceb244032875d3893d959c840d302776857bf2d4d6bb71`

## Totalização de vendas

Classe: `TotalizacaoVendas`. Identificador: `MovimentacaoSaida.TotalizacaoVendas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Operador; Pessoa; Vendedor; Data inicial; Data final; Origem; Situação

**Campos apresentados (DTO):** `DadosRelatorioResumoVendasSinteticoTotalDto.TotalLiquido`, `DadosRelatorioResumoVendasSinteticoTotalDto.TotalServicos`, `DadosRelatorioResumoVendasSinteticoTotalDto.Total`, `DadosRelatorioResumoVendasSinteticoTotalDto.TotalDesconto`, `DadosRelatorioResumoVendasSinteticoTotalDto.TotalProdutos`, `ResumoEspecies.Especie`, `ResumoEspecies.Valor`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ResumoVendasSintetico`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [TotalizacaoVendas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoVendas.cs) — SHA-256 `97be2515358920fec0e867f4ff0f18e489595360276bb44e9d1e13efc31ff1a0`
- [TotalizacaoVendas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoVendas.Designer.cs) — SHA-256 `1d703306fb823d5dcb95bc8d8458a3f4407c72bb3778271b3e076559c7adf263`

## Totalização por ECF

Classe: `TotalizacaoPorEcf`. Identificador: `MovimentacaoSaida.TotalizacaoPorEcf`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Mostrar total por data

**Campos apresentados (DTO):** `NumeroFabricacao`, `DataMovimento`, `Valor`

**Agrupamentos do layout:**

- `NumeroFabricacao`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterTotalizacaoPorEcf`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [TotalizacaoPorEcf.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEcf.cs) — SHA-256 `3de53008e8445e2000a9027879264d47f19221896926bf9e16e44a481124ae25`
- [TotalizacaoPorEcf.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEcf.Designer.cs) — SHA-256 `55fe5e58f3125ecf334a3d553055d2cfde4c0d96f8f161c81ff434a75c9a3600`

## Totalização por espécie

Classe: `TotalizacaoPorEspeciePdv`. Identificador: `MovimentacaoSaida.TotalizacaoPorEspeciePdv`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Operador; Data/Hora inicial; Data/Hora final; Origem; Situação; Agrupar por operador; Agrupar por dia; Centro de custo; Omitir espécie 'Crédito loja'

**Campos apresentados (DTO):** `DescricaoBandeira`, `Valor`, `Quantidade`, `Participacao`, `Descricao`, `Usuario`, `Data`

**Agrupamentos do layout:**

- `Usuario`
- `Data`
- `Descricao`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.TotalizacaoPorEspecie`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Situation 99 unions contingencia (14) and concluido (2)
- Uses raw datetime emission boundaries

**Fontes da ficha:**

- [TotalizacaoPorEspeciePdv.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdv.cs) — SHA-256 `82063c3d172d3975b3158a1c1ba3350a1e855653f8c2094e2a6a1f6a44480da8`
- [TotalizacaoPorEspeciePdv.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdv.Designer.cs) — SHA-256 `4f4570793b091dbfa269520cf9a28b6b74f7c1da46578886594aa9e27e547ff0`

## Totalização por espécie - 40 colunas

Classe: `TotalizacaoPorEspeciePdv40Colunas`. Identificador: `MovimentacaoSaida.TotalizacaoPorEspeciePdv40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Operador; Data/Hora inicial; Data/Hora final; Origem; Situação; Agrupar por operador; Agrupar por dia; Centro de custo; Omitir espécie 'Crédito loja'

**Campos apresentados (DTO):** `DescricaoBandeira`, `Valor`, `Quantidade`, `Participacao`, `Descricao`, `Usuario`, `Data`

**Agrupamentos do layout:**

- `Usuario`
- `Data`
- `Descricao`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.TotalizacaoPorEspecie`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Situation 99 unions contingencia (14) and concluido (2)
- Uses raw datetime emission boundaries

**Fontes da ficha:**

- [TotalizacaoPorEspeciePdv40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdv40Colunas.cs) — SHA-256 `51d9765ad5968c4408adb332748e585f104adf4b7791e65ac425a55f2843d27b`
- [TotalizacaoPorEspeciePdv40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdv40Colunas.Designer.cs) — SHA-256 `e84ac81fe976995b4c08ee58ef0036f12bb8a6ac1f8038f35af48309ad6ec47f`

## Totalização por espécie por vendedor

Classe: `TotalizacaoPorEspeciePdvVendedor`. Identificador: `MovimentacaoSaida.TotalizacaoPorEspeciePdvVendedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Data/Hora inicial; Data/Hora final; Origem; Situação; Agrupar por vendedor; Agrupar por dia; Centro de custo; Omitir espécie 'Crédito loja'

**Campos apresentados (DTO):** `DescricaoBandeira`, `Valor`, `Quantidade`, `Participacao`, `Descricao`, `Vendedor`, `Data`

**Agrupamentos do layout:**

- `Vendedor`
- `Data`
- `Descricao`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.TotalizacaoPorEspeciePorVendedor`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Seller apportioning rule not extracted
- Uses raw datetime emission boundaries

**Fontes da ficha:**

- [TotalizacaoPorEspeciePdvVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdvVendedor.cs) — SHA-256 `e3411ed229658e04a5684fd64e76efe220a84e3c1d4a4b7e1ce0298f21373eed`
- [TotalizacaoPorEspeciePdvVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdvVendedor.Designer.cs) — SHA-256 `452b1b7b5af40da1734f2ab30a222e47352f4ffb92fab24d0caa6d41dfc722a4`

## Totalização por espécie por vendedor - 40 colunas

Classe: `TotalizacaoPorEspeciePdvVendedor40Colunas`. Identificador: `MovimentacaoSaida.TotalizacaoPorEspeciePdvVendedor40Colunas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Data/Hora inicial; Data/Hora final; Origem; Situação; Agrupar por vendedor; Agrupar por dia; Centro de custo; Omitir espécie 'Crédito loja'

**Campos apresentados (DTO):** `DescricaoBandeira`, `Valor`, `Quantidade`, `Participacao`, `Descricao`, `Vendedor`, `Data`

**Agrupamentos do layout:**

- `Vendedor`
- `Data`
- `Descricao`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.TotalizacaoPorEspeciePorVendedor`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Seller apportioning rule not extracted
- Uses raw datetime emission boundaries

**Fontes da ficha:**

- [TotalizacaoPorEspeciePdvVendedor40Colunas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdvVendedor40Colunas.cs) — SHA-256 `5d31265a6d80dc9327f04f9e8edf865ebf7c14285cb540ff8612d44855c63518`
- [TotalizacaoPorEspeciePdvVendedor40Colunas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/TotalizacaoPorEspeciePdvVendedor40Colunas.Designer.cs) — SHA-256 `d9bfbed8a5371a994cb0924c52dfc9b805b1c7e3f7cd85573847d63794146f56`

## Totalização por NCM

Classe: `TotalizacaoPorNcm`. Identificador: `MovimentacaoSaida.TotalizacaoPorNcm`.

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

## Vendas em cartão

Classe: `VendasCartao`. Identificador: `MovimentacaoSaida.VendasCartao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data/Hora inicial; Data/Hora final; Exibir código de autorização

**Campos apresentados (DTO):** `Autorizacao`, `Rede`, `Valor`, `Tipo`

**Agrupamentos do layout:**

- `Tipo`
- `Rede`

**Expressões calculadas do layout:**

- `[VolumeRegistrado] - [VolumeFaturado]`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterRelatorioVendasCartao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasCartao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasCartao.cs) — SHA-256 `261609c8a1e101b6ba87a8f7b49cd0cee475c03fd26b3dadec9fcfe07043b4e2`
- [VendasCartao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasCartao.Designer.cs) — SHA-256 `792848a155c234d621d608c15fd2fb817509876b65836d46e4f8ff5c54f7666b`

## Vendas por administradora de cartões

Classe: `VendasPorAdministradoraCartao`. Identificador: `MovimentacaoSaida.VendasPorAdministradoraCartao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Administradora de cartão; Data inicial; Data final; Origem

**Campos apresentados (DTO):** `TotalCartao`, `TotalMovimentacao`, `ChaveAcesso`, `DataEmissao`, `Serie`, `Numero`, `Origem`

**Agrupamentos do layout:**

- `Origem`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterVendasPorAdministradoraCartao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasPorAdministradoraCartao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorAdministradoraCartao.cs) — SHA-256 `40cede3051a7f36e4c3905a5c80f7d339be2c982a2d702e9ceeb75a61764dfe2`
- [VendasPorAdministradoraCartao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorAdministradoraCartao.Designer.cs) — SHA-256 `f6468951bfd000c30782dd67736a4b5d891b682e6b70ba002c86c7d83ef7cd2a`

## Vendas por cliente

Classe: `VendasPorCliente`. Identificador: `MovimentacaoSaida.VendasPorCliente`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Origem; Ordenação

**Campos apresentados (DTO):** `Participacao`, `Total`, `DataUltimaMovimentacao`, `Nome`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.VendasPorCliente`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [VendasPorCliente.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorCliente.cs) — SHA-256 `edb89d3ab13c5d89fbd1da39169b3b4bf3302ea24cdc2fef81f1313624e50723`
- [VendasPorCliente.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorCliente.Designer.cs) — SHA-256 `0ac5c1e5cc459cb68f60fdee631aba1e04be5d1aaac1f93a6c6477123cfc712d`

## Vendas por ECF

Classe: `VendasPorEcf`. Identificador: `MovimentacaoSaida.VendasPorEcf`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** ECF; Situação das movimentações; Data/Hora inicial; Data/Hora final; Vendedor; Mostrar itens

**Campos apresentados (DTO):** `FormaPagamento`, `Situacao`, `Total`, `DataHoraEmissao`, `NomePessoa`, `Numero`, `Itens.TotalLiquido`, `Itens.TotalBruto`, `Itens.TotalDesconto`, `Itens.PrecoUnitario`, `Itens.Quantidade`, `Itens.UnidadeMedida`, `Itens.Descricao`, `Itens.CodigoInterno`, `NumeroFabricacao`

**Agrupamentos do layout:**

- `NumeroFabricacao`

**Expressões calculadas do layout:**

- `Iif([DataHoraEntradaSaida]=='01/01/0001 00:00:00', '' , [DataHoraEntradaSaida])`

**Contratos de serviço:**

- `IMovimentacaoRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoRelatorioModuleService.ObterMovimentacoesPorTipoMovimentacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasPorEcf.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorEcf.cs) — SHA-256 `fe18a55bdc6ace50697264b1977ed6338d9fbe34e358e509417d3576009368ab`
- [VendasPorEcf.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorEcf.Designer.cs) — SHA-256 `62fb8a73ebfcdd9aa865970be23995ac9eb5af35e0f5d630bb36594f96cff528`

## Vendas por operador

Classe: `VendasPorOperador`. Identificador: `MovimentacaoSaida.VendasPorOperador`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Operador; Data inicial; Data final; Exibir formas de pagamento; Exibir detalhes dos itens; Origem; Situação

**Campos apresentados (DTO):** `Movimentacoes.Total`, `Movimentacoes.TotalDesconto`, `Movimentacoes.TotalProdutos`, `Movimentacoes.Cliente`, `Movimentacoes.Data`, `Movimentacoes.Numero`, `Movimentacoes.Itens.CodigoInterno`, `Movimentacoes.Itens.Descricao`, `Movimentacoes.Itens.Quantidade`, `Movimentacoes.Itens.Preco`, `Movimentacoes.Itens.Desconto`, `Movimentacoes.Itens.Total`, `Movimentacoes.EspeciesRecebimento.Especie`, `Movimentacoes.EspeciesRecebimento.Valor`, `Movimentacoes.Origem`, `ResumoEspecies.Especie`, `ResumoEspecies.Valor`

**Agrupamentos do layout:**

- `Origem`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.VendasPorOperador`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [VendasPorOperador.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorOperador.cs) — SHA-256 `56a039afb3416084f2aa73693561ffc5741dd7f73055c605259654f59e334e60`
- [VendasPorOperador.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorOperador.Designer.cs) — SHA-256 `fb7022ccfbbd66ac455a52e18dfacee517defbd22c88ff4d89f5e3fff312a853`

## Vendas por produto

Classe: `VendasPorProduto`. Identificador: `MovimentacaoSaida.VendasPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Operador; Preço; Ordenação

**Campos apresentados (DTO):** `CodigoBarras`, `PercentualLucro`, `Lucro`, `TotalVenda`, `Preco`, `Quantidade`, `Descricao`, `CodigoInterno`, `CnpjEmpresa`, `NomeEmpresa`, `calculatedField1`

**Expressões calculadas do layout:**

- `[Lucro] * 100 / [TotalVenda]`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.VendasPorProduto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [VendasPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorProduto.cs) — SHA-256 `63616fc3d70ffc745bec8665618198f85067a62adae4ddc5ffe8ab67dba2d520`
- [VendasPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorProduto.Designer.cs) — SHA-256 `c989eb2569ac9727e039a389dd10d8020f22c945ca756cc123b18fce874668d9`

## Vendas por produto e operador

Classe: `VendasPorProdutoPorOperador`. Identificador: `MovimentacaoSaida.VendasPorProdutoPorOperador`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Operador; Ordenação

**Campos apresentados (DTO):** `TotalVenda`, `Quantidade`, `Descricao`, `CodigoInterno`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterVendasPorProdutoEOperador`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Operator link uses movement history names

**Fontes da ficha:**

- [VendasPorProdutoPorOperador.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorProdutoPorOperador.cs) — SHA-256 `4e7f5c184f94055f4ff5d311ffd7c94bb551df3a67343cc5ab4bf63f2d91e4ed`
- [VendasPorProdutoPorOperador.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorProdutoPorOperador.Designer.cs) — SHA-256 `9c8c1e4f5a1e9eb33aff08d220f6a8ad90262025a0e369678ed8101c119d76b4`

## Vendas por produto mensal

Classe: `VendasPorProdutoMensal`. Identificador: `MovimentacaoSaida.VendasPorProdutoMensal`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Produto; Ordenação; Exibir apenas itens com estoque atual abaixo da média de vendas

**Campos apresentados (DTO):** Não recuperados por extração literal.

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterVendasPorProdutoMensal`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Current stock is used, not the month-end stock

**Fontes da ficha:**

- [VendasPorProdutoMensal.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorProdutoMensal.cs) — SHA-256 `2dd17e1a890a0597b6c03e968cbaf0236684adcb7b855fa7f65610aa47da9863`
- [VendasPorProdutoMensal.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorProdutoMensal.Designer.cs) — SHA-256 `72d26e2a4128910f23716cf5a8db292de6dd887df6664b5d5d1c6ea0fe7f2994`

## Vendas por tipo de segmentação

Classe: `VendasPorTipoSegmentacao`. Identificador: `MovimentacaoSaida.VendasPorTipoSegmentacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo de segmentação; Data inicial; Data final; Origem

**Campos apresentados (DTO):** `TotalMovimentacao`, `ChaveAcesso`, `DataEmissao`, `Serie`, `Numero`, `PessoaNome`, `Origem`

**Agrupamentos do layout:**

- `PessoaDocumento`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.ObterVendasPorTipoSegmentacao`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-movimentacao-saida.md](../../docs/continuacao-movimentacao-saida.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- People are filtered by current segmentation

**Fontes da ficha:**

- [VendasPorTipoSegmentacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorTipoSegmentacao.cs) — SHA-256 `5650459e6950654be0b25ef84f6a3d418d5a6addeaa32247e10d2cbe03155cfd`
- [VendasPorTipoSegmentacao.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorTipoSegmentacao.Designer.cs) — SHA-256 `c2ba620d45cfe2b27dc7bb8e556bfcf445e3056957f27222ae41e9ab8b60489d`

## Vendas por UF

Classe: `VendasPorUf`. Identificador: `MovimentacaoSaida.VendasPorUf`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Sigla`, `Total`, `DataUltimaMovimentacao`, `Nome`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.VendasPorUf`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [VendasPorUf.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorUf.cs) — SHA-256 `66089be619378a642f46ae9898316e6bb70f5fe39914262451dd642e2a4e6173`
- [VendasPorUf.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorUf.Designer.cs) — SHA-256 `f4dedce37cacc844086b3f7ee33c6bab5af14760cf74a9db3c08538684a0c7e3`

## Vendas por UF detalhado

Classe: `VendasPorUfDetalhado`. Identificador: `MovimentacaoSaida.VendasPorUfDetalhado`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Pessoa; UF destino; Agrupar itens. (O preço de venda será a média dos preços de venda dos itens agrupados.)

**Campos apresentados (DTO):** `Codigo`, `Descricao`, `Quantidade`, `Preco`, `VendaTotal`, `CustoTotal`, `ValorLucro`, `PercentualLucro`, `DocumentoPessoa`, `NomePessoa`, `calculatedField3`, `calculatedField1`, `calculatedField2`, `calculatedField4`

**Agrupamentos do layout:**

- `DocumentoPessoa`

**Resumos do layout:**

- `Avg`

**Expressões calculadas do layout:**

- `[].Sum([VendaTotal])`
- `[].Sum([CustoTotal])`
- `[].Sum([ValorLucro])`
- `(([].Sum([VendaTotal])-[].Sum([CustoTotal]))/[].Sum([CustoTotal]))*100`
- `([CustoTotal]/[ValorLucro])`

**Contratos de serviço:**

- `IMovimentacaoSaidaRelatorioModuleService`

**Chamadas catalogadas:**

- `IMovimentacaoSaidaRelatorioModuleService.VendasPorUfDetalhado`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [VendasPorUfDetalhado.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorUfDetalhado.cs) — SHA-256 `fb7887ce0e3408dd68df05e10a4f805ef45e31531beb3ffb9330e5f173724129`
- [VendasPorUfDetalhado.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Movimentacao/Saida/VendasPorUfDetalhado.Designer.cs) — SHA-256 `275c2f2b717fe5d5e59f50faf85486fa9ed5dd105c760f4e938191e665b1f6d0`

