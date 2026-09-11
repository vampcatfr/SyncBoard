using SyncBoard.Application.Common.Persistence;
using SyncBoard.Domain.Cards;

namespace SyncBoard.Application.Cards.CreateCard;

public class CreateCardHandler
{
    private readonly IColumnRepository _columnRepository;
    private readonly ICardRepository _cardRepository;

    public CreateCardHandler(
        IColumnRepository columnRepository,
        ICardRepository cardRepository)
    {
        _columnRepository = columnRepository;
        _cardRepository = cardRepository;
    }

    public async Task<Guid?> HandleAsync(
        CreateCardCommand command,
        CancellationToken cancellationToken = default)
    {
        var column = await _columnRepository.GetByIdAsync(
            command.ColumnId,
            cancellationToken);

        if (column is null)
        {
            return null;
        }

        var card = new Card(
            command.ColumnId,
            command.Title,
            command.Position);

        await _cardRepository.AddAsync(
            card,
            cancellationToken);

        return card.Id;
    }
}