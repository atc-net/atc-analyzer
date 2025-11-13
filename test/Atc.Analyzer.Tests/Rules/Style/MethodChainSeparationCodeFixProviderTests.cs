namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<MethodChainSeparationAnalyzer, MethodChainSeparationCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class MethodChainSeparationCodeFixProviderTests
{
    [Fact]
    public Task FixTwoMethodChainsOnOneLine()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod(string str)
                                  {
                                      var result = str.Trim().ToLower();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod(string str)
                                       {
                                           var result = str
                                               .Trim()
                                               .ToLower();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 22, 5, 42),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixThreeMethodChainsOnOneLine()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod(string str)
                                  {
                                      var result = str.Trim().Replace("xxx", "x").ToLower();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod(string str)
                                       {
                                           var result = str
                                               .Trim()
                                               .Replace("xxx", "x")
                                               .ToLower();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 22, 5, 62),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixLinqMethodChainsOnOneLine()
    {
        const string source = """
                              using System.Linq;
                              using System.Collections.Generic;

                              public class Sample
                              {
                                  public void TestMethod(List<int> numbers)
                                  {
                                      var result = numbers.Where(x => x > 0).Select(x => x * 2).ToList();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Linq;
                                   using System.Collections.Generic;

                                   public class Sample
                                   {
                                       public void TestMethod(List<int> numbers)
                                       {
                                           var result = numbers
                                               .Where(x => x > 0)
                                               .Select(x => x * 2)
                                               .ToList();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(8, 22, 8, 75),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixStringBuilderChainsOnOneLine()
    {
        const string source = """
                              using System.Text;

                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var result = new StringBuilder().Append("hello").Append(" world").ToString();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Text;

                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var result = new StringBuilder()
                                               .Append("hello")
                                               .Append(" world")
                                               .ToString();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 22, 7, 85),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethodChainInReturnStatement()
    {
        const string source = """
                              public class Sample
                              {
                                  public string TestMethod(string str)
                                  {
                                      return str.Trim().ToLower();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public string TestMethod(string str)
                                       {
                                           return str
                                               .Trim()
                                               .ToLower();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 16, 5, 36),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixAwaitMethodChainOnOneLine()
    {
        const string source = """
                              using System.Threading.Tasks;

                              public class Sample
                              {
                                  private Task<string> dataTask = Task.FromResult("data");

                                  public async Task TestMethod()
                                  {
                                      var result = await dataTask.ContinueWith(t => t.Result).ConfigureAwait(false);
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Threading.Tasks;

                                   public class Sample
                                   {
                                       private Task<string> dataTask = Task.FromResult("data");

                                       public async Task TestMethod()
                                       {
                                           var result = await dataTask
                                               .ContinueWith(t => t.Result)
                                               .ConfigureAwait(false);
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(9, 22, 9, 86),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethodChainWithComplexArguments()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod(string str)
                                  {
                                      var result = str.Replace("xxx", "x").Replace("yyy", "y").Trim();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod(string str)
                                       {
                                           var result = str
                                               .Replace("xxx", "x")
                                               .Replace("yyy", "y")
                                               .Trim();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 22, 5, 72),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethodChainStartingWithNew()
    {
        const string source = """
                              using System;

                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var result = new Uri("http://example.com").ToString().ToLower();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System;

                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var result = new Uri("http://example.com")
                                               .ToString()
                                               .ToLower();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 22, 7, 72),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethodChainInAssignment()
    {
        const string source = """
                              public class Sample
                              {
                                  public void TestMethod(string str)
                                  {
                                      string result;
                                      result = str.Trim().ToLower();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void TestMethod(string str)
                                       {
                                           string result;
                                           result = str
                                               .Trim()
                                               .ToLower();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(6, 18, 6, 38),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethodChainInMethodArgument()
    {
        const string source = """
                              using System;

                              public class Sample
                              {
                                  public void TestMethod(string str)
                                  {
                                      Console.WriteLine(str.Trim().ToLower());
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System;

                                   public class Sample
                                   {
                                       public void TestMethod(string str)
                                       {
                                           Console.WriteLine(str
                                               .Trim()
                                               .ToLower());
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 27, 7, 47),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixDeepNestedIndentation()
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
                                              var result = "test".Trim().ToLower();
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
                                                   var result = "test"
                                                       .Trim()
                                                       .ToLower();
                                               }
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(9, 30, 9, 53),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixStringBuilderWithFourMethodCalls()
    {
        const string source = """
                              using System.Text;

                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var sb = new StringBuilder().Append("Hello").Append(" ").Append("World").ToString();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Text;

                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var sb = new StringBuilder()
                                               .Append("Hello")
                                               .Append(" ")
                                               .Append("World")
                                               .ToString();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 18, 7, 92),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixStringBuilderWithNewKeyword()
    {
        const string source = """
                              using System.Text;

                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var result = new StringBuilder().Append("Test").ToString();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Text;

                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var result = new StringBuilder()
                                               .Append("Test")
                                               .ToString();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 22, 7, 67),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixStringBuilderWithPartiallyFormattedChain()
    {
        const string source = """
                              using System.Text;

                              public class Sample
                              {
                                  public void TestMethod()
                                  {
                                      var sb = new StringBuilder().Append("Hello")
                                          .Append(" ").Append("World")
                                          .ToString();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Text;

                                   public class Sample
                                   {
                                       public void TestMethod()
                                       {
                                           var sb = new StringBuilder()
                                               .Append("Hello")
                                               .Append(" ")
                                               .Append("World")
                                               .ToString();
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC203",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 18, 9, 24),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}