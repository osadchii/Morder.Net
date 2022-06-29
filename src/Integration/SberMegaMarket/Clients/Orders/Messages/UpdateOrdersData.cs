using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class UpdateOrdersData : SberMegaMarketMessageData
{
    public UpdateOrdersData(string token) : base(token)
    {
    }

    public UpdateOrdersData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<string> Shipments { get; set; } = null!;
}