using Infrastructure.Extensions;

namespace TestFramework.PriceTypes;

public class PriceTypeService : BaseService
{
    private const string BaseUrl = "/api/v1/pricetype";

    public PriceTypeService(HttpClient client) : base(client)
    {
    }

    public async Task<ServiceActionResult<PriceType>> PostPriceType(Guid externalId)
    {
        var priceType = PriceType.Create(externalId);
        return new ServiceActionResult<PriceType>(await PostAsync(BaseUrl, priceType), priceType);
    }

    public async Task<ServiceActionResult<PriceType>> PostPriceType(PriceType priceType)
    {
        return new ServiceActionResult<PriceType>(await PostAsync(BaseUrl, priceType), priceType);
    }

    public Task<ServiceActionResult<ApiResult<PriceType>>> GetPriceTypeByExternalId(Guid externalId) =>
        GetPriceType(externalId.ToString());

    public async Task<ServiceActionResult<ApiResult<IEnumerable<PriceType>>>> GetPriceTypes()
    {
        var response = await GetAsync(BaseUrl);
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<IEnumerable<PriceType>>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<IEnumerable<PriceType>>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }

    public async Task<ServiceActionResult<PriceType>> DeletePriceTypeByExternalId(Guid externalId) =>
        new(await DeleteAsync($"{BaseUrl}/{externalId.ToString()}"));

    private async Task<ServiceActionResult<ApiResult<PriceType>>> GetPriceType(string id)
    {
        var response = await GetAsync($"{BaseUrl}/{id}");
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<PriceType>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<PriceType>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }
}