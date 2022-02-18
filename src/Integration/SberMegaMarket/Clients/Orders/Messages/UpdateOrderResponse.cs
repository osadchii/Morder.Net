using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients.Orders.Messages;

public class UpdateOrderResponse
{
    [JsonProperty("success")] public int Success { get; set; }

    [JsonProperty("data")] public UpdateOrderResponseData Data { get; set; } = null!;
}

public class UpdateOrderResponseData
{
    [JsonProperty("shipments")] public IEnumerable<UpdateOrderResponseDataShipment> Shipments { get; set; } = null!;
}

public class UpdateOrderResponseDataShipment
{
    [JsonProperty("shipmentId")] public string ShipmentId { get; set; } = null!;

    [JsonProperty("orderCode")] public string OrderCode { get; set; } = null!;

    [JsonProperty("confirmedTimeLimit")] public DateTime? ConfirmedTimeLimit { get; set; }

    [JsonProperty("creationDate")] public DateTime CreationDate { get; set; }

    [JsonProperty("shipmentDateTo")] public DateTime ShippingDate { get; set; }

    [JsonProperty("packingTimeLimit")] public DateTime? PackingTimeLimit { get; set; }

    [JsonProperty("shippingTimeLimit")] public DateTime? ShippingTimeLimit { get; set; }

    [JsonProperty("customerFullName")] public string CustomerFullName { get; set; } = null!;

    [JsonProperty("customerAddress")] public string CustomerAddress { get; set; } = null!;

    [JsonProperty("items")] public IEnumerable<UpdateOrderResponseDataShipmentItem> Items { get; set; } = null!;
}

public class UpdateOrderResponseDataShipmentItem
{
    [JsonProperty("itemIndex")] public string ItemIndex { get; set; } = null!;

    [JsonProperty("status")] public string? Status { get; set; }

    [JsonProperty("offerId")] public string OfferId { get; set; } = null!;

    [JsonProperty("price")] public decimal Price { get; set; }

    [JsonProperty("quantity")] public decimal Quantity { get; set; }
}