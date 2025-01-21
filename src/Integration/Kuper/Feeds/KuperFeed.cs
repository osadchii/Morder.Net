using Newtonsoft.Json;

namespace Integration.Kuper.Feeds;

public abstract class KuperFeed<T> : KuperFeed
{
    [JsonProperty("data")] public T[] Data { get; set; } = Array.Empty<T>();
}

public abstract class KuperFeed
{
    
}