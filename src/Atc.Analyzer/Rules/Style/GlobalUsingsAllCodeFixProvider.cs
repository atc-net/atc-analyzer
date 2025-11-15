namespace Atc.Analyzer.Rules.Style;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(GlobalUsingsAllCodeFixProvider))]
[Shared]
public sealed class GlobalUsingsAllCodeFixProvider : GlobalUsingsCodeFixProviderBase
{
    protected override string FixableDiagnosticId
        => RuleIdentifierConstants.Style.GlobalUsingsAll;

    protected override string ProviderName
        => nameof(GlobalUsingsAllCodeFixProvider);
}