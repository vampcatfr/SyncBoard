using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Columns.GetColumnsByBoardId;

public class GetColumnsByBoardIdHandler
{
    private readonly IBoardRepository _boardRepository;
    private readonly IColumnRepository _columnRepository;

    public GetColumnsByBoardIdHandler(
        IBoardRepository boardRepository,
        IColumnRepository columnRepository)
    {
        _boardRepository = boardRepository;
        _columnRepository = columnRepository;
    }

    public async Task<IReadOnlyCollection<GetColumnsByBoardIdResult>?> HandleAsync(
        GetColumnsByBoardIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            query.BoardId,
            cancellationToken);

        if (board is null)
        {
            return null;
        }

        var columns = await _columnRepository.GetByBoardIdAsync(
            query.BoardId,
            cancellationToken);

        return columns
            .Select(column => new GetColumnsByBoardIdResult(
                column.Id,
                column.Title,
                column.Position))
            .ToList();
    }
}