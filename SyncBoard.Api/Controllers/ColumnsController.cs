using Microsoft.AspNetCore.Mvc;
using SyncBoard.Application.Columns.CreateColumn;
using SyncBoard.Application.Columns.GetColumnsByBoardId;
using SyncBoard.Application.Columns.RenameColumn;
using SyncBoard.Application.Columns.MoveColumn;
using SyncBoard.Application.Columns.DeleteColumn;
using SyncBoard.Application.Common.Results;

namespace SyncBoard.Api.Controllers;

[ApiController]
[Route("api/boards/{boardId:guid}/columns")]
public class ColumnsController : ControllerBase
{
    private readonly CreateColumnHandler _createColumnHandler;
    private readonly GetColumnsByBoardIdHandler _getColumnsByBoardIdHandler;
    private readonly RenameColumnHandler _renameColumnHandler;
    private readonly MoveColumnHandler _moveColumnHandler;
    private readonly DeleteColumnHandler _deleteColumnHandler;

    public ColumnsController(
        CreateColumnHandler createColumnHandler, 
        GetColumnsByBoardIdHandler getColumnsByBoardIdHandler, 
        RenameColumnHandler renameColumnHandler, 
        MoveColumnHandler moveColumnHandler, 
        DeleteColumnHandler deleteColumnHandler)
    {
        _createColumnHandler = createColumnHandler;
        _getColumnsByBoardIdHandler = getColumnsByBoardIdHandler;
        _renameColumnHandler = renameColumnHandler;
        _moveColumnHandler = moveColumnHandler;
        _deleteColumnHandler = deleteColumnHandler;
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

        var result = await _createColumnHandler.HandleAsync(
            command,
            cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }

        return Created(
            $"/api/boards/{boardId}/columns/{result.Value}",
            result.Value);
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

        if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
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

        var result = await _renameColumnHandler.HandleAsync(
            command,
            cancellationToken);

        if (result.Status == ResultStatus.NotFound)
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

        var result = await _moveColumnHandler.HandleAsync(
            command,
            cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            return NotFound();
        }

        if (result.Status == ResultStatus.ValidationError)
        {
            return BadRequest();
        }

        return NoContent();
    }
    
    [HttpDelete("{columnId:guid}")]
    public async Task<IActionResult> Delete(
    Guid boardId,
    Guid columnId,
    CancellationToken cancellationToken)
    {
        var command = new DeleteColumnCommand(
            boardId,
            columnId);

        var result = await _deleteColumnHandler.HandleAsync(
            command,
            cancellationToken);

        if (result.Status == ResultStatus.NotFound)
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