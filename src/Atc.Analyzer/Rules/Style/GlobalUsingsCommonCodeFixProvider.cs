namespace Atc.Analyzer.Rules.Style;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(GlobalUsingsCommonCodeFixProvider))]
[Shared]
public sealed class GlobalUsingsCommonCodeFixProvider : GlobalUsingsCodeFixProviderBase
{
    protected override string FixableDiagnosticId
        => RuleIdentifierConstants.Style.GlobalUsingsCommon;

    protected override string ProviderName
        => nameof(GlobalUsingsCommonCodeFixProvider);
}