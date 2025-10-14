using Common.Domain;

namespace Authors.Domain.Events;

/// <summary>
/// Domain event raised when an author is created
/// </summary>
public class AuthorCreatedDomainEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public string AuthorId { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public AuthorCreatedDomainEvent(string authorId, string firstName, string lastName)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        AuthorId = authorId;
        FirstName = firstName;
        LastName = lastName;
    }
}
