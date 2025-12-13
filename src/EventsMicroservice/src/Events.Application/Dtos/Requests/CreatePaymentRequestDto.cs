namespace Events.Application.Dtos.Requests;

public record CreatePaymentRequestDto(
    Guid EventId,
    Guid UserId,
    long Amount
);