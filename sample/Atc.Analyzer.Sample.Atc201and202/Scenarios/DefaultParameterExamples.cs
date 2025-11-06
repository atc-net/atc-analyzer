namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class DefaultParameterExamples
{
    // Valid: Multiple parameters with default values
    public void MethodWithDefaultValues(
        int x = 0,
        string y = "default",
        bool z = false)
    {
        Console.WriteLine($"Defaults: {x}, {y}, {z}");
    }

    // Valid: Mix of required and optional parameters
    public void MethodWithMixedDefaults(
        string required,
        int optional = 10,
        bool flag = true)
    {
        Console.WriteLine($"Mixed: {required}, {optional}, {flag}");
    }
}