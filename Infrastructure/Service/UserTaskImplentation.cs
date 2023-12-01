using Domain.Entites;
using Infrastructure.Data;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service;

/// <summary>
/// Provides implementation for IUserTask interface, handling the business logic for user task operations.
/// </summary>

public class UserTaskImplentation : IUserTask
{
    private readonly UserTaskContext _context;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the UserTaskImplementation with the specified UserTaskContext and UnitOfWork.
    /// </summary>
    /// <param name="context">The UserTaskContext instance for database access.</param>
    /// <param name="unitOfWork">The UnitOfWork instance for managing transactions.</param>

    public UserTaskImplentation(UserTaskContext context, IUnitOfWork unitOfWork)
    {
        _context = context  ?? throw new ArgumentNullException(nameof(context));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));;
    }

    /// <summary>
    /// Adds a new UserTask to the database.
    /// </summary>
    /// <param name="userTask">The UserTask to add.</param>
    public async Task<UserTask> AddUserTask(UserTask userTask, CancellationToken cancellationToken = default)
    {
        if (userTask is not null)
        {
            var addedUser = await _context.UserTasks
                    .AddAsync(userTask, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return addedUser.Entity;
        }

        throw new ArgumentNullException(nameof(userTask));
    }

    /// <summary>
    /// Deletes a UserTask from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the UserTask to delete.</param>
    public async Task<bool> DeleteUserTask(Guid id, CancellationToken cancellationToken = default)
    {
        await _context.Set<UserTask>()
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;

    }

    /// <summary>
    /// Retrieves a sorted list of UserTasks based on specified sorting parameters.
    /// </summary>
    /// <param name="sortParams">The sorting parameters to apply.</param>
    public async Task<IEnumerable<UserTask>> GetSortedUserTasks(SortParams sortParams)
    {
        var query = _context.UserTasks.AsQueryable();

        if (sortParams.Date.HasValue)
        {
            query = query.Where(task => task.CurrentDate == sortParams.Date.Value || task.IsCurrentDate);
        }

        query = sortParams.SortBy switch
        {
            "StartTime" => sortParams.Ascending ? query.OrderBy(task => task.StartTime) : query.OrderByDescending(task => task.StartTime),
            "EndTime" =>  sortParams.Ascending ? query.OrderBy(task => task.EndTime) : query.OrderByDescending(task => task.EndTime),
            "UserId" =>  sortParams.Ascending ? query.OrderBy(task => task.UserId) : query.OrderByDescending(task => task.UserId),
            _ =>  sortParams.Ascending ? query.OrderBy(task => task.CurrentDate) : query.OrderByDescending(task => task.CurrentDate),
        };

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves tasks scheduled for today and upcoming days.
    /// </summary>
    public async Task<IEnumerable<IGrouping<DateOnly, UserTask>>> GetTodayAndUpcomingUserTasks()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var tasks = await _context.UserTasks
            .AsNoTracking()
            .Where(task => task.CurrentDate >= today)
            .OrderBy(task => task.CurrentDate)
            .ThenBy(task => task.StartTime)
            .ToListAsync();

        return tasks.GroupBy(task => task.CurrentDate);
    }

    /// <summary>
    /// Retrieves a UserTask by its ID.
    /// </summary>
    /// <param name="id">The ID of the UserTask to retrieve.</param>
    public async Task<UserTask> GetUserTask(Guid id)
    {
        var getUserById = await _context.UserTasks.FirstOrDefaultAsync(u => u.Id == id);

        return getUserById is null ? throw new ArgumentNullException(nameof(getUserById), "User not found") : getUserById;
    }

    /// <summary>
    /// Retrieves all UserTasks.
    /// </summary>
    public async Task<IEnumerable<UserTask>> GetUserTasks() => 
        await _context.UserTasks.AsNoTracking().ToListAsync();

    /// <summary>
    /// Updates a UserTask in the database.
    /// </summary>
    /// <param name="id">The ID of the UserTask to update.</param>
    /// <param name="userTask">The updated UserTask.</param>
    public async Task<UserTask> UpdateUserTask(Guid id, UserTask userTask, CancellationToken cancellationToken = default)
    {
        if (userTask is not null)
        {
            var getUserById = await _context.UserTasks.FirstOrDefaultAsync(u => u.Id == id);

            if (getUserById is null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }

            _context.Update(getUserById);

            await _unitOfWork.CompleteAsync();

            return getUserById;
            
        }
        throw new ArgumentNullException(nameof(userTask));
    }
}
