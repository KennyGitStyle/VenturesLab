using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

/// <summary>
/// Data Transfer Object for User. It represents user data that can be sent to and from the client.
/// </summary>
public class UserDto
{
    public string Id { get; set; } = string.Empty;
    [Required(ErrorMessage = "First name is required.")]
    public string Firstname { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last name is required.")]
    public string Lastname { get; set; }  = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public List<UserTaskDto> Tasks { get; set; } = new();
}
