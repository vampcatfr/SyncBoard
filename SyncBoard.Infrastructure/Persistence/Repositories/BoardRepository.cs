using Microsoft.EntityFrameworkCore;
using SyncBoard.Application.Common.Persistence;
using SyncBoard.Domain.Boards;

namespace SyncBoard.Infrastructure.Persistence.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _dbContext;

    public BoardRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Board board,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Boards.AddAsync(
            board,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Board?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Boards
            .FirstOrDefaultAsync(
                board => board.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Board>> GetAllAsync(
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.Boards
            .OrderByDescending(board => board.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}