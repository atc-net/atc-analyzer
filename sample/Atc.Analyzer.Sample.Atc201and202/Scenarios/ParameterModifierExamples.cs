namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class ParameterModifierExamples
{
    // Valid: Multiple parameters with ref modifier
    public void MethodWithRefParameters(
        ref int x,
        ref string y)
    {
        x *= 2;
        y += " modified";
    }

    // Valid: Multiple parameters with out modifier
    public void MethodWithOutParameters(
        out int x,
        out string y)
    {
        x = 42;
        y = "output value";
    }

    // Valid: Mixed parameter modifiers
    public void MethodWithMixedModifiers(
        ref int x,
        out string y,
        in bool z)
    {
        x += z ? 1 : 0;
        y = "result";
    }

    // Valid: ref and out with normal parameters
    public void MethodWithMixedParamTypes(
        int input,
        ref int refParam,
        out string outParam)
    {
        refParam = input * 2;
        outParam = input.ToString(CultureInfo.InvariantCulture);
    }
}