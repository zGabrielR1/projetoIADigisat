# DigiSat Insight 1.1.0 RC3

- MongoDB connection errors now distinguish accepted credentials with broader account privileges from permissions that could not be verified.
- Reports and the connection assistant share password-file parsing and resolve relative paths beside the application.
- Connection diagnostics include the configuration path and whether the password file exists, without returning its contents.
- Setup guidance explains config.env for the portable executable and .env for source execution.
- Includes the financial BI dashboard and parcel exploration from RC2.

Extract the Windows ZIP and keep your local config.env and password file beside the executable. A strict read-only connection requires a dedicated MongoDB account with the read role; a password file does not change account privileges.
