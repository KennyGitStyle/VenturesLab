using Infrastructure.Service;

namespace Infrastructure.Service;

public interface IUnitOfWork : IAsyncDisposable
{
    IUserTask UserTask { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
