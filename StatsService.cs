using AIJobExposure.Models;

namespace AIJobExposure.Data;

public static class StatsService
{
    static readonly (string Label, int Min, int Max)[] PayBrackets =
    {
        ("<£/€25k",   0,     24999),
        ("£/€25–35k", 25000, 34999),
        ("£/€35–50k", 35000, 49999),
        ("£/€50–75k", 50000, 74999),
        ("£/€75k+",   75000, int.MaxValue),
    };

    static readonly string[] EduLevels =
        { "No degree/HS", "Post-16/Vocational", "Bachelor's", "Master's", "Doctoral/Prof" };

    // Signal thresholds
    const double HighExposure        =  7.0;
    const double LowExposure         =  4.0;
    const double ConfirmingThreshold = -3.0;   // declining if change <= this
    const double DivergingThreshold  = -5.0;   // diverging if low-exposure AND change <= this

    public static string ComputeSignal(double score, double change3Yr)
    {
        if (score >= HighExposure && change3Yr <= ConfirmingThreshold) return "Confirming";
        if (score >= HighExposure && change3Yr >  ConfirmingThreshold) return "Lagging";
        if (score <= LowExposure  && change3Yr <= DivergingThreshold)  return "Diverging";
        return "Neutral";
    }

    public static OccupationStats Calculate(string region, List<Occupation> occupations)
    {
        long totalJobs   = occupations.Sum(o => (long)o.EmploymentThousands);
        double weightedExp = occupations.Sum(o => o.AiExposureScore * o.EmploymentThousands) / totalJobs;

        long wagesExposed = occupations
            .Where(o => o.AiExposureScore >= 7.0)
            .Sum(o => (long)o.MedianSalaryNumeric * o.EmploymentThousands) / 1_000_000L;

        TierStat BuildTier(double min, double max)
        {
            var g   = occupations.Where(o => o.AiExposureScore >= min && o.AiExposureScore <= max).ToList();
            long emp = g.Sum(o => (long)o.EmploymentThousands);
            return new TierStat(g.Count, emp, totalJobs == 0 ? 0 : Math.Round(emp * 100.0 / totalJobs, 1));
        }

        var histogram = Enumerable.Range(0, 10).Select(i =>
        {
            long emp = occupations
                .Where(o => o.AiExposureScore >= i && o.AiExposureScore < i + 1)
                .Sum(o => (long)o.EmploymentThousands);
            return new HistogramBin($"{i}–{i+1}", i, emp);
        }).ToList();

        var byPay = PayBrackets.Select(b =>
        {
            var g    = occupations.Where(o => o.MedianSalaryNumeric >= b.Min && o.MedianSalaryNumeric <= b.Max).ToList();
            long emp  = g.Sum(o => (long)o.EmploymentThousands);
            double avg = emp == 0 ? 0 : g.Sum(o => o.AiExposureScore * o.EmploymentThousands) / (double)emp;
            return new PayBracketStat(b.Label, Math.Round(avg, 1), emp);
        }).ToList();

        var byEdu = EduLevels.Select(lvl =>
        {
            var g    = occupations.Where(o => o.EducationLevel == lvl).ToList();
            long emp  = g.Sum(o => (long)o.EmploymentThousands);
            double avg = emp == 0 ? 0 : g.Sum(o => o.AiExposureScore * o.EmploymentThousands) / (double)emp;
            return new EducationStat(lvl, Math.Round(avg, 1), emp);
        }).ToList();

        // Trend signals
        TrendSignalStat BuildSignal(string signal)
        {
            var g    = occupations.Where(o => ComputeSignal(o.AiExposureScore, o.EmploymentChange3Yr) == signal).ToList();
            long emp  = g.Sum(o => (long)o.EmploymentThousands);
            var examples = g.OrderByDescending(o => o.EmploymentThousands).Take(3).Select(o => o.Name).ToList();
            return new TrendSignalStat(g.Count, emp, examples);
        }

        return new OccupationStats(
            Region: region,
            TotalJobsThousands: totalJobs,
            WeightedAverageExposure: Math.Round(weightedExp, 1),
            WagesExposedBillions: wagesExposed,
            TierBreakdown: new TierBreakdown(
                Minimal:  BuildTier(0.0, 2.0),
                Low:      BuildTier(2.1, 4.0),
                Moderate: BuildTier(4.1, 6.0),
                High:     BuildTier(6.1, 8.0),
                VeryHigh: BuildTier(8.1, 10.0)
            ),
            ExposureHistogram: histogram,
            ExposureByPay: byPay,
            ExposureByEducation: byEdu,
            TrendSignals: new TrendSignalBreakdown(
                Confirming: BuildSignal("Confirming"),
                Lagging:    BuildSignal("Lagging"),
                Diverging:  BuildSignal("Diverging"),
                Neutral:    BuildSignal("Neutral")
            ),
            TopExposed: occupations
                .OrderByDescending(o => o.AiExposureScore)
                .Take(5)
                .Select(o => new OccupationSummary(o.Name, o.Sector, o.AiExposureScore, o.EmploymentThousands))
                .ToList(),
            LeastExposed: occupations
                .OrderBy(o => o.AiExposureScore)
                .Take(5)
                .Select(o => new OccupationSummary(o.Name, o.Sector, o.AiExposureScore, o.EmploymentThousands))
                .ToList()
        );
    }
}
