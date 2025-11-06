namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class DerivedClass : BaseClass
{
    // Valid: Override method with multiple parameters
    public override void VirtualMethod(
        int parameter1,
        string parameter2)
    {
        Console.WriteLine($"Derived override: {parameter1}, {parameter2}");
    }

    // Valid: Override abstract method
    public override void AbstractMethod(
        int parameter1,
        string parameter2)
    {
        Console.WriteLine($"Abstract implementation: {parameter1}, {parameter2}");
    }

    // Valid: Override with no parameters
    public override void VirtualNoParams()
    {
        Console.WriteLine("Derived virtual no params");
    }

    public void DemonstrateOverrides()
    {
        VirtualMethod(1, "test");
        AbstractMethod(2, "test2");
        VirtualNoParams();
    }
}