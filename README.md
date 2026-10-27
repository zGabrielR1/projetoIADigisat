# DigiSat Insight

## Espaço de trabalho 1.0.1-rc.1 (release candidate)

1.0.1 RC1 adiciona consulta de validade de lotes por empresa, quantidades físicas reconstruídas dos movimentos, busca, paginação e exportação CSV/Excel da página exibida. Mantém a busca e renomeação de investigações salvas da RC3. Veja [novidades e limites](docs/release-1.0.1-rc.1.md).

Conversas com contexto e esclarecimento, comparações, perfis de cálculo, investigação paginada, qualidade dos dados, cópias salvas, evidências Excel/PDF/CSV, assistente de conexão, diagnósticos e agendamentos locais. Veja [guia de uso](docs/guia-1.0.md) e [validação e compilação](docs/validacao-1.0.md). A documentação abaixo preserva o histórico técnico dos relatórios anteriores.

Painel web interno para consultas de leitura, relatórios controlados e interpretação de perguntas. O projeto é dividido em duas frentes: **Insight** (consultas) e **engenharia reversa documentada** (mapa de como os relatórios do Suite G6 são montados). O guia da segunda frente está em [docs/engenharia-reversa-relatorios.md](docs/engenharia-reversa-relatorios.md).

## O que já está pronto

- Sete relatórios: faturamento, produtos, série diária, empresa, documentos, formas de pagamento e ticket médio.
- Ticket médio: total dos itens dividido pelos documentos de cada dia, com ticket do período. O nativo `TicketMedioPorOrigemETipo` também separa por empresa e tipo; o painel usa a empresa como filtro e o tipo como escopo, e informa essa diferença. Detalhes em [docs/continuacao-ticket-medio.md](docs/continuacao-ticket-medio.md).
- Formas de pagamento: agrupa `ItensPagamentos[].EspeciePagamento` (ou `Pagamentos[]` nos registros antigos) por código e descrição, com total, lançamentos e participação. Cartão, carteira digital e terminal POS entram pela espécie; o relatório nativo `TotalizacaoPorEspecie` detalha cada transação TEF, e o painel informa essa diferença no resultado. Evidência e limites em [docs/continuacao-formas-pagamento.md](docs/continuacao-formas-pagamento.md).
- Interface em português com períodos rápidos (hoje, ontem, 7 dias, mês, mês passado, ano), filtros por coleção/campos e execução automática ao trocar o relatório.
- Perguntas em português: classificador local por regras e, opcionalmente, um modelo compatível com OpenAI que escolhe apenas um tipo de relatório permitido. O modelo não recebe dados da base e não cria comandos Mongo/SQL.
- A pergunta também define o período ("últimos 30 dias", "semana passada", "em setembro", "01/09 a 15/09") e a empresa citada, quando o campo de empresa está mapeado.
- Escopo de cálculo explícito: **vendas (fiscal + registros)**, **vendas fiscais**, **somente NFC-e**, **somente NF-e de saída**, **somente notas manuais**, **somente registros de venda**, **compras e entradas** ou **tudo**. O filtro de **documentos válidos** exclui cancelados, rejeitados e inutilizados, e o painel informa quantos documentos ficaram fora.
- Total dos itens reproduzido com a mesma fórmula do DigiSat (bruto arredondado, descontos, despesas, IPI/ST, frete e seguro), inclusive quando o campo nativo não existe, com conferência pelo total de pagamentos registrados e drill-down documento a documento. A fórmula e o arredondamento `AwayFromZero` foram reconferidos no build instalado 2.0.26293.5 e auditados em 152.034 itens sem divergência.
- Tabela ordenável, indicadores, gráficos e exportação CSV com cabeçalhos em português.
- Suíte E2E com uvicorn real e cálculo independente no MongoDB: cobre os sete relatórios, todos os parâmetros, `/api/ask`, `/api/reconciliation`, autenticação opcional e a CLI de diagnóstico. Ver [docs/testes-e2e-relatorios.md](docs/testes-e2e-relatorios.md).
- Interface sem CDN, fontes externas ou etapa de build: funciona offline na rede interna.
- Descoberta amostral dos campos de uma coleção e sugestão automática de data, valor, produto e empresa.
- MongoDB somente leitura, limite de linhas, `maxTimeMS` e sem `allowDiskUse`.
- Identificação do ZIP de backup mais recente e script para restaurá-lo em uma instância Mongo isolada em loopback.
- Proteção HTTP Basic opcional na API. Para bind fora do localhost, usuário e senha são obrigatórios.

## Usar o painel

