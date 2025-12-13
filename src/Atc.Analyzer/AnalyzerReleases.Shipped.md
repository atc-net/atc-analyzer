; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 1.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
ATC201 | Style | Warning | Single parameter should be formatted correctly
ATC202 | Style | Warning | Multiple parameters should each be on separate lines
ATC203 | Style | Warning | Method chains should be on separate lines
ATC210 | Style | Warning | Use expression body syntax when appropriate
ATC220 | Style | Warning | Use global usings for all namespaces (strict policy)
ATC221 | Style | Warning | Use global usings for common namespaces (lenient policy)
ATC230 | Style | Warning | Require exactly one blank line between consecutive code blocks
ATC301 | Usage | Warning | Remove redundant RegexOptions.Compiled flag from [GeneratedRegex] attribute
