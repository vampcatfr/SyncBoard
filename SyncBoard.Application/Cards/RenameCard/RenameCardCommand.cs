namespace SyncBoard.Application.Cards.RenameCard;

public sealed record RenameCardCommand(
    Guid ColumnId,
    Guid CardId,
    string NewTitle);