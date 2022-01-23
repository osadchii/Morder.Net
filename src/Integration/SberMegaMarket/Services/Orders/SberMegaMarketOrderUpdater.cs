using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.Common.Services.Orders;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Orders.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

        var handled = 0;

        while (handled < orderNumbers.Count)
        {
            List<string> portion = orderNumbers.Skip(handled).Take(PortionSize).ToList();
            await LoadOrdersPortion(portion);

            handled += PortionSize;
        }
    }

    private async Task LoadOrdersPortion(IEnumerable<string> portion)
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<LoadOrdersData>>();
        var request = new SberMegaMarketMessage<LoadOrdersData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = portion
            }
        };

        string content = await client.SendRequest(ApiUrls.GetOrders, _sberMegaMarketDto, request);
        var response = content.FromJson<LoadOrderResponse>();

        if (response is null || response.Success != 1)
        {
            throw new Exception("Unexpected order update response." +
                                $"{Environment.NewLine}Url: {ApiUrls.GetOrders}" +
                                $"{Environment.NewLine}Request: {request.ToJson()}" +
                                $"{Environment.NewLine}Response: {response}");
        }

        var mediator = ServiceProvider.GetRequiredService<IMediator>();

        foreach (LoadOrderResponseDataShipment shipment in response.Data.Shipments)
        {
            if (!TryParse(shipment.OrderCode, out int orderId))
            {
                throw new Exception($"Can't parse order code {shipment.OrderCode}");
            }

            await mediator.Send(new UpdateSberMegaMarketOrderRequest()
            {
                OrderId = orderId,
                CustomerAddress = shipment.CustomerAddress,
                CustomerFullName = shipment.CustomerFullName,
                ShipmentId = shipment.ShipmentId,
                ConfirmedTimeLimit = shipment.ConfirmedTimeLimit.ToCommonTime().ToUtcTime(),
                PackingTimeLimit = shipment.PackingTimeLimit.ToCommonTime().ToUtcTime(),
                ShippingTimeLimit = shipment.ShippingTimeLimit.ToCommonTime().ToUtcTime(),
                Items = shipment.Items.Select(i => new UpdateSberMegaMarketOrderRequestItem()
                {
                    ItemIndex = i.ItemIndex,
                    Canceled = i.Status is "CUSTOMER_CANCELED" or "MERCHANT_CANCELED",
                    Finished = i.Status is "DELIVERED"
                })
            });
        }
    }
}