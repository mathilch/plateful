using Events.Domain.Entities;

namespace Events.Application.Dtos.Requests;

public record CreateEventRequestDto(
    string Name,
    string Description,
    int MaxAllowedParticipants,
    int MinAllowedAge,
    int MaxAllowedAge,
    DateTime StartDate,
    DateTime ReservationEndDate,
    string ImageThumbnail,
    bool IsPublic,
    EventFoodDetails EventFoodDetails,
    ICollection<EventImage> Images
);