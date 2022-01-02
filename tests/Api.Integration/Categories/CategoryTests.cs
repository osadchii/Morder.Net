using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
        string content = await response.Content.ReadAsStringAsync();

        Assert.True(HttpStatusCode.BadRequest == response.StatusCode, content);
    }

    [Fact]
    public async Task UpdateParentCategorySuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        string json = TestCases.UpdateParentCategoryRequest.ToJson();

        HttpResponseMessage response =
            await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        string content = await response.Content.ReadAsStringAsync();

        Assert.True(HttpStatusCode.OK == response.StatusCode, content);
    }

    [Fact]
    public async Task UpdateChildCategorySuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateChildCategoryRequest));
        string content = await response.Content.ReadAsStringAsync();

        Assert.True(HttpStatusCode.OK == response.StatusCode, content);
    }

    [Fact]
    public async Task GetAllCategories()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        HttpResponseMessage response = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<List<CategoryDto>>>();

        Assert.True(result is not null, content);

        CategoryDto? entry =
            result?.Value.FirstOrDefault(c => c.ExternalId == TestCases.UpdateParentCategoryRequest.ExternalId);

        Assert.True(entry is not null, content);
    }
}