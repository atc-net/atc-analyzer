namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<GlobalUsingsAllAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class GlobalUsingsAllAnalyzerTests
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
    public async Task ReportsDiagnostic_MultipleNamespaces()
    {
        const string code = """
                            [|using System;|]
                            [|using Xunit;|]
                            [|using MyCompany.Utilities;|]

                            public class Sample
                            {
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}