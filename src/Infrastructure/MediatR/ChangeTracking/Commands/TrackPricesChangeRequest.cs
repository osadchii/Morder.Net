using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Commands;

public class TrackPricesChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public IEnumerable<int> ProductIds { get; set; }
}