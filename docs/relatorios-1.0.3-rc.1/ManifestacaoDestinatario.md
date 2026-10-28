# Manifestação do destinatário

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de manifestações do destinatário

Classe: `ListaManifestacaoDestinatario`. Identificador: `ManifestacaoDestinatario.ListaManifestacaoDestinatario`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Quanto ao XML importado; Ordenação

**Campos apresentados (DTO):** `XmlImportadoToText`, `NumeroNota`, `ValorTotal`, `Situacao`, `RazaoSocial`, `DataHoraEmissao`, `ChaveAcesso`

**Contratos de serviço:**

- `IManifestacaoDestinatarioModuleService`

**Chamadas catalogadas:**

- `IManifestacaoDestinatarioModuleService.ObterManifestacoesDestinatarioPorPeriodo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- XML-imported filter is applied in memory on the screen

**Fontes da ficha:**

- [ListaManifestacaoDestinatario.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ManifestacaoDestinatario/ListaManifestacaoDestinatario.cs) — SHA-256 `ab237a4158f60ef0301f7adeb1a726ca461e71e90cd4ebada261b3e01cf0c236`
- [ListaManifestacaoDestinatario.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ManifestacaoDestinatario/ListaManifestacaoDestinatario.Designer.cs) — SHA-256 `0d98a847708822096684c7eb9314578d65cd3a64b73aed155b331958595d7b87`

