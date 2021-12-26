using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Stocks.Messages;

public class SberMegaMarketStock
{
    [JsonProperty("offerId")] public string OfferId { get; set; }

    [JsonProperty("quantity")] public string Quantity { get; set; }

    public SberMegaMarketStock()
    {
    }

    public SberMegaMarketStock(string articul, decimal quantity)
    {
        OfferId = articul;
        Quantity = quantity.ToString();
    }
}