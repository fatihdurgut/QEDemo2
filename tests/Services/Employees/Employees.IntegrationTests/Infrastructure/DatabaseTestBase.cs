using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Employees.IntegrationTests.Infrastructure;

/// <summary>
/// Base class for integration tests that require a PostgreSQL database
/// Uses Testcontainers to spin up a real PostgreSQL instance for testing
/// </summary>
public abstract class DatabaseTestBase : IAsyncLifetime
{
    protected IContainer? PostgresContainer { get; private set; }
    protected string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        // Create and start PostgreSQL container
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

        // Build connection string
        var host = PostgresContainer.Hostname;
        var port = PostgresContainer.GetMappedPublicPort(5432);
        ConnectionString = $"Host={host};Port={port};Database=testdb;Username=testuser;Password=testpass";

        // Additional setup can be done here (e.g., run migrations)
        await OnInitializeAsync();
    }

    /// <summary>
    /// Override this method to perform additional setup after the container starts
    /// </summary>
    protected virtual Task OnInitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (PostgresContainer is not null)
        {
            await PostgresContainer.DisposeAsync();
        }
    }
}
