using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Users.Domain.Entities;

namespace Users.Infrastructure.Repository;

public class UserDbContext : DbContext 
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(user =>
            {
                user.ToTable("Users");
                user.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
                user.Property(u => u.Name).IsRequired();
                
                user.Property(u => u.Email).IsRequired();
                user.HasIndex(u => u.Email).IsUnique();
                
                user.Property(u => u.Password).IsRequired();
                user.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd()
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }
        );
    }
}