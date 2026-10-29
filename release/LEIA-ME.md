# DigiSat Insight 1.1.0-rc.1

Assistente de relatórios e investigação para o suporte DigiSat. Executável portátil para Windows x64; Python e internet não são necessários. A conexão com a base MongoDB da instalação continua necessária. Versão candidata à homologação, não uma promessa de ausência de defeitos.

## Novidades da 1.1.0 RC1

Nova interface Pulse em toda a suíte: superfícies azul-marinho, ações em azul, tipografia Inter local, navegação compartilhada e menu adaptado ao celular. Os assets acompanham o executável e funcionam offline.

Abra **Business intelligence** para consultar faturamento, documentos, ticket médio, pagamentos, evolução diária, empresas e produtos. Escolha período, empresa e escopo; revise o modelo sugerido e clique em **Atualizar análise**. A comparação mostra o período anterior de mesma duração, inclusive no gráfico. Zeros, valores ausentes, falhas parciais e limites são informados separadamente.

Em **Explorar dados**, navegue de empresa para dia e documentos, pesquise nas linhas carregadas, ordene e exporte CSV. O subtotal corresponde somente às linhas exibidas. Em **Fontes e modelo**, mapeie data, valor, empresa e produto para a coleção da conexão atual; os modelos são guardados apenas no Insight.

**Salvar visão** guarda filtros, medida e caminho de exploração. A seleção abaixo do título abre a configuração e consulta novamente a base. O período pode acompanhar este mês, mês passado ou os últimos 7/30 dias, ou manter datas fixas. Um dia detalhado mantém datas fixas. **Copiar link** compartilha a configuração nesta instalação, com autenticação normal; não compartilha credenciais nem uma cópia dos resultados. Modelos e visões fazem parte do SQLite local e do backup do workspace.

O BI não altera a base DigiSat. Usa os cálculos do motor de relatórios e a conexão MongoDB já configurada; não inclui conectores externos, ETL, junções entre coleções ou fórmulas livres. A comparação de itens com pagamentos não é um extrato bancário, e valores divergentes precisam ser conferidos antes do fechamento. Modelos e visões ficam vinculados à origem de dados.

## Busca e filtros inteligentes mantidos da 1.0.3 RC3

A busca agora sugere situação, base de data, operação de cartão e consolidação quando o pedido e as opções documentadas do relatório permitem. Por exemplo, **contas a receber atrasadas deste mês** sugere a situação **Atrasadas** e a base **Data de vencimento**. Confira o quadro de sugestões; os filtros continuam editáveis. A ausência de período e pedidos com módulos ou situações diferentes são sinalizados.

Ao clicar em **Consultar no SuiteG6**, uma revisão mostra o relatório, a conexão e os filtros reais. **Executar estes filtros** faz a consulta; **Voltar aos filtros** permite corrigir o pedido. Links de sugestões usam opções permitidas e não aceitam referências de clientes ou objetos livres como filtros pré-definidos.

## Recursos da RC2 mantidos

Encontre relatórios em português na Central de relatórios ou no chat: por exemplo, **contas a pagar em aberto deste mês**, **clientes inadimplentes** ou **cadastro de CFOP**. A busca local cobre as 358 entradas do catálogo e sugere opções; escolher uma abre os filtros. O período reconhecido é levado como sugestão. Confirme os parâmetros e conecte sua conta antes de executar. A descoberta não consulta dados nem responde perguntas arbitrárias sobre toda a base.

Use **Favoritar** e **Só favoritos** para guardar os IDs dos relatórios neste navegador. **Copiar link** compartilha o relatório e o período sugerido, sem login ou referências de clientes. Além do CSV, há **JSON da página**, com contexto e valores decimais preservados. A tela organiza escolha, conexão e resultado em painéis, com modo claro/escuro e adaptação para telas pequenas.

## Relatórios SuiteG6

Abra **Relatórios nativos**, selecione a empresa e entre com um usuário autorizado do SuiteG6. Há consultas de serviço para 356 das 358 entradas do catálogo, em 40 módulos. Escolha o relatório e seus filtros; o resultado oferece conjuntos de dados, paginação, detalhes e CSV da página. O SuiteG6 instalado e seu serviço local são necessários. As bibliotecas do fornecedor não acompanham este pacote.

