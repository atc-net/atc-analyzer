namespace Atc.Analyzer.Sample.Atc221;

// These usings SHOULD be moved to GlobalUsings.cs and will trigger ATC221
#pragma warning disable ATC220, ATC221

using System.IO;             // ATC221: Should be in GlobalUsings.cs
using System.Linq;           // ATC221: Should be in GlobalUsings.cs
using System.Text;           // ATC221: Should be in GlobalUsings.cs

#pragma warning disable CA1303
#pragma warning disable CA1822

/// <summary>
/// Examples that WILL trigger ATC221.
/// These using directives should be moved to GlobalUsings.cs.
/// </summary>
internal sealed class InvalidExamples
{
    public void DemonstrateInvalidScenarios()
    {
        Console.WriteLine("=== Invalid Scenarios (Will trigger ATC221) ===");
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

        // Scenario 3: Using System.IO locally instead of globally
        Console.WriteLine("3. Using System.IO (should be in GlobalUsings.cs)");
        var tempPath = Path.GetTempPath();
        Console.WriteLine($"   Temp path: {tempPath}");
        Console.WriteLine("   -> FIX: Move 'using System.IO;' to GlobalUsings.cs");
        Console.WriteLine();
    }
}