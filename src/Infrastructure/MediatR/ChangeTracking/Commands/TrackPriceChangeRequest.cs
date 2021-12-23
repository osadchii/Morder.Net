using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Commands;

public class TrackPriceChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int ProductId { get; set; }

    public TrackPriceChangeRequest()
    {
    }

    public TrackPriceChangeRequest(int marketplaceId, int productId)
    {
        MarketplaceId = marketplaceId;
        ProductId = productId;
    }
}