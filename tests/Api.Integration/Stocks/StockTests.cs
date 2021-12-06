using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Stocks.Commands;
using Xunit;

namespace Api.Integration.Stocks;

public class StockTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string BaseUrl = "stock";

    public StockTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateStockWrongStockFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = TestCases.UpdateStockRequest.ToJson().FromJson<UpdateStockRequest>();
        request!.Value = -10;

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStockWrongProductExternalIdFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = TestCases.UpdateStockRequest.ToJson().FromJson<UpdateStockRequest>();
        request!.ProductExternalId = Guid.NewGuid();

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStockWrongWarehouseExternalIdFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = TestCases.UpdateStockRequest.ToJson().FromJson<UpdateStockRequest>();
        request!.WarehouseExternalId = Guid.NewGuid();

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStockSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateStockRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}