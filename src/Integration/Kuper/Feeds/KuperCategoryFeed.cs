using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Newtonsoft.Json;

namespace Integration.Kuper.Feeds;

public class KuperCategoryFeed : KuperFeed<KuperCategoryFeed.Item>
{
    public class Item
    {
        [JsonProperty("id")] public string Id { get; set; } = null!;

        [JsonProperty("name")] public string Name { get; set; } = null!;
    }

    public static KuperCategoryFeed Build(IEnumerable<Product> products, MarketplaceProductData data)
    {
        var categoryIds = products
            .Where(x => x.CategoryId.HasValue)
            .Select(x => x.CategoryId!.Value)
            .Distinct()
            .ToArray();

        var categories = data.Categories
            .Where(x => categoryIds.Contains(x.Key))
            .Select(x => new KuperCategoryFeed.Item
            {
                Id = x.Key.ToString(),
                Name = x.Value.Name
            })
            .ToArray();

        var feed = new KuperCategoryFeed
        {
            Data = categories
        };
        return feed;
    }
}