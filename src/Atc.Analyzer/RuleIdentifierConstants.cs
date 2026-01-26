namespace Atc.Analyzer;

/// <summary>
/// Defines the rule identifier for source generator diagnostics.
/// </summary>
internal static class RuleIdentifierConstants
{
    /// <summary>
    /// Style - RuleIdentifiers from ATC201 to RuleIdentifier ATC299.
    /// </summary>
    internal static class Style
    {
        /// <summary>
        /// Single parameter should be formatted correctly based on line length.
        /// </summary>
        internal const string ParameterInline = "ATC201";

        /// <summary>
        /// Multi parameters should be separated on individual lines.
        /// </summary>
        internal const string ParameterSeparation = "ATC202";

        /// <summary>
        /// Method chains with 2 or more calls should be placed on separate lines.
        /// </summary>
        internal const string MethodChainSeparation = "ATC203";

        /// <summary>
        /// Chained method calls in interpolated strings should be simplified.
        /// </summary>
        internal const string InterpolationMethodChain = "ATC204";

        /// <summary>
        /// Use expression body syntax when appropriate.
        /// </summary>
        internal const string ExpressionBody = "ATC210";

        /// <summary>
        /// Use global usings for all namespaces.
        /// </summary>
        internal const string GlobalUsingsAll = "ATC220";

        /// <summary>
        /// Use global usings for common namespaces (System, Microsoft, Atc).
        /// </summary>
        internal const string GlobalUsingsCommon = "ATC221";

        /// <summary>
        /// Require one blank line between consecutive code blocks.
        /// </summary>
        internal const string BlankLineBetweenCodeBlocks = "ATC230";
    }

    /// <summary>
    /// Usage - RuleIdentifiers from ATC301 to RuleIdentifier ATC399.
    /// </summary>
    internal static class Usage
    {
        /// <summary>
        /// Remove redundant RegexOptions.Compiled flag from [GeneratedRegex] attribute.
        /// </summary>
        internal const string GeneratedRegexCompiledFlag = "ATC301";
    }
}