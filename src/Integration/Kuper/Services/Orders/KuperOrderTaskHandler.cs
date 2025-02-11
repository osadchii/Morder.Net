using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Common.Services.Orders;
using Integration.Kuper.Clients.Orders;
using Integration.Kuper.Clients.Orders.Messages;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.Models.Marketplaces.TaskType;

namespace Integration.Kuper.Services.Orders;

public class KuperOrderTaskHandler : MarketplaceTaskHandler
{
    private readonly KuperDto _kuper;

    public KuperOrderTaskHandler(Marketplace marketplace, MarketplaceOrderTask task, IServiceProvider serviceProvider) :
        base(marketplace, task, serviceProvider)
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();

        _kuper = mapper.Map<KuperDto>(Marketplace);
    }

    public override Task Handle()
    {
        return OrderTask.Type switch
        {
            Confirm => HandleConfirmAndInWorkOrderTask(),
            Pack => HandlePackAndReadyForDeliveryOrderTask(),
            Reject => HandleRejectOrderTask(),
            _ => Task.CompletedTask
        };
    }

    private async Task HandleConfirmAndInWorkOrderTask()
    {
        await HandleConfirmOrderTask();
        await HandleInWorkOrderTask();
    }

    private async Task HandleConfirmOrderTask()
    {
        var client = ServiceProvider.GetRequiredService<IKuperOrdersClient>();

        var request = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.accepted",
                Payload = new Payload
                {
                    Number = Order.Id.ToString(),
                    OrderId = Order.Number,
                }
            }
        };

        await client.SendOrderNotification(_kuper, request);
    }

    private async Task HandlePackAndReadyForDeliveryOrderTask()
    {
        await HandlePackOrderTask();
        await HandleReadyForDeliveryOrderTask();
    }

    private async Task HandlePackOrderTask()
    {
        var client = ServiceProvider.GetRequiredService<IKuperOrdersClient>();

        var request = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.assembled",
                Payload = new Payload
                {
                    Number = Order.Id.ToString(),
                    OrderId = Order.Number,
                    Order = new Order
                    {
                        OriginalOrderId = Order.Number,
                        Positions = Order.Items
                            .Select(x => new OrderPosition
                            {
                                Id = x.Product.Articul,
                                Quantity = Convert.ToInt32(x.Count)
                            })
                            .ToArray(),
                        Packs = new OrderPack
                        {
                            Count = Order.Boxes.Count,
                        }
                    }
                }
            }
        };

        await client.SendOrderNotification(_kuper, request);
    }

    private async Task HandleReadyForDeliveryOrderTask()
    {
        var client = ServiceProvider.GetRequiredService<IKuperOrdersClient>();

        var request = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.ready_for_delivery",
                Payload = new Payload
                {
                    Number = Order.Id.ToString(),
                    OrderId = Order.Number,
                    Order = new Order
                    {
                        OriginalOrderId = Order.Number,
                        Positions = Order.Items
                            .Select(x => new OrderPosition
                            {
                                Id = x.Product.Articul,
                                Quantity = Convert.ToInt32(x.Count)
                            })
                            .ToArray(),
                        Packs = new OrderPack
                        {
                            Count = Order.Boxes.Count,
                        }
                    }
                }
            }
        };

        await client.SendOrderNotification(_kuper, request);
    }

    private async Task HandleInWorkOrderTask()
    {
        var client = ServiceProvider.GetRequiredService<IKuperOrdersClient>();

        var request = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.in_work",
                Payload = new Payload
                {
                    Number = Order.Id.ToString(),
                    OrderId = Order.Number,
                }
            }
        };

        await client.SendOrderNotification(_kuper, request);
    }

    private async Task HandleRejectOrderTask()
    {
        var client = ServiceProvider.GetRequiredService<IKuperOrdersClient>();

        var request = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.canceled",
                Payload = new Payload
                {
                    Number = Order.Id.ToString(),
                    OrderId = Order.Number,
                }
            }
        };

        await client.SendOrderNotification(_kuper, request);
    }
}