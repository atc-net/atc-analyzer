// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach
namespace Atc.Analyzer.Rules.Style;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ParameterSeparationCodeFixProvider))]
[Shared]
public sealed class ParameterSeparationCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.Style.ParameterSeparation];

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the node identified by the diagnostic
        var node = root.FindNode(diagnosticSpan);

        // The diagnostic points to the parameter list, find the declaration
        ParameterListSyntax? parameterList = null;
        SyntaxNode? declaration = null;

        // Check if the node is the parameter list itself
        if (node is ParameterListSyntax paramList)
        {
            parameterList = paramList;

            // Find the parent declaration
            declaration = node.Parent;
        }

        // If we have a declaration, register the code fix
        if (declaration is not null &&
            parameterList is not null &&
            declaration
                is MethodDeclarationSyntax
                or ConstructorDeclarationSyntax
                or LocalFunctionStatementSyntax
                or DelegateDeclarationSyntax)
        {
            // Handle different declaration types
            RegisterCodeFix(context, declaration, parameterList);
        }
    }

    private static void RegisterCodeFix(
        CodeFixContext context,
        SyntaxNode declaration,
        ParameterListSyntax parameterList)
    {
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Separate multi parameters on individual lines",
                createChangedDocument: c => FormatParametersAsync(context.Document, declaration, parameterList, c),
                equivalenceKey: nameof(ParameterSeparationCodeFixProvider)),
            context.Diagnostics);
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "OK.")]
    private static async Task<Document> FormatParametersAsync(
        Document document,
        SyntaxNode declaration,
        ParameterListSyntax parameterList,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        // Calculate indentation - get the declaration's indentation + 4 spaces for parameters
        var indentation = GetIndentation(declaration);
        var parameterIndentation = indentation + "    ";

        // Create proper line break trivia - detect from document
        var endOfLine = await GetEndOfLineTriviaFromDocumentAsync(document, root, cancellationToken).ConfigureAwait(false);

        // Build the formatted parameter list manually with correct trivia placement
        // Based on minimal reproduction test: APPROACH 2 which works!
        var formattedParams = new List<SyntaxNodeOrToken>();

        for (var i = 0; i < parameterList.Parameters.Count; i++)
        {
            var parameter = parameterList.Parameters[i];

            // First parameter gets just leading whitespace (newline comes from open paren)
            // Subsequent parameters get newline + whitespace in leading trivia
            var leadingTrivia = i == 0
                ? SyntaxFactory.TriviaList(SyntaxFactory.Whitespace(parameterIndentation))
                : SyntaxFactory.TriviaList(endOfLine, SyntaxFactory.Whitespace(parameterIndentation));

            var formattedParameter = parameter
                .WithLeadingTrivia(leadingTrivia)
                .WithTrailingTrivia();  // No trailing trivia

            formattedParams.Add(formattedParameter);

            // Add comma separator with NO trivia (not even empty - just the comma itself)
            if (i < parameterList.Parameters.Count - 1)
            {
                formattedParams.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
            }
        }

        // Create the new parameter list from formatted nodes and tokens
        var newParameters = SyntaxFactory.SeparatedList<ParameterSyntax>(formattedParams);

        // Create new parameter list:
        // - Open paren gets trailing newline
        // - Close paren gets no leading trivia (stays on same line as last parameter)
        var newParameterList = parameterList
            .WithOpenParenToken(parameterList.OpenParenToken.WithTrailingTrivia(endOfLine))
            .WithParameters(newParameters)
            .WithCloseParenToken(parameterList.CloseParenToken
                .WithLeadingTrivia()
                .WithTrailingTrivia(parameterList.CloseParenToken.TrailingTrivia));

        // Replace the parameter list in the declaration
        var newDeclaration = declaration switch
        {
            MethodDeclarationSyntax method => method.WithParameterList(newParameterList),
            ConstructorDeclarationSyntax constructor => constructor.WithParameterList(newParameterList),
            LocalFunctionStatementSyntax localFunction => localFunction.WithParameterList(newParameterList),
            DelegateDeclarationSyntax delegateDecl => delegateDecl.WithParameterList(newParameterList),
            _ => declaration,
        };

        var newRoot = root.ReplaceNode(declaration, newDeclaration);
        return document.WithSyntaxRoot(newRoot);
    }

    private static string GetIndentation(SyntaxNode node)
    {
        var leadingTrivia = node.GetLeadingTrivia();
        var lastWhitespace = leadingTrivia
            .Reverse()
            .FirstOrDefault(t => t.IsKind(SyntaxKind.WhitespaceTrivia));

        if (lastWhitespace != default)
        {
            return lastWhitespace.ToString();
        }

        // If no whitespace found in leading trivia, calculate from the tree
        var sourceText = node.SyntaxTree.GetText();
        var lineSpan = node.GetLocation().GetLineSpan();
        var startLine = lineSpan.StartLinePosition.Line;
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
                whitespaceCount += Constants.TabSpaces; // Treat tabs as 4 spaces
            }
            else
            {
                break;
            }
        }

        return new string(' ', whitespaceCount);
    }

    private static async Task<SyntaxTrivia> GetEndOfLineTriviaFromDocumentAsync(
        Document document,
        SyntaxNode root,
        CancellationToken cancellationToken)
    {
        // Try to read the end_of_line setting from EditorConfig
        var syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        if (syntaxTree is not null)
        {
            var options = document.Project.AnalyzerOptions.AnalyzerConfigOptionsProvider.GetOptions(syntaxTree);
            if (options.TryGetValue("end_of_line", out var endOfLineValue))
            {
                return endOfLineValue switch
                {
                    "crlf" => SyntaxFactory.CarriageReturnLineFeed,
                    "lf" => SyntaxFactory.LineFeed,
                    "cr" => SyntaxFactory.CarriageReturn,
                    _ => SyntaxFactory.LineFeed,
                };
            }
        }

        // Fallback: check the source text for line endings
        var sourceText = await root.SyntaxTree.GetTextAsync(cancellationToken).ConfigureAwait(false);
        foreach (var line in sourceText.Lines)
        {
            if (line.EndIncludingLineBreak > line.End)
            {
                var lineBreakText = sourceText.ToString(new Microsoft.CodeAnalysis.Text.TextSpan(line.End, line.EndIncludingLineBreak - line.End));
                return lineBreakText == "\r\n"
                    ? SyntaxFactory.CarriageReturnLineFeed
                    : SyntaxFactory.LineFeed;
            }
        }

        // Default to LF if we can't detect
        return SyntaxFactory.LineFeed;
    }
}