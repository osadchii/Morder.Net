using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Ozon.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Marketplace.Ozon.Handlers;

public class UpdateOzonOrderHandler : IRequestHandler<UpdateOzonOrderRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<UpdateOzonOrderHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateOzonOrderHandler(MContext context, ILogger<UpdateOzonOrderHandler> logger, IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateOzonOrderRequest request, CancellationToken cancellationToken)
    {
        Order? order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(o => o.Number == request.OrderNumber && o.MarketplaceId == request.MarketplaceId,
                cancellationToken);

        if (order is null)
        {
            throw new Exception(
                $"Order with {request.OrderNumber} id for marketplace with {request.MarketplaceId} id not found");
        }

        OrderStatus initialStatus = order.Status;

        order.Status = request.Status;
        order.ShippingDate = request.ShippingDate;
        order.CustomerAddress = request.CustomerAddress;
        order.TrackNumber = request.TrackNumber;

        if (request.CustomerFullName is not null)
        {
            order.Customer = request.CustomerFullName;
        }

        foreach (UpdateOzonOrderItem item in request.Items)
        {
            IEnumerable<Order.OrderItem> orderItems = order.Items
                .Where(orderItem => orderItem.Product.Articul == item.Articul && !orderItem.Canceled);

            foreach (Order.OrderItem orderItem in orderItems)
            {
                decimal handledCount = Math.Min(item.Count, orderItem.Count);
                item.Count -= handledCount;

                if (orderItem.Count > handledCount)
                {
                    decimal newLineCount = orderItem.Count - handledCount;
                    decimal newLineSum = newLineCount * orderItem.Price;
                    var newLine = new Order.OrderItem()
                    {
                        Canceled = true,
                        Price = orderItem.Price,
                        ProductId = orderItem.ProductId,
                        Count = newLineCount,
                        Sum = newLineSum,
                    };

                    order.Items.Add(newLine);

                    orderItem.Count = handledCount;
                    orderItem.Sum -= newLineSum;
                }

                if (item.Count > 0)
                {
                    throw new Exception(
                        $"Wrong order items count for product {item.Articul} in order {order.Number} from marketplace {order.MarketplaceId}." +
                        $"{Environment.NewLine}{item.Count} more than count in order");
                }
            }
        }

        if (order.Items.All(i => i.Canceled))
        {
            order.Status = OrderStatus.Canceled;
        }

        int saved = await _context.SaveChangesAsync(cancellationToken);

        if (saved == 0)
        {
            return Unit.Value;
        }

        _logger.LogInformation("Order {OrderId} with number {Number} updated from marketplace with id {MarketplaceId}",
            order.Id, order.Number, order.MarketplaceId);

        if (!order.Archived)
        {
            await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);
        }

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