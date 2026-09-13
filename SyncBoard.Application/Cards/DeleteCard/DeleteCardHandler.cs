using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Cards.DeleteCard;

public class DeleteCardHandler
{
    private readonly ICardRepository _cardRepository;

    public DeleteCardHandler(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public async Task<Result> HandleAsync(
        DeleteCardCommand command,
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

        await _cardRepository.DeleteAsync(
            card,
            cancellationToken);

        return Result.Success();
    }
}