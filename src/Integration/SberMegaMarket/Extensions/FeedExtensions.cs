using System.Xml;
using System.Xml.Serialization;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Integration.SberMegaMarket.Feeds;
using CategoryDb = Infrastructure.Models.Products.Category;

namespace Integration.SberMegaMarket.Extensions;

public static class FeedExtensions
{
    public static Offer ToOffer(this Product product, MarketplaceProductData productData, int warehouseId)
    {
        if (!int.TryParse(product.Articul, out int articul))
        {
            return null;
        }

        var price = productData.GetProductPrice(product);
        var stock = productData.GetProductStock(product, price);

        var available = stock > 0;

        var offer = new Offer
        {
            Available = available,
            Barcode = product.Barcode ?? string.Empty,
            Brand = product.Brand,
            Height = product.Height,
            Length = product.Length,
            Width = product.Width,
            Weight = product.Weight,
            CountryOfOrigin = product.CountryOfOrigin,
            Name = product.Name!,
            Price = price,
            Vendor = product.Vendor,
            VendorCode = product.VendorCode,
            Outlets = new List<Outlet>
            {
                new(warehouseId, stock)
            },
            CategoryId = product.CategoryId!.Value,
            Id = articul,
            Vat = product.Vat switch
            {
                Vat.Vat_20 => 1,
                Vat.Vat_20_120 => 1,
                Vat.Vat_10 => 2,
                Vat.Vat_10_110 => 2,
                _ => 5
            }
        };

        return offer;
    }

    public static Feeds.Category ToCategory(this CategoryDb category)
    {
        return new Feeds.Category(category.Id, category.Name!, category.ParentId);
    }

    public static void Save(this Feed feed, string fileName)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            Async = true
        };

        var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

        using var stream = new StreamWriter(fileName);
        using var writer = XmlWriter.Create(stream, settings);
        var serializer = new XmlSerializer(typeof(Feed));
        serializer.Serialize(writer, feed, ns);

        writer.Close();
        stream.Close();
    }
}