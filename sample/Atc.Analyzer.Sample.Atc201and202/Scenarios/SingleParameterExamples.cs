namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class SingleParameterExamples
{
    // Valid: Single parameter on same line (total line length < 80 chars)
    public void ShortMethod(int parameter1)
    {
        Console.WriteLine($"Single parameter: {parameter1}");
    }

    // Valid: Single parameter with longer method name but still under 80 chars
    public void MethodWithSingleParameter(string parameter1)
    {
        Console.WriteLine($"Parameter: {parameter1}");
    }

    // Valid: Single parameter with modifier
    public void MethodWithRefParameter(ref int parameter1)
    {
        parameter1++;
    }

    // Valid: Single parameter with nullable type
    public void MethodWithNullableParameter(string? parameter1)
    {
        Console.WriteLine($"Nullable: {parameter1 ?? "null"}");
    }
}