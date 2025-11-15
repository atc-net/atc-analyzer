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

# Run tests for a specific category (using xUnit.net filter)
cd test/Atc.Analyzer.Tests
dotnet test --filter-class "*Style*"

# Run a specific analyzer's tests
cd test/Atc.Analyzer.Tests
dotnet test --filter-class "*ParameterSeparationAnalyzerTests*"
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

### Code Fix Provider Implementation Pattern

When an analyzer can suggest automatic fixes, implement a CodeFixProvider in the same directory:

```csharp
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof({RuleName}CodeFixProvider))]
[Shared]
public sealed class {RuleName}CodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.{Category}.{RuleName}];

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var node = root.FindNode(diagnosticSpan);

        // Register code fix
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Fix description",
                createChangedDocument: c => FixAsync(context.Document, node, c),
                equivalenceKey: nameof({RuleName}CodeFixProvider)),
            context.Diagnostics);
    }

    private static async Task<Document> FixAsync(
        Document document,
        SyntaxNode node,
        CancellationToken cancellationToken)
    {
        // Apply fix and return updated document
    }
}
```

**Important notes for CodeFixProviders:**
- Always use `ConfigureAwait(false)` for async calls
- Return the modified document using `document.WithSyntaxRoot(newRoot)`
- Test code fixes thoroughly - they must produce syntactically correct code
- Code fixes should preserve comments, attributes, and formatting where possible

### Testing Pattern

Tests mirror the analyzer structure under `test/Atc.Analyzer.Tests/Rules/{Category}/`.

#### Test Organization

For analyzers with extensive test coverage, tests should be organized using partial classes split across two files:

1. **`{RuleName}AnalyzerNoDiagnosticTests.cs`** - Tests that verify no diagnostics are reported
   - All test methods must be prefixed with `NoDiagnostic_`
   - Contains test cases where the analyzer should NOT report any issues

2. **`{RuleName}AnalyzerReportsDiagnosticTests.cs`** - Tests that verify diagnostics are reported
   - All test methods must be prefixed with `ReportsDiagnostic_`
   - Contains test cases where the analyzer SHOULD report issues
   - Test code uses `[|...|]` markers to indicate expected diagnostic locations

Both files use the same partial class name: `{RuleName}AnalyzerTests`

Example structure:
```csharp
// File: ParameterInlineAnalyzerNoDiagnosticTests.cs
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class ParameterInlineAnalyzerTests
{
    [Fact]
    public async Task NoDiagnostic_MethodWithNoParameters()
    {
        // Test code that should NOT trigger diagnostics
    }
}

// File: ParameterInlineAnalyzerReportsDiagnosticTests.cs
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class ParameterInlineAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_MethodWithIssue()
    {
        // Test code with [|...|] markers for expected diagnostics
    }
}
```

**Note**: The `MA0048` suppression is required because the file names don't match the partial class name.

#### Testing Code Fix Providers

Code fix tests should be in a separate file: `{RuleName}CodeFixProviderTests.cs`

```csharp
public sealed class {RuleName}CodeFixProviderTests
{
    [Fact]
    public async Task FixMethod_Description()
    {
        const string source = """
                              // Source code that triggers the diagnostic
                              public void Method(int x, int y)
                              {
                              }
                              """;

        const string fixedSource = """
                                   // Expected code after fix is applied
                                   public void Method(
                                       int x,
                                       int y)
                                   {
                                   }
                                   """;

        // Specify expected diagnostic location
        var expected = new[]
        {
            new DiagnosticResult(
                "ATC###",
                DiagnosticSeverity.Warning)
                .WithSpan(lineNumber, startColumn, lineNumber, endColumn),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}
```

**Important testing notes:**
- Always specify the expected diagnostic location with `.WithSpan()` for reliable tests
- Line numbers are 1-based, column numbers are 0-based
- Use raw string literals (`"""..."""`) for multi-line test code
- Test multiple scenarios: methods, constructors, delegates, local functions
- Include tests with modifiers, attributes, default values, and edge cases
- The test framework requires explicit diagnostic locations to properly verify fixes

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

#### Global Usings Analyzers: Dual Policy Pattern

The project implements two separate analyzers for enforcing global usings, following a **clear separation of concerns** principle:

**ATC220: GlobalUsingsAllAnalyzer (Strict Policy)**
- Flags ALL using directives (except static usings and aliases)
- Requires every namespace to be moved to GlobalUsings.cs
- Best for: Internal projects, maximum consistency, zero boilerplate
- Implementation: `GlobalUsingsAllAnalyzer.cs` + `GlobalUsingsAllCodeFixProvider.cs`

**ATC221: GlobalUsingsCommonAnalyzer (Lenient Policy)**
- Flags only System/Microsoft/Atc namespaces
- Allows third-party usings in individual files
- Best for: Library projects, projects with many third-party dependencies
- Implementation: `GlobalUsingsCommonAnalyzer.cs` + `GlobalUsingsCommonCodeFixProvider.cs`

**Why Two Separate Analyzers?**

While these analyzers could theoretically be merged into one with different rule IDs, they are kept separate to:
1. **Clear separation of concerns**: Each analyzer has a single, well-defined responsibility
2. **Simpler code**: Each analyzer has its own straightforward filter logic
3. **Independent evolution**: Rules can evolve independently without affecting each other
4. **Easier testing**: Each analyzer can be tested in isolation
5. **Better maintainability**: Clear naming and organization

**Common Implementation Details:**

Both analyzers:
- Skip GlobalUsings.cs itself to avoid circular diagnostics
- Exclude static usings (`using static System.Console`)
- Exclude using aliases (`using Constants = Domain.Constants`)
- Exclude generated code files
- Share the same code fix pattern (move to GlobalUsings.cs with alphabetical ordering by namespace group)
- Use the same namespace grouping: System → Atc → Microsoft → Others

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

## Troubleshooting

### Visual Studio Not Showing Code Fixes

If code fixes don't appear in Visual Studio's lightbulb menu:

1. **Restart Visual Studio** - This is usually sufficient to reload updated analyzers
2. **Clean and Rebuild**:
   ```bash
   dotnet clean
   dotnet build
   ```
3. **Clear MEF Cache** (if restart doesn't work):
   ```powershell
   # Close Visual Studio first, then run:
   Remove-Item "$env:LOCALAPPDATA\Microsoft\VisualStudio\*\ComponentModelCache" -Recurse -Force
   ```
4. **Verify Analyzer Reference** - In sample projects, check the analyzer is referenced as:
   ```xml
   <ItemGroup>
     <ProjectReference Include="..\..\src\Atc.Analyzer\Atc.Analyzer.csproj">
       <ReferenceOutputAssembly>false</ReferenceOutputAssembly>
       <OutputItemType>Analyzer</OutputItemType>
     </ProjectReference>
   </ItemGroup>
   ```

### Test Framework Integration Issues

If code fix tests fail with the Microsoft.CodeAnalysis.Testing framework:

- **Always specify expected diagnostic locations** using `.WithSpan(line, startColumn, line, endColumn)`
- Without explicit spans, the framework may not properly match diagnostics between source and fixed code
- Example:
  ```csharp
  var expected = new[]
  {
      new DiagnosticResult("ATC202", DiagnosticSeverity.Warning)
          .WithSpan(3, 24, 3, 45),  // Explicit location
  };
  await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
  ```
