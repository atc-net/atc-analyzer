namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class NoParameterExamples
{
    // Valid: No parameters
    public void MethodWithNoParameters()
    {
        Console.WriteLine("No parameters - always valid");
    }

    // Valid: Static method with no parameters
    public static void StaticMethodWithNoParameters()
    {
        Console.WriteLine("Static method with no parameters");
    }

    // Valid: Async method with no parameters
    public async Task AsyncMethodWithNoParameters()
    {
        await Task.CompletedTask;
    }
}