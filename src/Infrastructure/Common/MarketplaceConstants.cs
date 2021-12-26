using Infrastructure.Marketplaces;

namespace Infrastructure.Common;

public static class MarketplaceConstants
{
    public static IEnumerable<MarketplaceType> MarketplacesHasNoExternalProductId => new[]
    {
        MarketplaceType.SberMegaMarket
    };
}