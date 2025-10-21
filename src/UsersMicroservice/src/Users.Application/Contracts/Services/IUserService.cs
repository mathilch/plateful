using Users.Application.Dtos;

namespace Users.Application.Contracts.Services;

public interface IUserService
{
    Task<Guid> CreateUserAsync(string name, string email, string password);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> UpdateUserAsync(Guid userId, string name, string email);
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<string> AuthenticateAndGenerateUserTokenAsync(string email, string password);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}
