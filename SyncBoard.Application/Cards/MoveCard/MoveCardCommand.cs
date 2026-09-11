namespace SyncBoard.Application.Cards.MoveCard;

public sealed record MoveCardCommand(
    Guid CardId,
    Guid SourceColumnId,
    Guid TargetColumnId,
    int NewPosition);