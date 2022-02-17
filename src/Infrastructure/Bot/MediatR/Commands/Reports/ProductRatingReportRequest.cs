using System.Text;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Reports;

public class ProductRatingReportRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int? MarketplaceId { get; set; }
}

public class ProductRatingReportHandler : IRequestHandler<ProductRatingReportRequest, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private const int ResultCount = 200;

    public ProductRatingReportHandler(ITelegramBotClient client, MContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<Unit> Handle(ProductRatingReportRequest request, CancellationToken cancellationToken)
    {
        Order[] orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Status != OrderStatus.Canceled
                        && o.Date >= request.From.ToUtcWithMoscowOffset() &&
                        o.Date <= request.To.ToUtcWithMoscowOffset()
                        && (!request.MarketplaceId.HasValue || o.MarketplaceId == request.MarketplaceId!.Value))
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToArrayAsync(cancellationToken);

        var result = new Dictionary<string, (decimal Count, decimal Sum)>();

        foreach (Order order in orders)
        {
            foreach (Order.OrderItem item in order.Items.Where(i => !i.Canceled))
            {
                string key = item.Product.Name!;
                if (result.ContainsKey(key))
                {
                    decimal count = result[key].Count + item.Count;
                    decimal sum = result[key].Sum + item.Sum;

                    result[key] = (count, sum);
                }
                else
                {
                    result.Add(key, (item.Count, item.Sum));
                }
            }
        }

        var rowCount = 0;
        var sb = new StringBuilder();
        sb.AppendLine($"Топ-{ResultCount}");

        foreach (KeyValuePair<string, (decimal Count, decimal Sum)> row in result.OrderByDescending(r => r.Value.Sum)
                     .Take(ResultCount))
        {
            sb.AppendLine($"{++rowCount}: <b>{row.Key}</b>");
            sb.AppendLine($"Количество: {row.Value.Count.ToFormatString()}");
            sb.AppendLine($"Сумма: {row.Value.Sum.ToFormatString()}");
        }

        sb.AppendLine();
        sb.AppendLine($"Всего продано позиций: {result.Count}");

        await _client.SendTextAsync(request.ChatId, sb.ToString());

        return Unit.Value;
    }
}