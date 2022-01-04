using AutoMapper;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
{
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateOrderHandler> _logger;
    private const OrderStatus Status = OrderStatus.Created;

    public CreateOrderHandler(IMapper mapper, MContext context, IMediator mediator, ILogger<CreateOrderHandler> logger)
    {
        _mapper = mapper;
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);
        await _mediator.Send(new SaveOrderStatusHistoryRequest()
        {
            Status = Status,
            OrderId = order.Id
        }, cancellationToken);

        _logger.LogInformation($"Created order {order.Number} with {order.ExternalId} external id");

        return order;
    }
}