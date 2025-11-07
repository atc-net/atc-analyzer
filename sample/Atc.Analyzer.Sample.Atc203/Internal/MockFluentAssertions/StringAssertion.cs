// ReSharper disable ConvertIfStatementToReturnStatement
namespace Atc.Analyzer.Sample.Atc203.Internal.MockFluentAssertions;

internal sealed class StringAssertion(string value)
{
    public StringAssertion Be(string expected)
    {
        if (value != expected)
        {
            throw new InvalidOperationException($"Expected '{expected}' but got '{value}'");
        }

        return this;
    }

    public StringAssertion And => this;

    public StringAssertion NotBeNullOrWhiteSpace()
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException("Expected value to not be null or whitespace");
        }

        return this;
    }

    public StringAssertion StartWith(string expected)
    {
        if (!value.StartsWith(expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected value to start with '{expected}'");
        }

        return this;
    }

    public StringAssertion EndWith(string expected)
    {
        if (!value.EndsWith(expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected value to end with '{expected}'");
        }

        return this;
    }
}