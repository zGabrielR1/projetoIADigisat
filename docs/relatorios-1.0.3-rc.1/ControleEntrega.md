# Controle de entregas

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Controles de entrega por produto

Classe: `ControlesEntregaPorProduto`. Identificador: `ControleEntrega.ControlesEntregaPorProduto`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Produto; Data inicial; Data final

**Campos apresentados (DTO):** `PessoaDocumentoNome`, `Data`, `Quantidade`, `QuantidadeDevolvida`, `QuantidadeEntregue`, `QuantidadePendente`, `Descricao`, `CodigoInterno`, `Marca`

**Agrupamentos do layout:**

- `NomeEmpresa`
- `CnpjEmpresa`

**Contratos de serviço:**

- `IControleEntregaModuleService`

**Chamadas catalogadas:**

- `IControleEntregaModuleService.ObterControlesEntregaPorProduto`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ControlesEntregaPorProduto.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ControleEntrega/ControlesEntregaPorProduto.cs) — SHA-256 `5fbde45f689efb4edbc8c62c6142110f22a551dcded6318c0919762c9bf8d78f`
- [ControlesEntregaPorProduto.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ControleEntrega/ControlesEntregaPorProduto.Designer.cs) — SHA-256 `4a9df3e976af5a9398c486a13a37fab38ea10bc748bf025a8b549300320a2fbb`

## Lista de controles de entrega

Classe: `ListaControleEntrega`. Identificador: `ControleEntrega.ListaControleEntrega`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Consolidado; Data inicial; Data final; Mostrar registros de entrega; Situação

**Campos apresentados (DTO):** `Situacao.Descricao`, `Observacao`, `DataInclusao`, `Cliente.NomeDocumentoToText`, `Numero`, `Empresa`, `CnpjEmpresa`, `Itens.QuantidadePendente`, `Itens.QuantidadeEntregue`, `Itens.QuantidadeDevolvida`, `Itens.Quantidade`, `Itens.Marca`, `Itens.ProdutoDescricao`, `Itens.UnidadeMedida`, `Itens.CodigoInterno`, `RegistrosEntrega.Responsavel`, `RegistrosEntrega.Data`, `RegistrosEntrega.Itens.ProdutoDescricao`, `RegistrosEntrega.Itens.Marca`, `RegistrosEntrega.Itens.UnidadeMedida`, `RegistrosEntrega.Itens.Quantidade`, `RegistrosEntrega.Itens.QuantidadeDevolvida`, `RegistrosEntrega.Itens.QuantidadeEntregue`, `RegistrosEntrega.Itens.CodigoInterno`

**Agrupamentos do layout:**

- `CnpjEmpresa`
- `TipoMovimentacao`
- `TipoMovimento`
- `NaturezaOperacao`

**Contratos de serviço:**

- `IControleEntregaModuleService`

**Chamadas catalogadas:**

- `IControleEntregaModuleService.ObterControlesEntregaPorPeriodo`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaControleEntrega.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/ControleEntrega/ListaControleEntrega.cs) — SHA-256 `c172452b0daabd61cd072a6ac61fbcd66c1d62895eaafc03908fc5a3b6071750`
- [ListaControleEntrega.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/ControleEntrega/ListaControleEntrega.Designer.cs) — SHA-256 `41786e2b69f531d717b97e7730abcdfc2b713a178ac9549ceeed0300c1280473`

