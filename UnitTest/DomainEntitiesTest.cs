using Domain.Entites;
using FluentAssertions;

namespace UnitTest;

public class DomainEntitiesTests
{
    [Fact]
    public void UserTask_CreatedWithExpectedValues()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var currentDate = new DateOnly(2022, 4, 30);
        var startTime = new TimeSpan(9, 0, 0);
        var endTime = new TimeSpan(17, 0, 0);
        var subject = "New Task";
        var description = "Description of the new task";

        // Act
        var userTask = new UserTask
        {
            Id = taskId,
            UserId = userId,
            CurrentDate = currentDate,
            StartTime = startTime,
            EndTime = endTime,
            Subject = subject,
            Description = description,
            IsCurrentDate = true
        };

        // Assert
        userTask.Id.Should().Be(taskId);
        userTask.UserId.Should().Be(userId);
        userTask.CurrentDate.Should().Be(currentDate);
        userTask.StartTime.Should().Be(startTime);
        userTask.EndTime.Should().Be(endTime);
        userTask.Subject.Should().Be(subject);
        userTask.Description.Should().Be(description);
        userTask.IsCurrentDate.Should().BeTrue();
    }

}
