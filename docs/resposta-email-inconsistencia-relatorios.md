# Achados — outubro, geração de 23/10 e relatórios de vendas

## Resposta curta

- A base disponível termina em **20/10/2026 13:43:34** (última NFC-e 001/218692). Uma geração feita hoje, 23/10, com esta base mostra o mesmo outubro de **R$ 89.813,23**; não existem vendas de 21 a 23/10 nela.
- O relatório de 15/10 mostrava **R$ 70.315,96** em outubro. A diferença de **R$ 19.497,27** é **R$ 4.980,20** no recorte de 15/10 (diferença entre o relatório de 15/10 e o dia completo) + **R$ 14.517,07** de 16 a 20/10.
- O ano na base atual soma **R$ 1.641.542,97**. Contra 15/10, só mudaram abril (+R$ 2.896,07), maio (+R$ 1.205,55) e outubro (+R$ 19.497,27); junho a setembro estão estáveis.
- O relatório nativo é **NFC-e + situação Concluído + finalidade normal**. O painel com “NFC-e válidas” mostra **R$ 89.911,16** porque inclui uma NFC-e Autorizado de R$ 97,93; com “todas as vendas” mostra **R$ 90.160,42** porque inclui três NF-e de saída de R$ 249,26 sem pagamento.
- O relatório mensal é bruto em relação à forma de pagamento: inclui cartão e não desconta parcelas/taxas. Em outubro o cartão é R$ 55.737,73 (62,06%).
- A diferença por CPF com vendedor selecionado é o total do documento versus os itens do vendedor. Em outubro, a exposição é: V1 R$ 159,93; V2 R$ 104,96; V3 R$ 19,99; V4 R$ 375,81. O caso de 05/10 da V2 é os R$ 104,96.

## 1. Base e corte

- Base ativa: MongoDB 127.0.0.1:12220, somente leitura. O ZIP `DigisatServer20261020_134345.zip` é um `mongodump` da mesma base.
- Última emissão: **2026-10-20 13:43:34**, série 001, número 218692.
- Última gravação: **2026-10-20 16:43:13 UTC** (13:43 local). O nome do backup (`20261020_134345`) bate com o corte.
- Não há documentos de 21, 22 ou 23/10. O último dia com movimento é 20/10, e ele é parcial (até 13:43): R$ 1.607,32.
- Consequência: o relatório de outubro gerado hoje, 23/10, com esta base, é idêntico ao da base de 20/10. Se o relatório novo do cliente mostra outro valor, ele foi gerado em uma base mais nova; a diferença é exatamente o que existe depois de 20/10 13:43.

## 2. O que o relatório mensal soma

`ObterTotalMovimentacoesPorMesPorVendedor` soma `Item.TotalLiquido` por item, mês e `VendedorReferencia`, com:

- origem NFC-e;
- situação **Concluído**;
- finalidade **normal** (`FinalidadeEmissaoNfe.Codigo = 1`);
- empresa e data de emissão do filtro;
- itens cancelados fora;
- itens auxiliares só quando a operação movimenta estoque **e** financeiro.

Fórmula do item:

`round2(preço × quantidade) − DescontoDigitado − DescontoProporcional + OutrasDespesasProporcional + OutrasDespesasDigitado + Percentual.ValorSt + IPI.Valor + Frete + Seguro`.

É **bruto em relação à forma de pagamento**: cartão, PIX, dinheiro, vales e crédito loja entram; parcelas e taxas de cartão não são descontadas. É “líquido” apenas de descontos do item.

Prova em setembro/2026: total R$ 170.356,74; cartão POS R$ 111.806,62 (65,63%). Se excluísse cartão, seria R$ 58.550,12; se excluísse também carteira digital/PIX, R$ 42.178,24.

Em outubro (01 a 20/10, NFC-e/Concluído): total R$ 89.813,23; cartão R$ 55.737,73 (62,06%), dinheiro R$ 21.948,51 (24,44%), carteira digital R$ 10.285,15 (11,45%), crédito loja R$ 1.296,69 (1,44%), vale presente R$ 545,15 (0,61%). Itens = pagamentos = R$ 89.813,23.

## 3. Outubro: 15/10 -> hoje

### 3.1 Timeline

