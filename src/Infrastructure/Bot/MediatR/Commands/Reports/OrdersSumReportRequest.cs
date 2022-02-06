using System.Text;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Reports;

public class OrdersSumReportRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class OrdersSumReportHandler : IRequestHandler<OrdersSumReportRequest, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;

    public OrdersSumReportHandler(ITelegramBotClient client, MContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<Unit> Handle(OrdersSumReportRequest request, CancellationToken cancellationToken)
    {
        Order[] orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Status != OrderStatus.Canceled &&
                        o.Date >= request.From.ToUtcWithMoscowOffset() && o.Date <= request.To.ToUtcWithMoscowOffset())
            .Include(o => o.Marketplace)
            .ToArrayAsync(cancellationToken);

        IEnumerable<MarketplaceType> types = orders.Select(o => o.Marketplace.Type).Distinct();

        bool extrapolation = DateTime.Now >= request.From && DateTime.Now <= request.To;

        var sb = new StringBuilder();

        foreach (MarketplaceType type in types)
        {
            AppendReport(sb, orders
                    .Where(o => o.Marketplace.Type == type).ToArray(),
                type.ToString(), false,
                request.From, request.To);
        }

        AppendReport(sb, orders, "Всего", extrapolation,
            request.From, request.To);

        await _client.SendTextAsync(request.ChatId, sb.ToString());

        return Unit.Value;
    }

    private static void AppendReport(StringBuilder sb, Order[] orders, string marketplaceName, bool extrapolation,
        DateTime from, DateTime to)
    {
        if (orders.Length == 0)
        {
            sb.AppendLine(MessageConstants.NoOrders);
            return;
        }

        IGrouping<DateTime, Order>[] sumsPerDay = orders
            .GroupBy(o => new DateTime(o.Date.Year, o.Date.Month, o.Date.Day))
            .ToArray();

        decimal avgPerDay = sumsPerDay.Average(s => s.Sum(o => o.Sum));
        decimal bestDaySum = sumsPerDay.Max(s => s.Sum(o => o.Sum));
        DateTime bestDay = sumsPerDay.First(s => s.Sum(o => o.Sum) == bestDaySum).Key;

        decimal sum = orders.Sum(o => o.Sum);

        sb.AppendLine($"<b>{marketplaceName}</b>");
        sb.AppendLine($"Сумма заказов: {sum.ToFormatString()}");
        sb.AppendLine($"Средний чек: {(sum / orders.Length).ToFormatString()}");
        sb.AppendLine($"Максимальный чек: {orders.Max(o => o.Sum).ToFormatString()}");
        sb.AppendLine($"В среднем в день: {avgPerDay.ToFormatString()}");
        sb.AppendLine($"Рекордный день: {bestDay.ToString("dd.MM.yyyy")}");
        sb.AppendLine($"Рекордная сумма: {bestDaySum.ToFormatString()}");
        sb.AppendLine();

        if (extrapolation)
        {
            TimeSpan span = DateTime.Now - from;
            decimal perSecond = sum / Convert.ToDecimal(span.TotalSeconds);
            var totalSeconds = Convert.ToDecimal((to - from).TotalSeconds);

            decimal total = totalSeconds * perSecond;

            sb.AppendLine($"Экстраполированная сумма на конец месяца: {total.ToFormatString()}");
            sb.AppendLine();
        }
    }
}