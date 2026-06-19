using FluentValidation;
using HealthTracker.Application.Features.SymptomLogs.Commands;

namespace HealthTracker.Application.Validators;

public class CreateSymptomLogValidator : AbstractValidator<CreateSymptomLogCommand>
{
    public CreateSymptomLogValidator()
    {
        RuleFor(x => x.SymptomName)
            .NotEmpty().WithMessage("Symptom name is required.")
            .MaximumLength(100).WithMessage("Symptom name cannot exceed 100 characters.");

        RuleFor(x => x.Severity)
            .InclusiveBetween(1, 10).WithMessage("Severity must be between 1 and 10.");

        RuleFor(x => x.LogDate)
            .NotEmpty().WithMessage("Log date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Log date cannot be in the future.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
            .When(x => x.Notes != null);
    }
}