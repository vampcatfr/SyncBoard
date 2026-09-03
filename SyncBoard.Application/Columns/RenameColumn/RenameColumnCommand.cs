namespace SyncBoard.Application.Columns.RenameColumn;

public sealed record RenameColumnCommand(
    Guid BoardId,
    Guid ColumnId,
    string NewTitle);