using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Commands;

public class TrackStockChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int ProductId { get; set; }

    public TrackStockChangeRequest()
    {
    }

    public TrackStockChangeRequest(int marketplaceId, int productId)
    {
        MarketplaceId = marketplaceId;
        ProductId = productId;
    }
}