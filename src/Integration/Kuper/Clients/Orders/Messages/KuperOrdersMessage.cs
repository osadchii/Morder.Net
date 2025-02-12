using Newtonsoft.Json;

namespace Integration.Kuper.Clients.Orders.Messages;

public class KuperOrdersMessage
{
    [JsonProperty("data")]
    public List<OrderData> Data { get; set; }

    [JsonProperty("appliedFilters")]
    public AppliedFilters AppliedFilters { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("nextPageToken")]
    public string NextPageToken { get; set; }
}

public class OrderData
{
    [JsonProperty("originalOrderId")]
    public string OriginalOrderId { get; set; }

    [JsonProperty("storeID")]
    public string StoreId { get; set; }

    [JsonProperty("storeImportKey")]
    public string StoreImportKey { get; set; }

    [JsonProperty("comment")]
    public string Comment { get; set; }

    [JsonProperty("clientType")]
    public string ClientType { get; set; }

    [JsonProperty("cancellationReason")]
    public string CancellationReason { get; set; }

    [JsonProperty("orderUUID")]
    public string OrderUuid { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("paymentState")]
    public string PaymentState { get; set; }

    [JsonProperty("cancellation")]
    public Cancellation Cancellation { get; set; }

    [JsonProperty("customer")]
    public Customer Customer { get; set; }

    [JsonProperty("address")]
    public Address Address { get; set; }

    [JsonProperty("delivery")]
    public Delivery Delivery { get; set; }

    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("paymentMethods")]
    public List<string> PaymentMethods { get; set; }

    [JsonProperty("shipmentMethod")]
    public string ShipmentMethod { get; set; }

    [JsonProperty("replacementPolicy")]
    public string ReplacementPolicy { get; set; }

    [JsonProperty("bonusCard")]
    public BonusCard BonusCard { get; set; }

    [JsonProperty("deliveryCost")]
    public string DeliveryCost { get; set; }

    [JsonProperty("terminalGroupId")]
    public string TerminalGroupId { get; set; }

    [JsonProperty("extensions")]
    public Extensions Extensions { get; set; }

    [JsonProperty("completeBefore")]
    public DateTime CompleteBefore { get; set; }

    [JsonProperty("positions")]
    public List<Position> Positions { get; set; }
}

public class Cancellation
{
    [JsonProperty("slug")]
    public string Slug { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

public class Customer
{
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("short_number")]
    public string ShortNumber { get; set; }

    [JsonProperty("inn")]
    public string Inn { get; set; }

    [JsonProperty("kpp")]
    public string Kpp { get; set; }

    [JsonProperty("legalAddress")]
    public string LegalAddress { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
}

public class Address
{
    [JsonProperty("lat")]
    public string Lat { get; set; }

    [JsonProperty("lon")]
    public string Lon { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("floor")]
    public string Floor { get; set; }

    [JsonProperty("street")]
    public string Street { get; set; }

    [JsonProperty("building")]
    public string Building { get; set; }

    [JsonProperty("elevator")]
    public string Elevator { get; set; }

    [JsonProperty("entrance")]
    public string Entrance { get; set; }

    [JsonProperty("apartment")]
    public string Apartment { get; set; }

    [JsonProperty("door_phone")]
    public string DoorPhone { get; set; }

    [JsonProperty("full_address")]
    public string FullAddress { get; set; }

    [JsonProperty("delivery_to_door")]
    public bool DeliveryToDoor { get; set; }
}

public class Delivery
{
    [JsonProperty("expectedFrom")]
    public DateTime ExpectedFrom { get; set; }

    [JsonProperty("expectedTo")]
    public DateTime ExpectedTo { get; set; }

    [JsonProperty("isPlanned")]
    public bool IsPlanned { get; set; }
}

public class Total
{
    [JsonProperty("totalPrice")]
    public string TotalPrice { get; set; }

    [JsonProperty("discountTotalPrice")]
    public string DiscountTotalPrice { get; set; }
}

public class BonusCard
{
    [JsonProperty("number")]
    public string Number { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }
}

public class Extensions
{
    [JsonProperty("cutlery_quantity")]
    public int CutleryQuantity { get; set; }
}

public class Position
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("originalQuantity")]
    public decimal OriginalQuantity { get; set; }

    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    [JsonProperty("totalDiscountPrice")]
    public string TotalDiscountPrice { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("weight")]
    public string Weight { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("discountPrice")]
    public string DiscountPrice { get; set; }

    [JsonProperty("totalPrice")]
    public string TotalPrice { get; set; }

    [JsonProperty("replacedByID")]
    public string ReplacedById { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("isCombo")]
    public bool IsCombo { get; set; }

    [JsonProperty("modifiers")]
    public List<Modifier> Modifiers { get; set; }

    [JsonProperty("extensions")]
    public PositionExtensions Extensions { get; set; }
}

public class Modifier
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("groupId")]
    public string GroupId { get; set; }

    [JsonProperty("groupName")]
    public string GroupName { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("default")]
    public bool Default { get; set; }
}

public class PositionExtensions
{
    [JsonProperty("meta")]
    public string Meta { get; set; }

    [JsonProperty("fiscalDataSku")]
    public string FiscalDataSku { get; set; }
}

public class AppliedFilters
{
    [JsonProperty("createdFrom")]
    public DateTime? CreatedFrom { get; set; }

    [JsonProperty("createdTo")]
    public DateTime? CreatedTo { get; set; }

    [JsonProperty("updatedFrom")]
    public DateTime? UpdatedFrom { get; set; }

    [JsonProperty("updatedTo")]
    public DateTime? UpdatedTo { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("paymentState")]
    public string PaymentState { get; set; }

    [JsonProperty("storeId")]
    public string StoreId { get; set; }

    [JsonProperty("storeImportKey")]
    public string StoreImportKey { get; set; }

    [JsonProperty("deliveryFrom")]
    public DateTime? DeliveryFrom { get; set; }

    [JsonProperty("deliveryTo")]
    public DateTime? DeliveryTo { get; set; }
}