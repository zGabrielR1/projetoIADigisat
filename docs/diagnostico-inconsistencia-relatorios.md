# Diagnóstico — inconsistência nos relatórios de vendas

Data: 2026-10-23. Base: `DigisatServer` ativa em `C:\Digisat\SuiteG6\Dados` (MongoDB 127.0.0.1:12220), somente leitura. O ZIP `D:\Downloads\DigisatServer20261020_134345.zip` é um dump (`Movimentacoes.bson.gz` com ~417 MB; 448 entradas) da mesma base. A base ativa reproduz exatamente os totais estáveis do relatório de 15/10 (setembro R$ 170.356,74; Vendedora 2 em setembro R$ 89.615,72), então foi usada para a análise. Nenhum registro foi alterado.

Ferramenta de reprodução: `engenharia/ferramentas/diagnostico_relatorios.py`.

## Resposta curta

1. **O relatório de vendas por mês é bruto ou líquido?** É **bruto em relação à forma de pagamento**: soma o `TotalLiquido` de cada item, que já desconta apenas descontos do item, mas **não** desconta parcelas de cartão nem taxas. Vendas no cartão entram normalmente. Ele é "líquido" só no sentido de item (descontos/acréscimos do item), não no sentido financeiro.
2. **Por que "vendas por CPF" muda quando um vendedor é selecionado?** Porque o relatório simplificado usa o **total do documento** (todos os itens) na coluna/agrupamento por CPF, mas projeta **somente os itens do vendedor** na lista de itens. Em documento com itens de mais de um vendedor, o total do CPF inclui os itens dos outros vendedores.
3. **A diferença de ~R$ 100,00 em 05/10 na Vendedora 2?** São exatamente **R$ 104,96**, de dois documentos compartilhados com a Vendedora 4: R$ 64,98 + R$ 39,98. Os pagamentos batem integralmente; não é cartão, taxa ou pagamento faltante.

## 1. Como o relatório mensal calcula

`ObterTotalMovimentacoesPorMesPorVendedor` percorre as movimentações do ano e, para cada item não cancelado, soma `item.TotalLiquido` em uma matriz `[vendedor][mês]`:

- filtros: empresa ativa, `PorDataEmissao(01/01 a 31/12)`, situação, origem, finalidade normal e NFC-e diferente de movimentação item financeiro;
- itens auxiliares só entram se a operação fiscal movimenta estoque **e** financeiro; itens de NFC-e entram normalmente;
- se um vendedor é informado, o filtro é aplicado **no item** (`VendedorReferencia`);
- vendedor vazio vira "Não informado";
- `TotalLiquido = round2(preço × quantidade) − DescontoDigitado − DescontoProporcional + OutrasDespesasProporcional + OutrasDespesasDigitado + Percentual.ValorSt + IPI.Valor + Frete + Seguro`.

Não há nenhuma subtração de `ItensPagamentos`, cartão, taxa de cartão ou parcela. O relatório é a soma do valor dos itens por vendedor.

### Prova de que o cartão está incluído

Setembro/2026, NFC-e concluída, escopo de vendas: itens = **R$ 170.356,74**; pagamentos = **R$ 170.356,74**. A composição dos pagamentos é:

| Espécie | Valor | Participação |
|---|---:|---:|
| Cartão POS | R$ 111.806,62 | 65,63% |
| Dinheiro | R$ 39.518,29 | 23,20% |
| Carteira digital | R$ 16.371,88 | 9,61% |
| Crédito loja | R$ 2.394,05 | 1,41% |
| Vale presente | R$ 265,90 | 0,16% |

Se o relatório excluísse cartão, setembro seria R$ 58.550,12; se excluísse também carteira digital/PIX, R$ 42.178,24. O valor mostrado é R$ 170.356,74, portanto o cartão está incluído. O documento 001/217242, de 05/10, é 100% cartão e seus itens aparecem nos totais das vendedoras.

## 2. Por que a visão por CPF difere com vendedor selecionado

O DTO do relatório simplificado (`MovimentacaoRelatorioBaseDto`) tem duas bases diferentes:

- `Total = movimentacao.Total` — total do **documento inteiro**, com todos os itens;
- `Itens = ProjectedAsCollectionItemRelatorioBaseDto(vendedorReferencia, ...)` — se há vendedor selecionado, a projeção filtra `ItensBase` por `VendedorReferencia` e devolve **somente os itens daquele vendedor**.

Assim, em um documento com itens de dois vendedores:

- a coluna/agrupamento por CPF, que usa `Total`, mostra o valor cheio do documento;
- a lista de itens e o total líquido dos itens mostram só a parte do vendedor filtrado;
- a diferença é o valor dos itens dos outros vendedores.

O relatório mensal **não** tem esse comportamento: ele agrupa os itens por vendedor e soma apenas os itens de cada um. Por isso o mensal é a base correta para comissão por vendedor; a visão simplificada com filtro de vendedor é uma visão por item dentro do documento.

## 3. O caso de 05/10 — Vendedora 2

Dois documentos de 05/10 têm itens da Vendedora 2 e da Vendedora 4. Ambos foram pagos integralmente em Cartão POS:

| Documento | Hora | Total | Vendedora 2 | Vendedora 4 | Pagamento |
|---|---|---:|---:|---:|---:|
| 001/217242 | 10:29 | R$ 119,96 | R$ 54,98 | R$ 64,98 | R$ 119,96 |
| 001/217322 | 16:19 | R$ 113,92 | R$ 73,94 | R$ 39,98 | R$ 113,92 |
| **Total** | | **R$ 233,88** | **R$ 128,92** | **R$ 104,96** | **R$ 233,88** |

