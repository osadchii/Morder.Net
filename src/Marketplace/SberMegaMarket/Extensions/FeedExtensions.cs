using System.Xml;
using System.Xml.Serialization;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Marketplace.SberMegaMarket.Feeds;
using Category = Marketplace.SberMegaMarket.Feeds.Category;
using CategoryDb = Infrastructure.Models.Products.Category;

namespace Marketplace.SberMegaMarket.Extensions;

public static class FeedExtensions
{
    public static Offer ToOffer(this Product product, MarketplaceProductData productData, int warehouseId)
    {
        decimal price = productData.GetProductPrice(product);
        decimal stock = productData.GetProductStock(product, price);

        if (!int.TryParse(product.Articul, out int articul))
        {
            throw new ArgumentException("Wrong articul");
        }

        bool available = stock > 0;

        var offer = new Offer
        {
            Available = available,
            Barcode = product.Barcode,
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

    public static Category ToCategory(this CategoryDb category)
    {
        return new Category(category.Id, category.Name!, category.ParentId);
    }

    public static void Save(this Feed feed, string fileName)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            Async = true
        };
        // Remove Namespace
        var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

        using var stream = new StreamWriter(fileName);
        using var writer = XmlWriter.Create(stream, settings);
        var serializer = new XmlSerializer(typeof(Feed));
        serializer.Serialize(writer, feed, ns);

        writer.Close();
        stream.Close();
    }
}