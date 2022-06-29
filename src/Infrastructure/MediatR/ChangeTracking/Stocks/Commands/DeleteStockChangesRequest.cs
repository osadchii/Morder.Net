using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Commands;

public class DeleteStockChangesRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public List<int> ProductIds { get; set; }

#pragma warning disable CS8618
    public DeleteStockChangesRequest()
#pragma warning restore CS8618
    {
    }

    public DeleteStockChangesRequest(int marketplaceId, List<int> ids)
    {
        MarketplaceId = marketplaceId;
        ProductIds = ids;
    }
}