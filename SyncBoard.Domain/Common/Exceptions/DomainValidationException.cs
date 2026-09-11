namespace SyncBoard.Domain.Common.Exceptions;

public sealed class DomainValidationException : Exception
{
    public DomainValidationException(string message)
        : base(message)
    {
    }
}