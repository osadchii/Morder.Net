using Infrastructure.Extensions;

namespace TestFramework.Warehouses;

public class WarehouseService : BaseService
{
    private const string BaseUrl = "/api/v1/warehouse";

    public WarehouseService(HttpClient client) : base(client)
    {
    }

    public async Task<ServiceActionResult<Warehouse>> PostWarehouse(Guid externalId)
    {
        var warehouse = Warehouse.Create(externalId);
        return new ServiceActionResult<Warehouse>(await PostAsync(BaseUrl, warehouse), warehouse);
    }

    public async Task<ServiceActionResult<Warehouse>> PostWarehouse(Warehouse warehouse)
    {
        return new ServiceActionResult<Warehouse>(await PostAsync(BaseUrl, warehouse), warehouse);
    }

    public Task<ServiceActionResult<ApiResult<Warehouse>>> GetWarehouseByExternalId(Guid externalId) =>
        GetWarehouse(externalId.ToString());

    public async Task<ServiceActionResult<ApiResult<IEnumerable<Warehouse>>>> GetWarehouses()
    {
        HttpResponseMessage response = await GetAsync(BaseUrl);
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<IEnumerable<Warehouse>>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<IEnumerable<Warehouse>>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }

    public async Task<ServiceActionResult<Warehouse>> DeleteWarehouseByExternalId(Guid externalId) =>
        new(await DeleteAsync($"{BaseUrl}/{externalId.ToString()}"));

    private async Task<ServiceActionResult<ApiResult<Warehouse>>> GetWarehouse(string id)
    {
        HttpResponseMessage response = await GetAsync($"{BaseUrl}/{id}");
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<Warehouse>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<Warehouse>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }
}