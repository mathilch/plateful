using Events.Api.Filters;
using Events.Api.Middlewares;
using Events.Application.ServiceCollectionExtensions;
using Events.Infrastructure.Context;
using Events.Infrastructure.ServiceExtensions;
using FluentValidation;

namespace Events.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        builder.Services.ConfigureApplicationServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureServices();

        var skipDatabaseSetup = builder.Environment.IsEnvironment("CICD") 
                              || builder.Environment.IsEnvironment("IntegrationTesting");
        if (!skipDatabaseSetup)
        {
            builder.Services.ConfigureDatabase(builder.Configuration);
            builder.Services.ConfigureExternalApis(builder.Configuration);
            builder.Services.ApplyMigrations();
        }

        builder.Services.AddScoped<ValidationFilter>();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        
        builder.Services.AddHealthChecks();

        builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.ConfigureJwtBearerAuthentication(builder.Configuration);
        builder.Services.ConfigureSwagger();
        builder.Services.AddAuthorization();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddHttpClient("UserApiClient", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UserApi:BaseAddress"));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Events API v1");
            c.RoutePrefix = string.Empty;
        });

        if (!skipDatabaseSetup)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<EventsDbContext>();
            DbInitializer.Seed(context!);
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseCors();
        
        app.MapHealthChecks("/health");
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        app.Run();
    }
}