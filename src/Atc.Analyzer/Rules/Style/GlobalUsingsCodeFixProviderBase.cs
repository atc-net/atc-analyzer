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

        var newRoot = root.RemoveNode(usingDirective, SyntaxRemoveOptions.KeepNoTrivia);
        if (newRoot is null)
        {
            return solution;
        }

        var updatedDocument = document.WithSyntaxRoot(newRoot);
        solution = updatedDocument.Project.Solution;

        // Step 2: Find or create GlobalUsings.cs
        var globalUsingsDocument = FindGlobalUsingsDocument(document.Project);
        if (globalUsingsDocument is null)
        {
            // Create new GlobalUsings.cs
            var newGlobalUsingsDocument = await CreateGlobalUsingsDocumentAsync(
                document.Project,
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
            .NormalizeWhitespace();

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

        // Create new global using directive
        var globalUsingDirective = SyntaxFactory.UsingDirective(usingDirective.Name!)
            .WithGlobalKeyword(SyntaxFactory.Token(SyntaxKind.GlobalKeyword).WithTrailingTrivia(SyntaxFactory.Space))
            .NormalizeWhitespace();

        // Find the correct position to insert (alphabetically within namespace groups)
        var usings = compilationUnit.Usings.ToList();
        var insertIndex = 0;
        if (namespaceName is not null)
        {
            insertIndex = FindInsertPosition(usings, namespaceName);
        }

        usings.Insert(insertIndex, globalUsingDirective);

        var newCompilationUnit = compilationUnit.WithUsings(SyntaxFactory.List(usings));
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
