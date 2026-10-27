# Rastreio estático adicional do PDV — caixa de 15/10

Data: 22/10/2026. Escopo: somente leitura dos binários instalados; sem execução do aplicativo e sem alteração do banco.

## Identidade e método

Alvo: `C:\Digisat\SuiteG6\Sistema\Modulos\Modulo.PontoDeVendas.dll`, versão de arquivo 2.0.26293.5, LastWriteTime 21/10/2026 09:11:32. SHA-256 `7BC4BC9BAD3413DAFB436456E69956BE8C7E2445C572FA4CB63F1463AE9C1D89`.

Decompilação ILSpy com resolução de referências em `C:\Digisat\SuiteG6\Sistema`. Fontes preservados em `engenharia/caixa-1510/pdv/Modulo.PontoDeVendas.decompiled.cs` e versões focadas em `pdv/focado/`. Os números de linha abaixo apontam para os arquivos focados quando indicado. São evidências estáticas, não observação do fechamento histórico.

## Entrada do fechamento no PDV

Em `focado/Otus.Modulo.PontoDeVendas.ViewModels.Paineis.PainelFechamentoTurno.decompiled.cs`:

- Linha 107 cria `_totalCreditoLoja` como `ItemInteracaoDecimal`, descrição “Total em crédito loja”. Linha 116 adiciona o campo à coleção `Campos`.
- Linhas 131–165, método `Load`, obtêm turno aberto, sincronizam dados, verificam pendências fiscais e abrem gaveta. Não atribuem valores financeiros ao campo crédito loja.
- Linha 278 limpa o campo. No fonte completo, `ItemInteracaoDecimal.Limpar`, linhas 37186–37189, define `Valor = 0m`; a propriedade base `Valor`, linhas 36931–36942, apenas lê/grava a propriedade, sem cálculo.
- Linhas 290–305, ao confirmar F2, criam `FechamentoDto`. A linha 304 faz `TotalCreditoLoja = _totalCreditoLoja.Valor`.
- Linha 307 envia o DTO para `FecharTurnoAsync`. Linha 309 inicia a impressão.

A busca no fonte completo pelas ocorrências de `TotalCreditoLoja` e `_totalCreditoLoja` não revelou preenchimento automático desse campo a partir de pagamentos, neste painel. Isso sustenta o comportamento do campo nesse binário; não prova quem inseriu 411,80 ou como o valor apareceu no relatório histórico.

## A impressão combina saídas de origens diferentes

Em `focado/Otus.Modulo.PontoDeVendas.ViewModels.Paineis.ImpressaoTurnoService.decompiled.cs`:

- Linhas 33–39: `ExecutarFechamentoTurno` pede `ObterConferenciaTurno(idTurnoFechado)` ao serviço de relatórios. Em seguida imprime a concatenação de `ObterRelatorioFechamentoDeTurno(_turnoDto)` e `ObterRelatorioConferenciaFechamentoDeTurno(result, ...)`.
- Linhas 42–46: a opção resumo sintético pede `ObterConferenciaTurnoSintetico(idTurnoFechado)` e passa turno e resultado ao gerador `ObterRelatorioResumoFechamentoDeTurno`.
- No fonte completo, a reimpressão chama o primeiro fluxo na linha 28218. O painel de resumo chama o segundo na linha 28032.

Logo, “gerado pelo sistema” não identifica por si só qual origem do valor foi impressa: é preciso mapear a linha e seção do documento ao gerador concreto.

## Limite relevante do gerador térmico atual

O fonte preservado pelo investigador principal em `engenharia/caixa-1510/gerador/DigisatServer.Domain.Main.Services.GeradorImpressoTurnoService.decompiled.cs`, método `ObterRelatorioFechamentoDeTurno`, começa na linha 1219 e rotula um bloco como “RESUMO INFORMADO PELO OPERADOR” na linha 1230. O bloco imprime várias espécies de `turnoDto.Fechamento`, porém não contém acesso a `TotalCreditoLoja` nem linha específica de crédito loja. As duas larguras de bobina repetem essa ausência.

Portanto, não há fundamento para dizer que uma linha “Crédito loja 411,80” veio desse bloco térmico atual. Pode haver outra saída, como o relatório de resumo do módulo Turnos, que deve ser rastreada diretamente. A simples existência do DTO com esse valor não conclui a origem da linha do documento apresentado ao usuário.

## Perguntas e estado

- Como o PDV atual preenche e transmite o campo de fechamento? Respondido estaticamente: campo decimal e cópia do valor para DTO.
- Como imprime e reimprime fechamento? Respondido: concatena bloco do DTO e bloco de conferência; resumo sintético é caminho separado.
- Esse caminho produz automaticamente 411,80 ou o adicional 20,00? Não encontrado no caminho de entrada; não demonstrado na saída específica.
- Qual método produziu o relatório histórico que contém 411,80? Pendente de identificar o documento/seção e confrontar com os geradores.
- O binário prova o comportamento em 15/10? Não: o binário instalado é posterior ao fato. É necessário manter a limitação temporal.
