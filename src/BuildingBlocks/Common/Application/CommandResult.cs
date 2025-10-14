namespace Common.Application;

/// <summary>
/// Result of a command execution
/// </summary>
public interface ICommandResult
{
    bool Success { get; }
    string? ErrorMessage { get; }
}

/// <summary>
/// Generic command result with data
/// </summary>
public class CommandResult : ICommandResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public static CommandResult Successful() 
        => new() { Success = true };

    public static CommandResult Failed(string errorMessage) 
        => new() { Success = false, ErrorMessage = errorMessage };
}

/// <summary>
/// Command result with typed data
/// </summary>
public class CommandResult<T> : CommandResult
{
    public T? Data { get; set; }

    public static CommandResult<T> Successful(T data) 
        => new() { Success = true, Data = data };

    public new static CommandResult<T> Failed(string errorMessage) 
        => new() { Success = false, ErrorMessage = errorMessage };
}
