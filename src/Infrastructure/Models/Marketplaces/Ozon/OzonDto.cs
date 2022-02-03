namespace Infrastructure.Models.Marketplaces.Ozon;

public class OzonDto : MarketplaceDto
{
    public OzonSettings Settings { get; set; } = null!;
}