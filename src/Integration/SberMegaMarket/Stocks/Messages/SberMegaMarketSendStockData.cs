using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Stocks.Messages;

public class SberMegaMarketSendStockData : SberMegaMarketMessageData
{
    public SberMegaMarketSendStockData(string token) : base(token)
    {
        Stocks = new List<SberMegaMarketStock>();
    }

    public SberMegaMarketSendStockData() : base("")
    {
        Stocks = new List<SberMegaMarketStock>();
    }

    [JsonProperty("stocks")] public List<SberMegaMarketStock> Stocks { get; set; }
}