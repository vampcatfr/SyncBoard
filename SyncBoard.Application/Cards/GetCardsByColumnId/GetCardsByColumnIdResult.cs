namespace SyncBoard.Application.Cards.GetCardsByColumnId;

public sealed record GetCardsByColumnIdResult(
    Guid Id,
    string Title,
    string? Description,
    int Position,
    DateTimeOffset CreatedAt);