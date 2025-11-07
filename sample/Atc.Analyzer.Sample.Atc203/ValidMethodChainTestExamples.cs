namespace Atc.Analyzer.Sample.Atc203;

public sealed class ValidMethodChainTestExamples
{
    [Fact]
    public void DemonstrateAllValidTestScenarios()
    {
        // Example 1: Simple Trim check with And
        var trimmed = "  Hello World  ".Trim();
        trimmed
            .Should().Be("Hello World")
            .And.NotBeNullOrWhiteSpace();

        // Example 2: Case transformation with And
        var upper = "test".ToUpperInvariant();
        upper
            .Should().Be("TEST")
            .And.StartWith("TE")
            .And.EndWith("ST");
    }
}