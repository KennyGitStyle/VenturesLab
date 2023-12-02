using Domain.Entites;
using FluentAssertions;
using Infrastructure.Service;
using Moq;

namespace UnitTest;

public class TaskServiceTests
{
    private readonly Mock<IUserTask> _mockUserTask;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _mockUserTask = new Mock<IUserTask>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        // Setup the mock to return the test user tasks
        _mockUserTask.Setup(service => service.GetUserTasks())
                     .ReturnsAsync(GetTestUserTasks());

        // Setup the UnitOfWork mock to use the UserTask mock
        _mockUnitOfWork.Setup(uow => uow.UserTask).Returns(_mockUserTask.Object);

        // Instantiate the class under test with the mocked dependencies
        _taskService = new TaskService(_mockUnitOfWork.Object);
    }

    private IEnumerable<UserTask> GetTestUserTasks()
    {
        return new List<UserTask>
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
    public async Task RetrieveAllTasks_ShouldCallGetUserTasks()
    {
        // Act
        var result = await _taskService.RetrieveAllTasks();

        // Assert
        _mockUserTask.Verify(service => service.GetUserTasks(), Times.Once);
        result.Should().NotBeNull();
    }
}

// Hypothetical TaskService class using IUnitOfWork
public class TaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserTask>> RetrieveAllTasks() => 
        await _unitOfWork.UserTask.GetUserTasks();
}
