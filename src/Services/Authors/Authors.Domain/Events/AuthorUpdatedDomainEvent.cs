using Common.Domain;

namespace Authors.Domain.Events;

/// <summary>
/// Domain event raised when author information is updated
/// </summary>
public class AuthorUpdatedDomainEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public string AuthorId { get; }

    public AuthorUpdatedDomainEvent(string authorId)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        AuthorId = authorId;
    }
}
