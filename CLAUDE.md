# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**AI Job Exposure** — an interactive dashboard visualising AI exposure risk across ~130 UK and EU-27 occupations. Built with ASP.NET Core 9 Minimal APIs serving a vanilla JS / Highcharts single-page app.

## Commands

```bash
dotnet build          # Build the project
dotnet run            # Run dev server (https://localhost:52270, http://localhost:52271)
```

There are no automated tests in this project.

## Architecture

### Backend (`Program.cs` → `StatsService.cs`)

Two Minimal API endpoints:

| Route | Purpose |
|---|---|
| `GET /api/occupations?region=[uk\|eu]` | Returns all occupations with computed trend signals |
| `GET /api/stats?region=[uk\|eu]` | Returns aggregated statistics (tier breakdowns, histograms, pay/education analysis) |

Data is **fully static and hardcoded** — no database. `UkOccupationData.cs` contains UK occupations; `EuOccupationData.cs` contains EU-27 occupations (despite the reversed naming).

**Trend signal logic** (`StatsService.ComputeSignal`):
- **Confirming** — AI exposure ≥ 7.0 AND employment change ≤ −3%
- **Lagging** — AI exposure ≥ 7.0 AND employment stable/growing
- **Diverging** — AI exposure ≤ 4.0 AND employment change ≤ −5%
- **Neutral** — everything else

### Frontend (`wwwroot/index.html`)

Single HTML file (~571 lines). No framework — vanilla JS with [Highcharts 11.3](https://code.highcharts.com/) loaded from CDN.

**View modes:**
- **AI Exposure** — treemap where node area = employment, colour = exposure score (green → red gradient)
- **Trend Signal** — same treemap recoloured by signal category (Confirming/Lagging/Diverging/Neutral)

Trend signal cards in the sidebar act as click filters, highlighting matching nodes in the treemap.

### Data Model (`Occupation.cs`)

Key fields on the `Occupation` record: `Id`, `Name`, `Sector`, `SectorId`, `SectorColour`, `EmploymentThousands`, `AiExposureScore` (0–10), `ExposureTier`, `MedianAnnualSalary`, `MedianSalaryNumeric`, `EducationLevel`, `EmploymentChange3Yr` (% over 3 years), `GrowthOutlook`, `Rationale`, `DataSource`.

## Colour & Design

- Dark theme: background `#0f1117`
- Exposure gradient: green (low) → yellow (medium) → red (high)
- Signal colours: Confirming `#ef4444`, Lagging `#f97316`, Diverging `#a855f7`, Neutral `#6b7280`
