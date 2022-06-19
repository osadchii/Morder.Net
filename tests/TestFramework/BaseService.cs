using System.Text;
using Infrastructure.Extensions;

namespace TestFramework;

public abstract class BaseService
{
    private readonly HttpClient _client;
    private const string JsonMediaType = "application/json";

    protected BaseService(HttpClient client)
    {
        _client = client;
    }

    protected Task<HttpResponseMessage> PostAsync(string url, object body) => _client.PostAsync(url, new StringContent(body.ToJson(), Encoding.UTF8, JsonMediaType));

    protected Task<HttpResponseMessage> GetAsync(string url) => _client.GetAsync(url);

    protected Task<HttpResponseMessage> DeleteAsync(string url) => _client.DeleteAsync(url);
}