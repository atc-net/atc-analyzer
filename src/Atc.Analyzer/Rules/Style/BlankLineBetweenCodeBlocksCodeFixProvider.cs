namespace Atc.Analyzer.Rules.Style;

/// <summary>
/// Code fix provider that adds or removes blank lines between consecutive code blocks.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BlankLineBetweenCodeBlocksCodeFixProvider))]
[Shared]
public sealed class BlankLineBetweenCodeBlocksCodeFixProvider : CodeFixProvider
{
    private const string AddBlankLineTitle = "Add blank line between code blocks";
    private const string RemoveExtraBlankLinesTitle = "Remove extra blank lines";

    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.Style.BlankLineBetweenCodeBlocks];

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document
            .GetSyntaxRootAsync(context.CancellationToken)
            .ConfigureAwait(false);

        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the statement identified by the diagnostic (the second statement in the pair)
        var node = root.FindNode(diagnosticSpan);
        if (node is not StatementSyntax secondStatement)
        {
            return;
        }

        // Get the parent block
        if (secondStatement.Parent is not BlockSyntax block)
        {
            return;
        }

        // Find the index of this statement - ensure this is not the first statement
        var index = block.Statements.IndexOf(secondStatement);
        if (index <= 0)
        {
            return;
        }

        // Determine which fix to offer based on the diagnostic message
        var isMissingBlankLine = diagnostic
            .GetMessage(CultureInfo.InvariantCulture)
            .Contains("Add a blank line", StringComparison.Ordinal);

        var title = isMissingBlankLine ? AddBlankLineTitle : RemoveExtraBlankLinesTitle;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: title,
                createChangedDocument: c => FixBlankLinesAsync(
                    context.Document,
                    secondStatement,
                    c),
                equivalenceKey: nameof(BlankLineBetweenCodeBlocksCodeFixProvider)),
            context.Diagnostics);
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "OK.")]
    private static async Task<Document> FixBlankLinesAsync(
        Document document,
        StatementSyntax secondStatement,
        CancellationToken cancellationToken)
    {
        var root = await document
            .GetSyntaxRootAsync(cancellationToken)
            .ConfigureAwait(false);

        if (root is null)
        {
            return document;
        }

        // Detect end-of-line style from the document
        var endOfLine = root.SyntaxTree.GetEndOfLineTrivia();

        // Get the leading trivia of the second statement
        var leadingTrivia = secondStatement.GetLeadingTrivia();

        // Separate comments and preprocessor directives from whitespace
        var preservedTrivia = new List<SyntaxTrivia>();
        var hasFoundPreservedTrivia = false;

        foreach (var trivia in leadingTrivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                trivia.IsKind(SyntaxKind.IfDirectiveTrivia) ||
                trivia.IsKind(SyntaxKind.ElseDirectiveTrivia) ||
                trivia.IsKind(SyntaxKind.ElifDirectiveTrivia) ||
                trivia.IsKind(SyntaxKind.EndIfDirectiveTrivia) ||
                trivia.IsKind(SyntaxKind.RegionDirectiveTrivia) ||
                trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia))
            {
                hasFoundPreservedTrivia = true;
                preservedTrivia.Add(trivia);
            }
            else if (hasFoundPreservedTrivia && trivia.IsKind(SyntaxKind.EndOfLineTrivia))
            {
                // Keep the newline after the preserved trivia
                preservedTrivia.Add(trivia);
            }
        }

        // Get the indentation from the second statement
        var indentation = GetIndentation(secondStatement);

        // Build new trivia with exactly one blank line
        var newTriviaList = new List<SyntaxTrivia>
        {
            endOfLine, // This is the blank line
        };

        // Add back any preserved trivia (comments, preprocessor directives) with their proper trivia
        if (preservedTrivia.Count > 0)
        {
            newTriviaList.Add(SyntaxFactory.Whitespace(indentation));
            newTriviaList.AddRange(preservedTrivia);
        }

        // Add the indentation for the statement
        newTriviaList.Add(SyntaxFactory.Whitespace(indentation));

        var newSecondStatement = secondStatement.WithLeadingTrivia(newTriviaList);

        var newRoot = root.ReplaceNode(secondStatement, newSecondStatement);
        return document.WithSyntaxRoot(newRoot);
    }

    private static string GetIndentation(SyntaxNode node)
    {
        var leadingTrivia = node.GetLeadingTrivia();
        var whitespace = leadingTrivia
            .Reverse()
            .FirstOrDefault(t => t.IsKind(SyntaxKind.WhitespaceTrivia));

        if (whitespace != default)
        {
            return whitespace.ToString();
        }

        // Fallback: calculate from source text
        var sourceText = node.SyntaxTree?.GetText();
        if (sourceText is null)
        {
            return "    ";
        }

        var lineSpan = node
            .GetLocation()
            .GetLineSpan();

        var startLine = lineSpan.StartLinePosition.Line;

        if (startLine < 0 || startLine >= sourceText.Lines.Count)
        {
            return "    ";
        }

        var line = sourceText.Lines[startLine];
        var lineText = line.ToString();

        // Count leading whitespace
        var whitespaceCount = 0;
        foreach (var ch in lineText)
        {
            if (ch == ' ')
            {
                whitespaceCount++;
            }
            else if (ch == '\t')
            {
                whitespaceCount += 4;
            }
            else
            {
                break;
            }
        }

        return new string(' ', whitespaceCount);
    }
}