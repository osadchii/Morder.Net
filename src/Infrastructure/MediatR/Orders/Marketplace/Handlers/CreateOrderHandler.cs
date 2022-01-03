using AutoMapper;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Commands;
using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
{
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public CreateOrderHandler(IMapper mapper, MContext context, IMediator mediator)
    {
        _mapper = mapper;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new TrackOrderChangeRequest(order.Id), cancellationToken);

        return order;
    }
}