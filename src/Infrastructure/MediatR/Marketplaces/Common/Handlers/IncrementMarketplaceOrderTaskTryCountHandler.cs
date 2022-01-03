using Infrastructure.MediatR.Marketplaces.Common.Commands;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class
    IncrementMarketplaceOrderTaskTryCountHandler : IRequestHandler<IncrementMarketplaceOrderTaskTryCountRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<IncrementMarketplaceOrderTaskTryCountHandler> _logger;

    public IncrementMarketplaceOrderTaskTryCountHandler(MContext context,
        ILogger<IncrementMarketplaceOrderTaskTryCountHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(IncrementMarketplaceOrderTaskTryCountRequest request,
        CancellationToken cancellationToken)
    {
        MarketplaceOrderTask task = await _context.MarketplaceOrderTasks
            .SingleAsync(t => t.Id == request.MarketplaceOrderTaskId, cancellationToken);

        task.TryCount++;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogWarning($"Incremented {request.MarketplaceOrderTaskId} task try counter");

        return Unit.Value;
    }
}