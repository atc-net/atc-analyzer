// ReSharper disable RedundantVerbatimStringPrefix
namespace Atc.Analyzer.Sample.Atc301.Scenarios;

#pragma warning disable CA1303, MA0009, MA0110, SYSLIB1045

/// <summary>
/// Valid examples of [GeneratedRegex] usage without RegexOptions.Compiled flag.
/// </summary>
internal static partial class ValidExamples
{
    /// <summary>
    /// GeneratedRegex without any options parameter - VALID.
    /// </summary>
    [GeneratedRegex(@"\d+")]
    private static partial Regex SimpleNumberRegex();

    /// <summary>
    /// GeneratedRegex with IgnoreCase option (no Compiled flag) - VALID.
    /// </summary>
    [GeneratedRegex(@"[a-z]+", RegexOptions.IgnoreCase)]
    private static partial Regex CaseInsensitiveRegex();

    /// <summary>
    /// GeneratedRegex with multiple options (no Compiled flag) - VALID.
    /// </summary>
    [GeneratedRegex(@"^\d+$", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex MultipleOptionsRegex();

    /// <summary>
    /// GeneratedRegex with named parameters (no Compiled flag) - VALID.
    /// </summary>
    [GeneratedRegex(
        pattern: @"[$>]",
        options: RegexOptions.ExplicitCapture,
        matchTimeoutMilliseconds: 1000)]
    private static partial Regex UserOrFtpPromptRegex();

    /// <summary>
    /// GeneratedRegex with None option explicitly - VALID.
    /// </summary>
    [GeneratedRegex(@"\w+", RegexOptions.None)]
    private static partial Regex NoneOptionRegex();

    /// <summary>
    /// Regular (non-generated) Regex with Compiled flag - VALID.
    /// This is a normal Regex construction, not a GeneratedRegex attribute,
    /// so ATC301 does not apply here.
    /// </summary>
    private static readonly Regex TraditionalCompiledRegex =
        new(@"\d+", RegexOptions.Compiled);

    public static void DemonstrateAllValidScenarios()
    {
        Console.WriteLine("Valid GeneratedRegex Examples:");
        Console.WriteLine();

        // Test SimpleNumberRegex
        var numberMatch = SimpleNumberRegex().Match("abc123def");
        Console.WriteLine($"SimpleNumberRegex matched: {numberMatch.Value}");

        // Test CaseInsensitiveRegex
        var caseMatch = CaseInsensitiveRegex().Match("ABC");
        Console.WriteLine($"CaseInsensitiveRegex matched: {caseMatch.Success}");

        // Test MultipleOptionsRegex
        var multiMatch = MultipleOptionsRegex().Match("123");
        Console.WriteLine($"MultipleOptionsRegex matched: {multiMatch.Success}");

        // Test UserOrFtpPromptRegex
        var promptMatch = UserOrFtpPromptRegex().Match("user$ ");
        Console.WriteLine($"UserOrFtpPromptRegex matched: {promptMatch.Value}");

        // Test NoneOptionRegex
        var noneMatch = NoneOptionRegex().Match("test");
        Console.WriteLine($"NoneOptionRegex matched: {noneMatch.Value}");

        // Test TraditionalCompiledRegex
        var traditionalMatch = TraditionalCompiledRegex.Match("456");
        Console.WriteLine($"TraditionalCompiledRegex matched: {traditionalMatch.Value}");
    }
}