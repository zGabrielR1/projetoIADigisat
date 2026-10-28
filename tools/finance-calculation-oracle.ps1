param([string]$OutputPath = "$PSScriptRoot\..\tests\fixtures\finance-calculation-oracle.json")
$ErrorActionPreference = 'Stop'
# Independent C# reconstruction of the arithmetic in the installed decompilation.
# This compiles only the code below. It does not load or execute SuiteG6 DLLs.
Add-Type -TypeDefinition @'
using System;
using System.Globalization;
public static class InsightFinanceOracle {
    public static double Period(DateTime start, DateTime end) {
        start = start.AddDays(-1); end = end.AddDays(-1);
        int firstDays = DateTime.DaysInMonth(start.Year, start.Month);
        int lastDays = DateTime.DaysInMonth(end.Year, end.Month);
        int months;
        if (start.Year == end.Year && start.Month == end.Month) months = -1;
        else {
            int days = (int)((end.Date - start.Date).TotalDays - ((firstDays - start.Day) + end.Day));
            decimal count = Math.Truncate(days / 365m) * 12m;
            decimal remaining = Math.Truncate(days % 365m);
            if (remaining >= 28m) count += (int)Math.Truncate(remaining / 30m);
            if (remaining % 30m >= 28m) count++;
            months = (int)count;
        }
        return Math.Round((double)(firstDays - start.Day) / firstDays + months + (double)end.Day / lastDays, 4, MidpointRounding.AwayFromZero);
    }
    public static string Simple(DateTime start, DateTime end) {
        if (start.Date >= end.Date) return "0.00";
        return Math.Round(1234.56m * (2.35m / 100m) * (decimal)Period(start, end), 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture);
    }
    public static string Compound(DateTime start, DateTime end) {
        if (start.Date >= end.Date) return "0.00";
        decimal growth = (decimal)(Math.Pow((double)(1m + 2.35m / 100m), Period(start,end)) - 1d);
        return Math.Round(1234.56m * growth, 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture);
    }
}
'@
$vectors = [Collections.Generic.List[object]]::new()
foreach ($year in @(2024,2026)) {
    foreach ($month in 1..12) {
        foreach ($day in @(1,15,28)) {
            $start = [datetime]::new($year,$month,$day)
            foreach ($delta in @(-1,0,1,30,365,800)) {
                $end = $start.AddDays($delta)
                $vectors.Add([ordered]@{
                    start=$start.ToString('yyyy-MM-dd'); end=$end.ToString('yyyy-MM-dd');
                    period=[InsightFinanceOracle]::Period($start,$end).ToString('F4',[Globalization.CultureInfo]::InvariantCulture);
                    simple=[InsightFinanceOracle]::Simple($start,$end);
                    compound=[InsightFinanceOracle]::Compound($start,$end)
                })
            }
        }
    }
}
$target = [IO.Path]::GetFullPath($OutputPath)
[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($target)) | Out-Null
$payload = [ordered]@{ evidence_type='independent_csharp_reconstruction'; target_executed=$false; clr=[Environment]::Version.ToString(); principal='1234.56'; percent='2.35'; vectors=$vectors }
[IO.File]::WriteAllText($target,($payload | ConvertTo-Json -Depth 6),[Text.UTF8Encoding]::new($false))
Write-Output "C# arithmetic vectors: $($vectors.Count)"
