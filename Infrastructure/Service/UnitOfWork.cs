using Infrastructure.Data;

namespace Infrastructure.Service;

/// <summary>
/// Implements the Unit of Work pattern, coordinating the work of multiple repositories by creating a single database context instance.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly UserTaskContext _context;
    private IUserTask? _userTask;

    // <summary>
    /// Initializes a new instance of the UnitOfWork with a given UserTaskContext.
    /// </summary>
    /// <param name="context">The UserTaskContext instance to be used by the UnitOfWork.</param>
    public UnitOfWork(UserTaskContext context) => 
        _context = context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Gets the UserTask repository instance, creating it if it doesn't already exist.
    /// </summary>
    public IUserTask UserTask => _userTask ??= new UserTaskImplentation(_context, this);

    /// <summary>
    /// Completes the unit of work by saving all changes made in the context to the database.
    /// </summary>
    /// <returns>The number of objects written to the underlying database.</returns>
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default) 
        => await _context.SaveChangesAsync();

    /// <summary>
    /// Disposes the database context asynchronously.
    /// </summary>
    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
