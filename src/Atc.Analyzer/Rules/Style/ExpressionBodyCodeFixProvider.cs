namespace Atc.Analyzer.Rules.Style;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExpressionBodyCodeFixProvider))]
[Shared]
public sealed class ExpressionBodyCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.Style.ExpressionBody];

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
        var node = root.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

        // Check if the diagnostic is on an arrow expression clause (move arrow to new line)
        // The diagnostic is reported on the arrow token, but FindNode returns the ArrowExpressionClauseSyntax
        if (node is ArrowExpressionClauseSyntax)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Move '=>' to new line",
                    createChangedDocument: c => MoveArrowToNewLineAsync(context.Document, node, c),
                    equivalenceKey: nameof(ExpressionBodyCodeFixProvider) + "_MoveArrowToNewLine"),
                diagnostic);
        }

        // Check if we need to convert block body to expression body
        else if (node is MethodDeclarationSyntax or AccessorDeclarationSyntax)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Use expression body",
                    createChangedDocument: c => ConvertToExpressionBodyAsync(context.Document, node, c),
                    equivalenceKey: nameof(ExpressionBodyCodeFixProvider) + "_ConvertToExpressionBody"),
                diagnostic);
        }
    }

    private static async Task<Document> ConvertToExpressionBodyAsync(
        Document document,
        SyntaxNode node,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        SyntaxNode? newNode = node switch
        {
            MethodDeclarationSyntax method => ConvertMethodToExpressionBody(method),
            AccessorDeclarationSyntax accessor => ConvertAccessorToExpressionBody(accessor),
            _ => null,
        };

        if (newNode is null)
        {
            return document;
        }

        var newRoot = root.ReplaceNode(node, newNode);
        return document.WithSyntaxRoot(newRoot);
    }

    private static MethodDeclarationSyntax? ConvertMethodToExpressionBody(MethodDeclarationSyntax method)
    {
        if (method.Body is null || method.Body.Statements.Count != 1)
        {
            return null;
        }

        if (method.Body.Statements[0] is not ReturnStatementSyntax returnStatement)
        {
            return null;
        }

        if (returnStatement.Expression is null)
        {
            return null;
        }

        // Create the arrow expression clause
        var arrowExpression = SyntaxFactory.ArrowExpressionClause(returnStatement.Expression.WithoutTrivia())
            .WithArrowToken(SyntaxFactory.Token(
                SyntaxFactory.TriviaList(SyntaxFactory.Space),
                SyntaxKind.EqualsGreaterThanToken,
                SyntaxFactory.TriviaList(SyntaxFactory.Space)));

        // Create the new method with expression body
        // Remove any trailing trivia from parameter list to avoid extra newlines
        var parameterList = method.ParameterList.WithoutTrailingTrivia();

        return method
            .WithParameterList(parameterList)
            .WithExpressionBody(arrowExpression)
            .WithBody(null)
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    private static AccessorDeclarationSyntax? ConvertAccessorToExpressionBody(AccessorDeclarationSyntax accessor)
    {
        if (accessor.Body is null || accessor.Body.Statements.Count != 1)
        {
            return null;
        }

        if (accessor.Body.Statements[0] is not ReturnStatementSyntax returnStatement)
        {
            return null;
        }

        if (returnStatement.Expression is null)
        {
            return null;
        }

        // Create the arrow expression clause
        var arrowExpression = SyntaxFactory.ArrowExpressionClause(returnStatement.Expression.WithoutTrivia())
            .WithArrowToken(SyntaxFactory.Token(
                SyntaxFactory.TriviaList(SyntaxFactory.Space),
                SyntaxKind.EqualsGreaterThanToken,
                SyntaxFactory.TriviaList(SyntaxFactory.Space)));

        // Create the new accessor with expression body
        // Remove any trailing trivia from keyword to avoid extra newlines
        var keyword = accessor.Keyword.WithTrailingTrivia(SyntaxFactory.TriviaList());

        return accessor
            .WithKeyword(keyword)
            .WithExpressionBody(arrowExpression)
            .WithBody(null)
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    private static async Task<Document> MoveArrowToNewLineAsync(
        Document document,
        SyntaxNode node,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        // Find the arrow token
        var arrowToken = node.ChildTokens().FirstOrDefault(t => t.IsKind(SyntaxKind.EqualsGreaterThanToken));
        if (arrowToken == default)
        {
            return document;
        }

        // Find the declaration node
        var declarationNode = node.AncestorsAndSelf().FirstOrDefault(n =>
            n is MethodDeclarationSyntax or PropertyDeclarationSyntax or AccessorDeclarationSyntax);

        if (declarationNode is null)
        {
            return document;
        }

        // Get the base indentation
        var baseIndentation = GetBaseIndentation(declarationNode);
        var arrowIndentation = baseIndentation + "    "; // Add one level of indentation

        // Get the line ending trivia
        var endOfLine = await GetEndOfLineTriviaFromDocumentAsync(document, root, cancellationToken).ConfigureAwait(false);

        SyntaxNode? newNode = declarationNode switch
        {
            MethodDeclarationSyntax method => MoveMethodArrowToNewLine(method, arrowIndentation, endOfLine),
            PropertyDeclarationSyntax property => MovePropertyArrowToNewLine(property, arrowIndentation, endOfLine),
            AccessorDeclarationSyntax accessor => MoveAccessorArrowToNewLine(accessor, arrowIndentation, endOfLine),
            _ => null,
        };

        if (newNode is null)
        {
            return document;
        }

        var newRoot = root.ReplaceNode(declarationNode, newNode);
        return document.WithSyntaxRoot(newRoot);
    }

    private static MethodDeclarationSyntax? MoveMethodArrowToNewLine(
        MethodDeclarationSyntax method,
        string arrowIndentation,
        SyntaxTrivia newLineTrivia)
    {
        if (method.ExpressionBody is null)
        {
            return null;
        }

        var indentationTrivia = SyntaxFactory.Whitespace(arrowIndentation);

        var newArrowExpression = method.ExpressionBody
            .WithArrowToken(method.ExpressionBody.ArrowToken
                .WithLeadingTrivia(newLineTrivia, indentationTrivia)
                .WithTrailingTrivia(SyntaxFactory.Space))
            .WithExpression(method.ExpressionBody.Expression.WithoutLeadingTrivia());

        // Remove trailing trivia from parameter list (the space before the arrow)
        var parameterList = method.ParameterList.WithoutTrailingTrivia();

        return method
            .WithParameterList(parameterList)
            .WithExpressionBody(newArrowExpression);
    }

    private static PropertyDeclarationSyntax? MovePropertyArrowToNewLine(
        PropertyDeclarationSyntax property,
        string arrowIndentation,
        SyntaxTrivia newLineTrivia)
    {
        if (property.ExpressionBody is null)
        {
            return null;
        }

        var indentationTrivia = SyntaxFactory.Whitespace(arrowIndentation);

        var newArrowExpression = property.ExpressionBody
            .WithArrowToken(property.ExpressionBody.ArrowToken
                .WithLeadingTrivia(newLineTrivia, indentationTrivia)
                .WithTrailingTrivia(SyntaxFactory.Space))
            .WithExpression(property.ExpressionBody.Expression.WithoutLeadingTrivia());

        // Remove trailing trivia from identifier (the space before the arrow)
        var identifier = property.Identifier.WithTrailingTrivia(SyntaxFactory.TriviaList());

        return property
            .WithIdentifier(identifier)
            .WithExpressionBody(newArrowExpression);
    }

    private static AccessorDeclarationSyntax? MoveAccessorArrowToNewLine(
        AccessorDeclarationSyntax accessor,
        string arrowIndentation,
        SyntaxTrivia newLineTrivia)
    {
        if (accessor.ExpressionBody is null)
        {
            return null;
        }

        var indentationTrivia = SyntaxFactory.Whitespace(arrowIndentation);

        var newArrowExpression = accessor.ExpressionBody
            .WithArrowToken(accessor.ExpressionBody.ArrowToken
                .WithLeadingTrivia(newLineTrivia, indentationTrivia)
                .WithTrailingTrivia(SyntaxFactory.Space))
            .WithExpression(accessor.ExpressionBody.Expression.WithoutLeadingTrivia());

        // Remove trailing trivia from keyword (the space before the arrow)
        var keyword = accessor.Keyword.WithTrailingTrivia(SyntaxFactory.TriviaList());

        return accessor
            .WithKeyword(keyword)
            .WithExpressionBody(newArrowExpression);
    }

    private static string GetBaseIndentation(SyntaxNode node)
    {
        // Find the member declaration that contains this node
        var memberNode = node.AncestorsAndSelf().FirstOrDefault(n =>
            n is MemberDeclarationSyntax or AccessorDeclarationSyntax);

        if (memberNode is null)
        {
            return string.Empty;
        }

        var leadingTrivia = memberNode.GetLeadingTrivia();
        var lastWhitespace = leadingTrivia
            .Reverse()
            .FirstOrDefault(t => t.IsKind(SyntaxKind.WhitespaceTrivia));

        if (lastWhitespace != default)
        {
            return lastWhitespace.ToString();
        }

        // If no whitespace found in leading trivia, calculate from the tree
        var sourceText = node.SyntaxTree.GetText();
        var lineSpan = memberNode.GetLocation().GetLineSpan();
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
                whitespaceCount += Constants.TabSpaces;
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