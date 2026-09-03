using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Boards.GetBoards;

public class GetBoardsHandler
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardsHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<IReadOnlyCollection<GetBoardsResult>> HandleAsync(
        GetBoardsQuery query,
        CancellationToken cancellationToken = default)
    {
        var boards = await _boardRepository.GetAllAsync(
            cancellationToken);

        return boards
            .Select(board => new GetBoardsResult(
                board.Id,
                board.Title,
                board.CreatedAt))
            .ToList();
    }
}