| Recorte | Valor | Composição |
|---|---:|---|
| 01 a 14/10 completos | R$ 69.248,35 | base |
| Relatório de 15/10 | R$ 70.315,96 | 69.248,35 + 1.067,61 (diferença para 01–14) |
| 15/10 completo | R$ 6.047,81 | 1.067,61 antes + 4.980,20 restantes |
| 16 a 20/10 | R$ 14.517,07 | 3.907,02 + 4.792,46 + 4.210,27 + 1.607,32 |
| **01 a 20/10 (hoje)** | **R$ 89.813,23** | 70.315,96 + 4.980,20 + 14.517,07 |

A diferença de **R$ 19.497,27** contra 15/10 não é recálculo de outubro: são vendas novas. O dia 20/10 é parcial porque a base foi extraída às 13:43.

### 3.2 Outubro por vendedor

| Vendedor | 15/10 | 01–15 atual | Δ 01–15 | 16–20 | 01–20 atual | Δ total |
|---|---:|---:|---:|---:|---:|---:|
| Vendedora 1 | 26.092,79 | 27.742,20 | +1.649,41 | 4.837,64 | 32.579,84 | +6.487,05 |
| Vendedora 2 | 22.552,09 | 24.562,00 | +2.009,91 | 5.865,57 | 30.427,57 | +7.875,48 |
| Vendedora 3 | 11.830,77 | 12.336,04 | +505,27 | 2.433,00 | 14.769,04 | +2.938,27 |
| Vendedora 4 | 9.769,34 | 10.584,95 | +815,61 | 1.380,86 | 11.965,81 | +2.196,47 |
| Não informado | 70,97 | 70,97 | 0,00 | 0,00 | 70,97 | 0,00 |
| **Total** | **70.315,96** | **75.296,16** | **+4.980,20** | **14.517,07** | **89.813,23** | **+19.497,27** |

Nove documentos de 01–15/10 têm evento posterior a 16/10, somando R$ 566,85 (todos Concluído): 001/216934, 001/217009, 001/217742, 001/218158, 001/218217, 001/218311, 001/218334, 001/218401 e 001/218406. O evento é “Concluído” e não necessariamente mudou valor; eles podem ajustar a divisão entre “antes” e “depois” da geração, mas não o total. A maior parte dos R$ 4.980,20 é venda de 15/10 após a geração.

### 3.3 Ano: 01/10, 15/10 e hoje

| Mês | Relatório 01/10 | Relatório 15/10 | Hoje (base 20/10) | Δ hoje vs 15/10 |
|---|---:|---:|---:|---:|
| Jan | 106.670,14 | 106.670,14 | 106.670,14 | 0,00 |
| Fev | 88.971,59 | 88.971,59 | 88.971,59 | 0,00 |
| Mar | 135.532,19 | 135.532,19 | 135.532,19 | 0,00 |
| Abr | 194.728,17 | 191.832,10 | 194.728,17 | **+2.896,07** |
| Mai | 226.532,51 | 227.054,74 | 228.260,29 | **+1.205,55** |
| Jun | 225.648,64 | 227.941,21 | 227.941,21 | 0,00 |
| Jul | 231.866,71 | 231.891,70 | 231.891,70 | 0,00 |
| Ago | 164.682,53 | 167.377,71 | 167.377,71 | 0,00 |
| Set | 165.586,93 | 170.356,74 | 170.356,74 | 0,00 |
| Out | 478,76 | 70.315,96 | 89.813,23 | **+19.497,27** |
| **Total** | **1.540.698,17** | **1.617.944,08** | **1.641.542,97** | **+23.598,89** |

Leitura:

- Junho a setembro estão estáveis entre 15/10 e hoje; outubro é o mês aberto.
- Setembro está em R$ 170.356,74 nas duas gerações. A diferença de +R$ 4.769,81 ocorreu entre 01/10 e 15/10 e não voltou a mudar. Na base atual, 5 documentos de setembro têm histórico após 01/10, somando R$ 552,24 (001/215160, 001/215376, 001/215471, 001/216112 e 001/216916); o restante (R$ 4.217,57) não tem histórico.
- Abril hoje está em R$ 194.728,17, exatamente o valor de 01/10; o relatório de 15/10 tinha R$ 191.832,10. A diferença de R$ 2.896,07 não tem evento de histórico posterior a 16/10 na base.
- Maio hoje está em R$ 228.260,29, acima de 01/10 (226.532,51) e de 15/10 (227.054,74). Também não há histórico posterior a 16/10.
- Sem histórico, a explicação provável é edição de item (preço/quantidade/vendedor) ou parâmetro diferente em uma das gerações. Não é o caso de outubro, que tem causa clara (vendas novas).

