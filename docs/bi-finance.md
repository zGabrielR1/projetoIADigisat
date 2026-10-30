# Financial BI

Open **Business intelligence → Financeiro** (`/bi-finance`). Select a due-date range, company and account to inspect pending receivables and payables. The initial range is today minus 365 days through today plus 30 days. Older pending titles are outside that range.

The overview shows corrected parcel amounts, overdue amounts, seven aging bands, the first seven future dates with pending parcels, and up to eight customers/suppliers ranked by amount. Select an aging-band amount, an agenda amount, a person or **Ver parcelas** to read the underlying parcels, 50 per page. Empty bands remain zero and unavailable sources remain unavailable. Pagination queries the source again; concurrent source changes can affect results.

The detail shows the exact due-date range, reference date, total for the complete detail scope and subtotal for the visible page. On phones, parcel rows become readable lists with the person, company, corrected amount, document, due date and account. **CSV desta página** exports only the displayed page, with source and scope metadata and no conversion of source text into spreadsheet formulas. Refresh the overview when its reference date has changed overnight; drill requests from an older reference date are rejected rather than silently reclassified.

`POST /api/bi/finance/overview` and `/parcels` use the existing authenticated finance engine and its history, interest, penalty, fee, record-count and timeout rules. All source access is read-only. BI aggregates the complete filtered set before pagination and sums rounded parcel cents. Individual source failures remain unavailable; an unavailable source is never represented as zero. These figures are neither bank balances nor historical snapshots. Native output homologation remains pending.

Missing or malformed parcel due dates reject the entire affected source analysis with a validation error. The overview keeps the other source available and never displays a partial total for the rejected source. Regression coverage includes empty, malformed, impossible and non-text dates after a valid parcel.

Validation: 53 finance/BI unit and isolated MongoDB tests passed. Live read-only checks confirmed customer drill-down. Browser checks covered 1440, 768 and 390 pixels in dark and light modes, and a simulated unavailable payable source. Taste inspection reported zero failures; remaining density and unequal panel-height warnings reflect secondary metadata and honest empty-source content. Screenshots are local ignored QA artifacts.

Drill-down increment: 17 focused BI and isolated MongoDB tests passed, including seven aging boundaries, scope intersection, stale reference rejection and pagination across 55 parcels. Downloaded CSV values reconciled with the visible detail; formula-looking text, quotes, delimiters and embedded newlines were checked. Range, date and person actions and the mobile parcel list were reviewed in both themes.
