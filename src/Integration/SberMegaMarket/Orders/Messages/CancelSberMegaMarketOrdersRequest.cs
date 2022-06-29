using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Orders.Messages;

public class CancelSberMegaMarketOrdersRequest
{
    [JsonProperty("merchantId")] public int MerchantId { get; set; }

    [JsonProperty("shipments")] public IEnumerable<CancelSberMegaMarketOrder> Shipments { get; set; } = null!;
}

public class CancelSberMegaMarketOrder
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<CancelSberMegaMarketOrderItem> Items { get; set; } = null!;
}

public class CancelSberMegaMarketOrderItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; } = null!;

    [JsonProperty("offerId")] public string OfferId { get; set; } = null!;
}