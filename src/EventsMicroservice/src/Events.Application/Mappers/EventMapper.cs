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
            e.MaxAllowedParticipants,
            e.MinAllowedAge,
            e.MaxAllowedAge,
            e.StartDate,
            e.ReservationEndDate,
            e.ImageThumbnail,
            e.CreatedDate,
            e.IsActive,
            e.IsPublic,
            e.EventFoodDetails,
            e.EventParticipants,
            e.EventImages
        );
    }

    public static EventOverviewDto ToEventOverviewDto(this Event e, IEnumerable<UserDto> users)
    {
        string[] mockImageThumbnails =
        {
           "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
           "https://img.freepik.com/free-photo/penne-pasta-tomato-sauce-with-chicken-tomatoes-wooden-table_2829-19744.jpg?semt=ais_hybrid&w=740&q=80",
           "https://cdn.foodfaithfitness.com/uploads/2025/02/a-crunchy_roll_sushi-feature-2.jpeg"
        };

        return new EventOverviewDto(
            e.EventId,
            e.UserId,
            users?.FirstOrDefault(x => x.Id == e.UserId)?.Name ?? string.Empty,
            4.4,
            e.Name,
            e.Description,
            e.MaxAllowedParticipants,
            e.MinAllowedAge,
            e.MaxAllowedAge,
            e.StartDate.Date.ToString("dd/MM/yyyy"),
            e.StartDate.ToString("hh:mm"),
            e.ReservationEndDate,
            e.EventFoodDetails?.Ingredients?.Split(',') ?? Enumerable.Empty<string>().ToArray(),
            e.EventParticipants.Count,
            mockImageThumbnails[new Random().Next(0, 2)],
            e.CreatedDate,
            new Random().Next(35, 85),
            e.IsActive,
            e.IsPublic,
            e.EventFoodDetails,
            e.EventParticipants,
            e.EventImages
        );
    }

    public static EventOverviewDto ToEventOverviewDto(this Event e, string userName)
    {
        string[] mockImageThumbnails =
        {
           "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
           "https://img.freepik.com/free-photo/penne-pasta-tomato-sauce-with-chicken-tomatoes-wooden-table_2829-19744.jpg?semt=ais_hybrid&w=740&q=80",
           "https://cdn.foodfaithfitness.com/uploads/2025/02/a-crunchy_roll_sushi-feature-2.jpeg"
        };

        return new EventOverviewDto(
            e.EventId,
            e.UserId,
            userName,
            4.4,
            e.Name,
            e.Description,
            e.MaxAllowedParticipants,
            e.MinAllowedAge,
            e.MaxAllowedAge,
            e.StartDate.Date.ToString("dd/MM/yyyy"),
            e.StartDate.ToString("hh:mm"),
            e.ReservationEndDate,
            e.EventFoodDetails?.Ingredients?.Split(',') ?? Enumerable.Empty<string>().ToArray(),
            e.EventParticipants.Count,
            mockImageThumbnails[new Random().Next(0, 2)],
            e.CreatedDate,
            new Random().Next(35, 85),
            e.IsActive,
            e.IsPublic,
            e.EventFoodDetails,
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
            MaxAllowedParticipants = dto.MaxAllowedParticipants,
            MinAllowedAge = dto.MinAllowedAge,
            MaxAllowedAge = dto.MaxAllowedAge,
            StartDate = dto.StartDate,
            ReservationEndDate = dto.ReservationEndDate,
            ImageThumbnail = dto.ImageThumbnail,
            IsActive = true,
            IsPublic = dto.IsPublic,
            EventFoodDetails = new EventFoodDetails { Name = dto.EventFoodDetails.Name, AdditionalFoodItems = dto.EventFoodDetails.AdditionalFoodItems, Ingredients = dto.EventFoodDetails.Ingredients },
            EventParticipants = Enumerable.Empty<EventParticipant>().ToList(),
            EventImages = dto.Images.ToList()
        };
    }
}