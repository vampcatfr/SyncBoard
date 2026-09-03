namespace SyncBoard.Application.Columns.MoveColumn;

public sealed record MoveColumnCommand(
    Guid BoardId,
    Guid ColumnId,
    int NewPosition);