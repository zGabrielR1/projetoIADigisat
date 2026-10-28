# DigiSat Insight 1.0.3 RC2

A RC2 evolui a interface existente e acrescenta descoberta dos relatórios em linguagem natural. A central mantém as 356 entradas com serviços disponíveis e as 358 fichas em 40 módulos.

## Novidades

- Busca local em português por assunto, módulo, título ou identidade exata, com sinônimos, tolerância limitada a variações de escrita e opções para escolha. Todas as 358 identidades foram verificadas como pesquisáveis.
- Sugestões de relatórios no chat, sem executar uma consulta de vendas incorreta para um pedido de outro módulo. As perguntas e continuações de vendas existentes mantêm seu fluxo.
- Reconhecimento de período e abertura dos filtros com datas sugeridas. Datas inválidas são apontadas; empresa, pessoas e referências não são inventadas. A consulta só ocorre após login e confirmação.
- Central organizada em busca, escolha/filtros, conexão e resultados, com tema claro/escuro e adaptação para telas pequenas. Catálogo e navegação principal receberam refinamentos.
- Favoritos locais, guardando apenas IDs de relatórios, filtro de favoritos, link compartilhável com relatório/período e JSON da página com contexto e decimais preservados.
- Respostas atrasadas são descartadas se a seleção ou os filtros mudarem durante uma consulta.

## Escopo e limites

A busca em linguagem natural descobre opções do catálogo; não é uma promessa de responder perguntas arbitrárias sobre toda a base. O usuário escolhe a sugestão, confirma os parâmetros e executa com suas permissões SuiteG6. Os planos de vendas e investigação existentes continuam separados da execução nativa.

As duas fichas cadastrais permanecem ausentes do assembly instalado. A ponte não reproduz filtros/cálculos locais das classes cliente nem layouts de impressão. As identidades e fichas da [RC1](release-1.0.3-rc.1.md) continuam como evidência de base; os contratos instalados não foram alterados por esta evolução de interface.

## Verificação

Seis testes novos cobrem todas as identidades, intenções em português, período, datas inválidas, consultas desconhecidas, limites da API e garantia de que a descoberta no chat não executa um plano de dados. Os testes de estabilidade/workspace existentes passaram. Os gates do pacote foram ampliados para verificar descoberta, autenticação e integração no chat dentro do executável.

A navegação no navegador confirmou busca por contas a pagar, seleção do relatório e formulário correspondente. A captura visual apresentou timeout no serviço do navegador; a verificação por screenshot não é declarada aprovada.
