using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Commands;

public class TrackAllPricesRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
}