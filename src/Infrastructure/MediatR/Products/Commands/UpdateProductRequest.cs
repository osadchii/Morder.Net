using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Products.Commands;

public class UpdateProductRequest : IRequest<Unit>
{
    [Required]
    [MaxLength(Limits.ProductName)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(Limits.ProductArticul)]
    public string? Articul { get; set; }

    [Required] public Guid? ExternalId { get; set; }

    public Guid? CategoryId { get; set; }

    public Category? Category { get; set; }

    public bool DeletionMark { get; set; }

    [Range(Limits.MinimalLength, Limits.MaximalLength)]
    public decimal? Length { get; set; }

    [Range(Limits.MinimalWidth, Limits.MaximalWidth)]
    public decimal? Width { get; set; }

    [Range(Limits.MinimalHeight, Limits.MaximalHeight)]
    public decimal? Height { get; set; }

    [Range(Limits.MinimalWeight, Limits.MaximalWeight)]
    public decimal? Weight { get; set; }

    [MaxLength(Limits.ProductBrand)] public string? Brand { get; set; }

    [MaxLength(Limits.ProductVendor)] public string? Vendor { get; set; }

    [MaxLength(Limits.ProductVendorCode)] public string? VendorCode { get; set; }

    [MaxLength(Limits.ProductBarcode)] public string? Barcode { get; set; }

    [EnumDataType(typeof(Vat))] public Vat? Vat { get; set; }
}