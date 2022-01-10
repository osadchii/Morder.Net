using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Meso;
using Infrastructure.Models.Products;
using Integration.Meso.Feeds;

namespace Integration.Meso.Extensions;

public static class FeedExtensions
{
    public static Task Save(this Feed feed, string fileName)
    {
        return File.WriteAllTextAsync(fileName, feed.ToJson());
    }

    public static MesoProduct? ToMesoProduct(this Product product, MarketplaceProductData productData, MesoDto _meso)
    {
        decimal price = productData.GetProductPrice(product);
        decimal stock = productData.GetProductStock(product, price);

        if (stock < _meso.MinimalStock || price < _meso.MinimalPrice)
        {
            return null;
        }

        var offer = new MesoProduct
        {
            Price = price,
            Code = product.Barcode,
            InternalCode = product.Articul!,
            MeasureUnit = product.ProductType switch
            {
                ProductType.Weight => "кг",
                _ => "шт"
            },
            ProductName = product.Name!,
            RemainsCount = stock,
            NdsPercent = product.Vat switch
            {
                Vat.No_vat => 0,
                Vat.Vat_0 => 0,
                Vat.Vat_10 => 10,
                Vat.Vat_10_110 => 10,
                Vat.Vat_20 => 20,
                Vat.Vat_20_120 => 20,
                _ => 0
            }
        };

        return offer;
    }
}