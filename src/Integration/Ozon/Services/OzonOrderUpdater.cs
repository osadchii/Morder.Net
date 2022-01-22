using AutoMapper;
using Infrastructure.MediatR.Orders.Marketplace.Ozon.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;
using Integration.Ozon.Clients.Orders;
using Integration.Ozon.Clients.Orders.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services;

public class OzonOrderUpdater : MarketplaceOrderUpdater
{
    private readonly OzonDto _ozon;

    public OzonOrderUpdater(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper) : base(
        marketplace, serviceProvider, mapper)
    {
        _ozon = Mapper.Map<OzonDto>(marketplace);
    }

    public override async Task UpdateAsync()
    {
        if (!_ozon.Settings.LoadOrders)
        {
            return;
        }

        List<string> orderNumbers = await GetOrderNumbersToUpdate(Marketplace.Id);

        if (orderNumbers.Count == 0)
        {
            return;
        }

        var client = ServiceProvider.GetRequiredService<IOzonGetOrdersClient>();
        List<OzonPosting> postings = await client.GetOrders(_ozon, orderNumbers);
        await UpdateOrders(postings);
    }

    private async Task UpdateOrders(List<OzonPosting> postings)
    {
        var mediator = ServiceProvider.GetRequiredService<IMediator>();
        foreach (OzonPosting posting in postings)
        {
            await mediator.Send(new UpdateOzonOrderRequest()
            {
                MarketplaceId = _ozon.Id,
                OrderNumber = posting.PostingNumber,
                Status = StatusConverter.OzonStatusToOrderStatus(posting.Status),
                CustomerAddress = posting.Custromer?.Address?.AddressTail,
                CustomerFullName = posting.Custromer?.Name,
                PackingTimeLimit = posting.ShipmentDate,
                TrackNumber = posting.TrackingNumber,
                Items = posting.Products.Select(p => new UpdateOzonOrderItem()
                {
                    Articul = p.OfferId,
                    Count = p.Quantity,
                    Price = decimal.Parse(p.Price)
                })
            });
        }
    }
}