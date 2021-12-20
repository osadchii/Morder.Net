using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Commands;

public class TrackStockChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }

    public TrackStockChangeRequest()
    {
    }

    public TrackStockChangeRequest(int marketplaceId, int warehouseId, int productId)
    {
        MarketplaceId = marketplaceId;
        WarehouseId = warehouseId;
        ProductId = productId;
    }
}