using Microsoft.AspNetCore.Mvc;
using SyncBoard.Application.Boards.CreateBoard;

namespace SyncBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly CreateBoardHandler _createBoardHandler;

    public BoardsController(CreateBoardHandler createBoardHandler)
    {
        _createBoardHandler = createBoardHandler;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        CreateBoardRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBoardCommand(request.Title);

        var boardId = await _createBoardHandler.HandleAsync(
            command,
            cancellationToken);

        return Created($"/api/boards/{boardId}", boardId);
    }
}

public sealed record CreateBoardRequest(string Title);