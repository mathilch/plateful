using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure.Context;

public static class DbInitializer
{
    public static void Seed(UserDbContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            var u1 = new User
            {
                Id = Guid.Parse("019b17fc-5147-794b-8a08-bfa49a2aaa90"),
                Name = "M L",
                Email = "mathiaslucht6@gmail.com",
                Password = "AQAAAAIAAYagAAAAEB2XG6SpuAoWW83UJv3XQN4v8TC8KjcJhUI9H21ax5qH7ptckGti101FiHMQuEXyog==",
                Verified = true,
                Score = 4.6f,
                Birthday = new DateOnly(2000, 1, 5),
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            var u2 = new User
            {
                Id = Guid.Parse("019b1802-a063-7095-9e29-8f9f3df736a1"),
                Name = "Test User",
                Email = "test@user.com",
                Password = "AQAAAAIAAYagAAAAEF10U5XQqf/HeLcY77542wQHIBmkdKNi+avIGYlPd0pcelrr31IY60QuWLfANZjXkw==",
                Verified = false,
                Score = 3.2f,
                Birthday = new DateOnly(2002, 7, 16),
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
            
            context.Users.AddRange(u1, u2);
            context.SaveChanges();
        }
        
        
    }
}