using MediatR;

namespace Infrastructure.MediatR.Products.Queries;

public class GetAllMarketplaceProductIdsRequest : IRequest<List<int>>
{
    public int MarketplaceId { get; set; }
}