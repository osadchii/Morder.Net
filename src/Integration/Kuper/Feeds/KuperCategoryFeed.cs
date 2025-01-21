using Newtonsoft.Json;

namespace Integration.Kuper.Feeds;

public class KuperCategoryFeed : KuperFeed
{
    [JsonProperty("data")] public Item[] Data { get; set; } = Array.Empty<Item>();

    public class Item
    {
        [JsonProperty("id")] public string Id { get; set; } = null!;

        [JsonProperty("name")] public string Name { get; set; } = null!;
    }
}