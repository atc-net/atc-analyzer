namespace Atc.Analyzer.Rules.Style;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ParameterInlineCodeFixProvider))]
[Shared]
public sealed class ParameterInlineCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.Style.ParameterInline];

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
            // Determine which fix to offer based on the diagnostic message
            var message = diagnostic.GetMessage(CultureInfo.InvariantCulture);
            var title = message.IndexOf("should be on a new line", StringComparison.Ordinal) >= 0
                ? "Move parameter to new line"
                : "Move parameter inline";

            RegisterCodeFix(context, declaration, parameterList, title);
        }
    }

    private static void RegisterCodeFix(
        CodeFixContext context,
        SyntaxNode declaration,
        ParameterListSyntax parameterList,
        string title)
    {
        context.RegisterCodeFix(
            CodeAction.Create(
                title: title,
                createChangedDocument: c => FormatParameterAsync(context.Document, declaration, parameterList, title, c),
                equivalenceKey: nameof(ParameterInlineCodeFixProvider)),
            context.Diagnostics);
    }

    private static async Task<Document> FormatParameterAsync(
        Document document,
        SyntaxNode declaration,
        ParameterListSyntax parameterList,
        string actionTitle,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        // Get the single parameter
        if (parameterList.Parameters.Count != 1)
        {
            return document;
        }

        var parameter = parameterList.Parameters[0];
        ParameterListSyntax newParameterList;

        if (actionTitle.IndexOf("inline", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            // Move parameter to same line as opening paren
            newParameterList = FormatParameterInline(parameterList, parameter);
        }
        else
        {
            // Move parameter to new line
            var indentation = GetIndentation(declaration);
            newParameterList = await FormatParameterOnNewLineAsync(parameterList, parameter, indentation, document, root, cancellationToken).ConfigureAwait(false);
        }

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

    private static ParameterListSyntax FormatParameterInline(
        ParameterListSyntax parameterList,
        ParameterSyntax parameter)
    {
        // Remove all leading and trailing trivia from parameter
        var cleanParameter = parameter
            .WithLeadingTrivia()
            .WithTrailingTrivia();

        // Update the parameter list with clean tokens and parameter
        return parameterList
            .WithOpenParenToken(parameterList.OpenParenToken.WithTrailingTrivia())
            .WithParameters(SyntaxFactory.SingletonSeparatedList(cleanParameter))
            .WithCloseParenToken(parameterList.CloseParenToken
                .WithLeadingTrivia()
                .WithTrailingTrivia(parameterList.CloseParenToken.TrailingTrivia));
    }

    private static async Task<ParameterListSyntax> FormatParameterOnNewLineAsync(
        ParameterListSyntax parameterList,
        ParameterSyntax parameter,
        string baseIndentation,
        Document document,
        SyntaxNode root,
        CancellationToken cancellationToken)
    {
        // Calculate indentation for the parameter (base + 4 spaces)
        var parameterIndentation = baseIndentation + "    ";

        // Create line break trivia - detect from document
        var endOfLine = await GetEndOfLineTriviaFromDocumentAsync(document, root, cancellationToken).ConfigureAwait(false);

        // Format the parameter with proper indentation
        var formattedParameter = parameter
            .WithLeadingTrivia(
                SyntaxFactory.TriviaList(
                    endOfLine,
                    SyntaxFactory.Whitespace(parameterIndentation)))
            .WithTrailingTrivia();

        // Create the new parameter list
        return parameterList
            .WithOpenParenToken(parameterList.OpenParenToken.WithTrailingTrivia())
            .WithParameters(SyntaxFactory.SingletonSeparatedList(formattedParameter))
            .WithCloseParenToken(parameterList.CloseParenToken
                .WithLeadingTrivia()
                .WithTrailingTrivia(parameterList.CloseParenToken.TrailingTrivia));
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