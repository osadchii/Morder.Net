namespace Infrastructure.Models.Prices;

public class MarketplacePriceDto
{
    public string Articul { get; set; } = null!;
    public int ProductId { get; set; }
    public int MarketplaceId { get; set; }
    public decimal Value { get; set; }
    public string ProductExternalId { get; set; } = null!;
}