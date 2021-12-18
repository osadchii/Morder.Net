namespace Infrastructure.Common;

[Serializable]
public class Result
{
    public ResultCode Status { get; set; }

    public bool IsSucceeded => Status == ResultCode.Success;

    public List<string> Errors;

    public static Result Ok => new();

    public Result()
    {
        Status = ResultCode.Success;
    }

    public Result(ResultCode status)
    {
        Status = status;
    }

    public Result(Exception exception)
    {
        Status = ResultCode.Error;
        Errors = new List<string>
        {
            exception.Message
        };
    }
}

[Serializable]
public class Result<T> : Result
{
    public T Value { get; set; }

    public Result(T value) : base()
    {
        Value = value;
    }
}

public static class ResultExtensions
{
    public static Result AsResult<T>(this T value)
    {
        if (value is null)
        {
            return new Result(ResultCode.NotFound);
        }

        return new Result<T>(value);
    }

    public static Result AsResult(this Exception exception)
    {
        return new Result(exception);
    }
}