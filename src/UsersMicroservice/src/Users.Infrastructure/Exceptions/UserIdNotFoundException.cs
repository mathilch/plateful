namespace Users.Infrastructure.Exceptions;

public class UserIdNotFoundException : InfrastructureException
{
    public UserIdNotFoundException(Guid id) 
        : base($"User with ID: {id} not found.") { }
    
}