# DigiSat Insight 1.0.1-rc.2

Novo módulo **Banco de Dados Editor**, com escrita como funcionalidade principal. A interface permite criar, editar e excluir documentos MongoDB na conexão configurada no Insight.

- Página dedicada acessível pelo botão no painel, com árvore de bancos, coleções e documentos.
- Construtor visual de filtros, MongoDB Extended JSON e consultas SELECT traduzidas para MongoDB.
- Editor de documento completo com tipos BSON preservados e `_id` imutável em edições.
- Sessões de escrita por banco, prévia antes/depois e aplicação explícita em um documento.
- Conflitos bloqueiam substituição de valores originais alterados externamente; campos novos independentes são preservados nas edições.
- Histórico local, download de cópias e recuperação como rascunho, incluídos no backup do workspace.
- Gravações incertas não são repetidas automaticamente; o histórico permite conferir os valores envolvidos.

O [guia do módulo](banco-de-dados-editor.md) descreve o SELECT suportado, os limites de consulta, as permissões e as particularidades de concorrência do MongoDB 3.4. Relatórios e estoque continuam usando APIs de leitura. O editor é um módulo de administração separado e não modifica automaticamente a política de verificação de contas dos relatórios.

Validação de escritas: fixtures sintéticas em um servidor MongoDB separado, com novo diretório de dados e porta aleatória, autenticação e API HTTP. Nenhum dado DigiSat foi inserido, alterado ou excluído durante esses testes.

Resultado: 120 testes nas suítes de cálculo, interpretação, workspace, estoque, estabilidade e editor aprovados; mais 24 testes HTTP dos relatórios, totalizando 144. A suíte do editor contém 23 testes e foi repetida com o executável compilado, com PATH sem Python. CRUD, consulta, histórico, backup, proteção de sessão, conflitos e conta `readWrite` limitada ao banco passaram. O [relatório do executável](verification-1.0.1-rc.2.json) vincula os gates ao SHA-256 do candidato.

A entrega local inclui `release/DigiSatInsight-1.0.1-rc.2-win64.zip` e `release/DigiSatInsight-1.0.1-rc.2.exe`. O executável versionado pode ser iniciado no mesmo diretório da instalação existente e usa seu `config.env` e diretório de dados. Feche a instância anterior antes de iniciá-lo na mesma porta. O pacote ZIP contém somente a distribuição sanitizada; suas configurações de exemplo não contêm credenciais.

Permanecem pendentes a conferência visual por operadores e a homologação em Windows limpo. A limitação de timeout nas consultas anuais de investigação/documentos registrada na RC1 continua aberta; esta RC não anuncia sua correção.
