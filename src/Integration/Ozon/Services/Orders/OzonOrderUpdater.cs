using AutoMapper;
using Infrastructure.MediatR.Orders.Marketplace.Ozon.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;
using Integration.Ozon.Clients.Orders;
using Integration.Ozon.Clients.Orders.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Ozon.Services.Orders;

public class OzonOrderUpdater : MarketplaceOrderUpdater
{
    private readonly OzonDto _ozon;
    private readonly ILogger<OzonOrderUpdater> _logger;

    public OzonOrderUpdater(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper) : base(
        marketplace, serviceProvider, mapper)
    {
        _logger = ServiceProvider.GetRequiredService<ILogger<OzonOrderUpdater>>();
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

        _logger.LogInformation("Loaded {Count} ozon orders from marketplace with {MarketplaceId} Id",
            postings.Count, _ozon.Id);

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
                ShippingDate = posting.ShipmentDate,
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