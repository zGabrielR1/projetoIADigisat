# DigiSat Insight 1.0.2-rc.2

Pequenos ajustes no Banco de Dados Editor:

- Botão **Redefinir consulta** restaura filtros e opções padrão sem enviar consultas ou descartar documentos em edição.
- Atalho **Ctrl+Enter / Cmd+Enter** executa a consulta a partir do formulário.
- Atualizar a árvore mantém o banco atual aberto e destaca a coleção selecionada.
- Trocar de coleção limpa a consulta compilada anterior; os controles da página são atualizados imediatamente ao terminar uma consulta.
- A validação descobre um mês com vendas na conexão atual e confere contagens, totais, pagamentos e perfis com agregações independentes. Não depende de nomes de vendedores, números de notas ou totais de outro cliente. A aplicação mantém descoberta de coleções/campos e resolução de empresas/vendedores pela conexão configurada.

Os testes do incidente histórico de outubro continuam disponíveis quando o vendedor de referência existe na base. Na base nova, são sinalizados como indisponíveis, sem falsificar resultados esperados. Sete testes de workspace usam o conjunto atual e incluem uma prova determinística de deduplicação de alertas, sem exigir divergências reais nos dados do cliente. As escritas de teste do editor continuam restritas ao MongoDB temporário isolado.

Validação: suíte de regressão executou 112 testes com sucesso e sinalizou duas classes históricas indisponíveis; E2E executou 24 testes, com um cenário histórico skipped. Sintaxe e exportação JavaScript aprovadas. Todos os gates do [executável](verification-1.0.2-rc.2.json) foram aprovados contra a base atual, incluindo os 29 testes de editor no MongoDB isolado. SHA-256: `984c6f2bfa3e1be999c675b7aa6cc5928a666bcc2576aa9f21438ed1b57ffa67`.

A entrega usa executável versionado e ZIP sem credenciais. Feche a instância anterior antes de iniciar o novo executável no mesmo diretório, preservando `config.env` e os dados locais.

Conferência visual por operadores e homologação em Windows limpo continuam pendentes. O timeout de investigações/documentos anuais de vendas registrado na linha 1.0.1 continua aberto.
