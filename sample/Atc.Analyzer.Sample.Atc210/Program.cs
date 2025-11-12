namespace Atc.Analyzer.Sample.Atc210;

#pragma warning disable CA1303

/// <summary>
/// This sample demonstrates VALID scenarios for ATC210 analyzer.
/// ATC210 (ExpressionBodyAnalyzer): Use expression body syntax when appropriate.
/// - Methods and properties with simple return statements should use expression body syntax (=>) for conciseness
/// - Expression bodies longer than 80 characters should place the arrow on a new line
/// - Multi-line expressions (like ternary) should have the arrow on a new line
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("ATC210: Expression Body Analyzer - Valid Scenarios Demo");

        // Demonstrate various valid scenarios
        var demo = new ValidExpressionBodyExamples();
        demo.DemonstrateAllValidScenarios();
    }
}