# Consultas nativas SuiteG6 — 1.0.3 RC1

Abra **Relatórios nativos** em `/native-reports`, selecione empresa e entre com um usuário normal do SuiteG6. Escolha módulo, relatório e chamada, preencha os filtros e execute. Quando houver referências cadastrais, use a pesquisa para selecionar o registro. O resultado permite escolher conjuntos de dados, consultar detalhes, paginar e exportar a página em CSV.

O catálogo contém 358 entradas em 40 módulos. Há 356 entradas vinculadas a 282 pares distintos de contrato/método. As assinaturas e os tipos dos parâmetros foram verificados por reflexão nas bibliotecas instaladas, versão **2.0.26300.5**. O mapa em `static/native-service-map.json` registra SHA-256, MVID e versão de quatro assemblies, além do inventário completo dos tipos de `Modulo.Relatorios.dll`. `Pessoa.FichaCadastral` e `Pessoa.FichaCadastral40Colunas` não estão nesse inventário e permanecem indisponíveis.

## O que a consulta executa

O componente local .NET utiliza as bibliotecas instaladas e o protocolo WCF do próprio SuiteG6 em `127.0.0.1:12221`. Cada execução autentica, chama um método permitido e encerra sua própria sessão nativa. O servidor continua responsável por autorização, licenciamento e regras de negócio. Um usuário já conectado em outra sessão pode ser recusado; use outro usuário autorizado. A integração não encerra sessões de terceiros.

As senhas ficam somente na memória do processo Insight por até 20 minutos. O navegador mantém apenas um identificador temporário, sem gravá-lo no armazenamento local. **Desconectar** remove a conexão do Insight. As bibliotecas proprietárias não acompanham a distribuição: o SuiteG6 instalado e o serviço local são necessários. A pesquisa auxiliar de referências usa a conexão MongoDB configurada no Insight, com escopo de empresa quando aplicável.

## Filtros e resultados

Tipos e enumerações vêm dos contratos instalados. Rótulos, valores fixos e parte das opções foram extraídos das fontes de interface retidas; essas fontes possuem identidade própria e não são apresentadas como código atual do build instalado. Os objetos de valor de origem e situação são lidos das bibliotecas atuais. A API rejeita parâmetros desconhecidos, tipos arbitrários e alterações de valores fixos.

Os conjuntos exibem os DTOs devolvidos pelo serviço, preservando o contexto dos registros pai. Não se inventam totais ou linhas para módulos sem dados. Valores decimais são preservados como texto exato na resposta. O limite é de 50 mil elementos e 16 MiB por resposta; reduza o período quando necessário. O CSV contém somente a página selecionada.

**Disponibilidade do serviço não significa reprodução integral do relatório impresso.** Filtros adicionais, ordenação, cálculos locais, sub-relatórios, agrupamentos e apresentação executados pela classe cliente/DevExpress não são executados por esta ponte. As fichas individuais documentam as chamadas e os elementos de layout recuperados. Para impressão fiscal ou conferência de um total apresentado pelo cliente original, confira também o relatório nativo. Nenhum dos layouts originais é declarado reproduzido.

## Evidência e manutenção

`tools/build-native-map.py` constrói a lista permitida a partir de fontes retidas com hashes conferidos; `native/build.ps1` compila o componente próprio, sem copiar DLLs do fornecedor. A operação `describe` valida os contratos atuais e registra a presença das classes. `tools/enrich-native-map.py` acrescenta metadados da interface e `tools/build-report-catalog.py` regenera as fichas.

A verificação de assinaturas cobre as 356 entradas. A execução real foi confirmada inicialmente em cadastro de CFOP e recebimentos, com 40 e 53 registros respectivamente. Isso não homologa dados ou impressão de todos os módulos: alguns dependem de licença, cadastros e movimentos específicos da empresa.
