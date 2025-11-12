namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<ParameterSeparationAnalyzer, ParameterSeparationCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

/// <summary>
/// Simplest possible test to isolate the issue.
/// </summary>
public sealed class SimplestCodeFixTest
{
    [Fact]
    public Task SimplestTest()
    {
        const string source = "public class C { public void M(int a, int b) { } }";
        const string fixedSource = @"public class C { public void M(
    int a,
    int b) { } }";

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 31, 1, 45),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}