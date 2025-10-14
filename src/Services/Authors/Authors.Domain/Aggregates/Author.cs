using Common.Domain;
using Authors.Domain.ValueObjects;
using Authors.Domain.Events;

namespace Authors.Domain.Aggregates;

/// <summary>
/// Author aggregate root
/// </summary>
public class Author : Entity, IAggregateRoot
{
    public string Id { get; private set; }
    public AuthorName Name { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Address? Address { get; private set; }
    public bool HasContract { get; private set; }

    private Author()
    {
        Id = string.Empty;
        Name = null!;
        Phone = null!;
    }

    private Author(string id, AuthorName name, PhoneNumber phone, Address? address, bool hasContract)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Address = address;
        HasContract = hasContract;
    }

    /// <summary>
    /// Creates a new author
    /// </summary>
    public static Author Create(string id, string firstName, string lastName, string phone, 
        string? street, string? city, string? state, string? zipCode, bool hasContract)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Author ID cannot be empty", nameof(id));

        if (id.Length > 11)
            throw new ArgumentException("Author ID cannot exceed 11 characters", nameof(id));

        var name = AuthorName.Create(firstName, lastName);
        var phoneNumber = PhoneNumber.Create(phone);
        
        Address? address = null;
        if (!string.IsNullOrWhiteSpace(street) && !string.IsNullOrWhiteSpace(city))
        {
            address = Address.Create(street, city, state ?? string.Empty, zipCode ?? string.Empty);
        }

        var author = new Author(id, name, phoneNumber, address, hasContract);
        author.AddDomainEvent(new AuthorCreatedDomainEvent(id, firstName, lastName));
        
        return author;
    }

    /// <summary>
    /// Updates author information
    /// </summary>
    public void Update(string firstName, string lastName, string phone, 
        string? street, string? city, string? state, string? zipCode, bool hasContract)
    {
        Name = AuthorName.Create(firstName, lastName);
        Phone = PhoneNumber.Create(phone);
        
        Address? address = null;
        if (!string.IsNullOrWhiteSpace(street) && !string.IsNullOrWhiteSpace(city))
        {
            address = Address.Create(street, city, state ?? string.Empty, zipCode ?? string.Empty);
        }
        Address = address;
        HasContract = hasContract;

        AddDomainEvent(new AuthorUpdatedDomainEvent(Id));
    }

    /// <summary>
    /// Signs a contract with the author
    /// </summary>
    public void SignContract()
    {
        if (HasContract)
            throw new InvalidOperationException("Author already has a contract");

        HasContract = true;
        AddDomainEvent(new AuthorUpdatedDomainEvent(Id));
    }

    /// <summary>
    /// Terminates the contract with the author
    /// </summary>
    public void TerminateContract()
    {
        if (!HasContract)
            throw new InvalidOperationException("Author does not have a contract");

        HasContract = false;
        AddDomainEvent(new AuthorUpdatedDomainEvent(Id));
    }
}