A conexão dura até 20 minutos e guarda a senha somente em memória. Cada consulta entra e sai de sua própria sessão nativa. Se o usuário já estiver conectado em outro lugar, utilize outro login autorizado. As permissões e licenças continuam sendo verificadas pelo SuiteG6.

As duas fichas cadastrais não estão presentes no assembly instalado. A ponte retorna dados dos serviços: filtros e cálculos locais da classe cliente e layouts de impressão não são reproduzidos. Confira o relatório original quando precisar desses elementos. **Catálogo SuiteG6** mostra as fichas documentadas; **Financeiro** oferece ainda 21 consultas específicas de pagamentos e recebimentos.

## Novidades do editor (1.0.2 RC2)

- **Redefinir consulta:** limpa os filtros, projeção, ordenação e paginação, mantendo o modo selecionado. Os resultados e o documento em edição permanecem disponíveis até executar outra consulta.
- **Ctrl+Enter / Cmd+Enter:** executa a consulta a partir dos campos do formulário.
- **Atualizar árvore:** mantém aberto o banco atual e destaca a coleção selecionada. Trocar de coleção também limpa a consulta compilada anterior.

- **Nova coleção:** selecione uma coleção no banco de destino, habilite a edição e informe o nome da nova coleção. A confirmação cria uma coleção vazia e registra a operação no histórico. Uma coleção existente não é substituída.
- **Esquema e índices:** mostra os tipos BSON e a presença dos campos em até 100 documentos, além dos índices visíveis à conta. **Usar campo** insere o caminho no construtor de filtros; os campos também ficam disponíveis como sugestões. A amostra considera até 25 elementos por array e oito níveis; não representa todo o esquema.
- **Salvar consulta / Consultas salvas:** preserva filtros e opções localmente por conexão e usuário. Carregar repõe os controles; **Executar consulta** continua sendo uma ação explícita. As consultas salvas entram no backup do workspace.
- **JSON da página / JSONL da página:** baixa exatamente os documentos e a projeção exibidos, em Extended JSON canônico, preservando ObjectId, datas, Decimal128 e Int64. JSON inclui o contexto da página; JSONL contém um documento por linha.

## Banco de Dados Editor

Abra **Banco de Dados Editor** na barra de ferramentas. A nova página usa a conexão já configurada. Expanda um banco e escolha uma coleção; execute uma consulta pelo construtor visual, pelo filtro MongoDB Extended JSON ou pelo modo SQL de consulta (`SELECT`). Os resultados têm uma árvore de campos; **Abrir documento completo** carrega o JSON para edição.

Para gravar, escolha **Habilitar edição** e digite `EDITAR NomeDoBanco`, usando o nome exibido. A conta MongoDB precisa estar autenticada e ter as permissões de inserir, alterar ou excluir na coleção. A sessão dura dez minutos. A política `INSIGHT_REQUIRE_READ_ONLY` continua valendo para relatórios; o editor é um módulo de administração com verificação própria de permissões e não altera essa configuração automaticamente.

Use **Novo documento** para inserir, altere o JSON completo para editar ou escolha **Excluir documento**. **Revisar alteração** mostra os valores antes/depois. Só **Aplicar alteração** envia uma gravação ao MongoDB; exclusão exige digitar o nome da coleção. O identificador `_id` é preservado nas edições.

**Histórico de gravações** permite consultar as últimas cinquenta operações, baixar cópias anteriores/posteriores e carregar uma cópia anterior como rascunho. Restaurar também exige revisar e aplicar uma nova gravação. O diário faz parte do SQLite local e do backup do espaço de trabalho; ele contém os documentos editados e deve acompanhar os cuidados de acesso e cópia dos dados locais.

Tipos BSON são representados em Extended JSON: `{"$oid":"..."}`, `{"$numberLong":"9007199254740993"}`, `{"$numberDecimal":"12.34"}` e `{"$date":...}`. Mantenha essas representações ao editar. Para inteiros grandes, use `$numberLong` em vez de números comuns no navegador.

