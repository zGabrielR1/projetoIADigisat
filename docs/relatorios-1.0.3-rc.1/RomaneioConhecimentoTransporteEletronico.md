# Romaneios de CT-e

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## RomaneiosConhecimentoTransporteEletronicoPorCidadeBairro

Classe: `RomaneiosConhecimentoTransporteEletronicoPorCidadeBairro`. Identificador: `RomaneioConhecimentoTransporteEletronico.RomaneiosConhecimentoTransporteEletronicoPorCidadeBairro`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Situação; Ordenação; Mostrar resumo por bairro

**Campos apresentados (DTO):** `NumeroRomaneio`, `DataSaida`, `NomeTomador`, `BairroTomador`, `NumeroDocumento`, `Valor`, `MunicipioTomador`

**Agrupamentos do layout:**

- `MunicipioTomador`

**Contratos de serviço:**

- `IRomaneioConhecimentoTransporteEletronicoModuleService`

**Chamadas catalogadas:**

- `IRomaneioConhecimentoTransporteEletronicoModuleService.ObterRomaneiosConhecimentoTransporteEletronicoPorCidadeEBairro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [RomaneiosConhecimentoTransporteEletronicoPorCidadeBairro.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/RomaneioConhecimentoTransporteEletronico/RomaneiosConhecimentoTransporteEletronicoPorCidadeBairro.cs) — SHA-256 `aadbf259d21ce626978b65284fe12419780a1ff35ca13233b03beb81cbdb97c0`

## RomaneiosConhecimentoTransporteEletronicoPorPeriodoSituacao

Classe: `RomaneiosConhecimentoTransporteEletronicoPorPeriodoSituacao`. Identificador: `RomaneioConhecimentoTransporteEletronico.RomaneiosConhecimentoTransporteEletronicoPorPeriodoSituacao`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Situação

**Campos apresentados (DTO):** `ConhecimentoTransporteEletronicoReferenciados.Numero`, `ConhecimentoTransporteEletronicoReferenciados.DataHoraEmissao`, `ConhecimentoTransporteEletronicoReferenciados.NomeTomador`, `ConhecimentoTransporteEletronicoReferenciados.Total`, `RomaneiosConhecimentoTransporteEletronico.Numero`, `RomaneiosConhecimentoTransporteEletronico.Data`, `RomaneiosConhecimentoTransporteEletronico.Placa`, `RomaneiosConhecimentoTransporteEletronico.Motorista`, `RomaneiosConhecimentoTransporteEletronico.Observacao`

**Contratos de serviço:**

- `IRomaneioConhecimentoTransporteEletronicoModuleService`

**Chamadas catalogadas:**

- `IRomaneioConhecimentoTransporteEletronicoModuleService.ObterRomaneiosConhecimentoTransporteEletronicoPorPeriodoSituacao`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [RomaneiosConhecimentoTransporteEletronicoPorPeriodoSituacao.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/RomaneioConhecimentoTransporteEletronico/RomaneiosConhecimentoTransporteEletronicoPorPeriodoSituacao.cs) — SHA-256 `ed7b4020b7e4d78931847d6acc79f090d7c14bec554b717400c3b5d365155127`

