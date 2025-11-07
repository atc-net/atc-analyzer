namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<ParameterInlineAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class ParameterInlineAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void ShortMethod[|(
                                    int parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleParameterOnSameLine_LongDeclaration()
    {
        // Line exceeds 80 characters, parameter should be broken
        const string code = """
                            public class Sample
                            {
                                public void MyLooooooooooooooooooooooooooooooooooooooooooonMethod[|(int parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ConstructorWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class MyClass
                            {
                                public MyClass[|(
                                    string name)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_StaticMethodWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public static void MyMethod[|(
                                    int parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_AsyncMethodWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public async Task MyMethodAsync[|(
                                    int parameter1)|]
                                {
                                    await Task.CompletedTask;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_PrivateMethodWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                private void MyMethod[|(
                                    int parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithGenericTypeAndSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod<T>[|(
                                    T parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleParameterWithDefaultValue_Short_OnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(
                                    int x = 0)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleRefParameter_Short_OnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(
                                    ref int x)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleNullableParameter_Short_OnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(
                                    int? x)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_VirtualMethodWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public virtual void MyMethod[|(
                                    int parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_OverrideMethodWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class Base
                            {
                                public virtual void MyMethod(int parameter1)
                                {
                                }
                            }

                            public class Derived : Base
                            {
                                public override void MyMethod[|(
                                    int parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_AbstractMethodWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public abstract class Sample
                            {
                                public abstract void MyMethod[|(
                                    int parameter1)|];
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_LocalFunctionWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void OuterMethod()
                                {
                                    void LocalFunction[|(
                                        int param1)|]
                                    {
                                    }

                                    LocalFunction(1);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_DelegateWithSingleShortParameterOnNewLine()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public delegate void MyHandler[|(
                                    int value)|];
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleComplexTypeParameter_Short_OnNewLine()
    {
        const string code = """
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void MyMethod[|(
                                    List<int> param1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleTupleParameter_Short_OnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(
                                    (int x, int y) tuple)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithSingleArrayParameter_Short_OnNewLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(
                                    string[] items)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}

