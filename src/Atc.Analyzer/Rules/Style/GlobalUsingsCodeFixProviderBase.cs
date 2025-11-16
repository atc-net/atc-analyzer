// ReSharper disable ConvertIfStatementToReturnStatement
namespace Atc.Analyzer.Rules.Style;

/// <summary>
/// Base class for code fix providers that move using directives to GlobalUsings.cs.
/// </summary>
public abstract class GlobalUsingsCodeFixProviderBase : CodeFixProvider
{
    private const string GlobalUsingsFileName = "GlobalUsings.cs";

    protected abstract string FixableDiagnosticId { get; }

    protected abstract string ProviderName { get; }

    public override ImmutableArray<string> FixableDiagnosticIds
        => [FixableDiagnosticId];

    public override FixAllProvider GetFixAllProvider()
        => GlobalUsingsFixAllProvider.Instance;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var node = root.FindNode(diagnosticSpan);

        if (node is not UsingDirectiveSyntax usingDirective)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Move to GlobalUsings.cs",
                createChangedSolution: c => MoveToGlobalUsingsAsync(context.Document, usingDirective, c),
                equivalenceKey: ProviderName),
            diagnostic);
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "OK.")]
    private static async Task<Solution> MoveToGlobalUsingsAsync(
        Document document,
        UsingDirectiveSyntax usingDirective,
        CancellationToken cancellationToken)
    {
        var solution = document.Project.Solution;

        // Step 1: Remove the using directive from the current document
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return solution;
        }

        // Remove the using directive and clean up any blank lines
        var newRoot = root.RemoveNode(usingDirective, SyntaxRemoveOptions.KeepNoTrivia);
        if (newRoot is null)
        {
            return solution;
        }

        // Clean up leading trivia (remove blank lines at the start of the file)
        if (newRoot is CompilationUnitSyntax compilationUnit)
        {
            var wasModified = false;

            // Get all leading trivia at the compilation unit level
            var leadingTrivia = compilationUnit.GetLeadingTrivia();
            if (leadingTrivia.Any())
            {
                // Remove all leading whitespace and newlines
                var cleanedTrivia = leadingTrivia
                    .SkipWhile(t => t.IsKind(SyntaxKind.EndOfLineTrivia) || t.IsKind(SyntaxKind.WhitespaceTrivia))
                    .ToList();

                if (cleanedTrivia.Count != leadingTrivia.Count)
                {
                    compilationUnit = compilationUnit.WithLeadingTrivia(cleanedTrivia);
                    wasModified = true;
                }
            }

            // Clean up leading trivia on the first member if there are no usings left
            if (!compilationUnit.Usings.Any() && compilationUnit.Members.Any())
            {
                var firstMember = compilationUnit.Members.First();
                var memberLeadingTrivia = firstMember.GetLeadingTrivia();

                // Count consecutive blank lines at the start
                var blankLinesCount = 0;
                foreach (var trivia in memberLeadingTrivia)
                {
                    if (trivia.IsKind(SyntaxKind.EndOfLineTrivia))
                    {
                        blankLinesCount++;
                    }
                    else if (!trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                    {
                        break;
                    }
                }

                // Remove all leading blank lines
                if (blankLinesCount > 0)
                {
                    var cleanedMemberTrivia = memberLeadingTrivia
                        .SkipWhile(t => t.IsKind(SyntaxKind.EndOfLineTrivia) || t.IsKind(SyntaxKind.WhitespaceTrivia))
                        .ToList();

                    var newFirstMember = firstMember.WithLeadingTrivia(cleanedMemberTrivia);
                    compilationUnit = compilationUnit.ReplaceNode(firstMember, newFirstMember);
                    wasModified = true;
                }
            }

            if (wasModified)
            {
                newRoot = compilationUnit;
            }
        }

        // Update the document with the cleaned root
        var updatedDocument = document.WithSyntaxRoot(newRoot);
        solution = updatedDocument.Project.Solution;

        // Step 2: Find or create GlobalUsings.cs
        var updatedProject = solution.GetProject(document.Project.Id);
        if (updatedProject is null)
        {
            return solution;
        }

        var globalUsingsDocument = FindGlobalUsingsDocument(updatedProject);
        if (globalUsingsDocument is null)
        {
            // Create new GlobalUsings.cs
            var newGlobalUsingsDocument = await CreateGlobalUsingsDocumentAsync(
                updatedProject,
                usingDirective,
                cancellationToken).ConfigureAwait(false);

            if (newGlobalUsingsDocument is not null)
            {
                solution = newGlobalUsingsDocument.Project.Solution;
            }
        }
        else
        {
            // Update existing GlobalUsings.cs
            var updatedGlobalUsingsDocument = await AddToGlobalUsingsAsync(
                globalUsingsDocument,
                usingDirective,
                cancellationToken).ConfigureAwait(false);

            solution = updatedGlobalUsingsDocument.Project.Solution;
        }

        // Final cleanup: ensure the source document has no leading blank lines
        // This is especially important for batch fixes where multiple usings are removed
        // Get the updated document from the current solution state
        var updatedProjectFinal = solution.GetProject(document.Project.Id);
        if (updatedProjectFinal is not null)
        {
            var finalDocument = updatedProjectFinal.GetDocument(document.Id);
            if (finalDocument is null)
            {
                return solution;
            }

            var finalRoot = await finalDocument.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (finalRoot is not CompilationUnitSyntax finalCompilationUnit)
            {
                return solution;
            }

            // Only clean up if there are no usings left
            if (!finalCompilationUnit.Usings.Any())
            {
                var needsCleanup = false;
                var newCompilationUnit = finalCompilationUnit;

                // Clean up compilation unit leading trivia
                var compLeadingTrivia = finalCompilationUnit.GetLeadingTrivia();
                if (compLeadingTrivia.Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia) || t.IsKind(SyntaxKind.WhitespaceTrivia)))
                {
                    var cleanedCompTrivia = compLeadingTrivia
                        .Where(t => !t.IsKind(SyntaxKind.EndOfLineTrivia) && !t.IsKind(SyntaxKind.WhitespaceTrivia))
                        .ToList();
                    newCompilationUnit = newCompilationUnit.WithLeadingTrivia(cleanedCompTrivia);
                    needsCleanup = true;
                }

                // Clean up first member leading trivia
                if (newCompilationUnit.Members.Any())
                {
                    var firstMember = newCompilationUnit.Members.First();
                    var leadingTrivia = firstMember.GetLeadingTrivia();

                    // Remove ALL leading EndOfLine and Whitespace trivia
                    var hasLeadingBlankTrivia = false;
                    foreach (var trivia in leadingTrivia)
                    {
                        if (trivia.IsKind(SyntaxKind.EndOfLineTrivia) || trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                        {
                            hasLeadingBlankTrivia = true;
                            break;
                        }

                        if (!trivia.IsKind(SyntaxKind.EndOfLineTrivia) && !trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                        {
                            break;
                        }
                    }

                    if (hasLeadingBlankTrivia)
                    {
                        var cleanedTrivia = leadingTrivia
                            .SkipWhile(t => t.IsKind(SyntaxKind.EndOfLineTrivia) || t.IsKind(SyntaxKind.WhitespaceTrivia))
                            .ToList();

                        var newFirstMember = firstMember.WithLeadingTrivia(cleanedTrivia);
                        newCompilationUnit = newCompilationUnit.ReplaceNode(
                            newCompilationUnit.Members.First(),
                            newFirstMember);
                        needsCleanup = true;
                    }
                }

                if (needsCleanup)
                {
                    solution = finalDocument.WithSyntaxRoot(newCompilationUnit).Project.Solution;
                }
            }
        }

        return solution;
    }

    private static Document? FindGlobalUsingsDocument(Project project)
        => project
            .Documents
            .FirstOrDefault(d =>
                Path
                    .GetFileName(d.FilePath!)
                    .Equals(GlobalUsingsFileName, StringComparison.OrdinalIgnoreCase));

    private static Task<Document?> CreateGlobalUsingsDocumentAsync(
        Project project,
        UsingDirectiveSyntax usingDirective,
        CancellationToken cancellationToken)
    {
        _ = cancellationToken; // Not used in this method but required for async pattern
        var namespaceName = usingDirective.Name?.ToString();
        if (string.IsNullOrEmpty(namespaceName))
        {
            return Task.FromResult<Document?>(null);
        }

        // Create a new GlobalUsings.cs file
        var globalUsingDirective = SyntaxFactory.UsingDirective(usingDirective.Name!)
            .WithGlobalKeyword(SyntaxFactory.Token(SyntaxKind.GlobalKeyword).WithTrailingTrivia(SyntaxFactory.Space))
            .NormalizeWhitespace()
            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

        var compilationUnit = SyntaxFactory.CompilationUnit()
            .WithUsings(SyntaxFactory.SingletonList(globalUsingDirective))
            .NormalizeWhitespace();

        var projectDirectory = Path.GetDirectoryName(project.FilePath);
        if (string.IsNullOrEmpty(projectDirectory))
        {
            return Task.FromResult<Document?>(null);
        }

        var globalUsingsPath = Path.Combine(projectDirectory, GlobalUsingsFileName);
        var newDocument = project.AddDocument(GlobalUsingsFileName, compilationUnit, filePath: globalUsingsPath);

        return Task.FromResult<Document?>(newDocument);
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "OK.")]
    private static async Task<Document> AddToGlobalUsingsAsync(
        Document globalUsingsDocument,
        UsingDirectiveSyntax usingDirective,
        CancellationToken cancellationToken)
    {
        var root = await globalUsingsDocument.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is not CompilationUnitSyntax compilationUnit)
        {
            return globalUsingsDocument;
        }

        var namespaceName = usingDirective.Name?.ToString();
        if (string.IsNullOrEmpty(namespaceName))
        {
            return globalUsingsDocument;
        }

        // Check if this using already exists
        var existingUsing = compilationUnit
            .Usings
            .FirstOrDefault(u => u.Name?.ToString() == namespaceName);

        if (existingUsing is not null)
        {
            // Already exists, no need to add
            return globalUsingsDocument;
        }

        // Determine line ending from existing usings or use default
        var lineEnding = compilationUnit.Usings.Any()
            ? compilationUnit.Usings.First().GetTrailingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.EndOfLineTrivia))
            : SyntaxFactory.CarriageReturnLineFeed;

        if (lineEnding == default)
        {
            lineEnding = SyntaxFactory.CarriageReturnLineFeed;
        }

        // Create new global using directive with proper formatting
        var globalUsingDirective = SyntaxFactory.UsingDirective(usingDirective.Name!)
            .WithGlobalKeyword(SyntaxFactory.Token(SyntaxKind.GlobalKeyword).WithTrailingTrivia(SyntaxFactory.Space))
            .NormalizeWhitespace()
            .WithTrailingTrivia(lineEnding);

        // Find the correct position to insert (alphabetically within namespace groups)
        var usings = compilationUnit.Usings.ToList();
        var insertIndex = 0;
        if (namespaceName is not null)
        {
            insertIndex = FindInsertPosition(usings, namespaceName);
        }

        // Ensure the previous using has a trailing newline if we're inserting after it
        if (insertIndex > 0 && insertIndex <= usings.Count)
        {
            var previousUsing = usings[insertIndex - 1];
            var previousTrailingTrivia = previousUsing.GetTrailingTrivia();
            var hasNewline = previousTrailingTrivia.Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia));

            if (!hasNewline)
            {
                usings[insertIndex - 1] = previousUsing.WithTrailingTrivia(lineEnding);
            }
        }

        usings.Insert(insertIndex, globalUsingDirective);

        // Sort all usings to ensure proper ordering
        var sortedUsings = usings
            .OrderBy(u => GetNamespaceGroup(u.Name?.ToString() ?? string.Empty))
            .ThenBy(u => u.Name?.ToString() ?? string.Empty, StringComparer.Ordinal)
            .Select(u => u.WithTrailingTrivia(lineEnding))
            .ToList();

        // Remove trailing newline from the last using directive to match expected formatting
        if (sortedUsings.Any())
        {
            var lastIndex = sortedUsings.Count - 1;
            sortedUsings[lastIndex] = sortedUsings[lastIndex].WithoutTrailingTrivia();
        }

        var newCompilationUnit = compilationUnit.WithUsings(SyntaxFactory.List(sortedUsings));
        return globalUsingsDocument.WithSyntaxRoot(newCompilationUnit);
    }

    private static int FindInsertPosition(
        List<UsingDirectiveSyntax> existingUsings,
        string namespaceName)
    {
        var namespaceGroup = GetNamespaceGroup(namespaceName);

        for (var i = 0; i < existingUsings.Count; i++)
        {
            var existingNamespace = existingUsings[i].Name?.ToString() ?? string.Empty;
            var existingGroup = GetNamespaceGroup(existingNamespace);

            // If we're in a different group and the existing group comes after ours, insert here
            if (existingGroup > namespaceGroup)
            {
                return i;
            }

            // If we're in the same group, check alphabetical order
            if (existingGroup == namespaceGroup &&
                string.Compare(namespaceName, existingNamespace, StringComparison.Ordinal) < 0)
            {
                return i;
            }
        }

        // Insert at the end
        return existingUsings.Count;
    }

    private static int GetNamespaceGroup(string namespaceName)
    {
        // System namespaces are always first (group 0)
        if (namespaceName.StartsWith("System", StringComparison.Ordinal))
        {
            return 0;
        }

        // All other namespaces come after, sorted alphabetically (group 1)
        return 1;
    }
}