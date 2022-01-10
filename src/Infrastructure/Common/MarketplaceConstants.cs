using Infrastructure.Models.Marketplaces;

namespace Infrastructure.Common;

public static class MarketplaceConstants
{
    public static IEnumerable<MarketplaceType> MarketplacesHasNoExternalProductId => new[]
    {
        MarketplaceType.SberMegaMarket
    };

    public static IEnumerable<MarketplaceType> MarketplacesHasFeed => new[]
    {
        MarketplaceType.Meso,
        MarketplaceType.SberMegaMarket
    };
}