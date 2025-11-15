namespace Atc.Analyzer.Sample.Atc220;

// These usings SHOULD be moved to GlobalUsings.cs and will trigger ATC220
#pragma warning disable ATC220
#pragma warning disable ATC221

using System.Linq;           // ATC220: Should be in GlobalUsings.cs
using System.Text;           // ATC220: Should be in GlobalUsings.cs
using Xunit;                 // ATC220: Should be in GlobalUsings.cs (ALL namespaces)

#pragma warning disable CA1303
#pragma warning disable CA1822

/// <summary>
/// Examples that WILL trigger ATC220.
/// These using directives should be moved to GlobalUsings.cs.
/// </summary>
internal sealed class InvalidExamples
{
    public void DemonstrateInvalidScenarios()
    {
        Console.WriteLine("=== Invalid Scenarios (Will trigger ATC220) ===");
        Console.WriteLine();

        // Scenario 1: Using System.Linq locally instead of globally
        Console.WriteLine("1. Using System.Linq (should be in GlobalUsings.cs)");
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var sum = numbers.Sum();
        Console.WriteLine($"   Sum: {sum}");
        Console.WriteLine("   -> FIX: Move 'using System.Linq;' to GlobalUsings.cs");
        Console.WriteLine();

        // Scenario 2: Using System.Text locally instead of globally
        Console.WriteLine("2. Using System.Text (should be in GlobalUsings.cs)");
        var builder = new StringBuilder();
        builder.Append("Hello");
        builder.Append(' ');
        builder.Append("World");
        Console.WriteLine($"   Result: {builder}");
        Console.WriteLine("   -> FIX: Move 'using System.Text;' to GlobalUsings.cs");
        Console.WriteLine();

        // Scenario 3: Using third-party namespace (Xunit) - ATC220 flags ALL namespaces
        Console.WriteLine("3. Using Xunit (should be in GlobalUsings.cs)");
        var fact = new FactAttribute();
        Console.WriteLine($"   Fact attribute: {fact.GetType().Name}");
        Console.WriteLine("   -> FIX: Move 'using Xunit;' to GlobalUsings.cs");
        Console.WriteLine();
    }
}