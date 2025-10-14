using Common.Domain;

namespace Authors.Domain.ValueObjects;

/// <summary>
/// Address value object
/// </summary>
public class Address : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }

    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public static Address Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));

        if (!string.IsNullOrWhiteSpace(state) && state.Length != 2)
            throw new ArgumentException("State must be 2 characters", nameof(state));

        if (!string.IsNullOrWhiteSpace(zipCode) && zipCode.Length != 5)
            throw new ArgumentException("ZIP code must be 5 characters", nameof(zipCode));

        return new Address(street, city, state, zipCode);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
    }

    public override string ToString() => $"{Street}, {City}, {State} {ZipCode}";
}
