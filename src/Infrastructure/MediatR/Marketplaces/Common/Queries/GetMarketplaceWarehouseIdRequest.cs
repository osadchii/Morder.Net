using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Queries;

public class GetMarketplaceWarehouseIdRequest : IRequest<int>
{
    public GetMarketplaceWarehouseIdRequest(int marketplaceId)
    {
        MarketplaceId = marketplaceId;
    }

    public GetMarketplaceWarehouseIdRequest()
    {
    }

    public int MarketplaceId { get; set; }
}