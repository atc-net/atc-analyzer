namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class MultipleParameterExamples
{
    // Valid: Two parameters on separate lines
    public void MethodWithTwoParameters(
        int parameter1,
        string parameter2)
    {
        Console.WriteLine($"Two params: {parameter1}, {parameter2}");
    }

    // Valid: Three parameters on separate lines
    public void MethodWithThreeParameters(
        int parameter1,
        string parameter2,
        bool parameter3)
    {
        Console.WriteLine($"Three params: {parameter1}, {parameter2}, {parameter3}");
    }

    // Valid: Multiple parameters with different types
    public void MethodWithMixedTypes(
        int number,
        string text,
        DateTime date,
        bool flag)
    {
        Console.WriteLine($"Mixed types: {number}, {text}, {date}, {flag}");
    }

    // Valid: Static method with multiple parameters
    public static void StaticMethodWithMultipleParams(
        string name,
        int age)
    {
        Console.WriteLine($"Static: {name}, {age}");
    }

    // Valid: Async method with multiple parameters
    public async Task AsyncMethodWithMultipleParams(
        int id,
        string name)
    {
        await Task.CompletedTask;
        Console.WriteLine($"Async: {id}, {name}");
    }
}