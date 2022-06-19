using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.Ozon.Commands;

public class UpdateOzonOrderRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public OrderStatus Status { get; set; }

    public DateTime ShippingDate { get; set; }

    public string? CustomerFullName { get; set; }

    public string? CustomerAddress { get; set; }
    public string? TrackNumber { get; set; }

    public IEnumerable<UpdateOzonOrderItem> Items { get; set; } = null!;
}

public class UpdateOzonOrderItem
{
    public string Articul { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Count { get; set; }
}