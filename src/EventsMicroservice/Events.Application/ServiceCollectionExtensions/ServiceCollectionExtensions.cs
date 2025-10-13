using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Application.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register application-layer services here (if/when you add them).
        // Example (later): services.AddScoped<IEventService, EventService>();

        return services;
    }
}
