# DigiSat Insight 1.1.0 RC5

- Financial BI now opens underlying parcels from overdue ranges and agenda amounts, in addition to customer and supplier drill-down.
- Detail views show their due-date window, calculation reference, complete filtered total and visible-page subtotal.
- Page CSV export includes the applied scope and source, preserves displayed cents, and escapes spreadsheet formula text.
- Phone parcel details use readable rows containing the person, company, amount, document, due date and account.
- Small UI refinements clarify clickable amounts, page position and export row counts, and update stale release labels.
- Overnight reference changes require an overview refresh before reusing aging ranges.

Validated with focused BI tests, an isolated MongoDB case covering 55 parcels across pages, actual-data CSV downloads, desktop/tablet/phone reviews in both themes, and packaged Windows release checks.

Upgrade using the Windows ZIP while retaining your existing config.env, password file and local data folder.
