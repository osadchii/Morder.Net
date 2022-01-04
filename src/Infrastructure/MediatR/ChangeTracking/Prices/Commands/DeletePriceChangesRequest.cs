using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Commands;

public class DeletePriceChangesRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public List<int> ProductIds { get; set; }

    public DeletePriceChangesRequest()
    {
    }

    public DeletePriceChangesRequest(int marketplaceId, List<int> ids)
    {
        MarketplaceId = marketplaceId;
        ProductIds = ids;
    }
}