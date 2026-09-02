
using SyncBoard.Domain.Columns;

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
            throw new ArgumentException(
                "Column id is required.",
                nameof(columnId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Card title cannot be empty.",
                nameof(title));
        }

        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(position));
        }

        Id = Guid.NewGuid();
        ColumnId = columnId;
        Title = title;
        Position = position;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
