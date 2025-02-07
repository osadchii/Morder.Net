using System.Net;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Handlers;

public class CancelOrderItemsByExternalIdHandler : IRequestHandler<CancelOrderItemsByExternalIdRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<CancelOrderItemsByExternalIdHandler> _logger;
    private const OrderStatus Status = OrderStatus.Canceled;

    public CancelOrderItemsByExternalIdHandler(MContext context, IMediator mediator,
        ILogger<CancelOrderItemsByExternalIdHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(CancelOrderItemsByExternalIdRequest request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .SingleOrDefaultAsync(o => o.MarketplaceId == request.MarketplaceId && o.Number == request.OrderNumber,
                cancellationToken);

        if (order is null)
        {
            throw new HttpRequestException($"Order with {request.OrderNumber} number not found", null,
                HttpStatusCode.NotFound);
        }

        IEnumerable<Order.OrderItem> items = order.Items.Where(i => request.ItemExternalIds.Contains(i.ExternalId))
            .ToArray();

        foreach (var item in items)
        {
            item.Canceled = true;
        }

        if (order.Items.All(i => i.Canceled))
        {
            order.Status = Status;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);

        if (order.Status == Status)
        {
            await _mediator.Send(new SaveOrderStatusHistoryRequest
            {
                Status = Status,
                OrderId = order.Id
            }, cancellationToken);
        }

        _logger.LogInformation(
            $"Cancelled {items.Count()} in order {order.Number} with {order.ExternalId} external id");

        return Unit.Value;
    }
}