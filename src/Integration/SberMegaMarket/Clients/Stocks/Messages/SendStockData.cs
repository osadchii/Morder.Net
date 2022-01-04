using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Stocks.Messages;

public class SendStockData : SberMegaMarketMessageData
{
    public SendStockData(string token) : base(token)
    {
        Stocks = new List<SberMegaMarketStock>();
    }

    public SendStockData() : base("")
    {
        Stocks = new List<SberMegaMarketStock>();
    }

    [JsonProperty("stocks")] public List<SberMegaMarketStock> Stocks { get; set; }
}