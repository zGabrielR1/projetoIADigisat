// Laboratory reproduction of the recovered monthly aggregation pattern.
// No SuiteG6 assembly, database, network, or customer data is used.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

class MonthlyArrayRace
{
    const int Months = 12;
    const int PageSize = 500;
    const int Documents = 60000;
    const decimal Amount = 1.01m;

    static decimal[] Aggregate(int degree, bool immutable)
    {
        var global = new ConcurrentDictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase);
        for (int skip = 0; skip < Documents; skip += PageSize)
        {
            Parallel.ForEach<int, Dictionary<string, decimal[]>>(
                Enumerable.Range(skip, Math.Min(PageSize, Documents - skip)),
                new ParallelOptions { MaxDegreeOfParallelism = degree },
                () => new Dictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase),
                (document, loop, local) => {
                    decimal[] values;
                    if (!local.TryGetValue("same-seller", out values))
                        local["same-seller"] = values = new decimal[Months];
                    values[document % Months] += Amount;
                    return local;
                },
                local => {
                    foreach (var pair in local)
                        global.AddOrUpdate(pair.Key, pair.Value, (key, existing) => {
                            if (immutable)
                            {
                                var next = new decimal[Months];
                                for (int month = 0; month < Months; month++)
                                    next[month] = existing[month] + pair.Value[month];
                                return next;
                            }
                            // This is the unsafe recovered merge, uninstrumented.
                            for (int month = 0; month < Months; month++)
                                existing[month] += pair.Value[month];
                            return existing;
                        });
                });
        }
        return global["same-seller"];
    }

    // A separate, explicitly instrumented schedule makes the read-modify-write
    // overlap visible. It is not a claim that SuiteG6 uses these barriers.
    static decimal ControlledReadModifyWrite()
    {
        var global = new ConcurrentDictionary<string, decimal[]>();
        global["same-seller"] = new decimal[Months];
        using (var bothRead = new Barrier(2))
        {
            Action update = () => global.AddOrUpdate("same-seller", new decimal[Months], (key, existing) => {
                decimal before = existing[0];
                bothRead.SignalAndWait();
                existing[0] = before + 1m;
                return existing;
            });
            var first = Task.Factory.StartNew(update, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            var second = Task.Factory.StartNew(update, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Task.WaitAll(first, second);
        }
        return global["same-seller"][0];
    }

    static object RunMode(string name, int degree, bool immutable, int trials)
    {
        decimal expected = (Documents / Months) * Amount;
        var samples = new List<object>();
        int failures = 0;
        decimal min = decimal.MaxValue, max = decimal.MinValue;
        var watch = Stopwatch.StartNew();
        for (int trial = 0; trial < trials; trial++)
        {
            decimal[] actual = Aggregate(degree, immutable);
            var wrong = Enumerable.Range(0, Months).Where(i => actual[i] != expected).ToArray();
            if (wrong.Length > 0)
            {
                failures++;
                if (samples.Count < 6) samples.Add(new { trial = trial + 1, month_totals = actual, differing_months = wrong.Select(i => i + 1).ToArray() });
            }
            decimal total = actual.Sum();
            min = Math.Min(min, total); max = Math.Max(max, total);
        }
        return new { name, degree, trials, divergent_runs = failures, expected_month_total = expected,
            expected_year_total = Documents * Amount, minimum_year_total = min, maximum_year_total = max,
            elapsed_ms = watch.ElapsedMilliseconds, divergent_samples = samples };
    }

    static int Main(string[] args)
    {
        int trials = args.Length > 0 ? int.Parse(args[0]) : 40;
        int productionDegree = Math.Max(2, Environment.ProcessorCount / 2);
        var result = new {
            scope = "Synthetic laboratory reproduction of an aggregation pattern; not execution of SuiteG6 and not proof of the customer's root cause.",
            utc = DateTime.UtcNow.ToString("o"), runtime = Environment.Version.ToString(),
            processor_count = Environment.ProcessorCount, document_count = Documents, page_size = PageSize,
            seller_count = 1, amount_per_document = Amount,
            modes = new object[] {
                RunMode("serial-control", 1, false, 3),
                RunMode("recovered-mutable-array", productionDegree, false, trials),
                RunMode("corrected-new-array", productionDegree, true, trials)
            },
            controlled_schedule = new {
                instrumentation = "Two synthetic workers pause after reading the same existing array cell, then each stores old+1. The original += operation is split solely to force an allowed interleaving.",
                expected = 2m, actual = ControlledReadModifyWrite()
            },
            limitations = new[] {
                "Only the local aggregation and merge shape is reproduced; filtering, DTOs, Mongo paging, runtime scheduling and report rendering are not reproduced.",
                "No customer records or installed application code is loaded or changed.",
                "Natural runs can pass by chance. A successful mutable run does not establish thread safety.",
                "Corrected merge allocates a new array and does not mutate an existing dictionary value; caller code must preserve that invariant."
            }
        };
        Console.WriteLine(new JavaScriptSerializer().Serialize(result));
        return 0;
    }
}
