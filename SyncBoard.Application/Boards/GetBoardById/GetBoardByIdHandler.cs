using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Boards.GetBoardById;

public class GetBoardByIdHandler
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardByIdHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<GetBoardByIdResult>> HandleAsync(
        GetBoardByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            query.Id,
            cancellationToken);

        if (board is null)
        {
            return Result<GetBoardByIdResult>.NotFound();
        }

        var result = new GetBoardByIdResult(
            board.Id,
            board.Title,
            board.CreatedAt);

        return Result<GetBoardByIdResult>.Success(result);
    }
}