namespace Atc.Analyzer.Sample.Atc301.Scenarios;

/// <summary>
/// Invalid examples that would trigger ATC301 warnings.
/// These are commented out to allow the sample project to build without warnings.
/// </summary>
internal static partial class InvalidExamples
{
    // INVALID: GeneratedRegex with RegexOptions.Compiled flag
    // The Compiled flag is redundant because GeneratedRegex already generates compiled code.
    // ⚠️ ATC301: The RegexOptions.Compiled flag is redundant for [GeneratedRegex] attributes
    //
    // [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    // private static partial Regex InvalidCompiledOnlyRegex();

    // INVALID: GeneratedRegex with Compiled flag combined with other options
    // ⚠️ ATC301: The RegexOptions.Compiled flag is redundant for [GeneratedRegex] attributes
    //
    // [GeneratedRegex(@"[a-z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    // private static partial Regex InvalidCompiledWithIgnoreCaseRegex();

    // INVALID: GeneratedRegex with Compiled flag in named parameter
    // ⚠️ ATC301: The RegexOptions.Compiled flag is redundant for [GeneratedRegex] attributes
    //
    // [GeneratedRegex(
    //     pattern: @"[$>]",
    //     options: RegexOptions.Compiled | RegexOptions.ExplicitCapture,
    //     matchTimeoutMilliseconds: 1000)]
    // private static partial Regex InvalidNamedParameterRegex();

    // INVALID: Multiple flags including Compiled
    // ⚠️ ATC301: The RegexOptions.Compiled flag is redundant for [GeneratedRegex] attributes
    //
    // [GeneratedRegex(
    //     @"^\d+$",
    //     RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    // private static partial Regex InvalidMultipleFlagsRegex();
}