using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class StickerPrintResponse
{
    [JsonProperty("success")] public int Success { get; set; }

    [JsonProperty("meta")] public Dictionary<string, object> Meta { get; set; }

    [JsonProperty("data")] public string Data { get; set; }
}