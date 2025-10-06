using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Users.Domain.Requests;
using Users.Infrastructure.Repository;

namespace Users.Infrastructure.Tests;


public sealed class UserRepositoryTests : IAsyncLifetime
{
    private readonly TestcontainerDatabase _postgresContainer;
    private UserDbContext _context = null!;
    private UserRepository _repository = null!;

    public UserRepositoryTests()
    {
        _postgresContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "testdb",
                Username = "postgres",
                Password = "postgres"
            })
            .WithImage("postgres:15")
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        // Create the extension before EF Core does anything
        await using var conn = new NpgsqlConnection(_postgresContainer.ConnectionString);
        await conn.OpenAsync();
        await using (var cmd = new NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        // Now build EF Core context
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseNpgsql(_postgresContainer.ConnectionString)
            .Options;

        _context = new UserDbContext(options);

        // Now it's safe to create schema
        await _context.Database.EnsureCreatedAsync();

        _repository = new UserRepository(_context);
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync().AsTask();
    }
    
    [Fact]
    public async Task AddUser_Should_Add_User()
    {
        var mockUser = new CreateUserRequest
        {
            Name = "Mock User",
            Email = "mock@example.com",
            Password = "password"
        };
        
        var addUser =  await _repository.AddUser(mockUser);
        var fetchUser = await _repository.GetUserByEmail(mockUser.Email);
        
        Assert.NotNull(addUser);
        Assert.NotNull(fetchUser);
        Assert.Equal(addUser.Id, fetchUser.Id);
    }
    
}
