namespace Domain.Entites;

/// <summary>
/// Represents a task assigned to a user. This class encapsulates details
/// such as the task's schedule, description, and its association with a specific user.
/// </summary>
public class UserTask
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public DateOnly CurrentDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsCurrentDate { get; init; }
}
