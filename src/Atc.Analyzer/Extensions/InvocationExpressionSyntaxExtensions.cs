namespace Atc.Analyzer.Extensions;

public static class InvocationExpressionSyntaxExtensions
{
    public static ExpressionSyntax GetMethodChainRoot(
        this InvocationExpressionSyntax invocation)
    {
        if (invocation is null)
        {
            throw new ArgumentNullException(nameof(invocation));
        }

        var current = (ExpressionSyntax)invocation;

        while (current.Parent is MemberAccessExpressionSyntax memberAccess &&
               memberAccess.Parent is InvocationExpressionSyntax parentInvocation)
        {
            current = parentInvocation;
        }

        // Handle await expressions
        if (current.Parent is AwaitExpressionSyntax awaitExpr)
        {
            // Check if the await expression itself is part of a larger chain
            while (awaitExpr.Parent is MemberAccessExpressionSyntax memberAccess &&
                   memberAccess.Parent is InvocationExpressionSyntax parentInvocation)
            {
                current = parentInvocation;
                if (current.Parent is AwaitExpressionSyntax nextAwait)
                {
                    awaitExpr = nextAwait;
                }
                else
                {
                    break;
                }
            }
        }

        return current;
    }

    public static List<InvocationExpressionSyntax> GetInvocationNodes(
        this InvocationExpressionSyntax invocation)
    {
        if (invocation is null)
        {
            throw new ArgumentNullException(nameof(invocation));
        }

        var invocationNodes = new List<InvocationExpressionSyntax>();
        CollectInvocationNodes(invocation, invocationNodes);
        return invocationNodes;
    }

    public static List<int> GetInvocationLines(
        this List<InvocationExpressionSyntax> invocationNodes)
    {
        if (invocationNodes is null)
        {
            throw new ArgumentNullException(nameof(invocationNodes));
        }

        var invocationLines = new List<int>();

        foreach (var invocation in invocationNodes)
        {
            var line = invocation.GetInvocationLine();
            invocationLines.Add(line);
        }

        return invocationLines;
    }

    public static int GetInvocationLine(
        this InvocationExpressionSyntax invocation)
    {
        if (invocation is null)
        {
            throw new ArgumentNullException(nameof(invocation));
        }

        // If the expression is a MemberAccessExpression, use the dot operator's line
        // Otherwise, use the start of the invocation expression
        if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            return memberAccess.OperatorToken.GetLocation().GetLineSpan().StartLinePosition.Line;
        }

        // For simple invocations like "Method()", use the start of the identifier
        return invocation.Expression.GetLocation().GetLineSpan().StartLinePosition.Line;
    }

    public static ExpressionSyntax? GetBaseExpression(
        this InvocationExpressionSyntax invocation)
    {
        if (invocation is null)
        {
            throw new ArgumentNullException(nameof(invocation));
        }

        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return null;
        }

        var current = memberAccess.Expression;

        // Traverse down to find the base expression (non-invocation, non-member-access)
        while (current is InvocationExpressionSyntax nestedInvocation)
        {
            if (nestedInvocation.Expression is MemberAccessExpressionSyntax nestedMemberAccess)
            {
                current = nestedMemberAccess.Expression;
            }
            else
            {
                break;
            }
        }

        // Handle await expressions
        if (current is AwaitExpressionSyntax awaitExpr)
        {
            current = awaitExpr.Expression;
        }

        return current;
    }

    public static bool IsFluentAssertionsShouldPattern(
        this List<InvocationExpressionSyntax> invocationNodes)
    {
        if (invocationNodes is null)
        {
            throw new ArgumentNullException(nameof(invocationNodes));
        }

        // The first method in the chain is the last element in the list
        // Check if it's named "Should"
        var firstInvocation = invocationNodes[invocationNodes.Count - 1];

        if (firstInvocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            return memberAccess.Name.Identifier.Text == "Should";
        }

        return false;
    }

    private static void CollectInvocationNodes(
        ExpressionSyntax expression,
        List<InvocationExpressionSyntax> invocationNodes)
    {
        switch (expression)
        {
            case InvocationExpressionSyntax invocation:
            {
                invocationNodes.Add(invocation);

                if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
                {
                    CollectInvocationNodes(memberAccess.Expression, invocationNodes);
                }

                break;
            }

            case MemberAccessExpressionSyntax memberAccess:
                CollectInvocationNodes(memberAccess.Expression, invocationNodes);
                break;
            case AwaitExpressionSyntax awaitExpression:
                CollectInvocationNodes(awaitExpression.Expression, invocationNodes);
                break;
        }
    }
}