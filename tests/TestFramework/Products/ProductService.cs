using Infrastructure.Extensions;

namespace TestFramework.Products;

public class ProductService : BaseService
{
    private const string BaseUrl = "/api/v1/product";
    
    public ProductService(HttpClient client) : base(client) { }

    public async Task<ServiceActionResult<Product>> PostProduct(Guid externalId, Guid? categoryId = null)
    {
        var product = Product.Create(externalId, categoryId);
        return new ServiceActionResult<Product>(await PostAsync(BaseUrl, product), product);
    }
    
    public async Task<ServiceActionResult<Product>> PostProduct(Product product)
    {
        return new ServiceActionResult<Product>(await PostAsync(BaseUrl, product), product);
    }

    public Task<ServiceActionResult<ApiResult<Product>>> GetProductByExternalId(Guid externalId) =>
        GetProduct(externalId.ToString());

    public Task<ServiceActionResult<ApiResult<Product>>> GetProductByArticul(string articul) => GetProduct(articul);

    public async Task<ServiceActionResult<ApiResult<IEnumerable<Product>>>> GetProducts()
    {
        HttpResponseMessage response = await GetAsync(BaseUrl);
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<IEnumerable<Product>>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<IEnumerable<Product>>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }
    
    public async Task<ServiceActionResult<Product>> DeleteProductByExternalId(Guid externalId) => new(await DeleteAsync($"{BaseUrl}/{externalId.ToString()}"));
    
    public async Task<ServiceActionResult<Product>> DeleteProductByArticul(string articul) => new(await DeleteAsync($"{BaseUrl}/{articul}"));
    
    private async Task<ServiceActionResult<ApiResult<Product>>> GetProduct(string id)
    {
        HttpResponseMessage response = await GetAsync($"{BaseUrl}/{id}");
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<Product>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<Product>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }
}