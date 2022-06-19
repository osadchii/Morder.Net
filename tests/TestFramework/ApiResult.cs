namespace TestFramework;

public class ApiResult<T> where T : class
{
    public List<string> Errors { get; set; } = null!;
    public T Value { get; set; } = null!;
}