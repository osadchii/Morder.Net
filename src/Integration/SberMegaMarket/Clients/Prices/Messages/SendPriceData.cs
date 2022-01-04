using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Prices.Messages;

public class SendPriceData : SberMegaMarketMessageData
{
    public SendPriceData(string token) : base(token)
    {
        Prices = new List<SberMegaMarketPrice>();
    }

    public SendPriceData() : base("")
    {
        Prices = new List<SberMegaMarketPrice>();
    }

    [JsonProperty("prices")] public List<SberMegaMarketPrice> Prices { get; set; }
}