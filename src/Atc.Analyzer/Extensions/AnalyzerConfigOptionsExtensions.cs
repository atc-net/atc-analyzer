namespace Atc.Analyzer.Extensions;

/// <summary>
/// Extension methods for <see cref="AnalyzerConfigOptions"/>.
/// </summary>
[SuppressMessage("", "CA1034:Do not nest type", Justification = "OK - CLang14 - extension")]
[SuppressMessage("", "S2325:Make a static method", Justification = "OK - CLang14 - extension")]
public static class AnalyzerConfigOptionsExtensions
{
    private const int DefaultMaxLineLength = 80;
    private const int MinMaxLineLength = 40;
    private const int MaxMaxLineLength = 500;

    private const int DefaultMinCount = 2;
    private const int MinMinCount = 2;
    private const int MaxMinCount = 10;

    private static readonly string[] DefaultNamespacePrefixes = ["System", "Microsoft", "Atc"];

    /// <param name="options">The analyzer config options.</param>
    extension(AnalyzerConfigOptions? options)
    {
        /// <summary>
        /// Gets the configured maximum line length for the specified rule from .editorconfig.
        /// </summary>
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
        public int GetMaxLineLength(string ruleId)
        {
            if (options is null)
            {
                return DefaultMaxLineLength;
            }

            var key = $"dotnet_diagnostic.{ruleId}.max_line_length";

            if (!options.TryGetValue(key, out var value) ||
                string.IsNullOrWhiteSpace(value) ||
                !int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var lineLength))
            {
                return DefaultMaxLineLength;
            }

            // Clamp to valid range (Math.Clamp not available in netstandard2.0)
            return Math.Max(MinMaxLineLength, Math.Min(MaxMaxLineLength, lineLength));
        }

        /// <summary>
        /// Gets the configured minimum parameter count for the specified rule from .editorconfig.
        /// </summary>
        /// <param name="ruleId">The rule identifier (e.g., "ATC202").</param>
        /// <returns>
        /// The configured minimum parameter count, or 2 if not configured or invalid.
        /// Values are clamped to the range [2, 10].
        /// </returns>
        /// <remarks>
        /// Configure in .editorconfig using:
        /// <code>
        /// dotnet_diagnostic.ATC202.min_parameter_count = 3
        /// </code>
        /// </remarks>
        public int GetMinParameterCount(string ruleId)
        {
            if (options is null)
            {
                return DefaultMinCount;
            }

            var key = $"dotnet_diagnostic.{ruleId}.min_parameter_count";

            if (!options.TryGetValue(key, out var value) ||
                string.IsNullOrWhiteSpace(value) ||
                !int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var count))
            {
                return DefaultMinCount;
            }

            return Math.Max(MinMinCount, Math.Min(MaxMinCount, count));
        }

        /// <summary>
        /// Gets the configured minimum chain length for the specified rule from .editorconfig.
        /// </summary>
        /// <param name="ruleId">The rule identifier (e.g., "ATC203").</param>
        /// <returns>
        /// The configured minimum chain length, or 2 if not configured or invalid.
        /// Values are clamped to the range [2, 10].
        /// </returns>
        /// <remarks>
        /// Configure in .editorconfig using:
        /// <code>
        /// dotnet_diagnostic.ATC203.min_chain_length = 3
        /// </code>
        /// </remarks>
        public int GetMinChainLength(string ruleId)
        {
            if (options is null)
            {
                return DefaultMinCount;
            }

            var key = $"dotnet_diagnostic.{ruleId}.min_chain_length";

            if (!options.TryGetValue(key, out var value) ||
                string.IsNullOrWhiteSpace(value) ||
                !int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var length))
            {
                return DefaultMinCount;
            }

            return Math.Max(MinMinCount, Math.Min(MaxMinCount, length));
        }

        /// <summary>
        /// Gets the configured namespace prefixes for the specified rule from .editorconfig.
        /// </summary>
        /// <param name="ruleId">The rule identifier (e.g., "ATC221").</param>
        /// <returns>
        /// The configured namespace prefixes as an array, or ["System", "Microsoft", "Atc"] if not configured.
        /// Values should be semicolon-separated in the config.
        /// </returns>
        /// <remarks>
        /// Configure in .editorconfig using:
        /// <code>
        /// dotnet_diagnostic.ATC221.namespace_prefixes = System;Microsoft;Atc;MyCompany
        /// </code>
        /// </remarks>
        public string[] GetNamespacePrefixes(string ruleId)
        {
            if (options is null)
            {
                return DefaultNamespacePrefixes;
            }

            var key = $"dotnet_diagnostic.{ruleId}.namespace_prefixes";
            if (!options.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
            {
                return DefaultNamespacePrefixes;
            }

            var prefixes = value
                .Split(';')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToArray();

            return prefixes.Length > 0
                ? prefixes
                : DefaultNamespacePrefixes;
        }
    }
}