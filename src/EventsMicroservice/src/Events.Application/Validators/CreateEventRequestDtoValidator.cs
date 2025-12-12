using Events.Application.Dtos.Requests;
using FluentValidation;
using FluentValidation.Validators;

namespace Events.Application.Validators;

public class CreateEventRequestDtoValidator : AbstractValidator<CreateEventRequestDto>
{
    public CreateEventRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.MaxAllowedParticipants)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            ;

        RuleFor(x => x.PricePerSeat)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MinAllowedAge)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(150);

        RuleFor(x => x.MaxAllowedAge)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(150);

        // Cross-field validation needs custom message
        RuleFor(x => x.MinAllowedAge)
            .LessThanOrEqualTo(x => x.MaxAllowedAge)
            .WithMessage("'Min Allowed Age' must be less than or equal to 'Max Allowed Age'.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.ReservationEndDate)
            .LessThanOrEqualTo(x => x.StartDate);

        RuleFor(x => x.ImageThumbnail)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.StreetAddress)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Region)
            .NotEmpty()
            .MaximumLength(128);
    }
}
