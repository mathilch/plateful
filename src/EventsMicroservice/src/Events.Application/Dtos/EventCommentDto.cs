namespace Events.Application.Dtos;

public record EventCommentDto(
    Guid CommentId,
    Guid EventId,
    Guid UserId,
    string Comment,
    DateTime CreatedAt
);