using System.Net;
using Api;
using TestFramework;
using TestFramework.Warehouses;
using Xunit;

namespace IntegrationTests.Warehouses;

public class WarehouseTests : BaseTest
{
    private readonly WarehouseService _warehouseService;

    public WarehouseTests(MorderWebApplicationFactory<Program> factory) : base(factory)
    {
        _warehouseService = new WarehouseService(Client);
    }

    [Fact]
    public async Task CreateWarehouseSuccess()
    {
        var warehouseId = Guid.NewGuid();

        ServiceActionResult<Warehouse> result = await _warehouseService.PostWarehouse(warehouseId);

        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UpdateWarehouseSuccess()
    {
        var priceTypeId = Guid.NewGuid();
        const string newName = "new warehouse name";

        ServiceActionResult<Warehouse> result = await _warehouseService.PostWarehouse(priceTypeId);
        Assert.True(result.Response.IsSuccessStatusCode);

        Warehouse warehouse = result.Entity;
        warehouse.Name = newName;
        result = await _warehouseService.PostWarehouse(warehouse);

        Assert.True(result.Response.IsSuccessStatusCode);

        ServiceActionResult<ApiResult<Warehouse>> getResult =
            await _warehouseService.GetWarehouseByExternalId(priceTypeId);

        Assert.True(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(newName, getResult.Entity.Value.Name);
    }

    [Fact]
    public async Task GetAllWarehouses()
    {
        var warehouseId = Guid.NewGuid();

        await _warehouseService.PostWarehouse(warehouseId);
        ServiceActionResult<ApiResult<IEnumerable<Warehouse>>> getResult = await _warehouseService.GetWarehouses();

        Assert.True(getResult.Response.IsSuccessStatusCode);
        Assert.Contains(getResult.Entity.Value, e => e.ExternalId == warehouseId);
    }

    [Fact]
    public async Task GetWarehouseByExternalIdSuccess()
    {
        var warehouseId = Guid.NewGuid();

        await _warehouseService.PostWarehouse(warehouseId);
        ServiceActionResult<ApiResult<Warehouse>> getResult =
            await _warehouseService.GetWarehouseByExternalId(warehouseId);

        Assert.True(getResult.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetWarehouseByExternalIdFail()
    {
        var warehouseId = Guid.NewGuid();
        ServiceActionResult<ApiResult<Warehouse>> getResult =
            await _warehouseService.GetWarehouseByExternalId(warehouseId);

        Assert.False(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResult.Response.StatusCode);
    }

    [Fact]
    public async Task DeleteWarehouseByExternalIdFail()
    {
        var warehouseId = Guid.NewGuid();

        ServiceActionResult<Warehouse> result = await _warehouseService.DeleteWarehouseByExternalId(warehouseId);

        Assert.False(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeleteWarehouseByExternalIdSuccess()
    {
        var warehouseId = Guid.NewGuid();

        await _warehouseService.PostWarehouse(warehouseId);

        ServiceActionResult<Warehouse> result = await _warehouseService.DeleteWarehouseByExternalId(warehouseId);

        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task SecondDeleteWarehouseByExternalIdFail()
    {
        var warehouseId = Guid.NewGuid();

        await _warehouseService.PostWarehouse(warehouseId);

        await _warehouseService.DeleteWarehouseByExternalId(warehouseId);
        ServiceActionResult<Warehouse> result = await _warehouseService.DeleteWarehouseByExternalId(warehouseId);

        Assert.False(result.Response.IsSuccessStatusCode);
    }
}