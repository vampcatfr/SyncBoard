using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Boards.DeleteBoard;

public class DeleteBoardHandler
{
    private readonly IBoardRepository _boardRepository;

    public DeleteBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result> HandleAsync(
        DeleteBoardCommand command,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            command.BoardId,
            cancellationToken);

        if (board is null)
        {
            return Result.NotFound();
        }

        await _boardRepository.DeleteAsync(
            board,
            cancellationToken);

        return Result.Success();
    }
}