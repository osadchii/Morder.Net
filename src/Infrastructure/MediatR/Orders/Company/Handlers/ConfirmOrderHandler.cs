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

public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<ConfirmOrderHandler> _logger;
    private const OrderStatus Status = OrderStatus.Reserved;

    public ConfirmOrderHandler(MContext context, IMediator mediator, ILogger<ConfirmOrderHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(ConfirmOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .SingleOrDefaultAsync(o => o.ExternalId == request.ExternalId!.Value, cancellationToken);

        if (order is null)
        {
            throw new HttpRequestException($"Order with {request.ExternalId!.Value} external id not found", null,
                HttpStatusCode.NotFound);
        }

        if (order.Status != OrderStatus.Created)
        {
            throw new HttpRequestException($"Order can be confirmed only in {nameof(OrderStatus.Created)} status");
        }

        order.Status = Status;

        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);
        await _mediator.Send(new CreateMarketplaceOrderTaskRequest()
        {
            Type = TaskType.Confirm,
            MarketplaceId = order.MarketplaceId,
            OrderId = order.Id
        }, cancellationToken);
        await _mediator.Send(new SaveOrderStatusHistoryRequest()
        {
            Status = Status,
            OrderId = order.Id,
            User = request.User
        }, cancellationToken);

        _logger.LogInformation("Confirmed order {OrderNumber} with {OrderExternalId} external id by {UserName}", order.Number, order.ExternalId, request.User);

        return Unit.Value;
    }
}