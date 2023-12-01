using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

/// <summary>
/// Provides an extension method for WebApplication to initialize the database.
/// This includes applying any pending migrations and seeding the database with initial data.
/// </summary>
public static class DbInitializer
{
    public static async Task UseDbInitializer(this WebApplication builder)
    {
       using var provider = builder.Services.CreateScope();

       var services = provider.ServiceProvider;
       var loggerFactory = services.GetRequiredService<ILoggerFactory>();

       try 
       {
          var context = services.GetRequiredService<UserTaskContext>();
          await context.Database.MigrateAsync();
          await UserTaskContextSeed.SeedDatabaseAsync(context, loggerFactory);
       } 
       catch(Exception ex)
       {
          var logger = loggerFactory.CreateLogger<Program>();
          logger.LogError($"An error occured while migration the database: {ex.Message}");
       }
    }
}
