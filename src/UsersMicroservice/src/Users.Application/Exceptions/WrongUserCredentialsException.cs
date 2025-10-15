namespace Users.Application.Exceptions;

public class WrongUserCredentialsException : ApplicationException
{
    public WrongUserCredentialsException(string email) 
        : base($"Wrong password for {email}") { }
    
}