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
    public async Task NoDiagnostic_SingleMethodCall()
    {
        const string code = """
                            using System.Threading;
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public async Task TestMethod(CancellationToken cancellationToken)
                                {
                                    var result = await GetDataAsync(cancellationToken);
                                }

                                private Task<string> GetDataAsync(CancellationToken cancellationToken)
                                {
                                    return Task.FromResult("data");
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_TwoMethodChainsOnSeparateLines()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = str
                                        .Trim()
                                        .ToLower();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_ThreeMethodChainsOnSeparateLines()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = str
                                        .Trim()
                                        .Replace("xxx", "x")
                                        .ToLower();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_AwaitWithConfigureAwaitOnSeparateLines()
    {
        const string code = """
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public async Task TestMethod()
                                {
                                    var result = await GetDataAsync()
                                        .ConfigureAwait(false);
                                }

                                private Task<string> GetDataAsync()
                                {
                                    return Task.FromResult("data");
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MultipleMethodChainsOnSeparateLines()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = str
                                        .Trim()
                                        .Replace("xxx", "x")
                                        .Replace("yyy", "y")
                                        .Trim();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodCallWithOneChainedMethod()
    {
        const string code = """
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public async Task TestMethod()
                                {
                                    var result = await GetDataAsync().ConfigureAwait(false);
                                }

                                private Task<string> GetDataAsync()
                                {
                                    return Task.FromResult("data");
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_LinqMethodChainsOnSeparateLines()
    {
        const string code = """
                            using System.Linq;
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void TestMethod(List<int> numbers)
                                {
                                    var result = numbers
                                        .Where(x => x > 0)
                                        .Select(x => x * 2)
                                        .ToList();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_StringBuilderChainsOnSeparateLines()
    {
        const string code = """
                            using System.Text;

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var result = new StringBuilder()
                                        .Append("hello")
                                        .Append(" world")
                                        .ToString();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_PropertyAccessAfterMethodCall()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    var length = str.Trim().Length;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_TwoMethodChainsInConditionalExpression()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod(string str)
                                {
                                    if (str.Trim().ToLower() == "test")
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_TaskChainingWithContinueWith()
    {
        const string code = """
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var task = Task.Run(() => 42).ContinueWith(t => t.Result * 2);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedCodeClass_WithMethodChains()
    {
        const string code = """
                            using System.CodeDom.Compiler;

                            [GeneratedCode("MyGen", "1.0.1")]
                            public class GeneratedSample
                            {
                                public void TestMethod(string str)
                                {
                                    var result = str.Trim().ToLower();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_AutoGeneratedHeader_WithMethodChains()
    {
        const string code = """
                            //------------------------------------------------------------------------------
                            // This code was auto-generated by ApiGenerator 2.0.
                            //
                            // Changes to this file may cause incorrect behavior and will be lost if
                            // the code is regenerated.
                            //------------------------------------------------------------------------------
                            namespace MyNamespace;

                            public class GeneratedClass
                            {
                                public void TestMethod(string str)
                                {
                                    var result = str.Trim().ToLower();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FluentAssertions_ShouldWithOneAssertion()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    actual.Should().Be("test");
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
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FluentAssertions_ShouldWithNotBeNull()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    actual.Should().NotBeNull();
                                }
                            }

                            public static class Extensions
                            {
                                public static Assertions Should(this string value) => new Assertions();
                            }

                            public class Assertions
                            {
                                public Assertions NotBeNull() => this;
                                public Assertions And => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FluentAssertions_ShouldWithBeEquivalentTo()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = new { Name = "test" };
                                    var expected = new { Name = "test" };
                                    actual.Should().BeEquivalentTo(expected);
                                }
                            }

                            public static class Extensions
                            {
                                public static Assertions Should<T>(this T value) => new Assertions();
                            }

                            public class Assertions
                            {
                                public Assertions BeEquivalentTo<T>(T expected) => this;
                                public Assertions And => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FluentAssertions_ShouldWithNotBeNullOrEmpty()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    actual.Should().NotBeNullOrEmpty();
                                }
                            }

                            public static class Extensions
                            {
                                public static Assertions Should(this string value) => new Assertions();
                            }

                            public class Assertions
                            {
                                public Assertions NotBeNullOrEmpty() => this;
                                public Assertions And => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FluentAssertions_ShouldNotBeNullWithAndOnSeparateLines()
    {
        const string code = """
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = new Dictionary<int, string>();
                                    var expected = 3;
                                    actual
                                        .Should().NotBeNull()
                                        .And.BeOfType<Dictionary<int, string>>()
                                        .And.HaveCount(expected);
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
                                public Assertions<T> HaveCount(int count) => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FluentAssertions_ShouldWithAndOnSeparateLines()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    actual
                                        .Should().Be("test")
                                        .And.NotBeNull();
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
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FluentAssertions_ShouldWithMultipleAnds_ProperlyFormatted()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    actual
                                        .Should().Be("test")
                                        .And.StartWith("te")
                                        .And.EndWith("st");
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
    public async Task NoDiagnostic_FluentAssertions_ShouldOnSeparateLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var actual = "test";
                                    actual
                                        .Should()
                                        .Be("test");
                                }
                            }

                            public static class Extensions
                            {
                                public static Assertions Should(this string value) => new Assertions();
                            }

                            public class Assertions
                            {
                                public Assertions Be(string expected) => this;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodCallWithArgumentFollowedByToString()
    {
        const string code = """
                            using System;
                            using System.Globalization;
                            using System.Threading;

                            public class Sample
                            {
                                private static int MethodA(string value)
                                {
                                    return 0;
                                }

                                private static string MethodB(string value)
                                {
                                    var methodValA = MethodA(value).ToString(Thread.CurrentThread.CurrentCulture);

                                    return value
                                        .Replace(methodValA, string.Empty, StringComparison.Ordinal)
                                        .Trim();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_TwoMethodChainsInIfStatementCondition()
    {
        const string code = """
                            using System.Linq;
                            using System.Xml.Linq;

                            public class Sample
                            {
                                public void TestMethod(XElement rootElement)
                                {
                                    if (rootElement.Attributes("Sdk").Count() == 1)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_TwoMethodChainsInIfStatementWithComplexCondition()
    {
        const string code = """
                            using System.Linq;

                            public class Sample
                            {
                                public void TestMethod(string text)
                                {
                                    if (text.Trim().ToLower() == "test" && text.Length > 0)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}

