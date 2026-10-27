# DigiSat Insight 1.0.2-rc.1

Melhorias no Banco de Dados Editor:

- **Criação de coleções vazias** no banco selecionado, com sessão de escrita, permissão verificada e registro no histórico. Coleções existentes são preservadas.
- **Explorador de esquema e índices**, com tipos BSON, presença de campos na amostra e caminhos reutilizáveis no construtor.
- **Consultas salvas** por conexão/usuário, com carregamento explícito e inclusão no backup local.
- **Exportação JSON/JSONL da página exibida**, preservando tipos BSON, grandes inteiros, quebras de linha e a projeção da consulta.
- Verificação de loopback real em acessos sem autenticação, além da validação de Host/Origin.

O [guia do editor](banco-de-dados-editor.md) detalha os limites da amostra, o SELECT suportado, as permissões e as cópias de recuperação. Escritas de teste usam somente o servidor MongoDB temporário criado pela suíte; nenhum documento ou coleção da base DigiSat foi alterado durante a validação.

Validação de código: 126 testes unitários/de integração e 24 testes ponta a ponta de relatórios aprovados, além das verificações JavaScript de exportação JSON/JSONL. A verificação do executável repete os 29 testes do editor em MongoDB isolado.

Todos os critérios do [relatório do executável](verification-1.0.2-rc.1.json) foram aprovados. SHA-256 do executável: `81a963031c087e014d80ca71fe8baaa5744a154b780c3b452e19508442b2e98f`. As esperas de inicialização da verificação foram ampliadas para acomodar máquinas lentas; os limites de consulta da aplicação foram preservados.

A entrega local usa executável versionado e ZIP sanitizado. O executável versionado, no mesmo diretório da instalação anterior, usa seu `config.env` e diretório de dados. Feche a instância anterior antes de iniciá-lo na mesma porta.

Conferência visual por operadores e homologação em Windows limpo continuam pendentes. O timeout de investigações/documentos anuais de vendas registrado na linha 1.0.1 continua aberto.
