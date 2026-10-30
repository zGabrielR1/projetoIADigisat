# Solução para a revenda — divergência do relatório "Movimentações por mês"

O problema não está na base nem na sincronização: está na soma que o relatório faz no servidor. A solução tem três frentes, e a revenda pode executar duas delas sem esperar o build corrigido.

## Frente 1 — Número confiável agora (sem esperar correção)

Enquanto o cálculo do relatório não for corrigido, o número de fechamento pode ser obtido por outro relatório nativo que percorre os mesmos dados sem a soma paralela. A opção mais direta é o **Itens movimentados por vendedor**.

### Opção A — relatório "Itens movimentados por vendedor"

Esse relatório usa a mesma especificação de filtros do mensal (empresa, emissão, situação, origem, finalidade normal e exclusão de NFC-e de item financeiro), mas monta a lista item a item em uma única passagem, sem paralelismo e sem acumulador compartilhado. Cada linha traz data, vendedor, quantidade e o `TotalLiquido` do item, e o layout fecha subtotal por vendedor. Rodando uma vez por período, o resultado é estável em qualquer execução.

Procedimento:

1. Emitir **Itens movimentados por vendedor** com período de 01/01/2026 00:00 a 31/12/2026 23:59:59, Origem "Nota fiscal de consumidor eletrônica" e Situação "Concluído", os mesmos parâmetros do mensal.
2. Exportar para Excel.
3. Montar uma tabela dinâmica com vendedor nas linhas, mês da coluna Data nas colunas e soma de Total nas células.
4. Calibrar nos meses em que os três relatórios coincidem, comparando vendedor por vendedor:

| Mês | VENDEDORA 1 | VENDEDORA 2 | VENDEDORA 3 | VENDEDORA 4 | Não informado |
|---|---:|---:|---:|---:|---:|
| Fevereiro | 40.691,46 | 48.280,13 | 0,00 | 0,00 | 0,00 |
| Março | 63.550,69 | 68.800,38 | 2.979,18 | 195,95 | 5,99 |
| Abril | 72.238,49 | 74.504,40 | 47.946,80 | 12,49 | 25,99 |

5. Se esses três meses baterem, usar a tabela dinâmica para os meses divergentes (janeiro, maio, junho, julho, agosto, setembro e outubro) e somar o valor de "Não informado" impresso em qualquer um dos três relatórios.

Duas diferenças conhecidas entre esse relatório e o mensal, que a calibração resolve:

- O detalhado não inclui itens sem vendedor, então a linha "Não informado" (R$ 143,43 no ano) não aparece. Ela nunca divergiu entre os três relatórios e pode ser somada a partir do mensal.
- O detalhado filtra apenas item não cancelado; o mensal exige também que a operação fiscal do item movimente estoque e financeiro. Se a loja tiver linhas auxiliares, o detalhado pode sair maior, e os meses de calibração mostram isso imediatamente.

### Opção B — recálculo direto na base

Quando houver acesso ao MongoDB do servidor, uma agregação única, sem paralelismo, devolve a mesma grade em todas as execuções. O recálculo já existe em [diagnostico_relatorios.py](../engenharia/ferramentas/diagnostico_relatorios.py), mas ainda precisa incorporar a exclusão de NFC-e com movimentação de item financeiro e a exigência de item que movimenta estoque e financeiro para ficar idêntico ao relatório nativo.

Procedimento:

1. Rodar o coletor de ambiente no servidor e nos terminais para registrar versões, hashes e o destino real das consultas:
   `powershell -NoProfile -ExecutionPolicy Bypass -File .\engenharia\ferramentas\coletar_sincronizacao.ps1 -Identificacao SERVIDOR -OutputDirectory .\diagnostico`
2. Executar o recálculo na base do servidor, em modo somente leitura, com empresa, ano, origem, situação e finalidade.
3. Calibrar com fevereiro, março e abril, como na opção A.
4. Entregar ao cliente a planilha por vendedor e mês, com a indicação de que ela substitui o total impresso até a correção.

## Frente 2 — Prova do defeito para o cliente

O teste que dispensa acesso à base e convence qualquer um:

1. No servidor, gerar o mesmo relatório três vezes seguidas, com os mesmos parâmetros, sem nenhuma venda nova no período.
2. Comparar os totais de um mês fechado nas três emissões.
3. Se os valores mudarem entre as emissões, o defeito está demonstrado: a base não mudou, a soma mudou.

Nos anexos de 28/10 a prova já está feita de outra forma: 51 das 60 células são idênticas nas três máquinas e as 9 restantes são sempre menores que o maior valor observado, com a máquina mais baixa mudando de mês (SERVIDOR em julho e agosto, CANGURU em maio e junho, ESCRITORIO em janeiro, julho, setembro e outubro). O relatório do SERVIDOR, que seria a referência natural, é o menor em dois meses.

