namespace Atc.Analyzer.Sample.Atc230;

#pragma warning disable CA1303

/// <summary>
/// This sample demonstrates ATC230 analyzer.
/// ATC230 (BlankLineBetweenCodeBlocksAnalyzer): Require exactly one blank line between consecutive code blocks.
/// - Consecutive block statements (if, for, foreach, while, do, switch, try, using, lock) should be separated by exactly one blank line
/// - Missing blank lines will be flagged
/// - Excessive blank lines (2+) will also be flagged
/// - Logically connected blocks (if-else, try-catch-finally) should NOT have blank lines
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("ATC230: Blank Line Between Code Blocks Analyzer Demo");

        // Demonstrate valid scenarios
        var validDemo = new ValidExamples();
        validDemo.DemonstrateAllValidScenarios();

        // Demonstrate invalid scenarios (these will trigger ATC230 warnings)
        var invalidDemo = new InvalidExamples();
        invalidDemo.DemonstrateInvalidScenarios();
    }
}