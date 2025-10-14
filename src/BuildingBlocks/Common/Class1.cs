namespace Common.Domain;

/// <summary>
/// Base class for all entities with identity
/// </summary>
public abstract class Entity
{
    private List<IDomainEvent>? _domainEvents;
    
    /// <summary>
    /// Domain events that occurred during entity lifecycle
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly() ?? new List<IDomainEvent>().AsReadOnly();

    /// <summary>
    /// Adds a domain event to be published
    /// </summary>
    protected void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(eventItem);
    }

    /// <summary>
    /// Removes a domain event
    /// </summary>
    protected void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    /// <summary>
    /// Clears all domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}
