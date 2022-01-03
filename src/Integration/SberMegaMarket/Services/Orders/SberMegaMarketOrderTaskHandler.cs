using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.Common.Services.Orders;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Orders.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket.Services.Orders;

public class SberMegaMarketOrderTaskHandler : MarketplaceTaskHandler
{
    public SberMegaMarketOrderTaskHandler(Marketplace marketplace, MarketplaceOrderTask task,
        IServiceProvider serviceProvider) : base(marketplace, task, serviceProvider)
    {
    }

    public override Task Handle()
    {
        switch (OrderTask.Type)
        {
            case TaskType.Confirm:
                return HandleConfirmTask();
            case TaskType.Pack:
            case TaskType.Ship:
            case TaskType.Reject:
            case TaskType.Sticker:
            default:
                throw new NotImplementedException();
        }
    }

    private async Task HandleConfirmTask()
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketOrderConfirmClient>();

        var sber = mapper.Map<SberMegaMarketDto>(Marketplace);
        var request = new SberMegaMarketMessage<SberMegaMarketOrderConfirmData>(sber.Settings.Token)
        {
            Data =
            {
                Shipments = new List<SberMegaMarketConfirmOrderShipment>()
                {
                    new()
                    {
                        OrderCode = Order.Id.ToString(),
                        ShipmentId = Order.Number,
                        Items = Order.Items.Select(i => new SberMegaMarketConfirmOrderShipmentItem()
                        {
                            ItemIndex = i.ExternalId!,
                            OfferId = i.Product.Articul!
                        })
                    }
                }
            }
        };

        await client.SendRequest(sber, request);
    }
}