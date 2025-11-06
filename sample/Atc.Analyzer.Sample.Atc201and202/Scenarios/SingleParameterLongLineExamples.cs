namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class SingleParameterLongLineExamples
{
    // Valid: Long method name with parameter on new line (exceeds 80 chars)
    public void MyVeryLongMethodNameThatExceedsEightyCharactersWhenCombinedWithParameter(
        int parameter1)
    {
        Console.WriteLine($"Long line parameter: {parameter1}");
    }

    // Valid: Single parameter broken correctly for readability
    public void AnotherVeryLongMethodNameThatRequiresParameterOnSeparateLine(
        string parameter1)
    {
        Console.WriteLine($"Parameter: {parameter1}");
    }
}