using Newtonsoft.Json;

namespace Integration.Meso.Feeds;

public class Feed
{
    [JsonProperty("products")] public List<MesoProduct> Products { get; set; } = new();
}

public class MesoProduct
{
    [JsonProperty("code")] public string Code { get; set; } = null!;

    [JsonProperty("internalCode")] public string InternalCode { get; set; } = null!;

    [JsonProperty("productName")] public string ProductName { get; set; } = null!;

    [JsonProperty("measureUnit")] public string MeasureUnit { get; set; } = null!;

    [JsonProperty("ndsPercent")] public double NdsPercent { get; set; }

    [JsonProperty("price")] public decimal Price { get; set; }

    [JsonProperty("remainsCount")] public decimal RemainsCount { get; set; }
}