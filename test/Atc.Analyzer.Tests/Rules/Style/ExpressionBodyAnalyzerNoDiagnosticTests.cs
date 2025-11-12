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
    public async Task NoDiagnostic_MethodWithExpressionBody_ShortSingleLine()
    {
        const string code = """
                            public class Sample
                            {
                                public static string GetData() => "Sample Data";
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithExpressionBody_MultiLineFormat()
    {
        const string code = """
                            public class Sample
                            {
                                public static string GetData()
                                    => "Sample Data";
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithTernaryExpression_ProperFormatting()
    {
        const string code = """
                            public class Sample
                            {
                                private static string[] SetHelpArgumentIfNeeded(string[] args)
                                    => args.Length == 0
                                        ? new[] { "help" }
                                        : args;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithMultipleStatements()
    {
        const string code = """
                            public class Sample
                            {
                                public static string GetData()
                                {
                                    var data = "Sample";
                                    return data;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithComplexLogic()
    {
        const string code = """
                            public class Sample
                            {
                                public static int Calculate(int value)
                                {
                                    if (value > 0)
                                    {
                                        return value * 2;
                                    }
                                    return 0;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithLoop()
    {
        const string code = """
                            public class Sample
                            {
                                public static int Sum(int[] values)
                                {
                                    var sum = 0;
                                    foreach (var value in values)
                                    {
                                        sum += value;
                                    }
                                    return sum;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_VoidMethod()
    {
        const string code = """
                            public class Sample
                            {
                                public static void DoSomething()
                                {
                                    return;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_PropertyWithExpressionBody()
    {
        const string code = """
                            public class Sample
                            {
                                public string Name => "Sample";
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_PropertyWithExpressionBody_MultiLine()
    {
        const string code = """
                            public class Sample
                            {
                                public string Name
                                    => "Sample";
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_PropertyWithGetSetAccessors()
    {
        const string code = """
                            public class Sample
                            {
                                private string name = string.Empty;

                                public string Name
                                {
                                    get { var x = name; return x; }
                                    set { name = value; }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_PropertyWithAutoProperty()
    {
        const string code = """
                            public class Sample
                            {
                                public string Name { get; set; } = string.Empty;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_GetAccessorWithExpressionBody()
    {
        const string code = """
                            public class Sample
                            {
                                private string name = string.Empty;

                                public string Name
                                {
                                    get => name;
                                    set => name = value;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_AsyncMethod()
    {
        const string code = """
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public static async Task<string> GetDataAsync()
                                    => await Task.FromResult("data");
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithLambdaExpression()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public static Func<int, int> GetMultiplier(int factor) => x => x * factor;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedCodeClass()
    {
        const string code = """
                            using System.CodeDom.Compiler;

                            [GeneratedCode("MyGen", "1.0.1")]
                            public class GeneratedSample
                            {
                                public static string GetData()
                                {
                                    return "Sample Data";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_AutoGeneratedHeader()
    {
        const string code = """
                            //------------------------------------------------------------------------------
                            // This code was auto-generated by Generator 1.0.
                            //------------------------------------------------------------------------------
                            public class GeneratedClass
                            {
                                public static string GetData()
                                {
                                    return "Sample Data";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithComplexExpression_FitsOn80Chars()
    {
        const string code = """
                            public class Sample
                            {
                                public static int Calculate(int x, int y) => x * y + (x - y) * 2;
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithMethodCall()
    {
        const string code = """
                            public class Sample
                            {
                                public static string GetUpperCase(string input) => input.ToUpper();
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_StaticConstructor()
    {
        const string code = """
                            public class Sample
                            {
                                static Sample()
                                {
                                    // Initialization code
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithAttributes()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                [Obsolete("Use GetNewData instead")]
                                public static string GetData() => "Sample Data";
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}