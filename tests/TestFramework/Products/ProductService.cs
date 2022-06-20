namespace TestFramework.Products;

public class ProductService : BaseService
{
    private const string BaseUrl = "/api/v1/product";
    
    public ProductService(HttpClient client) : base(client) { }

    public async Task<ServiceActionResult<Product>> PostProduct(Guid externalId)
    {
        var product = Product.Create(externalId);
        return new ServiceActionResult<Product>(await PostAsync(BaseUrl, product), product);
    }
}