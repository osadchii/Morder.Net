using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Commands;

public class TrackAllStocksRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
}