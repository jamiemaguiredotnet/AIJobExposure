# Visualising AI Job Exposure and Risk Across the UK and EU Markets

<img width="1912" height="948" alt="image" src="https://github.com/user-attachments/assets/c87299bc-3f11-4eeb-93ca-26cd114f956d" />

&nbsp;

An interactive dashboard visualising AI disruption risk across ~130 occupations in the UK and EU-27. Each occupation is scored 0–10 for AI exposure and mapped against real employment trend data, letting you see not just which jobs *could* be affected — but which ones *already are*.

Most AI-and-jobs analysis stops at exposure: *which roles could AI do?* This project adds a second dimension — **are those roles actually declining?** The trend signal layer cross-references AI exposure scores against real 3-year employment change data from ONS and Eurostat, surfacing the gap between theoretical risk and observed labour market movement.

&nbsp;

Live site: https://jamiemaguire.net/jobs

Blog: https://jamiemaguire.net/index.php/2026/03/21/visualising-ai-job-exposure-and-risk-across-the-uk-and-eu-markets/

&nbsp;

## What it shows

The dashboard presents occupations as a treemap where **node area = number of workers employed**. Two view modes let you explore the data differently:

- **AI Exposure** — nodes coloured on a green → yellow → red gradient by AI exposure score
- **Trend Signal** — nodes recoloured by a derived signal that cross-references exposure with actual 3-year employment change

&nbsp;

### Trend signals

| Signal | Meaning |
|---|---|
| **Confirming** (red) | High AI exposure (≥7.0) AND employment already declining (≤−3%) |
| **Lagging** (amber) | High AI exposure but employment still stable or growing — disruption not yet reflected in data |
| **Diverging** (purple) | Low AI exposure (≤4.0) but employment falling sharply (≤−5%) — other factors driving decline |
| **Neutral** (grey) | Everything else |

Clicking a signal card in the sidebar highlights matching occupations in the treemap — non-matching nodes are dimmed. Click again or use "Show all" to clear. Hover any node for a full breakdown including salary, growth outlook, rationale, and data source.

&nbsp;

### Sidebar stats

The sidebar shows aggregate statistics for the selected region:

- Total jobs covered and workforce coverage %
- Employment-weighted average AI exposure score
- Exposure histogram by score band
- Tier breakdown (Minimal / Low / Moderate / High / Very High)
- Average exposure by pay bracket and education level
- Annual wages at risk (sum of wages in occupations scoring 7+)

&nbsp;

## Regions

Toggle between **UK** (sourced from ONS LFS 2021–2024) and **EU-27** (Eurostat LFS 2021–2024).

&nbsp;

## Tech stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 9 Minimal APIs (.NET 9) |
| Frontend | Vanilla JS, [Highcharts 11.3](https://www.highcharts.com/) (treemap) |
| Data | Fully static — hardcoded C# records, no database |

&nbsp;

## Running locally

```bash
dotnet run
```

Opens at `https://localhost:52270` (or `http://localhost:52271`).

&nbsp;

## Interpreting the scores

A few things the AI exposure score does **not** mean:

- **High exposure ≠ job elimination.** Software developers score 8–9/10 because AI transforms almost every part of their workflow — yet demand for developers is growing. Exposure measures how much of a role AI can touch, not whether that role will shrink.
- **The score ignores demand elasticity.** A role can be highly automatable and still grow if the cost reduction unlocks new demand (e.g., cheaper code generation drives more software projects).
- **Regulatory and social factors are excluded.** Healthcare and legal roles face high theoretical exposure but are insulated by licensing, liability, and patient/client expectations.
- **Employment change data has lag.** Labour markets adjust slowly. A "Lagging" signal means the exposure risk hasn't shown up in the numbers *yet* — not that it won't.

The most actionable signals are **Confirming** (high exposure + real decline) and **Diverging** (declining despite low AI exposure, suggesting other structural forces at work).

&nbsp;

## Data notes

All occupation data is hardcoded in `UkOccupationData.cs` (UK) and `EuOccupationData.cs` (EU-27). AI exposure scores are researcher-assigned estimates on a 0–10 scale. Employment figures are in thousands. Three-year employment change percentages are drawn from ONS / Eurostat Labour Force Survey data.

The dataset covers a sample of occupations — it is not exhaustive of every role in either economy.

Unlike Karpathy's tool, which scores occupations dynamically, this is a point-in-time snapshot. The data does not refresh automatically — scores and employment figures reflect the state at the time of last update.

&nbsp;

## License

MIT
