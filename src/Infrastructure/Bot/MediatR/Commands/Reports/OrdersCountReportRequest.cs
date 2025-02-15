using System.Text;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Reports;

public class OrdersCountReportRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class OrdersCountReportHandler : IRequestHandler<OrdersCountReportRequest, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;

    public OrdersCountReportHandler(ITelegramBotClient client, MContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<Unit> Handle(OrdersCountReportRequest request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Status != OrderStatus.Canceled &&
                        o.Date >= request.From.ToUtcWithMoscowOffset() && o.Date <= request.To.ToUtcWithMoscowOffset())
            .Include(o => o.Marketplace)
            .ToArrayAsync(cancellationToken);

        var types = orders.Select(o => o.Marketplace.Type).Distinct();

        var sb = new StringBuilder();

        foreach (var type in types)
        {
            AppendReport(sb, orders
                    .Where(o => o.Marketplace.Type == type)
                    .Where(o => !o.ExpressOrder).ToArray(),
                type.ToString(), false);
            AppendReport(sb, orders
                    .Where(o => o.Marketplace.Type == type)
                    .Where(o => o.ExpressOrder).ToArray(),
                type.ToString(), true);
        }

        AppendReport(sb, orders, "Всего", false, true);

        await _client.SendTextAsync(request.ChatId, sb.ToString());

        return Unit.Value;
    }

    private static void AppendReport(StringBuilder sb, Order[] orders, string marketplaceName, bool express, bool total = false)
    {
        if (orders.Length == 0)
        {
            if (total)
            {
                sb.AppendLine(MessageConstants.NoOrders);
            }
            return;
        }

        var sumsPerDay = orders
            .GroupBy(o => new DateTime(o.Date.Year, o.Date.Month, o.Date.Day))
            .ToArray();

        var avgPerDay = sumsPerDay.Average(s => s.Count());
        decimal bestDayCount = sumsPerDay.Max(s => s.Count());
        var bestDay = sumsPerDay.First(s => s.Count() == bestDayCount).Key;

        var expressSuffix = express ? " (экспресс)" : "";
        sb.AppendLine($"<b>{marketplaceName}{expressSuffix}</b>");
        sb.AppendLine($"Количество заказов: {orders.Length}");
        sb.AppendLine($"В среднем в день: {Math.Round(avgPerDay)}");
        sb.AppendLine($"Рекордный день: {bestDay.ToString("dd.MM.yyyy")}");
        sb.AppendLine($"Рекордное количество: {bestDayCount.ToFormatString()}");
        sb.AppendLine();
    }
}