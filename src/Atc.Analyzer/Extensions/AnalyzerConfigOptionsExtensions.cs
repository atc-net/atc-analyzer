namespace Atc.Analyzer.Extensions;

/// <summary>
/// Extension methods for <see cref="AnalyzerConfigOptions"/>.
/// </summary>
public static class AnalyzerConfigOptionsExtensions
{
    private const int DefaultMaxLineLength = 80;
    private const int MinMaxLineLength = 40;
    private const int MaxMaxLineLength = 500;

    /// <summary>
    /// Gets the configured maximum line length for the specified rule from .editorconfig.
    /// </summary>
    /// <param name="options">The analyzer config options.</param>
    /// <param name="ruleId">The rule identifier (e.g., "ATC201").</param>
    /// <returns>
    /// The configured max line length, or 80 if not configured or invalid.
    /// Values are clamped to the range [40, 500].
    /// </returns>
    /// <remarks>
    /// Configure in .editorconfig using:
    /// <code>
    /// dotnet_diagnostic.ATC201.max_line_length = 120
    /// </code>
    /// </remarks>
    public static int GetMaxLineLength(this AnalyzerConfigOptions? options, string ruleId)
    {
        if (options is null)
        {
            return DefaultMaxLineLength;
        }

        var key = $"dotnet_diagnostic.{ruleId}.max_line_length";
        if (!options.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
        {
            return DefaultMaxLineLength;
        }

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var lineLength))
        {
            return DefaultMaxLineLength;
        }

        // Clamp to valid range (Math.Clamp not available in netstandard2.0)
        return Math.Max(MinMaxLineLength, Math.Min(MaxMaxLineLength, lineLength));
    }
}