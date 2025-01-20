using Infrastructure.MediatR.Marketplaces.Common.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class DeleteMarketplaceOrderTaskHandler : IRequestHandler<DeleteMarketplaceOrderTaskRequest, Unit>
{
    private readonly MContext _context;

    public DeleteMarketplaceOrderTaskHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMarketplaceOrderTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _context.MarketplaceOrderTasks
            .SingleAsync(t => t.Id == request.MarketplaceOrderTaskId, cancellationToken);

        _context.MarketplaceOrderTasks.Remove(task);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}