SQL é um subconjunto de consulta: `SELECT`, `WHERE` com `AND`, comparações, `IN`, `LIKE`, `IS NULL`, `ORDER BY`, `LIMIT` e `OFFSET`. A tradução MongoDB fica visível. Filtros de data e ObjectId estão disponíveis em Extended JSON e no construtor por tipo.

As gravações são diretas no MongoDB e obedecem à validação configurada nele. Uma edição modifica somente os campos de primeiro nível alterados no JSON e preserva campos novos independentes adicionados por outro processo. Valores originais modificados por outro processo bloqueiam a aplicação. No MongoDB 3.4, um campo novo acrescentado no último instante pode não bloquear uma exclusão. Em perda de conexão ou confirmação incerta, confira o documento e o histórico antes de preparar outra alteração; a mesma operação não é repetida automaticamente.

## Validade de lotes (1.0.1 RC1)

Limite conhecido desta RC: a rodada ampliada encontrou falha em consultas anuais de investigação/documentos de vendas. Use períodos menores nessas consultas enquanto a causa é revisada. A nova consulta de lotes foi validada separadamente.

Abra **Validade de lotes** na barra de ferramentas. Selecione a empresa, a data de referência e um horizonte de 0 a 366 dias. A consulta lista código, produto, lote, validade, quantidade física, unidade e situação. Busque por produto, código ou número do lote; use Anterior/Próxima para navegar.

Sem **Incluir vencidos**, só entram validades posteriores à referência; lotes que vencem na própria data ficam fora, como no relatório nativo. Com essa opção, entram também os vencidos e os que vencem na referência. O último dia do horizonte é inclusivo. **Somente lotes com quantidade positiva** remove saldos zero e negativos.

A quantidade vem da soma dos movimentos assinados vinculados a cada lote; reservas não são descontadas. Não é quantidade disponível para promessa de entrega. O painel não soma unidades de produtos distintos. Lotes com grade ou serial ainda não são suportados: o aplicativo recusa a consulta em vez de apresentar saldo parcial. O limite por consulta é de 20.000 lotes, com orçamento de 20 segundos.

**CSV desta página** e **Excel desta página** exportam os valores preservados da página exibida, com os filtros e a origem. Não exportam silenciosamente os outros lotes nem recalculam a base. Uma nova consulta pode refletir movimentos de estoque posteriores.

## Organização de investigações salvas

Em **Investigações salvas**, busque pelo título entre as últimas 200 cópias e use **Renomear** para organizar a lista. O novo título é preservado após reiniciar; os valores e filtros da cópia continuam os mesmos. A busca ignora maiúsculas e minúsculas.

## Iniciar e configurar

1. Extraia o ZIP em uma pasta local com permissão de gravação e abra `DigiSatInsight.exe`.
2. Mantenha a janela do executável aberta. O navegador abre em `http://127.0.0.1:8088`.
3. Use **Configurar conexão**, informe servidor, porta, banco, usuário e senha, e clique em **Testar conexão**. Salvar testa novamente, preserva a configuração anterior e aplica a conexão.
4. Use preferencialmente uma conta criada pelo administrador com role `read`. O ZIP exige, por padrão, comprovação das permissões de leitura. Contas com permissões amplas são recusadas enquanto essa exigência estiver habilitada. O administrador pode desabilitar a exigência conscientemente no assistente; isso não reduz as permissões da conta no MongoDB. O aplicativo não cria usuários, índices ou coleções no DigiSat.

O arquivo `config.env` permanece editável. A senha informada no assistente é armazenada em um arquivo local separado, nunca exibida pelos diagnósticos. Os arquivos de configuração e chave anteriores são preservados para recuperação. Na configuração existente de desenvolvimento, as opções atuais são mantidas durante a atualização.

Para outra porta: `DigiSatInsight.exe --port 8090`. Para não abrir o navegador: `--no-browser`. Para consultar a versão: `--version`. Se o mesmo painel já estiver na porta solicitada, o executável reutiliza a instância existente. Ctrl+C encerra o servidor. A interface HTTP fica no próprio computador.

## Conversar e esclarecer

