using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class PackPostingRequest
{
    [JsonProperty("posting_number")] public string PostingNumber { get; set; } = null!;

    [JsonProperty("packages")] public IEnumerable<PackPostingPackage> Packages { get; set; } = null!;
}

public class PackPostingPackage
{
    [JsonProperty("products")] public IEnumerable<PackPostingProduct> Products { get; set; } = null!;
}

public class PackPostingProduct
{
    [JsonProperty("quantity")] public int Quantity { get; set; }

    [JsonProperty("product_id")] public int ProductId { get; set; }
}

public class PackPostingResponse
{
    [JsonProperty("result")] public IEnumerable<string> Result { get; set; } = null!;
}