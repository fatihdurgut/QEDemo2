using MediatR;

namespace Common.Application;

/// <summary>
/// Base interface for queries (read operations)
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
