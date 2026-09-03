using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Boards.GetBoardById;

public class GetBoardByIdHandler
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardByIdHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<GetBoardByIdResult?> HandleAsync(
        GetBoardByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            query.Id,
            cancellationToken);

        if (board is null)
        {
            return null;
        }

        return new GetBoardByIdResult(
            board.Id,
            board.Title,
            board.CreatedAt);
    }
}