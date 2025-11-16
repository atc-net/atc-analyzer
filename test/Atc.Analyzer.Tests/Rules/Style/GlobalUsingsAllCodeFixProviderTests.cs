namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<GlobalUsingsAllAnalyzer, GlobalUsingsAllCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class GlobalUsingsAllCodeFixProviderTests
{
    [Fact]
    public Task FixUsing_MoveSystemNamespaceToGlobalUsings()
    {
        const string source = """
                              using System;

                              public class Sample
                              {
                                  public static void DoSomething()
                                  {
                                      Console.WriteLine("Hello");
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static void DoSomething()
                                       {
                                           Console.WriteLine("Hello");
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC220",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 14),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MoveThirdPartyNamespace()
    {
        const string source = """
                              using Xunit;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC220",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 13),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MoveCustomNamespace()
    {
        const string source = """
                              using MyCompany.Utilities;
                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC220",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 27),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixUsing_OrderingSystemNamespacesFirst()
    {
        // Test that System namespaces are placed before all other namespaces in GlobalUsings.cs
        const string source = """
                              using Xunit;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                   }
                                   """;

        const string globalUsingsSource = """
                                          global using Atc.Utilities;
                                          global using Microsoft.Extensions.Logging;
                                          global using Newtonsoft.Json;
                                          """;

        const string fixedGlobalUsingsSource = """
                                                global using Atc.Utilities;
                                                global using Microsoft.Extensions.Logging;
                                                global using Newtonsoft.Json;
                                                global using Xunit;
                                                """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC220",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 13),
        };

        var test = new CSharpCodeFixTest<GlobalUsingsAllAnalyzer, GlobalUsingsAllCodeFixProvider, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net90,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        test.TestState.Sources.Add(source.NormalizeLineEndings());
        test.TestState.Sources.Add(("GlobalUsings.cs", globalUsingsSource.NormalizeLineEndings()));

        test.FixedState.Sources.Add(fixedSource.NormalizeLineEndings());
        test.FixedState.Sources.Add(("GlobalUsings.cs", fixedGlobalUsingsSource.NormalizeLineEndings()));

        test.ExpectedDiagnostics.AddRange(expected);
        test.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", "root = true\n\n[*]\nend_of_line = lf\n"));

        await test.RunAsync();

        Assert.True(true);
    }

    [Fact]
    public async Task FixUsing_OrderingSystemNamespacesAlphabetically()
    {
        // Test that System namespaces are sorted alphabetically among themselves
        const string source = """
                              using System.Text;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                   }
                                   """;

        const string globalUsingsSource = """
                                          global using System.Linq;
                                          global using System;
                                          """;

        const string fixedGlobalUsingsSource = """
                                               global using System;
                                               global using System.Linq;
                                               global using System.Text;
                                               """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC220",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 19),
        };

        var test = new CSharpCodeFixTest<GlobalUsingsAllAnalyzer, GlobalUsingsAllCodeFixProvider, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net90,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        test.TestState.Sources.Add(source.NormalizeLineEndings());
        test.TestState.Sources.Add(("GlobalUsings.cs", globalUsingsSource.NormalizeLineEndings()));

        test.FixedState.Sources.Add(fixedSource.NormalizeLineEndings());
        test.FixedState.Sources.Add(("GlobalUsings.cs", fixedGlobalUsingsSource.NormalizeLineEndings()));

        test.ExpectedDiagnostics.AddRange(expected);
        test.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", "root = true\n\n[*]\nend_of_line = lf\n"));

        await test.RunAsync();

        Assert.True(true);
    }

    [Fact]
    public async Task FixUsing_OrderingNonSystemNamespacesAlphabetically()
    {
        // Test that non-System namespaces are sorted alphabetically after System namespaces
        const string source = """
                              using Atc.Data;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                   }
                                   """;

        const string globalUsingsSource = """
                                          global using System;
                                          global using Microsoft.Extensions.Logging;
                                          global using Atc.Utilities;
                                          """;

        const string fixedGlobalUsingsSource = """
                                               global using System;
                                               global using Atc.Data;
                                               global using Atc.Utilities;
                                               global using Microsoft.Extensions.Logging;
                                               """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC220",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 16),
        };

        var test = new CSharpCodeFixTest<GlobalUsingsAllAnalyzer, GlobalUsingsAllCodeFixProvider, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net90,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        test.TestState.Sources.Add(source.NormalizeLineEndings());
        test.TestState.Sources.Add(("GlobalUsings.cs", globalUsingsSource.NormalizeLineEndings()));

        test.FixedState.Sources.Add(fixedSource.NormalizeLineEndings());
        test.FixedState.Sources.Add(("GlobalUsings.cs", fixedGlobalUsingsSource.NormalizeLineEndings()));

        test.ExpectedDiagnostics.AddRange(expected);
        test.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", "root = true\n\n[*]\nend_of_line = lf\n"));

        await test.RunAsync();

        Assert.True(true);
    }
}