using System.Text;
using Infrastructure.Bot.MediatR.Orders.Queries;
using Infrastructure.Models.Marketplaces;
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
        Order[] orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Status != OrderStatus.Canceled && o.Date >= request.Date &&
                        o.Date <= request.Date.AddDays(1).AddMilliseconds(-1))
            .Include(o => o.Marketplace).ToArrayAsync(cancellationToken);

        IEnumerable<MarketplaceType> types = orders.Select(o => o.Marketplace.Type).Distinct();

        var sb = new StringBuilder();

        foreach (MarketplaceType type in types)
        {
            AppendSummary(sb, orders.Where(o => o.Marketplace.Type == type).ToArray(), type.ToString());
        }

        AppendSummary(sb, orders.ToArray(), "Всего");

        await _client.SendTextAsync(request.ChatId, sb.ToString());

        return Unit.Value;
    }

    private void AppendSummary(StringBuilder sb, Order[] orders, string marketplaceName)
    {
        if (orders.Length == 0)
        {
            sb.AppendLine(MessageConstants.NoOrders);
            return;
        }

        decimal sum = orders.Sum(o => o.Sum);

        sb.AppendLine($"<b>{marketplaceName}</b>");
        sb.AppendLine($"Сумма заказов: {Math.Round(sum)}");
        sb.AppendLine($"Количество заказов: {orders.Length}");
        sb.AppendLine($"Средний чек: {Math.Round(sum / orders.Length)}");
        sb.AppendLine($"Максимальный чек: {Math.Round(orders.Max(o => o.Sum))}");
        sb.AppendLine();
    }
}