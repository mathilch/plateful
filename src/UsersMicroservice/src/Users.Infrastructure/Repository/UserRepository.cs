using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Requests;

namespace Users.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    
    public UserRepository(UserDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }   

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> AddUser(CreateUserRequest createUser)
    {
        var user = new User
        {
            Name = createUser.Name,
            Email = createUser.Email,
            Password = createUser.Password,
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUser(Guid id, Action<User> op)
    {
        var user  = await _context.Users.FindAsync(id);
        if (user == null) return null;
        
        op(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> DeleteUser(Guid id)
    {
        var user  = await _context.Users.FindAsync(id);
        if (user == null) return null;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }
}