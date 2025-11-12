namespace Atc.Analyzer.Tests.Rules.Style;

using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.Text;

#pragma warning disable CA1305, CA2000, S1481

/// <summary>
/// Standalone test that doesn't use Microsoft.CodeAnalysis.Testing framework
/// to isolate the issue.
/// </summary>
public sealed class StandaloneCodeFixTest
{
    [Fact]
    public async Task ApplyCodeFixManually()
    {
        const string source = """
                              public class Sample
                              {
                                  public void Process(int id, string name)
                                  {
                                  }
                              }
                              """;

        // Create a workspace and document
        var workspace = new AdhocWorkspace();
        var projectInfo = ProjectInfo.Create(
            ProjectId.CreateNewId(),
            VersionStamp.Default,
            "TestProject",
            "TestProject",
            LanguageNames.CSharp);

        var project = workspace.AddProject(projectInfo);
        var document = workspace.AddDocument(project.Id, "Test.cs", SourceText.From(source));

        // Run the analyzer to get diagnostics
        var compilation = await document.Project.GetCompilationAsync();
        var syntaxTree = await document.GetSyntaxTreeAsync();

        if (compilation == null || syntaxTree == null)
        {
            Assert.Fail("Failed to get compilation or syntax tree");
            return;
        }

        var analyzer = new ParameterSeparationAnalyzer();
        var compilationWithAnalyzers = compilation.WithAnalyzers([analyzer]);
        var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();

        var relevantDiagnostics = diagnostics
            .Where(d => d.Id == "ATC202")
            .ToList();

        Assert.Single(relevantDiagnostics);

        var diagnostic = relevantDiagnostics[0];

        // Now apply the code fix
        var root = await document.GetSyntaxRootAsync();
        if (root == null)
        {
            Assert.Fail("Failed to get root");
            return;
        }

        var node = root.FindNode(diagnostic.Location.SourceSpan);

        // The diagnostic points to the parameter list, so find the parent method
        var method = node.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();

        if (method == null)
        {
            Assert.Fail($"Could not find method declaration. Node type: {node.GetType().Name}");
            return;
        }

        var codeFixProvider = new ParameterSeparationCodeFixProvider();

        // Create a code fix context
        var codeFixContext = new CodeFixContext(
            document,
            diagnostic,
            (action, diags) => { },  // We'll handle the action ourselves
            CancellationToken.None);

        // Get the code fixes
        var codeFixes = new List<CodeAction>();
        await codeFixProvider.RegisterCodeFixesAsync(new CodeFixContext(
            document,
            diagnostic,
            (action, diags) => codeFixes.Add(action),
            CancellationToken.None));

        if (codeFixes.Count == 0)
        {
            Assert.Fail($"No code fixes registered. Diagnostic: {diagnostic}, Location: {diagnostic.Location}, Node type: {node.GetType().Name}");
            return;
        }

        Assert.Single(codeFixes);

        var codeAction = codeFixes[0];

        // Apply the code action
        var operations = await codeAction.GetOperationsAsync(CancellationToken.None);
        var applyChangesOperation = operations.OfType<ApplyChangesOperation>().FirstOrDefault();

        Assert.NotNull(applyChangesOperation);

        // Apply the changes
        applyChangesOperation.Apply(workspace, CancellationToken.None);

        // Get the modified document
        var modifiedDocument = workspace.CurrentSolution.GetDocument(document.Id);
        Assert.NotNull(modifiedDocument);

        var modifiedRoot = await modifiedDocument.GetSyntaxRootAsync();
        Assert.NotNull(modifiedRoot);

        var modifiedMethod = modifiedRoot.DescendantNodes().OfType<MethodDeclarationSyntax>().First();
        var modifiedParams = modifiedMethod.ParameterList;

        // Check if parameters are on separate lines
        var modifiedTree = modifiedRoot.SyntaxTree;
        var param0Line = modifiedTree.GetLineSpan(modifiedParams.Parameters[0].Span).StartLinePosition.Line;
        var param1Line = modifiedTree.GetLineSpan(modifiedParams.Parameters[1].Span).StartLinePosition.Line;

        // Now run the analyzer again on the modified code
        var modifiedCompilation = await modifiedDocument.Project.GetCompilationAsync();
        if (modifiedCompilation != null)
        {
            var modifiedCompilationWithAnalyzers = modifiedCompilation.WithAnalyzers([analyzer]);
            var modifiedDiagnostics = await modifiedCompilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();

            var remainingDiagnostics = modifiedDiagnostics
                .Where(d => d.Id == "ATC202")
                .ToList();

            // Verify that the code fix successfully resolved the diagnostic
            Assert.Empty(remainingDiagnostics);

            // Verify parameters are on separate lines
            Assert.NotEqual(param0Line, param1Line);
        }
    }
}