using Infrastructure.Models.MarketplaceCategorySettings;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Models.Products;

namespace Infrastructure.Models.Marketplaces;

public class MarketplaceProductData
{
    public List<Product> Products { get; }
    public Dictionary<int, Category> Categories { get; }

    private readonly Marketplace _marketplace;
    private readonly Dictionary<int, decimal> _stocks;
    private readonly Dictionary<int, decimal> _prices;
    private readonly Dictionary<int, decimal> _specialPrices;
    private readonly Dictionary<int, MarketplaceCategorySetting> _categorySettings;
    private readonly Dictionary<int, MarketplaceProductSetting> _productSettings;

    public MarketplaceProductData(
        Marketplace marketplace,
        List<Product> products,
        Dictionary<int, Category> categories,
        Dictionary<int, decimal> stocks,
        Dictionary<int, decimal> prices,
        Dictionary<int, decimal> specialPrices,
        Dictionary<int, MarketplaceCategorySetting> categorySettings,
        Dictionary<int, MarketplaceProductSetting> productSettings)
    {
        Products = products;
        Categories = categories;
        _stocks = stocks;
        _prices = prices;
        _categorySettings = categorySettings;
        _productSettings = productSettings;
        _specialPrices = specialPrices;
        _marketplace = marketplace;
    }

    public decimal GetProductPrice(Product product)
    {
        decimal price = 0;
        if (_marketplace.PriceTypeId.HasValue)
        {
            _specialPrices.TryGetValue(product.Id, out price);
        }

        if (price == 0)
        {
            _prices.TryGetValue(product.Id, out price);
        }

        return price;
    }

    public decimal GetProductStock(Product product, decimal price = 0)
    {
        if (_marketplace.NullifyStocks)
        {
            return 0;
        }

        if (!_stocks.TryGetValue(product.Id, out decimal stock))
        {
            return 0;
        }

        if (stock < _marketplace.MinimalStock)
        {
            return 0;
        }

        if (_productSettings.TryGetValue(product.Id, out MarketplaceProductSetting? productSetting)
            && productSetting.NullifyStock)
        {
            return 0;
        }

        if (price == 0)
        {
            price = GetProductPrice(product);
        }

        if (price == 0)
        {
            return 0;
        }

        if (productSetting is not null && productSetting.IgnoreRestrictions)
        {
            return stock;
        }

        if (price < _marketplace.MinimalPrice)
        {
            return 0;
        }

        if (Categories.TryGetValue(product.CategoryId!.Value, out Category? categoryInfo))
        {
            if (categoryInfo.DeletionMark)
            {
                return 0;
            }
        }

        if (_categorySettings.TryGetValue(product.CategoryId.Value, out MarketplaceCategorySetting? categorySetting)
            && categorySetting.Blocked)
        {
            return 0;
        }

        return stock;
    }
}