namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<ParameterSeparationAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
public sealed class ParameterSeparationAnalyzerTests
{
    [Fact]
    public async Task MethodWithNoParameters_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod()
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithSingleShortParameter_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(int parameter1)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithLongNameButShortTotal_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyLoooooooooooooooooooooooooonMethod(int parameter1)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithSingleLongParameterCorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyLooooooooooooooooooooooooooooooooooooooooooonMethod(
                                    int parameter1)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithSingleLongParameterOnOneLine_ReportsDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyLooooooooooooooooooooooooooooooooooooooooooonMethod[|(int parameter1)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithTwoParametersCorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    int parameter1,
                                    int parameter2)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithTwoParametersOnOneLine_ReportsDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(int parameter1, int parameter2)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithThreeParametersCorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    int parameter1,
                                    int parameter2,
                                    int parameter3)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithThreeParametersOnOneLine_ReportsDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod[|(int parameter1, int parameter2, int parameter3)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ConstructorWithSingleParameter_NoDiagnostic()
    {
        const string code = """
                            public class MyClass
                            {
                                public MyClass(string name)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ConstructorWithMultipleParametersCorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class MyClass
                            {
                                public MyClass(
                                    string name,
                                    int age,
                                    bool isActive)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ConstructorWithMultipleParametersOnOneLine_ReportsDiagnostic()
    {
        const string code = """
                            public class MyClass
                            {
                                public MyClass[|(string name, int age, bool isActive)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AsyncMethodWithMultipleParametersCorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            using System.Threading.Tasks;

                            public class Sample
                            {
                                public async Task MyMethodAsync(
                                    int parameter1,
                                    string parameter2)
                                {
                                    await Task.CompletedTask;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task StaticMethodWithMultipleParametersCorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public static void MyMethod(
                                    int parameter1,
                                    string parameter2)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task PrivateMethodWithMultipleParametersOnOneLine_ReportsDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                private void MyMethod[|(int parameter1, string parameter2)|]
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithGenericTypeParameters_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod<T>(
                                    T parameter1,
                                    string parameter2)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithDefaultValues_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    int x = 0,
                                    string y = "test")
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithRefOutInParameters_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    ref int x,
                                    out string y,
                                    in bool z)
                                {
                                    y = "test";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithNullableTypes_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    int? x,
                                    string? y)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithAttributeOnParameter_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            using System.Diagnostics.CodeAnalysis;

                            public class Sample
                            {
                                public void MyMethod(
                                    [NotNull] string x,
                                    int y)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task VirtualMethodWithMultipleParameters_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public virtual void MyMethod(
                                    int parameter1,
                                    string parameter2)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task OverrideMethodWithMultipleParameters_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Base
                            {
                                public virtual void MyMethod(
                                    int parameter1,
                                    string parameter2)
                                {
                                }
                            }

                            public class Derived : Base
                            {
                                public override void MyMethod(
                                    int parameter1,
                                    string parameter2)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AbstractMethodWithMultipleParameters_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public abstract class Sample
                            {
                                public abstract void MyMethod(
                                    int parameter1,
                                    string parameter2);
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task LocalFunctionWithMultipleParameters_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void OuterMethod()
                                {
                                    void LocalFunction(
                                        int param1,
                                        string param2)
                                    {
                                    }

                                    LocalFunction(1, "test");
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task DelegateWithMultipleParameters_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public delegate void MyEventHandler(
                                    object sender,
                                    EventArgs e);
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithLongGenericType_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            using System.Collections.Generic;

                            public class Sample
                            {
                                public void MyMethod(
                                    Dictionary<string, List<int>> param1,
                                    int count)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithTupleParameter_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    (int x, string y) tuple,
                                    int count)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodWithArrayParameter_CorrectlyBroken_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(
                                    string[] items,
                                    int count)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task MethodInvocation_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var result = SomeMethod("a", "b", "c");
                                }

                                private string SomeMethod(
                                    string arg1,
                                    string arg2,
                                    string arg3)
                                {
                                    return arg1;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ConstructorInvocation_NoDiagnostic()
    {
        const string code = """
                            public class MyClass
                            {
                                public MyClass(
                                    string name,
                                    int age)
                                {
                                }
                            }

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    var obj = new MyClass("test", 25);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task TernaryOperator_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public string TestMethod(bool condition)
                                {
                                    return condition ? "value1" : "value2";
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task TernaryOperatorMultiline_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                public string TestMethod(
                                    bool removeLastVerb,
                                    string assemblyName)
                                {
                                    return removeLastVerb
                                        ? assemblyName.Substring(0, assemblyName.LastIndexOf(' '))
                                        : assemblyName;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task LambdaExpression_NoDiagnostic()
    {
        const string code = """
                            using System.Linq;

                            public class Sample
                            {
                                public void TestMethod(int[] items)
                                {
                                    var result = items.Where((item, index) => item > 0 && index > 0);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task SimpleLambda_NoDiagnostic()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public void TestMethod()
                                {
                                    Func<int, int, int> lambda = (x, y) => x + y;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task LinqQuery_NoDiagnostic()
    {
        const string code = """
                            using System.Linq;
                            using System.Collections.Generic;

                            public class Item
                            {
                                public bool IsActive { get; set; }
                                public string Name { get; set; } = string.Empty;
                                public int Value { get; set; }
                            }

                            public class Sample
                            {
                                public void TestMethod(List<Item> items)
                                {
                                    var query = from item in items
                                                where item.IsActive
                                                select new { item.Name, item.Value };
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ChainedMethodCalls_NoDiagnostic()
    {
        const string code = """
                            using System;

                            public class Sample
                            {
                                public void TestMethod(string assemblyName)
                                {
                                    var result = assemblyName.Replace("Api", "API", StringComparison.Ordinal)
                                                            .Replace("Old", "New", StringComparison.Ordinal);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task IndexerDeclaration_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                private string[,] matrix = new string[10, 10];

                                public string this[int row, int column]
                                {
                                    get => matrix[row, column];
                                    set => matrix[row, column] = value;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task IndexerUsage_NoDiagnostic()
    {
        const string code = """
                            public class Sample
                            {
                                private string[,] matrix = new string[10, 10];

                                public string this[int row, int column]
                                {
                                    get => matrix[row, column];
                                }

                                public void TestMethod()
                                {
                                    var value = this[0, 1];
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GeneratedCodeClass_WithMultipleParametersOnOneLine_NoDiagnostic()
    {
        const string code = """
                            using System.CodeDom.Compiler;

                            [GeneratedCode("MyGen", "1.0.1")]
                            public class GeneratedSample
                            {
                                public void MethodWithTwoParameters(int x, string y)
                                {
                                }

                                public void MethodWithThreeParameters(int x, string y, bool z)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GeneratedCodeClass_WithMixedTypes_NoDiagnostic()
    {
        const string code = """
                            using System.CodeDom.Compiler;
                            using System.Collections.Generic;

                            [GeneratedCode("ApiGenerator", "2.0")]
                            public class GeneratedApiClient
                            {
                                public void MethodWithMixedTypes(int id, string name, List<int> items, bool isActive)
                                {
                                }

                                public static void StaticMethodWithMultipleParams(int x, int y)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task GeneratedCodeClass_AsyncMethodWithMultipleParams_NoDiagnostic()
    {
        const string code = """
                            using System.CodeDom.Compiler;
                            using System.Threading.Tasks;

                            [GeneratedCode("T4", "1.0")]
                            public class GeneratedModel
                            {
                                public async Task ExecuteAsync(int id, string name)
                                {
                                    await Task.CompletedTask;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AutoGeneratedHeader_WithMultipleParametersOnOneLine_NoDiagnostic()
    {
        const string code = """
                            //------------------------------------------------------------------------------
                            // This code was auto-generated by ApiGenerator 2.0.
                            //
                            // Changes to this file may cause incorrect behavior and will be lost if
                            // the code is regenerated.
                            //------------------------------------------------------------------------------
                            namespace MyNamespace;

                            public class GeneratedClass
                            {
                                public void MethodWithTwoParameters(int x, string y)
                                {
                                }

                                public void MethodWithThreeParameters(int x, string y, bool z)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AutoGeneratedHeader_CaseInsensitive_NoDiagnostic()
    {
        const string code = """
                            // This file is AUTO-GENERATED by code generator
                            namespace MyNamespace;

                            public class GeneratedClass
                            {
                                public void Method(int a, string b, bool c)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task AutoGeneratedHeader_WithMultiLineComment_NoDiagnostic()
    {
        const string code = """
                            /*
                             * This code was auto-generated
                             * Do not modify manually
                             */
                            using System.Collections.Generic;

                            namespace MyNamespace;

                            public class GeneratedClass
                            {
                                public void Execute(int id, string name, List<int> items, bool isActive)
                                {
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}