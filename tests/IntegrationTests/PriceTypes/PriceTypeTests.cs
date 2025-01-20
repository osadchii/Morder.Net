using System.Net;
using Api;
using TestFramework;
using TestFramework.PriceTypes;
using Xunit;

namespace IntegrationTests.PriceTypes;

public class PriceTypeTests : BaseTest
{
    private readonly PriceTypeService _priceTypeService;

    public PriceTypeTests(MorderWebApplicationFactory<Program> factory) : base(factory)
    {
        _priceTypeService = new PriceTypeService(Client);
    }

    [Fact]
    public async Task CreatePriceTypeSuccess()
    {
        var priceTypeId = Guid.NewGuid();

        var result = await _priceTypeService.PostPriceType(priceTypeId);

        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UpdatePriceTypeSuccess()
    {
        var priceTypeId = Guid.NewGuid();
        const string newName = "new price type name";

        var result = await _priceTypeService.PostPriceType(priceTypeId);
        Assert.True(result.Response.IsSuccessStatusCode);

        var priceType = result.Entity;
        priceType.Name = newName;
        result = await _priceTypeService.PostPriceType(priceType);

        Assert.True(result.Response.IsSuccessStatusCode);

        var getResult =
            await _priceTypeService.GetPriceTypeByExternalId(priceTypeId);

        Assert.True(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(newName, getResult.Entity.Value.Name);
    }

    [Fact]
    public async Task GetAllPriceTypes()
    {
        var priceTypeId = Guid.NewGuid();

        await _priceTypeService.PostPriceType(priceTypeId);
        var getResult = await _priceTypeService.GetPriceTypes();

        Assert.True(getResult.Response.IsSuccessStatusCode);
        Assert.Contains(getResult.Entity.Value, e => e.ExternalId == priceTypeId);
    }

    [Fact]
    public async Task GetPriceTypeByExternalIdSuccess()
    {
        var priceTypeId = Guid.NewGuid();

        await _priceTypeService.PostPriceType(priceTypeId);
        var getResult =
            await _priceTypeService.GetPriceTypeByExternalId(priceTypeId);

        Assert.True(getResult.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetPriceTypeByExternalIdFail()
    {
        var priceTypeId = Guid.NewGuid();
        var getResult =
            await _priceTypeService.GetPriceTypeByExternalId(priceTypeId);

        Assert.False(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResult.Response.StatusCode);
    }

    [Fact]
    public async Task DeletePriceTypeByExternalIdFail()
    {
        var priceTypeId = Guid.NewGuid();

        var result = await _priceTypeService.DeletePriceTypeByExternalId(priceTypeId);

        Assert.False(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeletePriceTypeByExternalIdSuccess()
    {
        var priceTypeId = Guid.NewGuid();

        await _priceTypeService.PostPriceType(priceTypeId);

        var result = await _priceTypeService.DeletePriceTypeByExternalId(priceTypeId);

        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task SecondDeletePriceTypeByExternalIdFail()
    {
        var priceTypeId = Guid.NewGuid();

        await _priceTypeService.PostPriceType(priceTypeId);

        await _priceTypeService.DeletePriceTypeByExternalId(priceTypeId);
        var result = await _priceTypeService.DeletePriceTypeByExternalId(priceTypeId);

        Assert.False(result.Response.IsSuccessStatusCode);
    }
}