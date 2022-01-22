using AutoMapper;
using Infrastructure.Models.Marketplaces;

namespace Integration.Common.Services.Orders;

public abstract class MarketplaceOrderUpdater
{
    protected readonly Marketplace Marketplace;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IMapper Mapper;

    protected MarketplaceOrderUpdater(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper)
    {
        Marketplace = marketplace;
        ServiceProvider = serviceProvider;
        Mapper = mapper;
    }

    public abstract Task UpdateAsync();
}