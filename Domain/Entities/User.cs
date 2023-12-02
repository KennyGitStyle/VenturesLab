namespace Domain.Entites;

/// <summary>
/// Represents a user in the system. This class contains basic user information
/// and a collection of tasks associated with the user.
/// </summary>
public class User
{
    public Guid Id { get; init; }
    public string Firstname { get; init; } = string.Empty;
    public string Lastname { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public IEnumerable<UserTask> Tasks { get; init; } = Enumerable.Empty<UserTask>();
}
