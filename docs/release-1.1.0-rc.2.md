# DigiSat Insight 1.1.0 RC2

- Financial BI: pending receivables/payables, overdue amounts, aging bands and due-date agenda.
- Customer and supplier rankings with paginated parcel drill-down.
- Date, company and account filters; full-scope totals before pagination.
- Pulse design in dark and light themes with mobile layouts.

Connection setup: the portable executable reads config.env beside the executable. MONGO_PASSWORD_FILE also requires MONGO_USERNAME. INSIGHT_REQUIRE_READ_ONLY=1 requires a dedicated MongoDB account whose privileges are limited to read; supplying a password does not change account roles.
