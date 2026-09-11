using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Cards.DeleteCard;

public class DeleteCardHandler
{
    private readonly ICardRepository _cardRepository;

    public DeleteCardHandler(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public async Task<bool> HandleAsync(
        DeleteCardCommand command,
        CancellationToken cancellationToken = default)
    {
        var card = await _cardRepository.GetByIdAsync(
            command.CardId,
            cancellationToken);

        if (card is null)
        {
            return false;
        }

        if (card.ColumnId != command.ColumnId)
        {
            return false;
        }

        await _cardRepository.DeleteAsync(
            card,
            cancellationToken);

        return true;
    }
}