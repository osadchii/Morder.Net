using MediatR;

namespace Infrastructure.MediatR.ChangeTracking.Orders.Commands;

public class TrackOrderChangeRequest : IRequest<Unit>
{
    public int OrderId { get; set; }

    public TrackOrderChangeRequest()
    {
    }

    public TrackOrderChangeRequest(int orderId)
    {
        OrderId = orderId;
    }
}