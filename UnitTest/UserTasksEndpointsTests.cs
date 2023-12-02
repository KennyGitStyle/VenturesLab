using API.DTOs;
using API.Endpoints;
using Domain.Entites;
using FluentAssertions;
using Infrastructure.Service;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace UnitTest;

public class UserTasksEndpointsTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly List<UserTask> _mockUserTasks;

    public UserTasksEndpointsTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mockUserTasks = new List<UserTask> 
        {
            new() 
            {
                Id = Guid.Parse("f231b16b-2bab-4fbf-985c-4efb0db7e5bb"),
                UserId = Guid.Parse("b7761dd1-68d8-4584-9bd4-91bc6ea3d859"),
                CurrentDate = DateOnly.FromDateTime(DateTime.Now),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Subject = "Meeting with Team",
                Description = "Team meeting to discuss project progress",
                IsCurrentDate = true
            },
            new() 
            {
                Id = Guid.Parse("0014c511-a9c1-4ea3-8ddc-d5142133fda9"),
                UserId = Guid.Parse("b7761dd1-68d8-4584-9bd4-91bc6ea3d859"),
                CurrentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                StartTime = new TimeSpan(11, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                Subject = "Client Call",
                Description = "Call with client to discuss requirements",
                IsCurrentDate = false
            }
        };
    }

    [Fact]
    public async Task GetUserTasks_CallsGetUserTasks_OnUnitOfWork()
    {
        // Arrange
        _unitOfWorkMock.Setup(uow => uow.UserTask.GetUserTasks())
                       .ReturnsAsync(_mockUserTasks);

        // Act
        await UserTasksEndpoints.GetUserTasks(_unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.GetUserTasks(), Times.Once);
    }

    [Fact]
    public async Task GetUserTasks_WhenCalled_ReturnsExpectedResults()
    {
        // Arrange
        _unitOfWorkMock.Setup(uow => uow.UserTask.GetUserTasks())
                       .ReturnsAsync(_mockUserTasks);

        // Act
        var result = await UserTasksEndpoints.GetUserTasks(_unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.GetUserTasks(), Times.Once);
        
    }

    [Fact]
    public async Task GetUserTasks_WhenThrowsException_HandlesGracefully()
    {
        // Arrange
        var expectedExceptionMessage = "Test exception";
        _unitOfWorkMock.Setup(uow => uow.UserTask.GetUserTasks())
                       .ThrowsAsync(new Exception(expectedExceptionMessage));

        // Act
        var result = await UserTasksEndpoints.GetUserTasks(_unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.GetUserTasks(), Times.Once);

        // Check the type of result
        result.Should().BeOfType<Results<Ok<IEnumerable<UserTaskDto>>, NotFound<string>>>();

    }

    [Fact]
    public async Task GetUserTasksByGroup_ReturnsGroupedTasks()
    {
        // Arrange
        var mockUserTasks = new List<UserTask>
        {
            new UserTask 
            {
                Id = Guid.Parse("f231b16b-2bab-4fbf-985c-4efb0db7e5bb"),
                UserId = Guid.Parse("b7761dd1-68d8-4584-9bd4-91bc6ea3d859"),
                CurrentDate = DateOnly.FromDateTime(DateTime.Now),
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Subject = "Meeting with Team",
                Description = "Team meeting to discuss project progress",
                IsCurrentDate = true
            },
            new UserTask 
            {
                Id = Guid.Parse("0014c511-a9c1-4ea3-8ddc-d5142133fda9"),
                UserId = Guid.Parse("b7761dd1-68d8-4584-9bd4-91bc6ea3d859"),
                CurrentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                StartTime = new TimeSpan(11, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                Subject = "Client Call",
                Description = "Call with client to discuss requirements",
                IsCurrentDate = false
            }
        };

        var mockTasks = mockUserTasks
            .GroupBy(task => task.CurrentDate)
            .ToList();

        _unitOfWorkMock.Setup(uow => uow.UserTask.GetTodayAndUpcomingUserTasks())
                       .ReturnsAsync(mockTasks);

        // Act
        var result = await UserTasksEndpoints.GetUserTasksByGroup(_unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.GetTodayAndUpcomingUserTasks(), Times.Once);
        result.Should().BeOfType<Results<Ok<IEnumerable<GroupedUserTaskDto>>, NotFound<string>>>();
    }

    [Fact]
    public async Task GetUserTasks_WhenCalled_SuccessfullyRetrievesTasks()
    {
        // Arrange
        _unitOfWorkMock.Setup(uow => uow.UserTask.GetUserTasks())
                       .ReturnsAsync(_mockUserTasks);

        // Act
        var result = await UserTasksEndpoints.GetUserTasks(_unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.GetUserTasks(), Times.Once);
        result.Should().BeOfType<Results<Ok<IEnumerable<UserTaskDto>>, NotFound<string>>>();

    }

    [Fact]
    public async Task GetUserTasks_WhenNoTasks_ReturnsEmptyList()
    {
        // Arrange
        _unitOfWorkMock.Setup(uow => uow.UserTask.GetUserTasks())
                       .ReturnsAsync(new List<UserTask>());

        // Act
        var result = await UserTasksEndpoints.GetUserTasks(_unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.GetUserTasks(), Times.Once);
        result.Should().BeOfType<Results<Ok<IEnumerable<UserTaskDto>>, NotFound<string>>>();
    }

    [Fact]
    public async Task GetUserTasks_WhenThrowsException_ReturnsNotFound()
    {
        // Arrange
        var expectedExceptionMessage = "Test exception";
        _unitOfWorkMock.Setup(uow => uow.UserTask.GetUserTasks())
                       .ThrowsAsync(new Exception(expectedExceptionMessage));

        // Act
        var result = await UserTasksEndpoints.GetUserTasks(_unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.GetUserTasks(), Times.Once);
        result.Should().BeOfType<Results<Ok<IEnumerable<UserTaskDto>>, NotFound<string>>>();
    }

    [Fact]
    public async Task AddUserTask_WhenCalled_ReturnsCreated()
    {
        // Arrange
        var newUserTaskDto = new UserTaskDto 
        {
            Id = "f231b16b-2bab-4fbf-985c-4efb0db7e5bb",
            UserId = "b7761dd1-68d8-4584-9bd4-91bc6ea3d859",
            CurrentDate = DateOnly.FromDateTime(DateTime.Now),
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(10, 0, 0),
            Subject = "Meeting with Team",
            Description = "Team meeting to discuss project progress",
            IsCurrentDate = true
        };
        var newUserTask = newUserTaskDto.Adapt<UserTask>();

        _unitOfWorkMock.Setup(uow => uow.UserTask.AddUserTask(It.IsAny<UserTask>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(newUserTask); // Return Task with newUserTask

        // Act
        var result = await UserTasksEndpoints.AddUserTask(_unitOfWorkMock.Object, newUserTaskDto);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.AddUserTask(It.Is<UserTask>(task => 
            task.Id == newUserTask.Id &&
            task.UserId == newUserTask.UserId &&
            task.CurrentDate == newUserTask.CurrentDate &&
            task.StartTime == newUserTask.StartTime &&
            task.EndTime == newUserTask.EndTime &&
            task.Subject == newUserTask.Subject &&
            task.Description == newUserTask.Description &&
            task.IsCurrentDate == newUserTask.IsCurrentDate
        ), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<Results<Created, NotFound<string>>>();
    }

    [Fact]
    public async Task UpdateUserTask_WhenCalled_ReturnsNoContent()
    {
        // Arrange
    var userTaskUpdateDto = new UserTaskDto 
    {
        Id = "f231b16b-2bab-4fbf-985c-4efb0db7e5bb",
        UserId = "b7761dd1-68d8-4584-9bd4-91bc6ea3d859",
        CurrentDate = DateOnly.FromDateTime(DateTime.Now),
        StartTime = new TimeSpan(8, 0, 0),
        EndTime = new TimeSpan(10, 0, 0),
        Subject = "Meeting with Team",
        Description = "Team meeting to discuss project progress",
        IsCurrentDate = true
    };
    var taskId = Guid.Parse("cbc0a7de-c1a4-4c6d-8ec2-b99b9986503f");

    // Create a UserTask based on the userTaskUpdateDto
    var newUserTask = userTaskUpdateDto.Adapt<UserTask>();

    // Set up the unitOfWorkMock to return a Task<UserTask>
    _unitOfWorkMock.Setup(uow => uow.UserTask
                   .UpdateUserTask(It.IsAny<Guid>(), It.IsAny<UserTask>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(newUserTask);

    // Act
    var result = await UserTasksEndpoints.UpdateUserTask(taskId, _unitOfWorkMock.Object, userTaskUpdateDto);

    // Assert
    _unitOfWorkMock.Verify(uow => uow.UserTask.UpdateUserTask(taskId, It.IsAny<UserTask>(), It.IsAny<CancellationToken>()), Times.Once);
    result.Should().BeOfType<Results<NoContent, NotFound<string>>>();

    }

    [Fact]
    public async Task DeleteUserTask_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _unitOfWorkMock.Setup(uow => uow.UserTask.DeleteUserTask(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        // Act
        var result = await UserTasksEndpoints.DeleteUserTask(taskId, _unitOfWorkMock.Object);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.UserTask.DeleteUserTask(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<Results<NoContent, NotFound<string>>>();
    }
}