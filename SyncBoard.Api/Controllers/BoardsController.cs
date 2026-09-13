using Microsoft.AspNetCore.Mvc;
using SyncBoard.Application.Boards.CreateBoard;
using SyncBoard.Application.Boards.DeleteBoard;
using SyncBoard.Application.Boards.GetBoardById;
using SyncBoard.Application.Boards.GetBoards;
using SyncBoard.Application.Boards.RenameBoard;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly CreateBoardHandler _createBoardHandler;
    private readonly GetBoardByIdHandler _getBoardByIdHandler;
    private readonly GetBoardsHandler _getBoardsHandler;
    private readonly RenameBoardHandler _renameBoardHandler;
    private readonly DeleteBoardHandler _deleteBoardHandler;

    public BoardsController(
        CreateBoardHandler createBoardHandler,
        GetBoardByIdHandler getBoardByIdHandler,
        GetBoardsHandler getBoardsHandler,
        RenameBoardHandler renameBoardHandler,
        DeleteBoardHandler deleteBoardHandler)
    {
        _createBoardHandler = createBoardHandler;
        _getBoardByIdHandler = getBoardByIdHandler;
        _getBoardsHandler = getBoardsHandler;
        _renameBoardHandler = renameBoardHandler;
        _deleteBoardHandler = deleteBoardHandler;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        CreateBoardRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBoardCommand(
            request.Title);

        var result = await _createBoardHandler.HandleAsync(
            command,
            cancellationToken);

        return Created(
            $"/api/boards/{result.Value}",
            result.Value);
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

        if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetBoardsResult>>> GetAll(
        CancellationToken cancellationToken)
    {
        var query = new GetBoardsQuery();

        var result = await _getBoardsHandler.HandleAsync(
            query,
            cancellationToken);

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Rename(
        Guid id,
        RenameBoardRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RenameBoardCommand(
            id,
            request.Title);

        var result = await _renameBoardHandler.HandleAsync(
            command,
            cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBoardCommand(id);

        var result = await _deleteBoardHandler.HandleAsync(
            command,
            cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public sealed record CreateBoardRequest(
    string Title);

public sealed record RenameBoardRequest(
    string Title);