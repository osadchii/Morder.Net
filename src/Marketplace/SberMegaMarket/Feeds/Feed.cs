using System.ComponentModel;
using System.Xml.Serialization;

namespace Marketplace.SberMegaMarket.Feeds;

[XmlRootAttribute("yml_catalog")]
public class Feed
{
    [XmlAttribute("date")] public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

    [XmlElement("shop")] public Shop Shop { get; set; }

    public Feed()
    {
        Shop = new Shop();
    }
}

public class Shop
{
    [XmlElement("name")] public string? Name { get; set; }

    [XmlElement("company")] public string? Company { get; set; }

    [XmlElement("url")] public string? Url { get; set; }

    [XmlArray("currencies")]
    [XmlArrayItem("currency")]
    public List<Currency> Currencies { get; set; }

    [XmlArray("shipment-options")]
    [XmlArrayItem("option")]
    public List<ShipmentOption> ShipmentOptions { get; set; }

    [XmlArray("categories")]
    [XmlArrayItem("category")]
    public List<Category> Categories { get; set; }

    [XmlArray("offers")]
    [XmlArrayItem("offer")]
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


    public List<Outlet> Outlets { get; set; }

    [XmlElement("vat")] public int? Vat { get; set; }

    [XmlElement("barcode")] public string Barcode { get; set; }
    public string? Vendor { get; set; }
    public string? VendorCode { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Width { get; set; }
    public decimal? Length { get; set; }
    public decimal? Height { get; set; }
    public string? Brand { get; set; }
    public string? CountryOfOrigin { get; set; }

    public Offer()
    {
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

    public Category()
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

    public Currency()
    {
    }

    public Currency(string id, int rate)
    {
        Id = id;
        Rate = rate;
    }
}