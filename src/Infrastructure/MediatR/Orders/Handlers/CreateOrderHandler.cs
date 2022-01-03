using AutoMapper;
using Infrastructure.MediatR.Orders.Commands;
using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
{
    private readonly IMapper _mapper;
    private readonly MContext _context;

    public CreateOrderHandler(IMapper mapper, MContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return order;
    }
}