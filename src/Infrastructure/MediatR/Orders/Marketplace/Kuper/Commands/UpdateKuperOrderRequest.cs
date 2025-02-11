using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.Kuper.Commands;

public class UpdateKuperOrderRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public OrderStatus Status { get; set; }

    public string CustomerFullName { get; set; }

    public string CustomerAddress { get; set; }

    public DateTime ShippingDate { get; set; }
    public IEnumerable<UpdateKuperOrderItem> Items { get; set; } = null!;
}

public class UpdateKuperOrderItem
{
    public string Articul { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Count { get; set; }
}