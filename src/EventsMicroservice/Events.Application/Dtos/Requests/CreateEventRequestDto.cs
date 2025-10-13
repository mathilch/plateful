namespace Events.Application.Dtos.Requests;

public record CreateEventRequestDto(
    Guid UserId,
    string Name,
    string Description,
    string FoodName,
    int MaxAllowedParticipants,
    int MinAllowedAge,
    int MaxAllowedAge,
    DateTime StartDate,
    DateTime ReservationEndDate,
    string ImageThumbnail
);