1. Abra `http://127.0.0.1:8088` e confira o indicador de conexão no topo.
2. Pergunte em linguagem natural ou escolha um dos cinco relatórios.
3. Ajuste o período pelos atalhos ou pelas datas. A troca de período ou de relatório executa a consulta.
4. Abra **Filtros avançados** para trocar coleção, campos, empresa ou **escopo de movimentação**. O mapeamento inicial é sugerido a partir do esquema; o escopo aparece como um chip no resultado.
5. No faturamento, clique em **Detalhar diferenças** para comparar itens e pagamentos documento a documento, ver os documentos sem pagamento e o resumo por tipo.
6. Exporte em CSV quando precisar levar a tabela principal para o Excel.

O painel não roda a consulta a cada tecla: a pergunta é enviada ao clicar em **Gerar relatório** (ou Enter).

## Banco DigiSat encontrado nesta máquina

A pasta `C:\Digisat\SuiteG6\Dados` contém arquivos WiredTiger; a configuração `C:\Digisat\SuiteG6\MongoDB\mongod.conf` aponta o MongoDB para essa pasta, porta `12220` e autenticação ativada. A versão local é MongoDB `3.4.7`. Com o usuário fornecido, confirmei somente leitura que há bancos `DigisatClient` e `DigisatServer`; o `.env.example` seleciona o banco do servidor. Na auditoria de 21/10/2026, `Movimentacoes` tinha 525 documentos (emissão de 27/04/2026 a 15/10/2026) e 2.226 itens; `_t` é um array e o tipo efetivo é o último elemento. Índices observados: `DataHoraEmissao`, `EmpresaReferencia`, `Situacao.Codigo`, `EmpresaReferencia + _t + DataHoraEmissao` e `EmpresaReferencia + Situacao.Codigo + DataHoraEmissao`. `ProdutosServicos` tinha 2.844 documentos. Também examinei nomes, tipos, índices e contagens agregadas, sem exportar documentos ou dados pessoais. O driver está fixado em PyMongo 3.13 porque a linha 4 remove suporte a servidor MongoDB 3.4. O backup mais recente visto durante a implementação foi `DigisatServer20261020_080100.zip`; seu conteúdo inclui pares `.bson.gz` / `.metadata.json.gz`, portanto é um dump comprimido, não um diretório WiredTiger que possa ser aberto pelo painel.

O painel conecta somente em `127.0.0.1` por padrão. O backend desta versão oferece apenas descoberta de esquema e agregações/leituras, sem rotas de escrita. A credencial fornecida é do usuário `root`, que tem privilégios mais amplos no MongoDB; para uma instalação compartilhada, substitua-a por uma conta com role `read` nos bancos necessários. O painel não altera coleções nem tenta contornar autenticação. A senha fica em arquivo local ignorado pelo Git e é carregada em memória; nunca a copie para o `.env`.

## Iniciar no Windows

1. Instale Python 3.11+ e copie `.env.example` para `.env` (o `start.ps1` faz isso na primeira inicialização).
2. Confira `MONGO_DATABASE`, `MONGO_USERNAME`, `MONGO_AUTH_SOURCE` e `MONGO_PASSWORD_FILE`. O padrão aponta para `DigisatServer`, usuário `root` e para `senhamongo.txt` no diretório do projeto. Esse arquivo pode conter apenas a senha em uma linha ou um rótulo com “senha” seguido da senha na linha seguinte. Não inclua o segredo no `.env` nem o compartilhe.
3. Execute `powershell -ExecutionPolicy Bypass -File .\start.ps1`. Na primeira execução o script cria `.venv` e instala os pacotes listados em `requirements.txt`.
4. Abra `http://127.0.0.1:8088`.

O primeiro acesso à API informa os bancos que a conta consegue enxergar. Coloque o banco desejado em `MONGO_DATABASE` e reinicie. Campos são amostrados em até 30 documentos. Campos dentro de arrays aparecem com `[]` e são desenrolados para agregações por item. Confira o mapeamento antes de interpretar totais: nomes e formatos variam por coleção e versão.

### Conectar à base ativa

O padrão é `127.0.0.1:12220`, com `readPreference=secondaryPreferred`. Exemplo de URI (substitua valores localmente):

```text
mongodb://USUARIO:SENHA@127.0.0.1:12220/?authSource=admin&directConnection=true&readPreference=secondaryPreferred
```

Solicite ao administrador do MongoDB uma conta de consulta com `read` no banco correto. Como orientação, um administrador autorizado pode criar uma conta com `db.createUser({user: "insight", pwd: "...", roles: [{role: "read", db: "NOME_DO_BANCO"}]})` no banco de autenticação apropriado à configuração da empresa. Não execute isso com uma senha inventada ou na base sem aprovação do DBA.

### Usar uma cópia do backup

