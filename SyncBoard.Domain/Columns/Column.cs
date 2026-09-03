using SyncBoard.Domain.Boards;
using SyncBoard.Domain.Cards;

namespace SyncBoard.Domain.Columns;

public class Column
{
    private readonly List<Card> _cards = [];

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public int Position { get; private set; }

    public Guid BoardId { get; private set; }

    public Board Board { get; private set; } = null!;

    public IReadOnlyCollection<Card> Cards => _cards;

    public Column(Guid boardId, string title, int position)
    {
        if (boardId == Guid.Empty)
        {
            throw new ArgumentException(
                "Board id is required.",
                nameof(boardId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Column title cannot be empty.",
                nameof(title));
        }

        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(position));
        }

        Id = Guid.NewGuid();
        BoardId = boardId;
        Title = title;
        Position = position;
    }
    public void Rename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException(
                "Column title cannot be empty.",
                nameof(newTitle));
        }

        Title = newTitle;
    }
    public void MoveTo(int newPosition)
    {
        if (newPosition < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(newPosition));
        }

        Position = newPosition;
    }
}