namespace Events.Application.Exceptions;

public class CannotCommentException : EventApplicationException
{
    public CannotCommentException(Guid eventId) 
        : base($"Cannot comment on event {eventId}") { }

}