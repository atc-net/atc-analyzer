namespace Atc.Analyzer.Sample.Atc203;

#pragma warning disable CA1303, CA1304, CA1311, CA1834, MA0051, SA1402, S2325

internal sealed class ValidMethodChainExamples
{
    public void DemonstrateAllValidScenarios()
    {
        Console.WriteLine("\n=== Valid Method Chain Formatting Examples ===\n");

        // Single method call - always OK on one line
        Console.WriteLine("1. Single method calls:");
        const string text = "  Hello World  ";
        var trimmed = text.Trim();
        Console.WriteLine($"   Result: '{trimmed}'");

        // Two method calls - properly placed on separate lines
        Console.WriteLine("\n2. Two method chains - properly formatted:");
        var result1 = text
            .Trim()
            .ToLower();
        Console.WriteLine($"   Result: '{result1}'");

        // Multiple method chains - properly placed on separate lines
        Console.WriteLine("\n3. Multiple method chains - properly formatted:");
        var result2 = text
            .Trim()
            .Replace("World", "Universe", StringComparison.OrdinalIgnoreCase)
            .ToUpper();
        Console.WriteLine($"   Result: '{result2}'");

        // LINQ method chains - properly formatted
        Console.WriteLine("\n4. LINQ method chains - properly formatted:");
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var evenSquares = numbers
            .Where(x => x % 2 == 0)
            .Select(x => x * x)
            .ToList();
        Console.WriteLine($"   Even squares: {string.Join(", ", evenSquares)}");

        // StringBuilder chains - properly formatted
        Console.WriteLine("\n5. StringBuilder chains - properly formatted:");
        var sb = new StringBuilder()
            .Append("Hello")
            .Append(" ")
            .Append("World")
            .ToString();
        Console.WriteLine($"   Result: '{sb}'");

        // Property access after method call - single line is OK
        Console.WriteLine("\n6. Property access after method call:");
        var length = text.Trim().Length;
        Console.WriteLine($"   Length: {length}");

        // Starting method with one chained method - single line is OK
        Console.WriteLine("\n7. Async method chains - properly formatted:");
        _ = GetDataAsync().ConfigureAwait(false);
        Console.WriteLine("   Async call configured");

        // FluentAssertions pattern - .Should().Be() allowed on one line
        Console.WriteLine("\n8. FluentAssertions pattern - allowed on one line:");
        var testValue = "expected";
        testValue.Should().Be("expected");
        Console.WriteLine("   Assertion passed");

        // FluentAssertions with And - properly formatted on multiple lines
        Console.WriteLine("\n9. FluentAssertions with .And - properly formatted:");
        var trimmed2 = "  Hello World  ".Trim();
        trimmed2
            .Should().Be("Hello World")
            .And.NotBeNullOrWhiteSpace();
        Console.WriteLine("   Multiple assertions passed");

        // FluentAssertions with multiple Ands - properly formatted
        Console.WriteLine("\n10. FluentAssertions with multiple .And - properly formatted:");
        var upper2 = "test".ToUpperInvariant();
        upper2
            .Should().Be("TEST")
            .And.StartWith("TE")
            .And.EndWith("ST");
        Console.WriteLine("   Chained assertions passed");

        Console.WriteLine("\n=== All examples demonstrate VALID formatting ===");
    }

    private static async Task<string> GetDataAsync()
    {
        await Task.Delay(1);
        return "data";
    }
}