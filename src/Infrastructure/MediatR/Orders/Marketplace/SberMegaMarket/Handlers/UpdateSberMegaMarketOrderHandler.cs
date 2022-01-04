using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.MediatR.Orders.Marketplace.SberMegaMarket.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Marketplace.SberMegaMarket.Handlers;

public class UpdateSberMegaMarketOrderHandler : IRequestHandler<UpdateSberMegaMarketOrderRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<UpdateSberMegaMarketOrderHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateSberMegaMarketOrderHandler(MContext context, ILogger<UpdateSberMegaMarketOrderHandler> logger,
        IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateSberMegaMarketOrderRequest request, CancellationToken cancellationToken)
    {
        Order? order = await _context.Orders
            .SingleOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
        {
            throw new Exception($"Order with {request.OrderId} id for {request.ShipmentId} shipment id not found");
        }

        OrderStatus initialStatus = order.Status;

        order.Customer = request.CustomerFullName;
        order.CustomerAddress = request.CustomerAddress;
        order.ConfirmedTimeLimit = request.ConfirmedTimeLimit;
        order.PackingTimeLimit = request.PackingTimeLimit;
        order.ShippingTimeLimit = request.ShippingTimeLimit;

        if (request.Items.All(i => i.Canceled))
        {
            order.Status = OrderStatus.Canceled;
        }
        else if (request.Items.Where(i => !i.Canceled).All(i => i.Finished))
        {
            order.Status = OrderStatus.Finished;
        }

        foreach (UpdateSberMegaMarketOrderRequestItem item in request.Items.Where(i => i.Canceled))
        {
            IEnumerable<Order.OrderItem> orderItems =
                order.Items.Where(oi => !oi.Canceled && oi.ExternalId == item.ItemIndex);

            foreach (Order.OrderItem orderItem in orderItems)
            {
                orderItem.Canceled = true;
            }
        }

        int saved = await _context.SaveChangesAsync(cancellationToken);

        if (saved == 0)
        {
            return Unit.Value;
        }

        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);

        if (initialStatus != order.Status)
        {
            await _mediator.Send(new SaveOrderStatusHistoryRequest()
            {
                Status = order.Status,
                OrderId = order.Id
            }, cancellationToken);
        }

        return Unit.Value;
    }
}