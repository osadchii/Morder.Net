using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Meso;
using Infrastructure.Models.Products;
using Integration.Meso.Extensions;

namespace Integration.Meso.Feeds;

public class FeedBuilder
{
    private readonly Feed _feed;
    private readonly MesoDto _meso;

    public FeedBuilder(MarketplaceProductData data, MesoDto meso)
    {
        _meso = meso;
        _feed = new Feed();

        AddProducts(data.Products, data);
    }

    private void AddProducts(IEnumerable<Product> products, MarketplaceProductData marketplaceProductData)
    {
        foreach (var mesoProduct in products
                     .Select(product => product.ToMesoProduct(marketplaceProductData, _meso))
                     .Where(offer => offer is not null && offer.Price != 0))
        {
            _feed.Products.Add(mesoProduct!);
        }
    }

    public Feed Build()
    {
        return _feed;
    }
}