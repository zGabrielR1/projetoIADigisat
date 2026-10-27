# DigiSat Insight 1.0.0-rc.3

RC3 acrescenta organização das investigações salvas:

- Busca local por título entre as últimas 200 cópias, sem diferenciar maiúsculas e minúsculas, com mensagem quando não há correspondências.
- Renomeação pela lista de cópias salvas, com título obrigatório de até 120 caracteres.
- Os valores, filtros e data da consulta original são preservados. Renomear não consulta o MongoDB.
- O título atualizado permanece após reiniciar o aplicativo.

A API autenticada oferece `PATCH /api/workspace/saved/{id}` com corpo `{"title":"Novo título"}`. Entradas inválidas retornam 422; cópia inexistente retorna 404.

A descoberta de valores de empresa passa a usar limite de 20 segundos, adequado à leitura de toda a coleção histórica. O limite anterior de 5 segundos causou timeout na base local durante os testes integrados. Não foram criados índices nem alterados dados do MongoDB.

O teste de regressão confere preservação do snapshot, persistência do título, limites de entrada e cópia inexistente. A verificação do executável repete renomeação, rejeição de entradas inválidas e persistência após reinício.

Validação de 27/10/2026: 88 testes de cálculo, interpretação, workspace e estabilidade aprovados, mais 23 testes E2E com HTTP real e cálculo independente no MongoDB (111 no total). A rodada E2E final passou em 278,554 segundos, após corrigir o limite de tempo da descoberta de empresas. A sintaxe de `static/workspace.js` foi verificada com `node --check`.

Continuam pendentes a homologação em Windows limpo, a aceitação dos operadores e a medição com o volume de destino descritas em [validação](validacao-1.0.md). RC3 não promove o aplicativo a versão estável.
