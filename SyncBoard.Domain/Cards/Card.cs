using SyncBoard.Domain.Columns;
using SyncBoard.Domain.Common.Exceptions;

namespace SyncBoard.Domain.Cards;

public class Card
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public int Position { get; private set; }
    public Guid ColumnId { get; private set; }
    public Column Column { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }

    public Card(Guid columnId, string title, int position)
    {
        if (columnId == Guid.Empty)
        {
            throw new DomainValidationException(
                "Column id is required.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException(
                "Card title cannot be empty.");
        }

        if (position < 0)
        {
            throw new DomainValidationException(
                "Card position cannot be negative.");
        }

        Id = Guid.NewGuid();
        ColumnId = columnId;
        Title = title;
        Position = position;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Rename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new DomainValidationException(
                "Card title cannot be empty.");
        }

        Title = newTitle;
    }

    public void MoveTo(
        Guid newColumnId,
        int newPosition)
    {
        if (newColumnId == Guid.Empty)
        {
            throw new DomainValidationException(
                "Column id is required.");
        }

        if (newPosition < 0)
        {
            throw new DomainValidationException(
                "Card position cannot be negative.");
        }

        ColumnId = newColumnId;
        Position = newPosition;
    }
}