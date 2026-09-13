using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Boards.RenameBoard;

public class RenameBoardHandler
{
    private readonly IBoardRepository _boardRepository;

    public RenameBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result> HandleAsync(
        RenameBoardCommand command,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(
            command.BoardId,
            cancellationToken);

        if (board is null)
        {
            return Result.NotFound();
        }

        board.Rename(command.NewTitle);

        await _boardRepository.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}