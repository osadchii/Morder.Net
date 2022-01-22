using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class SaveOrdersStatusHistoryHandler : IRequestHandler<SaveOrdersStatusHistoryRequest, Unit>
{
    private readonly MContext _context;

    public SaveOrdersStatusHistoryHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SaveOrdersStatusHistoryRequest request, CancellationToken cancellationToken)
    {
        await _context.OrderStatusHistories.AddRangeAsync(request.Requests.Select(r => new OrderStatusHistory()
        {
            Date = DateTime.UtcNow,
            OrderId = r.OrderId,
            Status = r.Status
        }), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}