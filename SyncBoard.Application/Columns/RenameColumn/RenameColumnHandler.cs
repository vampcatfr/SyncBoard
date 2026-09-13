using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Columns.RenameColumn;

public class RenameColumnHandler
{
    private readonly IColumnRepository _columnRepository;

    public RenameColumnHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<Result> HandleAsync(
        RenameColumnCommand command,
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

        column.Rename(command.NewTitle);

        await _columnRepository.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}