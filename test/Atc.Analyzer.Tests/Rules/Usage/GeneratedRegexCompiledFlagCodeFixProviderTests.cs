namespace Atc.Analyzer.Tests.Rules.Usage;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<GeneratedRegexCompiledFlagAnalyzer, GeneratedRegexCompiledFlagCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class GeneratedRegexCompiledFlagCodeFixProviderTests
{
    [Fact]
    public async Task FixCompiledOption_WhenOnlyOption()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.Compiled)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        const string fixedSource = """
                                   using System.Text.RegularExpressions;

                                   public partial class Sample
                                   {
                                       [GeneratedRegex("\\d+")]
                                       private static partial Regex NumberRegex();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC301",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 29, 5, 50),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixCompiledOption_WhenCompiledIsFirstInCombination()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        const string fixedSource = """
                                   using System.Text.RegularExpressions;

                                   public partial class Sample
                                   {
                                       [GeneratedRegex("\\d+", RegexOptions.IgnoreCase)]
                                       private static partial Regex NumberRegex();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC301",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 29, 5, 50),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixCompiledOption_WhenCompiledIsLastInCombination()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        const string fixedSource = """
                                   using System.Text.RegularExpressions;

                                   public partial class Sample
                                   {
                                       [GeneratedRegex("\\d+", RegexOptions.IgnoreCase)]
                                       private static partial Regex NumberRegex();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC301",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 55, 5, 76),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixCompiledOption_WhenCompiledIsInMiddleOfCombination()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        const string fixedSource = """
                                   using System.Text.RegularExpressions;

                                   public partial class Sample
                                   {
                                       [GeneratedRegex("\\d+", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
                                       private static partial Regex NumberRegex();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC301",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 55, 5, 76),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixCompiledOption_WithNamedParameter()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", options: RegexOptions.Compiled)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        const string fixedSource = """
                                   using System.Text.RegularExpressions;

                                   public partial class Sample
                                   {
                                       [GeneratedRegex("\\d+")]
                                       private static partial Regex NumberRegex();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC301",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 38, 5, 59),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixCompiledOption_WithNamedParameterAndMultipleOptions()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", options: RegexOptions.Compiled | RegexOptions.IgnoreCase)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        const string fixedSource = """
                                   using System.Text.RegularExpressions;

                                   public partial class Sample
                                   {
                                       [GeneratedRegex("\\d+", options: RegexOptions.IgnoreCase)]
                                       private static partial Regex NumberRegex();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC301",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 38, 5, 59),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixCompiledOption_WithMultilineAttribute()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex(
                                      pattern: "\\d+",
                                      options: RegexOptions.Compiled | RegexOptions.IgnoreCase,
                                      matchTimeoutMilliseconds: 1000)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        const string fixedSource = """
                                   using System.Text.RegularExpressions;

                                   public partial class Sample
                                   {
                                       [GeneratedRegex(
                                           pattern: "\\d+",
                                           options: RegexOptions.IgnoreCase,
                                           matchTimeoutMilliseconds: 1000)]
                                       private static partial Regex NumberRegex();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC301",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 18, 7, 39),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}