using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Commands;

public class TrackAllStocksRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
}