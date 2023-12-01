using API.Config;
using Infrastructure.Data;
using Infrastructure.Service;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

/// <summary>
/// Provides extension methods for IServiceCollection to configure various services and functionalities 
/// such as API endpoints, CORS, Swagger, health checks, and database connections.
/// </summary>
public static class ServiceConfigurationExtension
{
    public static IServiceCollection AddConfigurationExtension(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        MappingProfile.Configure();
        services.AddMapster();
        services.AddScoped<IUserTask, UserTaskImplentation>();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
        services.AddSwaggerGen();
        services.AddHealthChecks();
            
        return services;
    }

    /// <summary>
    /// Adds and configures the database context for the application using a PostgreSQL connection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="config">The application's configuration to access the database connection string.</param>
    /// <returns>The IServiceCollection with the database context configured.</returns>
    public static IServiceCollection AddDbConnExtension(this IServiceCollection services, IConfiguration config) => 
        services.AddDbContext<UserTaskContext>(opts =>
        {
            opts.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });
}
