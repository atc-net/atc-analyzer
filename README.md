# Atc.Analyzer

A Roslyn analyzer to enforce some good practices in C# in terms of design, usage, security, performance, and style.

## Installation

```bash
dotnet add package Atc.Analyzer
```

## Usage

Once installed, the analyzer will automatically run during compilation and highlight code issues in your IDE. Code fixes are available for many rules and can be applied directly from your IDE.

## Rules

| Id | Category | Description | Severity | Is enabled | Code fix |
| --- | --- | --- | --- | --- | --- |
| [ATC201](docs/rules/ATC201.md) | Style | Single parameter should be kept inline when declaration is short | ⚠️ Warning | ✔️ Yes | ✅ Yes |
| [ATC202](docs/rules/ATC202.md) | Style | Multi parameters should be separated on individual lines | ⚠️ Warning | ✔️ Yes | ✅ Yes |
| [ATC203](docs/rules/ATC203.md) | Style | Method chains with 2 or more calls should be placed on separate lines | ⚠️ Warning | ✔️ Yes | ✅ Yes |
| [ATC210](docs/rules/ATC210.md) | Style | Use expression body syntax when appropriate | ⚠️ Warning | ✔️ Yes | ✅ Yes |
| [ATC220](docs/rules/ATC220.md) | Style | Use global usings for all namespaces (strict policy) | ⚠️ Warning | ✔️ Yes | ✅ Yes |
| [ATC221](docs/rules/ATC221.md) | Style | Use global usings for common namespaces (lenient policy) | ⚠️ Warning | ✔️ Yes | ✅ Yes |

### Severity Levels

- ❌ Error - Blocks compilation
- ⚠️ Warning - Produces a warning during compilation
- ℹ️ Info - Informational message
- 👻 Hidden - Not shown in IDE by default

## Categories

### Style

Rules that enforce consistent code formatting and style conventions.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.
