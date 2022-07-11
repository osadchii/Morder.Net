using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Orders.Messages;

public class SberMegaMarketResponse
{
    [JsonProperty("meta")] public Dictionary<string, object> Meta { get; set; }

    [JsonProperty("data")] public Dictionary<string, object> Data { get; set; } = new();

    [JsonProperty("success")] public int Success { get; set; } = 1;

    public SberMegaMarketResponse(Dictionary<string, object> meta, string error = null)
    {
        Meta = meta;

        if (error is not null)
        {
            Data = new Dictionary<string, object>()
            {
                ["error"] = error
            };
            Success = 0;
        }
    }
}