Exemplo: “Existe uma diferença no dia 05/10/2026 no vendedor 2 de cerca de R$100,00”. O assistente identifica período, vendedor e valor de referência e mostra o plano aplicado, os totais e as vendas correspondentes.

Continue com:

- “E o vendedor 3?” ou, após selecionar um vendedor, “E o 3?”;
- “E no dia seguinte?” ou “Compare com o período anterior”;
- “Só NFC-e”, “Somente concluídas”, “Sem cancelados”;
- “Todos os vendedores” ou “Todas as empresas”;
- “Mostre só as sem pagamento” ou “Só as compartilhadas”;
- “Compare vendedor 2 com vendedor 3 em 05/10/2026”;
- “Compare 05/10/2026 com 06/10/2026”.

O contexto permanece na conversa e pode ser retomado ao recarregar a página. **Nova conversa** começa sem o vendedor e a comparação anteriores. Perguntas incompletas, vendedores ambíguos e ajustes não reconhecidos geram uma pergunta de esclarecimento; não são executados enquanto a resposta estiver pendente. Use os botões oferecidos ou responda no campo da pergunta. Nomes podem ser informados como `vendedora "Ana Paula"`.

Datas sem ano usam o ano corrente e aparecem completas no plano. “Dia 5” sem mês precisa ser esclarecido. Sempre confira período, empresa, vendedor, perfil e situações exibidos. O provedor de IA compatível opcional classifica a pergunta entre operações permitidas; não recebe resultados da base nem produz comandos executáveis. Sem provedor, a interpretação contextual usa regras locais. Pedidos fora das operações implementadas precisam ser reformulados; o assistente não é um executor livre de consultas.

## Relatórios, perfis e investigação

Os sete relatórios existentes continuam disponíveis: faturamento, produtos, vendas por dia, empresas, documentos, formas de pagamento e ticket médio. Investigações mostram documentos e itens, atribuição por vendedor, itens de outros vendedores/sem vendedor e diferença entre itens e pagamentos. O valor aproximado mencionado serve para comparação, sem excluir vendas por valor.

Perfis:

- **Painel:** aplica os tipos e situações selecionados na interface.
- **NFC-e concluídas — itens por vendedor:** aplica NFC-e, situação Concluído, finalidade normal e itens não cancelados. As regras foram conferidas para a apuração mensal por vendedor. Não representa todos os relatórios nativos; pagamentos continuam pertencendo ao documento inteiro.
- **Vendas fiscais válidas:** NFC-e, NF-e de saída e notas manuais, excluindo cancelados, rejeitados e inutilizados.

O total do item usa preço × quantidade arredondado a duas casas, com metades afastadas de zero, menos descontos, mais despesas, ST, IPI, frete e seguro. As investigações excluem itens cancelados. Pagamentos usam ItensPagamentos, com Pagamentos como alternativa nos documentos antigos. Não são rateados automaticamente por vendedor. Formas de pagamento agrupam espécies; não reproduzem o detalhamento de cada transação TEF. Ticket médio não reproduz todas as separações do relatório nativo. Uma diferença demonstrada na base atual não comprova alterações históricas.

**Comparar períodos ou vendedores** também permite definir as duas datas, vendedor e empresa da comparação por formulário. Os resultados exibem valores da base, valores da comparação, diferença e percentual. A direção é comparação menos base; percentual não é calculado quando a base é zero.

## Buscar e navegar

Em Documentos, Investigação e Qualidade, busque número de documento, série, empresa ou produto. Escolha o tamanho da página e use Anterior/Próxima. Os totais abrangem todos os documentos encontrados; a tabela indica a faixa exibida. Cada venda pode ser expandida para consultar produtos, quantidades, vendedores e valores. Rankings agregados continuam sujeitos ao limite indicado; use a visão de documentos para a evidência completa.

## Salvar e exportar evidências

**Salvar investigação** preserva uma cópia dos resultados e do plano, identificada por título e data. Abrir essa cópia não consulta o MongoDB. O aviso “CÓPIA SALVA” distingue valores históricos de uma consulta atual. Executar ou continuar a conversa consulta a base novamente.

