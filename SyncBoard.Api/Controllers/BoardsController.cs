using Microsoft.AspNetCore.Mvc;
using SyncBoard.Application.Boards.CreateBoard;
using SyncBoard.Application.Boards.GetBoardById;
using SyncBoard.Application.Boards.GetBoards;

namespace SyncBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly CreateBoardHandler _createBoardHandler;
    private readonly GetBoardByIdHandler _getBoardByIdHandler;
    private readonly GetBoardsHandler _getBoardsHandler;

    public BoardsController(
        CreateBoardHandler createBoardHandler,
        GetBoardByIdHandler getBoardByIdHandler,
        GetBoardsHandler getBoardsHandler)
    {
        _createBoardHandler = createBoardHandler;
        _getBoardByIdHandler = getBoardByIdHandler;
        _getBoardsHandler = getBoardsHandler;
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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetBoardByIdResult>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetBoardByIdQuery(id);

        var result = await _getBoardByIdHandler.HandleAsync(
            query,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetBoardsResult>>> GetAll(
    CancellationToken cancellationToken)
    {
        var query = new GetBoardsQuery();

        var result = await _getBoardsHandler.HandleAsync(
            query,
            cancellationToken);

        return Ok(result);
    }
}

public sealed record CreateBoardRequest(string Title);