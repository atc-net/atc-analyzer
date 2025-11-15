namespace Atc.Analyzer.Sample.Atc221;

#pragma warning disable ATC220

// No usings from System/Microsoft/Atc here - they're in GlobalUsings.cs
using Xunit; // Third-party namespace - this is OK

#pragma warning disable CA1303
#pragma warning disable CA1822

/// <summary>
/// Examples that do NOT trigger ATC221.
/// These scenarios are considered valid.
/// </summary>
internal sealed class ValidExamples
{
    public void DemonstrateValidScenarios()
    {
        Console.WriteLine("=== Valid Scenarios (No ATC221 diagnostic) ===");
        Console.WriteLine();

        // Scenario 1: Using namespaces from GlobalUsings.cs
        Console.WriteLine("1. Using System namespace (defined in GlobalUsings.cs)");
        var now = DateTime.Now;
        Console.WriteLine($"   Current time: {now}");
        Console.WriteLine();

        // Scenario 2: Using System.Collections.Generic (from GlobalUsings.cs)
        Console.WriteLine("2. Using System.Collections.Generic (from GlobalUsings.cs)");
        var list = new List<string> { "Item1", "Item2", "Item3" };
        Console.WriteLine($"   List count: {list.Count}");
        Console.WriteLine();

        // Scenario 3: Using third-party namespace (Xunit) - this is OK to have locally
        Console.WriteLine("3. Using third-party namespace (Xunit) - local using is OK");
        var fact = new FactAttribute();
        Console.WriteLine($"   Fact attribute: {fact.GetType().Name}");
        Console.WriteLine();
    }
}