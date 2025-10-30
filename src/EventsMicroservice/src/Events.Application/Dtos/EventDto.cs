using Events.Domain.Entities;

namespace Events.Application.Dtos;

public record EventDto(
    Guid EventId,
    Guid UserId,
    string Name,
    string Description,
    int MaxAllowedParticipants,
    int MinAllowedAge,
    int MaxAllowedAge,
    DateTime StartDate,
    DateTime ReservationEndDate,
    string ImageThumbnail,
    DateTime CreatedDate,
    bool IsActive,
    bool IsPublic,
    EventFoodDetails EventFoodDetails,
    ICollection<EventParticipant> EventParticipants,
    ICollection<EventImage> EventImages
);