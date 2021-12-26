using Infrastructure.Models.Prices;
using MediatR;

namespace Infrastructure.MediatR.Prices.Queries;

public class GetMarketplacePricesRequest : IRequest<IEnumerable<MarketplacePriceDto>>
{
    public int MarketplaceId { get; set; }
    public int Limit { get; set; }
}