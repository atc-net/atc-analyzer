namespace Atc.Analyzer.Rules.Style.Internal;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct LineInfo(
    int arrowLineNumber,
    int identifierLineNumber,
    int expressionLineNumber,
    int declarationLineLength)
{
    public int ArrowLineNumber { get; } = arrowLineNumber;

    public int IdentifierLineNumber { get; } = identifierLineNumber;

    public int ExpressionLineNumber { get; } = expressionLineNumber;

    public int DeclarationLineLength { get; } = declarationLineLength;
}