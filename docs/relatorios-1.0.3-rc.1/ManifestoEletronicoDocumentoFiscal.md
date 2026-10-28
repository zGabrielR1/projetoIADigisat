# MDF-e

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de manifestações eletrônicas de documentos fiscais

Classe: `ListaManifestoEletronicoDocumentoFiscal`. Identificador: `ManifestoEletronicoDocumentoFiscal.ListaManifestoEletronicoDocumentoFiscal`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Documento condutor; Situação

**Campos apresentados (DTO):** `DocumentosReferenciados`, `Condutores`, `ValorCarga`, `ChaveAcesso`, `Situacao`, `PlacaVeiculo`, `TotalPesoBruto`, `DataHoraEmissao`, `NomeProprietarioVeiculo`, `Serie`, `Numero`

**Contratos de serviço:**

- `IManifestoEletronicoDocumentoFiscalRelatorioModuleService`

**Chamadas catalogadas:**

- `IManifestoEletronicoDocumentoFiscalRelatorioModuleService.ObterManifestosEletronicosDocumentoFiscalPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- Conductor filter is applied in memory after the query
- No null guard on modal/vehicle when filtering conductor
- Contract and application method names diverge

**Fontes da ficha:**

- [ListaManifestoEletronicoDocumentoFiscal.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ManifestoEletronicoDocumentoFiscal/ListaManifestoEletronicoDocumentoFiscal.cs) — SHA-256 `1f7da90ce1ded8629063087928825ff50a9c9e741ed8de69cd014ab3652c457e`
- [ListaManifestoEletronicoDocumentoFiscal.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ManifestoEletronicoDocumentoFiscal/ListaManifestoEletronicoDocumentoFiscal.Designer.cs) — SHA-256 `3a47dbc368de95da08896e1dadfb0c0c7c3b2b3a1d3ab449e678d9d9ae7f5a51`

