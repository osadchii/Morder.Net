namespace Infrastructure.Models.Products;

public class ProductDto
{
    public string? Name { get; set; }

    public string? Articul { get; set; }

    public Guid ExternalId { get; set; }

    public Guid? CategoryId { get; set; }

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
}