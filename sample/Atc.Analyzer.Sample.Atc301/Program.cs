namespace Atc.Analyzer.Sample.Atc301;

#pragma warning disable CA1303

/// <summary>
/// This sample demonstrates VALID scenarios for ATC301 analyzer.
/// ATC301 (GeneratedRegexCompiledFlagAnalyzer): Remove redundant RegexOptions.Compiled flag from [GeneratedRegex] attribute.
/// - The [GeneratedRegex] attribute generates compiled regex code at build time using source generators
/// - Adding RegexOptions.Compiled to the options parameter is redundant and has no effect
/// - This sample shows correct usage WITHOUT the Compiled flag
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("ATC301: GeneratedRegex Compiled Flag Analyzer - Valid Scenarios Demo");
        Console.WriteLine();

        // Demonstrate various valid scenarios
        ValidExamples.DemonstrateAllValidScenarios();
        Console.WriteLine();

        // Note: Invalid examples (with RegexOptions.Compiled) are commented out
        // because they would trigger ATC301 warnings
        Console.WriteLine("Invalid examples are commented out in Scenarios/InvalidExamples.cs");
    }
}