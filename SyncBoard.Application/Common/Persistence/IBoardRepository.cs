using SyncBoard.Domain.Boards;

namespace SyncBoard.Application.Common.Persistence;

public interface IBoardRepository
{
    Task AddAsync(
        Board board,
        CancellationToken cancellationToken = default);

    Task<Board?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Board>> GetAllAsync(
    CancellationToken cancellationToken = default);

}