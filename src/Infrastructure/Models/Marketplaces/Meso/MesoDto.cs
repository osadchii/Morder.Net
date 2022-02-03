namespace Infrastructure.Models.Marketplaces.Meso;

public class MesoDto : MarketplaceDto
{
    public MesoSettings Settings { get; set; } = null!;
}