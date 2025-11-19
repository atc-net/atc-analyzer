namespace Atc.Analyzer.Tests.Rules.Usage;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<GeneratedRegexCompiledFlagAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class GeneratedRegexCompiledFlagAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_GeneratedRegexWithCompiledOption()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", [|RegexOptions.Compiled|])]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task ReportsDiagnostic_GeneratedRegexWithCompiledAndIgnoreCase()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", [|RegexOptions.Compiled|] | RegexOptions.IgnoreCase)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task ReportsDiagnostic_GeneratedRegexWithIgnoreCaseAndCompiled()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.IgnoreCase | [|RegexOptions.Compiled|])]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task ReportsDiagnostic_GeneratedRegexWithMultipleOptionsIncludingCompiled()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.IgnoreCase | [|RegexOptions.Compiled|] | RegexOptions.Multiline)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task ReportsDiagnostic_GeneratedRegexWithNamedOptionsParameter()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", options: [|RegexOptions.Compiled|])]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task ReportsDiagnostic_GeneratedRegexWithNamedOptionsParameterAndMultipleFlags()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", options: [|RegexOptions.Compiled|] | RegexOptions.IgnoreCase)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }
}