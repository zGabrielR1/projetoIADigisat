# Romaneios de carga

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Romaneios de carga

Classe: `RomaneiosCargaPorPeriodoSituacaoEVendedor`. Identificador: `RomaneioCarga.RomaneiosCargaPorPeriodoSituacaoEVendedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Data inicial; Data final; Situação

**Campos apresentados (DTO):** `Itens.TotalPesoBruto`, `Itens.UnidadeMedida`, `Itens.Quantidade`, `Itens.Localizacao`, `Itens.ProdutoDescricao`, `Itens.CodigoInterno`, `Volumes.PesoLiquido`, `Volumes.PesoBruto`, `Volumes.Quantidade`, `Volumes.Marca`, `Volumes.Especie`, `Volumes.Numeracao`, `MovimentacoesReferenciadas.Numero`, `MovimentacoesReferenciadas.DataHoraEmissao`, `MovimentacoesReferenciadas.NomePessoa`, `MovimentacoesReferenciadas.QuantidadeVolumes`, `MovimentacoesReferenciadas.Total`, `RomaneiosCarga.Numero`, `RomaneiosCarga.Data`, `RomaneiosCarga.Placa`, `RomaneiosCarga.Motorista`, `RomaneiosCarga.Observacao`

**Contratos de serviço:**

- `IRomaneioCargaModuleService`

**Chamadas catalogadas:**

- `IRomaneioCargaModuleService.ObterRomaneiosCargaPorPeriodoSituacaoEVendedor`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [RomaneiosCargaPorPeriodoSituacaoEVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/RomaneioCarga/RomaneiosCargaPorPeriodoSituacaoEVendedor.cs) — SHA-256 `57bd2ffc7e5a79cd8239aeccf3547e72bb1995f14dbdf1be6b051445d3465fbc`
- [RomaneiosCargaPorPeriodoSituacaoEVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/RomaneioCarga/RomaneiosCargaPorPeriodoSituacaoEVendedor.Designer.cs) — SHA-256 `d389767f1d1b1d500863a00ddd2b9552ded83c28b36e26b399b0dc649248179c`

## Romaneios de carga por cidade e bairro

Classe: `RomaneiosCargaPorCidadeBairro`. Identificador: `RomaneioCarga.RomaneiosCargaPorCidadeBairro`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Situação; Ordenação; Mostrar resumo por bairro

**Campos apresentados (DTO):** `NumeroRomaneio`, `DataSaida`, `NomeCliente`, `Bairro`, `NumeroDocumento`, `Volumes`, `Valor`, `Cidade`

**Agrupamentos do layout:**

- `Cidade`

**Contratos de serviço:**

- `IRomaneioCargaModuleService`

**Chamadas catalogadas:**

- `IRomaneioCargaModuleService.ObterRomaneiosCargaPorCidadeEBairro`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [RomaneiosCargaPorCidadeBairro.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/RomaneioCarga/RomaneiosCargaPorCidadeBairro.cs) — SHA-256 `43f8f00ee0b731d8fb46076d7c9b30c9ef4c1962bb74bc8965b7cbe7caf51be6`
- [RomaneiosCargaPorCidadeBairro.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/RomaneioCarga/RomaneiosCargaPorCidadeBairro.Designer.cs) — SHA-256 `aa22376372f92cd0d78a15c418ffab5cf7c20b55075abec2b65058bf81c38f2f`

