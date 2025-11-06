namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<ParameterInlineAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class ParameterInlineAnalyzerTests
{
    [Fact]
    public async Task MethodWithNoParameters_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod()
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithSingleShortParameterOnSameLine_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(int parameter1)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithSingleParameterOnNewLine_LongDeclaration_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyLoooooooooooooooooooooooooooooooooooooooooooooooonMethod(
                                    int parameter1)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithSingleParameterOnSameLine_LongDeclaration_NoDiagnostic()
    {
        // This is handled by ATC201, not ATC202
        const string code = """
                            public class Sample
                            {
                                public void MyLooooooooooooooooooooooooooooooooooooooooooonMethod(int parameter1)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithMultipleParameters_NoDiagnostic()
    {
        // ATC201 handles multiple parameters, not ATC202
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    int parameter1,
                                    int parameter2)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ConstructorWithSingleShortParameterOnSameLine_NoDiagnostic()
    {
        const string code = """
                            public class MyClass
                            {
                                public MyClass(string name)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ConstructorWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task ConstructorWithSingleParameterOnNewLine_LongDeclaration_NoDiagnostic()
    {
        const string code = """
                            public class MyLooooooooooooooooooooooooooooooooooooooooooooooooooongClassName
                            {
                                public MyLooooooooooooooooooooooooooooooooooooooooooooooooooongClassName(
                                    string name)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task StaticMethodWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task AsyncMethodWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task PrivateMethodWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithGenericTypeAndSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithSingleParameterWithDefaultValue_Short_OnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithSingleRefParameter_Short_OnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithSingleNullableParameter_Short_OnNewLine_ReportsDiagnostic()
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
    public async Task VirtualMethodWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task OverrideMethodWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task AbstractMethodWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task LocalFunctionWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task DelegateWithSingleShortParameterOnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithSingleComplexTypeParameter_Short_OnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithSingleTupleParameter_Short_OnNewLine_ReportsDiagnostic()
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
    public async Task MethodWithSingleArrayParameter_Short_OnNewLine_ReportsDiagnostic()
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

    [Fact]
    public async Task MethodWithSingleLongTypeParameter_OnNewLine_NoDiagnostic()
    {
        const string code = """
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void MyMethodWithLongName(
                                    Dictionary<string, List<int>> parameterWithLongNameThatExceeds80Chars)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GeneratedCodeClass_WithViolations_NoDiagnostic()
    {
        const string code = """
                            using System.CodeDom.Compiler;

                            [GeneratedCode("MyGen", "1.0.1")]
                            public class GeneratedSample
                            {
                                public void ShortMethod(
                                    int x)
                                {
                                }

                                public void MethodWithSingleParameter(
                                    string name)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GeneratedCodeClass_SingleParameterOnNewLine_NoDiagnostic()
    {
        const string code = """
                            using System.CodeDom.Compiler;

                            [GeneratedCode("ApiGenerator", "2.0")]
                            public class GeneratedApiClient
                            {
                                public void Execute(
                                    int id)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GeneratedCodeClass_WithNullableParameter_NoDiagnostic()
    {
        const string code = """
                            using System.CodeDom.Compiler;

                            [GeneratedCode("T4", "1.0")]
                            public class GeneratedModel
                            {
                                public void SetValue(
                                    int? value)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AutoGeneratedHeader_WithViolations_NoDiagnostic()
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
                                public void ShortMethod(
                                    int x)
                                {
                                }

                                public void MethodWithSingleParameter(
                                    string name)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AutoGeneratedHeader_CaseInsensitive_NoDiagnostic()
    {
        const string code = """
                            // This file is AUTO-GENERATED by code generator
                            namespace MyNamespace;

                            public class GeneratedClass
                            {
                                public void Method(
                                    int parameter)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AutoGeneratedHeader_WithMultiLineComment_NoDiagnostic()
    {
        const string code = """
                            /*
                             * This code was auto-generated
                             * Do not modify manually
                             */
                            namespace MyNamespace;

                            public class GeneratedClass
                            {
                                public void Execute(
                                    int id)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}