namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class NotCoveredExamples
{
    private readonly int[,] matrix = new int[2, 2];

    public void DemonstrateExclusions()
    {
        // Valid: Method invocations can be on one line
        var result1 = SomeMethod("a", "b", "c");
        Console.WriteLine($"Method invocation result: {result1}");

        // Valid: Object creation can be on one line
        var obj = new ConstructorExamples("John", 30);
        obj.DisplayInfo();

        // Valid: Lambda expressions can be on one line
        Func<int, int, int> lambda = (x, y) => x + y;
        var sum = lambda(5, 10);
        Console.WriteLine($"Lambda sum: {sum}");

        // Valid: LINQ queries
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
        Console.WriteLine($"Even numbers: {string.Join(", ", evenNumbers)}");

        // Valid: Ternary operators
        var message = evenNumbers.Count > 0 ? "Has even numbers" : "No even numbers";
        Console.WriteLine(message);

        // Valid: Indexer usage
        var value = this[0, 1];
        Console.WriteLine($"Indexer value: {value}");

        // Valid: Chained method calls
        var text = "test string"
            .Replace("test", "demo", StringComparison.Ordinal)
            .Replace("string", "text", StringComparison.Ordinal);
        Console.WriteLine($"Chained result: {text}");
    }

    // Helper method for demonstration
    private static string SomeMethod(
        string a,
        string b,
        string c)
    {
        return $"{a}-{b}-{c}";
    }

    // Valid: Indexer declaration with multiple parameters
    public int this[int row, int column]
    {
        get => matrix[row, column];
        set => matrix[row, column] = value;
    }
}