using Microsoft.EntityFrameworkCore;
using SyncBoard.Application.Common.Persistence;
using SyncBoard.Domain.Columns;

namespace SyncBoard.Infrastructure.Persistence.Repositories;

public class ColumnRepository : IColumnRepository
{
    private readonly AppDbContext _dbContext;

    public ColumnRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Column column,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Columns.AddAsync(
            column,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Column?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Columns
            .FirstOrDefaultAsync(
                column => column.Id == id,
                cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Column>> GetByBoardIdAsync(
    Guid boardId,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.Columns
            .Where(column => column.BoardId == boardId)
            .OrderBy(column => column.Position)
            .ToListAsync(cancellationToken);
    }
}
