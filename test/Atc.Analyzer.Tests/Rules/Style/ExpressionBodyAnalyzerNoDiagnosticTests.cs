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

    [Fact]
    [SuppressMessage("", "S2699:Add at least one assertion to this test case", Justification = "OK - RunAsync is the assertion")]
    public async Task NoDiagnostic_MethodWithLongExpressionBody_WithCustomMaxLineLength()
    {
        // This line is ~85 characters, which exceeds default 80 but is within configured 120
        const string code = """
                            public class Sample
                            {
                                public static string GetLongStringData() => "This is a very long string data value";
                            }
                            """;

        var test = new CSharpAnalyzerTest<ExpressionBodyAnalyzer, DefaultVerifier>
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
            dotnet_diagnostic.ATC210.max_line_length = 120
            """));

        await test.RunAsync();
    }

    [Fact]
    public async Task NoDiagnostic_GetAccessorWithLongExpressionBody()
    {
        // Property accessor expression bodies are excluded from line length checks
        const string code = """
                            public class Sample
                            {
                                private readonly ISettingsService settingsService = null!;
                                private readonly Camera Camera = null!;

                                public bool ShowOverlayTitle
                                {
                                    get => Camera.Overrides?.ShowOverlayTitle ?? settingsService.CameraDisplay.ShowOverlayTitle;
                                    set { }
                                }
                            }

                            public class Camera
                            {
                                public CameraOverrides? Overrides { get; set; }
                            }

                            public class CameraOverrides
                            {
                                public bool? ShowOverlayTitle { get; set; }
                            }

                            public interface ISettingsService
                            {
                                ICameraDisplay CameraDisplay { get; }
                            }

                            public interface ICameraDisplay
                            {
                                bool ShowOverlayTitle { get; }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_SetAccessorWithLongExpressionBody()
    {
        // Property accessor expression bodies are excluded from line length checks
        const string code = """
                            using System;

                            public class Sample
                            {
                                private string value = string.Empty;

                                public string Value
                                {
                                    get => value;
                                    set => this.value = value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null");
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_AccessorWithVeryLongNullCoalescingExpression()
    {
        // Even very long accessor expression bodies should not trigger diagnostics
        const string code = """
                            public class Sample
                            {
                                private readonly IService service = null!;

                                public string DisplayName
                                {
                                    get => service.Configuration?.Settings?.DisplayName ?? service.Configuration?.Settings?.DefaultName ?? "Default";
                                    set { }
                                }
                            }

                            public interface IService
                            {
                                IConfiguration? Configuration { get; }
                            }

                            public interface IConfiguration
                            {
                                ISettings? Settings { get; }
                            }

                            public interface ISettings
                            {
                                string? DisplayName { get; }
                                string? DefaultName { get; }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}