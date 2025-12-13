namespace Atc.Analyzer.Sample.Atc230;

#pragma warning disable CA1303, CA1822, S1186, S4144, SA1507, SA1513

// These usings SHOULD be moved to GlobalUsings.cs and will trigger ATC230
#pragma warning disable ATC230

/// <summary>
/// Demonstrates INVALID formatting for ATC230.
/// These examples will trigger ATC230 warnings.
/// Run with: dotnet build sample/Atc.Analyzer.Sample.Atc230
/// You should see ATC230 warnings for each violation.
/// </summary>
internal sealed class InvalidExamples
{
    public void DemonstrateInvalidScenarios()
    {
        Console.WriteLine("\n=== INVALID Blank Line Formatting Examples ===");
        Console.WriteLine("(These should trigger ATC230 warnings when building)\n");

        // Example 1: Two if statements with NO blank line
        Console.WriteLine("1. Two if statements with NO blank line - INVALID");
        ExampleNoBlankLine();

        // Example 2: Two if statements with TOO MANY blank lines
        Console.WriteLine("\n2. Two if statements with 2+ blank lines - INVALID");
        ExampleTooManyBlankLines();

        Console.WriteLine("\n=== Check build output for ATC230 warnings ===");
    }

    // INVALID: Two if statements with no blank line between them
    private static void ExampleNoBlankLine()
    {
        if (DateTime.Now.Hour > 12)
        {
            Console.WriteLine("   Afternoon");
        }
        if (DateTime.Now.Hour < 6)
        {
            Console.WriteLine("   Early morning");
        }
    }

    // INVALID: Two if statements with too many blank lines (2+)
    private static void ExampleTooManyBlankLines()
    {
        if (DateTime.Now.Hour > 12)
        {
            Console.WriteLine("   Afternoon");
        }


        if (DateTime.Now.Hour < 6)
        {
            Console.WriteLine("   Early morning");
        }
    }
}