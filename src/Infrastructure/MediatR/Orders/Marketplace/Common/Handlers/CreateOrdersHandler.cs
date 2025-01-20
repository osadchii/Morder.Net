using AutoMapper;
using Infrastructure.MediatR.ChangeTracking.Orders.Commands;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Handlers;

public class CreateOrdersHandler : IRequestHandler<CreateOrdersRequest, IEnumerable<Order>>
{
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateOrdersHandler> _logger;

    public CreateOrdersHandler(IMapper mapper, MContext context, IMediator mediator,
        ILogger<CreateOrdersHandler> logger)
    {
        _mapper = mapper;
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> Handle(CreateOrdersRequest request, CancellationToken cancellationToken)
    {
        var orders = request.CreateOrderRequests.Select(r => _mapper.Map<Order>(r)).ToArray();

        await _context.AddRangeAsync(orders, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new TrackOrdersChangeRequest()
        {
            OrderIds = orders.Where(o => !o.Archived).Select(o => o.Id)
        }, cancellationToken);

        await _mediator.Send(new SaveOrdersStatusHistoryRequest()
        {
            Requests = orders.Select(o => new SaveOrderStatusHistoryRequest()
            {
                Status = o.Status,
                OrderId = o.Id
            })
        }, cancellationToken);

        _logger.LogInformation("Created {Count} orders",
            orders.Length);

        return orders;
    }
}