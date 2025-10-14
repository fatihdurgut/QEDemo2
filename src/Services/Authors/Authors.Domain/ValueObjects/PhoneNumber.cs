using Common.Domain;

namespace Authors.Domain.ValueObjects;

/// <summary>
/// Phone number value object
/// </summary>
public class PhoneNumber : ValueObject
{
    public string Value { get; private set; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            value = "UNKNOWN";

        if (value.Length > 12)
            throw new ArgumentException("Phone number cannot exceed 12 characters", nameof(value));

        return new PhoneNumber(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
