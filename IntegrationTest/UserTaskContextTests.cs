using System.Text.Json;
using Domain.Entites;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest;

public class UserTaskContextTests : IDisposable
{
    private readonly UserTaskContext _context;

    public UserTaskContextTests()
    {
        // Use a unique name for the in-memory database to ensure each test runs against its own database
        var dbName = $"TestDb_{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<UserTaskContext>()
            .UseSqlite($"Data Source={dbName}")
            .Options;
        _context = new UserTaskContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedUserTestData", "testuser.json");
        SeedDatabase(_context, jsonFilePath);

    }
    private static void SeedDatabase(UserTaskContext context, string jsonDataPath)
    {
        var userData = File.ReadAllText(jsonDataPath);
        var users = JsonSerializer.Deserialize<List<User>>(userData, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (users == null) return;

        foreach (var user in users)
        {
            if (!context.Users.Any(u => u.Id == user.Id))
            {
                context.Users.Add(user); // Add user if not already in DB
            }
        }

        context.SaveChanges();
    }
    [Fact]
    public async Task UserTasks_Get_ReturnsTasks()
    {
        // Act
        var tasks = await _context.UserTasks.ToListAsync();
        // Assert
        Assert.NotNull(tasks);
        Assert.NotEmpty(tasks);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}

