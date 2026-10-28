# Contas a pagar e receber — implementação para a 1.0.3

As **21 entradas** dos módulos Pagamento e Recebimento possuem consultas de dados em `/finance-reports`. São oito entradas de pagamentos e treze de recebimentos. O catálogo `/reports` abre cada consulta. A validação visual com a impressão do SuiteG6 continua pendente; as versões sintéticas e de 40 colunas compartilham a consulta de dados, sem reproduzir o formato da impressora.

## Usar

1. Abra **Contas a pagar e receber** na tela principal ou escolha uma ficha no catálogo.
2. Selecione relatório, período e empresa. Marque **Consolidado** para consultar todas as empresas quando o relatório nativo permitir.
3. Nos relatórios com data selecionável, escolha inclusão, vencimento, quitação ou emissão. As opções variam por entrada. “Atrasadas” e “a vencer” usam a data atual do servidor, independentemente do período consultado.
4. Em **Filtros adicionais**, escolha conta, pessoa, segmentação ou operação quando disponíveis. O relatório por vendedor exige a seleção do vendedor; a pesquisa de pessoas retorna somente nome e referência, sem exportar documentos ou contatos.
5. Consulte. Os indicadores e agrupamentos abrangem todo o resultado filtrado, mesmo ao paginar. O CSV exporta somente a página atual.

O relatório por competência é global no código nativo: não recebe empresa, conta ou pessoa. A interface mostra isso explicitamente e a API rejeita esses filtros, em vez de ignorá-los. O relatório por vendedor exige uma empresa e não oferece consolidação.

## Regras recuperadas do snapshot instalado

O rastreamento est?tico preservado cobre 19 tipos em cinco DLLs com dnSpy.Console e o parser gerenciado do REA, sem executar o SuiteG6. O [manifesto](finance-artifacts-1.0.3-rc.1.json) identifica cada DLL, tipo, arquivo descompilado, comando e Evidence ID. O REA devolveu metadata completa para cinco tipos. Nos outros 14 tipos, `Domain.Main` excedeu o orçamento de representação de metadata de 64 MiB; esses resultados são **unavailable**, não confirmações de membros. As regras desses tipos vêm da reconstrução separada do dnSpy vinculada ao hash do DLL instalado, com CIL do dnSpy consultado para resolver parâmetros ofuscados do cálculo pro rata. As fontes completas e os registros REA permanecem na pasta local `engenharia/assemblies-descompilados/installed-finance-1.0.3/`.

| Camada | Regra utilizada |
|---|---|
| `PagamentoRecebimentoRelatorioAppService` | Seleciona coleção, empresa/consolidação, data e situação; recebe pessoa, conta, plano, centro de custo ou segmentação conforme a entrada. |
| `ParcelaPagamentoSpecifications` / `ParcelaRecebimentoSpecifications` | Inclusão usa ObjectId; emissão, vencimento e quitação usam as respectivas propriedades. Plano e centro de custo podem corresponder a **qualquer** histórico, inclusive históricos diferentes. |
| `ParcelaSpecification<T>` | Usa parcelas ativas; sem consolidação filtra `EmpresaReferencia`. |
| `ParcelaRepositoryRead<T>.Preencher` | Resolve pessoa, empresas e contas pelos identificadores; a empresa e a conta apresentadas vêm do último histórico. |
| `Parcela.Valor` | Quitada: último `HistoricoPendente.Valor`. Demais situações: valor do último histórico. Não é a soma dos pagamentos. |
| `Parcela.ValorQuitacao` e `Desconto` | Somente quando a situação é quitada; valores do último histórico de quitação, incluindo subclasses. |
| `PagamentoRecebimentoProjection` e `RecebimentoProjection` | Quando o último histórico é exatamente do tipo/herança `HistoricoQuitadoParcial`, Valor e ValorCorrigido usam ValorQuitacao. A situação exibida passa a “Quitada parcial”. |
| `ParcelaRecebimento` | Acrescenta juros e multa ao principal somente enquanto a situação é pendente. |
| `ParcelaRecebimentoCartao` / `ParcelaRecebimentoCarteiraDigital` | Sobrescrevem ValorTotal para **Valor − ValorTaxa**. Taxa percentual é arredondada a duas casas; taxa fixa só é aplicada quando o principal é estritamente maior que a taxa. |
| `CalculoProRataDieService` | Desloca ambas as datas em um dia, usa frações dos meses inicial/final e a conversão intermediária 365/30/28 observada no código; arredonda o período a quatro casas. Não usa simplesmente dias/30. |
| `JuroSimples`, `JuroComposto`, `Multa` | Carência exige vencimento estritamente anterior à data-base menos a carência. Juros simples usam principal × percentual/100 × período; compostos usam potência em double convertida para decimal. Multa é percentual único. Arredondamento monetário AwayFromZero. |
| `DateTimeSerializerEx` | Serializa os ticks como UTC sem converter o relógio e devolve DateTime Unspecified. As datas gravadas são comparadas sem deslocamento de fuso. ObjectId representa um instante real e inclusão usa o fuso local do servidor. |

