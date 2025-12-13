namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<BlankLineBetweenCodeBlocksAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class BlankLineBetweenCodeBlocksAnalyzerTests
{
    [Fact]
    public async Task NoDiagnostic_SingleBlockStatement()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_TwoIfStatementsWithOneBlankLine()
    {
        const string code = """
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

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_ForAndWhileWithOneBlankLine()
    {
        const string code = """
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

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_IfElseChain()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method(int x)
                                {
                                    if (x > 0)
                                    {
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_IfElseIfElseChain()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method(int x)
                                {
                                    if (x > 0)
                                    {
                                    }
                                    else if (x < 0)
                                    {
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_TryCatchFinally()
    {
        const string code = """
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
                                    finally
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_NonBlockStatements()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    var x = 1;
                                    var y = 2;
                                    Console.WriteLine(x + y);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_BlockStatementFollowedByNonBlockStatement()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }
                                    var x = 1;
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_NonBlockStatementFollowedByBlockStatement()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    var x = 1;
                                    if (true)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_ForeachAndSwitchWithOneBlankLine()
    {
        const string code = """
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

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_UsingStatementWithOneBlankLine()
    {
        const string code = """
                            using System.IO;

                            public class Sample
                            {
                                public void Method()
                                {
                                    using (var stream = new MemoryStream())
                                    {
                                    }

                                    using (var stream2 = new MemoryStream())
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_LockStatementWithOneBlankLine()
    {
        const string code = """
                            public class Sample
                            {
                                private readonly object lockObj = new object();

                                public void Method()
                                {
                                    lock (lockObj)
                                    {
                                    }

                                    lock (lockObj)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_DoWhileWithOneBlankLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    do
                                    {
                                    }
                                    while (false);

                                    do
                                    {
                                    }
                                    while (false);
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_MultipleBlockStatementsWithOneBlankLineBetweenEach()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }

                                    for (int i = 0; i < 10; i++)
                                    {
                                    }

                                    while (false)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_NestedBlocksWithProperSpacing()
    {
        const string code = """
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

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_GeneratedCodeFile()
    {
        const string code = """
                            // This code was auto-generated
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

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_BlockStatementWithCommentBetween()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }

                                    // This is a comment
                                    if (true)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_CheckedStatementWithOneBlankLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    checked
                                    {
                                    }

                                    unchecked
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task NoDiagnostic_FixedStatementWithOneBlankLine()
    {
        const string code = """
                            public unsafe class Sample
                            {
                                public void Method()
                                {
                                    int[] arr = new int[10];
                                    fixed (int* p = arr)
                                    {
                                    }

                                    fixed (int* q = arr)
                                    {
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}