using Moq;
using SyncBoard.Application.Cards.MoveCard;
using SyncBoard.Application.Common.Persistence;
using SyncBoard.Application.Common.Results;
using SyncBoard.Domain.Cards;

namespace SyncBoard.Application.Tests.Cards.MoveCard;

public class MoveCardHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenCardDoesNotExist_ReturnsNotFound()
    {
        var cardRepository = new Mock<ICardRepository>();
        var columnRepository = new Mock<IColumnRepository>();

        cardRepository
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Card?)null);

        var handler = new MoveCardHandler(
            cardRepository.Object,
            columnRepository.Object);

        var command = new MoveCardCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            0);

        var result = await handler.HandleAsync(command);

        Assert.Equal(
            ResultStatus.NotFound,
            result.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenMovingInsideSameColumn_ReordersCards()
    {
        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();

        var cardA = new Card(columnId, "A", 0);
        var cardB = new Card(columnId, "B", 1);
        var cardC = new Card(columnId, "C", 2);

        var column = new SyncBoard.Domain.Columns.Column(
            boardId,
            "Backlog",
            0);

        var cardRepository = new Mock<ICardRepository>();
        var columnRepository = new Mock<IColumnRepository>();

        cardRepository
            .Setup(x => x.GetByIdAsync(
                cardC.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cardC);

        columnRepository
            .Setup(x => x.GetByIdAsync(
                columnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(column);

        cardRepository
            .Setup(x => x.GetByColumnIdForUpdateAsync(
                columnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Card>
            {
            cardA,
            cardB,
            cardC
            });

        var handler = new MoveCardHandler(
            cardRepository.Object,
            columnRepository.Object);

        var command = new MoveCardCommand(
            cardC.Id,
            columnId,
            columnId,
            0);

        var result = await handler.HandleAsync(command);

        Assert.Equal(
            ResultStatus.Success,
            result.Status);

        Assert.Equal(1, cardA.Position);
        Assert.Equal(2, cardB.Position);
        Assert.Equal(0, cardC.Position);
    }

    [Fact]
    public async Task HandleAsync_WhenMovingToAnotherColumn_ReordersBothColumns()
    {
        var boardId = Guid.NewGuid();

        var sourceColumnId = Guid.NewGuid();
        var targetColumnId = Guid.NewGuid();

        var sourceColumn = new SyncBoard.Domain.Columns.Column(
            boardId,
            "Backlog",
            0);

        var targetColumn = new SyncBoard.Domain.Columns.Column(
            boardId,
            "In Progress",
            1);

        var cardA = new Card(sourceColumnId, "A", 0);
        var cardB = new Card(sourceColumnId, "B", 1);
        var cardC = new Card(targetColumnId, "C", 0);

        var cardRepository = new Mock<ICardRepository>();
        var columnRepository = new Mock<IColumnRepository>();

        cardRepository
            .Setup(x => x.GetByIdAsync(
                cardB.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cardB);

        columnRepository
            .Setup(x => x.GetByIdAsync(
                sourceColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceColumn);

        columnRepository
            .Setup(x => x.GetByIdAsync(
                targetColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(targetColumn);

        cardRepository
            .Setup(x => x.GetByColumnIdForUpdateAsync(
                sourceColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Card>
            {
            cardA,
            cardB
            });

        cardRepository
            .Setup(x => x.GetByColumnIdForUpdateAsync(
                targetColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Card>
            {
            cardC
            });

        var handler = new MoveCardHandler(
            cardRepository.Object,
            columnRepository.Object);

        var command = new MoveCardCommand(
            cardB.Id,
            sourceColumnId,
            targetColumnId,
            0);

        var result = await handler.HandleAsync(command);

        Assert.Equal(
            ResultStatus.Success,
            result.Status);

        Assert.Equal(0, cardA.Position);

        Assert.Equal(targetColumnId, cardB.ColumnId);
        Assert.Equal(0, cardB.Position);

        Assert.Equal(1, cardC.Position);
    }

    [Fact]
    public async Task HandleAsync_WhenMovingCardToColumnFromAnotherBoard_ReturnsConflict()
    {
        var sourceBoardId = Guid.NewGuid();
        var targetBoardId = Guid.NewGuid();

        var sourceColumnId = Guid.NewGuid();
        var targetColumnId = Guid.NewGuid();

        var sourceColumn = new SyncBoard.Domain.Columns.Column(
            sourceBoardId,
            "Backlog",
            0);

        var targetColumn = new SyncBoard.Domain.Columns.Column(
            targetBoardId,
            "In Progress",
            0);

        var card = new Card(
            sourceColumnId,
            "Prepare CV",
            0);

        var cardRepository = new Mock<ICardRepository>();
        var columnRepository = new Mock<IColumnRepository>();

        cardRepository
            .Setup(x => x.GetByIdAsync(
                card.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);

        columnRepository
            .Setup(x => x.GetByIdAsync(
                sourceColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceColumn);

        columnRepository
            .Setup(x => x.GetByIdAsync(
                targetColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(targetColumn);

        var handler = new MoveCardHandler(
            cardRepository.Object,
            columnRepository.Object);

        var command = new MoveCardCommand(
            card.Id,
            sourceColumnId,
            targetColumnId,
            0);

        var result = await handler.HandleAsync(command);

        Assert.Equal(
            ResultStatus.Conflict,
            result.Status);
    }

    [Fact]
    public async Task HandleAsync_WhenNewPositionIsNegative_ReturnsValidationError()
    {
        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();

        var column = new SyncBoard.Domain.Columns.Column(
            boardId,
            "Backlog",
            0);

        var card = new Card(
            columnId,
            "Prepare CV",
            0);

        var cardRepository = new Mock<ICardRepository>();
        var columnRepository = new Mock<IColumnRepository>();

        cardRepository
            .Setup(x => x.GetByIdAsync(
                card.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);

        columnRepository
            .Setup(x => x.GetByIdAsync(
                columnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(column);

        var handler = new MoveCardHandler(
            cardRepository.Object,
            columnRepository.Object);

        var command = new MoveCardCommand(
            card.Id,
            columnId,
            columnId,
            -1);

        var result = await handler.HandleAsync(command);

        Assert.Equal(
            ResultStatus.ValidationError,
            result.Status);
    }
}