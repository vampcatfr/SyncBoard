using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Columns.MoveColumn;

public class MoveColumnHandler
{
    private readonly IColumnRepository _columnRepository;

    public MoveColumnHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<bool> HandleAsync(
        MoveColumnCommand command,
        CancellationToken cancellationToken = default)
    {
        var column = await _columnRepository.GetByIdAsync(
            command.ColumnId,
            cancellationToken);

        if (column is null)
        {
            return false;
        }

        if (column.BoardId != command.BoardId)
        {
            return false;
        }

        column.MoveTo(command.NewPosition);

        await _columnRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}