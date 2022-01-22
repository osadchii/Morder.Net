using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;

namespace Integration.Ozon.Services;

public class OzonOrderUpdater : MarketplaceOrderUpdater
{
    private readonly OzonDto _ozon;

    public OzonOrderUpdater(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper) : base(
        marketplace, serviceProvider, mapper)
    {
        _ozon = Mapper.Map<OzonDto>(marketplace);
    }

    public override Task UpdateAsync()
    {
        if (!_ozon.Settings.LoadOrders)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}