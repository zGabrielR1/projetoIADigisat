# Rastreio complementar — origem do fechamento de 15/10

Apurado em 22/10/2026. Complementa e atualiza [a primeira apuração](apuracao-caixa-1510.md). Não houve escrita no MongoDB, restauração de backup nem abertura/fechamento de turno para teste.

## Conclusão sustentada

O caminho rastreado na versão instalada trata `TotalCreditoLoja` como um valor de entrada do formulário. Não há nesse caminho uma soma automática dos recebimentos que produza 411,80. O formulário permite informar esse valor e o servidor o grava sem conciliá-lo com os lançamentos.

Isso identifica um mecanismo que permite a divergência: o fechamento pode receber 411,80 mesmo quando os lançamentos válidos somam 56,00. Não recupera o ato histórico de digitação, colagem ou cálculo externo, nem prova quem estava fisicamente operando. Não foi encontrado um pagamento adicional de 20,00 nem uma alteração financeira de 20,00 que justifique o excesso.

A frase anterior “falta rastrear a tela” está resolvida para a versão instalada; a causa histórica exata do preenchimento permanece sem evidência.

## Caminho da tela até o banco

Arquivos em `engenharia/caixa-1510/`, descompilados dos binários instalados com ILSpy 11.1.0.9782. Sem execução da aplicação-alvo, sem REA Evidence: REA MCP/CLI não estavam disponíveis, e foi usada a ferramenta local já instalada.

1. `modulo-completo/Modulo.Turnos.decompiled.cs`, `TurnoListaViewModel.FecharTurno`, linhas 1468–1478: passa o DTO do turno selecionado para o formulário. Não consulta pagamentos para preencher os campos.
2. `DigisatServer.Application.Core.Projections.AuxiliaresProjection.Turnos.TurnoProjection.decompiled.cs`, linhas 46–68: fechamento inexistente vira `FechamentoDtoNull`; fechamento existente copia os valores já salvos.
3. `DigisatServer.Application.Main.Dto.Auxiliares.Turnos.FechamentoDtoNull.decompiled.cs`: `TotalCreditoLoja` retorna zero. Portanto o caminho examinado de um turno ainda sem fechamento começa com zero, não com um total de vendas.
4. `modulo-completo/Modulo.Turnos.decompiled.cs`, linhas 2412–2433: `ProjectedAsViewModel` copia `Dto.Fechamento.TotalCreditoLoja` para a propriedade da tela.
5. `turnofechamentoview.xaml`, linhas 123–124: campo “Total crédito loja” é um `TextEdit` com vínculo `TotalCreditoLoja`, `Mode=TwoWay` e atualização a cada mudança. O controle não está marcado como somente leitura nesse XAML.
6. Mesmo módulo, linhas 2435–2457: `AtualizarDto` envia o valor da propriedade da tela para um novo `FechamentoDto`. `CalcularTotal`, linhas 2463–2465, apenas soma as doze espécies em `TotalGeral`; não calcula crédito loja a partir das vendas.
7. `TurnoFechamentoFichaViewModel.Persistir` chama `FecharTurnoAsync(formulario.ToDto<TurnoDto>())`.
8. `DigisatServer.Application.Main.AuxiliaresModulo.Services.TurnoAppService.decompiled.cs`, `FecharTurno`, linha 161: transfere `turnoDto.Fechamento.TotalCreditoLoja` ao builder que será gravado. O trecho não compara com os pagamentos antes de gravar.

O módulo também oferece “Corrigir fechamento” para turno já fechado ainda não conferido, condicionado à permissão. Isso demonstra possibilidade de edição; não demonstra que houve correção nesse caso.

### Limite de versão

Os binários instalados examinados têm versão 2.0.26293.5, posterior à data do turno. Hashes em `evidencias-tela-logs-caixa.json`. Não foi encontrada, nessa investigação, uma cópia do módulo cliente da versão executada na loja em 15/10. A lógica do servidor também foi confrontada com os fontes antigos já preservados; não se pode tratar a análise estática da instalação atual como gravação do que ocorreu naquela data.

## Backups históricos: identidade conferida

Os ZIPs de 15/10 às 11:06 e 14:10 e de 16/10 às 07:46 em `C:\DigiSat_Backup` são de outra empresa. Têm 1 turno de 22/09 e nenhum identificador de turno comum ao ZIP fornecido pelo usuário. Portanto a sugestão de compará-los como estados anteriores da loja estava errada. Detalhes em [comparação dos backups](comparacao-backups-historicos-caixa.md).

## Busca ampliada nos dados

