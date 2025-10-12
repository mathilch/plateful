using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Domain.Entities;

namespace Users.Application.Contracts.Repositories;

public interface IUserRepository
{
    Task<UserDto> GetUserById(Guid id);
    Task<UserDto> GetUserByEmailAndPassword(string email, string password);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto> AddUser(CreateUserRequestDto createUser);
    Task<UserDto> UpdateUser(Guid id, Action<User> op);
    Task<UserDto> DeleteUser(Guid id);
}
