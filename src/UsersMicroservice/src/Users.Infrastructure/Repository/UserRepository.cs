using Microsoft.EntityFrameworkCore;
using Users.Application.Contracts.Repositories;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Application.Mappers;
using Users.Domain.Entities;
using Users.Infrastructure.Context;

namespace Users.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            return null;
        }
        return user.ToDto();
    }

    public async Task<UserDto?> GetUserByEmailAndPassword(string email, string password)
    {
        //var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email && user.Password == password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test_user@mail.com",
            IsActive = true,
            Name = "Test User",
            CreatedDate = DateTime.UtcNow,
        };
        return user.ToDto();
    }

    public async Task<List<UserDto?>> GetAllUsers()
    {
        return await _context.Users.Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<UserDto?> AddUser(CreateUserRequestDto createUser)
    {
        var user = new User
        {
            Name = createUser.Name,
            Email = createUser.Email,
            Password = createUser.Password,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.ToDto();
    }

    public async Task<UserDto?> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user.ToDto();
    }

    public async Task<UserDto?> UpdateUser(Guid id, Action<User> op)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        op(user);
        await _context.SaveChangesAsync();
        return user.ToDto();
    }
}