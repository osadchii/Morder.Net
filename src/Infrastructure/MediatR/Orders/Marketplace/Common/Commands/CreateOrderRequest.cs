using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Commands;

public class CreateOrderRequest : IRequest<Order>
{
    public Guid ExternalId { get; set; }
    public bool Archived { get; set; }
    public int MarketplaceId { get; set; }
    public string Number { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public string Customer { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateTime ShippingDate { get; set; }
    public DateTime ConfirmedTimeLimit { get; set; }
    public DateTime PackingTimeLimit { get; set; }
    public DateTime ShippingTimeLimit { get; set; }
    public bool ExpressOrder { get; set; }
    public string? TrackNumber { get; set; }
    public IEnumerable<CreateOrderItem> Items { get; set; } = null!;
}

public class CreateOrderItem
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public decimal Count { get; set; }
    public decimal Sum { get; set; }
    public string? ExternalId { get; set; }
}