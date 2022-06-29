using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Orders.Messages;

public class CreateSberMegaMarketOrdersRequest
{
    [JsonProperty("merchantId")] public int MerchantId { get; set; }

    [JsonProperty("shipments")] public IEnumerable<CreateSberMegaMarketOrder> Shipments { get; set; } = null!;
}

public class CreateSberMegaMarketOrder
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("shipmentDate")] public DateTime ShipmentDate { get; set; }

    [JsonProperty("items")] public IEnumerable<CreateSberMegaMarketOrderItem> Items { get; set; } = null!;

    [JsonProperty("shipping")] public CreateSberMegaMarketOrderShipping Shipping { get; set; } = null!;
}

public class CreateSberMegaMarketOrderItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; } = null!;

    [JsonProperty("offerId")] public string OfferId { get; set; } = null!;

    [JsonProperty("price")] public decimal Price { get; set; }

    [JsonProperty("quantity")] public decimal Quantity { get; set; }
}

public class CreateSberMegaMarketOrderShipping
{
    [JsonProperty("shippingDate")] public DateTime ShippingDate { get; set; }
}