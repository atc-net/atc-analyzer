// ReSharper disable InvertIf
namespace Atc.Analyzer.Extensions;

[SuppressMessage("", "CA1024:Use properties where appropriate", Justification = "OK - CLang14 - extension")]
[SuppressMessage("", "CA1034:Do not nest type", Justification = "OK - CLang14 - extension")]
[SuppressMessage("", "CA1708:Names of 'Members'", Justification = "OK - CLang14 - extension")]
[SuppressMessage("", "S2325:Make a static method", Justification = "OK - CLang14 - extension")]
[SuppressMessage("", "S3928:The parameter name 'schema'", Justification = "OK - CLang14 - extension")]
[SuppressMessage("", "S3398:The parameter name 'schema'", Justification = "OK - CLang14 - extension")]
public static class InvocationExpressionSyntaxExtensions
{
    extension(InvocationExpressionSyntax invocation)
    {
        public ExpressionSyntax GetMethodChainRoot()
        {
            if (invocation is null)
            {
                throw new ArgumentNullException(nameof(invocation));
            }

            var current = (ExpressionSyntax)invocation;

            while (current.Parent is MemberAccessExpressionSyntax { Parent: InvocationExpressionSyntax parentInvocation })
            {
                current = parentInvocation;
            }

            // Handle await expressions
            if (current.Parent is AwaitExpressionSyntax awaitExpr)
            {
                // Check if the await expression itself is part of a larger chain
                while (awaitExpr.Parent is MemberAccessExpressionSyntax { Parent: InvocationExpressionSyntax parentInvocation })
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

        public List<InvocationExpressionSyntax> GetInvocationNodes()
        {
            if (invocation is null)
            {
                throw new ArgumentNullException(nameof(invocation));
            }

            var invocationNodes = new List<InvocationExpressionSyntax>();
            CollectInvocationNodes(invocation, invocationNodes);
            return invocationNodes;
        }

        public int GetInvocationLine()
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

        public ExpressionSyntax? GetBaseExpression()
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
    }

    extension(List<InvocationExpressionSyntax> invocationNodes)
    {
        public List<int> GetInvocationLines()
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

        public bool IsFluentAssertionsShouldPattern()
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