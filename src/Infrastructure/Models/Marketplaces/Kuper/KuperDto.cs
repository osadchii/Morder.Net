namespace Infrastructure.Models.Marketplaces.Kuper;

public class KuperDto : MarketplaceDto
{
    public KuperSettings Settings { get; set; } = null!;
}