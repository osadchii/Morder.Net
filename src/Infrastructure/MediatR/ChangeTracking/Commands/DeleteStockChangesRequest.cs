using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Commands;

public class DeleteStockChangesRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public List<int> ProductIds { get; set; }

    public DeleteStockChangesRequest()
    {
    }

    public DeleteStockChangesRequest(int marketplaceId, List<int> ids)
    {
        MarketplaceId = marketplaceId;
        ProductIds = ids;
    }
}