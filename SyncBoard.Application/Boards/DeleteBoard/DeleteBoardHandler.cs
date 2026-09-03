using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Boards.DeleteBoard;

public class DeleteBoardHandler
{
    private readonly IBoardRepository _boardRepository;

    public DeleteBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<bool> HandleAsync(
        DeleteBoardCommand command,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            command.BoardId,
            cancellationToken);

        if (board is null)
        {
            return false;
        }

        await _boardRepository.DeleteAsync(
            board,
            cancellationToken);

        return true;
    }
}