Excel, PDF e CSV com evidências permitem escolher:

- **Resultado salvo:** exporta os números preservados nessa execução, incluindo a página exibida; não recalcula.
- **Todos os documentos:** executa uma nova consulta paginada à base atual, incluindo as duas bases quando há comparação. Os rankings são convertidos em documentos de suporte para não ocultar vendas por um limite de ranking.

As evidências incluem pergunta, plano, fonte, datas, versão, conclusões, avisos e documentos. Excel e PDF também incluem os itens das vendas. A exportação ocorre em segundo plano, mostra progresso, permite solicitar cancelamento e só entrega arquivos concluídos. Excel limita cada folha à capacidade do formato; o PDF aceita até 2.000 documentos e 20.000 itens. Para períodos maiores, use Excel/CSV ou divida o período. A consulta completa admite até um milhão de documentos. Limites não produzem arquivos parciais silenciosos.

Consultas completas são leituras ao longo da exportação, sem snapshot transacional do MongoDB. Mudanças de quantidade durante a exportação fazem o arquivo falhar para permitir nova tentativa; outras mudanças simultâneas ainda são possíveis. A evidência distingue as conclusões originais das conclusões da nova consulta.

## Qualidade dos dados

**Verificar qualidade dos dados** conta vendas com itens sem vendedor, sem pagamento, diferenças entre itens e pagamentos, total negativo e participação de vários vendedores. Também lista até 100 grupos candidatos à duplicidade pelo mesmo tipo, empresa, série e número. Candidato à duplicidade não é comprovação de erro; os documentos precisam ser conferidos. Os filtros de conferência permitem restringir a listagem ao problema escolhido. Nenhum registro é corrigido automaticamente.

## Agendamentos e alertas

Crie um agendamento a partir de uma investigação, escolhendo intervalo, datas fixas/sempre hoje/sempre ontem, métrica e limite. A execução automática precisa ser ativada explicitamente. **Executar agora** permite testar a conferência mesmo pausada.

Agendamentos só executam enquanto o DigiSat Insight está aberto. Ao reabrir, verificações vencidas são retomadas. Um alerta é criado quando o resultado muda e ultrapassa o limite; resultados iguais são deduplicados. Alertas e evidências ficam no aplicativo, sem envio de e-mails, mensagens ou dados a terceiros. Falhas ficam registradas na lista.

## Diagnóstico, atualização e recuperação

Diagnóstico mostra conexão, verificação de permissões, datas dos documentos, índices disponíveis, tempos de consulta e eventos recentes sem senhas. Metas: até 5 segundos para consultas usuais e 20 segundos para investigações; precisam ser medidas com volume, índices e hardware representativos. Consultas possuem limites de tempo e não criam índices no MongoDB.

Conversas, cópias salvas, agendamentos, alertas e exportações ficam em `data` ao lado do executável. `INSIGHT_DATA_DIR` permite definir outra pasta. Em Investigações salvas, **Baixar backup do espaço de trabalho** produz uma cópia SQLite consistente. Para recuperar, encerre o aplicativo, preserve a pasta atual e copie o backup como `data/insight.sqlite3`. O backup contém resultados de negócio e deve ficar com a equipe autorizada.

Para atualizar, encerre o executável e substitua apenas o programa e o guia. Preserve `config.env`, chaves locais e a pasta `data`. A compilação guarda o executável anterior fora da entrega e a configuração anterior fica em `.previous` após uma alteração pelo assistente. Para voltar, encerre a versão nova, recoloque o executável anterior e a configuração correspondente; preserve os dados locais.

Para compartilhar esta entrega, use **DigiSatInsight-1.0.0-rc.2-win64.zip**. O ZIP contém apenas o executável, configuração sem credenciais, este guia, versão e hashes. A pasta de execução local pode conter credenciais e histórico adicionados durante o uso. Nenhuma cópia foi enviada para a rede pelo processo de entrega.

Antes da adoção definitiva, homologue a versão candidata em uma estação Windows limpa e em uma base representativa, conferindo os mesmos filtros dos relatórios nativos. Testes automatizados e verificações do executável reduzem regressões, mas não garantem ausência total de defeitos.
