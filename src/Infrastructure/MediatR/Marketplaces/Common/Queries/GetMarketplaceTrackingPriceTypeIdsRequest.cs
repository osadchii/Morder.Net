using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Queries;

public class GetMarketplaceTrackingPriceTypeIdsRequest : IRequest<List<int>>
{
    public GetMarketplaceTrackingPriceTypeIdsRequest(int marketplaceId)
    {
        MarketplaceId = marketplaceId;
    }

    public GetMarketplaceTrackingPriceTypeIdsRequest()
    {
    }

    public int MarketplaceId { get; set; }
}