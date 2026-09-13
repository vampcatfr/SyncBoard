using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Columns.MoveColumn;

public class MoveColumnHandler
{
    private readonly IColumnRepository _columnRepository;

    public MoveColumnHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<Result> HandleAsync(
        MoveColumnCommand command,
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

        if (command.NewPosition < 0)
        {
            return Result.ValidationError();
        }

        column.MoveTo(command.NewPosition);

        await _columnRepository.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}