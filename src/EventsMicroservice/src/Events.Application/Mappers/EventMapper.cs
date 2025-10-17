using Events.Application.Dtos;
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
            e.IsPublic
        );
    }
}