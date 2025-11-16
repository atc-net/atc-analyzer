// ReSharper disable InvertIf
namespace Atc.Analyzer.Rules.Style;

/// <summary>
/// Custom FixAllProvider for GlobalUsings code fixes that ensures proper cleanup
/// of blank lines after batch fixes.
/// </summary>
public sealed class GlobalUsingsFixAllProvider : FixAllProvider
{
    public static readonly GlobalUsingsFixAllProvider Instance = new();

    private GlobalUsingsFixAllProvider()
    {
    }

    public override async Task<CodeAction?> GetFixAsync(FixAllContext fixAllContext)
    {
        // Use the batch fixer as the base implementation
        var batchFixer = WellKnownFixAllProviders.BatchFixer;
        var batchFix = await batchFixer
            .GetFixAsync(fixAllContext)
            .ConfigureAwait(false);

        if (batchFix is null)
        {
            return null;
        }

        // Wrap the batch fix with our cleanup logic
        return CodeAction.Create(
            title: batchFix.Title,
            createChangedSolution: async ct =>
            {
                // Apply the batch fix
                var solution = await batchFix
                    .GetOperationsAsync(ct)
                    .ConfigureAwait(false);
                var operations = solution.ToList();

                // Get the modified solution from the operations
                Solution? modifiedSolution = null;
                foreach (var operation in operations)
                {
                    if (operation is ApplyChangesOperation applyChanges)
                    {
                        modifiedSolution = applyChanges.ChangedSolution;
                        break;
                    }
                }

                if (modifiedSolution is null)
                {
                    return fixAllContext.Solution;
                }

                // Clean up blank lines in all modified documents
                var cleanedSolution = await CleanupModifiedDocumentsAsync(
                    fixAllContext.Solution,
                    modifiedSolution,
                    fixAllContext.CancellationToken).ConfigureAwait(false);

                return cleanedSolution;
            },
            equivalenceKey: batchFix.EquivalenceKey);
    }

    private static async Task<Solution> CleanupModifiedDocumentsAsync(
        Solution originalSolution,
        Solution modifiedSolution,
        CancellationToken cancellationToken)
    {
        var currentSolution = modifiedSolution;

        // Find all documents that were modified
        var modifiedDocumentIds = modifiedSolution.GetChanges(originalSolution)
            .GetProjectChanges()
            .SelectMany(pc => pc.GetChangedDocuments())
            .ToList();

        foreach (var documentId in modifiedDocumentIds)
        {
            var document = currentSolution.GetDocument(documentId);
            if (document is null)
            {
                continue;
            }

            var root = await document
                .GetSyntaxRootAsync(cancellationToken)
                .ConfigureAwait(false);

            if (root is not CompilationUnitSyntax compilationUnit)
            {
                continue;
            }

            // Only clean up documents with no using directives left
            if (compilationUnit.Usings.Any())
            {
                continue;
            }

            var cleanedRoot = CleanupLeadingBlankLines(compilationUnit);
            if (cleanedRoot != compilationUnit)
            {
                currentSolution = document.WithSyntaxRoot(cleanedRoot).Project.Solution;
            }
        }

        return currentSolution;
    }

    private static CompilationUnitSyntax CleanupLeadingBlankLines(CompilationUnitSyntax compilationUnit)
    {
        var needsCleanup = false;
        var result = compilationUnit;

        // Clean up compilation unit leading trivia
        var compLeadingTrivia = compilationUnit.GetLeadingTrivia();
        if (compLeadingTrivia.Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia) ||
                                       t.IsKind(SyntaxKind.WhitespaceTrivia)))
        {
            var cleanedCompTrivia = compLeadingTrivia
                .Where(t => !t.IsKind(SyntaxKind.EndOfLineTrivia) &&
                            !t.IsKind(SyntaxKind.WhitespaceTrivia))
                .ToList();
            result = result.WithLeadingTrivia(cleanedCompTrivia);
            needsCleanup = true;
        }

        // Clean up first member leading trivia
        if (result.Members.Any())
        {
            var firstMember = result.Members.First();
            var leadingTrivia = firstMember.GetLeadingTrivia();

            // Check if there are leading blank lines
            var hasLeadingBlankTrivia = false;
            foreach (var trivia in leadingTrivia)
            {
                if (trivia.IsKind(SyntaxKind.EndOfLineTrivia) ||
                    trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    hasLeadingBlankTrivia = true;
                    break;
                }

                if (!trivia.IsKind(SyntaxKind.EndOfLineTrivia) &&
                    !trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    break;
                }
            }

            if (hasLeadingBlankTrivia)
            {
                var cleanedTrivia = leadingTrivia
                    .SkipWhile(t => t.IsKind(SyntaxKind.EndOfLineTrivia) ||
                                    t.IsKind(SyntaxKind.WhitespaceTrivia))
                    .ToList();

                var newFirstMember = firstMember.WithLeadingTrivia(cleanedTrivia);
                result = result.ReplaceNode(result.Members.First(), newFirstMember);
                needsCleanup = true;
            }
        }

        return needsCleanup ? result : compilationUnit;
    }
}