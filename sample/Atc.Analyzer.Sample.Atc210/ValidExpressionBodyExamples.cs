namespace Atc.Analyzer.Sample.Atc210;

#pragma warning disable CA1822, CA1859, CA1303, CA1304, CA1311, IDE0060, MA0051, S1172, S2292, S2325, S3400, SA1407, AsyncFixer01

/// <summary>
/// Demonstrates valid expression body formatting for ATC210.
/// </summary>
internal sealed class ValidExpressionBodyExamples
{
    private readonly string name = "Sample";

    public void DemonstrateAllValidScenarios()
    {
        Console.WriteLine("\n=== Valid Expression Body Formatting Examples ===\n");

        // 1. Short expression body on single line (less than 80 chars)
        Console.WriteLine("1. Short expression body - single line:");
        var result1 = GetSimpleData();
        Console.WriteLine($"   Result: '{result1}'");

        // 2. Expression body with arrow on new line
        Console.WriteLine("\n2. Expression body - arrow on new line:");
        var result2 = GetDataWithMultiLineFormat();
        Console.WriteLine($"   Result: '{result2}'");

        // 3. Ternary expression with proper formatting
        Console.WriteLine("\n3. Ternary expression - properly formatted:");
        var result3 = GetConditionalValue(true);
        Console.WriteLine($"   Result: {result3}");

        // 4. Property with expression body
        Console.WriteLine("\n4. Property with expression body:");
        Console.WriteLine($"   Name property: {Name}");

        // 5. Property with multi-line format
        Console.WriteLine("\n5. Property with multi-line format:");
        Console.WriteLine($"   Description property: {Description}");

        // 6. Methods that should NOT use expression body (multiple statements)
        Console.WriteLine("\n6. Method with multiple statements (block body is correct):");
        var result4 = GetProcessedData();
        Console.WriteLine($"   Result: '{result4}'");

        // 7. Method with complex logic (block body is correct)
        Console.WriteLine("\n7. Method with complex logic (block body is correct):");
        var result5 = Calculate(10);
        Console.WriteLine($"   Result: {result5}");

        // 8. Async method with expression body
        Console.WriteLine("\n8. Async method with expression body:");
        var result6 = GetDataAsync()
            .GetAwaiter()
            .GetResult();
        Console.WriteLine($"   Result: '{result6}'");

        // 9. Generic method with expression body
        Console.WriteLine("\n9. Generic method with expression body:");
        var result7 = GetDefault<int>();
        Console.WriteLine($"   Result: {result7}");

        // 10. Method returning lambda
        Console.WriteLine("\n10. Method returning lambda:");
        var multiplier = GetMultiplier(5);
        Console.WriteLine($"   5 * 3 = {multiplier(3)}");

        Console.WriteLine("\n=== All examples demonstrate VALID formatting ===");
    }

    // Example 1: Short expression body on single line
    public static string GetSimpleData() => "Sample Data";

    // Example 2: Expression body with arrow on new line (for better readability)
    public static string GetDataWithMultiLineFormat()
        => "Sample Data";

    // Example 3: Ternary expression with proper formatting (arrow on new line)
    public static int GetConditionalValue(bool condition)
        => condition
            ? 1
            : 0;

    // Example 4: Property with expression body
    public string Name => name;

    // Example 5: Property with multi-line format
    public string Description
        => "Sample description";

    // Example 6: Method with multiple statements (block body is correct)
    public static string GetProcessedData()
    {
        var data = "Sample";
        return data.ToUpper();
    }

    // Example 7: Method with complex logic (block body is correct)
    public static int Calculate(int value)
    {
        if (value > 0)
        {
            return value * 2;
        }

        return 0;
    }

    // Example 8: Async method with expression body
    public static async Task<string> GetDataAsync()
        => await Task.FromResult("data");

    // Example 9: Generic method with expression body
    public static T GetDefault<T>()
        => default(T)!;

    // Example 10: Method returning lambda
    public static Func<int, int> GetMultiplier(int factor) => x => x * factor;

    // Example 11: Property accessor with expression body
    private string backingField = string.Empty;

    public string PropertyWithAccessors
    {
        get => backingField;
        set => backingField = value;
    }

    // Example 12: Method with method call
    public static string GetUpperCase(string input) => input.ToUpper();

    // Example 13: Method with complex expression (fits on 80 chars)
    public static int CalculateComplex(
        int x,
        int y)
        => x * y + (x - y) * 2;

    // Example 14: Method returning new object
    public static object CreateObject() => new object();

    // Example 15: Method returning array
    public static int[] GetArray() => [1, 2, 3];
}