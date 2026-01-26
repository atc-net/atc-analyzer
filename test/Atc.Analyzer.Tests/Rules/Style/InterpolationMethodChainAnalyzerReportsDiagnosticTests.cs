namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<InterpolationMethodChainAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class InterpolationMethodChainAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_TwoChainedMethodCallsInInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Hello {[|myVar.ToString().ToLower()|]} world";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ThreeChainedMethodCallsInInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Hello {[|myVar.ToString().Trim().ToLower()|]} world";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MultipleChainedMethodCallsWithReplace()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Value: {[|myVar.Trim().Replace("a", "b")|]}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MultipleInterpolationsWithChains()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var a = "first";
                                    var b = "second";
                                    var result = $"{[|a.Trim().ToLower()|]} and {[|b.Trim().ToUpper()|]}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ChainedMethodCallsInVerbatimInterpolatedString()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $@"Hello {[|myVar.ToString().ToLower()|]} world";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ChainedMethodCallsInRawInterpolatedString()
    {
        const string code = """"
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"""Hello {[|myVar.ToString().ToLower()|]} world""";
                                }
                            }
                            """";

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ChainedMethodCallsWithFormat()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var number = 42.5;
                                    var result = $"Value: {[|number.ToString().Trim()|]:N2}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ChainedMethodCallsStartingFromProperty()
    {
        const string code = """
                            public class Sample
                            {
                                public string Name { get; set; } = "test";

                                public void TestMethod()
                                {
                                    var result = $"Hello {[|Name.Trim().ToLower()|]}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ChainedMethodCallsInNestedInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var inner = "inner";
                                    var result = $"Outer: {$"Inner: {[|inner.Trim().ToLower()|]}"}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ChainedLinqMethodsInInterpolation()
    {
        const string code = """
                            using System.Linq;
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var items = new List<string> { "a", "b", "c" };
                                    var result = $"First: {[|items.First().ToUpper()|]}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}
