namespace Events.Infrastructure.Exceptions;

public class EventIdNotFoundException : EventInfrastructureException
{
    public EventIdNotFoundException(Guid id) 
        : base($"Event with id {id} not found.") { }
}