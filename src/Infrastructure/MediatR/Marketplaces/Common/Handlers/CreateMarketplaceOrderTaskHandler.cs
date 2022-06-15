using Infrastructure.MediatR.Marketplaces.Common.Commands;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class CreateMarketplaceOrderTaskHandler : IRequestHandler<CreateMarketplaceOrderTaskRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<CreateMarketplaceOrderTaskHandler> _logger;

    public CreateMarketplaceOrderTaskHandler(MContext context, ILogger<CreateMarketplaceOrderTaskHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateMarketplaceOrderTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new MarketplaceOrderTask()
        {
            Date = DateTime.UtcNow,
            MarketplaceId = request.MarketplaceId,
            OrderId = request.OrderId,
            Type = request.Type,
            TryCount = 0,
            TaskContext = request.TaskContext
        };

        await _context.MarketplaceOrderTasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created {request.Type} task for {request.MarketplaceId} with {request.OrderId} order");

        return Unit.Value;
    }
}