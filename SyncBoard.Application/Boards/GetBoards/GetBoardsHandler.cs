using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Boards.GetBoards;

public class GetBoardsHandler
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardsHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<IReadOnlyCollection<GetBoardsResult>>> HandleAsync(
        GetBoardsQuery query,
        CancellationToken cancellationToken = default)
    {
        var boards = await _boardRepository.GetAllAsync(
            cancellationToken);

        var result = boards
            .Select(board => new GetBoardsResult(
                board.Id,
                board.Title,
                board.CreatedAt))
            .ToList();

        return Result<IReadOnlyCollection<GetBoardsResult>>
            .Success(result);
    }
}