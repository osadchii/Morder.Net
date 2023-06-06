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

public class PackOrderHandler : IRequestHandler<PackOrderRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<PackOrderHandler> _logger;
    private const OrderStatus Status = OrderStatus.Packed;

    public PackOrderHandler(MContext context, ILogger<PackOrderHandler> logger, IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(PackOrderRequest request, CancellationToken cancellationToken)
    {
        if (!request.Items!.Any())
        {
            throw new HttpRequestException("Need one or more boxes");
        }

        var numbers = request.Items!.Select(i => i.Number!.Value).Distinct().ToList();
        numbers.Sort();

        if (numbers[0] != 1 || numbers.Where((t, i) => t != i + 1).Any())
        {
            throw new HttpRequestException("Box numbers are out of order");
        }

        Order order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(o => o.ExternalId == request.ExternalId!.Value, cancellationToken);

        if (order is null)
        {
            throw new HttpRequestException($"Order with {request.ExternalId!.Value} external id not found", null,
                HttpStatusCode.NotFound);
        }

        if (order.Status != OrderStatus.Reserved)
        {
            throw new HttpRequestException($"Order can be confirmed only in {nameof(OrderStatus.Reserved)} status");
        }

        order.Boxes.Clear();

        foreach (PackOrderItem item in request.Items!)
        {
            int? productId = order.Items
                .Where(p => p.Product.ExternalId == item.ProductExternalId!.Value)
                .Select(i => i.ProductId)
                .FirstOrDefault();

            if (productId is null or 0)
            {
                throw new HttpRequestException($"Product with {item.ProductExternalId!.Value} external id not found");
            }

            order.Boxes.Add(new Order.OrderBox()
            {
                Count = item.Count!.Value,
                ProductId = productId.Value,
                Number = item.Number!.Value
            });
        }

        var itemCounts = order.Items
            .Where(i => !i.Canceled).GroupBy(i => i.ProductId, i => i.Count)
            .ToDictionary(i => i.Key, i => i.Sum());

        var boxCounts = order.Boxes
            .GroupBy(i => i.ProductId, i => i.Count)
            .ToDictionary(i => i.Key, i => i.Sum());

        var productIds = itemCounts.Keys.Union(boxCounts.Keys).Distinct();

        var wrongPack = productIds.Any(id =>
        {
            itemCounts.TryGetValue(id, out var itemCount);
            boxCounts.TryGetValue(id, out var boxCount);

            return itemCount != boxCount;
        });

        if (wrongPack)
        {
            throw new HttpRequestException("Count in no canceled items differs from count items in boxes");
        }

        order.Status = Status;
        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);
        await _mediator.Send(new CreateMarketplaceOrderTaskRequest()
        {
            Type = TaskType.Pack,
            MarketplaceId = order.MarketplaceId,
            OrderId = order.Id
        }, cancellationToken);

        await _mediator.Send(new CreateMarketplaceOrderTaskRequest()
        {
            Type = TaskType.Sticker,
            MarketplaceId = order.MarketplaceId,
            OrderId = order.Id
        }, cancellationToken);
        await _mediator.Send(new SaveOrderStatusHistoryRequest()
        {
            Status = Status,
            OrderId = order.Id,
            User = request.User
        }, cancellationToken);

        _logger.LogInformation("Packed order {OrderNumber} with {OrderExternalId} external id by {UserName}", 
            order.Number, order.ExternalId, request.User);

        return Unit.Value;
    }
}