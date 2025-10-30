using Events.Application.Dtos;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventFoodDetailsMapper
{
    public static EventFoodDetailsDto ToDto(this EventFoodDetails fd)
    {
        return new EventFoodDetailsDto(
            fd.Id,
            fd.EventId,
            fd.Name,
            fd.Ingredients,
            fd.AdditionalFoodItems
        );
    }

    public static EventFoodDetails ToEntity(this EventFoodDetailsDto dto)
    {
        return new EventFoodDetails
        {
            Id = dto.Id,
            EventId = dto.EventId,
            Name = dto.Name,
            Ingredients = dto.Ingredients,
            AdditionalFoodItems = dto.AdditionalFoodItems,
        };
    }
}