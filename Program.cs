using AIJobExposure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
        ctx.Context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate"
});

app.MapGet("/api/occupations", (string? region) =>
{
    var occs = (region?.ToLower() == "eu") ? EuOccupationData.Get() : UkOccupationData.Get();
    // Enrich with computed trend signal before serialising
    var result = occs.Select(o => new
    {
        o.Id, o.Name, o.Sector, o.SectorId, o.SectorColour,
        o.EmploymentThousands, o.AiExposureScore, o.ExposureTier, o.Rationale,
        o.MedianAnnualSalary, o.MedianSalaryNumeric, o.EducationLevel,
        o.EmploymentChange3Yr, o.GrowthOutlook, o.DataSource,
        TrendSignal = StatsService.ComputeSignal(o.AiExposureScore, o.EmploymentChange3Yr)
    });
    return Results.Ok(result);
});

app.MapGet("/api/stats", (string? region) =>
{
    var occs  = (region?.ToLower() == "eu") ? EuOccupationData.Get() : UkOccupationData.Get();
    var stats = StatsService.Calculate(region?.ToUpper() ?? "UK", occs);
    return Results.Ok(stats);
});

app.Run();
