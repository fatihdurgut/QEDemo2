namespace EventBus.Events;

/// <summary>
/// Base class for integration events that cross service boundaries
/// </summary>
public abstract record IntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime CreatedDate { get; init; }

    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
    }

    protected IntegrationEvent(Guid id, DateTime createdDate)
    {
        Id = id;
        CreatedDate = createdDate;
    }
}

