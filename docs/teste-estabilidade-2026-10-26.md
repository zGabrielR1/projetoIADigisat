# Teste de estabilidade - 26/10/2026

**Resultado:** o candidato corrigido **1.0.0-rc.2** passou nas verificações técnicas executadas nesta máquina. Não foi promovido a 1.0 estável porque a homologação em sistema limpo e com a equipe/ambiente de destino continua pendente.

SHA-256 do executável testado: `0833051be48c721980de5c989ed3e018ea4c811f46f33c6ce47a62500fd38ee7`.

## Cobertura executada

- **109 testes unittest aprovados:** 86 nas suítes de cálculo, interpretação, workspace e estabilidade, mais 23 testes E2E com HTTP e cálculo independente no MongoDB.
- Benchmark existente de **44 perguntas** aprovado, além de **12 novas regressões de estabilidade**.
- Executável real com PATH sem Python, autenticação, perguntas e continuações, esclarecimentos, perfis, paginação, exports PDF/Excel/CSV, agendamento, deduplicação, reinício e integridade do backup SQLite.
- Interface: data explícita preservada após esclarecer vendedor; intervalo excessivo corrigido por resposta; qualidade no perfil nativo visível; layout com 390 px sem transbordamento horizontal.
- ZIP extraído em pasta independente com espaços e acentos, sem fontes ou ambiente virtual. O assistente foi configurado pela API, uma senha incorreta foi rejeitada sem alterar a configuração e a conexão persistiu após reiniciar.
- Backup restaurado em outro diretório de dados, com reabertura da evidência salva e `PRAGMA integrity_check` aprovado.
- Rollback real do executável de RC2 para RC1 e atualização de volta para RC2, preservando os dados de teste.
- Três consultas simultâneas com conversas independentes: 0,274 s, 0,344 s e 0,287 s; lote concluído em 0,348 s.
- Exportação de período grande cancelada sem disponibilizar arquivo parcial.
- Preparação do pacote repetida duas vezes, com lista de arquivos e hashes verificados nas duas execuções.

Nenhum registro do MongoDB foi alterado. Escritas de configuração, histórico, exportação e recuperação foram feitas em cópias isoladas dentro de `.release-build`.

## Defeitos encontrados e corrigidos

1. Esclarecer o vendedor podia perder uma data explicitamente informada antes dele.
2. Intervalos excessivos podiam causar uma exceção não tratada; a resposta a um esclarecimento também precisava ser revalidada.
3. Referências como “naquele dia” podiam usar indevidamente um contexto que abrangia um mês inteiro.
4. A interpretação de linguagem podia trocar Qualidade por Investigação ao escolher o perfil nativo.
5. Comparação somente por empresa pedia um período adicional sem necessidade.
6. Cancelamento de exportação enfileirada ainda podia iniciar consultas; atualizações de progresso/cancelamento também precisavam ser atômicas.
7. Mesmo valor e quantidade de documentos em dias diferentes podiam suprimir um novo alerta. A assinatura agora inclui o período.
8. O assistente aceitava banco vazio/inexistente como configuração bem-sucedida.
9. Consultas anuais carregavam documentos completos pela expansão de itens, chegando a estourar o tempo limite. A projeção agora retém apenas os campos necessários. A investigação reutiliza o valor calculado de cada item em vez de repetir a fórmula.
10. Repetir a preparação do ZIP podia incluir o hash anterior de SHA256SUMS no próprio manifesto. O manifesto agora exclui a si mesmo.

Os testes pertinentes foram mantidos como regressões. Os cálculos financeiros continuaram compatíveis com as verificações independentes existentes.

## Medição controlada do executável

Máquina disponível: Windows 11, build 26200. Base existente: **69.206 movimentações**; **32.548** datadas de 2026. As consultas anuais encontraram **31.601 documentos** após os filtros de vendas e situação.

| Consulta | Documentos | Repetições | Mediana | Pior medição | Orçamento aplicado | Resultado |
|---|---:|---:|---:|---:|---:|---|
| Investigação de 05/10 | 116 | 3 | 0,218 s | 0,245 s | 20 s | Aprovado |
| Faturamento de setembro | 3.278 | 3 | 0,815 s | 0,825 s | 5 s | Aprovado |
| Faturamento de 2026 até 26/10 | 31.601 | 2 | 6,512 s | 6,562 s | 20 s, período amplo | Aprovado |
| Investigação de 2026 até 26/10 | 31.601 | 2 | 16,908 s | 16,916 s | 20 s | Aprovado |

O orçamento de 5 s foi aplicado à consulta mensal usual. A consulta anual foi tratada como consulta ampla, com orçamento de 20 s; **não** significa que o faturamento anual tenha atingido 5 s. São amostras pequenas e locais, não percentis de produção nem um teste prolongado de carga. As medições controladas foram executadas depois de encerrar as suítes concorrentes.

No diagnóstico anterior à otimização, a primeira agregação anual de faturamento consumia aproximadamente 13,5 s. Após limitar a projeção antes da expansão, a mesma etapa foi observada em aproximadamente 3,6 s; o tempo total do endpoint está na tabela.

## Limites da conclusão e gates restantes

- **Sistema operacional limpo não testado:** Windows Sandbox não está disponível. Extração isolada e PATH sem Python não substituem uma máquina/VM limpa; Windows 10 também não foi homologado aqui.
- **Aceitação da equipe não executada:** a validação automatizada e a revisão de interface não substituem o uso pelos operadores de suporte com seus casos reais.
- **Volume previsto não comprovado:** a base contém 69 mil movimentos no total, mas não possui 50 mil vendas em um único mês. O teste máximo desta rodada foi de 31.601 documentos correspondentes. Não extrapolar os tempos para 50 mil/mês ou outras máquinas.
- **Conta real somente leitura:** a recusa de conta com privilégios amplos foi verificada no executável, e o caminho positivo foi coberto por testes controlados. A conexão positiva com uma conta real dedicada com role `read` precisa ser homologada pelo administrador no ambiente de destino.

Com esses gates pendentes, a entrega correta é RC2 pronto para homologação, sem promessa de ausência total de defeitos.

## Reprodução

Instalar dependências de desenvolvimento de `requirements-dev.txt` e executar as suítes listadas em `validacao-1.0.md`, acrescentando `test_stability` ao comando unittest. O teste integrado exige a base de homologação usada nos casos conhecidos.

`tools/verify-release.py` gera o relatório técnico ligado ao hash do candidato. `finalize-release.ps1 -PrepareOnly` prepara um pacote isolado, sem substituir a instalação em uso. `tools/check-release-package.py --release-dir <pasta>` verifica manifesto e conteúdo. `tools/stability-check.py --archive <zip>` executa instalação, recuperação, rollback, concorrência e medição de desempenho em cópias locais.

Evidência bruta desta rodada: `.release-build/stability-20261026-115502-de1764/results.json`; confirmação do executável e exports: `.release-build/build-20261026-114556-573/verification/verification.json`. Esses arquivos de teste não entram no ZIP de entrega.
