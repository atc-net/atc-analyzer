namespace Atc.Analyzer.Rules.Style;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InterpolationMethodChainCodeFixProvider))]
[Shared]
public sealed class InterpolationMethodChainCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds
        => [RuleIdentifierConstants.Style.InterpolationMethodChain];

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

        // Find the invocation expression at the diagnostic location
        var node = root.FindNode(diagnosticSpan);
        if (node is not InvocationExpressionSyntax invocation)
        {
            // Try to find the invocation in descendants
            invocation = node.DescendantNodesAndSelf()
                .OfType<InvocationExpressionSyntax>()
                .FirstOrDefault();

            if (invocation is null)
            {
                return;
            }
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Extract to local variable",
                createChangedDocument: c => ExtractToVariableAsync(context.Document, invocation, c),
                equivalenceKey: nameof(InterpolationMethodChainCodeFixProvider)),
            context.Diagnostics);
    }

    private static async Task<Document> ExtractToVariableAsync(
        Document document,
        InvocationExpressionSyntax invocation,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        // Find the containing statement where we'll insert the variable declaration
        var containingStatement = invocation.Ancestors()
            .OfType<StatementSyntax>()
            .FirstOrDefault();

        if (containingStatement is null)
        {
            return document;
        }

        // Get semantic model to generate unique variable name
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        // Generate a unique variable name
        var variableName = GenerateUniqueVariableName(invocation, containingStatement, semanticModel);

        // Detect line ending from document
        var endOfLine = DetectEndOfLine(root);

        // Get the indentation of the containing statement
        var indentation = GetIndentation(containingStatement);

        // Create the variable declaration: var variableName = <expression>;
        var variableDeclaration = SyntaxFactory.LocalDeclarationStatement(
            SyntaxFactory.VariableDeclaration(
                SyntaxFactory.IdentifierName("var"))
            .WithVariables(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier(variableName))
                    .WithInitializer(
                        SyntaxFactory.EqualsValueClause(invocation.WithoutTrivia())))))
            .WithLeadingTrivia(SyntaxFactory.Whitespace(indentation))
            .WithTrailingTrivia(endOfLine);

        // Create the identifier to replace the invocation
        var identifierReplacement = SyntaxFactory.IdentifierName(variableName)
            .WithTriviaFrom(invocation);

        // Build new root with both changes
        var editor = new SyntaxEditor(root, document.Project.Solution.Workspace.Services);

        // Insert the variable declaration before the containing statement
        editor.InsertBefore(containingStatement, variableDeclaration);

        // Replace the invocation with the variable identifier
        editor.ReplaceNode(invocation, identifierReplacement);

        var newRoot = editor.GetChangedRoot();
        return document.WithSyntaxRoot(newRoot);
    }

    private static string GenerateUniqueVariableName(
        InvocationExpressionSyntax invocation,
        StatementSyntax containingStatement,
        SemanticModel? semanticModel)
    {
        // Try to derive a meaningful name from the expression
        var baseName = DeriveVariableName(invocation);

        if (semanticModel is null)
        {
            return baseName;
        }

        // Get all existing identifiers in scope to avoid conflicts
        var identifierNames = containingStatement.Parent?
            .DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Select(i => i.Identifier.Text) ?? [];

        var existingNames = new HashSet<string>(identifierNames, StringComparer.Ordinal);

        // Also check local declarations
        var localDeclarations = containingStatement.Parent?
            .DescendantNodes()
            .OfType<VariableDeclaratorSyntax>()
            .Select(v => v.Identifier.Text) ?? [];

        foreach (var name in localDeclarations)
        {
            existingNames.Add(name);
        }

        // Make sure the name is unique
        var uniqueName = baseName;
        var counter = 1;
        while (existingNames.Contains(uniqueName))
        {
            uniqueName = $"{baseName}{counter}";
            counter++;
        }

        return uniqueName;
    }

    private static string DeriveVariableName(InvocationExpressionSyntax invocation)
    {
        // Try to get a meaningful name from the last method in the chain
        if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            var methodName = memberAccess.Name.Identifier.Text;

            // Common transformations for readable names
            return methodName switch
            {
                "ToString" => "stringValue",
                "ToLower" or "ToLowerInvariant" => "lowerCaseValue",
                "ToUpper" or "ToUpperInvariant" => "upperCaseValue",
                "Trim" or "TrimStart" or "TrimEnd" => "trimmedValue",
                "Replace" => "replacedValue",
                "Substring" => "substringValue",
                "Format" => "formattedValue",
                "First" or "FirstOrDefault" => "firstItem",
                "Last" or "LastOrDefault" => "lastItem",
                "Single" or "SingleOrDefault" => "singleItem",
                "Count" => "itemCount",
                "Sum" => "totalSum",
                "Average" => "averageValue",
                "Max" => "maxValue",
                "Min" => "minValue",
                _ => "formattedValue",
            };
        }

        return "formattedValue";
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
                whitespaceCount += 4; // Treat tabs as 4 spaces
            }
            else
            {
                break;
            }
        }

        return new string(' ', whitespaceCount);
    }

    private static SyntaxTrivia DetectEndOfLine(SyntaxNode root)
    {
        var sourceText = root.SyntaxTree.GetText();
        foreach (var line in sourceText.Lines)
        {
            if (line.EndIncludingLineBreak > line.End)
            {
                var lineBreakText = sourceText.ToString(
                    new Microsoft.CodeAnalysis.Text.TextSpan(
                        line.End,
                        line.EndIncludingLineBreak - line.End));
                return lineBreakText == "\r\n"
                    ? SyntaxFactory.CarriageReturnLineFeed
                    : SyntaxFactory.LineFeed;
            }
        }

        return SyntaxFactory.LineFeed;
    }
}