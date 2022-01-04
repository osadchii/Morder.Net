using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Queries;

public class GetOrderStickerRequest : IRequest<OrderSticker>
{
    public Guid ExternalId { get; set; }
}