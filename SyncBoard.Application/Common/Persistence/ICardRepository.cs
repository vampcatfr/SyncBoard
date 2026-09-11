using SyncBoard.Domain.Cards;

namespace SyncBoard.Application.Common.Persistence;

public interface ICardRepository
{
    Task AddAsync(
        Card card,
        CancellationToken cancellationToken = default);

    Task<Card?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Card>> GetByColumnIdAsync(
        Guid columnId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Card>> GetByColumnIdForUpdateAsync(
        Guid columnId,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Card card,
        CancellationToken cancellationToken = default);
}