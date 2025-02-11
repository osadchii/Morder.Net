using Infrastructure;
using Infrastructure.MediatR.Marketplaces.Common.Commands;
using Infrastructure.Models.Marketplaces;
using Integration.Kuper.Services.Orders;
using Integration.Ozon.Services.Orders;
using Integration.SberMegaMarket.Services.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.Orders;

public interface ITaskHandleService
{
    Task HandleTasks(int maxTryCount);
}

public class TaskHandleService : ITaskHandleService
{
    private readonly ILogger<TaskHandleService> _logger;
    private readonly MContext _context;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;

    public TaskHandleService(ILogger<TaskHandleService> logger, MContext context, IMediator mediator,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _context = context;
        _mediator = mediator;
        _serviceProvider = serviceProvider;
    }

    public async Task HandleTasks(int maxTryCount)
    {
        try
        {
            var marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive)
                .ToListAsync();

            foreach (var marketplace in marketplaces)
            {
                var tasks = await _context.MarketplaceOrderTasks
                    .AsNoTracking()
                    .OrderBy(t => t.Date)
                    .Where(t => t.MarketplaceId == marketplace.Id && t.TryCount < maxTryCount)
                    .Include(t => t.Order)
                    .ThenInclude(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Include(t => t.Order)
                    .ThenInclude(o => o.Boxes)
                    .ThenInclude(b => b.Product)
                    .ToListAsync();

                foreach (var task in tasks)
                {
                    MarketplaceTaskHandler handler = marketplace.Type switch
                    {
                        MarketplaceType.SberMegaMarket => new SberMegaMarketOrderTaskHandler(marketplace, task,
                            _serviceProvider),
                        MarketplaceType.Ozon => new OzonOrderTaskHandler(marketplace, task, _serviceProvider),
                        MarketplaceType.Kuper => new KuperOrderTaskHandler(marketplace, task, _serviceProvider),
                        _ => null
                    };

                    if (handler is null)
                    {
                        continue;
                    }

                    IBaseRequest taskResultHandler;

                    try
                    {
                        await handler.Handle();
                        taskResultHandler = new DeleteMarketplaceOrderTaskRequest
                        {
                            MarketplaceOrderTaskId = task.Id
                        };
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                        taskResultHandler = new IncrementMarketplaceOrderTaskTryCountRequest
                        {
                            MarketplaceOrderTaskId = task.Id
                        };
                    }

                    await _mediator.Send(taskResultHandler);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while executing tasks");
        }
    }
}