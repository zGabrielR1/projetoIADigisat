# Relação vendedor clientes

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Clientes por vendedor

Classe: `ClientesPorVendedor`. Identificador: `RelacaoVendedorCliente.ClientesPorVendedor`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor; Ordenação

**Campos apresentados (DTO):** `MunicipioToText`, `FormaPagamento`, `TabelaPreco`, `DocumentoCliente`, `NomeCliente`, `NomeVendedor`, `Telefone`

**Agrupamentos do layout:**

- `NomeVendedor`

**Resumos do layout:**

- `Count`

**Contratos de serviço:**

- `IRelacaoVendedorClienteRelatorioModuleService`

**Chamadas catalogadas:**

- `IRelacaoVendedorClienteRelatorioModuleService.ObterClientesPorVendedor`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No company predicate in the application method
- Active clients only

**Fontes da ficha:**

- [ClientesPorVendedor.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/RelacaoVendedorCliente/ClientesPorVendedor.cs) — SHA-256 `572b4cb0cbdde0c07f1555a56091062ce78665dd8924cfef6128e63ebe4b185d`
- [ClientesPorVendedor.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/RelacaoVendedorCliente/ClientesPorVendedor.Designer.cs) — SHA-256 `87c6cd6dc155c6127bb890de6c4a78a6aba98b155aa360d4186cfc466d5ce146`

## Relação Vendedor Clientes

Classe: `RelacaoVendedorClientes`. Identificador: `RelacaoVendedorCliente.RelacaoVendedorClientes`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Vendedor

**Campos apresentados (DTO):** `DocumentoCliente`, `NomeCliente`, `Telefone`

**Resumos do layout:**

- `Count`

**Contratos de serviço:**

- `IRelacaoVendedorClienteRelatorioModuleService`

**Chamadas catalogadas:**

- `IRelacaoVendedorClienteRelatorioModuleService.ObterRelacaoVendedorClientes`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-modulos-restantes.md](../../docs/continuacao-modulos-restantes.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Specification identity in the installed build not reconfirmed
- Driver translation of the predicates unknown
- No active/client predicate
- Smaller DTO than ClientesPorVendedor
- No company predicate in the application method

**Fontes da ficha:**

- [RelacaoVendedorClientes.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/RelacaoVendedorCliente/RelacaoVendedorClientes.cs) — SHA-256 `439b407c36a3f5746d4358fea43015584551bc710e12bfae0462e1b371a132ca`
- [RelacaoVendedorClientes.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/RelacaoVendedorCliente/RelacaoVendedorClientes.Designer.cs) — SHA-256 `55f0679f409bf61932daf0fde637b0dc170c8277b7272fc348bfa3257cd2f2bd`

