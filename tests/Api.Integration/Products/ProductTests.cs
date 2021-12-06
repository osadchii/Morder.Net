using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Products.Commands;
using Infrastructure.Models.Products;
using Xunit;

namespace Api.Integration.Products;

public class ProductTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string BaseUrl = "product";

    public ProductTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateProductSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateProductRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProductDuplicateArticulFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = TestCases.UpdateProductRequest.ToJson().FromJson<UpdateProductRequest>();
        request!.ExternalId = Guid.NewGuid();

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProductWrongCategoryFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        var request = TestCases.UpdateProductRequest.ToJson().FromJson<UpdateProductRequest>();
        request!.CategoryId = Guid.NewGuid();

        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetProductByExternalId()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        string getUrl = $"{url}/{TestCases.UpdateProductRequest.ExternalId}";

        HttpResponseMessage response = await _client.GetAsync(getUrl);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<ProductDto>>();

        Assert.NotNull(result);
        Assert.Equal(TestCases.UpdateProductRequest.Articul, result?.Value.Articul);
    }
}