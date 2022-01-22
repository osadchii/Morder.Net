using AutoMapper;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;
using Integration.Ozon.Clients.Orders;
using Integration.Ozon.Clients.Orders.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services;

public class OzonOrderLoader : MarketplaceOrderLoader
{
    private readonly OzonDto _ozon;

    public OzonOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper,
        DateTime startDate)
        : base(marketplace, serviceProvider, mapper, startDate)
    {
        _ozon = Mapper.Map<OzonDto>(Marketplace);
    }

    public override async Task LoadOrders()
    {
        if (!_ozon.Settings.LoadOrders)
        {
            return;
        }

        var client = ServiceProvider.GetRequiredService<IOzonLoadOrderListClient>();

        List<OzonPosting> postings = await client.GetOrders(_ozon, StartDate);

        if (postings.Count == 0)
        {
            return;
        }

        var adapter = ServiceProvider.GetRequiredService<IOzonOrderAdapter>();
        var mediatr = ServiceProvider.GetRequiredService<IMediator>();

        IEnumerable<string> doesNotExists = await mediatr.Send(new OrdersDoesNotExistsRequest()
        {
            MarketplaceId = _ozon.Id,
            Numbers = postings.Select(r => r.PostingNumber)
        });

        IEnumerable<CreateOrderRequest> requests = await adapter
            .CreateOrderRequests(_ozon, postings.IntersectBy(doesNotExists, x => x.PostingNumber).ToArray());

        await mediatr.Send(new CreateOrdersRequest()
        {
            CreateOrderRequests = requests
        });
    }
}