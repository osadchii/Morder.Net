using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Unit = MediatR.Unit;

namespace Infrastructure.Bot.MediatR.Commands.Orders.Commands;

public class SendOrderForConfirmation : IRequest<Unit>
{
    public int OrderId { get; set; }
}

public class SendOrderForConfirmationHandler : IRequestHandler<SendOrderForConfirmation, Unit>
{
    private readonly MContext _context;
    private readonly ITelegramBotClient _client;

    public SendOrderForConfirmationHandler(MContext context, ITelegramBotClient client)
    {
        _context = context;
        _client = client;
    }

    public async Task<Unit> Handle(SendOrderForConfirmation request, CancellationToken cancellationToken)
    {
        var users = await _context.BotUsers
            .AsNoTracking()
            .Where(x => x.ConfirmsOrders)
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!users.Any())
        {
            return Unit.Value;
        }

        var order = await _context.Orders
            .AsNoTracking()
            .Include(x => x.Marketplace)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .AsSplitQuery()
            .SingleOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new Exception($"Order with id {request.OrderId} not found");
        }

        var textBuilder = new StringBuilder();
        textBuilder.Append($"Поступил новый заказ от маркетплейса <b>{order.Marketplace.Name}</b>");
        textBuilder.AppendLine();
        textBuilder.AppendLine($"<b>Номер заказа маркетплейса:</b> {order.Number}");
        textBuilder.AppendLine();
        textBuilder.AppendLine("<b>Состав:</b>");

        foreach (var orderItem in order.Items)
        {
            textBuilder.AppendLine(
                $"— {orderItem.Product.Name}, Артикул: <b>{orderItem.Product.Articul}</b>, Количество: {orderItem.Count}");
        }

        textBuilder.AppendLine();
        textBuilder.AppendLine("Подтвердите получение заказа.");

        var text = textBuilder.ToString();

        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("✅ Подтвердить", $"confirm|{order.Id}"),
            InlineKeyboardButton.WithCallbackData("\u274c Отменить", $"cancel|{order.Id}")
        };

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        foreach (var user in users)
        {
            await _client.SendTextMessageAsync(user.ChatId, text, ParseMode.Html, replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
        }
        
        return Unit.Value;
    }
}