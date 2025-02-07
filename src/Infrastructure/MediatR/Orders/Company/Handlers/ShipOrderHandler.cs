using System.Net;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Marketplaces.Common.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class ShipOrderHandler : IRequestHandler<ShipOrderRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<ShipOrderHandler> _logger;

    public ShipOrderHandler(MContext context, IMediator mediator, ILogger<ShipOrderHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(ShipOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(o => o.ExternalId == request.ExternalId!.Value, cancellationToken);

        if (order is null)
        {
            throw new HttpRequestException($"Order with {request.ExternalId!.Value} external id not found", null,
                HttpStatusCode.NotFound);
        }

        if (order.Status != OrderStatus.Packed)
        {
            throw new HttpRequestException($"Order can be shipped only in {nameof(OrderStatus.Packed)} status");
        }

        order.Status = OrderStatus.Shipped;
        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);
        await _mediator.Send(new CreateMarketplaceOrderTaskRequest
        {
            Type = TaskType.Ship,
            MarketplaceId = order.MarketplaceId,
            OrderId = order.Id
        }, cancellationToken);

        if (order.Status == OrderStatus.Shipped)
        {
            await _mediator.Send(new SaveOrderStatusHistoryRequest
            {
                Status = OrderStatus.Shipped,
                OrderId = order.Id,
                User = request.User
            }, cancellationToken);
        }

        _logger.LogInformation("Shipped order {OrderNumber} with {OrderExternalId} external id by {UserName}", 
            order.Number, order.ExternalId, request.User);

        return Unit.Value;
    }
}