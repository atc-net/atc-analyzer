namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<MethodChainSeparationAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class MethodChainSeparationAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_TwoMethodChainsOnOneLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = [|str.Trim().ToLower()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ThreeMethodChainsOnOneLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = [|str.Trim().Replace("xxx", "x").ToLower()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MultipleMethodChainsOnOneLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = [|str.Trim().Replace("xxx", "x").Replace("yyy", "y").Trim()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_LinqMethodChainsOnOneLine()
    {
        const string code = """
                            using System.Linq;
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void TestMethod(List<int> numbers)
                                {
                                    var result = [|numbers.Where(x => x > 0).Select(x => x * 2).ToList()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_StringBuilderChainsOnOneLine()
    {
        const string code = """
                            using System.Text;

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var result = [|new StringBuilder().Append("hello").Append(" world").ToString()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodChainInReturnStatement()
    {
        const string code = """
                            public class Sample
                            {
                                public string TestMethod(string str)
                                {
                                    return [|str.Trim().ToLower()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodChainInMethodArgument()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    Console.WriteLine([|str.Trim().ToLower()|]);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_NestedMethodChainOnOneLine()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = [|str.Replace("a", "b").Replace("c", "d")|];
                                    Console.WriteLine([|result.Trim().ToUpper()|]);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_FirstMethodOnSameLineWithBaseExpression_RestOnSeparateLines()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public void TestMethod(string text)
                                {
                                    var result = [|text.Trim()
                                        .Replace("World", "Universe", StringComparison.OrdinalIgnoreCase)
                                        .ToUpper()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_FirstMethodOnSameLineWithBaseExpression_ThreeMethods()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string text)
                                {
                                    var result = [|text.Trim()
                                        .ToLower()
                                        .Replace("x", "y")|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_FluentAssertions_ShouldWithMultipleAssertions()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    [|actual.Should().Be("test").And.NotBeNull()|];
                                }
                            }

                            public static class Extensions
                            {
                                public static Assertions Should(this string value) => new Assertions();
                            }

                            public class Assertions
                            {
                                public Assertions Be(string expected) => this;
                                public Assertions And => this;
                                public Assertions NotBeNull() => this;
                                public Assertions NotBeNullOrWhiteSpace() => this;
                                public Assertions StartWith(string expected) => this;
                                public Assertions EndWith(string expected) => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_FluentAssertions_ShouldNotBeNullWithMultipleAssertionsOnOneLine()
    {
        const string code = """
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = new Dictionary<int, string>();
                                    [|actual.Should().NotBeNull().And.BeOfType<Dictionary<int, string>>()|];
                                }
                            }

                            public static class Extensions
                            {
                                public static Assertions<T> Should<T>(this T value) => new Assertions<T>();
                            }

                            public class Assertions<T>
                            {
                                public Assertions<T> NotBeNull() => this;
                                public Assertions<T> And => this;
                                public Assertions<T> BeOfType<TType>() => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_FluentAssertions_ShouldWithMultipleAndsOnOneLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    [|actual.Should().Be("test").And.StartWith("te")|];
                                }
                            }

                            public static class Extensions
                            {
                                public static Assertions Should(this string value) => new Assertions();
                            }

                            public class Assertions
                            {
                                public Assertions Be(string expected) => this;
                                public Assertions And => this;
                                public Assertions StartWith(string expected) => this;
                                public Assertions EndWith(string expected) => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ThreeMethodChainsInIfStatementCondition()
    {
        const string code = """
                            using System.Linq;

                            public class Sample
                            {
                                public void TestMethod(string text)
                                {
                                    if ([|text.Trim().ToLower().Replace("a", "b")|] == "test")
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_TwoMethodChainsInVariableAssignment()
    {
        const string code = """
                            using System.Linq;

                            public class Sample
                            {
                                public void TestMethod(string text)
                                {
                                    var result = [|text.Trim().ToLower()|];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}

