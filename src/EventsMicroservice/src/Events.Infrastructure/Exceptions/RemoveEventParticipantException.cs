namespace Events.Infrastructure.Exceptions;

public class RemoveEventParticipantException : EventInfrastructureException 
{
    public RemoveEventParticipantException(Guid eventId, Guid userId)
        : base($"Cannot remove eventparticipant with id {userId} from event with id {eventId}") { }
    
}