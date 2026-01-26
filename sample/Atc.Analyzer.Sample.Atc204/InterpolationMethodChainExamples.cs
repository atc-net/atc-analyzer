namespace Atc.Analyzer.Sample.Atc204;

/// <summary>
/// Examples that should trigger ATC204 - Chained method calls in interpolated strings.
/// </summary>
public static class InterpolationMethodChainExamples
{
    // Should trigger ATC204: Two chained method calls in interpolation
    public static void TwoChainedMethodCalls()
    {
        const string name = "John Doe";
        var message = $"Hello {name.Trim().ToUpper()}!";
        Console.WriteLine(message);
    }

    // Should trigger ATC204: Three chained method calls in interpolation
    public static void ThreeChainedMethodCalls()
    {
        const string input = "  HELLO WORLD  ";
        var result = $"Processed: {input.Trim().ToLower().Replace(" ", "-")}";
        Console.WriteLine(result);
    }

    // Should trigger ATC204: Chained calls with LINQ
    public static void LinqChainedCalls()
    {
        var items = new List<string> { "apple", "banana", "cherry" };
        var output = $"First item: {items.First().ToUpper()}";
        Console.WriteLine(output);
    }

    // Should trigger ATC204: Multiple interpolations with chains
    public static void MultipleInterpolationsWithChains()
    {
        const string firstName = "john";
        const string lastName = "doe";
        var fullName = $"{firstName.Trim().ToUpper()} {lastName.Trim().ToUpper()}";
        Console.WriteLine(fullName);
    }

    // Should trigger ATC204: Verbatim interpolated string
    public static void VerbatimInterpolatedString()
    {
        const string path = @"C:\Users\Test";
        var message = $@"Path is: {path.Trim().ToLower()}";
        Console.WriteLine(message);
    }
}