using Infrastructure.Models.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Queries;

public class GetMarketplaceProductDataRequest : IRequest<MarketplaceProductData>
{
    public int MarketplaceId { get; set; }
}