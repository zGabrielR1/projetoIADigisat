# Business intelligence

Abra `/bi` ou **Business intelligence** no menu do DigiSat Insight. O módulo usa a conexão MongoDB já configurada e os cálculos do motor de relatórios existente. As consultas não alteram a base DigiSat. Os modelos são gravados apenas no SQLite local do Insight.

## Painel de vendas

Revise o mapeamento sugerido e clique em **Atualizar análise**. Escolha o período, a empresa e o escopo; por padrão, entram vendas fiscais e registros de venda, excluindo cancelados, rejeitados e inutilizados quando esses campos existem. O período pode ter de 1 a 366 dias.

O painel apresenta faturamento, documentos, ticket médio, pagamentos registrados, evolução diária, empresas e produtos. A comparação usa o período imediatamente anterior com a mesma quantidade de dias, preservando empresa e escopo. Quando a base anterior é zero, a variação percentual fica indefinida e o painel mostra a variação absoluta. Um pagamento não disponível não é exibido como zero.

O faturamento principal considera todo o escopo. Os rankings consultam até 8 grupos e mostram os 5 primeiros; os demais ficam disponíveis em **Explorar**. Participações usam o faturamento principal como denominador. A série é diária, sem interpolação suavizada. Dias sem movimento entram com zero apenas quando a série completa foi carregada. Limites e erros parciais são exibidos.

**Pagamentos registrados** é uma conferência dos arrays de pagamento do movimento; não é um extrato bancário nem um relatório de contas a receber. Diferenças entre itens e pagamentos devem ser investigadas antes do fechamento. Os blocos são consultas independentes, não uma transação de leitura: a base pode mudar durante sua execução.

## Exploração e drill-down

- Selecione uma empresa no ranking para abrir sua evolução diária.
- Selecione um dia na tabela para ver os documentos dessa empresa e data.
- Use o caminho acima da tabela para voltar ao período ou à empresa.
- As datas de maior faturamento no gráfico também abrem seus documentos.
- Agrupe por dia, empresa ou produto, ou liste os documentos. Pesquise nas linhas carregadas e ordene pelos cabeçalhos.
- **Exportar CSV** exporta o resumo ou as linhas visíveis após a pesquisa, com datas e limites indicados na tela. Strings com prefixos de fórmula são neutralizadas para abertura em planilhas.

O detalhe de produto apresenta valor e linhas de itens do grupo. Não filtra documentos por produto e não trata número de linhas como quantidade vendida. A navegação empresa → dia → documentos preserva os filtros no servidor. A pesquisa de texto opera somente sobre o resultado carregado, e não sobre toda a base.

### Visões salvas e links

Use **Salvar visão** para guardar nome, mapeamento, filtros, medida do gráfico e, na exploração, agrupamento, limite, pesquisa, ordenação, empresa e dia detalhados. A seleção abaixo do título reabre a configuração e executa uma nova consulta; não guarda uma fotografia dos resultados.

Para este mês, mês passado ou últimos 7/30 dias, **Acompanhar o período automaticamente** recalcula as datas ao abrir a visão. Desmarque para fixar o intervalo. Um dia detalhado sempre usa datas fixas, para evitar que uma data antiga seja reinterpretada como parte de um período móvel.

**Copiar link** abre a visão salva nesta mesma instalação do Insight, com a autenticação normal. Alterações não salvas desabilitam o link até salvar uma nova visão ou restaurar a configuração original. Em **Gerenciar visões**, abrir ou excluir uma configuração não altera modelos nem a base DigiSat. Cada visão mantém uma cópia do mapeamento e continua utilizável se o modelo nomeado original for excluído. A origem permanece vinculada à conexão em que foi salva.

O gráfico agora desenha o período anterior com linha cinza tracejada. As séries são alinhadas pela posição do dia nos intervalos, não pelo dia da semana; a legenda informa o intervalo anterior e o tooltip mostra a data real de cada série. Dias não carregados em séries limitadas ficam como lacunas, sem zeros inventados. Uma falha na série anterior não elimina uma comparação de KPIs que tenha sido concluída.

O rodapé da exploração soma somente as linhas exibidas após a pesquisa, usando os centavos efetivamente mostrados na tabela. Não representa o total da base quando há filtros de texto ou limites. Para valores de itens embutidos, o detalhe de documento apresenta a soma do campo mapeado, em vez de uma lista de valores. Atualizar a mesma exploração preserva a pesquisa e a ordenação.

## Fontes e modelo

Escolha a coleção e mapeie data, valor, empresa e produto. O esquema é observado em uma amostra de até 30 documentos; campos raros podem não aparecer. Data deve ser um campo de data e valor deve ser numérico ou `@item_total`, calculado pela fórmula existente do DigiSat.

