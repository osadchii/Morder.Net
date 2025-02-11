using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Bot.MediatR.Commands.Orders.Commands;

public class HandlerCallbackQueryRequest : IRequest<Unit>
{
    public int BotUserId { get; set; }
    public string Data { get; set; }
}

public class HandlerCallbackQueryRequestHandler : IRequestHandler<HandlerCallbackQueryRequest, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public HandlerCallbackQueryRequestHandler(ITelegramBotClient telegramBotClient, MContext context, IMediator mediator)
    {
        _telegramBotClient = telegramBotClient;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(HandlerCallbackQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.BotUsers
            .AsNoTracking()
            .Where(x => x.Id == request.BotUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception($"User with id {request.BotUserId} not found");
        }

        if (!user.ConfirmsOrders)
        {
            return Unit.Value;
        }
        
        var separatedData = request.Data.Split("|");
        var action = separatedData[0];
        var orderId = int.Parse(separatedData[1]);

        var order = await _context.Orders
            .Where(x => x.Id == orderId)
            .Include(x => x.Marketplace)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
        {
            throw new Exception($"Order with id {orderId} not found");
        }

        var orderConfirmed = action.Equals("confirm", StringComparison.InvariantCultureIgnoreCase);

        var readableAction = orderConfirmed
            ? "✅ подтвержден"
            : "\u274c отменен";
        
        var messagesByOrder = await _context.TelegramMessages
            .Where(x => x.OrderId == orderId)
            .Include(telegramMessage => telegramMessage.BotUser)
            .ToListAsync(cancellationToken);

        foreach (var message in messagesByOrder)
        {
            message.Text = message.Text.Replace("Подтвердите получение заказа.",
                $"Заказ {readableAction} пользователем {user}");

            await _telegramBotClient.EditMessageTextAsync(message.BotUser.ChatId, message.MessageId, message.Text, parseMode:ParseMode.Html, cancellationToken: cancellationToken);
        }

        if (order.Marketplace.Type == MarketplaceType.Kuper)
        {
            order.ConfirmedByBotUserId = user.Id;
            _context.Orders.Update(order);
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        if (orderConfirmed)
        {
            await _mediator.Send(new ConfirmOrderRequest
            {
                ExternalId = order.ExternalId,
                User = user.ToString()
            }, cancellationToken);
        }
        else
        {
            await _mediator.Send(new RejectOrderRequest
            {
                ExternalId = order.ExternalId,
                User = user.ToString(),
                Items = order.Items
                    .Where(x => !x.Canceled)
                    .Select(x => new RejectOrderItem
                    {
                        Count = x.Count,
                        ProductExternalId = x.Product.ExternalId
                    })
            }, cancellationToken);
        }
        
        return Unit.Value;
    }
}