- Todas as coleções BSON do ZIP fornecido foram percorridas; a revisão ampliou o filtro inicial para todos os campos numéricos, classificando códigos/quantidades separadamente de dinheiro.
- 411,80 aparece como valor monetário apenas no fechamento investigado. Não foi encontrado 355,80. Os cinco lançamentos antigos de 20,00 são de 16/06,14/08,08/09,24/09 e 08/10, sem vínculo com o turno de 15/10.
- Nos 97 documentos com campos monetários e alguma data de 15/10 examinados na revisão, não apareceu delta de 20,00 entre históricos consecutivos. Houve 459,40→459,00 e correções 335,80→0,10. Isso não exclui alteração que não tenha sido preservada no histórico.
- Na venda 474, `ItensPagamentos` contém 335,80 de crédito e 93,90 a prazo; troco zero. Desconto/acréscimo/taxa da parcela analisada não produzem 20,00.
- Há uma inconsistência real: a cópia embutida `PagamentoRecebimento.Parcelas` da venda 474 ainda guarda a parcela de 335,80 quitada, enquanto o documento correspondente em `Recebimentos` está estornado. Uma consulta baseada nessa cópia pode continuar mostrando 335,80, mas não foi demonstrado que essa cópia alimentou o fechamento, e ela não explica os 20,00.

Evidência: `evidencias-backups-historicos-caixa.json`, `evidencias-variacoes-caixa.json` e `evidencias-caixa-1510.json`.

## Logs e possibilidade de recuperar o ato de preenchimento

- `C:\Digisat\SuiteG 6\Log`, diretório usado por `GerarLog`: vazio na inspeção. O trecho de servidor preservado registra início/fim de `FecharTurno`, não os valores do DTO.
- Varridos os arquivos de logs em `Servidor\Logs` e `MongoDB\log` pelos identificadores do turno/parcela, `TotalCreditoLoja` e 411,8/411,80; não foi encontrado registro correspondente. Logs de integração encontrados não são uma trilha de auditoria do campo.
- O log local Mongo referente a 15/10 identifica host Suporte 13 e encerra às 17:56, antes do fechamento 18:27 da loja. Não estabelece uma captura da operação da cliente.
- O intervalo disponível do `local.oplog.rs` começa em 20/10/2026, sem cobrir 15/10. Não se mudou configuração de log/profiling nem se tentou recuperar arquivos apagados.

A evidência disponível não permite reconstruir o conteúdo anterior do campo, sua sequência de teclas ou um documento já removido sem histórico. É necessário comprovante/anotação usado para o fechamento, relato da operadora, log da máquina original da loja ou backup histórico da empresa correta para avançar nessa atribuição.

## Pergunta operacional agora precisa

“No fechamento de 15/10 você informou 411,80 em Total crédito loja. A venda 474 era 335,80 e a 476 era 56,00, somando 391,80 mesmo incluindo a estornada. Qual anotação, cálculo ou relatório você usou para preencher 411,80 ? ”

Essa pergunta é sustentada pela diferença e pelo formulário examinado. Não é acusação nem prova de erro pessoal. Os 20,00 são excesso no valor informado sobre a soma bruta identificada; não há evidência de saída física de 20,00 do caixa.

## Complemento: PDV e origem do número no relatório

O caminho próprio do PDV foi examinado depois da tela de gestão de turnos. Veja [o rastreio do PDV](rastreio-pdv-caixa.md). `PainelFechamentoTurno.F2Method` copia `_totalCreditoLoja.Valor` para `FechamentoDto.TotalCreditoLoja`; não foi encontrada soma de pagamentos para preencher esse campo. Esse achado se refere à versão instalada, posterior ao turno.

Para o relatório gráfico `ResumoTurno`, a origem é distinta da conferência: `TurnoRelatorioAppService.ObterResumoTurno` constrói a linha de crédito loja com `Valor = byId.TurnoFechamento.CreditoLoja` (fonte preservado, linha 138). `Modulo.Turnos.Reports.ResumoTurno.Processar` usa o DTO como DataSource, e o controle de valor tem binding `ResumoTurnoLancamentos.Valor` (`engenharia/caixa-1510/modulo-completo/Modulo.Turnos.decompiled.cs`, linhas 3589–3600 e 3867). Portanto esse relatório reproduz o valor armazenado; não há soma das vendas nessa linha.

Na conferência gráfica, o layout tem bindings distintos para `EspeciesRecebimento.ValorInformado` e `ValorCalculado`; a rotina de cálculo já descrita neste documento produz o segundo a partir dos lançamentos válidos, e lê o primeiro do fechamento.

A impressão térmica atual do PDV concatena blocos de origens diferentes. Seu bloco “informado pelo operador” não imprime crédito loja no código examinado. Por isso não se deve equiparar automaticamente a linha de um relatório histórico a esse bloco térmico. O título e o caminho de emissão exatos do documento do usuário ainda precisam ser associados ao layout correspondente; a pergunta foi apresentada ao usuário. Não há prova de uma fórmula que some os registros conhecidos e produza 411,80.

O e-mail foi revisto para explicar os critérios do resumo e da conferência, sem sugerir que a simples geração do documento prova um total calculado e sem atribuir erro à operadora. O ato histórico de preenchimento continua não recuperado.
