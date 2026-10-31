# DigiSat Insight 1.1.1 RC6

- Auditoria independente do mensal por vendedor: somas decimais seriais, exclusões explicadas, comparação de execuções e identidade do snapshot.
- Conferência de preço unitário fracionado, com quantidade zero explicitamente indefinida.
- APIs `/api/audit/monthly` e `/api/audit/unit-prices` incluídas no executável portátil; CLI equivalente disponível no código-fonte.
- Avisos dos serviços nativos vinculados ao build analisado, preservando os valores e a identidade remota como não verificada.
- Mantém os refinamentos de interface e conexão da RC5.

Veja [como usar e interpretar as auditorias](auditorias-independentes.md). Elas operam sobre dados normalizados e pré-selecionados; não capturam automaticamente a base Mongo/WCF nem reproduzem todos os filtros do domínio.

Esta distribuição usa o perfil de validação **Windows isolado**: testes de regras, execução real do PE sem Python externo, autenticação, APIs de auditoria e integridade do pacote. Os resultados detalhados são publicados em `verification.json`. Homologação com SuiteG6/base do cliente, CRUD Mongo real e revisão visual completa continuam pendentes; não foram substituídos por dados sintéticos.
