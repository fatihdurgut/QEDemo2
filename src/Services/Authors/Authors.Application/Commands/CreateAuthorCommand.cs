using Common.Application;
using FluentValidation;

namespace Authors.Application.Commands;

/// <summary>
/// Command to create a new author
/// </summary>
public record CreateAuthorCommand : ICommand<CommandResult<string>>
{
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Phone { get; init; } = "UNKNOWN";
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? ZipCode { get; init; }
    public bool HasContract { get; init; }
}

/// <summary>
/// Validator for CreateAuthorCommand
/// </summary>
public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Author ID is required")
            .MaximumLength(11).WithMessage("Author ID cannot exceed 11 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(20).WithMessage("First name cannot exceed 20 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(40).WithMessage("Last name cannot exceed 40 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(12).WithMessage("Phone cannot exceed 12 characters");

        RuleFor(x => x.State)
            .Length(2).WithMessage("State must be 2 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.State));

        RuleFor(x => x.ZipCode)
            .Length(5).WithMessage("ZIP code must be 5 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ZipCode));
    }
}
