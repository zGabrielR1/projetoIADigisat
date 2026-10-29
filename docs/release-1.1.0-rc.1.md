# DigiSat Insight 1.1.0 RC1

Esta versão inaugura a área de Business intelligence e redesenha toda a suíte com a identidade Pulse aprovada: azul-marinho, acentos azuis/índigo, tipografia Inter local e navegação compartilhada para desktop e celular.

## Business intelligence

- Painel de faturamento, documentos, ticket médio e pagamentos registrados, com empresa, período, escopo e situação dos movimentos.
- Gráfico diário com comparação ao período anterior de mesma duração, datas reais nos tooltips e lacunas explícitas para séries incompletas.
- Rankings de empresas e produtos, participação sobre o faturamento total e drill-down empresa → dia → documentos.
- Explorador com agrupamento, pesquisa nas linhas carregadas, ordenação, subtotal visível e exportação CSV com fonte, filtros e datas.
- Modelos locais de dados: coleção, data, valor, empresa e produto, validação de tipos e nível dos itens, sem fórmulas livres ou duplicação de total por produto.
- Visões salvas com mapeamento independente, filtros, medida, pesquisa, ordenação e caminho de detalhamento. Períodos fixos ou móveis; links para reabrir a configuração e consultar a base atual.
- Persistência no SQLite do Insight, autenticação existente, vínculo à origem de dados e estados de carregamento, ausência, erro e atualização.

## Interface e distribuição

As seis áreas usam a navegação compartilhada. No celular, Visão geral, BI e Relatórios ficam acessíveis na barra inferior; as demais ferramentas abrem em Mais. A fonte Inter, os gráficos SVG e todos os estilos estão no executável, sem CDN ou dependências de internet para a interface. Cache de assets é versionado por hash em todas as páginas do pacote.

O pacote Windows x64 mantém configuração sem credenciais, guia, versão e hashes SHA-256. Atualizações não substituem o SQLite nem a configuração local. Visões e modelos de desenvolvimento não são incluídos no ZIP de distribuição.

## Verificação

Os gates do executável incluem as regras de BI, matemática de apresentação, assets/fontes empacotados, autenticação, consulta de vendas comparada ao cálculo independente, modelos/visões locais e persistência após reiniciar. Permanecem os gates de relatórios, financeiro, estoque, editor isolado, exportações, backup e autenticação. As gravações de testes do editor usam sua própria instância MongoDB isolada; o BI consulta a fonte DigiSat em leitura.

Foram executados 23 testes Python específicos de BI e 5 testes Node de alinhamento de períodos e soma de valores exibidos, além das regressões aplicáveis. A interface foi conferida em 1440, 768 e 390 pixels, nos dois temas, e a inspeção Taste final do overview terminou com 0 FAIL / 0 WARN, preservando as exceções explícitas da identidade Pulse.

## Limites desta candidata

O BI usa a conexão MongoDB configurada e os cálculos de relatórios existentes. Não inclui ingestão de arquivos, conectores externos, ETL, joins entre coleções, medidas arbitrárias ou agendamento específico de BI. As consultas de cada bloco são independentes; dados alterados durante a execução podem produzir diferenças. Rankings e tabelas têm limites informados; o subtotal visível não representa toda a base quando há pesquisa ou truncamento.

Pagamentos registrados são uma conferência dos arrays dos movimentos, não um extrato bancário. A ponte nativa mantém 356 entradas disponíveis e 358 fichas em 40 módulos; duas classes estão ausentes do build instalado. Filtros/cálculos locais das classes cliente e layouts de impressão continuam fora da ponte. A homologação funcional do conjunto nativo continua pendente, por isso esta versão permanece **release candidate**.
