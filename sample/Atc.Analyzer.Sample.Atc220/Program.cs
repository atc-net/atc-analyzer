namespace Atc.Analyzer.Sample.Atc220;

#pragma warning disable CA1303

/// <summary>
/// This sample demonstrates scenarios for ATC220 analyzer.
/// ATC220 (GlobalUsingsAllAnalyzer): All using directives should be moved to GlobalUsings.cs.
/// - ALL using directives should be defined globally in GlobalUsings.cs
/// - Applies to: regular using directives for all namespaces
/// - Does NOT apply to: static usings, using aliases, or namespaces already in GlobalUsings.cs
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("ATC220: Use Global Usings (All) Analyzer - Demo");
        Console.WriteLine("This sample demonstrates valid and invalid scenarios.");
        Console.WriteLine();

        var validExample = new ValidExamples();
        validExample.DemonstrateValidScenarios();

        var invalidExample = new InvalidExamples();
        invalidExample.DemonstrateInvalidScenarios();
    }
}