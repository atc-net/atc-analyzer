namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class ComplexParameterTypeExamples
{
    // Valid: Dictionary parameter with multiple params
    public void MethodWithDictionary(
        Dictionary<string, List<int>> param1,
        int count)
    {
        Console.WriteLine($"Dictionary param with {count} items");
    }

    // Valid: Tuple parameters
    public void MethodWithTuple(
        (int x, string y) tuple,
        int count)
    {
        Console.WriteLine($"Tuple: ({tuple.x}, {tuple.y}), count: {count}");
    }

    // Valid: Array parameters
    public void MethodWithArrays(
        string[] items,
        int[] numbers)
    {
        Console.WriteLine($"Arrays: {items.Length} items, {numbers.Length} numbers");
    }

    // Valid: Action/Func parameters
    public void MethodWithDelegates(
        Action<string> callback,
        Func<int, bool> predicate)
    {
        callback("test");
        Console.WriteLine($"Predicate result: {predicate(5)}");
    }
}