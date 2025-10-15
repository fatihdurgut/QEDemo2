namespace Identity.Domain.ValueObjects;

/// <summary>
/// Value object representing a user's name
/// </summary>
public record UserName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public UserName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    public string FullName => $"{FirstName} {LastName}";
}
