namespace SyncBoard.Application.Boards.GetBoardById;

public sealed record GetBoardByIdResult(
    Guid Id,
    string Title,
    DateTimeOffset CreatedAt);