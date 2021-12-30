using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Prices.Messages;

public class SberMegaMarketSendPriceData : SberMegaMarketMessageData
{
    public SberMegaMarketSendPriceData(string token) : base(token)
    {
        Prices = new List<SberMegaMarketPrice>();
    }

    public SberMegaMarketSendPriceData() : base("")
    {
        Prices = new List<SberMegaMarketPrice>();
    }

    [JsonProperty("prices")] public List<SberMegaMarketPrice> Prices { get; set; }
}