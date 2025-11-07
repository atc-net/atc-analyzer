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

        // Handle different declaration types
        BaseMethodDeclarationSyntax? methodDeclaration = node as MethodDeclarationSyntax;
        methodDeclaration ??= node as ConstructorDeclarationSyntax;

        if (methodDeclaration is not null)
        {
            RegisterCodeFix(context, methodDeclaration, methodDeclaration.ParameterList);
            return;
        }

        // Handle local functions
        if (node is LocalFunctionStatementSyntax localFunction)
        {
            RegisterCodeFix(context, localFunction, localFunction.ParameterList);
            return;
        }

        // Handle delegates
        if (node is DelegateDeclarationSyntax delegateDeclaration)
        {
            RegisterCodeFix(context, delegateDeclaration, delegateDeclaration.ParameterList);
        }
    }

    private static void RegisterCodeFix(
        CodeFixContext context,
        SyntaxNode declaration,
        ParameterListSyntax parameterList)
    {
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Separate multi parameters onto individual lines",
                createChangedDocument: c => FormatParametersAsync(context.Document, declaration, parameterList, c),
                equivalenceKey: nameof(ParameterSeparationCodeFixProvider)),
            context.Diagnostics);
    }

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

        // Calculate indentation
        var indentation = GetIndentation(declaration);
        var parameterIndentation = indentation + "    ";

        // Create formatted parameters with proper trivia
        var endOfLine = declaration.SyntaxTree.GetEndOfLineTrivia();
        var formattedParameters = default(SeparatedSyntaxList<ParameterSyntax>);

        for (var i = 0; i < parameterList.Parameters.Count; i++)
        {
            var param = parameterList.Parameters[i].WithoutTrivia();

            // Add newline and indentation before each parameter
            var formattedParam = param.WithLeadingTrivia(
                endOfLine,
                SyntaxFactory.Whitespace(parameterIndentation));

            formattedParameters = formattedParameters.Add(formattedParam);
        }

        // Create new parameter list with formatted parameters
        var newParameterList = SyntaxFactory.ParameterList(
            parameterList.OpenParenToken.WithoutTrivia(),
            formattedParameters,
            parameterList.CloseParenToken.WithoutTrivia());

        // Add Formatter annotation to tell Roslyn to format this
        newParameterList = newParameterList.WithAdditionalAnnotations(Formatter.Annotation);

        // Replace the old parameter list with the new one
        SyntaxNode newDeclaration = declaration switch
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
                whitespaceCount += 4; // Treat tabs as 4 spaces
            }
            else
            {
                break;
            }
        }

        return new string(' ', whitespaceCount);
    }
}