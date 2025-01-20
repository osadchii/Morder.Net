using Infrastructure.Models.Orders;
using Integration.SberMegaMarket.Clients.Orders.Messages;

namespace Integration.SberMegaMarket.Clients.Orders;

public static class StatusConverter
{
    public static OrderStatus GetOrderStatusBySberMegaMarketOrder(UpdateOrderResponseDataShipment shipment,
        bool unknownStatusIsFinished)
    {
        OrderStatus status;

        var nonCanceledItems = shipment.Items
            .Where(i => !SberMegaMarketOrderStatuses.IsCanceled(i.Status)).ToArray();

        if (shipment.Items.All(i => SberMegaMarketOrderStatuses.IsCanceled(i.Status)))
        {
            status = OrderStatus.Canceled;
        }
        else if (nonCanceledItems
                 .All(i => i.Status is SberMegaMarketOrderStatuses.Delivered))
        {
            status = OrderStatus.Finished;
        }
        else if (nonCanceledItems.Any(i =>
                     i.Status is SberMegaMarketOrderStatuses.PendingConfirmation or SberMegaMarketOrderStatuses.New))
        {
            status = OrderStatus.Created;
        }
        else if (nonCanceledItems.Any(i =>
                     i.Status is SberMegaMarketOrderStatuses.Confirmed or SberMegaMarketOrderStatuses.PendingPacking))
        {
            status = OrderStatus.Reserved;
        }
        else if (nonCanceledItems.Any(i => i.Status is SberMegaMarketOrderStatuses.Packed))
        {
            status = OrderStatus.Packed;
        }
        else if (nonCanceledItems.Any(i => i.Status is SberMegaMarketOrderStatuses.Shipped))
        {
            status = OrderStatus.Shipped;
        }
        else if (unknownStatusIsFinished)
        {
            status = OrderStatus.Finished;
        }
        else
        {
            status = OrderStatus.Created;
        }

        return status;
    }
}