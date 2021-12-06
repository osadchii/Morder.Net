using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.Models.Warehouses;
using Xunit;

namespace Api.Integration.Warehouses;

public class WarehouseTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string BaseUrl = "warehouse";

    public WarehouseTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateWarehouseSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateWarehouseRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWarehouseByExternalId()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        string getUrl = $"{url}/{TestCases.UpdateWarehouseRequest.ExternalId}";

        HttpResponseMessage response = await _client.GetAsync(getUrl);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<WarehouseDto>>();

        Assert.NotNull(result);
        Assert.Equal(TestCases.UpdateWarehouseRequest.Name, result?.Value.Name);
    }
}