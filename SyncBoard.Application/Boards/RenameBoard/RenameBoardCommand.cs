namespace SyncBoard.Application.Boards.RenameBoard;

public sealed record RenameBoardCommand(
    Guid BoardId,
    string NewTitle);