using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventMapper
{
    public static EventDto ToDto(this Event e)
    {
        return new EventDto(
            e.EventId,
            e.UserId,
            e.Name,
            e.Description,
            e.FoodName,
            e.MaxAllowedParticipants,
            e.MinAllowedAge,
            e.MaxAllowedAge,
            e.StartDate,
            e.ReservationEndDate,
            e.ImageThumbnail,
            e.CreatedDate,
            e.IsActive,
            e.IsPublic,
            e.EventParticipants,
            e.EventImages
        );
    }

    public static Event ToEntity(this CreateEventRequestDto dto, Guid userId)
    {
        return new Event
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
            FoodName = dto.FoodName,
            MaxAllowedParticipants = dto.MaxAllowedParticipants,
            MinAllowedAge = dto.MinAllowedAge,
            MaxAllowedAge = dto.MaxAllowedAge,
            StartDate = dto.StartDate,
            ReservationEndDate = dto.ReservationEndDate,
            ImageThumbnail = dto.ImageThumbnail,
            IsActive = true,
            IsPublic = dto.IsPublic,
            EventParticipants = Enumerable.Empty<EventParticipant>().ToList(),
            EventImages = dto.Images
        };
    }
}