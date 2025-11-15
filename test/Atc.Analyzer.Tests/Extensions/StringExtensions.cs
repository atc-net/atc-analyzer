namespace Atc.Analyzer.Tests.Extensions;

/// <summary>
/// Extension methods for string manipulation in tests.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Normalizes all line ending characters to LF (\n) for cross-platform test compatibility.
    /// </summary>
    /// <param name="value">The string value to normalize.</param>
    /// <returns>The string with all line endings normalized to LF.</returns>
    /// <remarks>
    /// This method is primarily used in tests to ensure consistent line endings
    /// regardless of the platform or how the string was authored.
    /// Transforms: "\r\n" (CRLF) → "\n" (LF) and "\r" (CR) → "\n" (LF).
    /// </remarks>
    public static string NormalizeLineEndings(this string value)
        => value
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace("\r", "\n", StringComparison.Ordinal);
}