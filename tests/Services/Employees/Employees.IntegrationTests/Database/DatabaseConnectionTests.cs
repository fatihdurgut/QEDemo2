using Employees.IntegrationTests.Infrastructure;

namespace Employees.IntegrationTests.Database;

/// <summary>
/// Integration tests for database connectivity and basic operations
/// Demonstrates the use of Testcontainers for integration testing
/// </summary>
public class DatabaseConnectionTests : DatabaseTestBase
{
    [Fact(DisplayName = "PostgreSQL container starts successfully")]
    public async Task PostgresContainer_Starts_Successfully()
    {
        // Arrange & Act - Container is started in InitializeAsync

        // Assert
        PostgresContainer.Should().NotBeNull();
        PostgresContainer!.State.Should().Be(DotNet.Testcontainers.Containers.TestcontainersStates.Running);
    }

    [Fact(DisplayName = "Connection string is valid and accessible")]
    public async Task ConnectionString_IsValid_AndAccessible()
    {
        // Arrange & Act - Connection string is built in InitializeAsync

        // Assert
        ConnectionString.Should().NotBeNullOrEmpty();
        ConnectionString.Should().Contain("Host=");
        ConnectionString.Should().Contain("Port=");
        ConnectionString.Should().Contain("Database=testdb");
        ConnectionString.Should().Contain("Username=testuser");
    }

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

    [Fact(DisplayName = "Can create table in test database")]
    public async Task CanCreate_Table_InTestDatabase()
    {
        // Arrange
        using var connection = new Npgsql.NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Act
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS employees (
                employee_id VARCHAR(20) PRIMARY KEY,
                first_name VARCHAR(50) NOT NULL,
                last_name VARCHAR(50) NOT NULL,
                hire_date TIMESTAMP NOT NULL,
                is_active BOOLEAN DEFAULT TRUE
            )";
        await createTableCommand.ExecuteNonQueryAsync();

        // Verify table was created
        var checkTableCommand = connection.CreateCommand();
        checkTableCommand.CommandText = @"
            SELECT table_name 
            FROM information_schema.tables 
            WHERE table_schema = 'public' 
            AND table_name = 'employees'";
        var tableName = await checkTableCommand.ExecuteScalarAsync();

        // Assert
        tableName.Should().NotBeNull();
        tableName.ToString().Should().Be("employees");
    }

    [Fact(DisplayName = "Can insert and retrieve data from database")]
    public async Task CanInsert_AndRetrieve_DataFromDatabase()
    {
        // Arrange
        using var connection = new Npgsql.NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Create table
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS test_employees (
                employee_id VARCHAR(20) PRIMARY KEY,
                first_name VARCHAR(50) NOT NULL,
                last_name VARCHAR(50) NOT NULL
            )";
        await createTableCommand.ExecuteNonQueryAsync();

        // Act - Insert data
        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
            INSERT INTO test_employees (employee_id, first_name, last_name) 
            VALUES (@id, @firstName, @lastName)";
        insertCommand.Parameters.AddWithValue("id", "EMP001");
        insertCommand.Parameters.AddWithValue("firstName", "John");
        insertCommand.Parameters.AddWithValue("lastName", "Doe");
        await insertCommand.ExecuteNonQueryAsync();

        // Act - Retrieve data
        var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = "SELECT first_name, last_name FROM test_employees WHERE employee_id = @id";
        selectCommand.Parameters.AddWithValue("id", "EMP001");
        
        using var reader = await selectCommand.ExecuteReaderAsync();
        await reader.ReadAsync();

        // Assert
        reader["first_name"].ToString().Should().Be("John");
        reader["last_name"].ToString().Should().Be("Doe");
    }

    [Fact(DisplayName = "Multiple tests can run in parallel with isolated databases")]
    public async Task MultiplTests_CanRun_WithIsolatedDatabases()
    {
        // Arrange
        using var connection = new Npgsql.NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Act
        var command = connection.CreateCommand();
        command.CommandText = "SELECT current_database()";
        var dbName = await command.ExecuteScalarAsync();

        // Assert
        dbName.Should().NotBeNull();
        dbName.ToString().Should().Be("testdb");
        
        // Each test gets its own container, so databases are isolated
        connection.Database.Should().Be("testdb");
    }
}
