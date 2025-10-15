# Adding Tests to a New Service

This guide explains how to add unit and integration tests to a new microservice following the established patterns.

## Prerequisites

- .NET 8 SDK installed
- Docker Desktop running (for integration tests)
- Understanding of xUnit, Moq, and FluentAssertions

## Step 1: Create Test Projects

### Unit Test Project

```bash
# From repository root
cd tests/Services
mkdir [ServiceName]
cd [ServiceName]

# Create unit test project
dotnet new xunit -n [ServiceName].UnitTests -o [ServiceName].UnitTests

# Add to solution
cd ../../../
dotnet sln add tests/Services/[ServiceName]/[ServiceName].UnitTests/[ServiceName].UnitTests.csproj
```

### Integration Test Project

```bash
# From tests/Services/[ServiceName]
dotnet new xunit -n [ServiceName].IntegrationTests -o [ServiceName].IntegrationTests

# Add to solution
cd ../../../
dotnet sln add tests/Services/[ServiceName]/[ServiceName].IntegrationTests/[ServiceName].IntegrationTests.csproj
```

## Step 2: Configure Unit Test Project

Edit `[ServiceName].UnitTests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <!-- Additional packages beyond Directory.Build.props -->
    <PackageReference Include="Moq" Version="4.20.70" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\..\..\src\Services\[ServiceName]\[ServiceName].Domain\[ServiceName].Domain.csproj" />
    <ProjectReference Include="..\..\..\..\src\Services\[ServiceName]\[ServiceName].Application\[ServiceName].Application.csproj" />
  </ItemGroup>

</Project>
```

## Step 3: Configure Integration Test Project

Edit `[ServiceName].IntegrationTests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <!-- Additional packages beyond Directory.Build.props -->
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.0" />
    <PackageReference Include="Npgsql" Version="9.0.4" />
    <PackageReference Include="Testcontainers" Version="3.10.0" />
    <PackageReference Include="Testcontainers.PostgreSql" Version="3.10.0" />
    <PackageReference Include="Testcontainers.RabbitMq" Version="3.10.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\..\..\src\Services\[ServiceName]\[ServiceName].API\[ServiceName].API.csproj" />
    <ProjectReference Include="..\..\..\..\src\Services\[ServiceName]\[ServiceName].Infrastructure\[ServiceName].Infrastructure.csproj" />
  </ItemGroup>

</Project>
```

## Step 4: Create Test Directory Structure

```bash
cd tests/Services/[ServiceName]/[ServiceName].UnitTests

# Create unit test directories
mkdir -p Domain/Entities
mkdir -p Domain/Services
mkdir -p Application/Commands
mkdir -p Application/Queries

cd ../[ServiceName].IntegrationTests

# Create integration test directories
mkdir -p API
mkdir -p Database
mkdir -p Infrastructure
```

## Step 5: Write Your First Unit Test

Create `Domain/Entities/[Entity]Tests.cs`:

```csharp
using [ServiceName].Domain.Entities;

namespace [ServiceName].UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for [Entity] domain entity
/// </summary>
public class [Entity]Tests
{
    [Fact(DisplayName = "Constructor creates valid entity with all properties")]
    public void Constructor_ValidParameters_CreatesEntitySuccessfully()
    {
        // Arrange
        var id = "TEST001";
        var name = "Test Name";

        // Act
        var entity = new [Entity](id, name);

        // Assert
        entity.Id.Should().Be(id);
        entity.Name.Should().Be(name);
    }

    [Fact(DisplayName = "Constructor throws when id is empty")]
    public void Constructor_EmptyId_ThrowsArgumentException()
    {
        // Arrange
        var id = "";
        var name = "Test Name";

        // Act
        var act = () => new [Entity](id, name);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*ID cannot be empty*")
            .And.ParamName.Should().Be("id");
    }
}
```

## Step 6: Write Your First Integration Test

Create `Infrastructure/DatabaseTestBase.cs` (copy from Employees service):

