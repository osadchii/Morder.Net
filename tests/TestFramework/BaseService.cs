using System.Text;
using Infrastructure.Extensions;
using TestFramework.Users;

namespace TestFramework;

public abstract class BaseService
{
    private readonly HttpClient _client;
    private const string JsonMediaType = "application/json";

    protected BaseService(HttpClient client)
    {
        _client = client;
    }

    protected async Task<HttpResponseMessage> PostAsync(string url, object body, bool skipAuthorize = false)
    {
        if (!skipAuthorize)
        {
            await Authorize();
        }
        return await _client.PostAsync(url, new StringContent(body.ToJson(), Encoding.UTF8, JsonMediaType));
    } 

    protected async Task<HttpResponseMessage> GetAsync(string url)
    {
        await Authorize();
        return await _client.GetAsync(url);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        await Authorize();
        return await _client.DeleteAsync(url);
    }

    private async Task Authorize()
    {
        if (_client.DefaultRequestHeaders.Authorization is not null && !_client.DefaultRequestHeaders.Authorization.Parameter.IsNullOrEmpty())
        {
            return;
        }
        
        var userService = new UserService(_client);

        var user = new User()
        {
            Name = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };
        
        await userService.RegisterUser(user);
        var tokenResult = await userService.GetToken(user);

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResult.Entity.Value.Token}");
    }
}