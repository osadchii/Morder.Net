using System.Net;
using Infrastructure.Extensions;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Marketplaces.Common.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.TaskContext;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class RejectOrderHandler : IRequestHandler<RejectOrderRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<RejectOrderHandler> _logger;
    private const OrderStatus Status = OrderStatus.Canceled;

    public RejectOrderHandler(MContext context, IMediator mediator, ILogger<RejectOrderHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(RejectOrderRequest request, CancellationToken cancellationToken)
    {
        if (!request.Items!.Any())
        {
            throw new HttpRequestException("Need one or more items");
        }

        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(o => o.ExternalId == request.ExternalId!.Value, cancellationToken);

        if (order is null)
        {
            throw new HttpRequestException($"Order with {request.ExternalId!.Value} external id not found", null,
                HttpStatusCode.NotFound);
        }

        if (order.Status != OrderStatus.Reserved && order.Status != OrderStatus.Created)
        {
            throw new HttpRequestException(
                $"Order can be rejected only in {nameof(OrderStatus.Created)} or {nameof(OrderStatus.Reserved)} status");
        }

        var taskContext = new RejectOrderContext();

        foreach (var item in request.Items!)
        {
            var toReject = item.Count!.Value;

            var orderItems = order.Items
                .Where(i => !i.Canceled && i.Product.ExternalId == item.ProductExternalId);

            foreach (var orderItem in orderItems)
            {
                var rejectingCount = Math.Min(toReject, orderItem.Count);

                if (rejectingCount != orderItem.Count)
                {
                    var newLine = new Order.OrderItem
                    {
                        Canceled = false,
                        Count = orderItem.Count - rejectingCount,
                        Price = orderItem.Price,
                        Sum = (orderItem.Count - rejectingCount) * orderItem.Price,
                        ExternalId = orderItem.ExternalId,
                        ProductId = orderItem.ProductId
                    };

                    order.Items.Add(newLine);
                }

                orderItem.Count = rejectingCount;
                orderItem.Sum = orderItem.Count * orderItem.Price;
                orderItem.Canceled = true;
                
                taskContext.Add(new RejectOrderContextItem
                {
                    ProductId = orderItem.ProductId,
                    Articul = orderItem.Product.Articul!,
                    ItemIndex = orderItem.ExternalId ?? string.Empty,
                    Count = orderItem.Count
                });

                toReject -= rejectingCount;

                if (toReject == 0)
                {
                    break;
                }
            }

            if (toReject > 0)
            {
                throw new HttpRequestException("There are not enough items in the order to cancel");
            }
        }

        if (order.Items.All(i => i.Canceled))
        {
            order.Status = Status;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);
        await _mediator.Send(new CreateMarketplaceOrderTaskRequest
        {
            Type = TaskType.Reject,
            MarketplaceId = order.MarketplaceId,
            OrderId = order.Id,
            TaskContext = taskContext.ToJson()
        }, cancellationToken);

        if (order.Status == Status)
        {
            await _mediator.Send(new SaveOrderStatusHistoryRequest
            {
                Status = Status,
                OrderId = order.Id,
                User = request.User
            }, cancellationToken);
        }

        _logger.LogInformation("Packed order {Number} with {ExternalId} external id by {UserName}", 
            order.Number, order.ExternalId, request.User);

        return Unit.Value;
    }
}