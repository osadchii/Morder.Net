using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Integration.Marketplaces.SberMegaMarket;

public class SberMegaMarketTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly MContext _context;
    private const string BaseUrl = "sbermegamarket";

    public SberMegaMarketTests(MorderWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        IServiceScope scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<MContext>();
    }

    [Fact]
    public async Task CreateSberMegaMarketSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);

        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateSberMegaMarketRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<SberMegaMarketDto>>();

        Assert.NotNull(result);
        Assert.Equal(TestCases.UpdateSberMegaMarketRequest.Settings!.Token,
            result!.Value.Settings.Token);
        Assert.Equal(TestCases.UpdateSberMegaMarketRequest.MinimalPrice,
            result.Value.MinimalPrice);

        TestCases.UpdateSberMegaMarketRequest.Id = result.Value.Id;
    }

    [Fact]
    public async Task UpdateSberMegaMarketSuccess()
    {
        int lastId = await _context.Marketplaces
            .Select(m => m.Id)
            .OrderByDescending(i => i)
            .LastAsync();
        string url = MorderWebApplicationFactory.ServiceUrl(1, BaseUrl);
        string marketplaceUrl = $"{url}/{lastId}";

        var updated = TestCases.UpdateSberMegaMarketRequest.ToJson()
            .FromJson<UpdateSberMegaMarketRequest>();

        updated!.Settings!.Token = "New test Token";
        updated.MinimalPrice = 5000;

        HttpResponseMessage response =
            await _client.PostAsync(marketplaceUrl, JsonContent.Create(updated));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<SberMegaMarketDto>>();

        Assert.NotNull(result);
        Assert.Equal(updated.Settings!.Token,
            result!.Value.Settings.Token);
        Assert.Equal(updated.MinimalPrice,
            result.Value.MinimalPrice);
    }
}