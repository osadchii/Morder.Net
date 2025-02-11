using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.BotUsers;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

    public HandlerCallbackQueryRequestHandler(ITelegramBotClient telegramBotClient, MContext context,
        IMediator mediator)
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

        switch (action)
        {
            case "confirm_approved":
                await ConfirmOrder(order, user, cancellationToken);
                break;
            case "cancel_approved":
                await CancelOrder(order, user, cancellationToken);
                break;
            case "confirm":
                await RequestConfirmationForConfirmation(order, user, cancellationToken);
                break;
            case "cancel":
                await RequestConfirmationForCancelling(order, user, cancellationToken);
                break;
            case "return":
                await ReturnToActionChoice(order, user, cancellationToken);
                break;
        }

        return Unit.Value;
    }

    private async Task ReturnToActionChoice(Order order, BotUser user, CancellationToken cancellationToken)
    {
        var messages = await _context.TelegramMessages
            .Where(x => x.OrderId == order.Id)
            .Where(x => x.BotUserId == user.Id)
            .Include(telegramMessage => telegramMessage.BotUser)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            message.Text = message.Text
                .Replace(Constants.ApproveConfirmOrderText, Constants.ConfirmOrderText)
                .Replace(Constants.ApproveCancelOrderText, Constants.ConfirmOrderText);

            await _telegramBotClient.EditMessageTextAsync(message.BotUser.ChatId, message.MessageId, message.Text,
                parseMode: ParseMode.Html, replyMarkup: Constants.OrderActionChoice(order.Id),
                cancellationToken: cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task RequestConfirmationForConfirmation(Order order, BotUser user,
        CancellationToken cancellationToken)
    {
        var messages = await _context.TelegramMessages
            .Where(x => x.OrderId == order.Id)
            .Where(x => x.BotUserId == user.Id)
            .Include(telegramMessage => telegramMessage.BotUser)
            .ToListAsync(cancellationToken);

        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("Да", $"confirm_approved|{order.Id}"),
            InlineKeyboardButton.WithCallbackData("Нет", $"return|{order.Id}")
        };

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        foreach (var message in messages)
        {
            message.Text = message.Text.Replace(Constants.ConfirmOrderText, Constants.ApproveConfirmOrderText);

            await _telegramBotClient.EditMessageTextAsync(message.BotUser.ChatId, message.MessageId, message.Text,
                parseMode: ParseMode.Html, replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task RequestConfirmationForCancelling(Order order, BotUser user, CancellationToken cancellationToken)
    {
        var messages = await _context.TelegramMessages
            .Where(x => x.OrderId == order.Id)
            .Where(x => x.BotUserId == user.Id)
            .Include(telegramMessage => telegramMessage.BotUser)
            .ToListAsync(cancellationToken);

        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("Да", $"cancel_approved|{order.Id}"),
            InlineKeyboardButton.WithCallbackData("Нет", $"return|{order.Id}")
        };

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        foreach (var message in messages)
        {
            message.Text = message.Text.Replace(Constants.ConfirmOrderText, Constants.ApproveCancelOrderText);

            await _telegramBotClient.EditMessageTextAsync(message.BotUser.ChatId, message.MessageId, message.Text,
                parseMode: ParseMode.Html, replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task ConfirmOrder(Order order, BotUser user, CancellationToken cancellationToken)
    {
        var messagesByOrder = await _context.TelegramMessages
            .Where(x => x.OrderId == order.Id)
            .Include(telegramMessage => telegramMessage.BotUser)
            .ToListAsync(cancellationToken);

        foreach (var message in messagesByOrder)
        {
            message.Text = message.Text.Replace(Constants.ApproveConfirmOrderText,
                $"Заказ ✅ подтвержден пользователем {user}");

            await _telegramBotClient.EditMessageTextAsync(message.BotUser.ChatId, message.MessageId, message.Text,
                parseMode: ParseMode.Html, cancellationToken: cancellationToken);
        }

        if (order.Marketplace.Type == MarketplaceType.Kuper)
        {
            order.ConfirmedByBotUserId = user.Id;
            _context.Orders.Update(order);
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new ConfirmOrderRequest
        {
            ExternalId = order.ExternalId,
            User = user.ToString()
        }, cancellationToken);
    }

    private async Task CancelOrder(Order order, BotUser user, CancellationToken cancellationToken)
    {
        var messagesByOrder = await _context.TelegramMessages
            .Where(x => x.OrderId == order.Id)
            .Include(telegramMessage => telegramMessage.BotUser)
            .ToListAsync(cancellationToken);

        foreach (var message in messagesByOrder)
        {
            message.Text = message.Text.Replace(Constants.ApproveCancelOrderText,
                $"Заказ \u274c отменен пользователем {user}");

            await _telegramBotClient.EditMessageTextAsync(message.BotUser.ChatId, message.MessageId, message.Text,
                parseMode: ParseMode.Html, cancellationToken: cancellationToken);
        }

        if (order.Marketplace.Type == MarketplaceType.Kuper)
        {
            order.ConfirmedByBotUserId = user.Id;
            _context.Orders.Update(order);
        }

        await _context.SaveChangesAsync(cancellationToken);
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
}