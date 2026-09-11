namespace SyncBoard.Application.Cards.CreateCard;

public sealed record CreateCardCommand(
    Guid ColumnId,
    string Title,
    int Position);