using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Columns.DeleteColumn;

public class DeleteColumnHandler
{
    private readonly IColumnRepository _columnRepository;

    public DeleteColumnHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<bool> HandleAsync(
        DeleteColumnCommand command,
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

        await _columnRepository.DeleteAsync(
            column,
            cancellationToken);

        return true;
    }
}