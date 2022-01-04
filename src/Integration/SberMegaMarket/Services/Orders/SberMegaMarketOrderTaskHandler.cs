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
                return HandleShippingTask();
            case TaskType.Reject:
            case TaskType.Sticker:
            default:
                throw new NotImplementedException();
        }
    }

    private async Task HandlePackingTask()
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<OrderPackingData>>();

        var request = new SberMegaMarketMessage<OrderPackingData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = new List<OrderPackingShipment>()
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

        await client.SendRequest(ApiUrls.PackingOrder, _sberMegaMarketDto, request);
    }

    private async Task HandleShippingTask()
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<OrderShippingData>>();

        var request = new SberMegaMarketMessage<OrderShippingData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = new List<OrderShippingShipment>()
                {
                    new()
                    {
                        ShipmentId = Order.Number,
                        Boxes = GetShippingBoxes(),
                        Shipping = new OrderShippingShipmentShipping()
                        {
                            ShippingDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow,
                                    TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"))
                                .ToString("yyyy-MM-ddTHH:mm:ss")
                        }
                    }
                }
            }
        };

        await client.SendRequest(ApiUrls.ShippingOrder, _sberMegaMarketDto, request);
    }

    private async Task HandleConfirmTask()
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<OrderConfirmData>>();

        var request = new SberMegaMarketMessage<OrderConfirmData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = new List<ConfirmOrderShipment>()
                {
                    new()
                    {
                        OrderCode = Order.Id.ToString(),
                        ShipmentId = Order.Number,
                        Items = Order.Items.Select(i => new ConfirmOrderShipmentItem()
                        {
                            ItemIndex = i.ExternalId!,
                            OfferId = i.Product.Articul!
                        })
                    }
                }
            }
        };

        await client.SendRequest(ApiUrls.ConfirmOrder, _sberMegaMarketDto, request);
    }

    private IEnumerable<OrderShippingShipmentBox> GetShippingBoxes()
    {
        return Order.Items.Where(i => !i.Canceled)
            .Select(i =>
            {
                Order.OrderBox box = Order.Boxes
                    .First(b => b.ProductId == i.ProductId && b.Count > 0);

                box.Count--;

                var sberBox = new OrderShippingShipmentBox()
                {
                    BoxIndex = box.Number,
                    BoxCode = $"{_sberMegaMarketDto.Settings.MerchantId}*{Order.Id}*{box.Number}"
                };

                return sberBox;
            });
    }

    private IEnumerable<OrderPackingShipmentItem> GetPackingBoxes()
    {
        return Order.Items.Where(i => !i.Canceled)
            .Select(i =>
            {
                Order.OrderBox box = Order.Boxes
                    .First(b => b.ProductId == i.ProductId && b.Count > 0);

                box.Count--;

                var sberBox = new OrderPackingShipmentItem()
                {
                    ItemIndex = i.ExternalId!,
                    Boxes = new List<OrderPackingShipmentItemBox>()
                    {
                        new()
                        {
                            BoxIndex = box.Number,
                            BoxCode = $"{_sberMegaMarketDto.Settings.MerchantId}*{Order.Id}*{box.Number}"
                        }
                    }
                };

                return sberBox;
            });
    }
}