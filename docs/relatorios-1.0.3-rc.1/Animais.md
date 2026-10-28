# Animais

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Aniversariantes

Classe: `Aniversariantes`. Identificador: `Animais.Aniversariantes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `NomeProprietario`, `Telefone`, `DataNascimento`, `Idade`, `Nome`

**Expressões calculadas do layout:**

- `DateDiffYear([DataNascimento],LocalDateTimeNow())`

**Contratos de serviço:**

- `IAnimalModuleService`

**Chamadas catalogadas:**

- `IAnimalModuleService.ObterAniversariantes`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: engenharia-reversa-relatorios.md](../../docs/engenharia-reversa-relatorios.md)

**Fontes da ficha:**

- [Aniversariantes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Animal/Aniversariantes.cs) — SHA-256 `f0f0aba64f99332ea48dc1401e6a714b74df8a7ccba7a5cb1ca231213cbac66f`
- [Aniversariantes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Animal/Aniversariantes.Designer.cs) — SHA-256 `5aedcad8d5f64c8923cd561625d442c0218606437f30749678df80a6b12db04c`

## Lista de consultas

Classe: `ListaConsultas`. Identificador: `Animais.ListaConsultas`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pet; Data inicial; Data final; Ordenação

**Campos apresentados (DTO):** `Observacao`, `Diagnostico`, `Tratamento`, `ExamesSolicitados`, `SuspeitaClinica`, `SinaisClinicos`, `Anamnese`, `VeterinarioNome`, `ValorServico`, `ServicoDescricao`, `DataConsulta`, `AnimalNome`, `ProprietarioNome`

**Agrupamentos do layout:**

- `AnimalReferencia`

**Contratos de serviço:**

- `IAnimalModuleService`

**Chamadas catalogadas:**

- `IAnimalModuleService.ObterConsultas`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaConsultas.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Animal/ListaConsultas.cs) — SHA-256 `fa2a046fdf331f9c3383fc6ea28ca6f02686706114cfd9d2189c684e07411ec2`
- [ListaConsultas.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Animal/ListaConsultas.Designer.cs) — SHA-256 `6f109f4b58d81532cc968eb3ef08b1cf0be8983b44555f493da30ea47d9c44ac`

## Lista de internações

Classe: `ListaInternacoes`. Identificador: `Animais.ListaInternacoes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pet; Data inicial de inclusão; Data final de inclusão; Exibir óbitos/altas

**Campos apresentados (DTO):** `DataHoraInternacao`, `TituloAltaObito`, `DataAltaObito`, `Sexo`, `Idade`, `Raca`, `Especie`, `AnimalDescricao`

**Agrupamentos do layout:**

- `AnimalReferencia`

**Contratos de serviço:**

- `IInternacaoModuleService`

**Chamadas catalogadas:**

- `IInternacaoModuleService.ObterInternacoesRelatorio`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaInternacoes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Animal/ListaInternacoes.cs) — SHA-256 `79dbbd75c732f8ed3138b799d7c70b4d574d470cace54484db8c93cbd02408ff`
- [ListaInternacoes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Animal/ListaInternacoes.Designer.cs) — SHA-256 `6dabe8adcd5de23c7e456d910a1f9640f5a7e80756c5a1904456d7e7ee629902`

## Lista de pets

Classe: `ListaAnimais`. Identificador: `Animais.ListaAnimais`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Ordenação; Espécie; Raça

**Campos apresentados (DTO):** `Microchip`, `Sexo`, `VivoToText`, `Raca`, `Especie`, `IdadeToText`, `NomeAnimal`, `NomeProprietario`, `TelefoneProprietario`, `EnderecoProprietario`, `DocumentoProprietario`

**Agrupamentos do layout:**

- `IdProprietario`

**Contratos de serviço:**

- `IAnimalModuleService`

**Chamadas catalogadas:**

- `IAnimalModuleService.ObterAnimais`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaAnimais.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Animal/ListaAnimais.cs) — SHA-256 `4d17ebaf9ec5be2a8419edf6c5a1db46c5623895fce97702e6a626cc60099532`
- [ListaAnimais.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Animal/ListaAnimais.Designer.cs) — SHA-256 `1729ce3befaf1cf3d766ef512baa377d1e580ab7838304de05ec7318e63962a5`

## Lista de vacinas

Classe: `ListaVacinacoes`. Identificador: `Animais.ListaVacinacoes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Pet; Aplicada; Data/Hora inicial; Data/Hora final; Ordenação

**Campos apresentados (DTO):** `aplicada`, `LaboratorioNome`, `VeterinarioNome`, `DataAplicacao`, `AnimalNome`, `ProprietarioNome`, `Itens.Quantidade`, `Itens.PrecoUnitario`, `Itens.ProdutoServico.Descricao`

**Agrupamentos do layout:**

- `AnimalReferencia`

**Expressões calculadas do layout:**

- `Iif([DoseAplicada]==true, 'Sim','Não')`
- `Iif([ProximaDoseAplicada]==true, 'Sim','Não')`

**Contratos de serviço:**

- `IAnimalModuleService`

**Chamadas catalogadas:**

- `IAnimalModuleService.ObterVacinacoes`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaVacinacoes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Animal/ListaVacinacoes.cs) — SHA-256 `cf4010bff652ade6040d56d18273175b71b50665ecfd2ec7a1280368ea6a9ad2`
- [ListaVacinacoes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Animal/ListaVacinacoes.Designer.cs) — SHA-256 `773dad8d503ebb85235c5b83ece9d910e2d1af802d0870b7550c4cd2bdbbf9c3`

