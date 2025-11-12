namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<ParameterInlineAnalyzer, ParameterInlineCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class ParameterInlineCodeFixProviderTests
{
    // ===== INLINE FIXES (move parameter to same line) =====
    [Fact]
    public Task FixMethod_MoveParameterInline_ShortMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  public void ShortMethod(
                                      int parameter1)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void ShortMethod(int parameter1)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 28, 4, 24),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_MoveParameterInline_WithModifiers()
    {
        const string source = """
                              public class Sample
                              {
                                  public static void MyMethod(
                                      int parameter1)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static void MyMethod(int parameter1)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 32, 4, 24),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_MoveParameterInline_AsyncMethod()
    {
        const string source = """
                              using System.Threading.Tasks;

                              public class Sample
                              {
                                  public async Task MyMethodAsync(
                                      int parameter1)
                                  {
                                      await Task.CompletedTask;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Threading.Tasks;

                                   public class Sample
                                   {
                                       public async Task MyMethodAsync(int parameter1)
                                       {
                                           await Task.CompletedTask;
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 36, 6, 24),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixConstructor_MoveParameterInline()
    {
        const string source = """
                              public class MyClass
                              {
                                  public MyClass(
                                      string name)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class MyClass
                                   {
                                       public MyClass(string name)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 19, 4, 21),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixLocalFunction_MoveParameterInline()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process()
                                  {
                                      int Calculate(
                                          int x)
                                      {
                                          return x * 2;
                                      }

                                      var result = Calculate(5);
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process()
                                       {
                                           int Calculate(int x)
                                           {
                                               return x * 2;
                                           }

                                           var result = Calculate(5);
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 22, 6, 19),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixDelegate_MoveParameterInline()
    {
        const string source = """
                              using System;

                              public delegate void Handler(
                                  object sender);
                              """;

        const string fixedSource = """
                                   using System;

                                   public delegate void Handler(object sender);
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 29, 4, 19),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_MoveParameterInline_WithDefaultValue()
    {
        const string source = """
                              public class Sample
                              {
                                  public void MyMethod(
                                      int count = 10)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void MyMethod(int count = 10)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 25, 4, 24),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_MoveParameterInline_RefParameter()
    {
        const string source = """
                              public class Sample
                              {
                                  public void MyMethod(
                                      ref int value)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void MyMethod(ref int value)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 25, 4, 23),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    // ===== SEPARATE LINE FIXES (move parameter to new line) =====
    [Fact]
    public Task FixMethod_MoveParameterToNewLine_LongDeclaration()
    {
        const string source = """
                              public class Sample
                              {
                                  public void MyLooooooooooooooooooooooooooooooooooooooooooonMethod(int parameter1)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void MyLooooooooooooooooooooooooooooooooooooooooooonMethod(
                                           int parameter1)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 70, 3, 86),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_MoveParameterToNewLine_LongWithModifiers()
    {
        const string source = """
                              public class Sample
                              {
                                  public static async System.Threading.Tasks.Task MyLoooooooooooonMethod(int x)
                                  {
                                      await System.Threading.Tasks.Task.CompletedTask;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static async System.Threading.Tasks.Task MyLoooooooooooonMethod(
                                           int x)
                                       {
                                           await System.Threading.Tasks.Task.CompletedTask;
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 75, 3, 82),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixConstructor_MoveParameterToNewLine_LongDeclaration()
    {
        const string source = """
                              public class MyVeryLongClassNameThatExceedsTheMaximumAllowedLineLength
                              {
                                  public MyVeryLongClassNameThatExceedsTheMaximumAllowedLineLength(string name)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class MyVeryLongClassNameThatExceedsTheMaximumAllowedLineLength
                                   {
                                       public MyVeryLongClassNameThatExceedsTheMaximumAllowedLineLength(
                                           string name)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 69, 3, 82),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixDelegate_MoveParameterToNewLine_LongDeclaration()
    {
        const string source = """
                              using System;

                              public delegate void MyVeryLongDelegateNameThatExceedsTheMaximumAllowedLength(int x);
                              """;

        const string fixedSource = """
                                   using System;

                                   public delegate void MyVeryLongDelegateNameThatExceedsTheMaximumAllowedLength(
                                       int x);
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC201",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 78, 3, 85),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}