namespace Users.Infrastructure.Exceptions;

public class DuplicateUserEmailException : InfrastructureException
{
    public DuplicateUserEmailException(string email) 
        : base($"User with email {email} already exists.") { }
}