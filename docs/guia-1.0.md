# DigiSat Insight 1.0.0-rc.3

Assistente de relatórios e investigação para o suporte DigiSat. Executável portátil para Windows x64; Python e internet não são necessários. A conexão com a base MongoDB da instalação continua necessária. Versão candidata à homologação, não uma promessa de ausência de defeitos.

## Novidades da RC3

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
