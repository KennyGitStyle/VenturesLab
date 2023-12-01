using System.Text.Json;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

/// <summary>
/// Provides functionality to seed the UserTaskContext database with initial data.
/// </summary>
public class UserTaskContextSeed
{
    /// <summary>
    /// Seeds the UserTaskContext database with User data from a JSON file.
    /// </summary>
    /// <param name="taskContext">The UserTaskContext instance for database access.</param>
    /// <param name="loggerFactory">Factory to create a logger for logging errors.</param>
    public static async Task SeedDatabaseAsync(UserTaskContext taskContext, ILoggerFactory loggerFactory)
    {
        var userData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/users.json");
        var users = JsonSerializer.Deserialize<List<User>>(userData);
        try
        {
            if (users is not null)
            {
                var userIds = users.Select(u => u.Id).ToList();

                var existingUserIds = await taskContext.Users
                    .AsNoTracking()
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => u.Id)
                    .ToListAsync();

                var usersToAdd = users.Where(u => !existingUserIds.Contains(u.Id)).ToList();

                if (usersToAdd.Count > 0)
                {
                    await taskContext.Users.AddRangeAsync(usersToAdd);
                    await taskContext.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<UserTaskContextSeed>();
            logger.LogError($"Error occurred while seeding the database: {ex.Message}");
        }
    }
}
