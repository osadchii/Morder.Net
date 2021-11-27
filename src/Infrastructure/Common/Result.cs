using Infrastructure.Enums;

namespace Infrastructure.Common;

[Serializable]
public class Result
{
    public ResultCode Status { get; set; }

    public bool IsSucceeded => Status == ResultCode.Success;

    public bool HasErrors => Errors?.Any() == true;

    public Exception Exception { get; protected set; }

    public Dictionary<string, List<string>> Errors { get; protected set; }

    public static Result Ok => new();

    public Result()
    {
        Status = ResultCode.Success;
    }

    public Result(ResultCode status)
    {
        Status = status;
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
    public static Result<T> AsResult<T>(this T value)
    {
        return new Result<T>(value);
    }
}