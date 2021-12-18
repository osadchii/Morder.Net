using Infrastructure.Models.Companies;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Products;
using Marketplace.SberMegaMarket.Extensions;
using Microsoft.Extensions.Logging;
using DbCategory = Infrastructure.Models.Products.Category;

namespace Marketplace.SberMegaMarket.Feeds;

public class FeedBuilder
{
    private readonly Feed _feed;
    private readonly SberMegaMarketDto _sber;
    private readonly ILogger? _logger;

    public FeedBuilder(MarketplaceProductData data, SberMegaMarketDto sber, ILogger? logger = null)
    {
        _sber = sber;
        _logger = logger;
        _feed = new Feed();

        AddCategories(data.Categories.Values.ToList());
        AddProducts(data.Products, data);
    }

    private FeedBuilder AddCategories(List<DbCategory> categories)
    {
        foreach (DbCategory category in categories)
        {
            _feed.Shop.Categories.Add(category.ToCategory());
        }

        return this;
    }

    private FeedBuilder AddProducts(List<Product> products, MarketplaceProductData marketplaceProductData)
    {
        foreach (Product product in products)
        {
            try
            {
                _feed.Shop.Offers.Add(product.ToOffer(marketplaceProductData, _sber.Settings.WarehouseId));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while adding product to feed");
            }
        }

        return this;
    }

    public FeedBuilder AddCompanyInformation(CompanyDto companyDto)
    {
        _feed.Shop.Company = companyDto.Name;
        _feed.Shop.Name = companyDto.Shop;
        _feed.Shop.Url = companyDto.Url;

        return this;
    }

    public Feed Build()
    {
        return _feed;
    }
}