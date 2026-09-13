using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Cards.RenameCard;

public class RenameCardHandler
{
    private readonly ICardRepository _cardRepository;

    public RenameCardHandler(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public async Task<Result> HandleAsync(
        RenameCardCommand command,
        CancellationToken cancellationToken = default)
    {
        var card = await _cardRepository.GetByIdAsync(
            command.CardId,
            cancellationToken);

        if (card is null)
        {
            return Result.NotFound();
        }

        if (card.ColumnId != command.ColumnId)
        {
            return Result.NotFound();
        }

        card.Rename(command.NewTitle);

        await _cardRepository.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}