namespace Users.Infrastructure.Exceptions;

public class WrongUserCredentialsException : InfrastructureException
{
    public WrongUserCredentialsException(string email) 
        : base($"Wrong password for {email}") { }
    
}