# Visualising AI Job Exposure and Risk Across the UK and EU Markets

An interactive dashboard visualising AI disruption risk across ~130 occupations in the UK and EU-27. Each occupation is scored 0–10 for AI exposure and mapped against real employment trend data, letting you see not just which jobs *could* be affected — but which ones *already are*.

## What it shows

The dashboard presents occupations as a treemap where **node area = number of workers employed**. Two view modes let you explore the data differently:

- **AI Exposure** — nodes coloured on a green → yellow → red gradient by AI exposure score
- **Trend Signal** — nodes recoloured by a derived signal that cross-references exposure with actual 3-year employment change

### Trend signals

| Signal | Meaning |
|---|---|
| **Confirming** (red) | High AI exposure (≥7.0) AND employment already declining (≤−3%) |
| **Lagging** (amber) | High AI exposure but employment still stable or growing — disruption not yet reflected in data |
| **Diverging** (purple) | Low AI exposure (≤4.0) but employment falling sharply (≤−5%) — other factors driving decline |
| **Neutral** (grey) | Everything else |

Clicking a signal card in the sidebar filters the treemap to show only occupations in that category. Hover any node for a full breakdown including salary, growth outlook, rationale, and data source.

### Sidebar stats

The sidebar shows aggregate statistics for the selected region:

- Total jobs covered and workforce coverage %
- Employment-weighted average AI exposure score
- Exposure histogram by score band
- Tier breakdown (Minimal / Low / Moderate / High / Very High)
- Average exposure by pay bracket and education level
- Annual wages at risk (sum of wages in occupations scoring 7+)

## Regions

Toggle between **UK** (sourced from ONS LFS 2021–2024) and **EU-27** (Eurostat LFS 2021–2024).

## Tech stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 9 Minimal APIs (.NET 9) |
| Frontend | Vanilla JS, [Highcharts 11.3](https://www.highcharts.com/) (treemap) |
| Data | Fully static — hardcoded C# records, no database |

## Running locally

```bash
dotnet run
```

Opens at `https://localhost:52270` (or `http://localhost:52271`).

## Data notes

All occupation data is hardcoded in `UkOccupationData.cs` (UK) and `EuOccupationData.cs` (EU-27). AI exposure scores are researcher-assigned estimates on a 0–10 scale. Employment figures are in thousands. Three-year employment change percentages are drawn from ONS / Eurostat Labour Force Survey data.

The dataset covers a sample of occupations — it is not exhaustive of every role in either economy.

## License

MIT
