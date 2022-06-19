namespace TestFramework;

public class ServiceActionResult<T> where T : class
{
    public T Entity { get; set; } = null!;
    public HttpResponseMessage Response { get; init; }

    public ServiceActionResult(HttpResponseMessage response)
    {
        Response = response;
    }

    public ServiceActionResult(HttpResponseMessage response, T entity) : this(response)
    {
        Entity = entity;
    }
}