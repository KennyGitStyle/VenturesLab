using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Database context class for the User and UserTask entities.
/// Manages the entity framework interactions and configurations for these entities.
/// </summary>
public class UserTaskContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the UserTaskContext with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public UserTaskContext(DbContextOptions<UserTaskContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }

    public DbSet<UserTask> UserTasks { get; set; }

    /// <summary>
    /// Configures the model relationships using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Provides a simple API for configuring the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<UserTask>()
            .HasOne<User>()
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId);
    }
}
