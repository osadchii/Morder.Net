using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Prices.Messages;

public class SberMegaMarketPrice
{
    [JsonProperty("offerId")] public string OfferId { get; set; } = null!;

    [JsonProperty("price")] public decimal Price { get; set; }

    [JsonProperty("isDeleted")] public bool IsDeleted => false;

    public SberMegaMarketPrice()
    {
    }

    public SberMegaMarketPrice(string articul, decimal price)
    {
        OfferId = articul;
        Price = price;
    }
}