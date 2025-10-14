namespace Common.Domain;

/// <summary>
/// Generic repository interface following DDD patterns
/// </summary>
public interface IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    /// Unit of work for managing transactions
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}
