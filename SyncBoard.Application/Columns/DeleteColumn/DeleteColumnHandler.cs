using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Columns.DeleteColumn;

public class DeleteColumnHandler
{
    private readonly IColumnRepository _columnRepository;

    public DeleteColumnHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<Result> HandleAsync(
        DeleteColumnCommand command,
        CancellationToken cancellationToken = default)
    {
        var column = await _columnRepository.GetByIdAsync(
            command.ColumnId,
            cancellationToken);

        if (column is null)
        {
            return Result.NotFound();
        }

        if (column.BoardId != command.BoardId)
        {
            return Result.NotFound();
        }

        await _columnRepository.DeleteAsync(
            column,
            cancellationToken);

        return Result.Success();
    }
}