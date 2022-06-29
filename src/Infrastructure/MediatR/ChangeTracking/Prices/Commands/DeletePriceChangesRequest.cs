using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Commands;

public class DeletePriceChangesRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public List<int> ProductIds { get; set; }

#pragma warning disable CS8618
    public DeletePriceChangesRequest()
#pragma warning restore CS8618
    {
    }

    public DeletePriceChangesRequest(int marketplaceId, List<int> ids)
    {
        MarketplaceId = marketplaceId;
        ProductIds = ids;
    }
}