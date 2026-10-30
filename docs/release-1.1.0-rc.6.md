# DigiSat Insight 1.1.0 RC6

- Sales dashboards now let you select any day from the chart or date list, compare its revenue or document count with the corresponding previous-period day, and open its underlying documents.
- Daily comparisons use displayed cents, preserve unavailable values and avoid percentage changes from zero or negative baselines. Changed filters require a refresh before opening daily documents.
- The sales layout gives the trend and products more room, keeps reconciliation and company panels sized to their content, and highlights reconciliation differences.
- Rankings show complete names, aligned values and positions. Phone indicators keep monetary values on one line. Both approved themes and local offline assets are preserved.
- Financial BI treats malformed due dates as an unavailable source, preserving the other source without returning partial totals.

Validated on desktop, tablet and phone in dark and light modes, plus live product/company/day exploration. All packaged Windows release gates passed, covering BI, financial calculations, native reports, database editor, exports, authentication and restart persistence. The portable ZIP passed its file allowlist, credential-free configuration, Windows x64 and SHA-256 checks.

Upgrade using the Windows ZIP while retaining your existing config.env, password file and local data folder.
