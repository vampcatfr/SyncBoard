namespace SyncBoard.Application.Boards.GetBoards;

public sealed record GetBoardsResult(
    Guid Id,
    string Title,
    DateTimeOffset CreatedAt);