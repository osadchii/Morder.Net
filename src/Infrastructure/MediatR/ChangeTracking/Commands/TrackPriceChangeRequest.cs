using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Commands;

public class TrackPriceChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int PriceTypeId { get; set; }
    public int ProductId { get; set; }

    public TrackPriceChangeRequest()
    {
    }

    public TrackPriceChangeRequest(int marketplaceId, int priceTypeId, int productId)
    {
        MarketplaceId = marketplaceId;
        PriceTypeId = priceTypeId;
        ProductId = productId;
    }
}