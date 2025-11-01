# Copilot Instructions

This repository contains the official Protobuf client and server example.

## Critical: Always Build and Test

**ALWAYS build and run tests before declaring any task complete or making a pull request.**

When making code changes:
1. **Build first**: Run `dotnet build` to ensure the code compiles without errors
2. **Run tests**: Execute `dotnet test` to verify all tests pass
3. **Fix issues**: Address any build errors or test failures before proceeding
4. **Verify iteratively**: Build and test frequently during development, not just at the end
5. **Check warnings**: Treat warnings as errors - the build is configured with `TreatWarningsAsErrors=true`

**Never skip these steps.** Even small changes can have unexpected impacts. A passing build and test suite is the minimum bar for any code change.

## C# Coding Standards

### Language Features
- Use **file-scoped namespaces** for all C# files
- Enable **implicit usings** and **nullable reference types**
- Treat warnings as errors

### Code Style
- Follow the conventions in `.editorconfig`
- Use clear, descriptive XML documentation comments for public APIs
- Follow async/await patterns consistently
- Use file-scoped namespaces: `namespace Client;`
- Follow the official C# 13 coding conventions and best practices.
- Use modern C# 13 features such as:
- All generated code must be idiomatic, clear, and maintainable.
- Use XML documentation comments for all public types and members.
- Prefer expressive variable and method names; avoid abbreviations.
- Write unit tests for all new methods using xUnit.
- Avoid obsolete patterns and APIs; use the latest .NET 9 libraries.
- Ensure thread safety and proper use of async/await patterns.
- Use dependency injection for services and data access.
- All code should compile without warnings or errors.
- Include comments where logic is non-trivial.

### Naming Conventions
- Use descriptive names for parameters with `[Description("...")]` attributes

## Architecture Patterns

### Dependency Injection
- Use Microsoft.Extensions.DependencyInjection patterns
- Support both builder patterns and options configuration

### JSON Serialization
- Use `System.Text.Json` exclusively for all JSON operations
- Set `JsonIgnoreCondition.WhenWritingNull` for optional properties to minimize payload size
- Use `JsonSerializerDefaults.Web` for camelCase property naming
- Protocol types are decorated with `[JsonSerializable]` attributes for AOT support
- Custom converters: `CustomizableJsonStringEnumConverter` for flexible enum serialization

### Async Patterns
- All I/O operations should be async
- Use `ValueTask<T>` for hot paths that may complete synchronously
- Always accept `CancellationToken` parameters for async operations
- Name parameters consistently: `cancellationToken`

### MCP Protocol
- Follow the MCP specification at https://spec.modelcontextprotocol.io/ ([specification docs](https://github.com/modelcontextprotocol/modelcontextprotocol/tree/main/docs/specification))
- Use JSON-RPC 2.0 for message transport
- Support all standard MCP capabilities (e.g. tools, prompts, resources, sampling)
- Implement proper error handling with `McpException` and `McpErrorCode`

### Error Handling
- Use standard error codes: `InvalidRequest`, `MethodNotFound`, `InvalidParams`, `InternalError`
- Let domain exceptions bubble up and convert to `InternalError` at transport boundary
- Include detailed error messages in exception `Message` property for debugging

## Testing

### Test Organization
- Unit tests in `tests/` for core functionality

### Test Infrastructure and Helpers
- **XunitLoggerProvider**: Routes `ILogger` output to xUnit's `ITestOutputHelper`
- **KestrelInMemoryTransport** (AspNetCore.Tests): In-memory Kestrel connection for HTTP transport testing without network stack

### Test Best Practices
- Mock external dependencies (filesystem, HTTP clients) rather than calling real services
- Use `CancellationTokenSource` with timeouts to prevent hanging tests
- Dispose resources properly (servers, clients, transports) using `IDisposable` or `await using`
- Run tests with: `dotnet test --filter '(Execution!=Manual)'`

## Build and Development

### Build Commands
- **Restore**: `dotnet restore`
- **Build**: `dotnet build`
- **Test**: `dotnet test`
- **Clean**: `dotnet clean`

### Development Workflow
**Critical**: Always follow this workflow when making changes:
1. Make code changes
2. Build immediately: `dotnet build` - fix any compilation errors
3. Run tests: `dotnet test` - fix any test failures
4. Repeat steps 1-3 iteratively as you develop
5. Only after successful build and tests should you consider the change complete

Do not skip or defer building and testing. These are mandatory steps for every code change, no matter how small.

### SDK Requirements
- The repo currently requires the .NET SDK 9.0 to build and run tests.
- Target frameworks: .NET 9.0, .NET 8.0, .NET Standard 2.0

### Project Structure
- Source code: `src/`
- Tests: `tests/`

## Documentation

- API documentation is generated using DocFX
- Conceptual documentation is in `docs/concepts/`
- Keep README files up to date in package directories
- Use `///` XML comments for all public APIs
- Include `<remarks>` sections for detailed explanations

## Security

- Never commit secrets or API keys
- Use environment variables for sensitive configuration
- Support authentication mechanisms (OAuth, API keys)
- Validate all user inputs
- Follow secure coding practices per SECURITY.md