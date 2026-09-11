namespace SyncBoard.Application.Cards.DeleteCard;

public sealed record DeleteCardCommand(
    Guid ColumnId,
    Guid CardId);