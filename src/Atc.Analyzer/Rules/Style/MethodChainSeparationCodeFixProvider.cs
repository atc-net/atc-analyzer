namespace Atc.Analyzer.Rules.Style;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MethodChainSeparationCodeFixProvider))]
[Shared]
public sealed class MethodChainSeparationCodeFixProvider : CodeFixProvider
{
    private sealed class MethodChainRewriter : CSharpSyntaxRewriter
    {
        private readonly SyntaxTriviaList indentationTrivia;
        private readonly HashSet<MemberAccessExpressionSyntax> memberAccessesToFormat;

        public MethodChainRewriter(
            string methodIndentation,
            HashSet<MemberAccessExpressionSyntax> memberAccessesToFormat)
        {
            this.memberAccessesToFormat = memberAccessesToFormat;
            var endOfLine = SyntaxFactory.CarriageReturnLineFeed;
            indentationTrivia = SyntaxFactory.TriviaList(endOfLine, SyntaxFactory.Whitespace(methodIndentation));
        }

        public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            // Visit the expression first (to handle nested member accesses)
            var newExpression = (ExpressionSyntax?)Visit(node.Expression);
            if (newExpression is null)
            {
                return node;
            }

            // Only format member accesses that are directly part of the method chain
            if (!memberAccessesToFormat.Contains(node))
            {
                return node.WithExpression(newExpression);
            }

            // Add line break before the dot operator
            return node
                .WithExpression(newExpression)
                .WithOperatorToken(node.OperatorToken
                    .WithLeadingTrivia(indentationTrivia)
                    .WithTrailingTrivia());
        }
    }

    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.Style.MethodChainSeparation];

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

        // Find the innermost node at the diagnostic location
        var node = root.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

        // The diagnostic could be on an InvocationExpression or AwaitExpression
        InvocationExpressionSyntax? invocationExpression = null;
        if (node is InvocationExpressionSyntax invocation)
        {
            invocationExpression = invocation;
        }
        else if (node is AwaitExpressionSyntax awaitExpression &&
                 awaitExpression.Expression is InvocationExpressionSyntax awaitedInvocation)
        {
            invocationExpression = awaitedInvocation;
        }
        else if (node.Parent is InvocationExpressionSyntax parentInvocation)
        {
            // If the node is part of an invocation (e.g., member access), get the invocation
            invocationExpression = parentInvocation;
        }

        if (invocationExpression is null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Break method chain onto separate lines",
                createChangedDocument: c => FormatMethodChainAsync(context.Document, invocationExpression, c),
                equivalenceKey: nameof(MethodChainSeparationCodeFixProvider)),
            context.Diagnostics);
    }

    private static async Task<Document> FormatMethodChainAsync(
        Document document,
        InvocationExpressionSyntax invocationExpression,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        // Get all invocation nodes in the chain
        var invocationNodes = invocationExpression.GetInvocationNodes()
            .Where(inv => inv.Expression is MemberAccessExpressionSyntax || inv.ArgumentList.Arguments.Count == 0)
            .ToList();

        if (invocationNodes.Count < 2)
        {
            return document;
        }

        // Get the base indentation from the statement containing the invocation
        var baseIndentation = GetBaseIndentation(invocationExpression);
        var methodIndentation = baseIndentation + "    "; // Base + 4 spaces for each method

        // Build the formatted method chain
        var newInvocationExpression = FormatMethodChain(invocationNodes, methodIndentation);

        // Replace the original invocation with the formatted one
        var newRoot = root.ReplaceNode(invocationExpression, newInvocationExpression);
        return document.WithSyntaxRoot(newRoot);
    }

    private static InvocationExpressionSyntax FormatMethodChain(
        List<InvocationExpressionSyntax> invocationNodes,
        string methodIndentation)
    {
        // Get the outermost invocation
        var rootInvocation = invocationNodes[0];

        // Collect member accesses that are directly part of the method chain
        var memberAccessesToFormat = new HashSet<MemberAccessExpressionSyntax>();
        foreach (var invocation in invocationNodes)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                memberAccessesToFormat.Add(memberAccess);
            }
        }

        // Use a syntax rewriter to transform the method chain
        var rewriter = new MethodChainRewriter(methodIndentation, memberAccessesToFormat);
        var result = (InvocationExpressionSyntax?)rewriter.Visit(rootInvocation);

        return result ?? rootInvocation;
    }

    private static string GetBaseIndentation(SyntaxNode node)
    {
        // Find the statement that contains this node
        var statement = node.FirstAncestorOrSelf<StatementSyntax>();
        if (statement is null)
        {
            return string.Empty;
        }

        var leadingTrivia = statement.GetLeadingTrivia();
        var lastWhitespace = leadingTrivia
            .Reverse()
            .FirstOrDefault(t => t.IsKind(SyntaxKind.WhitespaceTrivia));

        if (lastWhitespace != default)
        {
            return lastWhitespace.ToString();
        }

        // If no whitespace found in leading trivia, calculate from the tree
        var sourceText = node.SyntaxTree.GetText();
        var lineSpan = statement.GetLocation().GetLineSpan();
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
}