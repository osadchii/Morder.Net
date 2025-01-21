using Newtonsoft.Json;

namespace Integration.Kuper.Feeds;

public class KuperCategoryFeed : KuperFeed<KuperCategoryFeed.Item>
{

    public class Item
    {
        [JsonProperty("id")] public string Id { get; set; } = null!;

        [JsonProperty("name")] public string Name { get; set; } = null!;
    }
}