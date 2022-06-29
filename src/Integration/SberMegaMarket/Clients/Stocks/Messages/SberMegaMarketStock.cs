using System.Globalization;
using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Stocks.Messages;

public class SberMegaMarketStock
{
    [JsonProperty("offerId")] public string OfferId { get; set; } = null!;

    [JsonProperty("quantity")] public string Quantity { get; set; } = null!;

    public SberMegaMarketStock()
    {
    }

    public SberMegaMarketStock(string articul, decimal quantity)
    {
        OfferId = articul;
        Quantity = quantity.ToString(CultureInfo.InvariantCulture);
    }
}