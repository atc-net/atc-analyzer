# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Atc.Analyzer is a Roslyn-based C# code analyzer that enforces coding best practices. It's built as a .NET analyzer package targeting netstandard2.0 for compatibility with the Roslyn analysis framework.

## Build & Development Commands

### Building
```bash
dotnet build
```

### Testing
```bash
# Run all tests
dotnet test

# Run tests in a specific project
dotnet test test/Atc.Analyzer.Tests/Atc.Analyzer.Tests.csproj

# Run a specific test
dotnet test --filter "FullyQualifiedName~ParameterSeparationAnalyzerTests"
```

### Code Quality
The project uses the `atc-coding-rules-updater` tool to maintain consistent coding standards:

```powershell
# Update coding rules (PowerShell)
.\atc-coding-rules-updater.ps1
```

This updates `.editorconfig` and analyzer configurations from the atc-coding-rules repository.

## Architecture

### Analyzer Structure

The project follows a category-based organization for analyzer rules:

- **Design** (ATC001-ATC099): Architectural and design rules
- **Naming** (ATC101-ATC199): Naming convention rules
- **Style** (ATC201-ATC299): Code style and formatting rules
- **Usage** (ATC301-ATC399): API usage rules
- **Performance** (ATC401-ATC499): Performance-related rules
- **Security** (ATC501-ATC599): Security best practices

Rule identifiers and categories are defined in:
- `src/Atc.Analyzer/RuleIdentifierConstants.cs` - Rule ID constants
- `src/Atc.Analyzer/RuleCategoryConstants.cs` - Category constants

### Analyzer Implementation Pattern

Analyzers are organized under `src/Atc.Analyzer/Rules/{Category}/`:

```csharp
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class {RuleName}Analyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor Rule = new(
        RuleIdentifierConstants.{Category}.{RuleName},
        title: "...",
        messageFormat: "...",
        RuleCategoryConstants.{Category},
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "...",
        helpLinkUri: "...");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

    public override void Initialize(AnalysisContext context)
    {
        // Analyzer implementation
    }
}
```

### Testing Pattern

Tests mirror the analyzer structure under `test/Atc.Analyzer.Tests/Rules/{Category}/`. Each analyzer should have a corresponding test class: `{RuleName}AnalyzerTests.cs`.

## Key Technical Details

- **Target Framework**: netstandard2.0 (for analyzer), net9.0 (for tests)
- **Language Version**: C# 13.0
- **Nullable Reference Types**: Enabled
- **Implicit Usings**: Enabled
- **Roslyn Version**: Microsoft.CodeAnalysis.CSharp 4.14.0

### Global Usings

The project uses global usings (defined in `src/Atc.Analyzer/GlobalUsings.cs`):
```csharp
global using System.Collections.Immutable;
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.Diagnostics;
```

### Code Analysis Configuration

The project enforces strict code quality standards:
- **Analysis Level**: latest-All
- **Warnings as Errors**: Enabled in Release builds
- **Multiple analyzers**: AsyncFixer, Asyncify, Meziantou.Analyzer, SecurityCodeScan, StyleCop, SonarAnalyzer

Settings are configured in:
- `Directory.Build.props` - Shared MSBuild properties and analyzer references
- `.editorconfig` - ATC coding rules (from atc-coding-rules repository)

## Development Workflow

1. When adding a new analyzer rule:
   - Add the rule ID constant to `RuleIdentifierConstants.cs` under the appropriate category
   - Create the analyzer in `src/Atc.Analyzer/Rules/{Category}/{RuleName}Analyzer.cs`
   - Create corresponding tests in `test/Atc.Analyzer.Tests/Rules/{Category}/{RuleName}AnalyzerTests.cs`
   - Ensure the rule ID follows the numbering convention for its category

2. Rule ID ranges must be respected:
   - Design: ATC001-ATC099
   - Naming: ATC101-ATC199
   - Style: ATC201-ATC299
   - Usage: ATC301-ATC399
   - Performance: ATC401-ATC499
   - Security: ATC501-ATC599

3. The project uses Nerdbank.GitVersioning for version management (configured in `version.json`)

## Solution Structure

- `src/Atc.Analyzer/` - Main analyzer project (Roslyn analyzer)
- `test/Atc.Analyzer.Tests/` - Unit tests for analyzers
- `sample/` - Sample projects to test analyzer behavior
- `Atc.Analyzer.slnx` - Solution file (new XML-based format)
