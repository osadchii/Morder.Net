using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;
using Integration.Ozon.Clients.Orders;
using Integration.Ozon.Clients.Orders.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services;

public class OzonOrderLoader : MarketplaceOrderLoader
{
    private readonly OzonDto _ozon;

    public OzonOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper) : base(
        marketplace, serviceProvider, mapper)
    {
        _ozon = Mapper.Map<OzonDto>(marketplace);
    }

    public override async Task LoadAsync()
    {
        if (!_ozon.Settings.LoadOrders)
        {
            return;
        }

        var client = ServiceProvider.GetRequiredService<IOzonLoadUnfulfilledOrdersClient>();
        IEnumerable<Posting> orders = await client.GetOrders(_ozon);
    }
}