Itens:

- 001/217242: Vendedora 2 — CAL. SANDRA 24,99 + CALCINHA ALTA LATERAL 29,99 = 54,98; Vendedora 4 — MEIA SAPATILHA 24,99 + MEIA LUPO KIT3 39,99 = 64,98.
- 001/217322: Vendedora 2 — 4 × MEIA CAL. CLAS. 14,99 + 2 × SAPATILHA FIO 6,99 = 73,94; Vendedora 4 — 2 × TANGA SUELANE 19,99 = 39,98.

No dia 05/10, todos os documentos que contêm a Vendedora 2 somam R$ 2.145,11; os itens dela somam R$ 2.040,15. A diferença é **R$ 104,96**, exatamente os itens da Vendedora 4 nos dois documentos. Se a visão por CPF/relatório simplificado atribui o documento inteiro à Vendedora 2, ela mostra R$ 2.145,11; o relatório mensal mostra R$ 2.040,15. É a mesma causa descrita no item 2, não um erro de pagamento.

## 4. Comparação dos relatórios de 01/10 e 15/10

A diferença de outubro é **momento de geração**, não fórmula:

- O relatório de 01/10 foi gerado no início do dia 01/10 e mostrava R$ 478,76 em outubro.
- O relatório de 15/10 foi gerado durante o dia 15/10 e mostrava R$ 70.315,96.
- A base completa de 01–15/10 soma R$ 75.296,16. A diferença de R$ 4.980,20 é o restante das vendas de 15/10 (e eventuais ajustes) posterior ao momento de geração. O valor "Não informado" (R$ 70,97) é idêntico nos dois recortes, coerente com a venda sem vendedor ter ocorrido antes da geração.

Os demais meses mudaram entre as duas gerações:

- **Setembro:** +R$ 4.769,81, todo na Vendedora 2. A base atual tem 5 documentos de setembro com histórico posterior a 01/10, somando R$ 552,24 (todos ainda concluídos): 001/215160 (R$ 112,45), 001/215376 (R$ 119,94), 001/215471 (R$ 137,95), 001/216112 (R$ 42,97) e 001/216916 (R$ 138,93). O restante (R$ 4.217,57) não é explicado por mudança de situação registrada; pode ser edição de preço/quantidade ou reatribuição de vendedor sem histórico, ou parâmetro diferente entre as duas gerações. Fica como pendência de conferência.
- **Abril:** −R$ 2.896,07. Nenhum documento de abril na base atual tem histórico posterior a 01/10. A redução não é explicada por cancelamento registrado; mesma limitação de edição sem histórico/parâmetro.
- **Maio a agosto:** variações menores; mesma limitação de estado final sem histórico de edição de item.

Para fechar esses pontos, é necessário comparar os parâmetros exatos das duas gerações (origem, situação, empresa, vendedor, tipo de data) e exportar o detalhe de cada uma. A base atual só mostra o estado final.

## 5. Recomendações

- **Comissão:** usar o relatório mensal por vendedor. Ele soma somente os itens do vendedor e inclui cartão; é a base bruta correta. Não usar o total do documento/CPF com filtro de vendedor como valor do vendedor.
- **Sintegra:** conferir o total sem filtro de vendedor (todos os vendedores) ou o total do documento no relatório simplificado. Com vendedor selecionado, a visão é um subconjunto por item, não por documento.
- **Se o cliente quiser CPF por vendedor:** o relatório precisa exibir as duas bases explicitamente ("total do documento" e "total dos itens do vendedor") ou ratear o documento; caso contrário, documentos compartilhados sempre vão divergir.
- **Se o cliente quiser conferir o mensal com o simplificado:** comparar o mensal **sem filtro de vendedor** com a soma dos documentos do simplificado; os totais devem bater. Com filtro de vendedor, comparar apenas a coluna de itens.

## 6. Reprodução

```powershell
.\.venv\Scripts\python.exe engenharia/ferramentas/diagnostico_relatorios.py --year 2026
.\.venv\Scripts\python.exe engenharia/ferramentas/diagnostico_relatorios.py --date 2026-10-05 --seller-ref 6496ee283a5e352378b9e8f1
```

O script é somente leitura, usa `app.mongo_client()` e a mesma fórmula de item do painel. Ele imprime os totais mensais por vendedor e os documentos compartilhados de um dia, com `documento`, `vendedor`, `outros` e `pago`.

## Limites

- A análise foi feita na base ativa, que é posterior ao ZIP; os meses estáveis (jan–mar e setembro) reproduzem exatamente o relatório de 15/10. O ZIP é um dump da mesma base e não foi restaurado.
- Não houve execução de assembly nem observação de runtime. A regra do relatório mensal e a separação `Total`/`Itens` vêm do C# descompilado preservado (2.0.26273.6) e foram conferidas contra os dados.
- O valor de `movimentacao.Total` é o total do documento no domínio; a composição exata de descontos/acréscimos de documento não foi reextraída nesta análise. A fórmula de item foi auditada em 152.034 itens sem divergência de arredondamento.
- As diferenças de setembro/abril não registradas em histórico permanecem sem causa comprovada; são pendências de conferência, não conclusões.
