using Employees.Domain.Entities;

namespace Employees.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for Employee domain entity following MethodName_Condition_ExpectedResult pattern
/// </summary>
public class EmployeeTests
{
    [Fact(DisplayName = "Constructor creates valid employee with all properties")]
    public void Constructor_ValidParameters_CreatesEmployeeSuccessfully()
    {
        // Arrange
        var employeeId = "EMP001";
        var firstName = "John";
        var lastName = "Doe";
        var middleInitial = "M";
        var hireDate = DateTime.UtcNow.AddYears(-5);
        var jobLevel = "Senior Developer";

        // Act
        var employee = new Employee(employeeId, firstName, lastName, middleInitial, hireDate, jobLevel);

        // Assert
        employee.EmployeeId.Should().Be(employeeId);
        employee.FirstName.Should().Be(firstName);
        employee.LastName.Should().Be(lastName);
        employee.MiddleInitial.Should().Be(middleInitial);
        employee.HireDate.Should().Be(hireDate);
        employee.JobLevel.Should().Be(jobLevel);
        employee.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = "Constructor throws when employee ID is empty")]
    public void Constructor_EmptyEmployeeId_ThrowsArgumentException()
    {
        // Arrange
        var employeeId = "";
        var firstName = "John";
        var lastName = "Doe";
        var hireDate = DateTime.UtcNow.AddYears(-1);
        var jobLevel = "Developer";

        // Act
        var act = () => new Employee(employeeId, firstName, lastName, null, hireDate, jobLevel);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Employee ID cannot be empty*")
            .And.ParamName.Should().Be("employeeId");
    }

    [Fact(DisplayName = "Constructor throws when first name is empty")]
    public void Constructor_EmptyFirstName_ThrowsArgumentException()
    {
        // Arrange
        var employeeId = "EMP001";
        var firstName = "";
        var lastName = "Doe";
        var hireDate = DateTime.UtcNow.AddYears(-1);
        var jobLevel = "Developer";

        // Act
        var act = () => new Employee(employeeId, firstName, lastName, null, hireDate, jobLevel);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*First name cannot be empty*")
            .And.ParamName.Should().Be("firstName");
    }

    [Fact(DisplayName = "Constructor throws when last name is empty")]
    public void Constructor_EmptyLastName_ThrowsArgumentException()
    {
        // Arrange
        var employeeId = "EMP001";
        var firstName = "John";
        var lastName = "";
        var hireDate = DateTime.UtcNow.AddYears(-1);
        var jobLevel = "Developer";

        // Act
        var act = () => new Employee(employeeId, firstName, lastName, null, hireDate, jobLevel);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Last name cannot be empty*")
            .And.ParamName.Should().Be("lastName");
    }

    [Fact(DisplayName = "Constructor throws when hire date is in future")]
    public void Constructor_FutureHireDate_ThrowsArgumentException()
    {
        // Arrange
        var employeeId = "EMP001";
        var firstName = "John";
        var lastName = "Doe";
        var hireDate = DateTime.UtcNow.AddDays(1);
        var jobLevel = "Developer";

        // Act
        var act = () => new Employee(employeeId, firstName, lastName, null, hireDate, jobLevel);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Hire date cannot be in the future*")
            .And.ParamName.Should().Be("hireDate");
    }

    [Fact(DisplayName = "GetFullName returns first and last name without middle initial")]
    public void GetFullName_NoMiddleInitial_ReturnsFirstAndLastName()
    {
        // Arrange
        var employee = new Employee(
            "EMP001", "John", "Doe", null, 
            DateTime.UtcNow.AddYears(-1), "Developer");

        // Act
        var fullName = employee.GetFullName();

        // Assert
        fullName.Should().Be("John Doe");
    }

    [Fact(DisplayName = "GetFullName returns full name with middle initial")]
    public void GetFullName_WithMiddleInitial_ReturnsFullNameWithMiddleInitial()
    {
        // Arrange
        var employee = new Employee(
            "EMP001", "John", "Doe", "M",
            DateTime.UtcNow.AddYears(-1), "Developer");

        // Act
        var fullName = employee.GetFullName();

        // Assert
        fullName.Should().Be("John M. Doe");
    }

    [Fact(DisplayName = "UpdateJobLevel updates job level successfully")]
    public void UpdateJobLevel_ValidJobLevel_UpdatesSuccessfully()
    {
        // Arrange
        var employee = new Employee(
            "EMP001", "John", "Doe", null,
            DateTime.UtcNow.AddYears(-1), "Developer");
        var newJobLevel = "Senior Developer";

        // Act
        employee.UpdateJobLevel(newJobLevel);

        // Assert
        employee.JobLevel.Should().Be(newJobLevel);
    }

    [Fact(DisplayName = "UpdateJobLevel throws when job level is empty")]
    public void UpdateJobLevel_EmptyJobLevel_ThrowsArgumentException()
    {
        // Arrange
        var employee = new Employee(
            "EMP001", "John", "Doe", null,
            DateTime.UtcNow.AddYears(-1), "Developer");

        // Act
        var act = () => employee.UpdateJobLevel("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Job level cannot be empty*")
            .And.ParamName.Should().Be("newJobLevel");
    }

    [Fact(DisplayName = "Deactivate sets IsActive to false")]
    public void Deactivate_ActiveEmployee_SetsIsActiveToFalse()
    {
        // Arrange
        var employee = new Employee(
            "EMP001", "John", "Doe", null,
            DateTime.UtcNow.AddYears(-1), "Developer");

        // Act
        employee.Deactivate();

        // Assert
        employee.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = "Reactivate sets IsActive to true")]
    public void Reactivate_InactiveEmployee_SetsIsActiveToTrue()
    {
        // Arrange
        var employee = new Employee(
            "EMP001", "John", "Doe", null,
            DateTime.UtcNow.AddYears(-1), "Developer");
        employee.Deactivate();

        // Act
        employee.Reactivate();

        // Assert
        employee.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = "GetYearsOfService returns correct years for 5-year employee")]
    public void GetYearsOfService_FiveYearEmployee_ReturnsFive()
    {
        // Arrange
        var hireDate = DateTime.UtcNow.AddYears(-5).AddDays(-10);
        var employee = new Employee(
            "EMP001", "John", "Doe", null,
            hireDate, "Developer");

        // Act
        var yearsOfService = employee.GetYearsOfService();

        // Assert
        yearsOfService.Should().Be(5);
    }

    [Fact(DisplayName = "GetYearsOfService returns zero for new employee")]
    public void GetYearsOfService_NewEmployee_ReturnsZero()
    {
        // Arrange
        var hireDate = DateTime.UtcNow.AddMonths(-6);
        var employee = new Employee(
            "EMP001", "John", "Doe", null,
            hireDate, "Developer");

        // Act
        var yearsOfService = employee.GetYearsOfService();

        // Assert
        yearsOfService.Should().Be(0);
    }
}
