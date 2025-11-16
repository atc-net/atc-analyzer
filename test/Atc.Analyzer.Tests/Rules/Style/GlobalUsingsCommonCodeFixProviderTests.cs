namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<GlobalUsingsCommonAnalyzer, GlobalUsingsCommonCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class GlobalUsingsCommonCodeFixProviderTests
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
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 14),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MoveSystemCollectionsGeneric()
    {
        const string source = """
                              using System.Collections.Generic;

                              public class Sample
                              {
                                  public static List<string> GetList() => new();
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static List<string> GetList() => new();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 34),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MoveSystemLinq()
    {
        const string source = """
                              using System.Linq;

                              public class Sample
                              {
                                  public static int GetCount(int[] values) => values.Count();
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static int GetCount(int[] values) => values.Count();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 19),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MoveMicrosoftNamespace()
    {
        const string source = """
                              using Microsoft.Extensions.Logging;

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
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 36),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MoveAtcNamespace()
    {
        const string source = """
                              using Atc.Utilities;

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
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 21),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MoveMultipleSystemUsings()
    {
        const string source = """
                              using System;
                              using System.Collections.Generic;
                              using System.Linq;

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
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 14),
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(2, 1, 2, 34),
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 1, 3, 19),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_PreserveThirdPartyUsings()
    {
        const string source = """
                              using System;
                              using Xunit;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   using Xunit;

                                   public class Sample
                                   {
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 14),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixUsing_MixedSystemAndThirdPartyUsings()
    {
        const string source = """
                              using System;
                              using Xunit;
                              using System.Collections.Generic;
                              using FluentAssertions;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   using Xunit;
                                   using FluentAssertions;

                                   public class Sample
                                   {
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 14),
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 1, 3, 34),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixUsing_OrderingSystemNamespacesFirst()
    {
        // Test that System namespaces are placed before all other namespaces in GlobalUsings.cs
        const string source = """
                              using Xunit;
                              using System.Linq;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   using Xunit;

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
                                               global using System.Linq;
                                               global using Atc.Utilities;
                                               global using Microsoft.Extensions.Logging;
                                               global using Newtonsoft.Json;
                                               """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(2, 1, 2, 19),
        };

        var test = new CSharpCodeFixTest<GlobalUsingsCommonAnalyzer, GlobalUsingsCommonCodeFixProvider, DefaultVerifier>
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
                              using Xunit;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   using Xunit;

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
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 19),
        };

        var test = new CSharpCodeFixTest<GlobalUsingsCommonAnalyzer, GlobalUsingsCommonCodeFixProvider, DefaultVerifier>
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
                              using Xunit;

                              public class Sample
                              {
                              }
                              """;

        const string fixedSource = """
                                   using Xunit;

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
                "ATC221",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 1, 1, 16),
        };

        var test = new CSharpCodeFixTest<GlobalUsingsCommonAnalyzer, GlobalUsingsCommonCodeFixProvider, DefaultVerifier>
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