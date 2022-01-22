using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Orders.Commands;

public class TrackOrdersChangeRequest : IRequest<Unit>
{
    public IEnumerable<int> OrderIds { get; set; }
}