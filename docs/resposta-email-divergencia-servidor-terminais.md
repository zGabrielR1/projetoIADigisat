# Resposta ao e-mail de 28/10/2026 — divergência entre SERVIDOR, CANGURU e ESCRITORIO

Assunto sugerido: **Relatórios por mês: a diferença está no cálculo do relatório, não nos dados**

Bom dia, Giovani.

Obrigado pelos três anexos. Eles fecham a dúvida que estava em aberto: a diferença não vem dos dados nem da sincronização. Ela acontece na soma que o próprio relatório faz.

Comparei os três PDFs célula a célula. São 5 vendedores por 12 meses, 60 valores. Cinquenta e um estão idênticos nas três emissões. Nos nove restantes, o valor de um relatório é sempre menor que o dos outros dois, e a máquina que fica abaixo muda conforme o mês. Em maio e junho, quem perde é o CANGURU. Em julho e agosto, é o SERVIDOR. Em janeiro, julho, setembro e outubro, é o ESCRITORIO.

Isso descarta a hipótese de base incompleta nos terminais. Se os terminais tivessem dados faltando, o servidor, que é a origem da base, nunca ficaria abaixo deles. Em julho, o SERVIDOR mostra R$ 229.043,64, o ESCRITORIO R$ 229.686,62 e o CANGURU R$ 231.891,70. Em agosto ocorre o mesmo: R$ 166.062,25 no SERVIDOR contra R$ 167.377,71 nos dois terminais. O relatório do servidor não é o correto, e repetir a sincronização não muda isso.

O defeito está na rotina que totaliza o relatório. Ela lê as movimentações do ano em blocos de 500 documentos, soma cada bloco em paralelo e junta os blocos em um acumulador por vendedor e mês. Nessa junção, duas tarefas podem ler o mesmo valor ao mesmo tempo e uma sobrescrever a soma da outra. O resultado é sempre menor que o real, nunca maior, e o mês prejudicado depende de como as tarefas se cruzaram naquela execução. Três relatórios pedidos ao mesmo tempo disputam o mesmo processador do servidor, o que aumenta a chance de cada um perder uma parte diferente. A base fica intacta; o que varia é a soma.

Sobre a sua observação: você está certo. Os valores de meses fechados não deveriam mudar, e eles não mudaram na base. O que muda é o total que o relatório consegue somar a cada emissão. Meses anteriores aparecem diferentes porque a perda caiu neles naquela rodada, não porque houve venda, cancelamento ou alteração nova.

Sobre qual valor usar: como a falha só subtrai, o valor correto de cada célula é no mínimo o maior entre os três relatórios. Somando esses valores, o total de 2026 fica em R$ 1.679.455,08, contra R$ 1.675.291,56 do SERVIDOR, R$ 1.674.440,08 do ESCRITORIO e R$ 1.674.032,03 do CANGURU. É um piso: se alguma célula perdeu valor nas três emissões, o total real é um pouco maior. O número definitivo sai com a versão corrigida.

O que já está em andamento:

- A correção da soma foi reproduzida e validada em ensaio: com o mesmo conjunto de dados, o cálculo atual acertou 1 de 40 execuções e a correção acertou 40 de 40.
- A mesma rotina é usada por outros relatórios mensais, que também serão revisados.
- Depois da homologação, será publicada uma versão de suporte com o cálculo corrigido.

Para confirmar no ambiente de vocês, há um teste simples: gerar o mesmo relatório três vezes seguidas, no mesmo computador, com os mesmos parâmetros, sem nenhuma venda nova no período. Se os totais de meses fechados mudarem entre as três emissões, o defeito está confirmado. Se puder me enviar também a versão completa do sistema no servidor e em um terminal, confirmo se o código instalado é o mesmo que analisamos aqui.

Enquanto isso, evite usar o total desses relatórios para fechamento de meses anteriores. Se precisar de um número agora, há dois caminhos. O mais simples é emitir o relatório "Itens movimentados por vendedor" para o mesmo período, com a mesma origem e situação, exportar para Excel e totalizar por vendedor e mês: esse relatório lê os mesmos documentos sem a soma paralela e não varia entre execuções. O outro é o recálculo direto na base, com os mesmos filtros e soma única, que eu posso levantar para vocês.

Fico à disposição para acompanhar o teste.

Abraço,
Gabriel
