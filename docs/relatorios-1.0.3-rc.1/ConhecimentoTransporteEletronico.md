# CT-e

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Apuração de ICMS

Classe: `ApuracaoIcms`. Identificador: `ConhecimentoTransporteEletronico.ApuracaoIcms`.

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

Classe: `ApuracaoIcmsPorMovimentacao`. Identificador: `ConhecimentoTransporteEletronico.ApuracaoIcmsPorMovimentacao`.

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

## Lista de conhecimentos de transporte eletrônico

Classe: `ListaConhecimentoTransporteEletronico`. Identificador: `ConhecimentoTransporteEletronico.ListaConhecimentoTransporteEletronico`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Filtrar pessoa; Pessoa; Mostrar notas fiscais eletrônicas; Situação; Município origem; UF origem; Município destino; UF destino

**Campos apresentados (DTO):** `MunicipioDestino`, `Situacao`, `ValorPrestacaoServico`, `DataEntregaFinal`, `CodigoOperacaoFiscal`, `ChaveAcesso`, `NomeDestinatario`, `ValorReceber`, `DataHoraEmissao`, `NomeRemetente`, `Serie`, `Numero`, `Empresa`, `CnpjEmpresa`, `Nfes.ChaveAcesso`, `Nfes.ValorTotalNota`, `Nfes.ValorTotalProdutos`, `Nfes.PesoTotal`, `Nfes.DataEmissao`, `Nfes.Numero`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Contratos de serviço:**

- `IConhecimentoTransporteEletronicoRelatorioModuleService`

**Chamadas catalogadas:**

- `IConhecimentoTransporteEletronicoRelatorioModuleService.ObterConhecimentosTransporteEletronicoPorPeriodo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaConhecimentoTransporteEletronico.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ConhecimentoTransporteEletronico/ListaConhecimentoTransporteEletronico.cs) — SHA-256 `526ad90d51594a3398864a4d253848efbabbc080ae8b34435d5fc7d302a9822f`
- [ListaConhecimentoTransporteEletronico.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ConhecimentoTransporteEletronico/ListaConhecimentoTransporteEletronico.Designer.cs) — SHA-256 `c65fcd1f0d75fa41f93b9448fefcfcb0b6297b2af4859fd73a699a0a85b32f36`

