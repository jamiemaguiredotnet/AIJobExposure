namespace AIJobExposure.Models;

public record Occupation(
    string Id,
    string Name,
    string Sector,
    string SectorId,
    string SectorColour,
    int EmploymentThousands,
    double AiExposureScore,
    string ExposureTier,
    string Rationale,
    string MedianAnnualSalary,
    int MedianSalaryNumeric,
    string EducationLevel,
    double EmploymentChange3Yr,   // % change over 3 years (ONS/Eurostat)
    string GrowthOutlook,
    string DataSource
);

public record OccupationStats(
    string Region,
    long TotalJobsThousands,
    double WeightedAverageExposure,
    long WagesExposedBillions,
    TierBreakdown TierBreakdown,
    List<HistogramBin> ExposureHistogram,
    List<PayBracketStat> ExposureByPay,
    List<EducationStat> ExposureByEducation,
    TrendSignalBreakdown TrendSignals,
    List<OccupationSummary> TopExposed,
    List<OccupationSummary> LeastExposed
);

public record TierBreakdown(
    TierStat Minimal, TierStat Low, TierStat Moderate, TierStat High, TierStat VeryHigh);

public record TierStat(int Count, long EmploymentThousands, double Percentage);
public record OccupationSummary(string Name, string Sector, double Score, int EmploymentThousands);
public record HistogramBin(string Label, int ScoreFloor, long EmploymentThousands);
public record PayBracketStat(string Bracket, double AvgExposure, long EmploymentThousands);
public record EducationStat(string Level, double AvgExposure, long EmploymentThousands);

public record TrendSignalBreakdown(
    TrendSignalStat Confirming,   // high exposure + declining employment
    TrendSignalStat Lagging,      // high exposure + stable/growing employment
    TrendSignalStat Diverging,    // low exposure + declining employment (other factors)
    TrendSignalStat Neutral       // everything else
);

public record TrendSignalStat(int Count, long EmploymentThousands, List<string> Examples);
