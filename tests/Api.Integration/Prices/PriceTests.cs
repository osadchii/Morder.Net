using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Prices.Commands;
using Infrastructure.MediatR.Stocks.Commands;
using Xunit;

namespace Api.Integration.Prices;

public class PriceTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string BaseUrl = "price";

    public PriceTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdatePriceWrongPriceFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = TestCases.UpdatePriceRequest.ToJson().FromJson<UpdatePriceRequest>();
        request!.Value = -10;

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePriceWrongProductExternalIdFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = TestCases.UpdatePriceRequest.ToJson().FromJson<UpdatePriceRequest>();
        request!.ProductExternalId = Guid.NewGuid();

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePriceWrongPriceTypeExternalIdFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = Stocks.TestCases.UpdateStockRequest.ToJson().FromJson<UpdatePriceRequest>();
        request!.PriceTypeExternalId = Guid.NewGuid();

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePriceSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(TestCases.UpdatePriceRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}