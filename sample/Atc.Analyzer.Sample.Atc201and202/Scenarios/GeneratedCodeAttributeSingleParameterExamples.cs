namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

[GeneratedCode("MyGen", "1.0.1")]
internal class GeneratedCodeAttributeSingleParameterExamples
{
    public void ShortMethod(
        int parameter1)
    {
        Console.WriteLine($"Single parameter: {parameter1}");
    }

    public void MethodWithSingleParameter(
        string parameter1)
    {
        Console.WriteLine($"Parameter: {parameter1}");
    }

    public void MethodWithRefParameter(ref int parameter1)
    {
        parameter1++;
    }

    public void MethodWithNullableParameter(
        string? parameter1)
    {
        Console.WriteLine($"Nullable: {parameter1 ?? "null"}");
    }
}