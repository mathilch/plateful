using Events.Application.Contracts.Repositories;
using Events.Infrastructure.Context;
using Events.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        return services;
    }

    public static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<EventsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("EventsDb"))
        );

        return services;
    }

    public static IServiceCollection ApplyMigrations(this IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var ctx = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
        ctx.Database.Migrate();

        return services;
    }
}
