using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Cards.GetCardsByColumnId;

public class GetCardsByColumnIdHandler
{
    private readonly IColumnRepository _columnRepository;
    private readonly ICardRepository _cardRepository;

    public GetCardsByColumnIdHandler(
        IColumnRepository columnRepository,
        ICardRepository cardRepository)
    {
        _columnRepository = columnRepository;
        _cardRepository = cardRepository;
    }

    public async Task<IReadOnlyCollection<GetCardsByColumnIdResult>?> HandleAsync(
        GetCardsByColumnIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var column = await _columnRepository.GetByIdAsync(
            query.ColumnId,
            cancellationToken);

        if (column is null)
        {
            return null;
        }

        var cards = await _cardRepository.GetByColumnIdAsync(
            query.ColumnId,
            cancellationToken);

        return cards
            .Select(card => new GetCardsByColumnIdResult(
                card.Id,
                card.Title,
                card.Description,
                card.Position,
                card.CreatedAt))
            .ToList();
    }
}