namespace Atc.Analyzer.Tests.Rules.Usage;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<GeneratedRegexCompiledFlagAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class GeneratedRegexCompiledFlagAnalyzerTests
{
    [Fact]
    public async Task NoDiagnostic_GeneratedRegexWithoutOptions()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+")]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedRegexWithIgnoreCaseOption()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.IgnoreCase)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedRegexWithMultilineOption()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("^\\d+$", RegexOptions.Multiline)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedRegexWithMultipleOptionsWithoutCompiled()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedRegexWithNamedOptionsParameter()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", options: RegexOptions.IgnoreCase)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedRegexWithTimeoutParameter()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public partial class Sample
                              {
                                  [GeneratedRegex("\\d+", RegexOptions.None, 1000)]
                                  private static partial Regex NumberRegex();
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NoDiagnostic_RegularRegexWithCompiled()
    {
        const string source = """
                              using System.Text.RegularExpressions;

                              public class Sample
                              {
                                  private static readonly Regex NumberRegex = new Regex("\\d+", RegexOptions.Compiled);
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NoDiagnostic_NonRegexAttribute()
    {
        const string source = """
                              using System;

                              public class SampleAttribute : Attribute
                              {
                                  public SampleAttribute(string pattern, int options)
                                  {
                                  }
                              }

                              public class Test
                              {
                                  [Sample("test", 1)]
                                  public void Method()
                                  {
                                  }
                              }
                              """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(source);
    }
}