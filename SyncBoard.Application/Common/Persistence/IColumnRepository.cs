using SyncBoard.Domain.Columns;

namespace SyncBoard.Application.Common.Persistence;

public interface IColumnRepository
{
    Task AddAsync(
        Column column,
        CancellationToken cancellationToken = default);

    Task<Column?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Column>> GetByBoardIdAsync(
    Guid boardId,
    CancellationToken cancellationToken = default);
}