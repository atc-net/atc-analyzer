namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<ParameterSeparationAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class ParameterSeparationAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_MethodWithTwoParametersOnOneLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(int parameter1, int parameter2)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MethodWithThreeParametersOnOneLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(int parameter1, int parameter2, int parameter3)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ConstructorWithMultipleParametersOnOneLine()
    {
        const string code = """
                            public class MyClass
                            {
                                public MyClass[|(string name, int age, bool isActive)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_PrivateMethodWithMultipleParametersOnOneLine()
    {
        const string code = """
                            public class Sample
                            {
                                private void MyMethod[|(int parameter1, string parameter2)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    [SuppressMessage("", "S2699:Add at least one assertion to this test case", Justification = "OK - RunAsync is the assertion")]
    public async Task ReportsDiagnostic_MethodWithThreeParametersOnOneLine_WithCustomMinParamCount()
    {
        // This method has 3 parameters on one line, and we set min_parameter_count = 3
        // so it SHOULD trigger a diagnostic
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(int parameter1, int parameter2, int parameter3)|]
                                {
                                }
                            }
                            """;

        var test = new CSharpAnalyzerTest<ParameterSeparationAnalyzer, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net90,
            TestCode = code,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        test.TestState.AnalyzerConfigFiles.Add((
            "/.editorconfig",
            """
            root = true

            [*]
            dotnet_diagnostic.ATC202.min_parameter_count = 3
            """));

        await test.RunAsync();
    }
}