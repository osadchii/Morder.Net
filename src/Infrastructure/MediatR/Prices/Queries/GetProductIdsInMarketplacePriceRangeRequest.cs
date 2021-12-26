using MediatR;

namespace Infrastructure.MediatR.Prices.Queries;

public class GetProductIdsInMarketplacePriceRangeRequest : IRequest<IEnumerable<int>>
{
    public int MarketplaceId { get; set; }
    public decimal MinimalPrice { get; set; }
    public decimal MaximalPrice { get; set; }
}