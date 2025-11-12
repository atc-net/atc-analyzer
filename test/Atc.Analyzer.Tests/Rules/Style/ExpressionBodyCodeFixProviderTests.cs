namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<ExpressionBodyAnalyzer, ExpressionBodyCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class ExpressionBodyCodeFixProviderTests
{
    // ===== CONVERT BLOCK BODY TO EXPRESSION BODY =====
    [Fact]
    public Task FixMethod_ConvertToExpressionBody_SimpleReturn()
    {
        const string source = """
                              public class Sample
                              {
                                  public static string GetData()
                                  {
                                      return "Sample Data";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static string GetData() => "Sample Data";
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 26, 3, 33),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_IntReturn()
    {
        const string source = """
                              public class Sample
                              {
                                  public static int GetNumber()
                                  {
                                      return 42;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static int GetNumber() => 42;
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 23, 3, 32),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_Calculation()
    {
        const string source = """
                              public class Sample
                              {
                                  public static int Calculate(int x, int y)
                                  {
                                      return x + y;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static int Calculate(int x, int y) => x + y;
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 23, 3, 32),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_MethodCall()
    {
        const string source = """
                              public class Sample
                              {
                                  public static string GetUpperCase(string input)
                                  {
                                      return input.ToUpper();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static string GetUpperCase(string input) => input.ToUpper();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 26, 3, 38),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_TernaryExpression()
    {
        const string source = """
                              public class Sample
                              {
                                  public static int GetValue(bool condition)
                                  {
                                      return condition ? 1 : 0;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static int GetValue(bool condition) => condition ? 1 : 0;
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 23, 3, 31),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_NullCoalescing()
    {
        const string source = """
                              public class Sample
                              {
                                  public static string GetName(string? input)
                                  {
                                      return input ?? "default";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static string GetName(string? input) => input ?? "default";
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 26, 3, 33),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_AsyncMethod()
    {
        const string source = """
                              using System.Threading.Tasks;

                              public class Sample
                              {
                                  public static async Task<string> GetAsync()
                                  {
                                      return await Task.FromResult("x");
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Threading.Tasks;

                                   public class Sample
                                   {
                                       public static async Task<string> GetAsync() => await Task.FromResult("x");
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 38, 5, 46),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_Lambda()
    {
        const string source = """
                              using System;

                              public class Sample
                              {
                                  public static Func<int, int> GetMultiplier(int factor)
                                  {
                                      return x => x * factor;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System;

                                   public class Sample
                                   {
                                       public static Func<int, int> GetMultiplier(int factor) => x => x * factor;
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 34, 5, 47),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_NewObject()
    {
        const string source = """
                              public class Sample
                              {
                                  public static object CreateObject()
                                  {
                                      return new object();
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static object CreateObject() => new object();
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 26, 3, 38),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_Array()
    {
        const string source = """
                              public class Sample
                              {
                                  public static int[] GetArray()
                                  {
                                      return new[] { 1, 2, 3 };
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static int[] GetArray() => new[] { 1, 2, 3 };
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 25, 3, 33),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_GenericMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  public static T GetDefault<T>()
                                  {
                                      return default(T)!;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static T GetDefault<T>() => default(T)!;
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 21, 3, 31),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_PrivateMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  private static string GetData()
                                  {
                                      return "Sample Data";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       private static string GetData() => "Sample Data";
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 27, 3, 34),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixMethod_ConvertToExpressionBody_InstanceMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  public string GetData()
                                  {
                                      return "Sample Data";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public string GetData() => "Sample Data";
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 19, 3, 26),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public Task FixAccessor_ConvertToExpressionBody_GetAccessor()
    {
        const string source = """
                              public class Sample
                              {
                                  private string name = string.Empty;

                                  public string Name
                                  {
                                      get
                                      {
                                          return name;
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       private string name = string.Empty;

                                       public string Name
                                       {
                                           get => name;
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 9, 7, 12),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    // ===== MOVE ARROW TO NEW LINE (LINE TOO LONG) =====
    [Fact]
    public Task FixMethod_MoveArrowToNewLine_LineTooLong()
    {
        const string source = """
                              public class Sample
                              {
                                  public static string GetData() => "Sample Dataaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public static string GetData()
                                           => "Sample Dataaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 36, 3, 38),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    // ===== MOVE ARROW TO NEW LINE (MULTI-LINE TERNARY) =====
    [Fact]
    public Task FixMethod_MoveArrowToNewLine_MultiLineTernary()
    {
        const string source = """
                              public class Sample
                              {
                                  private static string[] SetHelpArgumentIfNeeded(string[] args) =>
                                      args.Length == 0
                                          ? new[] { "help" }
                                          : args;
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       private static string[] SetHelpArgumentIfNeeded(string[] args)
                                           => args.Length == 0
                                               ? new[] { "help" }
                                               : args;
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC210",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 68, 3, 70),
        };

        return CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}