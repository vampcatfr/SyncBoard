namespace SyncBoard.Application.Columns.DeleteColumn;

public sealed record DeleteColumnCommand(
    Guid BoardId,
    Guid ColumnId);