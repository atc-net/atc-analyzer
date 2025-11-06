namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class ValidFormatExamples
{
    public void DemonstrateAllValidScenarios()
    {
        Console.WriteLine("\n=== ATC201 Valid Scenarios Demo ===\n");

        // No parameters
        var noParams = new NoParameterExamples();
        noParams.MethodWithNoParameters();
        NoParameterExamples.StaticMethodWithNoParameters();

        // Single parameter
        var singleParam = new SingleParameterExamples();
        singleParam.ShortMethod(42);
        singleParam.MethodWithSingleParameter("test");

        // Single parameter on long lines
        var longLine = new SingleParameterLongLineExamples();
        longLine.MyVeryLongMethodNameThatExceedsEightyCharactersWhenCombinedWithParameter(100);

        // Multiple parameters
        var multiParam = new MultipleParameterExamples();
        multiParam.MethodWithTwoParameters(1, "two");
        multiParam.MethodWithThreeParameters(1, "two", true);
        multiParam.MethodWithMixedTypes(42, "text", DateTime.UtcNow, true);

        // Parameter modifiers
        var modifiers = new ParameterModifierExamples();
        var x = 5;
        var y = "test";
        modifiers.MethodWithRefParameters(ref x, ref y);
        modifiers.MethodWithOutParameters(out var outX, out var outY);

        // Default values
        var defaults = new DefaultParameterExamples();
        defaults.MethodWithDefaultValues();
        defaults.MethodWithDefaultValues(10, "custom");

        // Nullable types
        var nullables = new NullableParameterExamples();
        nullables.MethodWithNullableParams(null, null, null);
        nullables.MethodWithMixedNullability("required", null, 5);

        // Complex types
        var complex = new ComplexParameterTypeExamples();
        complex.MethodWithTuple((1, "test"), 10);
        complex.MethodWithArrays(new[] { "a", "b" }, new[] { 1, 2 });

        // Local functions
        var localFunc = new LocalFunctionExamples();
        localFunc.MethodWithLocalFunctions();

        // Delegates
        var delegates = new DelegateExamples();
        delegates.UseDelegates();

        // Not covered scenarios
        var notCovered = new NotCoveredExamples();
        notCovered.DemonstrateExclusions();

        Console.WriteLine("\n=== All Valid Scenarios Demonstrated ===\n");
    }
}