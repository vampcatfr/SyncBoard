using Microsoft.AspNetCore.Mvc;
using SyncBoard.Application.Columns.CreateColumn;
using SyncBoard.Application.Columns.GetColumnsByBoardId;
using SyncBoard.Application.Columns.RenameColumn;
using SyncBoard.Application.Columns.MoveColumn;

namespace SyncBoard.Api.Controllers;

[ApiController]
[Route("api/boards/{boardId:guid}/columns")]
public class ColumnsController : ControllerBase
{
    private readonly CreateColumnHandler _createColumnHandler;
    private readonly GetColumnsByBoardIdHandler _getColumnsByBoardIdHandler;
    private readonly RenameColumnHandler _renameColumnHandler;
    private readonly MoveColumnHandler _moveColumnHandler;

    public ColumnsController(CreateColumnHandler createColumnHandler, GetColumnsByBoardIdHandler getColumnsByBoardIdHandler, RenameColumnHandler renameColumnHandler, MoveColumnHandler moveColumnHandler)
    {
        _createColumnHandler = createColumnHandler;
        _getColumnsByBoardIdHandler = getColumnsByBoardIdHandler;
        _renameColumnHandler = renameColumnHandler;
        _moveColumnHandler = moveColumnHandler;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        Guid boardId,
        CreateColumnRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateColumnCommand(
            boardId,
            request.Title,
            request.Position);

        var columnId = await _createColumnHandler.HandleAsync(
            command,
            cancellationToken);

        if (columnId is null)
        {
            return NotFound();
        }

        return Created(
            $"/api/boards/{boardId}/columns/{columnId}",
            columnId);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetColumnsByBoardIdResult>>> GetByBoardId(
    Guid boardId,
    CancellationToken cancellationToken)
    {
        var query = new GetColumnsByBoardIdQuery(boardId);

        var result = await _getColumnsByBoardIdHandler.HandleAsync(
            query,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPatch("{columnId:guid}")]
    public async Task<IActionResult> Rename(
    Guid boardId,
    Guid columnId,
    RenameColumnRequest request,
    CancellationToken cancellationToken)
    {
        var command = new RenameColumnCommand(
            boardId,
            columnId,
            request.Title);

        var renamed = await _renameColumnHandler.HandleAsync(
            command,
            cancellationToken);

        if (!renamed)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpPatch("{columnId:guid}/position")]
    public async Task<IActionResult> Move(
    Guid boardId,
    Guid columnId,
    MoveColumnRequest request,
    CancellationToken cancellationToken)
    {
        var command = new MoveColumnCommand(
            boardId,
            columnId,
            request.Position);

        var moved = await _moveColumnHandler.HandleAsync(
            command,
            cancellationToken);

        if (!moved)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public sealed record CreateColumnRequest(
    string Title,
    int Position);
public sealed record RenameColumnRequest(string Title);
public sealed record MoveColumnRequest(int Position);