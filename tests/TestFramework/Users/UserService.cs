using Infrastructure.Extensions;

namespace TestFramework.Users;

public class UserService : BaseService
{
    private const string BaseUrl = "/api/v1/user";
    
    public UserService(HttpClient client) : base(client)
    {
    }

    public  Task RegisterUser(User user) => PostAsync($"{BaseUrl}/register", user, true);

    public async Task<ServiceActionResult<ApiResult<TokenDto>>> GetToken(User user)
    {
        HttpResponseMessage response = await PostAsync($"{BaseUrl}/gettoken", user, true);
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<TokenDto>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<TokenDto>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }

}