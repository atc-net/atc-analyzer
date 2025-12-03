namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<GlobalUsingsCommonAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class GlobalUsingsCommonAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_SystemNamespace()
    {
        const string code = """
                            [|using System;|]

                            public class Sample
                            {
                                public static void DoSomething()
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_SystemCollectionsGeneric()
    {
        const string code = """
                            [|using System.Collections.Generic;|]

                            public class Sample
                            {
                                public static List<string> GetList() => new();
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_SystemLinq()
    {
        const string code = """
                            [|using System.Linq;|]

                            public class Sample
                            {
                                public static int GetCount(int[] values) => values.Count();
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_SystemText()
    {
        const string code = """
                            [|using System.Text;|]

                            public class Sample
                            {
                                public static StringBuilder CreateBuilder() => new();
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MicrosoftNamespace()
    {
        const string code = """
                            [|using Microsoft.Extensions.Logging;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MicrosoftCodeAnalysis()
    {
        const string code = """
                            [|using Microsoft.CodeAnalysis;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_AtcNamespace()
    {
        const string code = """
                            [|using Atc.Utilities;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MultipleSystemUsings()
    {
        const string code = """
                            [|using System;|]
                            [|using System.Collections.Generic;|]
                            [|using System.Linq;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MixedSystemAndThirdParty()
    {
        const string code = """
                            [|using System;|]
                            using Xunit;
                            [|using System.Collections.Generic;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_SystemThreadingTasks()
    {
        const string code = """
                            [|using System.Threading.Tasks;|]

                            public class Sample
                            {
                                public static async Task DoSomethingAsync()
                                {
                                    await Task.CompletedTask;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_SystemIO()
    {
        const string code = """
                            [|using System.IO;|]

                            public class Sample
                            {
                                public static string ReadFile(string path) => File.ReadAllText(path);
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_AllTargetNamespaces()
    {
        const string code = """
                            [|using System;|]
                            [|using Microsoft.Extensions.Configuration;|]
                            [|using Atc.Data;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    [SuppressMessage("", "S2699:Add at least one assertion to this test case", Justification = "OK - RunAsync is the assertion")]
    public async Task ReportsDiagnostic_CustomNamespacePrefix_WithCustomPrefixes()
    {
        // Custom prefix should trigger diagnostic when ONLY custom prefix is configured
        const string code = """
                            [|using MyCompany.Utilities;|]
                            using ThirdParty.Library;
                            using System;

                            public class Sample
                            {
                            }
                            """;

        var test = new CSharpAnalyzerTest<GlobalUsingsCommonAnalyzer, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net90,
            TestCode = code,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        // Use ONLY MyCompany as prefix - this verifies custom prefixes work
        // System should NOT be flagged with this config (only MyCompany should be)
        test.TestState.AnalyzerConfigFiles.Add((
            "/.editorconfig",
            """
            root = true

            [*]
            dotnet_diagnostic.ATC221.namespace_prefixes = MyCompany
            """));

        await test.RunAsync();
    }
}