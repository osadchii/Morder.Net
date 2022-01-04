using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class SaveOrderStatusHistoryHandler : IRequestHandler<SaveOrderStatusHistoryRequest, Unit>
{
    private readonly MContext _context;

    public SaveOrderStatusHistoryHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SaveOrderStatusHistoryRequest request, CancellationToken cancellationToken)
    {
        await _context.OrderStatusHistories.AddAsync(new OrderStatusHistory()
        {
            Date = DateTime.UtcNow,
            OrderId = request.OrderId,
            Status = request.Status
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}