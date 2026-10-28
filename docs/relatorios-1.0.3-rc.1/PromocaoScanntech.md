# Clube de Promoção

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Itens vendidos com promocão Scanntech

Classe: `ItensVendidosComPromocaoScanntech`. Identificador: `PromocaoScanntech.ItensVendidosComPromocaoScanntech`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Promoção; Ordenação

**Campos apresentados (DTO):** `DataHora`, `CodigoBarras`, `Desconto`, `Quantidade`, `Descricao`, `Empresa`, `CnpjEmpresa`, `TituloPromocao`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IPromocaoScanntechModuleService`

**Chamadas catalogadas:**

- `IPromocaoScanntechModuleService.ObterItensVendidosComPromocaoScanntech`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-promocao-scanntech.md](../../docs/continuacao-promocao-scanntech.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [ItensVendidosComPromocaoScanntech.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PromocaoScanntech/ItensVendidosComPromocaoScanntech.cs) — SHA-256 `24bd7d7fe10c745e5dc670c1af8dc42eb357e9446781fa538e09b6e882d343bd`
- [ItensVendidosComPromocaoScanntech.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PromocaoScanntech/ItensVendidosComPromocaoScanntech.Designer.cs) — SHA-256 `ee2dd9c95fcf8d40b9b4c14aa325352e51107b9c57f24e0699dec5761151b867`

## Lista de movimentações Scanntech

Classe: `ListaMovimentacoesScanntech`. Identificador: `PromocaoScanntech.ListaMovimentacoesScanntech`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final

**Campos apresentados (DTO):** `Cancelada`, `PessoaToText`, `Total`, `Pdv`, `Numero`, `DataHora`

**Agrupamentos do layout:**

- `DataHora`

**Contratos de serviço:**

- `IPromocaoScanntechModuleService`

**Chamadas catalogadas:**

- `IPromocaoScanntechModuleService.ObterMovimentacoesSincronizadas`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-promocao-scanntech.md](../../docs/continuacao-promocao-scanntech.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [ListaMovimentacoesScanntech.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PromocaoScanntech/ListaMovimentacoesScanntech.cs) — SHA-256 `1fd2962c70298872fc0dbe1b6d0f242f8017338b9a0b4d20f44a3d94410a49ca`
- [ListaMovimentacoesScanntech.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PromocaoScanntech/ListaMovimentacoesScanntech.Designer.cs) — SHA-256 `bae300d41d589c98a9e1cc11b5ec4ac1513332114d10b0685ceb1250aec79c38`

## Vendas por produto com promoção Scanntech

Classe: `VendasPorProdutoComPromocaoScanntech`. Identificador: `PromocaoScanntech.VendasPorProdutoComPromocaoScanntech`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Produto

**Campos apresentados (DTO):** `PrecoUnitario`, `Total`, `DataHora`, `Numero`, `Desconto`, `Quantidade`, `Descricao`, `CodigoBarras`, `Empresa`, `CnpjEmpresa`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `Empresa`

**Contratos de serviço:**

- `IPromocaoScanntechModuleService`

**Chamadas catalogadas:**

- `IPromocaoScanntechModuleService.ObterItensVendidosComPromocaoScanntechPorProduto`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-promocao-scanntech.md](../../docs/continuacao-promocao-scanntech.md)

**Limites registrados na pesquisa:**

- No REA Evidence
- No runtime observation
- Source identity is local decompiled file only

**Fontes da ficha:**

- [VendasPorProdutoComPromocaoScanntech.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/PromocaoScanntech/VendasPorProdutoComPromocaoScanntech.cs) — SHA-256 `3c5e5daccf19b727322fe5bbea800f63d5044d42c884b976dcbe7518e9e0e7b8`
- [VendasPorProdutoComPromocaoScanntech.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/PromocaoScanntech/VendasPorProdutoComPromocaoScanntech.Designer.cs) — SHA-256 `c54442e41ca6692e2f5cbe9e0bddd76b5a23a46aa5ad6bf3a374264d878f6221`

