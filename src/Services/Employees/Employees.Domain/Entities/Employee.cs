namespace Employees.Domain.Entities;

/// <summary>
/// Represents an employee aggregate root in the domain
/// </summary>
public class Employee
{
    public string EmployeeId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? MiddleInitial { get; private set; }
    public DateTime HireDate { get; private set; }
    public string JobLevel { get; private set; }
    public bool IsActive { get; private set; }

    private Employee()
    {
        // EF Core constructor
        EmployeeId = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        JobLevel = string.Empty;
    }

    public Employee(
        string employeeId,
        string firstName,
        string lastName,
        string? middleInitial,
        DateTime hireDate,
        string jobLevel)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));
        
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        if (hireDate > DateTime.UtcNow)
            throw new ArgumentException("Hire date cannot be in the future", nameof(hireDate));

        EmployeeId = employeeId;
        FirstName = firstName;
        LastName = lastName;
        MiddleInitial = middleInitial;
        HireDate = hireDate;
        JobLevel = jobLevel;
        IsActive = true;
    }

    public string GetFullName()
    {
        return string.IsNullOrWhiteSpace(MiddleInitial)
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {MiddleInitial}. {LastName}";
    }

    public void UpdateJobLevel(string newJobLevel)
    {
        if (string.IsNullOrWhiteSpace(newJobLevel))
            throw new ArgumentException("Job level cannot be empty", nameof(newJobLevel));

        JobLevel = newJobLevel;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Reactivate()
    {
        IsActive = true;
    }

    public int GetYearsOfService()
    {
        return (int)((DateTime.UtcNow - HireDate).TotalDays / 365.25);
    }
}
