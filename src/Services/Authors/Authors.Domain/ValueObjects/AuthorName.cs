using Common.Domain;

namespace Authors.Domain.ValueObjects;

/// <summary>
/// Author name value object
/// </summary>
public class AuthorName : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    private AuthorName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static AuthorName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        if (firstName.Length > 20)
            throw new ArgumentException("First name cannot exceed 20 characters", nameof(firstName));

        if (lastName.Length > 40)
            throw new ArgumentException("Last name cannot exceed 40 characters", nameof(lastName));

        return new AuthorName(firstName, lastName);
    }

    public string FullName => $"{FirstName} {LastName}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString() => FullName;
}