```csharp
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace [ServiceName].IntegrationTests.Infrastructure;

public abstract class DatabaseTestBase : IAsyncLifetime
{
    protected IContainer? PostgresContainer { get; private set; }
    protected string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        PostgresContainer = new ContainerBuilder()
            .WithImage("postgres:15-alpine")
            .WithEnvironment("POSTGRES_USER", "testuser")
            .WithEnvironment("POSTGRES_PASSWORD", "testpass")
            .WithEnvironment("POSTGRES_DB", "testdb")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
            .WithCleanUp(true)
            .Build();

        await PostgresContainer.StartAsync();

        var host = PostgresContainer.Hostname;
        var port = PostgresContainer.GetMappedPublicPort(5432);
        ConnectionString = $"Host={host};Port={port};Database=testdb;Username=testuser;Password=testpass";

        await OnInitializeAsync();
    }

    protected virtual Task OnInitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (PostgresContainer is not null)
        {
            await PostgresContainer.DisposeAsync();
        }
    }
}
```

Create `Database/DatabaseConnectionTests.cs`:

```csharp
using [ServiceName].IntegrationTests.Infrastructure;

namespace [ServiceName].IntegrationTests.Database;

public class DatabaseConnectionTests : DatabaseTestBase
{
    [Fact(DisplayName = "Can connect to database and execute query")]
    public async Task CanConnect_ToDatabase_AndExecuteQuery()
    {
        // Arrange
        using var connection = new Npgsql.NpgsqlConnection(ConnectionString);

        // Act
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT 1";
        var result = await command.ExecuteScalarAsync();

        // Assert
        connection.State.Should().Be(System.Data.ConnectionState.Open);
        result.Should().NotBeNull();
        Convert.ToInt32(result).Should().Be(1);
    }
}
```

## Step 7: Run Tests

```bash
# Run unit tests
dotnet test tests/Services/[ServiceName]/[ServiceName].UnitTests

# Run integration tests  
dotnet test tests/Services/[ServiceName]/[ServiceName].IntegrationTests

# Run with coverage
dotnet test tests/Services/[ServiceName]/[ServiceName].UnitTests \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura \
  /p:Threshold=85
```

## Step 8: Verify Coverage

After running tests with coverage:

```bash
# Check the coverage report
cat tests/Services/[ServiceName]/[ServiceName].UnitTests/TestResults/coverage.cobertura.xml
```

Expected output should show:
- **Line Coverage**: ≥ 85%
- **Branch Coverage**: ≥ 80%
- **Method Coverage**: ≥ 85%

## Common Patterns

### Testing Domain Entities

- Test constructor validation
- Test business rule enforcement
- Test state transitions
- Test calculated properties/methods

### Testing Application Services

- Mock repository dependencies
- Test command/query handlers
- Verify events are published
- Test validation logic

### Testing API Controllers

- Use WebApplicationFactory
- Test HTTP response codes
- Test request/response serialization
- Test error handling

### Testing Repositories

- Use Testcontainers
- Test CRUD operations
- Test queries with filters
- Test transactions

## Tips

1. **Keep tests focused**: One test per behavior
2. **Use descriptive names**: Follow `MethodName_Condition_ExpectedResult` pattern
3. **Arrange, Act, Assert**: Structure all tests consistently
4. **Mock external dependencies**: Only test your code
5. **Test edge cases**: Don't just test the happy path
6. **Keep tests fast**: Unit tests should run in milliseconds
7. **Isolate tests**: Tests should not depend on each other

## Reference Implementation

See `tests/Services/Employees/` for complete examples of:
- Unit test project structure
- Integration test setup with Testcontainers
- Domain entity tests
- Database integration tests

## Troubleshooting

### Tests won't build

1. Check that all package references are correct
2. Verify project references point to the right paths
3. Ensure .NET 8 SDK is installed: `dotnet --version`

### Integration tests fail

1. Ensure Docker Desktop is running
2. Check if port 5432 is available
3. Verify Testcontainers has proper permissions
4. Check container logs for errors

### Low coverage

1. Add tests for uncovered code paths
2. Test exception handling branches
3. Test edge cases and validation logic
4. Review coverage report for specific gaps

## Next Steps

1. Add more unit tests to achieve ≥ 85% coverage
2. Implement API integration tests
3. Add repository integration tests
4. Configure CI/CD pipeline for automated testing

For more details, see:
- [Testing Strategy](../README.md)
- [Quick Start Guide](../../../TESTING.md)
- [Employees Service Tests](../Employees/) (reference implementation)
