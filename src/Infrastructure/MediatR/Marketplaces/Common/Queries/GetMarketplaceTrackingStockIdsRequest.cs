using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Queries;

public class GetMarketplaceTrackingStockIdsRequest : IRequest<List<int>>
{
}