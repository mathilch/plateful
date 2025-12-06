using Users.Application.Contracts.Services;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;

namespace Users.Api.Tests;

public class InMemoryUserService : IUserService
{
    private readonly Dictionary<Guid, UserDto> _users = new();
    public Task<Guid> CreateUserAsync(CreateUserRequestDto createUserDto)
    {
        var id = Guid.NewGuid();
        var user = new UserDto(
            id,
            createUserDto.Name,
            createUserDto.Email,
            false,
            0,
            createUserDto.Birthday,
            DateTime.Now,
            true
        );
        
        _users.Add(id, user);
        return Task.FromResult(id);
    }

    public Task DeleteUserAsync(Guid userId)
    {
        _users.Remove(userId);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateUserAsync(Guid userId, string name, string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        _users.TryGetValue(userId, out var user);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<UserDto>?> GetMultipleUsersByIdsAsync(IEnumerable<Guid> userIds)
    {
        var users = _users.Where(x => userIds.Contains(x.Key)).Select(x => x.Value);
        return Task.FromResult<IEnumerable<UserDto>?>(users);
    }

    public Task<string> AuthenticateAndGenerateUserTokenAsync(LoginUserRequestDto requestDto)
    {
        var user = _users.Values.FirstOrDefault(x => x.Email == requestDto.Email);
        if (user == null) throw new Exception("User not found");
        return Task.FromResult("dummy-token");
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return Task.FromResult<IEnumerable<UserDto>>(_users.Values);
    }

    public Task DeactivateUserAsync(Guid id)
    {
        if (_users.TryGetValue(id, out var user))
        {
            var updated = user with { IsActive = false };
            _users[id] = updated;
        }
        return Task.CompletedTask;
    }
}