## Frente 3 — Correção definitiva no produto

A correção é pequena e localizada. Em `ObterTotalMovimentacoesPorMesPorVendedor`, o merge entre as partições paralelas altera um array compartilhado:

```csharp
acumuladoGlobal.AddOrUpdate(chave, parcial, (key, existing) => {
    for (int i = 0; i < 12; i++)
        existing[i] += parcial[i];
    return existing;
});
```

O dicionário concorrente protege o mapa, não a soma do array. A correção publica um array novo, sem efeito colateral no callback:

```csharp
acumuladoGlobal.AddOrUpdate(chave, parcial, (key, existing) => {
    var novo = new decimal[12];
    for (int i = 0; i < 12; i++)
        novo[i] = existing[i] + parcial[i];
    return novo;
});
```

Alternativa equivalente: guardar as partições e reduzi-las em série depois do `Parallel.ForEach`, padrão que o mesmo arquivo já usa em outro totalizador.

Escopo a corrigir no conjunto recuperado ([MovimentacaoSaidaRelatorioAppService.cs](../engenharia/sincronizacao-2026-10-30/fontes/MovimentacaoSaidaRelatorioAppService.cs:148)):

| Linha | Rotina | Relatório |
|---|---|---|
| 213 | `ObterTotalMovimentacoesPorMesPorVendedor` | Movimentações por mês (por vendedor) |
| 3893 | `TotalMovimentadoPorVendedor` | Total movimentado por vendedor |
| 6046 | `ObterTotalMovimentacoesPorMesPorTipo` | Movimentações por mês (por tipo) |

O conjunto recuperado cobre parte do produto. A auditoria completa precisa ser feita no fonte oficial, procurando `AddOrUpdate` com array mutável e qualquer soma em estrutura compartilhada dentro de `Parallel.ForEach`.

Detalhe operacional importante: no build analisado, esse cálculo roda no processo do servidor, e o terminal apenas solicita o resultado. Se a versão do cliente seguir esse desenho, a atualização do servidor resolve para todos os terminais. Confirmar com o coletor de ambiente antes de prometer isso ao cliente.

## Homologação e critérios de aceite

O build só deve ser liberado com estas verificações:

- Dez execuções seguidas do mesmo relatório, mesmos parâmetros, sem alteração na base: valores idênticos nas dez.
- Comparação com soma serial de referência na mesma base, tolerância de R$ 0,01 por célula.
- Base com mais de 500 documentos no período, para exercitar a paginação.
- Documentos com itens de dois vendedores, itens cancelados e documentos cancelados.
- Graus de paralelismo 1, 2, 3 e 8, todos com o mesmo resultado.
- Relatórios de origem e situação diferentes, para cobrir os filtros.

Enquanto o build não passar nesses critérios, o recálculo da Frente 1 é a fonte oficial dos números.

## Contenção enquanto o defeito existe

- Não repetir a sincronização como tentativa de correção: ela não altera o cálculo e mexe em dados sem necessidade.
- Não usar o total desses relatórios para fechar mês nem para calcular comissão. A perda observada foi de 0,25% a 0,32% do total anual, distribuída em vendedores e meses diferentes, o que muda valores individuais de comissão.
- Se a revenda precisar de um número nativo, usar um relatório detalhado de itens por vendedor e período e totalizar fora do sistema; o detalhado não passa por essa rotina de soma paralela.
- Registrar versão e hash de cada máquina avaliada, para que a conclusão não dependa de suposição sobre o ambiente.
- Não apagar coleções locais, `LastTs` ou checkpoints, e não reemitir documentos. Nenhum dado foi perdido: a diferença está na soma.

## O que entregar para a revenda

1. Este documento e o [laudo com as três tabelas](analise-relatorios-mes-2026-10-28.md).
2. O [texto de resposta ao cliente](resposta-email-divergencia-servidor-terminais.md), adaptado pela revenda.
3. O recálculo determinístico da base, com a grade vendedor × mês e exportação em planilha.
4. O coletor de ambiente, para registrar versões e destino das consultas.
5. O build corrigido, quando homologado, com nota de versão informando o defeito e o que foi alterado.

## Mensagem curta para a revenda repassar

> A diferença entre os relatórios não é de dados. O totalizador do relatório soma os blocos de documentos em paralelo e, em algumas execuções, perde parte da soma. Por isso o mesmo relatório, na mesma base e no mesmo momento, sai diferente, e o mês afetado muda a cada emissão. A base está íntegra e os documentos fiscais não são afetados.
>
> Enquanto a correção é publicada, os totais de meses fechados devem ser apurados pelo relatório "Itens movimentados por vendedor" exportado para Excel e totalizado por mês, ou por recálculo direto na base com os mesmos filtros do mensal. A correção do totalizador já está identificada e em homologação; a atualização é no servidor.
