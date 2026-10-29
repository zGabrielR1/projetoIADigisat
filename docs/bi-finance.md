# Financial BI

Open **Business intelligence → Financeiro** (`/bi-finance`). Select a due-date range, company and account to inspect pending receivables and payables. The initial range is today minus 365 days through today plus 30 days. Older pending titles are outside that range.

The overview shows corrected parcel amounts, overdue amounts, seven aging bands, the first seven future dates with pending parcels, and up to eight customers/suppliers ranked by amount. Select a person or **Ver parcelas** to read the underlying parcels, 50 per page. Pagination queries the source again; concurrent source changes can affect results.

`POST /api/bi/finance/overview` and `/parcels` use the existing authenticated finance engine and its history, interest, penalty, fee, record-count and timeout rules. All source access is read-only. BI aggregates the complete filtered set before pagination and sums rounded parcel cents. Individual source failures remain unavailable; an unavailable source is never represented as zero. These figures are neither bank balances nor historical snapshots. Native output homologation remains pending.

Validation: 53 finance/BI unit and isolated MongoDB tests passed. Live read-only checks confirmed customer drill-down. Browser checks covered 1440, 768 and 390 pixels in dark and light modes, and a simulated unavailable payable source. Taste inspection reported zero failures; remaining density and unequal panel-height warnings reflect secondary metadata and honest empty-source content. Screenshots are local ignored QA artifacts.
