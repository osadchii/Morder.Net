using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.Models.Products;
using Xunit;

namespace Api.Integration.Categories;

public class CategoryTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string BaseUrl = "category";

    public CategoryTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateChildWithoutParentFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        UpdateCategoryRequest request = new()
        {
            Name = TestCases.UpdateChildCategoryRequest.Name,
            ExternalId = TestCases.UpdateChildCategoryRequest.ExternalId,
            ParentId = Guid.NewGuid()
        };
        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateParentCategorySuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateParentCategoryRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateChildCategorySuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateChildCategoryRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllCategories()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        HttpResponseMessage response = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<List<CategoryDto>>>();

        Assert.NotNull(result);

        CategoryDto? entry =
            result?.Value.FirstOrDefault(c => c.ExternalId == TestCases.UpdateParentCategoryRequest.ExternalId);
        Assert.NotNull(entry);
    }
}