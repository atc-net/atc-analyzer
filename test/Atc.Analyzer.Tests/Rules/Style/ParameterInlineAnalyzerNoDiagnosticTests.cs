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
    public async Task NoDiagnostic_MethodWithNoParameters()
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
    public async Task NoDiagnostic_MethodWithSingleShortParameterOnSameLine()
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
    public async Task NoDiagnostic_MethodWithSingleParameterOnNewLine_LongDeclaration()
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
    public async Task NoDiagnostic_MethodWithMultipleParameters()
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
    public async Task NoDiagnostic_ConstructorWithSingleShortParameterOnSameLine()
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
    public async Task NoDiagnostic_ConstructorWithSingleParameterOnNewLine_LongDeclaration()
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
    public async Task NoDiagnostic_MethodWithSingleLongTypeParameter_OnNewLine()
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
    public async Task NoDiagnostic_GeneratedCodeClass_WithViolations()
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
    public async Task NoDiagnostic_GeneratedCodeClass_SingleParameterOnNewLine()
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
    public async Task NoDiagnostic_GeneratedCodeClass_WithNullableParameter()
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
    public async Task NoDiagnostic_AutoGeneratedHeader_WithViolations()
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
    public async Task NoDiagnostic_AutoGeneratedHeader_CaseInsensitive()
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
    public async Task NoDiagnostic_AutoGeneratedHeader_WithMultiLineComment()
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

    [Fact]
    public async Task NoDiagnostic_AttributeConstructorWithSingleShortParameterOnSameLine()
    {
        const string code = """
                            using System;
                            using System.Diagnostics.CodeAnalysis;

                            public sealed class EnumGuidAttribute : Attribute
                            {
                                [SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "OK.")]
                                public EnumGuidAttribute(string value)
                                {
                                    GlobalIdentifier = new Guid(value);
                                }

                                public Guid GlobalIdentifier { get; }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}