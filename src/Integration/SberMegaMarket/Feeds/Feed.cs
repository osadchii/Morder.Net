using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;
using Infrastructure.Extensions;

namespace Integration.SberMegaMarket.Feeds;

[XmlRoot("yml_catalog")]
public class Feed
{
    [XmlAttribute("date")]
    public string Date { get; set; }
        = DateTime.UtcNow.ToMoscowTime()
            .ToString("yyyy-MM-dd HH:mm");

    [XmlElement("shop")] public Shop Shop { get; set; }

    public Feed()
    {
        Shop = new Shop();
    }
}

public class Shop
{
    [XmlElement("name")] public string Name { get; set; }

    [XmlElement("company")] public string Company { get; set; }

    [XmlElement("url")] public string Url { get; set; }

    [XmlArray("currencies")]
    [XmlArrayItem("currency")]
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public List<Currency> Currencies { get; set; }

    [XmlArray("shipment-options")]
    [XmlArrayItem("option")]
    // ReSharper disable once CollectionNeverQueried.Global
    public List<ShipmentOption> ShipmentOptions { get; set; }

    [XmlArray("categories")]
    [XmlArrayItem("category")]
    // ReSharper disable once CollectionNeverQueried.Global
    public List<Category> Categories { get; set; }

    [XmlArray("offers")]
    [XmlArrayItem("offer")]
    // ReSharper disable once CollectionNeverQueried.Global
    public List<Offer> Offers { get; set; }

    public Shop()
    {
        Currencies = new List<Currency>
        {
            new("RUR", 1)
        };

        ShipmentOptions = new List<ShipmentOption>();
        Categories = new List<Category>();
        Offers = new List<Offer>();
    }
}

public class Offer
{
    [XmlAttribute("id")] public int Id { get; set; }

    [XmlAttribute("available")] public bool Available { get; set; }

    [XmlElement("name")] public string Name { get; set; }

    [XmlElement("price")] public decimal Price { get; set; }

    [XmlElement("categoryId")] public int CategoryId { get; set; }

    [XmlArray("outlets")]
    [XmlArrayItem("outlet")]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public List<Outlet> Outlets { get; set; }

    [XmlElement("vat")] public int? Vat { get; set; }

    [XmlElement("barcode")] public string Barcode { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string Vendor { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string VendorCode { get; set; }

    [XmlIgnore] public decimal? Weight { get; set; }

    [XmlIgnore] public decimal? Width { get; set; }

    [XmlIgnore] public decimal? Length { get; set; }

    [XmlIgnore] public decimal? Height { get; set; }

    [XmlIgnore] public string Brand { get; set; }

    [XmlIgnore] public string CountryOfOrigin { get; set; }

    [XmlElement("param")]
    public List<OfferParam> Params
    {
        get
        {
            List<OfferParam> offerParams = new();

            if (Weight.HasValue)
                offerParams.Add(new OfferParam(nameof(Weight), Weight.Value));

            if (Width.HasValue)
                offerParams.Add(new OfferParam(nameof(Width), Width.Value));

            if (Length.HasValue)
                offerParams.Add(new OfferParam(nameof(Length), Length.Value));

            if (Height.HasValue)
                offerParams.Add(new OfferParam(nameof(Height), Height.Value));

            if (!Brand.IsNullOrEmpty())
                offerParams.Add(new OfferParam("Бренд", Brand!));

            if (!CountryOfOrigin.IsNullOrEmpty())
                offerParams.Add(new OfferParam("СтранаИзготовитель", CountryOfOrigin!));


            return offerParams;
        }
    }

    // ReSharper disable once EmptyConstructor
#pragma warning disable CS8618
    public Offer()
#pragma warning restore CS8618
    {
    }
}

public class OfferParam
{
    [XmlAttribute("name")] public string Name { get; set; }

    [XmlText] public string Text { get; set; }

#pragma warning disable CS8618
    public OfferParam()
#pragma warning restore CS8618
    {
    }

    public OfferParam(string name, string text)
    {
        Name = name;
        Text = text;
    }

    public OfferParam(string name, decimal value)
    {
        Name = name;
        Text = value.ToString(CultureInfo.InvariantCulture);
    }
}

public class Outlet
{
    [XmlAttribute("outletId")] public int OutletId { get; set; }

    [XmlAttribute("instock")] public decimal InStock { get; set; }

    public Outlet()
    {
    }

    public Outlet(int id, decimal stock)
    {
        OutletId = id;
        InStock = stock;
    }
}

public class ShipmentOption
{
    [XmlAttribute("days")] public int Days { get; set; }

    [XmlAttribute("order-before")] public int OrderBefore { get; set; }

    public ShipmentOption()
    {
    }

    public ShipmentOption(int days, int orderBefore)
    {
        Days = days;
        OrderBefore = orderBefore;
    }
}

public class Category
{
    [XmlAttribute("id")] public int Id { get; set; }

    [XmlIgnore] public int? ParentId { get; set; }

    [XmlAttribute("parentId")]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public int ParentIdSerialised
    {
        get => ParentId ?? 0;
        set => ParentId = value;
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeParentIdSerialised()
    {
        return ParentId.HasValue;
    }

    [XmlText] public string Name { get; set; }

#pragma warning disable CS8618
    public Category()
#pragma warning restore CS8618
    {
    }

    public Category(int id, string name, int? parentId = null)
    {
        Id = id;
        Name = name;
        ParentId = parentId;
    }
}

public class Currency
{
    [XmlAttribute("id")] public string Id { get; set; }

    [XmlAttribute("rate")] public int Rate { get; set; }

#pragma warning disable CS8618
    public Currency()
#pragma warning restore CS8618
    {
    }

    public Currency(string id, int rate)
    {
        Id = id;
        Rate = rate;
    }
}