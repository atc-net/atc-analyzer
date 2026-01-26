namespace Atc.Analyzer.Sample.Atc204;

/// <summary>
/// Examples that should NOT trigger ATC204 - Valid interpolation patterns.
/// </summary>
public static class ValidInterpolationExamples
{
    // Valid: Single method call - no chain
    public static void SingleMethodCall()
    {
        const string name = "John Doe";
        var message = $"Hello {name.ToUpper()}!";
        Console.WriteLine(message);
    }

    // Valid: Simple variable in interpolation
    public static void SimpleVariable()
    {
        const string name = "John";
        var message = $"Hello {name}!";
        Console.WriteLine(message);
    }

    // Valid: Property access without method calls
    public static void PropertyAccess()
    {
        const string text = "Hello";
        var message = $"Length is: {text.Length}";
        Console.WriteLine(message);
    }

    // Valid: Already extracted to variable
    public static void ExtractedToVariable()
    {
        const string name = "  john doe  ";
        var formattedName = name.Trim().ToUpper();
        var message = $"Hello {formattedName}!";
        Console.WriteLine(message);
    }

    // Valid: Single LINQ call
    public static void SingleLinqCall()
    {
        var items = new List<string> { "apple", "banana" };
        var message = $"First: {items.First()}";
        Console.WriteLine(message);
    }
}