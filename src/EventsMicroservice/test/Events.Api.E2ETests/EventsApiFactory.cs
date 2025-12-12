using Events.Api;
using Events.Application.Contracts.ExternalApis;
using Events.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Xunit;

namespace Events.Api.E2ETests;

public class EventsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18")
        .WithDatabase("eventsdb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();

    private string ConnectionString => _dbContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:FoodClubEventsDbConnection"] = ConnectionString,
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience",
                ["Jwt:SecretKey"] = "super-secret-test-key-super-secret-test-key"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<EventsDbContext>));
            services.AddDbContext<EventsDbContext>(options =>
                options.UseNpgsql(ConnectionString));

            services.RemoveAll<IUserApiService>();
            services.AddSingleton<IUserApiService, FakeUserApiService>();

            services.RemoveAll<IPaymentService>();
            services.AddSingleton<IPaymentService, FakePaymentService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                options.DefaultChallengeScheme = TestAuthHandler.Scheme;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.Scheme, _ => { });
        });
    }

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        // Force the host to build so services are available
        _ = Server;
        
        // Apply migrations and seed using the test container
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
        await context.Database.MigrateAsync();
        DbInitializer.Seed(context);
    }

    public new async ValueTask DisposeAsync() => await _dbContainer.DisposeAsync();
}

