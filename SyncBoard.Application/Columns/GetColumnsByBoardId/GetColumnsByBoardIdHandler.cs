using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;


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

    public async Task<Result<IReadOnlyCollection<GetColumnsByBoardIdResult>>> HandleAsync(
        GetColumnsByBoardIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            query.BoardId,
            cancellationToken);

        if (board is null)
        {
            return Result<IReadOnlyCollection<GetColumnsByBoardIdResult>>
                .NotFound();
        }

        var columns = await _columnRepository.GetByBoardIdAsync(
            query.BoardId,
            cancellationToken);

        var result = columns
            .Select(column => new GetColumnsByBoardIdResult(
                column.Id,
                column.Title,
                column.Position))
            .ToList();

        return Result<IReadOnlyCollection<GetColumnsByBoardIdResult>>
            .Success(result);
    }
}