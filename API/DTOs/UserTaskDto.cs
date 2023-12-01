namespace API.DTOs;

/// <summary>
/// Data Transfer Object for UserTask. It represents the data of a user task that can be exchanged between the client and the server.
/// </summary>
public class UserTaskDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateOnly CurrentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCurrentDate { get; set; }
}