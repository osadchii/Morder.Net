using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Queries;

public class GetMarketplaceTrackingPriceIdsRequest : IRequest<List<int>>
{
}