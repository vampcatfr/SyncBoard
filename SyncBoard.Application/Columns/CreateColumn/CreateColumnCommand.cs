namespace SyncBoard.Application.Columns.CreateColumn;

public sealed record CreateColumnCommand(
    Guid BoardId,
    string Title,
    int Position);