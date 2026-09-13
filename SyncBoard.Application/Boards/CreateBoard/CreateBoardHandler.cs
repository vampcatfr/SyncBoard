using SyncBoard.Application.Common.Persistence;
using SyncBoard.Domain.Boards;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Application.Boards.CreateBoard;

public class CreateBoardHandler
{
    private readonly IBoardRepository _boardRepository;

    public CreateBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<Guid>> HandleAsync(
        CreateBoardCommand command,
        CancellationToken cancellationToken = default)
    {
        var board = new Board(command.Title);

        await _boardRepository.AddAsync(
            board,
            cancellationToken);

        return Result<Guid>.Success(board.Id);
    }
}