# Connection checks

Open **Configurar conexão** to see the configuration path, whether a password file is configured, and whether that file exists. The form uses the current runtime settings; variables supplied before startup take priority over configuration files.

**Testar conexão** checks credential preparation, the MongoDB session, database access and read-only permissions separately. A failed step leaves subsequent checks marked as not executed. When strict read-only verification is enabled, broader or unverifiable permissions prevent approval. With verification optional, those permissions remain marked for review.

The test does not save configuration. Changing a form field clears the old result. **Testar e salvar** performs a fresh check before applying settings. **Copiar diagnóstico sem senha** copies the tested server, database, username, authentication database, policy and check results; it excludes password values and connection strings. Share it with the database administrator when requesting a dedicated account with the `read` role.

The installation diagnostics also display the configuration path and password-file status.

Validation: six mocked inspection scenarios and eight local workspace tests pass. Browser checks covered a reachable database with broader account privileges, strict verification, a closed loopback port, result invalidation after edits, clipboard copy, and unchanged configuration after testing. Screenshots were reviewed at 1440, 768 and 390 pixels in both themes without dialog overflow.
