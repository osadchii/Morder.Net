using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Stocks.Commands;

public class TrackStocksChangeRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public IEnumerable<int> ProductIds { get; set; } = null!;
}