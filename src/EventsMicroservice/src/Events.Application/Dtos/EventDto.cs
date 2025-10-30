using Events.Domain.Entities;

namespace Events.Application.Dtos;

public record EventDto(
    Guid EventId,
    Guid UserId,
    string Name,
    string Description,
    string FoodName,
    int MaxAllowedParticipants,
    int MinAllowedAge,
    int MaxAllowedAge,
    DateTime StartDate,
    DateTime ReservationEndDate,
    string ImageThumbnail,
    DateTime CreatedDate,
    bool IsActive,
    bool IsPublic,
    ICollection<EventParticipant> EventParticipants,
    ICollection<EventImage> EventImages
);