using Events.Api.Middlewares;
using Events.Application.ServiceCollectionExtensions;
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
        if(!builder.Environment.IsEnvironment("CICD"))
        {
            builder.Services.ConfigureDatabase(builder.Configuration);
            builder.Services.ApplyMigrations();
        }

        builder.Services.AddControllers();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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
            client.BaseAddress = new Uri("https://localhost:7083/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("CICD"))
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Events API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseExceptionHandler(options => { });
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors();
        app.Run();
    }
}
