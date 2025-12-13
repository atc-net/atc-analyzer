namespace Atc.Analyzer.Tests.Rules.Style;

#pragma warning disable SA1135 // Using directives must be qualified
using AnalyzerVerifier = CSharpAnalyzerVerifier<BlankLineBetweenCodeBlocksAnalyzer>;
#pragma warning restore SA1135 // Using directives must be qualified

[SuppressMessage("", "AsyncFixer01:The method does not need to use async/await", Justification = "OK - Test code")]
[SuppressMessage("Naming", "MA0048:File name must match type name", Justification = "OK - Partial class")]
public sealed partial class BlankLineBetweenCodeBlocksAnalyzerTests
{
    [Fact]
    public async Task ReportsDiagnostic_TwoIfStatementsNoBlankLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }
                                    [|if (true)
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_TwoIfStatementsTwoBlankLines()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }


                                    [|if (true)
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_TwoIfStatementsThreeBlankLines()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }



                                    [|if (true)
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ForAndWhileNoBlankLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                    }
                                    [|while (true)
                                    {
                                        break;
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_ForeachAndIfNoBlankLine()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method(int[] items)
                                {
                                    foreach (var item in items)
                                    {
                                    }
                                    [|if (true)
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_SwitchAndUsingNoBlankLine()
    {
        const string code = """
                            using System.IO;

                            public class Sample
                            {
                                public void Method(int x)
                                {
                                    switch (x)
                                    {
                                        default:
                                            break;
                                    }
                                    [|using (var stream = new MemoryStream())
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_NestedBlocksNoBlankLine()
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
                                        [|if (true)
                                        {
                                        }|]
                                    }
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_MultipleViolationsInSameMethod()
    {
        const string code = """
                            public class Sample
                            {
                                public void Method()
                                {
                                    if (true)
                                    {
                                    }
                                    [|if (true)
                                    {
                                    }|]
                                    [|for (int i = 0; i < 10; i++)
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_TryStatementsNoBlankLine()
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
                                    [|try
                                    {
                                    }
                                    catch
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_LockStatementsNoBlankLine()
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
                                    [|lock (lockObj)
                                    {
                                    }|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }

    [Fact]
    public async Task ReportsDiagnostic_DoWhileStatementsNoBlankLine()
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
                                    [|do
                                    {
                                    }
                                    while (false);|]
                                }
                            }
                            """;

        await AnalyzerVerifier.VerifyAnalyzerAsync(code);
    }
}