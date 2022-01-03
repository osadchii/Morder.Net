using System.Net;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Orders.Handlers;

public class CancelOrderItemsByExternalIdHandler : IRequestHandler<CancelOrderItemsByExternalIdRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public CancelOrderItemsByExternalIdHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(CancelOrderItemsByExternalIdRequest request, CancellationToken cancellationToken)
    {
        Order? order = await _context.Orders
            .SingleOrDefaultAsync(o => o.MarketplaceId == request.MarketplaceId && o.Number == request.OrderNumber,
                cancellationToken);

        if (order is null)
        {
            throw new HttpRequestException($"Order with {request.OrderNumber} number not found", null,
                HttpStatusCode.NotFound);
        }

        IEnumerable<Order.OrderItem> items = order.Items.Where(i => request.ItemExternalIds.Contains(i.ExternalId));

        foreach (Order.OrderItem item in items)
        {
            item.Canceled = true;
        }

        if (order.Items.All(i => i.Canceled))
        {
            order.Status = OrderStatus.Canceled;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);

        return Unit.Value;
    }
}