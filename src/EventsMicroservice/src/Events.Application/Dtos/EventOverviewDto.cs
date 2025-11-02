using Events.Domain.Entities;

namespace Events.Application.Dtos;

public record EventOverviewDto(
    Guid EventId,
    Guid UserId,
    string HostName,
    double HostRating,
    string Name,
    string Description,
    int MaxAllowedParticipants,
    int MinAllowedAge,
    int MaxAllowedAge,
    string StartDate,
    string StartTime,
    DateTime ReservationEndDate,
    string[] Tags,
    int ParticipantsCount,
    string ImageThumbnail,
    DateTime CreatedDate,
    double Price,
    bool IsActive,
    bool IsPublic,
    EventFoodDetails EventFoodDetails,
    ICollection<EventParticipant> EventParticipants,
    ICollection<EventImage> EventImages
    );
