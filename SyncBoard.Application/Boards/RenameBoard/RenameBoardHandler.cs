using SyncBoard.Application.Common.Persistence;

namespace SyncBoard.Application.Boards.RenameBoard;

public class RenameBoardHandler
{
    private readonly IBoardRepository _boardRepository;

    public RenameBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<bool> HandleAsync(
        RenameBoardCommand command,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            command.BoardId,
            cancellationToken);

        if (board is null)
        {
            return false;
        }

        board.Rename(command.NewTitle);

        await _boardRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}