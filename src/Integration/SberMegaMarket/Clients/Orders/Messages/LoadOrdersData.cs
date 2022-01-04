using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class LoadOrdersData : SberMegaMarketMessageData
{
    public LoadOrdersData(string token) : base(token)
    {
    }

    public LoadOrdersData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<string> Shipments { get; set; }
}