namespace Atc.Analyzer.Sample.Atc203.Internal.Extensions;

internal static class StringAssertionExtensions
{
    public static StringAssertion Should(this string value) => new(value);
}