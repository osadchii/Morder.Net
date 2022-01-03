using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Orders;
using Integration.Common.Services.Orders;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Orders.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket.Services.Orders;

public class SberMegaMarketOrderTaskHandler : MarketplaceTaskHandler
{
    private readonly SberMegaMarketDto _sberMegaMarketDto;

    public SberMegaMarketOrderTaskHandler(Marketplace marketplace, MarketplaceOrderTask task,
        IServiceProvider serviceProvider) : base(marketplace, task, serviceProvider)
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();

        _sberMegaMarketDto = mapper.Map<SberMegaMarketDto>(Marketplace);
    }

    public override Task Handle()
    {
        switch (OrderTask.Type)
        {
            case TaskType.Confirm:
                return HandleConfirmTask();
            case TaskType.Pack:
                return HandlePackingTask();
            case TaskType.Ship:
            case TaskType.Reject:
            case TaskType.Sticker:
            default:
                throw new NotImplementedException();
        }
    }

    private async Task HandlePackingTask()
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketOrderPackingClient>();

        var request = new SberMegaMarketMessage<SberMegaMarketOrderPackingData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = new List<SberMegaMarketOrderPackingShipment>()
                {
                    new()
                    {
                        OrderCode = Order.Id.ToString(),
                        ShipmentId = Order.Number,
                        Items = GetPackingBoxes()
                    }
                }
            }
        };

        await client.SendRequest(_sberMegaMarketDto, request);
    }

    private async Task HandleConfirmTask()
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketOrderConfirmClient>();

        var request = new SberMegaMarketMessage<SberMegaMarketOrderConfirmData>(_sberMegaMarketDto.Settings.Token)
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

        await client.SendRequest(_sberMegaMarketDto, request);
    }

    private IEnumerable<SberMegaMarketOrderPackingShipmentItem> GetPackingBoxes()
    {
        var boxNumber = 1;
        return Order.Items.Where(i => !i.Canceled)
            .Select(i =>
            {
                Order.OrderBox box = Order.Boxes
                    .First(b => b.ProductId == i.ProductId && b.Count > 0);

                box.Count--;

                var sberBox = new SberMegaMarketOrderPackingShipmentItem()
                {
                    ItemIndex = i.ExternalId!,
                    Boxes = new List<SberMegaMarketOrderPackingShipmentItemBox>()
                    {
                        new()
                        {
                            BoxIndex = boxNumber,
                            BoxCode = $"{_sberMegaMarketDto.Settings.MerchantId}*{Order.Id}*{boxNumber}"
                        }
                    }
                };

                if (box.Count == 0)
                {
                    boxNumber++;
                }

                return sberBox;
            });
    }
}