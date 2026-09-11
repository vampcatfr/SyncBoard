using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Cards.RenameCard;

public class RenameCardHandler
{
    private readonly ICardRepository _cardRepository;

    public RenameCardHandler(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public async Task<bool> HandleAsync(
        RenameCardCommand command,
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

        card.Rename(command.NewTitle);

        await _cardRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}