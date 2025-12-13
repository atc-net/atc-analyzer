namespace Atc.Analyzer.Sample.Atc230;

#pragma warning disable CA1031, CA1303, CA1822, S1186, S2325

/// <summary>
/// Demonstrates valid formatting for ATC230.
/// All examples show correct blank line usage between code blocks.
/// </summary>
internal sealed class ValidExamples
{
    public void DemonstrateAllValidScenarios()
    {
        Console.WriteLine("\n=== Valid Blank Line Formatting Examples ===\n");

        // Example 1: Two if statements with exactly one blank line
        Console.WriteLine("1. Two if statements with one blank line - VALID:");
        ExampleTwoIfStatementsWithBlankLine();

        // Example 2: For and while with one blank line
        Console.WriteLine("\n2. For and while with one blank line - VALID:");
        ExampleForAndWhileWithBlankLine();

        // Example 3: if-else chain (no blank line needed)
        Console.WriteLine("\n3. If-else chain (no blank line) - VALID:");
        ExampleIfElseChain(5);

        // Example 4: try-catch-finally (no blank line needed)
        Console.WriteLine("\n4. Try-catch-finally (no blank line) - VALID:");
        ExampleTryCatchFinally();

        // Example 5: Block with comment between
        Console.WriteLine("\n5. Block with comment between - VALID:");
        ExampleWithComment();

        Console.WriteLine("\n=== All examples demonstrate VALID formatting ===");
    }

    // Valid: Two if statements with exactly one blank line between them
    private static void ExampleTwoIfStatementsWithBlankLine()
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

    // Valid: For and while with exactly one blank line between them
    private static void ExampleForAndWhileWithBlankLine()
    {
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"   Iteration {i}");
        }

        while (false)
        {
            // This won't execute
        }
    }

    // Valid: if-else chain has NO blank line between if and else (logically connected)
    private static void ExampleIfElseChain(int value)
    {
        if (value > 0)
        {
            Console.WriteLine("   Positive");
        }
        else if (value < 0)
        {
            Console.WriteLine("   Negative");
        }
        else
        {
            Console.WriteLine("   Zero");
        }
    }

    // Valid: try-catch-finally has NO blank line between blocks (logically connected)
    private static void ExampleTryCatchFinally()
    {
        try
        {
            Console.WriteLine("   In try block");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   Caught: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("   In finally block");
        }
    }

    // Valid: Comment between blocks with one blank line before the comment
    private static void ExampleWithComment()
    {
        if (true)
        {
            Console.WriteLine("   First block");
        }

        // This comment is allowed, with one blank line before it
        if (true)
        {
            Console.WriteLine("   Second block");
        }
    }
}