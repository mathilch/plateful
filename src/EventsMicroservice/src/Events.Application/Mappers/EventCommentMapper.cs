using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventCommentMapper
{
    public static EventCommentDto ToDto(this EventComment ec)
    {
        return new EventCommentDto(
            ec.Id,
            ec.EventId,
            ec.UserId,
            ec.Comment,
            ec.CreatedDate
        );
    }

    public static EventComment ToEntity(this CreateEventCommentRequestDto dto, Guid eventId, Guid userId)
    {
        return new EventComment
        {
            EventId = eventId,
            UserId = userId,
            Comment = dto.Comment,
        };
    }
}