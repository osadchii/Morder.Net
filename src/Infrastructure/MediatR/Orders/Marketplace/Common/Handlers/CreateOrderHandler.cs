using AutoMapper;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
{
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(IMapper mapper, MContext context, IMediator mediator, ILogger<CreateOrderHandler> logger)
    {
        _mapper = mapper;
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Where(o =>  o.Number == request.Number)
            .Where(o => o.MarketplaceId == request.MarketplaceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            order = _mapper.Map<Order>(request);
            await _context.Orders.AddAsync(order, cancellationToken);
        }
        else
        {
            request.ExternalId = order.ExternalId;
            if (order.Status != OrderStatus.Created && request.Status == OrderStatus.Created)
            {
                request.Status = order.Status;
            }
            _mapper.Map(request, order);
        }
        await _context.SaveChangesAsync(cancellationToken);

        if (!order.Archived)
        {
            await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);
        }

        await _mediator.Send(new SaveOrderStatusHistoryRequest
        {
            Status = order.Status,
            OrderId = order.Id
        }, cancellationToken);

        _logger.LogInformation("Created order {Number} with {ExternalId} external id",
            order.Number, order.ExternalId);

        return order;
    }
}