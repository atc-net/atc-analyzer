namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class LocalFunctionExamples
{
    public void MethodWithLocalFunctions()
    {
        // Valid: Local function with no parameters
        static void LocalNoParams()
        {
            Console.WriteLine("Local function no params");
        }

        // Valid: Local function with single parameter
        static void LocalSingleParam(int x)
        {
            Console.WriteLine($"Local single param: {x}");
        }

        // Valid: Local function with multiple parameters on separate lines
        static void LocalMultipleParams(
            int x,
            string y)
        {
            Console.WriteLine($"Local multiple params: {x}, {y}");
        }

        // Valid: Async local function
        static async Task LocalAsync(
            int id,
            string name)
        {
            await Task.CompletedTask;
            Console.WriteLine($"Local async: {id}, {name}");
        }

        // Call local functions
        LocalNoParams();
        LocalSingleParam(42);
        LocalMultipleParams(10, "test");
        LocalAsync(1, "async test")
            .Wait();
    }
}