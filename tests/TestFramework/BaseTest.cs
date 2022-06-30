using Api;
using TestFramework.Users;
using Xunit;

namespace TestFramework;

public abstract class BaseTest : IClassFixture<MorderWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    protected BaseTest(MorderWebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
        var userService = new UserService(Client);

        var user = new User()
        {
            Name = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };
        
        userService.RegisterUser(user).GetAwaiter().GetResult();
        ServiceActionResult<ApiResult<TokenDto>> tokenResult = userService.GetToken(user).GetAwaiter().GetResult();

        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResult.Entity.Value.Token}");
    }
}