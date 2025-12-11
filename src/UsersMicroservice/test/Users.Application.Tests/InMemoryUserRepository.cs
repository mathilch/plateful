using Users.Application.Contracts.Repositories;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Domain.Entities;

namespace Users.Application.Tests;

public class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, UserDto> _users = new();
    private readonly Dictionary<String, (UserDto userDto, String hashedPassword)> _usersByEmail = new();
    
    public Task<UserDto> GetUserById(Guid id)
    {
        _users.TryGetValue(id, out var userDto);
        return Task.FromResult(userDto);
    }

    public Task<IEnumerable<UserDto>> GetMultipleUsersByIds(IEnumerable<Guid> ids)
    {
        var result =
            ids
                .Where(id => _users.ContainsKey(id))
                .Select(id => _users[id]);
        
        return Task.FromResult(result);
    }

    public Task<(UserDto userDto, string hashedPassword)> GetUserByEmail(string email)
    {
        _usersByEmail.TryGetValue(email, out var userDto);
        return Task.FromResult(userDto);
    }

    public Task<List<UserDto>> GetAllUsers()
    {
        return Task.FromResult(_users.Values.ToList());
    }

    public Task<UserDto> AddUser(CreateUserRequestDto createUser)
    {
        var id = Guid.NewGuid();

        var user = new UserDto(
            id,
            createUser.Name,
            createUser.Email,
            false,
            0,
            createUser.Birthday,
            DateTime.Now,
            true
        );

        _users[id] = user;
        _usersByEmail[createUser.Email] = (user, createUser.Password);
        
        return Task.FromResult(user);
    }

    public Task<UserDto> UpdateUser(Guid id, Action<User> op)
    {
        throw new NotImplementedException("Not yet");
    }

    public Task DeleteUser(Guid id)
    {
        if (_users.TryGetValue(id, out var user))
            _usersByEmail.Remove(user.Email);
        _users.Remove(id);
        
        return Task.CompletedTask;
    }

    public Task DeactivateUser(Guid id)
    {
        throw new NotImplementedException("Not yet");
    }
}