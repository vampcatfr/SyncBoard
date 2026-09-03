namespace SyncBoard.Application.Columns.GetColumnsByBoardId;

public sealed record GetColumnsByBoardIdResult(
    Guid Id,
    string Title,
    int Position);