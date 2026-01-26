namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<InterpolationMethodChainAnalyzer, InterpolationMethodChainCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class InterpolationMethodChainCodeFixProviderTests
{
    [Fact]
    public Task FixTwoChainedMethodsInInterpolation()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "test";
                                      var result = $"Hello {myVar.ToString().ToLower()} world";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "test";
                                           var lowerCaseValue = myVar.ToString().ToLower();
                                           var result = $"Hello {lowerCaseValue} world";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 31, 6, 57),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixThreeChainedMethodsInInterpolation()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "test";
                                      var result = $"Hello {myVar.ToString().Trim().ToLower()} world";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "test";
                                           var lowerCaseValue = myVar.ToString().Trim().ToLower();
                                           var result = $"Hello {lowerCaseValue} world";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 31, 6, 64),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsInVerbatimString()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "test";
                                      var result = $@"Hello {myVar.ToString().ToLower()} world";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "test";
                                           var lowerCaseValue = myVar.ToString().ToLower();
                                           var result = $@"Hello {lowerCaseValue} world";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 32, 6, 58),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsInRawString()
    {
        const string source = """"
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "test";
                                      var result = $"""Hello {myVar.ToString().ToLower()} world""";
                                  }
                              }
                              """";

        const string fixedSource = """"
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "test";
                                           var lowerCaseValue = myVar.ToString().ToLower();
                                           var result = $"""Hello {lowerCaseValue} world""";
                                       }
                                   }
                                   """";

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 33, 6, 59),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsInReturnStatement()
    {
        const string source = """
                              public class Sample
                              {
                                  public string TestMethod()
                                  {
                                      var myVar = "test";
                                      return $"Hello {myVar.ToString().ToLower()} world";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public string TestMethod()
                                       {
                                           var myVar = "test";
                                           var lowerCaseValue = myVar.ToString().ToLower();
                                           return $"Hello {lowerCaseValue} world";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 25, 6, 51),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsInMethodArgument()
    {
        const string source = """
                              using System;

                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "test";
                                      Console.WriteLine($"Hello {myVar.ToString().ToLower()} world");
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System;

                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "test";
                                           var lowerCaseValue = myVar.ToString().ToLower();
                                           Console.WriteLine($"Hello {lowerCaseValue} world");
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(8, 36, 8, 62),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsWithTrimMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "  test  ";
                                      var result = $"Hello {myVar.ToString().Trim()} world";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "  test  ";
                                           var trimmedValue = myVar.ToString().Trim();
                                           var result = $"Hello {trimmedValue} world";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 31, 6, 54),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsWithReplaceMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "test";
                                      var result = $"Hello {myVar.Trim().Replace("a", "b")} world";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "test";
                                           var replacedValue = myVar.Trim().Replace("a", "b");
                                           var result = $"Hello {replacedValue} world";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 31, 6, 61),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsWithToUpperMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var myVar = "test";
                                      var result = $"Hello {myVar.ToString().ToUpper()} world";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var myVar = "test";
                                           var upperCaseValue = myVar.ToString().ToUpper();
                                           var result = $"Hello {upperCaseValue} world";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 31, 6, 57),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedLinqMethodsInInterpolation()
    {
        const string source = """
                              using System.Linq;
                              using System.Collections.Generic;

                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var items = new List<string> { "a", "b", "c" };
                                      var result = $"First: {items.First().ToUpper()}";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Linq;
                                   using System.Collections.Generic;

                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var items = new List<string> { "a", "b", "c" };
                                           var upperCaseValue = items.First().ToUpper();
                                           var result = $"First: {upperCaseValue}";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(9, 32, 9, 55),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixNestedInterpolationChainedMethods()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var inner = "inner";
                                      var result = $"Outer: {$"Inner: {inner.Trim().ToLower()}"}";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var inner = "inner";
                                           var lowerCaseValue = inner.Trim().ToLower();
                                           var result = $"Outer: {$"Inner: {lowerCaseValue}"}";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(6, 42, 6, 64),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixChainedMethodsDeepNested()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      if (true)
                                      {
                                          if (true)
                                          {
                                              var myVar = "test";
                                              var result = $"Hello {myVar.ToString().ToLower()} world";
                                          }
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           if (true)
                                           {
                                               if (true)
                                               {
                                                   var myVar = "test";
                                                   var lowerCaseValue = myVar.ToString().ToLower();
                                                   var result = $"Hello {lowerCaseValue} world";
                                               }
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC204",
                DiagnosticSeverity.Info)
                .WithSpan(10, 39, 10, 65),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}