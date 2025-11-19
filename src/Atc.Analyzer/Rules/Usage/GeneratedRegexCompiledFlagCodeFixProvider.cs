namespace Atc.Analyzer.Rules.Usage;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(GeneratedRegexCompiledFlagCodeFixProvider))]
[Shared]
public sealed class GeneratedRegexCompiledFlagCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.Usage.GeneratedRegexCompiledFlag];

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

        // Find the attribute node
        var node = root.FindNode(diagnosticSpan);

        // Find the attribute containing this node
        var attribute = node.FirstAncestorOrSelf<AttributeSyntax>();
        if (attribute is null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Remove redundant RegexOptions.Compiled flag",
                createChangedDocument: c => RemoveCompiledFlagAsync(context.Document, attribute, c),
                equivalenceKey: nameof(GeneratedRegexCompiledFlagCodeFixProvider)),
            context.Diagnostics);
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "OK.")]
    private static async Task<Document> RemoveCompiledFlagAsync(
        Document document,
        AttributeSyntax attribute,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        if (attribute.ArgumentList is null)
        {
            return document;
        }

        // Find the options argument
        AttributeArgumentSyntax? optionsArgument = null;
        var optionsArgumentIndex = -1;

        // Check for named parameter "options"
        for (var i = 0; i < attribute.ArgumentList.Arguments.Count; i++)
        {
            var argument = attribute.ArgumentList.Arguments[i];
            if (argument.NameEquals?.Name.Identifier.Text == "options")
            {
                optionsArgument = argument;
                optionsArgumentIndex = i;
                break;
            }
        }

        // If not found as named parameter, check for second positional parameter
        if (optionsArgument is null && attribute.ArgumentList.Arguments.Count >= 2)
        {
            optionsArgument = attribute.ArgumentList.Arguments[1];
            optionsArgumentIndex = 1;
        }

        if (optionsArgument is null)
        {
            return document;
        }

        // Try to remove just the Compiled flag from the expression
        var newExpression = RemoveCompiledFromExpression(optionsArgument.Expression);

        AttributeSyntax newAttribute;

        if (newExpression is null)
        {
            // The Compiled flag was the only option, remove the entire options argument
            var newArguments = attribute.ArgumentList.Arguments.RemoveAt(optionsArgumentIndex);

            // Clean up separators and whitespace
            var newArgumentList = attribute.ArgumentList.WithArguments(newArguments);

            // If we removed the last argument and there's trailing trivia, clean it up
            if (newArguments.Count > 0 && optionsArgumentIndex == attribute.ArgumentList.Arguments.Count - 1)
            {
                var lastArgIndex = newArguments.Count - 1;
                var lastArg = newArguments[lastArgIndex];
                newArgumentList = newArgumentList.WithArguments(
                    newArguments.Replace(lastArg, lastArg.WithTrailingTrivia()));
            }

            newAttribute = attribute.WithArgumentList(newArgumentList);
        }
        else
        {
            // Replace the options argument with the modified expression
            var newOptionsArgument = optionsArgument.WithExpression(newExpression);
            var newArguments = attribute.ArgumentList.Arguments.Replace(optionsArgument, newOptionsArgument);
            newAttribute = attribute.WithArgumentList(attribute.ArgumentList.WithArguments(newArguments));
        }

        var newRoot = root.ReplaceNode(attribute, newAttribute);
        return document.WithSyntaxRoot(newRoot);
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "OK.")]
    private static ExpressionSyntax? RemoveCompiledFromExpression(ExpressionSyntax expression)
    {
        // If this is just RegexOptions.Compiled by itself, return null to indicate removal
        if (IsCompiledFlag(expression))
        {
            return null;
        }

        // If this is a binary expression with OR operator
        if (expression is BinaryExpressionSyntax binaryExpression && binaryExpression.IsKind(SyntaxKind.BitwiseOrExpression))
        {
            var leftIsCompiled = IsCompiledFlag(binaryExpression.Left);
            var rightIsCompiled = IsCompiledFlag(binaryExpression.Right);

            if (leftIsCompiled)
            {
                // Remove left side and the OR operator, return just the right side
                // Preserve any leading trivia from the binary expression
                return binaryExpression.Right.WithLeadingTrivia(binaryExpression.GetLeadingTrivia());
            }

            if (rightIsCompiled)
            {
                // Remove right side and the OR operator, return just the left side
                // Preserve any trailing trivia from the binary expression
                return binaryExpression.Left.WithTrailingTrivia(binaryExpression.GetTrailingTrivia());
            }

            // Check if Compiled is nested deeper in the left or right side
            var newLeft = RemoveCompiledFromExpression(binaryExpression.Left);
            if (newLeft is not null && newLeft != binaryExpression.Left)
            {
                return binaryExpression.WithLeft(newLeft);
            }

            var newRight = RemoveCompiledFromExpression(binaryExpression.Right);
            if (newRight is not null && newRight != binaryExpression.Right)
            {
                return binaryExpression.WithRight(newRight);
            }

            // If the left side was removed (null), return just the right
            if (newLeft is null)
            {
                return binaryExpression.Right.WithLeadingTrivia(binaryExpression.GetLeadingTrivia());
            }

            // If the right side was removed (null), return just the left
            if (newRight is null)
            {
                return binaryExpression.Left.WithTrailingTrivia(binaryExpression.GetTrailingTrivia());
            }
        }

        // If this is a parenthesized expression
        if (expression is ParenthesizedExpressionSyntax parenthesizedExpression)
        {
            var newInner = RemoveCompiledFromExpression(parenthesizedExpression.Expression);
            if (newInner is null)
            {
                return null;
            }

            if (newInner != parenthesizedExpression.Expression)
            {
                // If the inner expression changed, update it
                // If the inner is no longer a binary expression, we might want to remove the parentheses
                if (newInner is not BinaryExpressionSyntax)
                {
                    return newInner.WithTriviaFrom(parenthesizedExpression);
                }

                return parenthesizedExpression.WithExpression(newInner);
            }
        }

        // If this is a cast expression
        if (expression is CastExpressionSyntax castExpression)
        {
            var newInner = RemoveCompiledFromExpression(castExpression.Expression);
            if (newInner is null)
            {
                return null;
            }

            if (newInner != castExpression.Expression)
            {
                return castExpression.WithExpression(newInner);
            }
        }

        return expression;
    }

    private static bool IsCompiledFlag(ExpressionSyntax expression)
    {
        // Strip parentheses
        while (expression is ParenthesizedExpressionSyntax parenthesized)
        {
            expression = parenthesized.Expression;
        }

        // Check if this is RegexOptions.Compiled
        if (expression is MemberAccessExpressionSyntax memberAccess)
        {
            return memberAccess.Name.Identifier.Text == "Compiled"
                   && memberAccess.Expression is IdentifierNameSyntax { Identifier.Text: "RegexOptions" };
        }

        return false;
    }
}