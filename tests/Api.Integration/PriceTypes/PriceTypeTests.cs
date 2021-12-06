using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.Models.Prices;
using Xunit;

namespace Api.Integration.PriceTypes;

public class PriceTypeTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string BaseUrl = "pricetype";

    public PriceTypeTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdatePriceTypeSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdatePriceTypeRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPriceTypeByExternalId()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        string getUrl = $"{url}/{TestCases.UpdatePriceTypeRequest.ExternalId}";

        HttpResponseMessage response = await _client.GetAsync(getUrl);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<PriceTypeDto>>();

        Assert.NotNull(result);
        Assert.Equal(TestCases.UpdatePriceTypeRequest.Name, result?.Value.Name);
    }
}