namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class NullableParameterExamples
{
    // Valid: Multiple nullable parameters
    public void MethodWithNullableParams(
        int? x,
        string? y,
        DateTime? z)
    {
        Console.WriteLine($"Nullables: {x}, {y}, {z}");
    }

    // Valid: Mix of nullable and non-nullable
    public void MethodWithMixedNullability(
        string nonNullable,
        string? nullable,
        int count)
    {
        Console.WriteLine($"Mixed nullability: {nonNullable}, {nullable}, {count}");
    }
}