using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.SberMegaMarket.Commands;

public class UpdateSberMegaMarketOrderRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public string ShipmentId { get; set; } = null!;

    public int OrderId { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime ConfirmedTimeLimit { get; set; }

    public DateTime PackingTimeLimit { get; set; }

    public DateTime ShippingTimeLimit { get; set; }

    public DateTime ShippingDate { get; set; }

    public string CustomerFullName { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public IEnumerable<UpdateSberMegaMarketOrderRequestItem> Items { get; set; } = null!;
}

public class UpdateSberMegaMarketOrderRequestItem
{
    public string ItemIndex { get; set; } = null!;

    public bool Canceled { get; set; }
}