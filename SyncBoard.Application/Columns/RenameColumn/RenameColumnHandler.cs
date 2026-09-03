using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Columns.RenameColumn;

public class RenameColumnHandler
{
    private readonly IColumnRepository _columnRepository;

    public RenameColumnHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<bool> HandleAsync(
        RenameColumnCommand command,
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

        column.Rename(command.NewTitle);

        await _columnRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}