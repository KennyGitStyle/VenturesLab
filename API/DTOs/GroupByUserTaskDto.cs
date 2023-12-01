namespace API.DTOs;
/// <summary>
/// Data Transfer Object representing a group of user tasks organized by a specific date.
/// </summary>
public class GroupedUserTaskDto
{
    public DateOnly Date { get; set; }
    public IEnumerable<UserTaskDto> Tasks { get; set; } = Enumerable.Empty<UserTaskDto>();
}

