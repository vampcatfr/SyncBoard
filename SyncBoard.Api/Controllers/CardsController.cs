using Microsoft.AspNetCore.Mvc;
using SyncBoard.Application.Cards.CreateCard;
using SyncBoard.Application.Cards.GetCardsByColumnId;
using SyncBoard.Application.Cards.RenameCard;
using SyncBoard.Application.Cards.MoveCard;
using SyncBoard.Application.Cards.DeleteCard;

namespace SyncBoard.Api.Controllers;

[ApiController]
[Route("api/columns/{columnId:guid}/cards")]
public class CardsController : ControllerBase
{
    private readonly CreateCardHandler _createCardHandler;
    private readonly GetCardsByColumnIdHandler _getCardsByColumnIdHandler;
    private readonly RenameCardHandler _renameCardHandler;
    private readonly MoveCardHandler _moveCardHandler;
    private readonly DeleteCardHandler _deleteCardHandler;

    public CardsController(
        CreateCardHandler createCardHandler,
        GetCardsByColumnIdHandler getCardsByColumnIdHandler,
        RenameCardHandler renameCardHandler,
        MoveCardHandler moveCardHandler,
        DeleteCardHandler deleteCardHandler)
    {
        _createCardHandler = createCardHandler;
        _getCardsByColumnIdHandler = getCardsByColumnIdHandler;
        _renameCardHandler = renameCardHandler;
        _moveCardHandler = moveCardHandler;
        _deleteCardHandler = deleteCardHandler;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        Guid columnId,
        CreateCardRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCardCommand(
            columnId,
            request.Title,
            request.Position);

        var cardId = await _createCardHandler.HandleAsync(
            command,
            cancellationToken);

        if (cardId is null)
        {
            return NotFound();
        }

        return Created(
            $"/api/columns/{columnId}/cards/{cardId}",
            cardId);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetCardsByColumnIdResult>>> GetByColumnId(
    Guid columnId,
    CancellationToken cancellationToken)
    {
        var query = new GetCardsByColumnIdQuery(columnId);

        var cards = await _getCardsByColumnIdHandler.HandleAsync(
            query,
            cancellationToken);

        if (cards is null)
        {
            return NotFound();
        }

        return Ok(cards);
    }

    [HttpPatch("{cardId:guid}")]
    public async Task<IActionResult> Rename(
    Guid columnId,
    Guid cardId,
    RenameCardRequest request,
    CancellationToken cancellationToken)
    {
        var command = new RenameCardCommand(
            columnId,
            cardId,
            request.Title);

        var renamed = await _renameCardHandler.HandleAsync(
            command,
            cancellationToken);

        if (!renamed)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpPatch("{cardId:guid}/move")]
    public async Task<IActionResult> Move(
    Guid columnId,
    Guid cardId,
    MoveCardRequest request,
    CancellationToken cancellationToken)
    {
        var command = new MoveCardCommand(
            cardId,
            columnId,
            request.TargetColumnId,
            request.Position);

        var moved = await _moveCardHandler.HandleAsync(
            command,
            cancellationToken);

        if (!moved)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{cardId:guid}")]
    public async Task<IActionResult> Delete(
    Guid columnId,
    Guid cardId,
    CancellationToken cancellationToken)
    {
        var command = new DeleteCardCommand(
            columnId,
            cardId);

        var deleted = await _deleteCardHandler.HandleAsync(
            command,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public sealed record CreateCardRequest(
    string Title,
    int Position);
public sealed record RenameCardRequest(string Title);
public sealed record MoveCardRequest(
    Guid TargetColumnId,
    int Position);