## 4. Por que o painel pode mostrar 89.911,16 ou 90.160,42

O painel tem escopos mais amplos que o relatório nativo:

| Regra | Total outubro | Documentos | Diferença |
|---|---:|---:|---:|
| Nativa: NFC-e + Concluído + finalidade normal | **89.813,23** | 1.768 | base |
| Painel “NFC-e válidas” (exclui cancelado/rejeitado/inutilizado) | 89.911,16 | 1.769 | **+97,93** |
| Painel “todas as vendas” | 90.160,42 | 1.772 | **+249,26** vs nativa |

- **+97,93**: NFC-e 001/217030, de 02/10, situação **Autorizado**, Vendedora 2. A regra nativa usa **Concluído**; “válidas” inclui Autorizado. O pagamento também entra, por isso o cartão do painel é R$ 55.835,66 contra R$ 55.737,73 do nativo.
- **+249,26**: três NF-e de saída 007/3829 (15,30), 007/3830 (92,02) e 007/3831 (141,94), de 05/10, todas Concluído. Elas não têm pagamento registrado; por isso o painel “todas as vendas” tem itens R$ 90.160,42 e pagamentos R$ 89.911,16, diferença de R$ 249,26.
- Antes de comparar números, alinhar **origem** (NFC-e), **situação** (Concluído) e **finalidade** (normal). O relatório do cliente usa NFC-e/Concluído; o painel com “válidas” é mais amplo.

## 5. CPF x vendedor em outubro

O relatório simplificado usa `Total = movimentacao.Total` (documento inteiro) e `Itens` filtrados pelo vendedor. Em documento compartilhado, a visão por CPF pode mostrar o documento cheio e os itens somente do vendedor. Em outubro, a exposição é:

| Vendedor | Docs compartilhados | Total dos documentos | Itens do vendedor | Valor dos outros |
|---|---:|---:|---:|---:|
| Vendedora 4 | 7 | 660,69 | 284,88 | **375,81** |
| Vendedora 1 | 4 | 370,84 | 210,91 | **159,93** |
| Vendedora 2 | 2 | 233,88 | 128,92 | **104,96** |
| Vendedora 3 | 1 | 55,97 | 35,98 | **19,99** |

A soma dos “outros” é R$ 660,69, o total dos documentos compartilhados. A V4 tem a maior exposição porque participa de mais documentos compartilhados; a V2 tem os R$ 104,96 do caso de 05/10.

## 6. O caso de 05/10 (Vendedora 2)

São exatamente **R$ 104,96**, em dois documentos compartilhados com a Vendedora 4, ambos pagos em cartão:

| Documento | Hora | Total | Vendedora 2 | Vendedora 4 | Pago |
|---|---|---:|---:|---:|---:|
| 001/217242 | 10:29 | 119,96 | 54,98 | 64,98 | 119,96 |
| 001/217322 | 16:19 | 113,92 | 73,94 | 39,98 | 113,92 |
| **Total** | | **233,88** | **128,92** | **104,96** | **233,88** |

Itens:

- **001/217242** — V2: CAL. SANDRA 24,99 + CALCINHA ALTA LATERAL 29,99 = 54,98; V4: MEIA SAPATILHA 24,99 + MEIA LUPO KIT3 39,99 = 64,98.
- **001/217322** — V2: 4 × MEIA CAL. CLAS. 14,99 = 59,96 + 2 × SAPATILHA FIO 6,99 = 13,98 → 73,94; V4: 2 × TANGA SUELANE 19,99 = 39,98.

No dia, os documentos que contêm a V2 somam R$ 2.145,11; os itens dela somam R$ 2.040,15; a diferença é R$ 104,96. Pagamentos integrais; não é cartão, taxa ou pagamento faltante.

## 7. O que está comprovado e o que falta

**Comprovado:**

