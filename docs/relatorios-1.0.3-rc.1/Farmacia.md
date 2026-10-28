# Farmacia

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Balanço de aquisição de medicamentos para conferência

Classe: `BalancoAquisicaoMedicamentoParaConferencia`. Identificador: `Farmacia.BalancoAquisicaoMedicamentoParaConferencia`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo da Receita ; Classe Terapêutica; Ordenação

**Campos apresentados (DTO):** `NumeroAutorizacaoFuncinamento`, `NumeroLicencaFuncionamento`, `Exercicio`, `Cnpj`, `RazaoSocial`, `Itens.CodigoDcb`, `Itens.DescricaoDcb`, `Itens.NomeMedicamento`, `Itens.ApresentacaoConcentracao`, `Itens.EstoqueInicial`, `Itens.EntradaAquisicao`, `Itens.SaidaVenda`, `Itens.Perda`, `Itens.EstoqueFinal`

**Contratos de serviço:**

- `ISngpcModuleService`

**Chamadas catalogadas:**

- `ISngpcModuleService.ObterBalancoAquisicaoMedicamentoParaConferencia`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-farmacia-sngpc-pbm.md](../../docs/continuacao-farmacia-sngpc-pbm.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Receita query has no company predicate in the analysed method
- EstoqueQuantidade sign convention not confirmed

**Fontes da ficha:**

- [BalancoAquisicaoMedicamentoParaConferencia.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Farmacia/BalancoAquisicaoMedicamentoParaConferencia.cs) — SHA-256 `10fd116897c660bae70fe0c179c512472c7b01797091272f4707c45a41c956c2`
- [BalancoAquisicaoMedicamentoParaConferencia.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Farmacia/BalancoAquisicaoMedicamentoParaConferencia.Designer.cs) — SHA-256 `f95f1c1b4b717964b00d805a0eb3e06715fb8e1ca80c00dcc497882cd159d92e`

## Balanço de medicamentos

Classe: `BalancoAquisicaoMedicamentoSemClasseTerapeutica`. Identificador: `Farmacia.BalancoAquisicaoMedicamentoSemClasseTerapeutica`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo da Receita ; Ordenação

**Campos apresentados (DTO):** `NumeroAutorizacaoFuncinamento`, `NumeroLicencaFuncionamento`, `Exercicio`, `Cnpj`, `RazaoSocial`, `Itens.CodigoDcb`, `Itens.DescricaoDcb`, `Itens.NomeMedicamento`, `Itens.ApresentacaoConcentracao`, `Itens.EstoqueInicial`, `Itens.EntradaAquisicao`, `Itens.SaidaVenda`, `Itens.Perda`, `Itens.EstoqueFinal`

**Contratos de serviço:**

- `ISngpcModuleService`

**Chamadas catalogadas:**

- `ISngpcModuleService.ObterBalancoAquisicaoMedicamentoParaConferenciaSemClasseTerapeutica`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-farmacia-sngpc-pbm.md](../../docs/continuacao-farmacia-sngpc-pbm.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Receita query has no company predicate in the analysed method
- EstoqueQuantidade sign convention not confirmed

**Fontes da ficha:**

- [BalancoAquisicaoMedicamentoSemClasseTerapeutica.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Farmacia/BalancoAquisicaoMedicamentoSemClasseTerapeutica.cs) — SHA-256 `9e7462a8407113685006bd4f83236c4c04ef5af7d3e921f3a80a563d8f309cd9`
- [BalancoAquisicaoMedicamentoSemClasseTerapeutica.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Farmacia/BalancoAquisicaoMedicamentoSemClasseTerapeutica.Designer.cs) — SHA-256 `0bb6177d2f4c73c0b1056acb5e5b4add7285c84d64e1db79064304507eef74ad`

## Livro definitivo de aquisição de medicamentos

Classe: `BalancoAquisicaoMedicamentoLivroDefinitivo`. Identificador: `Farmacia.BalancoAquisicaoMedicamentoLivroDefinitivo`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Classe Terapêutica; Ordenação

**Campos apresentados (DTO):** `RazaoSocial`, `Cnpj`, `Exercicio`, `NumeroLicencaFuncionamento`, `NumeroAutorizacaoFuncinamento`, `Itens.CodigoDcb`, `Itens.DescricaoDcb`, `Itens.NomeMedicamento`, `Itens.ApresentacaoConcentracao`, `Itens.CnpjEmpresaFornecedora`, `Itens.EmpresaFornecedora`, `Itens.NumeroNotaFiscal`, `Itens.QuantidadeAdquirida`

**Contratos de serviço:**

- `ISngpcModuleService`

**Chamadas catalogadas:**

- `ISngpcModuleService.ObterBalancoAquisicaoMedicamentoLivroDefinitivo`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-farmacia-sngpc-pbm.md](../../docs/continuacao-farmacia-sngpc-pbm.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Receita query has no company predicate in the analysed method
- EstoqueQuantidade sign convention not confirmed

**Fontes da ficha:**

- [BalancoAquisicaoMedicamentoLivroDefinitivo.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Farmacia/BalancoAquisicaoMedicamentoLivroDefinitivo.cs) — SHA-256 `27f8b179b19073c1f31a5eb72787115ad04c052c9ece6338a843a5d08e29199b`
- [BalancoAquisicaoMedicamentoLivroDefinitivo.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Farmacia/BalancoAquisicaoMedicamentoLivroDefinitivo.Designer.cs) — SHA-256 `523d4921482170446ae474d6dda76646958b08022405162e3001a0541b755b0b`

## Relação mensal de notificação de receita 'A' (RMNRA)

Classe: `RelacaoMensalNotificacaoReceitaA`. Identificador: `Farmacia.RelacaoMensalNotificacaoReceitaA`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo do filtro da data; Data inicial; Data final; Classe Terapêutica; Ordenação

**Campos apresentados (DTO):** `Endereco`, `Telefone`, `RazaoSocial`, `Cnpj`, `NumeroLicencaFuncionamento`, `NumeroAutorizacaoFuncinamento`, `Itens.CodigoDcb`, `Itens.DescricaoDcb`, `Itens.NomeMedicamento`, `Itens.ApresentacaoConcentracao`, `Itens.NumeroNotificacao`, `Itens.DataNotificacao`, `Itens.NomePrescritor`, `Itens.CrmPrescritor`, `Itens.QuantidadePrescrita`, `Itens.QuantidadeDispensada`

**Contratos de serviço:**

- `ISngpcModuleService`

**Chamadas catalogadas:**

- `ISngpcModuleService.ObterRelacaoNotificacaoReceita`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-farmacia-sngpc-pbm.md](../../docs/continuacao-farmacia-sngpc-pbm.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Receita query has no company predicate in the analysed method
- EstoqueQuantidade sign convention not confirmed

**Fontes da ficha:**

- [RelacaoMensalNotificacaoReceitaA.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Farmacia/RelacaoMensalNotificacaoReceitaA.cs) — SHA-256 `4ac6b7441e90a883c7b870c1ada58be1cc6ffa52938a4d7236907d68d9e1f09b`
- [RelacaoMensalNotificacaoReceitaA.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Farmacia/RelacaoMensalNotificacaoReceitaA.Designer.cs) — SHA-256 `fda5f723edb621e0b7eeeee3e8bf7511304a18507dc3f6167c477d33e4ef38c4`

## Relação mensal de notificação de receita 'B' (RMNRB1 e RMNRB2)

Classe: `RelacaoMensalNotificacaoReceitaB`. Identificador: `Farmacia.RelacaoMensalNotificacaoReceitaB`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Tipo do filtro da data; Data inicial; Data final; Classe Terapêutica; Subdivisão do tipo de receituário; Ordenação

**Campos apresentados (DTO):** `Endereco`, `Telefone`, `RazaoSocial`, `Cnpj`, `NumeroLicencaFuncionamento`, `NumeroAutorizacaoFuncinamento`, `Itens.CodigoDcb`, `Itens.DescricaoDcb`, `Itens.NomeMedicamento`, `Itens.ApresentacaoConcentracao`, `Itens.NumeroNotificacao`, `Itens.DataNotificacao`, `Itens.NomePrescritor`, `Itens.CrmPrescritor`, `Itens.QuantidadePrescrita`, `Itens.QuantidadeDispensada`

**Contratos de serviço:**

- `ISngpcModuleService`

**Chamadas catalogadas:**

- `ISngpcModuleService.ObterRelacaoNotificacaoReceita`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-farmacia-sngpc-pbm.md](../../docs/continuacao-farmacia-sngpc-pbm.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Receita query has no company predicate in the analysed method
- EstoqueQuantidade sign convention not confirmed

**Fontes da ficha:**

- [RelacaoMensalNotificacaoReceitaB.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Farmacia/RelacaoMensalNotificacaoReceitaB.cs) — SHA-256 `852400833e52e3b3497a617be416eada6c6f7d85b378dfe436fcfddb35a75bf2`
- [RelacaoMensalNotificacaoReceitaB.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Farmacia/RelacaoMensalNotificacaoReceitaB.Designer.cs) — SHA-256 `3f83054e8107050861ed8080e7f803a2d211e84ee50afe27a4c97e83f2caa12c`

## RelacaoMensalVendasMedicamentosSujeitosControleEspecial

Classe: `RelacaoMensalVendasMedicamentosSujeitosControleEspecial`. Identificador: `Farmacia.RelacaoMensalVendasMedicamentosSujeitosControleEspecial`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Classe Terapêutica; Ordenação

**Campos apresentados (DTO):** `Endereco`, `Telefone`, `RazaoSocial`, `Cnpj`, `NumeroLicencaFuncionamento`, `NumeroAutorizacaoFuncinamento`, `Itens.CodigoDcb`, `Itens.DescricaoDcb`, `Itens.NomeMedicamento`, `Itens.ApresentacaoConcentracao`, `Itens.QuantidadeVendida`, `Itens.NumeroLote`, `Itens.NumeroNotaFiscal`, `Itens.DataEmissaoNotaFiscal`, `Itens.EnderecoCompletoComprador`, `Itens.CnpjComprador`, `Itens.UfComprador`

**Contratos de serviço:**

- `ISngpcModuleService`

**Chamadas catalogadas:**

- `Não identificada na classe de entrada`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-farmacia-sngpc-pbm.md](../../docs/continuacao-farmacia-sngpc-pbm.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Receita query has no company predicate in the analysed method
- EstoqueQuantidade sign convention not confirmed

**Fontes da ficha:**

- [RelacaoMensalVendasMedicamentosSujeitosControleEspecial.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Farmacia/RelacaoMensalVendasMedicamentosSujeitosControleEspecial.cs) — SHA-256 `20abf23d34c2968daa86daa3e3a6927441ed4bcc3d747b18ae801a6a9e1e393d`

## Transações PBM

Classe: `TransacoesPbm`. Identificador: `Farmacia.TransacoesPbm`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Tipo

**Campos apresentados (DTO):** `ValorRepasse`, `ValorTotal`, `NumeroMovimentacao`, `TipoMovimentacao`, `cnpj`, `DataInclusao`, `NumeroAutorizacao`, `Itens.PrecoUnitario`, `Itens.Desconto`, `Itens.TotalRepasse`, `Itens.Total`, `Itens.PrecoFarmacia`, `Itens.Quantidade`, `Itens.Descricao`, `Itens.CodigoBarras`

**Contratos de serviço:**

- `ITransacaoPbmModuleService`

**Chamadas catalogadas:**

- `ITransacaoPbmModuleService.ObterRelatorioTransacoesPbm`

**Pesquisa da regra:** Entrada catalogada; execução interna pendente Rastreamento complementar disponível; runtime não homologado..

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)
- [Rastreamento: continuacao-farmacia-sngpc-pbm.md](../../docs/continuacao-farmacia-sngpc-pbm.md)

**Limites registrados na pesquisa:**

- No runtime observation
- Receita query has no company predicate in the analysed method
- EstoqueQuantidade sign convention not confirmed

**Fontes da ficha:**

- [TransacoesPbm.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Farmacia/TransacoesPbm.cs) — SHA-256 `4e013cd82475e25075a6b4fa3ce947aabdbf0441523e044f19f4f6863bcace43`
- [TransacoesPbm.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Farmacia/TransacoesPbm.Designer.cs) — SHA-256 `3e3370ba4106012ef4978a350dddecf78ed7c6fe2e878fdb5cec454220f086de`

