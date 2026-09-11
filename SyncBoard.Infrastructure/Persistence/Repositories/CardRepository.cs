using Microsoft.EntityFrameworkCore;
using SyncBoard.Application.Common.Persistence;
using SyncBoard.Domain.Cards;

namespace SyncBoard.Infrastructure.Persistence.Repositories;

public class CardRepository : ICardRepository
{
    private readonly AppDbContext _dbContext;

    public CardRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Card card,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Cards.AddAsync(
            card,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Card?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cards
            .FirstOrDefaultAsync(
                card => card.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Card>> GetByColumnIdAsync(
        Guid columnId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cards
            .AsNoTracking()
            .Where(card => card.ColumnId == columnId)
            .OrderBy(card => card.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Card>> GetByColumnIdForUpdateAsync(
        Guid columnId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cards
            .Where(card => card.ColumnId == columnId)
            .OrderBy(card => card.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Card card,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Cards.Remove(card);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}