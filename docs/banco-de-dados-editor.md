# Banco de Dados Editor

Módulo de administração do DigiSat Insight, acessível pela barra de ferramentas ou por `/database-editor`. Sua funcionalidade principal é a gravação de documentos: inserir, editar e excluir, usando a mesma conexão MongoDB configurada no aplicativo.

## Fluxo na interface

1. Expanda um banco e selecione uma coleção existente. A árvore mostra os bancos e as coleções acessíveis à conta atual.
2. Localize documentos pelo construtor visual, pelo filtro MongoDB Extended JSON ou por uma consulta SELECT. Os resultados mostram campos e arrays em árvore, com abertura do documento completo.
3. Habilite a sessão de edição para o banco selecionado, digitando `EDITAR NomeDoBanco`. A conta deve estar autenticada e possuir a ação necessária na coleção: `insert`, `update` ou `remove`.
4. Crie um documento ou edite o JSON de um documento aberto. `_id` permanece imutável em edições; em uma inserção, o editor gera um ObjectId quando o campo não foi informado.
5. Revise os valores antes/depois e aplique. Uma requisição opera em um documento. Para excluir, confirme o nome exato da coleção.
6. Consulte o histórico para baixar os valores originais ou carregar uma cópia anterior como rascunho. Uma restauração segue o mesmo processo de prévia e aplicação.

Em MongoDB 3.4, uma conta `readWrite` limitada a um banco pode não ter permissão global para listar todos os bancos. Nesse caso, o editor abre o banco configurado no Insight e lista suas coleções, mantendo a edição disponível sem exigir a role `root`.

## Consultas

O construtor possui caminhos de campos, operadores e tipos de valor. Condições podem ser combinadas com AND ou OR. Texto “contém” é tratado literalmente; o valor `a.*` procura esses caracteres, sem virar uma expressão regular.

No modo MongoDB, use Extended JSON para preservar tipos:

```json
{"_id":{"$oid":"6496ee283a5e352378b9e8f1"}}
```

```json
{"DataHoraEmissao":{"$gte":{"$date":"2026-10-01T00:00:00Z"}}}
```

Projeção aceita inclusão/exclusão por 1/0; ordenação aceita 1/-1. O servidor acrescenta `_id` como desempate para paginação estável. Datas seguem instantes UTC; dados BSON são exibidos no formato canônico para preservar Int64, Decimal128, datas e identificadores.

Exemplo de SQL:

```sql
SELECT Descricao, CodigoInterno
FROM `ProdutosServicos`
WHERE Ativo = TRUE AND Descricao LIKE 'Café%'
ORDER BY CodigoInterno ASC
LIMIT 50 OFFSET 0;
```

O SELECT é traduzido em um filtro MongoDB, sem motor SQL ou execução de scripts. Suporta seleção de campos, WHERE com AND, comparações, IN, LIKE, IS NULL/IS NOT NULL, ORDER BY, LIMIT e OFFSET. LIKE usa `%` e `_` e ignora diferenças entre maiúsculas/minúsculas. NULL segue a semântica do MongoDB: igualdade a nulo pode incluir campo ausente. Funções SQL, OR no modo SQL, JOIN e comandos SQL de escrita ficam fora desse subconjunto; OR está disponível no construtor e no filtro MongoDB.

Limites: 200 documentos por página, deslocamento até 100.000, cinco segundos no servidor por consulta, até 1 MiB por documento e 2 MiB por resposta de resultados. Documentos maiores podem ser consultados com uma projeção menor, mas o editor completo está limitado a 1 MiB. A paginação usa ordenação determinística e não congela a base contra alterações externas.

## Escritas e recuperação

Sessões de edição duram dez minutos; prévias duram cinco. Tokens ficam somente na memória da página e do servidor, vinculados ao usuário/origem de acesso e ao banco. Mudanças nas credenciais ou conexão invalidam sessões antigas. Os relatórios mantêm sua política de leitura; o editor usa preferência pelo primário, escrita com confirmação `w=1` e sua própria verificação de permissões. Uma conta somente `read` consegue consultar, mas não habilitar gravações.

Antes de qualquer escrita, o SQLite local registra a operação planejada e o documento original. Na aplicação, o registro passa a enviado antes de chamar o MongoDB; uma operação enviada não pode ser aplicada duas vezes. O retorno do MongoDB define o estado confirmado, recusado ou incerto. Desconexão e falha de confirmação não provocam repetição automática.

Edições produzem `$set`/`$unset` somente para campos de primeiro nível efetivamente alterados. Os valores originais são comparados no filtro atômico, inclusive objetos/arrays inteiros quando um campo aninhado foi alterado. Campos novos independentes acrescentados externamente são preservados; conflito no mesmo campo bloqueia a alteração. Alterar `_id` exige criar outro documento e excluir o anterior em operações separadas.

Exclusões usam `findOneAndDelete` com `_id` e os campos originais, recuperando o documento removido para o diário. No MongoDB 3.4, esse filtro não garante detectar um campo adicional desconhecido criado entre a leitura e a exclusão. A operação permanece uma exclusão por documento, não uma transação com bloqueio do Suite G6.

O histórico é local, vinculado ao servidor/conta MongoDB e ao usuário do painel. Cópias podem ser baixadas e reaproveitadas como rascunhos. O backup do workspace inclui `editor_changes`. Guardar o SQLite e suas cópias requer o mesmo cuidado dos documentos editados. Não existe transação conjunta entre SQLite e MongoDB: em interrupção durante aplicação, o estado pode exigir conferência manual. Uma atualização de rascunho nunca grava automaticamente na base.

Bancos `admin`, `config` e `local`, além das coleções `system.*`, ficam fora das operações de gravação deste módulo. O módulo trabalha em coleções existentes e usa operações documentais tipadas; criação de usuários, concessão de permissões e alterações de índices/esquema continuam sob administração do MongoDB.

## Implementação e validação

- `insight_editor_query.py`: modelos, limites, compilação do construtor/JSON e parser SELECT.
- `insight_editor.py`: navegação, consulta, sessão, prévia, aplicação e histórico.
- `static/editor.html`, `editor.js`, `editor.css`: página dedicada com árvore e editor JSON.
- `test_editor.py`: consultas, tipos BSON, conflitos, gravação única, CRUD real, recuperação local e backup.

Os testes de escrita iniciam seu próprio `mongod` em um dbpath novo dentro de `.release-build`, em porta aleatória de loopback. Conferem o caminho de dados antes de criar usuários ou inserir fixtures; reiniciam essa instância com autenticação e exercitam a API HTTP real. Não usam a conexão DigiSat para criar, editar ou excluir dados de teste.

Executar `python -m unittest test_editor -v`. `INSIGHT_TEST_MONGOD` permite informar o executável MongoDB de teste; o padrão usa o binário local da instalação. Para verificar um candidato compilado, `INSIGHT_EDITOR_TEST_EXE` deve apontar para um executável dentro de `.release-build`; nesse caso a suíte inicia o PE com PATH sem Python e repete o CRUD isolado.
