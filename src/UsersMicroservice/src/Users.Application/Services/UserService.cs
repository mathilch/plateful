using Users.Application.Contracts.Repositories;
using Users.Application.Contracts.Services;
using Users.Application.Dtos;

namespace Users.Application.Services;

public class UserService(IUserRepository _userRepository, ITokenService _tokenService) : IUserService
{
    public Task<Guid> CreateUserAsync(string name, string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> AuthenticateAndGenerateUserTokenAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAndPassword(email, password);
        if (user is null)
        {
            throw new Exception("user not found");
        }
        return _tokenService.CreateToken(user.Id, user.Email);
    }

    public Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUserAsync(Guid userId, string name, string email)
    {
        throw new NotImplementedException();
    }
}
