using TestFramework.Categories;

namespace TestFramework.Products;

public class Product
{
    public string? Name { get; set; }

    public string? Articul { get; set; }

    public string? CountryOfOrigin { get; set; }

    public Guid? ExternalId { get; set; }

    public Guid? CategoryId { get; set; }

    public Category? Category { get; set; }

    public bool DeletionMark { get; set; }

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public string? Brand { get; set; }

    public string? Vendor { get; set; }

    public string? VendorCode { get; set; }

    public string? Barcode { get; set; }

    public Vat? Vat { get; set; }

    public ProductType ProductType { get; set; }

    public static Product Create(Guid id)
    {
        return new Product()
        {
            Name = id.ToString(),
            Articul = id.ToString(),
            DeletionMark = false,
            ExternalId = id,
        };
    }
}
public enum Vat
{
    Vat_20,
    Vat_20_120,
    Vat_10,
    Vat_10_110,
    Vat_0,
    No_vat
}

public enum ProductType
{
    Piece,
    Weight,
    Tobacco,
    Alcohol
}