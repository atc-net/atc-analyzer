namespace Atc.Analyzer.Sample.Atc220;

// No regular usings here - they're in GlobalUsings.cs
#pragma warning disable CA1303
#pragma warning disable CA1822

/// <summary>
/// Examples that do NOT trigger ATC220.
/// These scenarios are considered valid.
/// </summary>
internal sealed class ValidExamples
{
    public void DemonstrateValidScenarios()
    {
        Console.WriteLine("=== Valid Scenarios (No ATC220 diagnostic) ===");
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

        // Note: Using aliases (e.g., using Constants = SomeNamespace;) are allowed
        Console.WriteLine("3. Note: Using aliases are allowed locally");
    }
}