using Domain.Entites;
using Infrastructure.Helper;

namespace Infrastructure.Service;

public interface IUserTask
{
    Task<IEnumerable<UserTask>> GetSortedUserTasks(SortParams sortParams);
    Task<IEnumerable<IGrouping<DateOnly, UserTask>>> GetTodayAndUpcomingUserTasks();
    Task<IEnumerable<UserTask>> GetUserTasks();
    Task<UserTask> GetUserTask(Guid id);
    Task<UserTask> AddUserTask(UserTask userTask,  CancellationToken cancellationToken = default);
    Task<UserTask> UpdateUserTask(Guid id, UserTask userTask, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserTask(Guid id, CancellationToken cancellationToken = default);
}