A data e a empresa devem estar no nível do documento. Produto e valor em arrays devem compartilhar a mesma lista, com no máximo um nível de expansão; repetir o valor total de um documento para cada produto é rejeitado. As medidas são fixas, sem execução de fórmulas livres. Não há junções entre coleções, ingestão de CSV/Excel, ETL, conexão a outras plataformas ou agendamento específico de atualização BI nesta versão.

**Usar neste painel** aplica o mapeamento à sessão. **Salvar modelo** o disponibiliza na seleção de modelos. O modelo salvo fica vinculado ao banco, ao modo de conexão e a uma impressão digital da conexão; uma troca de origem exige escolher ou criar um modelo compatível. A URI e as credenciais não são armazenadas no modelo. Excluir um modelo remove apenas a configuração local.

## API

| Endpoint | Uso |
| --- | --- |
| `GET /api/bi/models` | Modelos locais e disponibilidade na conexão atual |
| `POST /api/bi/models` | Salvar nome e mapeamento após validar o esquema |
| `DELETE /api/bi/models/{id}` | Excluir um modelo local |
| `POST /api/bi/overview` | Painel consolidado e comparação |
| `POST /api/bi/explore` | Agrupamento ou documentos com o recorte selecionado |
| `GET /api/bi/views` | Visões locais e disponibilidade na conexão atual |
| `POST /api/bi/views` | Salvar configuração e caminho de exploração |
| `GET /api/bi/views/{id}` | Abrir configuração com datas relativas resolvidas |
| `DELETE /api/bi/views/{id}` | Excluir somente a configuração local |

As rotas seguem a autenticação já configurada para `/api/`. Overview e explore recebem `mapping` ou `model_id`, datas, empresa opcional, escopo, validade e comparação; explore acrescenta dimensão e limite de 1 a 500 linhas, reduzido ao limite global quando necessário. O resumo não soma rankings truncados para produzir os KPIs.

## Validação e desenho

Taste: direção **Data terminal**, seed `621892274`. Mantidos Inter, o shell identificado e a paleta Pulse aprovada pelo usuário, incluindo o azul/índigo e os gradientes da navegação; esses requisitos explícitos prevalecem sobre as proibições genéricas de paleta e gradiente da skill. Dentro do BI, números são tabulares, KPIs compartilham um único contêiner e seleções de abas usam uma linha de acento. O elemento próprio do produto é o caminho do documento DigiSat: empresa → emissão → nota, ligado ao mapeamento da coleção.

Perguntas das telas: **Visão geral:** o que explica o resultado das vendas? **Explorar dados:** quais empresas, dias e documentos o compõem? **Fontes e modelo:** quais campos e regras produziram essas medidas?

Testes offline: `python -m unittest discover -s tests -p test_bi.py`. A revisão visual usa desktop, tablet e celular, modos claro/escuro e estados sem consulta, carregado e erro. Os dados de teste unitário são sintéticos; nenhum dado fictício está embutido no produto.

Validação desta implementação: 14 testes de BI, 8 testes de estado local, 6 testes do catálogo e o teste Node de exportação do editor passaram. Na base configurada, foram exercitados painel, empresa → dia → documentos, pesquisa, salvamento e reuso do modelo `Vendas por emissão · BI` e download do CSV com fonte, escopo e datas. Uma falha de atualização simulada preservou os resultados anteriores e exibiu o erro. As três áreas de BI foram verificadas em 1440, 768 e 390 pixels, nos dois temas, sem overflow horizontal nem erros JavaScript. O inspector Taste do overview terminou com **0 FAIL / 0 WARN**, com as exceções de marca explicitadas acima. O lint retornou três avisos justificáveis: sombra em dialog flutuante, preenchimento de zeros em datas e travessões usados como valores ausentes.

Revisão visual final: a pergunta principal é respondida pelo faturamento e pela concentração diária calculada; painéis seguem as mesmas bordas; cor destaca a série e a ação principal; medidas e exportação derivam da mesma resposta; zero e ausência são distintos; o gráfico usa datas e escala explícitas; no celular os filtros recolhem e os destinos adicionais abrem em um menu. O escopo que ainda fica de fora é integração multiorigem/ETL e modelagem com joins ou medidas livres.

Incremento de visões salvas: 23 testes Python de BI e 5 testes Node de apresentação passaram, incluindo períodos móveis/ano bissexto, série anterior, lacunas, cópia de mapeamento independente do modelo e soma de centavos exibidos. Verificadas na base configurada a reabertura por link, restauração de pesquisa/ordenação/detalhamento e atualização sem perder esses controles. As 18 combinações de área, tema e largura foram verificadas sem overflow ou erros JavaScript. A inspeção Taste do overview foi repetida e terminou em 0 FAIL / 0 WARN; a revisão corrigiu a compressão do cabeçalho em tablet e um espaço vazio excessivo no gráfico antes da primeira consulta. A visão de exemplo `Vendas · mês atual` fica disponível; as configurações temporárias de QA foram excluídas.
