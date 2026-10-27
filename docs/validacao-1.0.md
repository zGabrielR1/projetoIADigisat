# Validação e critérios da 1.0

Implementação: `insight_plans.py`, `insight_conversation.py`, `insight_engine.py`, `insight_store.py`, `insight_connection.py`, `insight_exports.py`, `insight_schedule.py`, `insight_workspace.py` e `static/workspace.*`. APIs novas ficam em `/api/workspace`; as APIs anteriores permanecem disponíveis. A autenticação existente protege também as rotas novas. Todas as operações no DigiSat continuam de leitura; persistência própria usa SQLite local.

## Cobertura repetível

- `test_workspace.py`: benchmark de perguntas em português; refinamento contextual; esclarecimentos sem consulta; datas inválidas; tipagem contra injeção; comparação mantendo vendedor/empresa; fixtures financeiras; banco local e backup; contas amplas e somente leitura; preservação de configuração/chaves; paginação e busca; cópias salvas; perfis; qualidade; exportações PDF/Excel/CSV; agendamentos e deduplicação.
- `tests/fixtures/question-benchmark.json`: casos de classificação, período, vendedor, escopo, situação, comparação, pedidos desconhecidos e perguntas incompletas.
- `tests/fixtures/synthetic-sales.json`: vendas compartilhadas, itens cancelados, descontos/despesas/impostos, arredondamento negativo, venda sem pagamento, vendedor ausente e item gratuito. Não contém dados de clientes.
- Suítes existentes: testes dos sete relatórios, classificação, filtros, cálculos e HTTP real, autenticação e ferramenta de diagnóstico.

Executar `python -m unittest test_workspace test_question_investigation test_company_interpretation test_payments_report test_revenue_formula test_ticket_report` e `python test_e2e_reports.py -v` no ambiente do projeto. Os testes integrados exigem acesso de leitura à base local e são identificados como dependentes dessa base; as fixtures e o benchmark não dependem do conteúdo do cliente.

Para desenvolvimento, instalar `requirements-dev.txt`; os leitores/renderizadores de PDF são dependências de validação, não exigências de instalação para o usuário do executável.

## Critérios de homologação

- Sem falhas nas suítes pertinentes; cálculos nativos conhecidos reproduzidos com filtros iguais.
- Conversa e esclarecimento exercitados pela interface, incluindo recarga e retomada.
- PDF inspecionado visualmente e Excel reaberto com conferência dos valores.
- Executável exercitado fora da árvore do código-fonte, com PATH sem Python; imports, SQLite, exportações, porta ocupada e credenciais testados.
- ZIP conferido por lista explícita de arquivos, sem senhas, base local, fixtures, testes, agentes ou engenharia. Manifesto SHA-256 corresponde ao executável testado.
- **Gate externo:** homologação em Windows limpo e numa base representativa com suporte/usuários reais. Não pode ser substituída por testes nesta única máquina.
- **Gate de volume:** medir consultas usuais e investigações com o volume real esperado. Metas são 5 s e 20 s, respectivamente; timeout não comprova desempenho para qualquer consulta. Não alterar índices do cliente automaticamente.

O artefato atual é identificado como `1.0.1-rc.2` até a homologação externa. Não declarar “zero bugs” nem compatibilidade integral com todos os relatórios do Suite G6.

`test_editor.py` cobre o novo módulo de administração, incluindo CRUD real em instância MongoDB isolada, sessão/permissões, parser SELECT, BSON, conflitos, repetição de requisição, histórico e cópias de recuperação. A verificação do PE exige os gates `database_editor_write_crud`, `database_editor_query_modes` e `database_editor_history`, obtidos executando a suíte com o candidato em um servidor de teste próprio. O relatório de verificação contra a base DigiSat continua usando somente leituras; as escritas do editor são testadas em dbpath separado.

`test_stock.py` cobre cálculo por movimentos assinados, datas de fronteira, quantidades zero/negativas, busca literal, entradas inválidas, variantes não suportadas, limites, paginação e exportações preservadas CSV/Excel. O teste HTTP `test_stock_expiry_matches_independent_lot_balances` lê as coleções separadamente e confere os saldos sem reutilizar o executor. A verificação do executável exige o gate `stock_expiry`, incluindo autenticação, consulta e recusa de empresa inválida/exportação completa incompatível.

`test_stability.py` acrescenta regressões de esclarecimento com data explícita, limites de período após resposta, contexto ambíguo de dia, qualidade no perfil nativo, comparação apenas por empresa, cancelamento de exportação, alertas em dias distintos e configuração de banco vazio. `tools/stability-check.py` testa o ZIP isolado, configuração/reinício, restauração de backup, rollback e atualização entre RCs, concorrência limitada e tempos de consulta. O arquivo `insight_version.py` mantém a versão consistente entre aplicação, launcher e ferramentas de entrega.

Resultados da rodada de estabilidade: [teste de 26/10/2026](teste-estabilidade-2026-10-26.md). `finalize-release.ps1 -BuildRoot <pasta> -PrepareOnly` cria uma entrega isolada antes de substituir o executável local; `check-release-package.py --release-dir <pasta>` e `stability-check.py --archive <zip>` aceitam essa entrega preparada.

## Compilação e recuperação

`powershell -ExecutionPolicy Bypass -File build-release.ps1 -StageOnly` gera um candidato isolado e verifica `--version`. Depois dos testes, `finalize-release.ps1 -BuildRoot <caminho>` preserva o executável anterior, atualiza a release local, cria ZIP com configuração sem credenciais e atualiza hashes. Não faz upload. Cada compilação usa uma pasta nova, sem apagar trabalho anterior. Os assets recebem versões por conteúdo para evitar JavaScript antigo no navegador.

`tools/verify-release.py` exercita o candidato com PATH sem Python, autenticação, perguntas, esclarecimentos, paginação, perfis, exportações, backup, agendamentos e persistência após reinício. Usa os casos conhecidos da base de homologação local. Gera um relatório com o hash do executável. A finalização exige esse relatório aprovado e correspondente ao arquivo; o build sem `-StageOnly` executa a verificação antes de finalizar. Uma instância local aberta precisa ser encerrada para a substituição do programa.

O SQLite usa transações e fecha as conexões após cada operação; backups usam a API de backup do SQLite. Configuração salva pelo assistente é testada antes da troca e usa nova chave para não sobrescrever a chave anterior. Os scripts de build não copiam credenciais nem resultados de negócio para o ZIP. Atualização não substitui a configuração local existente.
