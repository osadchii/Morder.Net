using Infrastructure;
using Infrastructure.Bot.MediatR.Commands.Orders.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Integration.Common.Services.Orders;

public interface ISendNotificationService
{
    Task SendNotifications();
}

public class SendNotificationService : ISendNotificationService
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public SendNotificationService(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task SendNotifications()
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Include(x => x.Marketplace)
            .Where(x => x.Marketplace.Type == MarketplaceType.Kuper)
            .Where(x => x.Status == OrderStatus.Created)
            .ToListAsync();

        foreach (var order in orders)
        {
            await _mediator.Send(new SendOrderForConfirmation
            {
                OrderId = order.Id,
            });
        }
    }
}