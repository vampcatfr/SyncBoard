namespace SyncBoard.Application.Common.Results;

public enum ResultStatus
{
    Success,
    NotFound,
    ValidationError,
    Conflict
}

public sealed class Result
{
    public ResultStatus Status { get; }

    public bool IsSuccess =>
        Status == ResultStatus.Success;

    private Result(ResultStatus status)
    {
        Status = status;
    }

    public static Result Success()
    {
        return new Result(ResultStatus.Success);
    }

    public static Result NotFound()
    {
        return new Result(ResultStatus.NotFound);
    }

    public static Result ValidationError()
    {
        return new Result(ResultStatus.ValidationError);
    }

    public static Result Conflict()
    {
        return new Result(ResultStatus.Conflict);
    }
}