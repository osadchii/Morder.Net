using System.Net;
using Api;
using TestFramework;
using TestFramework.Categories;
using TestFramework.Products;
using Xunit;

namespace IntegrationTests.Products;

public class ProductTest : BaseTest
{
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;

    public ProductTest(MorderWebApplicationFactory<Program> factory) : base(factory)
    {
        _productService = new ProductService(Client);
        _categoryService = new CategoryService(Client);
    }

    [Fact]
    public async Task GetAllCategories()
    {
        var productId = Guid.NewGuid();
        
        await _productService.PostProduct(productId);
        ServiceActionResult<ApiResult<IEnumerable<Product>>> getResult = await _productService.GetProducts();
        
        Assert.True(getResult.Response.IsSuccessStatusCode);
        Assert.Contains(getResult.Entity.Value, e => e.ExternalId == productId);
    }

    [Fact]
    public async Task CreateProductSuccess()
    {
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.PostProduct(productId);
        
        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateProductWrongCategoryFail()
    {
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.PostProduct(productId, Guid.NewGuid());
        
        Assert.False(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateProductExistingCategorySuccess()
    {
        var categoryId = Guid.NewGuid();
        ServiceActionResult<Category> category = await _categoryService.PostCategory(categoryId);
        
        Assert.True(category.Response.IsSuccessStatusCode);
        
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.PostProduct(productId, categoryId);
        
        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UpdateProductSuccess()
    {
        // Create product
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.PostProduct(productId);
        
        Assert.True(result.Response.IsSuccessStatusCode);
        
        // Create category
        var categoryId = Guid.NewGuid();
        ServiceActionResult<Category> category = await _categoryService.PostCategory(categoryId);
        
        Assert.True(category.Response.IsSuccessStatusCode);
        
        // Update product with new category
        Product product = result.Entity;
        product.CategoryId = categoryId;
        result = await _productService.PostProduct(product);
        
        Assert.True(result.Response.IsSuccessStatusCode);

        ServiceActionResult<ApiResult<Product>> getProduct = await _productService.GetProductByExternalId(productId);

        Assert.True(getProduct.Response.IsSuccessStatusCode);
        Assert.Equal(categoryId, getProduct.Entity.Value.CategoryId ?? Guid.Empty);
    }

    [Fact]
    public async Task UpdateProductWrongCategoryFail()
    {
        // Create product
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.PostProduct(productId);
        
        Assert.True(result.Response.IsSuccessStatusCode);
        
        var categoryId = Guid.NewGuid();
        
        // Update product with wrong category
        Product product = result.Entity;
        product.CategoryId = categoryId;
        result = await _productService.PostProduct(product);
        
        Assert.False(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetProductByExternalIdSuccess()
    {
        // Create product
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.PostProduct(productId);
        
        Assert.True(result.Response.IsSuccessStatusCode);

        ServiceActionResult<ApiResult<Product>> getResult = await _productService.GetProductByExternalId(productId);

        Assert.True(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(productId, getResult.Entity.Value.ExternalId ?? Guid.Empty);
    }

    [Fact]
    public async Task GetProductByExternalIdWrongIdFail()
    {
        var productId = Guid.NewGuid();

        ServiceActionResult<ApiResult<Product>> getResult = await _productService.GetProductByExternalId(productId);

        Assert.False(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResult.Response.StatusCode);
    }

    [Fact]
    public async Task GetProductByArticulSuccess()
    {
        // Create product
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.PostProduct(productId);
        
        Assert.True(result.Response.IsSuccessStatusCode);
        
        var articul = result.Entity.Articul!;

        ServiceActionResult<ApiResult<Product>> getResult = await _productService.GetProductByArticul(articul);

        Assert.True(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(productId, getResult.Entity.Value.ExternalId ?? Guid.Empty);
        Assert.Equal(articul, getResult.Entity.Value.Articul ?? string.Empty);
    }

    [Fact]
    public async Task GetProductByArticulWrongArticulFail()
    {
        var articul = Product.GetRandomArticul();

        ServiceActionResult<ApiResult<Product>> getResult = await _productService.GetProductByArticul(articul);

        Assert.False(getResult.Response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResult.Response.StatusCode);
    }

    [Fact]
    public async Task DeleteProductByExternalIdFail()
    {
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> result = await _productService.DeleteProductByExternalId(productId);
        
        Assert.False(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeleteProductByExternalIdSuccess()
    {
        var productId = Guid.NewGuid();
        
        await _productService.PostProduct(productId);
        
        ServiceActionResult<Product> result = await _productService.DeleteProductByExternalId(productId);
        
        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task SecondDeleteProductByExternalIdFail()
    {
        var productId = Guid.NewGuid();
        
        await _productService.PostProduct(productId);
        
        await _productService.DeleteProductByExternalId(productId);
        ServiceActionResult<Product> result = await _productService.DeleteProductByExternalId(productId);
        
        Assert.False(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeleteProductByArticulFail()
    {
        var articul = Product.GetRandomArticul();
        
        ServiceActionResult<Product> result = await _productService.DeleteProductByArticul(articul);
        
        Assert.False(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeleteProductByArticulSuccess()
    {
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> productResult = await _productService.PostProduct(productId);
        Assert.True(productResult.Response.IsSuccessStatusCode);

        var articul = productResult.Entity.Articul!;
        
        ServiceActionResult<Product> result = await _productService.DeleteProductByArticul(articul);
        
        Assert.True(result.Response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task SecondDeleteProductByArticulFail()
    {
        var productId = Guid.NewGuid();
        
        ServiceActionResult<Product> productResult = await _productService.PostProduct(productId);
        Assert.True(productResult.Response.IsSuccessStatusCode);

        var articul = productResult.Entity.Articul!;
        
        await _productService.DeleteProductByArticul(articul);
        ServiceActionResult<Product> result = await _productService.DeleteProductByArticul(articul);
        
        Assert.False(result.Response.IsSuccessStatusCode);
    }
}