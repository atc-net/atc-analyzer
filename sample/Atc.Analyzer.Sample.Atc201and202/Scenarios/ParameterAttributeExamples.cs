namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class ParameterAttributeExamples
{
    // Valid: Parameters with attributes
    public void MethodWithAttributedParams(
        [NotNull] string x,
        int y)
    {
        Console.WriteLine($"Attributed: {x}, {y}");
    }

    // Valid: Multiple attributes on parameters
    public void MethodWithMultipleAttributes(
        [NotNull] [DisallowNull] string param1,
        int param2)
    {
        Console.WriteLine($"Multiple attributes: {param1}, {param2}");
    }
}