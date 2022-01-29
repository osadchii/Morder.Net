using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class StickerPrintData : SberMegaMarketMessageData
{
    public StickerPrintData(string token) : base(token)
    {
    }

    public StickerPrintData() : base("")
    {
    }

    [JsonProperty("shipments")] public IEnumerable<StickerPrintShipment> Shipments { get; set; } = null!;
}

public class StickerPrintShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("boxCodes")] public IEnumerable<string> BoxCodes { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<StickerPrintShipmentItem> Items { get; set; } = null!;
}

public class StickerPrintShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; } = null!;

    [JsonProperty("quantity")] public int Quantity { get; set; }

    [JsonProperty("boxes")] public IEnumerable<StickerPrintShipmentItemBox> Boxes { get; set; } = null!;
}

public class StickerPrintShipmentItemBox
{
    [JsonProperty("boxIndex")] public string BoxIndex { get; set; } = null!;

    [JsonProperty("boxCode")] public string BoxCode { get; set; } = null!;
}