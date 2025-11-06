namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

[GeneratedCode("MyGen", "1.0.1")]
internal class GeneratedCodeAttributeMultipleParameterExamples
{
    public void MethodWithTwoParameters(int parameter1, string parameter2)
    {
        Console.WriteLine($"Two params: {parameter1}, {parameter2}");
    }

    public void MethodWithThreeParameters(int parameter1, string parameter2, bool parameter3)
    {
        Console.WriteLine($"Three params: {parameter1}, {parameter2}, {parameter3}");
    }

    public void MethodWithMixedTypes(int number, string text, DateTime date, bool flag)
    {
        Console.WriteLine($"Mixed types: {number}, {text}, {date}, {flag}");
    }

    public static void StaticMethodWithMultipleParams(string name, int age)
    {
        Console.WriteLine($"Static: {name}, {age}");
    }

    public async Task AsyncMethodWithMultipleParams(int id, string name)
    {
        await Task.CompletedTask;
        Console.WriteLine($"Async: {id}, {name}");
    }
}