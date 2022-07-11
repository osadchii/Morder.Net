using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;

namespace Infrastructure.Models.Marketplaces;

public abstract class MarketplaceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductType> ProductTypes { get; set; }
    public decimal MinimalPrice { get; set; }
    public decimal MinimalStock { get; set; }
    public Guid WarehouseExternalId { get; set; }
    public Warehouse Warehouse { get; set; }
    public bool IsActive { get; set; }
    public bool NullifyStocks { get; set; }
    public bool StockChangesTracking { get; set; }
    public bool PriceChangesTracking { get; set; }
    public int StockSendLimit { get; set; }
    public int PriceSendLimit { get; set; }
    public Guid? PriceTypeExternalId { get; set; }
    public PriceType PriceType { get; set; }
}