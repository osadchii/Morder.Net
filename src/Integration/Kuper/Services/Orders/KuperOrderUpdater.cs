using AutoMapper;
using Infrastructure.MediatR.Orders.Marketplace.Kuper.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Common.Services.Orders;
using Integration.Kuper.Clients.Orders;
using Integration.Kuper.Clients.Orders.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Kuper.Services.Orders;

public class KuperOrderUpdater : MarketplaceOrderUpdater
{
    private readonly KuperDto _kuper;
    private readonly ILogger<KuperOrderUpdater> _logger;
    
    public KuperOrderUpdater(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper) : base(marketplace, serviceProvider, mapper)
    {
        _logger = ServiceProvider.GetRequiredService<ILogger<KuperOrderUpdater>>();
        _kuper = Mapper.Map<KuperDto>(marketplace);
    }

    public override async Task UpdateAsync()
    {
        if (_kuper.Settings.LoadOrders != true)
        {
            return;
        }

        var orderNumbers = await GetOrderNumbersToUpdate(Marketplace.Id);

        if (orderNumbers.Count == 0)
        {
            return;
        }

        var client = ServiceProvider.GetRequiredService<IKuperOrdersClient>();

        var kuperOrders = new List<OrderData>();

        using var httpClient = new HttpClient();

        var token = await client.GetToken(_kuper, httpClient);
        
        foreach (var orderNumber in orderNumbers)
        {
            var order = await client.GetOrder(_kuper, orderNumber, token);
            kuperOrders.Add(order);
        }
        
        _logger.LogInformation("Loaded {Count} orders from Kuper", kuperOrders.Count);
        
        var mediator = ServiceProvider.GetRequiredService<IMediator>();

        foreach (var order in kuperOrders)
        {
            var request = new UpdateKuperOrderRequest
            {
                MarketplaceId = _kuper.Id,
                OrderNumber = order.OriginalOrderId,
                Status = StatusConverter.KuperStatusToOrderStatus(order.State),
                CustomerAddress = order.Address?.FullAddress,
                CustomerFullName = order.Customer?.Name,
                ShippingDate = order.Delivery.ExpectedTo,
                Items = order.Positions
                    .Select(x => new UpdateKuperOrderItem
                    {
                        Articul = x.Id,
                        Count = x.Quantity,
                        Price = decimal.TryParse(x.Price, out var price) ? price : 0,
                    })
            };
            
            await mediator.Send(request);
        }
    }
}