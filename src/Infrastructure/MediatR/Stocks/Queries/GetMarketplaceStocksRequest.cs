using Infrastructure.Models.Warehouses;
using MediatR;

namespace Infrastructure.MediatR.Stocks.Queries;

public class GetMarketplaceStocksRequest : IRequest<IEnumerable<MarketplaceStockDto>>
{
    public int MarketplaceId { get; set; }
    public int Limit { get; set; }
}