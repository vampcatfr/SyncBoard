using SyncBoard.Domain.Columns;

namespace SyncBoard.Domain.Boards;

public class Board
{
    private readonly List<Column> _columns = [];

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<Column> Columns => _columns;

    public Board(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Board title cannot be empty.",
                nameof(title));
        }

        Id = Guid.NewGuid();
        Title = title;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Rename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException(
                "Board title cannot be empty.",
                nameof(newTitle));
        }

        Title = newTitle;
    }
}