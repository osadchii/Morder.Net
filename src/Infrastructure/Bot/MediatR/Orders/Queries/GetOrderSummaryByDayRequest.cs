using MediatR;

namespace Infrastructure.Bot.MediatR.Orders.Queries;

public class GetOrderSummaryByDayRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public DateTime Date { get; set; }
}