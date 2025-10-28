namespace Events.Application.Exceptions;

public class CommentIdNotFoundException : EventApplicationException
{
    public CommentIdNotFoundException(Guid id) : base($"Comment with id {id} not found.") { }
    
}