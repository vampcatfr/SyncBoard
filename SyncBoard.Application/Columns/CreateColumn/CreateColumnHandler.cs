using SyncBoard.Application.Common.Persistence;
using SyncBoard.Domain.Columns;

namespace SyncBoard.Application.Columns.CreateColumn;

public class CreateColumnHandler
{
    private readonly IBoardRepository _boardRepository;
    private readonly IColumnRepository _columnRepository;

    public CreateColumnHandler(
        IBoardRepository boardRepository,
        IColumnRepository columnRepository)
    {
        _boardRepository = boardRepository;
        _columnRepository = columnRepository;
    }

    public async Task<Guid?> HandleAsync(
        CreateColumnCommand command,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            command.BoardId,
            cancellationToken);

        if (board is null)
        {
            return null;
        }

        var column = new Column(
            command.BoardId,
            command.Title,
            command.Position);

        await _columnRepository.AddAsync(
            column,
            cancellationToken);

        return column.Id;
    }
}