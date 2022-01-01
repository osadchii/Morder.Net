using Infrastructure.Models.Companies;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Products;
using Integration.SberMegaMarket.Extensions;
using Microsoft.Extensions.Logging;
using DbCategory = Infrastructure.Models.Products.Category;

namespace Integration.SberMegaMarket.Feeds;

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

        _feed.Shop.ShipmentOptions.Add(new ShipmentOption(sber.Settings.ShippingDays,
            sber.Settings.OrderBefore));

        AddCategories(data.Categories.Values.ToList());
        AddProducts(data.Products, data);
    }

    private void AddCategories(List<DbCategory> categories)
    {
        foreach (DbCategory category in categories)
        {
            _feed.Shop.Categories.Add(category.ToCategory());
        }
    }

    private void AddProducts(List<Product> products, MarketplaceProductData marketplaceProductData)
    {
        foreach (Offer? offer in products
                     .Select(product => product.ToOffer(marketplaceProductData, _sber.Settings.WarehouseId))
                     .Where(offer => offer is not null && offer.Price != 0))
        {
            _feed.Shop.Offers.Add(offer!);
        }
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