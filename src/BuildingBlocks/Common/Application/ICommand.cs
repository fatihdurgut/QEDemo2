using MediatR;

namespace Common.Application;

/// <summary>
/// Base interface for commands (write operations)
/// </summary>
public interface ICommand : IRequest<ICommandResult>
{
}

/// <summary>
/// Base interface for commands with typed result
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
