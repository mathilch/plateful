using Events.Application.Dtos.Requests;
using FluentValidation;

namespace Events.Application.Validators;

public class CreateEventRequestDtoValidator : AbstractValidator<CreateEventRequestDto>
{
    public CreateEventRequestDtoValidator()
    {
        // Name validation
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters");

        // Description validation
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(150).WithMessage("Description cannot exceed 150 characters");

        // MaxAllowedParticipants validation
        RuleFor(x => x.MaxAllowedParticipants)
            .GreaterThan(0).WithMessage("MaxAllowedParticipants must be at least 1");

        // PricePerSeat validation
        RuleFor(x => x.PricePerSeat)
            .GreaterThanOrEqualTo(0).WithMessage("PricePerSeat cannot be negative");

        // Age range validation
        RuleFor(x => x.MinAllowedAge)
            .GreaterThanOrEqualTo(0).WithMessage("MinAllowedAge cannot be negative")
            .LessThanOrEqualTo(150).WithMessage("MinAllowedAge must be at most 150");

        RuleFor(x => x.MaxAllowedAge)
            .GreaterThanOrEqualTo(0).WithMessage("MaxAllowedAge cannot be negative")
            .LessThanOrEqualTo(150).WithMessage("MaxAllowedAge must be at most 150");

        // Cross-field: MinAllowedAge must not exceed MaxAllowedAge
        RuleFor(x => x)
            .Must(x => x.MinAllowedAge <= x.MaxAllowedAge)
            .WithMessage("MinAllowedAge cannot be greater than MaxAllowedAge")
            .WithName("MinAllowedAge");

        // Date validations
        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("StartDate must be in the future");

        // Cross-field: EndDate must be after StartDate (if provided)
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("EndDate must be after StartDate");

        // Cross-field: ReservationEndDate must be before or equal to StartDate
        RuleFor(x => x.ReservationEndDate)
            .LessThanOrEqualTo(x => x.StartDate)
            .WithMessage("ReservationEndDate must be before or on the StartDate");

        // ImageThumbnail validation
        RuleFor(x => x.ImageThumbnail)
            .NotEmpty().WithMessage("ImageThumbnail is required")
            .MaximumLength(150).WithMessage("ImageThumbnail cannot exceed 150 characters");

        // Address validations
        RuleFor(x => x.StreetAddress)
            .NotEmpty().WithMessage("StreetAddress is required")
            .MaximumLength(256).WithMessage("StreetAddress cannot exceed 256 characters");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("PostalCode is required")
            .MaximumLength(32).WithMessage("PostalCode cannot exceed 32 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(128).WithMessage("City cannot exceed 128 characters");

        RuleFor(x => x.Region)
            .NotEmpty().WithMessage("Region is required")
            .MaximumLength(128).WithMessage("Region cannot exceed 128 characters");
    }
}
