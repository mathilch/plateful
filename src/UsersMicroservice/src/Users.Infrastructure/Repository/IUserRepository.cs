using Users.Domain.Entities;
using Users.Domain.Requests;

namespace Users.Infrastructure.Repository;

public interface IUserRepository
{
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByEmail(string email);
    Task<List<User>> GetAllUsers();
    
    Task<User?> AddUser(CreateUserRequest createUser);
    Task<User?> UpdateUser(Guid id, Action<User> op);
    Task<User?> DeleteUser(Guid id);
}