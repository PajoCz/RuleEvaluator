# RuleEvaluator

[![CI](https://github.com/PajoCz/RuleEvaluator/actions/workflows/ci.yml/badge.svg)](https://github.com/PajoCz/RuleEvaluator/actions/workflows/ci.yml)

| Item | Link |
|:-----|:-----|
| NuGet | [![NuGet version (RuleEvaluator)](https://img.shields.io/nuget/v/RuleEvaluator.svg?style=flat-square)](https://www.nuget.org/packages/RuleEvaluator/) |
| NuGet Repository | [![NuGet version (RuleEvaluator.Repository.Database)](https://img.shields.io/nuget/v/RuleEvaluator.Repository.Database.svg?style=flat-square)](https://www.nuget.org/packages/RuleEvaluator.Repository.Database/) |

A lightweight rule engine for .NET with regex and decimal interval matching. **No DI container required.**

## Quick Start

```csharp
// No Windsor, no DI container — just create a factory and go
var factory = new DefaultCellFactory();
var rules = new RuleItems(factory);

// Add rules: input cells matched by regex, output cell for the result
rules.AddRuleItem(".*", "Premium", factory.CreateCell("HighTier", CellInputOutputType.Output));
rules.AddRuleItem(".*", "Basic",   factory.CreateCell("LowTier",  CellInputOutputType.Output));

// Find the first matching rule
var match = rules.Find("AnyProduct", "Premium");
Console.WriteLine(match?.Output(0).FilterValue); // "HighTier"
```

## Features

- **Regex matching** — input cells are matched using regular expressions
- **Decimal interval matching** — supports interval syntax like `INTERVAL<10;20>`, `Interval(5;15)`
- **Chain of responsibility** — decimal interval matcher falls through to regex matcher automatically
- **No DI container needed** — `DefaultCellFactory` wires the matcher pipeline for you
- **Diagnostics API** — `FindWithDiagnostics()` / `FindAllWithDiagnostics()` return match details and timing
- **Database loading** — load rules from MSSQL or PostgreSQL via `RuleItemsRepository`
- **Custom exception hierarchy** — `RuleNotFoundException`, `MatcherException`, `InputParameterCountMismatchException`, etc.

## Interval Syntax

```csharp
// Interval with inclusive/exclusive bounds
rules.AddRuleItem("INTERVAL<10;20>",  factory.CreateCell("Result1", CellInputOutputType.Output)); // 10 <= x <= 20
rules.AddRuleItem("INTERVAL(10;20)",  factory.CreateCell("Result2", CellInputOutputType.Output)); // 10 < x < 20
rules.AddRuleItem("Interval<10;20)",  factory.CreateCell("Result3", CellInputOutputType.Output)); // 10 <= x < 20

var match = rules.Find(15m);
```

## Diagnostics

```csharp
var rules = new RuleItems(new DefaultCellFactory());
rules.EnableDiagnostics = true;
rules.AddRuleItem(".*", factory.CreateCell("Output", CellInputOutputType.Output));

var result = rules.FindWithDiagnostics("input");
Console.WriteLine(result.HasMatch);                    // True
Console.WriteLine(result.Diagnostics?.RulesEvaluated); // 1
Console.WriteLine(result.Diagnostics?.Elapsed);        // 00:00:00.0001234
```

## Database Repository

```csharp
var factory = new DefaultCellFactory();
var cache = new CacheWrapperMemory();
var repo = new RuleItemsRepository(factory, cache, connectionString,
    "Schema.p_GetColumns", "Schema.p_GetData", TimeSpan.FromMinutes(10));

var rules = repo.Load("MyRuleSet");
var match = rules.Find("A", "B", 15);
```

## Projects

| Project | Description |
|:--------|:------------|
| `RuleEvaluator` | Core rule engine (no external dependencies) |
| `RuleEvaluator.Repository.Contract` | Repository interfaces and cache abstraction |
| `RuleEvaluator.Repository.Database` | MSSQL/PostgreSQL implementation with Dapper |
| `RuleEvaluator.Test` | Unit tests |
| `RuleEvaluator.Repository.Database.Test` | Integration tests (require database) |

## License

MIT — see [licence.txt](licence.txt)
