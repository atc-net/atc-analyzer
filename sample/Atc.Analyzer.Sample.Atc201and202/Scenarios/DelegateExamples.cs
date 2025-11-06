namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class DelegateExamples
{
    // Valid: Delegate with multiple parameters
    public delegate void MyDelegate(
        int parameter1,
        string parameter2);

    // Valid: Event handler delegate
    public delegate void MyEventHandler(
        object sender,
        EventArgs e);

    // Valid: Generic delegate
    public delegate TResult MyGenericDelegate<T, TResult>(
        T input,
        int count);

    public void UseDelegates()
    {
        // Valid: Lambda expressions are OK on one line (not covered by ATC201)
        MyDelegate handler = (x, y) => Console.WriteLine($"Lambda: {x}, {y}");
        handler(1, "test");

        // Valid: Anonymous function assignment
        Func<int, int, int> add = (a, b) => a + b;
        Console.WriteLine($"Add result: {add(5, 3)}");
    }
}