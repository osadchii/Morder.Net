using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Orders.Messages;

public class BaseSberMegaMarketOrderRequest<T> where T : class
{
    [JsonProperty("meta")] public Dictionary<string, object> Meta { get; set; } = null!;

    [JsonProperty("data")] public T Data { get; set; } = null!;
}