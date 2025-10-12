using Events.Application.ServiceCollectionExtensions;
using Events.Infrastructure.ServiceExtensions;

namespace Events.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureApplicationServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureServices();
        builder.Services.ConfigureDatabase(builder.Configuration);
        builder.Services.ApplyMigrations();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
