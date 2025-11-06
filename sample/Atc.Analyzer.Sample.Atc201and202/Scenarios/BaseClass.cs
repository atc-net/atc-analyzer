namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal abstract class BaseClass
{
    // Valid: Virtual method with multiple parameters
    public virtual void VirtualMethod(
        int parameter1,
        string parameter2)
    {
        Console.WriteLine($"Base virtual: {parameter1}, {parameter2}");
    }

    // Valid: Abstract method with multiple parameters
    public abstract void AbstractMethod(
        int parameter1,
        string parameter2);

    // Valid: Virtual with no parameters
    public virtual void VirtualNoParams()
    {
        Console.WriteLine("Virtual no params");
    }
}