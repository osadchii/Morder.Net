using System.Collections.Concurrent;
using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.Common.Services.Orders;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Orders;
using Integration.SberMegaMarket.Clients.Orders.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Int32;

namespace Integration.SberMegaMarket.Services.Orders;

public class SberMegaMarketOrderUpdater : MarketplaceOrderUpdater
{
    private readonly SberMegaMarketDto _sberMegaMarketDto;
    private const int PortionSize = 100;

    public SberMegaMarketOrderUpdater(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper) : base(
        marketplace, serviceProvider, mapper)
    {
        _sberMegaMarketDto = Mapper.Map<SberMegaMarketDto>(Marketplace);
    }

    public override async Task UpdateAsync()
    {
        List<string> orderNumbers = await GetOrderNumbersToUpdate(Marketplace.Id);

        if (orderNumbers.Count == 0)
        {
            return;
        }

        var shipments = new ConcurrentBag<UpdateOrderResponseDataShipment>();
        var portions = new List<List<string>>();

        var handled = 0;

        while (handled < orderNumbers.Count)
        {
            List<string> portion = orderNumbers.Skip(handled).Take(PortionSize).ToList();
            portions.Add(portion);

            handled += PortionSize;
        }

        await Parallel.ForEachAsync(portions, new ParallelOptions()
        {
            MaxDegreeOfParallelism = 10
        }, async (list, _) => { await LoadOrdersPortion(list, shipments); });

        await UpdateOrders(shipments);
    }

    private async Task UpdateOrders(ConcurrentBag<UpdateOrderResponseDataShipment> shipments)
    {
        var mediator = ServiceProvider.GetRequiredService<IMediator>();
        foreach (UpdateOrderResponseDataShipment shipment in shipments)
        {
            TryParse(shipment.OrderCode, out int orderId);

            await mediator.Send(new UpdateSberMegaMarketOrderRequest()
            {
                MarketplaceId = _sberMegaMarketDto.Id,
                OrderId = orderId,
                Status = StatusConverter.GetOrderStatusBySberMegaMarketOrder(shipment, false),
                CustomerAddress = shipment.CustomerAddress,
                CustomerFullName = shipment.CustomerFullName,
                ShipmentId = shipment.ShipmentId,
                ConfirmedTimeLimit = shipment.ConfirmedTimeLimit.HasValue
                    ? shipment.ConfirmedTimeLimit.Value.ToCommonTime().ToUtcTime()
                    : new DateTime().ToUtcTime(),
                PackingTimeLimit = shipment.PackingTimeLimit.HasValue
                    ? shipment.PackingTimeLimit.Value.ToCommonTime().ToUtcTime()
                    : new DateTime().ToUtcTime(),
                ShippingTimeLimit = shipment.ShippingTimeLimit.HasValue
                    ? shipment.ShippingTimeLimit.Value.ToCommonTime().ToUtcTime()
                    : new DateTime().ToUtcTime(),
                ShippingDate = shipment.ShippingDate.HasValue
                    ? shipment.ShippingDate.Value.ToCommonTime().ToUtcTime()
                    : new DateTime().ToUtcTime(),
                Items = shipment.Items.Select(i => new UpdateSberMegaMarketOrderRequestItem()
                {
                    ItemIndex = i.ItemIndex,
                    Canceled = SberMegaMarketOrderStatuses.IsCanceled(i.Status)
                })
            });
        }
    }

    private async Task LoadOrdersPortion(IEnumerable<string> portion,
        ConcurrentBag<UpdateOrderResponseDataShipment> result)
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<UpdateOrdersData>>();
        var request = new SberMegaMarketMessage<UpdateOrdersData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = portion
            }
        };

        string content;
        try
        {
            content = await client.SendRequest(ApiUrls.GetOrders, _sberMegaMarketDto, request);
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider.GetRequiredService<ILogger<SberMegaMarketOrderUpdater>>();
            logger.LogWarning("Error while getting orders from SberMegaMarket: {Message}", ex.Message);
            return;
        }

        var response = content.FromJson<UpdateOrderResponse>();

        if (response is null || response.Success != 1)
        {
            throw new Exception("Unexpected order update response." +
                                $"{Environment.NewLine}Url: {ApiUrls.GetOrders}" +
                                $"{Environment.NewLine}Request: {request.ToJson()}" +
                                $"{Environment.NewLine}Response: {response}");
        }

        foreach (UpdateOrderResponseDataShipment shipment in response.Data.Shipments)
        {
            result.Add(shipment);
        }
    }
}