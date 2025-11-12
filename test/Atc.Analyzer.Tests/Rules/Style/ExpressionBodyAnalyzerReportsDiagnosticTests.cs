namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<ExpressionBodyAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class ExpressionBodyAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_MethodWithSimpleReturnStatement()
    {
        const string code = """
                            public class Sample
                            {
                                public static string [|GetData|]()
                                {
                                    return "Sample Data";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSimpleReturnStatement_IntValue()
    {
        const string code = """
                            public class Sample
                            {
                                public static int [|GetNumber|]()
                                {
                                    return 42;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSimpleReturnStatement_Expression()
    {
        const string code = """
                            public class Sample
                            {
                                public static int [|Calculate|](int x, int y)
                                {
                                    return x + y;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_PrivateMethod()
    {
        const string code = """
                            public class Sample
                            {
                                private static string [|GetData|]()
                                {
                                    return "Sample Data";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_InstanceMethod()
    {
        const string code = """
                            public class Sample
                            {
                                public string [|GetData|]()
                                {
                                    return "Sample Data";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ExpressionBodyExceeds80Characters()
    {
        const string code = """
                            public class Sample
                            {
                                public static string GetData() [|=>|] "Sample Dataaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_TernaryExpressionWithArrowOnSameLine()
    {
        const string code = """
                            public class Sample
                            {
                                private static string[] SetHelpArgumentIfNeeded(string[] args) [|=>|]
                                    args.Length == 0
                                        ? new[] { "help" }
                                        : args;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_GetAccessorWithBlockBody()
    {
        const string code = """
                            public class Sample
                            {
                                private string name = string.Empty;

                                public string Name
                                {
                                    [|get|]
                                    {
                                        return name;
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithMethodCallReturn()
    {
        const string code = """
                            public class Sample
                            {
                                public static string [|GetUpperCase|](string input)
                                {
                                    return input.ToUpper();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithTernaryInReturn()
    {
        const string code = """
                            public class Sample
                            {
                                public static int [|GetValue|](bool condition)
                                {
                                    return condition ? 1 : 0;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithNullCoalescing()
    {
        const string code = """
                            public class Sample
                            {
                                public static string [|GetName|](string? input)
                                {
                                    return input ?? "default";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_AsyncMethodWithReturn()
    {
        const string code = """
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public static async Task<string> [|GetDataAsync|]()
                                {
                                    return await Task.FromResult("data");
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithLambdaReturn()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public static Func<int, int> [|GetMultiplier|](int factor)
                                {
                                    return x => x * factor;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithNewObjectReturn()
    {
        const string code = """
                            public class Sample
                            {
                                public static object [|CreateObject|]()
                                {
                                    return new object();
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithArrayReturn()
    {
        const string code = """
                            public class Sample
                            {
                                public static int[] [|GetArray|]()
                                {
                                    return new[] { 1, 2, 3 };
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_GenericMethod()
    {
        const string code = """
                            public class Sample
                            {
                                public static T [|GetDefault|]<T>()
                                {
                                    return default(T)!;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}