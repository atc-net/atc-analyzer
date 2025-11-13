namespace Atc.Analyzer.Sample.Atc203;

#pragma warning disable CA1303

/// <summary>
/// This sample demonstrates VALID scenarios for ATC203 analyzer.
/// ATC203 (MethodChainSeparationAnalyzer): Method chains with 2 or more calls should be placed on separate lines.
/// - Method chains with 2+ calls: Each method call must be on a separate line for better readability
/// - Applies to: all method invocations including LINQ, async/await, StringBuilder, etc.
/// - Does NOT apply to: single method calls, property access after method call (e.g., str.Trim().Length)
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("ATC203: Method Chain Separation Analyzer - Valid Scenarios Demo");

        // Demonstrate various valid scenarios
        var demo = new ValidMethodChainExamples();
        demo.DemonstrateAllValidScenarios();
    }
}