namespace Atc.Analyzer.Sample.Atc221;

#pragma warning disable CA1303

/// <summary>
/// This sample demonstrates scenarios for ATC221 analyzer.
/// ATC221 (UseGlobalUsingsAnalyzer): Using directives for System, Microsoft, and Atc namespaces should be moved to GlobalUsings.cs.
/// - Using directives from System, Microsoft, and Atc namespaces should be defined globally in GlobalUsings.cs
/// - Applies to: regular using directives only
/// - Does NOT apply to: static usings, using aliases, or namespaces already in GlobalUsings.cs
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("ATC221: Use Global Usings Analyzer - Demo");
        Console.WriteLine("This sample demonstrates valid and invalid scenarios.");
        Console.WriteLine();

        var validExample = new ValidExamples();
        validExample.DemonstrateValidScenarios();

        var invalidExample = new InvalidExamples();
        invalidExample.DemonstrateInvalidScenarios();
    }
}