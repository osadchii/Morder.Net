using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models.Products;

[Table("Product", Schema = "dbo")]
public class Product : BaseEntity, IHasId, IHasExternalId, IHasDeletionMark
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(Limits.ProductName)]
    public string Name { get; set; }

    [Required]
    [MaxLength(Limits.ProductArticul)]
    public string Articul { get; set; }

    [Required] public Guid ExternalId { get; set; }

    [ForeignKey("Category")] public int? CategoryId { get; set; }

    public Category Category { get; set; }

    public bool DeletionMark { get; set; }

    [MaxLength(Limits.ProductCountryOfOrigin)]
    public string CountryOfOrigin { get; set; }

    [Range(Limits.MinimalLength, Limits.MaximalLength)]
    public decimal? Length { get; set; }

    [Range(Limits.MinimalWidth, Limits.MaximalWidth)]
    public decimal? Width { get; set; }

    [Range(Limits.MinimalHeight, Limits.MaximalHeight)]
    public decimal? Height { get; set; }

    [Range(Limits.MinimalWeight, Limits.MaximalWeight)]
    public decimal? Weight { get; set; }
    
    [Range(Limits.MinimalVolume, Limits.MaximalVolume)]
    public decimal? Volume { get; set; }
    [MaxLength(Limits.ProductBrand)] public string Brand { get; set; }

    [MaxLength(Limits.ProductVendor)] public string Vendor { get; set; }

    [MaxLength(Limits.ProductVendorCode)] public string VendorCode { get; set; }

    [MaxLength(Limits.ProductBarcode)] public string Barcode { get; set; }

    public Vat? Vat { get; set; }

    public ProductType? ProductType { get; set; }
}