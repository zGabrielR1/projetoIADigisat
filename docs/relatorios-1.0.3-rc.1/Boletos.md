# Gestão de Boletos

[Todos os módulos](README.md)

Parâmetros e campos transcritos da apresentação nativa. Um binding de DTO pode depender de projeções, cálculos e sub-relatórios; não deve ser usado como campo MongoDB sem rastreamento.

## Lista de boletos

Classe: `ListaBoletos`. Identificador: `Boletos.ListaBoletos`.

**Execução no Insight:** Conjuntos de dados disponíveis pelos serviços originais do SuiteG6, com login nativo. Pós-processamento da tela e formato de impressão não são reproduzidos.

**Parâmetros nativos:** Data inicial; Data final; Conta; Situação do boleto; Situação da antecipação

**Campos apresentados (DTO):** `ParcelaRecebimento.Pessoa.Nome`, `Documento`, `NossoNumeroFormatado`, `ParcelaRecebimento.DataVencimento`, `dataRemessaFormatada`, `valorParcela`, `ParcelaRecebimento.Conta.Agencia.Banco.Descricao`, `agenciaDigito`, `contaDigito`, `dataAntecipacaoFormatada`

**Expressões calculadas do layout:**

- `[ParcelaRecebimento.ValorTotal]`
- `Concat([ParcelaRecebimento.ContaBoleto.Agencia.NumeroConta], ' - ' ,[ParcelaRecebimento.ContaBoleto.Agencia.DigitoConta])`
- `Concat([ParcelaRecebimento.ContaBoleto.Agencia.NumeroAgencia],  ' - ' ,[ParcelaRecebimento.ContaBoleto.Agencia.DigitoAgencia])`
- `Iif([DataRemessa] == '01/01/0001 00:00:00', '', [DataRemessa])`
- `Iif([DataAntecipacao] == '01/01/0001 00:00:00', '', [DataAntecipacao])`

**Contratos de serviço:**

- `IBoletoModuleService`

**Chamadas catalogadas:**

- `IBoletoModuleService.ObterListaBoletos`

**Pesquisa da regra:** Rastreado em profundidade no documento principal.

- [Rastreamento: relatorios-nativos-1.0.3.md](../../docs/relatorios-nativos-1.0.3.md)

**Fontes da ficha:**

- [ListaBoletos.cs](../../engenharia/assemblies-descompilados/2.0.26273.6/Modulo.Relatorios/Modulo.Relatorios/Reports/Boletos/ListaBoletos.cs) — SHA-256 `342f5320edb233cc06d9e4a659f5ecfa99836613e61158677928b2259376c0bf`
- [ListaBoletos.Designer.cs](../../engenharia/codigo-descompilado/Modulo.Relatorios/Reports/Boletos/ListaBoletos.Designer.cs) — SHA-256 `61603372cf5713cad883d05ae391dac197c19bb63a72041c034c9f8a9156ec87`

