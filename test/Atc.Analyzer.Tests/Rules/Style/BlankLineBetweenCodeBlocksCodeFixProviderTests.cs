namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<BlankLineBetweenCodeBlocksAnalyzer, BlankLineBetweenCodeBlocksCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class BlankLineBetweenCodeBlocksCodeFixProviderTests
{
    [Fact]
    public async Task Fix_AddBlankLineBetweenTwoIfStatements()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Method()
                                  {
                                      if (true)
                                      {
                                      }
                                      if (true)
                                      {
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Method()
                                       {
                                           if (true)
                                           {
                                           }

                                           if (true)
                                           {
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC230",
                DiagnosticSeverity.Warning)
                .WithSpan(8, 9, 10, 10),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task Fix_AddBlankLineBetweenForAndWhile()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Method()
                                  {
                                      for (int i = 0; i < 10; i++)
                                      {
                                      }
                                      while (true)
                                      {
                                          break;
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Method()
                                       {
                                           for (int i = 0; i < 10; i++)
                                           {
                                           }

                                           while (true)
                                           {
                                               break;
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC230",
                DiagnosticSeverity.Warning)
                .WithSpan(8, 9, 11, 10),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task Fix_RemoveExtraBlankLinesBetweenTwoIfStatements()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Method()
                                  {
                                      if (true)
                                      {
                                      }


                                      if (true)
                                      {
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Method()
                                       {
                                           if (true)
                                           {
                                           }

                                           if (true)
                                           {
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC230",
                DiagnosticSeverity.Warning)
                .WithSpan(10, 9, 12, 10),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task Fix_RemoveThreeExtraBlankLines()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Method()
                                  {
                                      if (true)
                                      {
                                      }



                                      if (true)
                                      {
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Method()
                                       {
                                           if (true)
                                           {
                                           }

                                           if (true)
                                           {
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC230",
                DiagnosticSeverity.Warning)
                .WithSpan(11, 9, 13, 10),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task Fix_AddBlankLineInNestedBlock()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Method()
                                  {
                                      if (true)
                                      {
                                          if (true)
                                          {
                                          }
                                          if (true)
                                          {
                                          }
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Method()
                                       {
                                           if (true)
                                           {
                                               if (true)
                                               {
                                               }

                                               if (true)
                                               {
                                               }
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC230",
                DiagnosticSeverity.Warning)
                .WithSpan(10, 13, 12, 14),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task Fix_AddBlankLineBetweenForeachAndSwitch()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Method(int[] items)
                                  {
                                      foreach (var item in items)
                                      {
                                      }
                                      switch (items.Length)
                                      {
                                          default:
                                              break;
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Method(int[] items)
                                       {
                                           foreach (var item in items)
                                           {
                                           }

                                           switch (items.Length)
                                           {
                                               default:
                                                   break;
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC230",
                DiagnosticSeverity.Warning)
                .WithSpan(8, 9, 12, 10),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task Fix_AddBlankLineBetweenTryBlocks()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Method()
                                  {
                                      try
                                      {
                                      }
                                      catch
                                      {
                                      }
                                      try
                                      {
                                      }
                                      catch
                                      {
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Method()
                                       {
                                           try
                                           {
                                           }
                                           catch
                                           {
                                           }

                                           try
                                           {
                                           }
                                           catch
                                           {
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC230",
                DiagnosticSeverity.Warning)
                .WithSpan(11, 9, 16, 10),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}