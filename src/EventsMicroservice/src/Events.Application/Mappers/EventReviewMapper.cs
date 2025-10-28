using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventReviewMapper
{
    public static EventReviewDto ToDto(this EventReview ec)
    {
        return new EventReviewDto(
            ec.Id,
            ec.EventId,
            ec.UserId,
            ec.ReviewStars,
            ec.ReviewComment,
            ec.CreatedDate
        );
    }

    public static EventReview ToEntity(this CreateEventReviewRequestDto dto, Guid eventId, Guid userId)
    {
        return new EventReview
        {
            EventId = eventId,
            UserId = userId,
            ReviewStars = dto.Stars,
            ReviewComment = dto.Comment,
        };
    }
}