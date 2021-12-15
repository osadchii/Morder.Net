using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Integration.Marketplaces.SberMegaMarket;

public class SberMegaMarketTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string BaseUrl = "sbermegamarket";

    public SberMegaMarketTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateSberMegaMarketSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateSberMegaMarketRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}