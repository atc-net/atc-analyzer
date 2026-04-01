namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<GlobalUsingsAllAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "S2699:Add at least one assertion to this test case", Justification = "OK - RunAsync is the assertion")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class GlobalUsingsAllAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_SystemNamespace_WhenExcludeCommonDisabled()
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

        var test = new CSharpAnalyzerTest<GlobalUsingsAllAnalyzer, DefaultVerifier>
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
            dotnet_diagnostic.ATC220.exclude_common_namespaces = false
            """));

        await test.RunAsync();
    }

    [Fact]
    public async Task ReportsDiagnostic_SystemCollectionsGeneric_WhenExcludeCommonDisabled()
    {
        const string code = """
                            [|using System.Collections.Generic;|]

                            public class Sample
                            {
                                public static List<string> GetList() => new();
                            }
                            """;

        var test = new CSharpAnalyzerTest<GlobalUsingsAllAnalyzer, DefaultVerifier>
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
            dotnet_diagnostic.ATC220.exclude_common_namespaces = false
            """));

        await test.RunAsync();
    }

    [Fact]
    public async Task ReportsDiagnostic_ThirdPartyNamespace()
    {
        const string code = """
                            [|using Xunit;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_CustomNamespace()
    {
        const string code = """
                            [|using MyCompany.Utilities;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MicrosoftNamespace_WhenExcludeCommonDisabled()
    {
        const string code = """
                            [|using Microsoft.Extensions.Logging;|]

                            public class Sample
                            {
                            }
                            """;

        var test = new CSharpAnalyzerTest<GlobalUsingsAllAnalyzer, DefaultVerifier>
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
            dotnet_diagnostic.ATC220.exclude_common_namespaces = false
            """));

        await test.RunAsync();
    }

    [Fact]
    public async Task ReportsDiagnostic_AtcNamespace_WhenExcludeCommonDisabled()
    {
        const string code = """
                            [|using Atc.Utilities;|]

                            public class Sample
                            {
                            }
                            """;

        var test = new CSharpAnalyzerTest<GlobalUsingsAllAnalyzer, DefaultVerifier>
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
            dotnet_diagnostic.ATC220.exclude_common_namespaces = false
            """));

        await test.RunAsync();
    }

    [Fact]
    public async Task ReportsDiagnostic_MultipleNamespaces_OnlyNonCommon()
    {
        const string code = """
                            using System;
                            [|using Xunit;|]
                            [|using MyCompany.Utilities;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_AllNamespaces_WhenExcludeCommonDisabled()
    {
        const string code = """
                            [|using System;|]
                            [|using Xunit;|]
                            [|using MyCompany.Utilities;|]

                            public class Sample
                            {
                            }
                            """;

        var test = new CSharpAnalyzerTest<GlobalUsingsAllAnalyzer, DefaultVerifier>
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
            dotnet_diagnostic.ATC220.exclude_common_namespaces = false
            """));

        await test.RunAsync();
    }
}