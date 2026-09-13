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

public sealed class Result<T>
{
    public ResultStatus Status { get; }

    public T? Value { get; }

    public bool IsSuccess =>
        Status == ResultStatus.Success;

    private Result(
        ResultStatus status,
        T? value = default)
    {
        Status = status;
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(
            ResultStatus.Success,
            value);
    }

    public static Result<T> NotFound()
    {
        return new Result<T>(
            ResultStatus.NotFound);
    }

    public static Result<T> ValidationError()
    {
        return new Result<T>(
            ResultStatus.ValidationError);
    }

    public static Result<T> Conflict()
    {
        return new Result<T>(
            ResultStatus.Conflict);
    }
}