namespace Infrastructure.Models.Marketplaces.YandexMarket;

public class YandexMarketDto : MarketplaceDto
{
    public YandexMarketSettings Settings { get; set; } = null!;
}