using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;

namespace Infrastructure.MediatR.Marketplaces;

public abstract class BaseUpdateMarketplaceRequest
{
    public int? Id { get; set; }

    [Required] public string? Name { get; set; }

    [Required] public List<ProductType>? ProductTypes { get; set; }

    [Required] public decimal? MinimalPrice { get; set; }

    [Required] public decimal? MinimalStock { get; set; }

    [Required] public Guid? WarehouseExternalId { get; set; }

    public Warehouse? Warehouse { get; set; }

    public bool IsActive { get; set; }
    public bool NullifyStocks { get; set; }

    public Guid? PriceTypeExternalId { get; set; }

    public PriceType? PriceType { get; set; }
}