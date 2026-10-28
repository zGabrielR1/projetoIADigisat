# DigiSat Insight 1.0.3-rc.1

O candidato Windows x64 foi compilado e passou nos gates automatizados do pacote. A integração nativa disponibiliza consultas de serviço para **356 das 358 entradas** em **40 módulos**, com 282 pares distintos de contrato/método. A distribuição é local; não foi criada tag nem feita publicação remota.

## Conteúdo

- [Catálogo e fichas por relatório](relatorios-1.0.3-rc.1/README.md), com 358 entradas e 330 classes distintas; página `/reports`.
- [Consultas nativas](relatorios-nativos-1.0.3.md), em `/native-reports`: login normal SuiteG6, parâmetros tipados, pesquisa de referências, conjuntos de dados, detalhes, paginação e CSV da página.
- [Financeiro](financeiro-1.0.3.md), em `/finance-reports`: 21 consultas específicas de pagamentos e recebimentos.
- Mapa de contratos verificado contra bibliotecas instaladas 2.0.26300.5, com hashes, MVID e inventário completo das classes em `static/native-service-map.json`.
- Componente próprio .NET Framework x86 incorporado ao executável x64. Usa as DLLs instaladas e não redistribui bibliotecas proprietárias.

## Validação

Os oito testes da ponte nativa e seis testes de catálogo passaram. A regressão adicional de estabilidade, workspace e estoque passou em 28 testes. Os gates do executável verificaram financeiro com MongoDB temporário isolado, editor com CRUD isolado, autenticação, rejeição de operadores, persistência, paginação, exportações PDF/Excel/CSV, backup SQLite, modo somente leitura, assets e metadados nativos.

As [21 verificações financeiras da base atual](finance-verification-1.0.3-rc.1.json) passaram, sem escrita na base do cliente. Os testes financeiros incluem 432 vetores contra reconstrução C# independente e seis cenários em MongoDB 3.4 isolado.

A [execução nativa amostrada](native-verification-1.0.3-rc.1.json) registra apenas contagens e estados. CFOP retornou 40 registros; recebimentos, 53; crédito disponível retornou um conjunto vazio; produtos por grupo retornou um registro. Os contratos foram verificados para 356 entradas; esses exemplos não homologam todos os módulos, licenças ou regras de apresentação.

A [inspeção REA do assembly de relatórios](native-report-artifact-1.0.3-rc.1.json) confirma sua identidade estática. Foi usada a CLI existente porque as ferramentas MCP REA não estão expostas nesta sessão. As fontes de interface retidas e o [rastreamento financeiro](finance-artifacts-1.0.3-rc.1.json) têm hashes próprios e representam snapshots separados da verificação dos contratos atuais. Cinco inspeções financeiras REA têm metadata completa; 14 ficaram indisponíveis por orçamento de metadata de Domain.Main, com reconstrução separada por dnSpy.

## Limites do candidato

O chat em linguagem natural ainda usa os planos existentes de vendas e investigação. Ele não está integrado ao catálogo das 356 entradas nativas e não interpreta perguntas arbitrárias sobre toda a base. Nesta RC1, a seleção e execução dessas entradas ocorre pela tela de relatórios nativos.

`Pessoa.FichaCadastral` e `Pessoa.FichaCadastral40Colunas` estão ausentes do inventário completo do assembly instalado. Permanecem indisponíveis, com indicação explícita no catálogo.

A ponte executa os serviços nativos e apresenta seus DTOs. Filtros, cálculos e ordenações adicionais executados localmente pela classe cliente, sub-relatórios e layouts DevExpress não são reproduzidos. Há **zero layouts nativos declarados reproduzidos**. As fichas documentam a evidência recuperada desses elementos; a homologação de equivalência de impressão permanece aberta.

A automação de interface retornou inventário sem aplicativos ou navegadores conectados. A conferência visual não foi executada e não é declarada aprovada.
