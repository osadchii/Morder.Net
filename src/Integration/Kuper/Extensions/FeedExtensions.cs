using System.Globalization;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using Integration.Kuper.Feeds;
using static Integration.Kuper.Feeds.KuperProductFeed.KuperFeedAttributes;

namespace Integration.Kuper.Extensions;

public static class FeedExtensions
{
    public static Task Save(this KuperProductFeed feed, string fileName)
    {
        return File.WriteAllTextAsync(fileName, feed.ToJson());
    }

    public static KuperProductFeed.Item ToKuperProduct(this Product product,
        MarketplaceProductData marketplaceProductData,
        IProductImageService productImageService)
    {
        marketplaceProductData.Categories.TryGetValue(product.CategoryId ?? 0, out var category);

        var item = new KuperProductFeed.Item
        {
            Id = product.Articul,
            Name = product.Name,
            Status = "ACTIVE",
            Barcodes = product.Barcode.IsNullOrEmpty() ? null : new[] { product.Barcode },
            PackType = product.ProductType switch
            {
                ProductType.Piece => "per_item",
                ProductType.Weight => "per_kilo",
                _ => null
            },
            CategoryIds = category is not null ? new[] { category.Id.ToString() } : Array.Empty<string>(),
            Images = new[]
            {
                new KuperProductFeed.Item.Image
                {
                    Name = productImageService.GetImageNameByArticul(product.Articul),
                    Url = productImageService.GetImageUrlByArticul(product.Articul)
                }
            }
        };

        var attributes = new List<KuperProductFeed.Item.Attribute>();

        AddAttribute(attributes, IsAlcohol, false);
        AddAttribute(attributes, IsExcisable, false);
        AddAttribute(attributes, IsOwnBrand, false);
        AddAttribute(attributes, IsPrivateLabel, false);
        AddAttribute(attributes, Brand, product.Brand);
        AddAttribute(attributes, Country, product.CountryOfOrigin);
        AddAttribute(attributes, VendorCode, product.VendorCode);
        AddAttribute(attributes, Height, product.Height);
        AddAttribute(attributes, Length, product.Length);

        if (product.Weight.HasValue)
        {
            AddAttribute(attributes, WeightNet, product.Weight.Value);
            AddAttribute(attributes, WeightNetUnit, "G");
        }

        if (product.Volume.HasValue)
        {
            AddAttribute(attributes, WeightNet, product.Volume.Value);
            AddAttribute(attributes, WeightNetUnit, "ML");
        }

        item.Attributes = attributes.ToArray();

        return item;
    }

    private static void AddAttribute(List<KuperProductFeed.Item.Attribute> attributes, string name, string value)
    {
        if (value.IsNullOrEmpty())
        {
            return;
        }

        attributes.Add(new KuperProductFeed.Item.Attribute
        {
            Name = name,
            Values = new[] { value }
        });
    }

    private static void AddAttribute(List<KuperProductFeed.Item.Attribute> attributes, string name, bool value) =>
        AddAttribute(attributes, name, value.ToString().ToLowerInvariant());

    private static void AddAttribute(List<KuperProductFeed.Item.Attribute> attributes, string name, decimal value) =>
        AddAttribute(attributes, name, value.ToString(CultureInfo.InvariantCulture));

    private static void AddAttribute(List<KuperProductFeed.Item.Attribute> attributes, string name, decimal? value)
    {
        if (!value.HasValue)
        {
            return;
        }

        AddAttribute(attributes, name, value.Value);
    }
}