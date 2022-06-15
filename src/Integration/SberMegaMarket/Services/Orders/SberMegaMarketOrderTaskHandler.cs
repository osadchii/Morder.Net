using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Marketplaces.TaskContext;
using Infrastructure.Models.Orders;
using Integration.Common.Services.Orders;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Orders.Messages;
using MediatR;
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
        return OrderTask.Type switch
        {
            TaskType.Confirm => HandleConfirmTask(),
            TaskType.Pack => HandlePackingTask(),
            TaskType.Ship => HandleShippingTask(),
            TaskType.Reject => HandleRejectingTask(),
            TaskType.Sticker => HandlerStickerTask(),
            _ => Task.CompletedTask
        };
    }

    private async Task HandlerStickerTask()
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<StickerPrintData>>();

        var request = new SberMegaMarketMessage<StickerPrintData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = new List<StickerPrintShipment>()
                {
                    new()
                    {
                        ShipmentId = Order.Number,
                        Items = Order.Items.Where(i => i.Canceled)
                            .Select(i =>
                            {
                                Order.OrderBox box = Order.Boxes
                                    .First(b => b.ProductId == i.ProductId && b.Count > 0);

                                box.Count--;

                                var sberBox = new StickerPrintShipmentItem()
                                {
                                    ItemIndex = i.ExternalId!,
                                    Boxes = new List<StickerPrintShipmentItemBox>()
                                    {
                                        new()
                                        {
                                            BoxIndex = box.Number.ToString(),
                                            BoxCode =
                                                $"{_sberMegaMarketDto.Settings.MerchantId}*{Order.Id}*{box.Number}"
                                        }
                                    }
                                };

                                return sberBox;
                            }),
                        BoxCodes = Order.Boxes.Select(b =>
                            $"{_sberMegaMarketDto.Settings.MerchantId}*{Order.Id}*{b.Number}")
                    }
                }
            }
        };

        var content = await client.SendRequest(ApiUrls.StickerPrint, _sberMegaMarketDto, request);
        var response = content.FromJson<StickerPrintResponse>();

        if (response is null || response.Success != 1)
        {
            throw new Exception("Unexpected sticker print response." +
                                $"{Environment.NewLine}Url: {ApiUrls.StickerPrint}" +
                                $"{Environment.NewLine}Request: {request.ToJson()}" +
                                $"{Environment.NewLine}Response: {response}");
        }

        var mediator = ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new SaveOrderStickerFromStringRequest()
        {
            Content = response.Data,
            FileName = "SberMegaMarket Sticker.html",
            OrderId = Order.Id
        });
    }

    private async Task HandleRejectingTask()
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<OrderRejectingData>>();

        var taskContext = OrderTask.TaskContext.FromJson<RejectOrderContext>()!;

        var request = new SberMegaMarketMessage<OrderRejectingData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = new List<OrderRejectingShipment>()
                {
                    new()
                    {
                        ShipmentId = Order.Number,
                        Items = taskContext.Items
                            .Select(i => new OrderRejectingShipmentItem()
                            {
                                ItemIndex = i.ItemIndex,
                                OfferId = i.Articul
                            }),
                        Reason = new OrderRejectingShipmentReason()
                        {
                            Type = RejectingReason.OutOfStock
                        }
                    }
                }
            }
        };

        await client.SendRequest(ApiUrls.RejectOrder, _sberMegaMarketDto, request);
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
                            ShippingDate = DateTime.UtcNow.ToMoscowTime()
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
                Shipments = new ConfirmOrderShipment[]
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