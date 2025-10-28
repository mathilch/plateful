namespace Events.Application.Exceptions;

public class UnauthorizedUserForCommentException : EventApplicationException
{
    public UnauthorizedUserForCommentException(Guid commentId, Guid commentUserId, Guid currentUserId)
        : base($"User {currentUserId} does not have access to comment {commentId}, only {commentUserId} does") { }
    
}