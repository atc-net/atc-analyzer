namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class GenericExamples<T>
{
    // Valid: Generic method with multiple parameters
    public void GenericMethod<TValue>(
        TValue parameter1,
        string parameter2)
    {
        Console.WriteLine($"Generic: {parameter1}, {parameter2}");
    }

    // Valid: Multiple type parameters
    public void MultipleGenericMethod<TKey, TValue>(
        TKey key,
        TValue value)
    {
        Console.WriteLine($"Multiple generics: {key}, {value}");
    }

    // Valid: Generic with constraints
    public void GenericWithConstraints<TItem>(
        TItem item,
        int count)
        where TItem : class
    {
        Console.WriteLine($"Constrained generic: {item}, {count}");
    }

    public void DemonstrateGenericMethods()
    {
        GenericMethod("value", "text");
        MultipleGenericMethod("key", 42);
        GenericWithConstraints("item", 5);
    }
}