# Verificação da 1.1.0 RC1

O candidato Windows x64 foi compilado com PyInstaller e o host nativo .NET. `tools/verify-release.py` terminou com todos os gates aprovados para `1.1.0-rc.1`.

## Executável empacotado

- Inicialização com PATH mínimo, versão correta e autenticação da API.
- Assets de todas as páginas, módulos BI, fonte Inter e avisos de licença conferidos por hash contra o staging.
- Faturamento/documentos de BI conferidos contra cálculos independentes na origem configurada; comparação diária e validação de filtros verificadas.
- Modelos e visões criados apenas no SQLite isolado de QA; reabertura da configuração e persistência após reiniciar confirmadas.
- Regressões do chat, esclarecimentos, perfis de cálculo, paginação, resultados salvos e exportações PDF/Excel/CSV.
- Agendamentos, deduplicação de alertas e integridade do backup SQLite.
- Estoque e financeiro, incluindo regras com MongoDB isolado e conferência financeira de leitura na origem configurada.
- CRUD, criação de coleção, consultas, esquema e histórico do editor verificados exclusivamente em instância MongoDB de testes isolada.
- Contratos nativos, catálogo de 358 fichas/356 entradas disponíveis, descoberta em português e prévia de filtros sem execução.
- Modo estrito de leitura respeitando as permissões efetivas da conta.

## Interface e regras

23 testes Python específicos de BI e 5 testes Node de apresentação passaram. Também passaram os testes pertinentes de catálogo, estado local e serialização de exportação do editor. A matemática cobre períodos iguais, ano bissexto, preservação de datas do período anterior, lacunas em séries incompletas, soma de centavos exibidos e independência das visões em relação a modelos excluídos.

A revisão de navegador verificou os três espaços de BI em 1440, 768 e 390 pixels, nos temas claro e escuro, sem overflow horizontal ou erros JavaScript. A inspeção Taste do overview terminou com 0 FAIL / 0 WARN, mantendo a identidade Pulse explicitamente aprovada. Foram exercitados reabertura por link, drill-down, pesquisa, ordenação, subtotal, CSV e recuperação de falha preservando dados anteriores.

## Distribuição

`tools/check-release-package.py` confere Windows x64 PE, versão, hashes e o allowlist do ZIP: executável, configuração sem credenciais, guia, manifesto e SHA256SUMS. Estado local, credenciais, dados consultados, testes, referências de engenharia e instruções de agentes não entram no pacote.

O SHA-256 do executável aprovado é `1e469b3ad3bea8a9284a64d5df4929d7ad916f2018f654fe22b7abee11070814`. Os hashes dos demais arquivos acompanham `SHA256SUMS` na release.

A aprovação dos gates não substitui homologação completa dos relatórios nativos e dos fluxos de negócio na instalação de destino. Os limites funcionais estão descritos nas notas da 1.1.0 RC1 e no guia incluído.
