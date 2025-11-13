namespace Atc.Analyzer.Sample.Atc201and202;

/// <summary>
/// This sample demonstrates all VALID scenarios for ATC201 and ATC202 analyzers.
/// ATC201 (ParameterInlineAnalyzer): Single parameter should be kept inline when declaration is short
/// ATC202 (ParameterSeparationAnalyzer): Method parameters should be separated on individual lines
/// - Methods with 2+ parameters: ALL parameters must be on separate lines (ATC202)
/// - Methods with 1 parameter on long line: Must be on a new line if total line length exceeds 80 characters (ATC202)
/// - Methods with 1 parameter on short line: Must be on the same line (ATC201)
/// - Applies to: method declarations, constructors, local functions, delegates
/// - Does NOT apply to: method invocations, lambdas, LINQ, ternary operators, indexers
/// </summary>
internal class Program
{
    private static void Main()
    {
        Console.WriteLine("ATC201/ATC202: Parameter Formatting Analyzers - Valid Scenarios Demo");

        // Demonstrate various valid scenarios
        var demo = new ValidFormatExamples();
        demo.DemonstrateAllValidScenarios();

        var derived = new DerivedClass();
        derived.DemonstrateOverrides();

        var generic = new GenericExamples<string>();
        generic.DemonstrateGenericMethods();
    }
}