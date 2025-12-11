using Users.Application.Contracts.Services;

namespace Users.Application.Tests;

public class DummyTokenService : ITokenService
{
    public string CreateToken(Guid userId, string email, string userName)
    {
        return $"token-{userId}-{email}-{userName}";
    }
}