namespace Infrastructure.Models.Warehouses;

public class MarketplaceStockDto
{
    public int ProductId { get; set; }
    public int MarketplaceId { get; set; }
    public decimal Value { get; set; }
    public string ProductExternalId { get; set; }
}