1. Execute `powershell -ExecutionPolicy Bypass -File .\restore-latest-backup.ps1`. O script seleciona o ZIP mais recente, valida os caminhos internos, extrai em uma pasta de trabalho nova e inicia um `mongod` separado na porta `12231`, vinculado somente a `127.0.0.1`.
2. O dump é restaurado no banco local `InsightBackup`. Nenhum arquivo é copiado para `C:\Digisat\SuiteG6\Dados` e nenhum registro da base ativa é alterado.
3. Em `.env`, configure `DIGISAT_MODE=backup` e `MONGO_DATABASE=InsightBackup`; reinicie o painel.

A instância de backup fica ativa após o script terminar para que o painel possa usá-la. A pasta de restauração fica em `%LOCALAPPDATA%\DigiSatInsight\backups\<data-hora>`. Pare o processo Mongo de backup e remova uma pasta somente quando não precisar mais dela. A restauração requer `mongorestore.exe` da mesma instalação local; use uma cópia isolada e valide antes de depender dos números.

## Acesso na rede interna

Por padrão o serviço escuta somente localhost. Para acesso remoto, configure `INSIGHT_USER` e `INSIGHT_PASSWORD` no `.env`, defina `$env:INSIGHT_HOST = '0.0.0.0'` na sessão do PowerShell antes de executar `start.ps1` e publique por um proxy interno com HTTPS e controle de acesso corporativo. Não exponha a porta Uvicorn diretamente na rede: Basic Auth sem TLS não protege a senha em trânsito. Mantenha firewall restrito à LAN.

## IA, escopo e limites

Sem `AI_ENDPOINT` e `AI_MODEL`, o texto é classificado por regras locais de palavras-chave. Atualmente ele escolhe entre sete operações: faturamento, produtos, documentos, série diária, empresas, formas de pagamento ou ticket médio. O período e a empresa mencionados na pergunta são extraídos por regras locais de data em português; a IA não participa dessa extração. Com um endpoint compatível configurado, o serviço envia apenas a pergunta digitada e pede ao modelo um rótulo entre as mesmas sete operações fixas. O backend valida o rótulo e constrói o pipeline com campos escolhidos no catálogo do Mongo; nenhuma pergunta vira código executável. O resultado numérico sempre é calculado no backend.

O escopo padrão da interface é **vendas (fiscal + registros)** e **documentos válidos**. O primeiro inclui NFC-e, NF-e de saída, notas manuais e registros de venda, mas exclui compras, pré-vendas, pedidos e documentos auxiliares de venda. Também é possível escolher **vendas fiscais** (sem os registros antigos de PDV), um tipo específico (NFC-e, NF-e de saída, nota manual ou registro de venda), **compras e entradas** ou **tudo**. O filtro de documentos válidos remove situações `Cancelado`, `Rejeitado` e `Inutilizado` quando a coleção possui `Situacao.Descricao`; o escopo de movimentação depende de `_t` e fica desativado sem ele. O número de documentos excluídos aparece no resultado.

No relatório de faturamento, o botão **Detalhar diferenças** consulta `/api/reconciliation` e mostra: total dos itens, total dos pagamentos, diferença e percentual, documentos com e sem pagamento, resumo por tipo de movimentação e os documentos com maior desvio. A conferência usa a mesma fórmula dos itens do faturamento e lê `ItensPagamentos[].Valor` ou, por documento, `Pagamentos[].Valor` quando o primeiro não existe. Na auditoria, `ItensPagamentos` estava em 454 documentos e `Pagamentos` em 8, sem documento com os dois arrays preenchidos. Isso permite separar, por exemplo, uma nota manual sem pagamento registrado de uma NFC-e conciliada.

