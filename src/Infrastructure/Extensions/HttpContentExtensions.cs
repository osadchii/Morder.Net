namespace Infrastructure.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T> ReadAsObject<T>(this HttpContent content)
    {
        var body = await content.ReadAsStringAsync();
        return body.FromJson<T>();
    }
}