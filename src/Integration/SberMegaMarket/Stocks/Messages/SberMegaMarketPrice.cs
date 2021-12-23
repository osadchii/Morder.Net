using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Stocks.Messages;

public class SberMegaMarketPrice
{
    [JsonProperty("offerId")] public string OfferId { get; set; }

    [JsonProperty("price")] public decimal Price { get; set; }

    public SberMegaMarketPrice()
    {
    }

    public SberMegaMarketPrice(string articul, decimal price)
    {
        OfferId = articul;
        Price = price;
    }
}