using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Prices.Commands;

public class TrackAllPricesRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
}