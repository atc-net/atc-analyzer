namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using CodeFixVerifier = CSharpCodeFixVerifier<ParameterSeparationAnalyzer, ParameterSeparationCodeFixProvider>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class ParameterSeparationCodeFixProviderTests
{
    [Fact]
    public async Task FixMethod_TwoParametersOnSameLine()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process(int id, string name)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process(
                                           int id,
                                           string name)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 24, 3, 45),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_ThreeParametersOnSameLine()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process(int id, string name, bool active)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process(
                                           int id,
                                           string name,
                                           bool active)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 24, 3, 58),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithModifiers()
    {
        const string source = """
                              using System.Threading.Tasks;

                              public class Sample
                              {
                                  public static async Task ProcessAsync(int id, string name)
                                  {
                                      await Task.CompletedTask;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Threading.Tasks;

                                   public class Sample
                                   {
                                       public static async Task ProcessAsync(
                                           int id,
                                           string name)
                                       {
                                           await Task.CompletedTask;
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 42, 5, 63),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithDefaultValues()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process(int id = 0, string name = "default")
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process(
                                           int id = 0,
                                           string name = "default")
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 24, 3, 61),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithNullableParameters()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process(int? id, string? name)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process(
                                           int? id,
                                           string? name)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 24, 3, 47),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithGenericParameters()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process<T>(T value, string name)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process<T>(
                                           T value,
                                           string name)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 27, 3, 49),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixConstructor_TwoParametersOnSameLine()
    {
        const string source = """
                              public class Sample
                              {
                                  public Sample(int id, string name)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public Sample(
                                           int id,
                                           string name)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 18, 3, 39),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixConstructor_WithMultipleParameters()
    {
        const string source = """
                              using System;

                              public class Sample
                              {
                                  public Sample(int id, string name, bool active, DateTime created)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System;

                                   public class Sample
                                   {
                                       public Sample(
                                           int id,
                                           string name,
                                           bool active,
                                           DateTime created)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 18, 5, 70),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixLocalFunction_TwoParametersOnSameLine()
    {
        const string source = """
                              public class Sample
                              {
                                  public void ProcessData()
                                  {
                                      int Calculate(int x, int y)
                                      {
                                          return x + y;
                                      }

                                      var result = Calculate(1, 2);
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void ProcessData()
                                       {
                                           int Calculate(
                                               int x,
                                               int y)
                                           {
                                               return x + y;
                                           }

                                           var result = Calculate(1, 2);
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 22, 5, 36),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixDelegate_TwoParametersOnSameLine()
    {
        const string source = """
                              using System;

                              public delegate void ProcessHandler(object sender, EventArgs e);
                              """;

        const string fixedSource = """
                                   using System;

                                   public delegate void ProcessHandler(
                                       object sender,
                                       EventArgs e);
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 36, 3, 64),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixDelegate_ThreeParametersOnSameLine()
    {
        const string source = """
                              using System;

                              public delegate void ProcessHandler(object sender, EventArgs e, string message);
                              """;

        const string fixedSource = """
                                   using System;

                                   public delegate void ProcessHandler(
                                       object sender,
                                       EventArgs e,
                                       string message);
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 36, 3, 80),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_NestedClass()
    {
        const string source = """
                              public class Outer
                              {
                                  public class Inner
                                  {
                                      public void Process(int id, string name)
                                      {
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Outer
                                   {
                                       public class Inner
                                       {
                                           public void Process(
                                               int id,
                                               string name)
                                           {
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 28, 5, 49),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_DeeplyNestedClass()
    {
        const string source = """
                              public class Level1
                              {
                                  public class Level2
                                  {
                                      public class Level3
                                      {
                                          public void Process(int id, string name)
                                          {
                                          }
                                      }
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Level1
                                   {
                                       public class Level2
                                       {
                                           public class Level3
                                           {
                                               public void Process(
                                                   int id,
                                                   string name)
                                               {
                                               }
                                           }
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(7, 32, 7, 53),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithAttributes()
    {
        const string source = """
                              using System;

                              public class Sample
                              {
                                  [Obsolete("Use ProcessNew instead")]
                                  public void Process(int id, string name)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System;

                                   public class Sample
                                   {
                                       [Obsolete("Use ProcessNew instead")]
                                       public void Process(
                                           int id,
                                           string name)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(6, 24, 6, 45),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithParamModifiers()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process(ref int id, out string name)
                                  {
                                      name = "test";
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process(
                                           ref int id,
                                           out string name)
                                       {
                                           name = "test";
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 24, 3, 53),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithParamsKeyword()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process(string name, params int[] values)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void Process(
                                           string name,
                                           params int[] values)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 24, 3, 58),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_WithComplexTypes()
    {
        const string source = """
                              using System.Collections.Generic;

                              public class Sample
                              {
                                  public void Process(Dictionary<string, int> data, List<string> names)
                                  {
                                  }
                              }
                              """;

        const string fixedSource = """
                                   using System.Collections.Generic;

                                   public class Sample
                                   {
                                       public void Process(
                                           Dictionary<string, int> data,
                                           List<string> names)
                                       {
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 24, 5, 74),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_InterfaceMethod()
    {
        const string source = """
                              public interface ISample
                              {
                                  void Process(int id, string name);
                              }
                              """;

        const string fixedSource = """
                                   public interface ISample
                                   {
                                       void Process(
                                           int id,
                                           string name);
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 17, 3, 38),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_AbstractMethod()
    {
        const string source = """
                              public abstract class Sample
                              {
                                  public abstract void Process(int id, string name);
                              }
                              """;

        const string fixedSource = """
                                   public abstract class Sample
                                   {
                                       public abstract void Process(
                                           int id,
                                           string name);
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 33, 3, 54),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixMethod_ExpressionBodiedMethod()
    {
        const string source = """
                              public class Sample
                              {
                                  public int Add(int x, int y) => x + y;
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public int Add(
                                           int x,
                                           int y) => x + y;
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(3, 19, 3, 33),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact(Skip = "Primary constructors are not supported by ParameterSeparationAnalyzer")]
    public async Task FixConstructor_PrimaryConstructor()
    {
        const string source = """
                              public class Sample(int id, string name)
                              {
                              }
                              """;

        const string fixedSource = """
                                   public class Sample(
                                       int id,
                                       string name)
                                   {
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 13, 1, 34),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact(Skip = "Primary constructors (record) are not supported by ParameterSeparationAnalyzer")]
    public async Task FixMethod_RecordPrimaryConstructor()
    {
        const string source = """
                              public record Sample(int Id, string Name);
                              """;

        const string fixedSource = """
                                   public record Sample(
                                       int Id,
                                       string Name);
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(1, 14, 1, 34),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Fact]
    public async Task FixLocalFunction_StaticLocalFunction()
    {
        const string source = """
                              public class Sample
                              {
                                  public void ProcessData()
                                  {
                                      static int Calculate(int x, int y)
                                      {
                                          return x + y;
                                      }

                                      var result = Calculate(1, 2);
                                  }
                              }
                              """;

        const string fixedSource = """
                                   public class Sample
                                   {
                                       public void ProcessData()
                                       {
                                           static int Calculate(
                                               int x,
                                               int y)
                                           {
                                               return x + y;
                                           }

                                           var result = Calculate(1, 2);
                                       }
                                   }
                                   """;

        var expected = new[]
        {
            new DiagnosticResult(
                "ATC202",
                DiagnosticSeverity.Warning)
                .WithSpan(5, 29, 5, 43),
        };

        await CodeFixVerifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}