### Diferenças entre relatórios

- **Em aberto:** período por vencimento, situação pendente; a espécie é aplicada depois da projeção. O valor 99 significa todas.
- **Quitados:** período por data de quitação e situação quitada.
- **Quitados por competência:** basta que **qualquer histórico** tenha `Data` no período; a parcela precisa estar quitada. O histórico correspondente ao período não precisa ser o da quitação. O serviço nativo consulta todas as empresas.
- **Por pessoa / segmentação:** filtros opcionais de plano e centro de custo. Recebimentos também aceitam conta; pagamentos por pessoa não aceitam conta. Selecionar data de quitação já exige situação quitada, mesmo se outro status incompatível for informado.
- **Por vendedor:** consulta clientes vinculados ao vendedor no cadastro atual, exclui Consumidor e não exige que o cliente esteja ativo. Isso não equivale a procurar vendedores nos itens de uma venda.
- **Cartões / taxas:** exige `ParcelaRecebimentoCartao`, sem filtro de situação; permite inclusão, vencimento ou emissão. A operação vem do tipo da taxa de rede (Débito/Débito parcelado ou Crédito); sem taxa, usa o tipo de cartão do último histórico. A opção de remover taxa zero conserva somente taxas **maiores que zero**.

## API e limites

- `GET /api/finance/reports`: operações e filtros permitidos, sem consultar o banco.
- `GET /api/finance/options`: empresas, contas e tipos de segmentação da conexão atual.
- `GET /api/finance/people?q=...`: busca literal de 2–100 caracteres, até 50 pessoas por busca.
- `POST /api/finance/query`: plano validado com `report_id`, `start`, `end`, empresa/consolidação e filtros específicos. Não aceita consulta Mongo ou código arbitrário.

As APIs usam a mesma autenticação HTTP e conexão de relatórios do Insight. Não executam os serviços WCF nem a identidade/permissão de usuário do SuiteG6. A autorização efetiva de leitura é a configurada para o Insight/MongoDB.

A consulta aceita até 50.000 parcelas, prazo total de 20 segundos, página de até 200 linhas e período de até dez anos. Não retorna totais parciais ao atingir um limite ou encontrar valores inválidos. O limite do nativo de pagamentos em aberto é verificado antes dos filtros de período/situação; no Insight é verificado depois desses filtros e antes do filtro posterior de espécie. Essa diferença de limite é deliberada e não constitui equivalência integral de comportamento.

Valores monetários são devolvidos como strings decimais e somados no backend com Decimal. A interface preserva os centavos sem conversão dos inteiros para Number. As páginas consultam a base novamente; uma alteração concorrente pode mudar linhas e totais.

A tabela contém o núcleo financeiro: empresa, pessoa, documento, parcela, datas, espécie, situação, valores, juros, multa, descontos, taxas, conta, rede e operação. Colunas cadastrais adicionais do layout nativo, telefone/endereço, NossoNúmero, sub-relatórios e ordenações específicas de todas as versões ainda não são reproduzidos. Os agrupamentos da tabela são uma visão de dados; não certificam as bandas de impressão do DevExpress.

## Verificação

Os testes incluem 432 vetores de período, juros simples e compostos contra uma reconstrução C# independente, testes de estados de histórico e testes reais com MongoDB 3.4 em uma instância temporária isolada. Nenhuma escrita de teste é feita na conexão configurada do cliente. Os dados temporários incluem cliente inativo, consumidor, empresas com nomes iguais, cartão com/sem taxa, carteira digital, segmentação e vendedor cadastral. Os 21 cenários também passaram na [verificação somente leitura da base atual](finance-verification-1.0.3-rc.1.json), com predicados independentes, contagens, principal, valor quitado, desconto e taxa. A base atual não possui segmentações preenchidas; esse vínculo foi verificado na base temporária.

Comandos:

```powershell
.\.venv\Scripts\python.exe -m unittest discover -s tests -p 'test_finance*.py' -v
powershell -ExecutionPolicy Bypass -File tools\finance-calculation-oracle.ps1
.\.venv\Scripts\python.exe tools\trace-installed-finance.py --rea --reuse
.\.venv\Scripts\python.exe tools\trace-installed-finance.py --check
.\.venv\Scripts\python.exe tools\verify-finance.py
```

O script C# compila somente as fórmulas reconstruídas contidas nele; não carrega DLLs do SuiteG6. A concordância dos vetores valida a tradução das fórmulas recuperadas, e não uma observação de execução do relatório nativo.