- O corte da base é 20/10 13:43; não há 21–23/10. A geração de hoje com esta base é R$ 89.813,23 em outubro.
- A diferença de outubro contra 15/10 é R$ 19.497,27, decomposta em R$ 4.980,20 (15/10 após a geração) + R$ 14.517,07 (16–20/10).
- O relatório inclui cartão e não desconta parcelas/taxas; é líquido apenas de descontos do item.
- A diferença por CPF com vendedor é total do documento versus itens do vendedor; em outubro, V2 = R$ 104,96.
- O painel “válidas” adiciona R$ 97,93 (Autorizado) e “todas as vendas” adiciona R$ 249,26 (NF-e de saída sem pagamento).

**Em aberto:**

- Confirmar se os relatórios novos de outubro e de hoje foram gerados nesta base (corte 20/10 13:43) ou em uma base mais nova; se mais nova, a diferença é o que existe depois do corte.
- Confirmar os parâmetros das gerações de 01/10, 15/10 e hoje: origem, situação, finalidade, empresa, vendedor e tipo de data.
- Abril (+R$ 2.896,07) e maio (+R$ 1.205,55): sem histórico posterior a 16/10; precisam dos exports ou da confirmação de parâmetros.
- Definir se a comissão é sobre o valor antes ou depois dos descontos do item.
- Definir se a visão por CPF com vendedor deve mostrar o total rateado ou o total do documento.

## 8. Recomendações

- **Comissão:** relatório mensal por vendedor, NFC-e/Concluído. Ele atribui a cada vendedor somente os itens dele e inclui cartão.
- **Sintegra:** total sem filtro de vendedor (todos) ou total do documento no simplificado. Com vendedor selecionado, a visão é um subconjunto por item, não por documento.
- **Painel:** para reproduzir o relatório nativo, alinhar a situação para **Concluído**; “válidas” inclui Autorizado e “todas as vendas” inclui NF-e de saída.
- **CPF x vendedor:** se o cliente precisa que feche sempre, exibir as duas bases (“total do documento” e “total dos itens do vendedor”) ou ratear o documento.
- **Abril/maio:** comparar os exports das gerações; a base atual só mostra o estado final e não registra edição de item.

## 9. Apêndice — outubro por dia

| Dia | Total | Observação |
|---|---:|---|
| 01/10 | 4.606,91 | |
| 02/10 | 8.025,67 | |
| 03/10 | 3.394,27 | |
| 05/10 | 6.164,19 | caso V2 R$ 104,96 |
| 06/10 | 6.201,97 | |
| 07/10 | 6.978,32 | |
| 08/10 | 9.632,88 | |
| 09/10 | 7.884,78 | |
| 10/10 | 6.094,83 | |
| 13/10 | 5.427,46 | |
| 14/10 | 4.837,07 | fim do recorte 01–14 |
| 15/10 | 6.047,81 | 1.067,61 no relatório de 15/10; 4.980,20 depois |
| 16/10 | 3.907,02 | |
| 17/10 | 4.792,46 | |
| 19/10 | 4.210,27 | |
| 20/10 | 1.607,32 | parcial até 13:43 (corte da base) |
| **Total** | **89.813,23** | |

Dias sem movimento: 04, 11, 12, 18, 21, 22 e 23/10. O dia 20/10 é parcial; por isso uma base mais nova pode ter um valor maior apenas no dia 20 e nos dias 21–23.

## 10. Reprodutibilidade e limites

```powershell
.\.venv\Scripts\python.exe engenharia/ferramentas/diagnostico_relatorios.py --year 2026
.\.venv\Scripts\python.exe engenharia/ferramentas/diagnostico_relatorios.py --date 2026-10-05 --seller-ref 6496ee283a5e352378b9e8f1
.\.venv\Scripts\python.exe test_e2e_reports.py -v
```

- `docs/diagnostico-inconsistencia-relatorios.md` — diagnóstico detalhado.
- `docs/testes-e2e-relatorios.md` — parâmetros e testes.
- `test_e2e_reports.py` — 23 testes E2E, todos passando.
- `engenharia/ferramentas/diagnostico_relatorios.py` — ferramenta somente leitura.

Limites:

- A base ativa é posterior ao ZIP; os meses estáveis reproduzem exatamente o relatório de 15/10.
- `movimentacao.Total` é o total do documento no domínio; a composição exata de descontos/acréscimos de documento não foi reextraída.
- Abril/maio não têm histórico de alteração; a causa é inferida, não comprovada.
- Não houve execução de assembly nem observação de runtime; a regra vem da descompilação do build instalado e da base.
