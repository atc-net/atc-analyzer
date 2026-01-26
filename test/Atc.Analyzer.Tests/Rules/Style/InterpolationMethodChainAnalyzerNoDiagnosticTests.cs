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
    public async Task NoDiagnostic_SingleMethodCallInInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Hello {myVar.ToString()} world";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_PropertyAccessInInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Length is {myVar.Length}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_VariableAccessInInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Value is {myVar}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_NonChainedMethodCallsInInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var result = $"Hello {Method1()} {Method2()}";
                                }

                                private string Method1() => "method1";
                                private string Method2() => "method2";
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_SingleMethodCallWithPropertyInInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Length is {myVar.Trim().Length}";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_EmptyInterpolation()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var result = $"Hello world";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedCodeClass_WithMethodChainInInterpolation()
    {
        const string code = """
                            using System.CodeDom.Compiler;

                            [GeneratedCode("MyGen", "1.0.1")]
                            public class GeneratedSample
                            {
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Hello {myVar.ToString().ToLower()} world";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_AutoGeneratedHeader_WithMethodChainInInterpolation()
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
                                public void TestMethod()
                                {
                                    var myVar = "test";
                                    var result = $"Hello {myVar.ToString().ToLower()} world";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}