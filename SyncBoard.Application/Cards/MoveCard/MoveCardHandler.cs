using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Cards.MoveCard;

public class MoveCardHandler
{
    private readonly ICardRepository _cardRepository;
    private readonly IColumnRepository _columnRepository;

    public MoveCardHandler(
        ICardRepository cardRepository,
        IColumnRepository columnRepository)
    {
        _cardRepository = cardRepository;
        _columnRepository = columnRepository;
    }

    public async Task<Result> HandleAsync(
        MoveCardCommand command,
        CancellationToken cancellationToken = default)
    {
        var card = await _cardRepository.GetByIdAsync(
            command.CardId,
            cancellationToken);

        if (card is null)
        {
            return Result.NotFound();
        }

        if (card.ColumnId != command.SourceColumnId)
        {
            return Result.NotFound();
        }

        var sourceColumn = await _columnRepository.GetByIdAsync(
            command.SourceColumnId,
            cancellationToken);

        if (sourceColumn is null)
        {
            return Result.NotFound();
        }

        var targetColumn = await _columnRepository.GetByIdAsync(
            command.TargetColumnId,
            cancellationToken);

        if (targetColumn is null)
        {
            return Result.NotFound();
        }

        if (sourceColumn.BoardId != targetColumn.BoardId)
        {
            return Result.Conflict();
        }

        if (command.NewPosition < 0)
        {
            return Result.ValidationError();
        }

        if (command.SourceColumnId == command.TargetColumnId)
        {
            var cards = (await _cardRepository.GetByColumnIdForUpdateAsync(
                command.SourceColumnId,
                cancellationToken))
                .Where(x => x.Id != card.Id)
                .ToList();

            var newPosition = Math.Min(
                command.NewPosition,
                cards.Count);

            cards.Insert(newPosition, card);

            for (var i = 0; i < cards.Count; i++)
            {
                cards[i].MoveTo(
                    command.SourceColumnId,
                    i);
            }
        }
        else
        {
            var sourceCards = (await _cardRepository.GetByColumnIdForUpdateAsync(
                command.SourceColumnId,
                cancellationToken))
                .Where(x => x.Id != card.Id)
                .ToList();

            for (var i = 0; i < sourceCards.Count; i++)
            {
                sourceCards[i].MoveTo(
                    command.SourceColumnId,
                    i);
            }

            var targetCards = (await _cardRepository.GetByColumnIdForUpdateAsync(
                command.TargetColumnId,
                cancellationToken))
                .ToList();

            var newPosition = Math.Min(
                command.NewPosition,
                targetCards.Count);

            targetCards.Insert(newPosition, card);

            for (var i = 0; i < targetCards.Count; i++)
            {
                targetCards[i].MoveTo(
                    command.TargetColumnId,
                    i);
            }
        }

        await _cardRepository.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}