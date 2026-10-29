# DigiSat Insight 1.1.0 RC4

- New light theme across all seven screens: white cards and sidebar, a neutral gray canvas, deep teal actions/navigation, and muted green chart data inspired by the supplied light-mode reference.
- Consistent styling for reports, BI, financial BI, database editor, menus and dialogs. Dark mode keeps the approved Pulse palette.
- Fixed the financial report selector overflowing smaller screens.
- Connection assistant shows configuration path and password-file availability, plus separate credential, MongoDB session, database access and read-only permission checks.
- Diagnostic results can be copied without the password. Testing does not save settings; editing a connection field clears the old result.

Validation includes desktop, tablet and phone browser reviews, light/dark style comparison, focused connection inspection tests and the packaged executable release checks.

Upgrade: extract the Windows ZIP and retain your existing config.env, password file and local data folder. If strict read-only verification rejects your MongoDB account, use a dedicated account with the read role on the selected database. A password file does not change account privileges.