Os relatórios usam agregações genéricas sobre uma coleção configurada e campos selecionados. O campo de data da coleção `Movimentacoes` é `DataHoraEmissao`; produtos aparecem nos itens embutidos em `ItensBase[].ProdutoServico.Descricao`. Na auditoria de 21/10/2026, `ItensBase[].TotalLiquido` e `TotalBruto` existiam em 963 dos 2.226 itens (os dois tipos “Digitado”); os 1.074 itens de NFC-e e os demais itens fiscais recentes não tinham nenhum dos dois (1.263 itens). Por isso, o valor sugerido é **Valor dos itens (calculado)** com a mesma fórmula usada pelo DigiSat em `MovimentacaoSaidaAppService`: `round2(preço × quantidade) − DescontoDigitado − DescontoProporcional + OutrasDespesasProporcional + OutrasDespesasDigitado + Percentual.ValorSt + IPI.Valor + Frete + Seguro`. A fórmula foi conferida contra os 963 itens que ainda têm `TotalLiquido` e contra os totais de 24 documentos com todos os itens preenchidos, sem divergência acima de R$ 0,01; no período de outubro/2026 o total calculado também bateu com `ItensPagamentos[].Valor`. Campos fiscais ausentes entram como zero. Para faturamento, o painel mostra também o total efetivamente registrado em `ItensPagamentos[].Valor` (ou `Pagamentos[].Valor` nos registros antigos de venda). Quando há diferença, o aviso de conferência e o drill-down indicam os documentos responsáveis, separam os que não têm pagamento registrado e resumem o desvio por tipo de movimentação; isso pode apontar notas manuais, devoluções ou outras movimentações fora do padrão. Em setembro/2026, por exemplo, a diferença de R$ 419,60 (2,67%) ficou concentrada em 2 notas manuais e 1 NF-e de saída sem pagamento; em outubro/2026, itens e pagamentos fecharam em R$ 8.417,31. O painel ainda não reproduz automaticamente todas as especificações, tipos de movimentação, regras fiscais, cancelamentos e condições do relatório nativo. Antes de usar valores como faturamento oficial, mapeie a coleção `Movimentacoes` e compare com o relatório nativo no mesmo período. A base ativa consultada tinha 525 documentos na auditoria, ainda uma amostra pequena para 50 mil documentos mensais; esse volume não foi medido, e desempenho real depende de volume, índices, máquina, topologia, rede e formato dos documentos.

## Estrutura

- `app.py`: API de leitura, descoberta de esquema, classificação e agregações permitidas.
- `static/`: interface responsiva, visualização e exportação.
- `restore-latest-backup.ps1`: restauração local isolada do backup mais recente.
- `docs/engenharia-reversa-relatorios.md`: fluxo geral de execução e rastreamentos funcionais já aprofundados.
- `docs/catalogo-relatorios.md`: inventário dos relatórios registrados, módulos, chamadas de serviço encontradas e estado da cobertura por entrada.
- `docs/continuacao-formas-pagamento.md`: regra nativa de `TotalizacaoPorEspecie`, evidência do assembly instalado e limites da visão de pagamentos.
- `docs/continuacao-formula-faturamento.md`: fórmula do total do item, arredondamento `AwayFromZero`, suporte do MongoDB 3.4 e auditoria item a item.
- `docs/continuacao-ticket-medio.md`: regra nativa de `TicketMedioPorOrigemETipo` e a visão diária do painel.
- `docs/continuacao-modulos-restantes.md`: as 19 últimas entradas do catálogo, com 17 rastreadas e 2 ausentes dos assemblies instalados.
- `docs/diagnostico-inconsistencia-relatorios.md`: diagnóstico da base do cliente para vendas por mês, vendas por CPF e a diferença de 05/10.
- `docs/testes-e2e-relatorios.md`: referência de cada parâmetro da API e da CLI e o que a suíte E2E verifica.
- `docs/resposta-email-inconsistencia-relatorios.md`: achados consolidados (outubro, geração de 23/10, CPF x vendedor e 05/10).
- `docs/email-resposta-inconsistencia-relatorios.txt`: e-mail pronto para envio, em texto puro.
- `docs/evidencias-formas-pagamento-reconstruction.json`: registro da importação REA da descompilação do build instalado, com hashes do artefato, do método e do texto.
- `docs/evidencias-formula-faturamento-reconstruction.json` e `docs/evidencias-arredondamento-reconstruction.json`: o mesmo registro para a fórmula do item e para o arredondamento.
- `engenharia/assemblies-descompilados/2.0.26293.5/`: descompilações do build instalado usadas como evidência, separadas da extração 2.0.26273.6.
- `test_payments_report.py`: testes offline do resumo de espécies, da classificação e da validação do tipo.
- `test_revenue_formula.py`: testes offline da expressão de arredondamento compatível com 3.4 e do conjunto de campos da fórmula.
- `test_ticket_report.py`: testes offline do resumo diário do ticket, da expressão de total por documento e da classificação.
- `engenharia/ferramentas/diagnostico_relatorios.py`: ferramenta somente leitura que reproduz o mensal por vendedor e os documentos compartilhados de um dia.
- `test_e2e_reports.py`: suíte ponta a ponta da API e da CLI, com servidor uvicorn, cálculo independente no Mongo e testes de documentação dos parâmetros.
- `engenharia/codigo-descompilado/`: C# reconstruído dos assemblies instalados para consulta, com um `LEIA-ME.md` indicando a origem e as limitações.
- `engenharia/assemblies-descompilados/2.0.26273.6/`: extrações adicionais das DLLs instaladas de relatórios, serviços WCF, aplicação, projeções e DTOs usadas para fechar caminhos não presentes na primeira extração.
