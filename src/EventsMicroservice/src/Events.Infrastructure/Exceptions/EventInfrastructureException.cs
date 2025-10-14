namespace Events.Infrastructure.Exceptions;

public class EventInfrastructureException : Exception
{
    protected EventInfrastructureException(string message) : base(message) { }
}