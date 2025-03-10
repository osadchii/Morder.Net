using System.Text;
using Infrastructure.Bot.MediatR.Orders.Queries;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Orders.Handlers;

public class GetOrderSummaryByDayHandler : IRequestHandler<GetOrderSummaryByDayRequest, Unit>
{
    private readonly MContext _context;
    private readonly ITelegramBotClient _client;

    public GetOrderSummaryByDayHandler(MContext context, ITelegramBotClient client)
    {
        _context = context;
        _client = client;
    }

    public async Task<Unit> Handle(GetOrderSummaryByDayRequest request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Status != OrderStatus.Canceled && o.Date >= request.Date.ToUtcWithMoscowOffset() &&
                        o.Date <= request.Date.AddDays(1).AddMilliseconds(-1).ToUtcWithMoscowOffset())
            .Include(o => o.Marketplace).ToArrayAsync(cancellationToken);

        var types = orders.Select(o => o.Marketplace.Type).Distinct();

        var sb = new StringBuilder();

        foreach (var type in types)
        {
            AppendSummary(sb, orders.Where(o => o.Marketplace.Type == type)
                .Where(x => !x.ExpressOrder).ToArray(), type.ToString(), false);
            AppendSummary(sb, orders.Where(o => o.Marketplace.Type == type)
                .Where(x => x.ExpressOrder).ToArray(), type.ToString(), true);
        }

        AppendSummary(sb, orders.ToArray(), "Всего", false, true);

        await _client.SendTextAsync(request.ChatId, sb.ToString());

        return Unit.Value;
    }

    private static void AppendSummary(StringBuilder sb, Order[] orders, string marketplaceName, bool express, bool total = false)
    {
        if (orders.Length == 0)
        {
            if (total)
            {
                sb.AppendLine(MessageConstants.NoOrders);
            }
            return;
        }

        var sum = orders.Sum(o => o.Sum);
        
        var expressSuffix = express ? " (экспресс)" : string.Empty;

        sb.AppendLine($"<b>{marketplaceName}{expressSuffix}</b>");
        sb.AppendLine($"Сумма заказов: {sum.ToFormatString()}");
        sb.AppendLine($"Количество заказов: {orders.Length}");
        sb.AppendLine($"Средний чек: {(sum / orders.Length).ToFormatString()}");
        sb.AppendLine($"Максимальный чек: {orders.Max(o => o.Sum).ToFormatString()}");
        sb.AppendLine();
    }
}