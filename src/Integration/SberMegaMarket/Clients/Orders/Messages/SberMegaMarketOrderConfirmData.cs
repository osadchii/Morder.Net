using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class SberMegaMarketOrderConfirmData : SberMegaMarketMessageData
{
    public SberMegaMarketOrderConfirmData(string token) : base(token)
    {
    }

    public SberMegaMarketOrderConfirmData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<SberMegaMarketConfirmOrderShipment> Shipments { get; set; }
}

public class SberMegaMarketConfirmOrderShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; }

    [JsonProperty("orderCode")] public string OrderCode { get; set; }

    [JsonProperty("items")] public IEnumerable<SberMegaMarketConfirmOrderShipmentItem> Items { get; set; }
}

public class SberMegaMarketConfirmOrderShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; }

    [JsonProperty("offerId")] public string OfferId { get; set; }
}