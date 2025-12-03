namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<ParameterSeparationAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("", "SA1135::The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class ParameterSeparationAnalyzerTests
{
    [Fact]
    public async Task NoDiagnostic_MethodWithNoParameters()
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
    public async Task NoDiagnostic_MethodWithSingleShortParameter()
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
    public async Task NoDiagnostic_MethodWithLongNameButShortTotal()
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
    public async Task NoDiagnostic_MethodWithSingleLongParameterCorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithTwoParametersCorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithThreeParametersCorrectlyBroken()
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
    public async Task NoDiagnostic_ConstructorWithSingleParameter()
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
    public async Task NoDiagnostic_ConstructorWithMultipleParametersCorrectlyBroken()
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
    public async Task NoDiagnostic_AsyncMethodWithMultipleParametersCorrectlyBroken()
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
    public async Task NoDiagnostic_StaticMethodWithMultipleParametersCorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithGenericTypeParameters_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithDefaultValues_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithRefOutInParameters_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithNullableTypes_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithAttributeOnParameter_CorrectlyBroken()
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
    public async Task NoDiagnostic_VirtualMethodWithMultipleParameters_CorrectlyBroken()
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
    public async Task NoDiagnostic_OverrideMethodWithMultipleParameters_CorrectlyBroken()
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
    public async Task NoDiagnostic_AbstractMethodWithMultipleParameters_CorrectlyBroken()
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
    public async Task NoDiagnostic_LocalFunctionWithMultipleParameters_CorrectlyBroken()
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
    public async Task NoDiagnostic_DelegateWithMultipleParameters_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithLongGenericType_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithTupleParameter_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodWithArrayParameter_CorrectlyBroken()
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
    public async Task NoDiagnostic_MethodInvocation()
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
    public async Task NoDiagnostic_ConstructorInvocation()
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
    public async Task NoDiagnostic_TernaryOperator()
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
    public async Task NoDiagnostic_TernaryOperatorMultiline()
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
    public async Task NoDiagnostic_LambdaExpression()
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
    public async Task NoDiagnostic_SimpleLambda()
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
    public async Task NoDiagnostic_LinqQuery()
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
    public async Task NoDiagnostic_ChainedMethodCalls()
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
    public async Task NoDiagnostic_IndexerDeclaration()
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
    public async Task NoDiagnostic_IndexerUsage()
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
    public async Task NoDiagnostic_GeneratedCodeClass_WithMultipleParametersOnOneLine()
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
    public async Task NoDiagnostic_GeneratedCodeClass_WithMixedTypes()
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
    public async Task NoDiagnostic_GeneratedCodeClass_AsyncMethodWithMultipleParams()
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
    public async Task NoDiagnostic_AutoGeneratedHeader_WithMultipleParametersOnOneLine()
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
    public async Task NoDiagnostic_AutoGeneratedHeader_CaseInsensitive()
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
    public async Task NoDiagnostic_AutoGeneratedHeader_WithMultiLineComment()
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

    [Fact]
    public async Task NoDiagnostic_MethodWithSingleLongParameterWithDefaultValue_CorrectlyBroken()
    {
        const string code = """
                            using System.Data;
                            using System.Globalization;

                            public enum DropDownFirstItemType
                            {
                                None,
                            }

                            public class Sample
                            {
                                public static DataTable CreateKeyValueDataTableOfIntString(
                                    DropDownFirstItemType dropDownFirstItemType = DropDownFirstItemType.None)
                                {
                                    var dt = new DataTable
                                    {
                                        Locale = CultureInfo.InvariantCulture,
                                    };

                                    return dt;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MethodWithSingleParameterWithDefaultValue_OnOneLine()
    {
        const string code = """
                            using System.Data;
                            using System.Globalization;

                            public enum ItemType
                            {
                                None,
                            }

                            public static class Sample
                            {
                                public static DataTable Create(ItemType itemType = ItemType.None)
                                {
                                    var dt = new DataTable
                                    {
                                        Locale = CultureInfo.InvariantCulture,
                                    };

                                    return dt;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    [SuppressMessage("", "S2699:Add at least one assertion to this test case", Justification = "OK - RunAsync is the assertion")]
    public async Task NoDiagnostic_MethodWithTwoParametersOnOneLine_WithCustomMinParamCount()
    {
        // This method has 2 parameters on one line, but we set min_parameter_count = 3
        // so it should NOT trigger a diagnostic
        const string code = """
                            public class Sample
                            {
                                public void MyMethod(int parameter1, int parameter2)
                                {
                                }
                            }
                            """;

        var test = new CSharpAnalyzerTest<ParameterSeparationAnalyzer, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net90,
            TestCode = code,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        test.TestState.AnalyzerConfigFiles.Add((
            "/.editorconfig",
            """
            root = true

            [*]
            dotnet_diagnostic.ATC202.min_parameter_count = 3
            """));

        await test.RunAsync();
    }
}