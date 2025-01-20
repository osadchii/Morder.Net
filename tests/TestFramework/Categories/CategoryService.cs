using Infrastructure.Extensions;

namespace TestFramework.Categories;

public class CategoryService : BaseService
{
    private const string BaseUrl = "/api/v1/category";

    public CategoryService(HttpClient client) : base(client)
    {
    }

    public async Task<ServiceActionResult<Category>> PostCategory(Guid externalId, Guid? parentId = null)
    {
        var category = Category.Create(externalId, parentId);
        return new ServiceActionResult<Category>(await PostAsync(BaseUrl, category), category);
    }

    public async Task<ServiceActionResult<Category>> PostCategory(Category category)
    {
        return new ServiceActionResult<Category>(await PostAsync(BaseUrl, category), category);
    }

    public Task<ServiceActionResult<ApiResult<Category>>> GetCategory(Guid externalId) =>
        GetCategory(externalId.ToString());

    public Task<ServiceActionResult<ApiResult<Category>>> GetCategory(int id) => GetCategory(id.ToString());

    public async Task<ServiceActionResult<ApiResult<IEnumerable<Category>>>> GetCategories()
    {
        var response = await GetAsync(BaseUrl);
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<IEnumerable<Category>>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<IEnumerable<Category>>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }

    public async Task<ServiceActionResult<Category>> DeleteCategory(int id) => new(await DeleteAsync($"{BaseUrl}/{id.ToString()}"));

    public async Task<ServiceActionResult<Category>> DeleteCategory(Guid externalId) => new(await DeleteAsync($"{BaseUrl}/{externalId.ToString()}"));

    private async Task<ServiceActionResult<ApiResult<Category>>> GetCategory(string id)
    {
        var response = await GetAsync($"{BaseUrl}/{id}");
        var content = await response.Content.ReadAsStringAsync();
        var result = new ServiceActionResult<ApiResult<Category>>(response);

        try
        {
            result.Entity = content.FromJson<ApiResult<Category>>() ?? default!;
        }
        catch
        {
            // ignore
        }

        return result;
    }
}