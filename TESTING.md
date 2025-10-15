# Testing Infrastructure Setup Guide

## Quick Start

### Running Backend Tests

```bash
# From repository root
cd tests

# Run all tests with coverage
./run-all-tests.sh

# Run only unit tests
dotnet test --filter "FullyQualifiedName~UnitTests"

# Run only integration tests
dotnet test --filter "FullyQualifiedName~IntegrationTests"

# Run tests for specific service
dotnet test tests/Services/Employees/Employees.UnitTests
```

### Running Frontend Tests

```bash
# From repository root
cd src/WebApps/PubsWebApp

# Run tests once with coverage
npm run test:coverage

# Run tests in watch mode for development
npm run test:watch

# Run tests for CI (headless)
npm run test:ci
```

## Test Project Structure

### Employees Service (Reference Implementation)

```
tests/Services/Employees/
├── Employees.UnitTests/
│   ├── Domain/
│   │   └── Entities/
│   │       └── EmployeeTests.cs         # Domain entity tests
│   └── Employees.UnitTests.csproj
└── Employees.IntegrationTests/
    └── Employees.IntegrationTests.csproj
```

## Coverage Reports

After running tests with coverage, reports are generated in:
- **Unit Tests**: `tests/Services/[Service]/[Service].UnitTests/TestResults/`
- **Integration Tests**: `tests/Services/[Service]/[Service].IntegrationTests/TestResults/`

### Coverage Thresholds

The following thresholds are enforced:
- **Domain Layer**: ≥ 85% line coverage
- **Application Layer**: ≥ 85% line coverage
- **Overall**: ≥ 85% line coverage

Tests will fail if coverage drops below these thresholds.

## Test Naming Convention

All tests follow the pattern: `MethodName_Condition_ExpectedResult()`

Examples:
- `Constructor_ValidParameters_CreatesEmployeeSuccessfully()`
- `Constructor_EmptyEmployeeId_ThrowsArgumentException()`
- `GetFullName_NoMiddleInitial_ReturnsFirstAndLastName()`

## Writing New Tests

### 1. Unit Test Example

```csharp
[Fact(DisplayName = "Descriptive test scenario")]
public void MethodName_Condition_ExpectedResult()
{
    // Arrange - Setup test data and mocks
    var employee = new Employee("EMP001", "John", "Doe", null, 
        DateTime.UtcNow.AddYears(-1), "Developer");

    // Act - Execute the method being tested
    var fullName = employee.GetFullName();

    // Assert - Verify the expected outcome using FluentAssertions
    fullName.Should().Be("John Doe");
}
```

### 2. Integration Test Example

```csharp
public class EmployeeApiTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetEmployees_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/employees");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        await _factory.DisposeAsync();
    }
}
```

## CI/CD Integration

Tests are automatically run on:
- Every push to `main` or `develop` branches
- Every pull request to `main` or `develop` branches
- Manual workflow dispatch

See `.github/workflows/testing.yml` for the complete CI/CD pipeline.

## Code Quality Gates

### Automated Checks
- ✅ All tests must pass
- ✅ Code coverage ≥ 85%
- ✅ No critical security vulnerabilities
- ✅ SonarQube quality gate passed
- ✅ No linting errors

### Pull Request Requirements
- All automated checks must pass
- Code review approval required
- All conversations resolved

## Troubleshooting

### Tests fail locally but pass in CI
- Ensure you're using the correct .NET version (8.0)
- Check for timezone-dependent tests
- Verify database state is clean

### Low code coverage
- Add tests for uncovered branches
- Test exception handling paths
- Review edge cases

### Flaky tests
- Avoid using `Thread.Sleep()` - use proper async/await
- Ensure tests are independent (no shared state)
- Use test fixtures for setup/teardown

## Next Steps

1. **Extend Coverage**: Add tests for remaining services (Authors, Publishers, etc.)
2. **Integration Tests**: Implement database and message bus integration tests
3. **E2E Tests**: Set up Cypress for end-to-end testing
4. **Performance Tests**: Add k6 load testing scripts
5. **Security Tests**: Configure OWASP ZAP and Snyk scanning

## Resources

- [Test README](./README.md) - Complete testing documentation
- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions](https://fluentassertions.com/)
- [Testcontainers](https://dotnet.testcontainers.org/)

## Support

For questions or issues with the testing infrastructure:
1. Check the [Testing README](./README.md)
2. Review existing test examples in `tests/Services/Employees/`
3. Open an issue with the `testing` label
